using BE;
using DAL;
using SERVICE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_USUARIO
    {
        private const int INTENTOS_MAXIMOS = 3;
        private int intentosLoginActuales = 0;

        /// <summary>
        /// Realiza el login de un usuario.
        /// Retorna el usuario si es exitoso, null si hay error.
        /// Lanza excepciones con mensajes informativos para cada caso.
        /// </summary>
        public BE_USUARIO Login(string username, string password)
        {
            MP_USUARIO usuarioDAL = new MP_USUARIO();
            BLL_BITACORA bitacoraBLL = new BLL_BITACORA();

            BE_USUARIO usuario = usuarioDAL.BuscarPorUserName(username);

            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            // Verificar si el usuario está bloqueado
            if (usuario.Bloqueado)
                throw new Exception(SV_Mensajes.GetUsuarioBloqueadoMessage());

            // PASS alterado / insertado a mano sin hash Base64 válido = integridad comprometida
            if (!BLL_GESTOR.EsHashValido(usuario.HashedPassword))
            {
                throw new Exception(
                    "Error de integridad: el hash de contraseña del usuario está alterado o tiene un formato inválido. " +
                    "No se puede autenticar hasta restaurar o recalcular los dígitos verificadores.");
            }

            // Verificar contraseña
            if (!BLL_GESTOR.VerifyPassword(password, usuario.HashedPassword))
            {
                // Registrar intento fallido y verificar bloqueo
                intentosLoginActuales++;
                int intentosRestantes = INTENTOS_MAXIMOS - intentosLoginActuales;

                if (intentosLoginActuales >= INTENTOS_MAXIMOS)
                {
                    BloquearUsuario(usuario);
                    throw new Exception(SV_Mensajes.GetUsuarioBloqueadoPorIntentosMessage());
                }

                throw new Exception(SV_Mensajes.GetIntentoFallidoMessage(intentosLoginActuales, INTENTOS_MAXIMOS));
            }

            // Login exitoso: restablecer intentos
            intentosLoginActuales = 0;

            // Verificar integridad de datos (DVH/DVV) - solo avisa, no bloquea
            System.Diagnostics.Debug.WriteLine($"[LOGIN] Usuario: {usuario.Username}, DVHorizontal: {usuario.DVHorizontal}");
            BE_DigitoVerificadorVertical dvv = ObtenerDVVDelUsuario(usuario.ID);
            System.Diagnostics.Debug.WriteLine($"[LOGIN] DVV obtenido: {(dvv == null ? "NULL" : "EXISTE")}");
            var (integridadOK, mensajeIntegridad) = BLL_IntegridadDatos.VerificarIntegridadCompleta(usuario, dvv);

            if (!integridadOK)
            {
                try
                {
                    bitacoraBLL.IngresarRegistro(usuario, $"ADVERTENCIA DE INTEGRIDAD: {mensajeIntegridad}");
                }
                catch (Exception exBit)
                {
                    System.Diagnostics.Debug.WriteLine($"[INTEGRIDAD] No se pudo auditar en bitácora: {exBit.Message}");
                }
                System.Diagnostics.Debug.WriteLine($"[INTEGRIDAD] {mensajeIntegridad}");
            }

            AsignarRol(usuario);

            SV_Singleton.GetInstance().Usuario = usuario;
            bitacoraBLL.IngresarRegistro(usuario, SV_Mensajes.GetLoginMessage());

            return usuario;
        }

        private void AsignarRol(BE_USUARIO usuario)
        {
            if (usuario == null) return;

            try
            {
                MP_USUARIO mpUsuario = new MP_USUARIO();
                string rol = mpUsuario.ObtenerRolUsuario(usuario.ID);
                usuario.Rol = string.IsNullOrWhiteSpace(rol) ? "USUARIO" : rol;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo rol: {ex.Message}");
                usuario.Rol = "USUARIO";
            }
        }

        public void Logout()
        {
            var usuario = SV_Singleton.GetInstance().Usuario;

            if (usuario != null)
            {
                BLL_BITACORA bitacoraBLL = new BLL_BITACORA();
                bitacoraBLL.IngresarRegistro(usuario, SV_Mensajes.GetLogoutMessage());
            }

            SV_Singleton.GetInstance().LimpiarSesion();
        }

        public void CrearUsuario(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El nombre de usuario es obligatorio.");

            username = username.Trim();

            if (username.Length < 3)
                throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres.");

            if (username.Length > 50)
                throw new ArgumentException("El nombre de usuario no puede superar los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña es obligatoria.");

            if (password.Length <= 4)
                throw new ArgumentException("La contraseña debe tener más de 4 caracteres.");

            try
            {
                MP_USUARIO usuarioDAL = new MP_USUARIO();
                BLL_BITACORA bitacoraBLL = new BLL_BITACORA();

                if (usuarioDAL.BuscarPorUserName(username) != null)
                    throw new ArgumentException("El nombre de usuario ya existe.");

                BE_USUARIO usuario = new BE_USUARIO
                {
                    Username = username,
                    HashedPassword = BLL_GESTOR.HashPassword(password),
                    Bloqueado = false
                };

                int filas = usuarioDAL.Alta(usuario);

                // Con SET NOCOUNT ON, ExecuteNonQuery puede devolver -1 aunque el INSERT haya sido exitoso.
                usuario = usuarioDAL.BuscarPorUserName(username);
                if (usuario == null)
                {
                    if (filas == -1)
                        throw new InvalidOperationException("Error del sistema al crear el usuario. No se pudo guardar en la base de datos.");
                    throw new InvalidOperationException("Error del sistema al crear el usuario. La operación no persistió el registro.");
                }

                CalcularYGuardarDVHUsuario(usuario, usuarioDAL);
                CalcularYGuardarDVVUsuario(usuario);

                var usuarioActual = SV_Singleton.GetInstance().Usuario;
                BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "CREACION", "", username, usuarioActual, "Usuario creado");

                bitacoraBLL.IngresarRegistro(usuario, SV_Mensajes.GetNewUserMessage());
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error del sistema al crear el usuario. Detalle: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Bloquea un usuario tras exceder intentos de login
        /// </summary>
        private void BloquearUsuario(BE_USUARIO usuario)
        {
            if (usuario == null)
                return;

            // Crear snapshot antes de bloquear (con contenido real)
            BLL_Rollback.GuardarSnapshotUsuario(usuario,
                $"Snapshot antes de bloquear al usuario {usuario.Username} por intentos fallidos");

            usuario.Bloqueado = true;
            MP_USUARIO mpUsuario = new MP_USUARIO();
            mpUsuario.BloquearUsuario(usuario);

            // Recalcular DVH y DVV después de cambiar estado
            CalcularYGuardarDVHUsuario(usuario, mpUsuario);
            CalcularYGuardarDVVUsuario(usuario);

            // Registrar bloqueo en auditoría
            BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "BLOQUEADO", "false", "true", usuario, "Bloqueado por intentos fallidos");

            BLL_BITACORA bitacoraBLL = new BLL_BITACORA();
            bitacoraBLL.IngresarRegistro(usuario, SV_Mensajes.GetUsuarioBloqueadoPorIntentosMessage());
        }

        /// <summary>
        /// Desbloquea un usuario (operación administrativa)
        /// </summary>
        public void DesbloquearUsuario(BE_USUARIO usuario)
        {
            if (usuario == null)
                return;

            // Crear snapshot antes de desbloquear (con contenido real)
            BLL_Rollback.GuardarSnapshotUsuario(usuario,
                $"Snapshot antes de desbloquear al usuario {usuario.Username}");

            usuario.Bloqueado = false;
            MP_USUARIO mpUsuario = new MP_USUARIO();
            mpUsuario.DesbloquearUsuario(usuario);

            // Recalcular DVH y DVV después de cambiar estado
            CalcularYGuardarDVHUsuario(usuario, mpUsuario);
            CalcularYGuardarDVVUsuario(usuario);

            // Registrar desbloqueo en auditoría
            var usuarioActual = SV_Singleton.GetInstance().Usuario;
            BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "BLOQUEADO", "true", "false", usuarioActual, "Desbloqueado por administrador");

            BLL_BITACORA bitacoraBLL = new BLL_BITACORA();
            bitacoraBLL.IngresarRegistro(usuario, "Usuario desbloqueado por administrador");
        }

        /// <summary>
        /// Actualiza el estado de un usuario (bloqueo/desbloqueo)
        /// </summary>
        public void ActualizarUsuario(BE_USUARIO usuario)
        {
            if (usuario == null)
                return;

            try
            {
                var usuarioActual = SV_Singleton.GetInstance().Usuario;
                MP_USUARIO mpUsuario = new MP_USUARIO();
                BLL_BITACORA bitacoraBLL = new BLL_BITACORA();

                if (usuario.Bloqueado)
                {
                    string rolObjetivo = mpUsuario.ObtenerRolUsuario(usuario.ID);
                    if (EsRolAdministrador(rolObjetivo))
                        throw new InvalidOperationException("No se puede bloquear a un usuario administrador.");
                }

                // Guardar snapshot antes del cambio
                string estadoNuevo = usuario.Bloqueado ? "bloqueado" : "desbloqueado";
                BLL_Rollback.GuardarSnapshotUsuario(usuario, $"Cambio de estado antes de ser {estadoNuevo}");

                if (usuario.Bloqueado)
                {
                    mpUsuario.BloquearUsuario(usuario);
                    BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "BLOQUEADO", "false", "true", usuarioActual, "Usuario bloqueado por administrador");
                    bitacoraBLL.IngresarRegistro(usuarioActual, $"Usuario {usuario.Username} bloqueado por administrador");
                }
                else
                {
                    mpUsuario.DesbloquearUsuario(usuario);
                    BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "BLOQUEADO", "true", "false", usuarioActual, "Usuario desbloqueado por administrador");
                    bitacoraBLL.IngresarRegistro(usuarioActual, $"Usuario {usuario.Username} desbloqueado por administrador");
                }

                // Recalcular DVH y DVV después del cambio
                CalcularYGuardarDVHUsuario(usuario, mpUsuario);
                CalcularYGuardarDVVUsuario(usuario);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error actualizando usuario: {ex.Message}");
            }
        }

        public static bool EsRolAdministrador(string rol)
        {
            if (string.IsNullOrWhiteSpace(rol))
                return false;

            string normalizado = rol.Trim().ToUpperInvariant();
            return normalizado == "ADMIN" || normalizado == "ADMINISTRADOR";
        }

        /// <summary>
        /// Obtiene el máximo de intentos de login permitidos
        /// </summary>
        public int ObtenerIntentosMaximos()
        {
            return INTENTOS_MAXIMOS;
        }

        /// <summary>
        /// Calcula y guarda el DVH para un usuario
        /// </summary>
        private void CalcularYGuardarDVHUsuario(BE_USUARIO usuario, MP_USUARIO mpUsuario)
        {
            BLL_IntegridadDatos.CalcularDVHUsuario(usuario);
            mpUsuario.ActualizarDVHorizontal(usuario);
        }

        /// <summary>
        /// Calcula y guarda el DVV para la tabla USUARIO (uno por tabla)
        /// </summary>
        private void CalcularYGuardarDVVUsuario(BE_USUARIO usuario)
        {
            BE_DigitoVerificadorVertical dvv = BLL_IntegridadDatos.CalcularDVVUsuario(usuario);
            dvv.TipoEntidad = "USUARIO";
            dvv.IdEntidad = 0;

            MP_DigitoVerificadorVertical mpDVV = new MP_DigitoVerificadorVertical();

            // Obtener DVV existente por tipo de entidad
            var dvvExistente = mpDVV.BuscarPorTipoEntidad("USUARIO");

            if (dvvExistente != null)
            {
                // Actualizar si existe
                dvv.ID = dvvExistente.ID;
                mpDVV.Editar(dvv);
            }
            else
            {
                // Crear si no existe
                mpDVV.Alta(dvv);
            }
        }

        public List<BE_USUARIO> ListarUsuarios()
        {
            MP_USUARIO mpUsuario = new MP_USUARIO();
            var lista = mpUsuario.Listar();
            foreach (var u in lista)
                AsignarRol(u);
            return lista;
        }

        public void EditarUsuario(int idUsuario, string nuevoUsername, string nuevaPassword = null)
        {
            MP_USUARIO mpUsuario = new MP_USUARIO();
            BLL_BITACORA bitacoraBLL = new BLL_BITACORA();
            var usuarioActual = SV_Singleton.GetInstance().Usuario;

            BE_USUARIO usuario = mpUsuario.BuscarPorId(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            BLL_Rollback.GuardarSnapshotUsuario(usuario, $"Antes de editar usuario {usuario.Username}");

            string usernameAnterior = usuario.Username;
            if (!string.IsNullOrWhiteSpace(nuevoUsername) && nuevoUsername != usuario.Username)
            {
                if (mpUsuario.BuscarPorUserName(nuevoUsername) != null)
                    throw new Exception("El nombre de usuario ya existe.");

                usuario.Username = nuevoUsername;
                BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "USERNAME", usernameAnterior, nuevoUsername, usuarioActual, "Username editado");
            }

            if (!string.IsNullOrWhiteSpace(nuevaPassword))
            {
                string passAnterior = usuario.HashedPassword;
                usuario.HashedPassword = BLL_GESTOR.HashPassword(nuevaPassword);
                BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "PASS", "[hash]", "[hash-nuevo]", usuarioActual, "Contraseña restablecida");
            }

            CalcularYGuardarDVHUsuario(usuario, mpUsuario);
            int filas = mpUsuario.Editar(usuario);
            if (filas < 1)
                throw new Exception("Error al editar el usuario.");

            CalcularYGuardarDVVUsuario(usuario);
            bitacoraBLL.IngresarRegistro(usuarioActual ?? usuario, $"Usuario editado: {usernameAnterior} -> {usuario.Username}");
        }

        public void EliminarUsuario(int idUsuario)
        {
            MP_USUARIO mpUsuario = new MP_USUARIO();
            BLL_BITACORA bitacoraBLL = new BLL_BITACORA();
            var usuarioActual = SV_Singleton.GetInstance().Usuario;

            BE_USUARIO usuario = mpUsuario.BuscarPorId(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            if (usuarioActual != null && usuarioActual.ID == idUsuario)
                throw new Exception("No se puede eliminar el usuario de la sesión actual.");

            string rolObjetivo = mpUsuario.ObtenerRolUsuario(idUsuario);
            if (EsRolAdministrador(rolObjetivo))
                throw new InvalidOperationException("No se puede eliminar a un usuario administrador.");

            BLL_Rollback.GuardarSnapshotUsuario(usuario, $"Antes de eliminar usuario {usuario.Username}");

            int filas = mpUsuario.Borrar(usuario);
            if (filas < 1)
                throw new Exception("Error al eliminar el usuario (puede tener dependencias).");

            BLL_ControlCambios.RegistrarCambio("USUARIO", idUsuario, "ELIMINACION", usuario.Username, "", usuarioActual, "Usuario eliminado");
            bitacoraBLL.IngresarRegistro(usuarioActual ?? usuario, $"Usuario eliminado: {usuario.Username}");

            // Recalcular DVV de la tabla tras eliminación
            var restantes = mpUsuario.Listar();
            if (restantes.Count > 0)
                CalcularYGuardarDVVUsuario(restantes[0]);
        }

        public void AsignarRolUsuario(int idUsuario, int idRol, string nombreRol)
        {
            MP_USUARIO mpUsuario = new MP_USUARIO();
            var usuario = mpUsuario.BuscarPorId(idUsuario);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            string rolAnterior = mpUsuario.ObtenerRolUsuario(idUsuario);
            mpUsuario.AsignarRol(idUsuario, idRol);
            usuario.Rol = nombreRol;

            var usuarioActual = SV_Singleton.GetInstance().Usuario;
            BLL_ControlCambios.RegistrarCambio("USUARIO", idUsuario, "ROL", rolAnterior, nombreRol, usuarioActual, "Rol asignado");
            new BLL_BITACORA().IngresarRegistro(usuarioActual, $"Rol '{nombreRol}' asignado a {usuario.Username}");
        }

        public BE_DigitoVerificadorVertical ObtenerDVVDelUsuario(int idUsuario)
        {
            try
            {
                MP_DigitoVerificadorVertical mpDVV = new MP_DigitoVerificadorVertical();
                return mpDVV.BuscarPorTipoEntidad("USUARIO");
            }
            catch
            {
                return null;
            }
        }
    }
}
