using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BLL
{
    public class BLL_ControlCambios
    {
        private static MP_CambiosEntidad mpCambios = new MP_CambiosEntidad();

        public static void RegistrarCambio(string tipoEntidad, int idEntidad, string nombreAtributo,
            string valorAnterior, string valorNuevo, BE_USUARIO usuarioModifico, string descripcion = "")
        {
            if (usuarioModifico == null)
                return;

            if (valorAnterior == valorNuevo)
                return;

            BE_CambiosEntidad cambio = new BE_CambiosEntidad(
                tipoEntidad, idEntidad, nombreAtributo,
                valorAnterior, valorNuevo, usuarioModifico, descripcion
            );

            try
            {
                mpCambios.Alta(cambio);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al registrar cambio: {ex.Message}");
            }
        }

        public static List<BE_CambiosEntidad> ObtenerHistorialEntidad(string tipoEntidad, int idEntidad)
        {
            return mpCambios.ListarPorEntidad(tipoEntidad, idEntidad)
                .OrderByDescending(c => c.FechaCambio)
                .ToList();
        }

        public static List<BE_CambiosEntidad> ObtenerCambiosPorUsuario(int idUsuario)
        {
            return mpCambios.ListarPorUsuario(idUsuario)
                .OrderByDescending(c => c.FechaCambio)
                .ToList();
        }

        public static List<BE_CambiosEntidad> ObtenerCambiosPorFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            return mpCambios.ListarPorFechas(fechaDesde, fechaHasta)
                .OrderByDescending(c => c.FechaCambio)
                .ToList();
        }

        public static List<BE_CambiosEntidad> ObtenerCambiosRecientes(int cantidad = 50)
        {
            return mpCambios.Listar()
                .OrderByDescending(c => c.FechaCambio)
                .Take(cantidad)
                .ToList();
        }

        public static string GenerarReporteCambios(List<BE_CambiosEntidad> cambios)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("===================================");
            sb.AppendLine("REPORTE DE CAMBIOS / AUDITORIA");
            sb.AppendLine($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($"Total de cambios: {cambios.Count}");
            sb.AppendLine("===================================");
            sb.AppendLine();

            foreach (var cambio in cambios.Take(100))
            {
                sb.AppendLine($"[{cambio.FechaCambio:dd/MM/yyyy HH:mm:ss}] {cambio.TipoEntidad}(ID:{cambio.IdEntidad})");
                sb.AppendLine($"  Usuario: {cambio.UsuarioModifico?.Username ?? "Sistema"}");
                sb.AppendLine($"  Atributo: {cambio.NombreAtributo}");
                sb.AppendLine($"  Anterior: '{cambio.ValorAnterior}'");
                sb.AppendLine($"  Nuevo: '{cambio.ValorNuevo}'");
                if (!string.IsNullOrEmpty(cambio.Descripcion))
                    sb.AppendLine($"  Descripcion: {cambio.Descripcion}");
                sb.AppendLine();
            }

            if (cambios.Count > 100)
                sb.AppendLine($"... y {cambios.Count - 100} cambios mas");

            return sb.ToString();
        }

        public static Dictionary<string, int> ObtenerEstadisticasPorEntidad()
        {
            var cambios = mpCambios.Listar();
            return cambios.GroupBy(c => c.TipoEntidad)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public static Dictionary<string, int> ObtenerEstadisticasPorUsuario()
        {
            var cambios = mpCambios.Listar();
            return cambios.Where(c => c.UsuarioModifico != null)
                .GroupBy(c => c.UsuarioModifico.Username)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public static (bool exito, string mensaje) RevertirCambio(BE_CambiosEntidad cambio, BE_USUARIO usuarioReverti)
        {
            try
            {
                if (cambio == null)
                    return (false, "Cambio no encontrado");

                if (cambio.TipoEntidad == "USUARIO")
                {
                    return RevertirCambioUsuario(cambio, usuarioReverti);
                }
                else if (cambio.TipoEntidad == "ROL")
                {
                    return RevertirCambioRol(cambio, usuarioReverti);
                }
                else
                {
                    return (false, $"No se puede revertir entidad '{cambio.TipoEntidad}'");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al revertir cambio: {ex.Message}");
            }
        }

        private static (bool exito, string mensaje) RevertirCambioUsuario(BE_CambiosEntidad cambio, BE_USUARIO usuarioReverti)
        {
            try
            {
                var mpUsuario = new MP_USUARIO();
                var usuario = mpUsuario.BuscarPorId(cambio.IdEntidad);

                if (usuario == null)
                    return (false, "Usuario no encontrado");

                string valorActual = obtenerValorUsuario(usuario, cambio.NombreAtributo);

                switch (cambio.NombreAtributo.ToUpper())
                {
                    case "USERNAME":
                        usuario.Username = cambio.ValorAnterior;
                        RecalcularYPersistirUsuario(usuario, mpUsuario);
                        RegistrarCambio("USUARIO", usuario.ID, "USERNAME", valorActual, cambio.ValorAnterior, usuarioReverti, $"Cambio revertido (Memento)");
                        return (true, $"Usuario revertido: {cambio.NombreAtributo}");

                    case "BLOQUEADO":
                        usuario.Bloqueado = cambio.ValorAnterior == "True" || cambio.ValorAnterior == "true" || cambio.ValorAnterior == "1";
                        if (usuario.Bloqueado)
                            mpUsuario.BloquearUsuario(usuario);
                        else
                            mpUsuario.DesbloquearUsuario(usuario);
                        RecalcularYPersistirUsuario(usuario, mpUsuario);
                        RegistrarCambio("USUARIO", usuario.ID, "BLOQUEADO", valorActual, cambio.ValorAnterior, usuarioReverti, $"Cambio revertido (Memento)");
                        return (true, $"Usuario revertido: {cambio.NombreAtributo}");

                    case "PASS":
                    case "PASSWORD":
                    case "HASHEDPASSWORD":
                        // No se puede restaurar el hash anterior si se registró como placeholder;
                        // si el valor anterior parece un hash real, se restaura.
                        if (string.IsNullOrWhiteSpace(cambio.ValorAnterior) || cambio.ValorAnterior == "[hash]")
                            return (false, "No hay hash anterior restaurable para PASS");
                        usuario.HashedPassword = cambio.ValorAnterior;
                        RecalcularYPersistirUsuario(usuario, mpUsuario);
                        RegistrarCambio("USUARIO", usuario.ID, "PASS", valorActual, "[restaurado]", usuarioReverti, $"Cambio revertido (Memento)");
                        return (true, $"Usuario revertido: {cambio.NombreAtributo}");

                    case "ROL":
                        return (false, "La reversión de ROL se realiza desde Gestión de Usuarios / Asignar Rol");

                    case "ELIMINACION":
                        return (false, "No se puede revertir una eliminación desde este cambio; use un Snapshot");

                    case "CREACION":
                        return (false, "No se puede revertir una creación desde este cambio; use Eliminar usuario");

                    default:
                        return (false, $"No se puede revertir atributo '{cambio.NombreAtributo}'");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error revertiendo usuario: {ex.Message}");
            }
        }

        private static void RecalcularYPersistirUsuario(BE_USUARIO usuario, MP_USUARIO mpUsuario)
        {
            BLL_IntegridadDatos.CalcularDVHUsuario(usuario);
            mpUsuario.Editar(usuario);
            mpUsuario.ActualizarDVHorizontal(usuario);

            var dvv = BLL_IntegridadDatos.CalcularDVVUsuario(usuario);
            BLL_IntegridadDatos.PersistirDVVUsuario(dvv);
        }

        private static (bool exito, string mensaje) RevertirCambioRol(BE_CambiosEntidad cambio, BE_USUARIO usuarioReverti)
        {
            try
            {
                var mpRol = new MP_ROL();
                var rol = mpRol.ObtenerPorId(cambio.IdEntidad);

                if (rol == null)
                    return (false, "Rol no encontrado");

                string valorActual = obtenerValorRol(rol, cambio.NombreAtributo);

                switch (cambio.NombreAtributo.ToUpper())
                {
                    case "NOMBRE":
                        rol.Nombre = cambio.ValorAnterior;
                        mpRol.Editar(rol);
                        RegistrarCambio("ROL", rol.ID, "NOMBRE", valorActual, cambio.ValorAnterior, usuarioReverti, $"Cambio revertido (Memento)");
                        return (true, $"Rol revertido: {cambio.NombreAtributo}");

                    case "DESCRIPCION":
                        rol.Descripcion = cambio.ValorAnterior;
                        mpRol.Editar(rol);
                        RegistrarCambio("ROL", rol.ID, "DESCRIPCION", valorActual, cambio.ValorAnterior, usuarioReverti, $"Cambio revertido (Memento)");
                        return (true, $"Rol revertido: {cambio.NombreAtributo}");

                    default:
                        return (false, $"No se puede revertir atributo '{cambio.NombreAtributo}'");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error revertiendo rol: {ex.Message}");
            }
        }

        private static string obtenerValorUsuario(BE_USUARIO usuario, string nombreAtributo)
        {
            switch (nombreAtributo.ToUpper())
            {
                case "USERNAME":
                    return usuario.Username;
                case "BLOQUEADO":
                    return usuario.Bloqueado.ToString();
                case "PASS":
                case "PASSWORD":
                case "HASHEDPASSWORD":
                    return usuario.HashedPassword ?? "";
                case "ROL":
                    return usuario.Rol ?? "";
                default:
                    return "";
            }
        }

        private static string obtenerValorRol(BE_Rol rol, string nombreAtributo)
        {
            switch (nombreAtributo.ToUpper())
            {
                case "NOMBRE":
                    return rol.Nombre;
                case "DESCRIPCION":
                    return rol.Descripcion ?? "";
                default:
                    return "";
            }
        }
    }
}
