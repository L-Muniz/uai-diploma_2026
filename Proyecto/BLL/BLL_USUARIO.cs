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

        
        public BE_USUARIO Login(string username, string password)
        {
            MP_USUARIO usuarioDAL = new MP_USUARIO();
            BLL_BITACORA bitacoraBLL = new BLL_BITACORA();

            BE_USUARIO usuario = usuarioDAL.BuscarPorUserName(username);

            if (usuario == null)
                throw new Exception("Usuario no encontrado.");

            if (usuario.Bloqueado)
                throw new Exception(SV_Mensajes.GetUsuarioBloqueadoMessage());

            if (!BLL_GESTOR.EsHashValido(usuario.HashedPassword))
            {
                throw new Exception(
                    "Error de integridad: el hash de contraseña del usuario está alterado o tiene un formato inválido. " +
                    "No se puede autenticar hasta restaurar o recalcular los dígitos verificadores.");
            }

            if (!BLL_GESTOR.VerifyPassword(password, usuario.HashedPassword))
            {
                
                intentosLoginActuales++;
                int intentosRestantes = INTENTOS_MAXIMOS - intentosLoginActuales;

                if (intentosLoginActuales >= INTENTOS_MAXIMOS)
                {
                    BloquearUsuario(usuario);
                    throw new Exception(SV_Mensajes.GetUsuarioBloqueadoPorIntentosMessage());
                }

                throw new Exception(SV_Mensajes.GetIntentoFallidoMessage(intentosLoginActuales, INTENTOS_MAXIMOS));
            }

            intentosLoginActuales = 0;

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

        // Reglas de negocio únicas para usuario: las usan crear, editar, restaurar memento y revertir cambio.
        public static string ValidarUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("El nombre de usuario es obligatorio.");

            string limpio = username.Trim();

            if (limpio.Length < 3)
                throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres.");

            if (limpio.Length > 50)
                throw new ArgumentException("El nombre de usuario no puede superar los 50 caracteres.");

            return limpio;
        }

        public static void ValidarPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("La contraseña es obligatoria.");

            if (password.Length <= 4)
                throw new ArgumentException("La contraseña debe tener más de 4 caracteres.");
        }

        public void CrearUsuario(string username, string password)
        {
            username = ValidarUsername(username);
            ValidarPassword(password);

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

        
        private void BloquearUsuario(BE_USUARIO usuario)
        {
            if (usuario == null)
                return;

            usuario.Bloqueado = true;
            MP_USUARIO mpUsuario = new MP_USUARIO();
            mpUsuario.BloquearUsuario(usuario);

            CalcularYGuardarDVHUsuario(usuario, mpUsuario);
            CalcularYGuardarDVVUsuario(usuario);

            BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "BLOQUEADO", "false", "true", usuario, "Bloqueado por intentos fallidos");

            BLL_BITACORA bitacoraBLL = new BLL_BITACORA();
            bitacoraBLL.IngresarRegistro(usuario, SV_Mensajes.GetUsuarioBloqueadoPorIntentosMessage());
        }

        
        public void DesbloquearUsuario(BE_USUARIO usuario)
        {
            if (usuario == null)
                return;

            BLL_UsuarioMemento.Capturar(usuario.ID, $"Antes de desbloquear al usuario {usuario.Username}");

            usuario.Bloqueado = false;
            MP_USUARIO mpUsuario = new MP_USUARIO();
            mpUsuario.DesbloquearUsuario(usuario);

            CalcularYGuardarDVHUsuario(usuario, mpUsuario);
            CalcularYGuardarDVVUsuario(usuario);

            var usuarioActual = SV_Singleton.GetInstance().Usuario;
            BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "BLOQUEADO", "true", "false", usuarioActual, "Desbloqueado por administrador");

            BLL_BITACORA bitacoraBLL = new BLL_BITACORA();
            bitacoraBLL.IngresarRegistro(usuario, "Usuario desbloqueado por administrador");
        }

        
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

                string estadoNuevo = usuario.Bloqueado ? "bloqueado" : "desbloqueado";
                BLL_UsuarioMemento.Capturar(usuario.ID, $"Cambio de estado antes de ser {estadoNuevo}");

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

        
        public int ObtenerIntentosMaximos()
        {
            return INTENTOS_MAXIMOS;
        }

        
        private void CalcularYGuardarDVHUsuario(BE_USUARIO usuario, MP_USUARIO mpUsuario)
        {
            BLL_IntegridadDatos.CalcularDVHUsuario(usuario);
            mpUsuario.ActualizarDVHorizontal(usuario);
        }

        
        private void CalcularYGuardarDVVUsuario(BE_USUARIO usuario)
        {
            BE_DigitoVerificadorVertical dvv = BLL_IntegridadDatos.CalcularDVVUsuario(usuario);
            BLL_IntegridadDatos.PersistirDVVUsuario(dvv);
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

            // Todas las validaciones se hacen antes de capturar el memento y de tocar el usuario.
            string username = string.IsNullOrWhiteSpace(nuevoUsername) ? usuario.Username : ValidarUsername(nuevoUsername);
            bool cambiaUsername = username != usuario.Username;
            bool cambiaPassword = !string.IsNullOrWhiteSpace(nuevaPassword);

            if (cambiaUsername && mpUsuario.BuscarPorUserName(username) != null)
                throw new Exception("El nombre de usuario ya existe.");

            if (cambiaPassword)
                ValidarPassword(nuevaPassword);

            BLL_UsuarioMemento.Capturar(idUsuario, $"Antes de editar usuario {usuario.Username}");

            string usernameAnterior = usuario.Username;
            if (cambiaUsername)
                usuario.Username = username;
            if (cambiaPassword)
                usuario.HashedPassword = BLL_GESTOR.HashPassword(nuevaPassword);

            CalcularYGuardarDVHUsuario(usuario, mpUsuario);
            int filas = mpUsuario.Editar(usuario);
            if (filas == 0)
                throw new Exception("Error al editar el usuario.");

            CalcularYGuardarDVVUsuario(usuario);

            // Se registran los cambios recién cuando la persistencia fue exitosa.
            if (cambiaUsername)
                BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "USERNAME", usernameAnterior, usuario.Username, usuarioActual, "Username editado");
            if (cambiaPassword)
                BLL_ControlCambios.RegistrarCambio("USUARIO", usuario.ID, "PASS", "[hash]", "[hash-nuevo]", usuarioActual, "Contraseña restablecida");

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

            BLL_UsuarioMemento.Capturar(idUsuario, $"Antes de eliminar usuario {usuario.Username}");

            int filas = mpUsuario.Borrar(usuario);
            if (filas == 0)
                throw new Exception("Error al eliminar el usuario (puede tener dependencias).");

            BLL_ControlCambios.RegistrarCambio("USUARIO", idUsuario, "ELIMINACION", usuario.Username, "", usuarioActual, "Usuario eliminado");
            bitacoraBLL.IngresarRegistro(usuarioActual ?? usuario, $"Usuario eliminado: {usuario.Username}");

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
