using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using SERVICE;

namespace BLL
{
    public class BLL_BITACORA
    {
        public class CriteriosBusqueda
        {
            public int? IdUsuario { get; set; }
            public DateTime? FechaDesde { get; set; }
            public DateTime? FechaHasta { get; set; }
            public string Descripcion { get; set; }
            public TipoEvento? TipoEvento { get; set; }
        }

        public enum TipoEvento
        {
            Todos = 0,
            Login = 1,
            Logout = 2,
            CreacionUsuario = 3,
            BloqueoUsuario = 4,
            DesbloqueUsuario = 5,
            CambioContrasena = 6,
            Otros = 7
        }

        public List<BE_BITACORA> Listar()
        {
            MP_BITACORA mpBitacora = new MP_BITACORA();
            return mpBitacora.Listar()
                .OrderByDescending(b => b.Fecha)
                .ThenByDescending(b => b.ID)
                .ToList();
        }

        public List<BE_BITACORA> ListarXID(BE_USUARIO usuario)
        {
            MP_BITACORA mpBitacora = new MP_BITACORA();
            return mpBitacora.ListarXID(usuario)
                .OrderByDescending(b => b.Fecha)
                .ThenByDescending(b => b.ID)
                .ToList();
        }

        public void IngresarRegistro(BE_USUARIO usuario, string descripcion)
        {
            if (usuario == null)
                throw new ArgumentException("No se puede registrar en bitácora sin usuario.");

            if (string.IsNullOrWhiteSpace(descripcion))
                descripcion = "(sin descripción)";

            if (descripcion.Length > 500)
                descripcion = descripcion.Substring(0, 497) + "...";

            BE_BITACORA bitacora = new BE_BITACORA
            {
                Usuario = usuario,
                Descripcion = descripcion,
                Fecha = DateTime.Now
            };
            // La operación de negocio ya quedó guardada cuando se registra en bitácora:
            // un fallo de bitácora no debe mostrarla como error. Se informa en el log de depuración.
            try
            {
                MP_BITACORA mpBitacora = new MP_BITACORA();
                int filas = mpBitacora.Alta(bitacora);

                if (filas != 1 && filas != -1)
                {
                    System.Diagnostics.Debug.WriteLine("Bitácora: no se registró la descripción: " + descripcion);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Bitácora: error al registrar '" + descripcion + "': " + ex.Message);
            }
        }

        public List<BE_BITACORA> ObtenerTodosLosRegistros()
        {
            return Listar();
        }

        public List<BE_BITACORA> FiltrarPorUsuario(BE_USUARIO usuario)
        {
            if (usuario == null)
                return new List<BE_BITACORA>();

            return ListarXID(usuario);
        }

        public List<BE_BITACORA> FiltrarPorFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<BE_BITACORA> todos = ObtenerTodosLosRegistros();

            return todos.Where(b => b.Fecha >= fechaDesde && b.Fecha <= fechaHasta)
                        .OrderByDescending(b => b.Fecha)
                        .ToList();
        }

        public List<BE_BITACORA> FiltrarPorTipoEvento(TipoEvento tipo)
        {
            List<BE_BITACORA> todos = ObtenerTodosLosRegistros();

            if (tipo == TipoEvento.Todos)
                return todos;

            return todos.Where(b => ClasificarTipoEvento(b.Descripcion) == tipo)
                        .OrderByDescending(b => b.Fecha)
                        .ToList();
        }

        public List<BE_BITACORA> FiltrarPorDescripcion(string textoBusqueda)
        {
            if (string.IsNullOrEmpty(textoBusqueda))
                return ObtenerTodosLosRegistros();

            List<BE_BITACORA> todos = ObtenerTodosLosRegistros();

            return todos.Where(b => b.Descripcion.ToLower().Contains(textoBusqueda.ToLower()))
                        .OrderByDescending(b => b.Fecha)
                        .ToList();
        }

        public List<BE_BITACORA> FiltrarAvanzado(CriteriosBusqueda criterios)
        {
            if (criterios == null)
                return ObtenerTodosLosRegistros();

            List<BE_BITACORA> resultados = ObtenerTodosLosRegistros();

            if (criterios.IdUsuario.HasValue && criterios.IdUsuario.Value > 0)
            {
                resultados = resultados.Where(b => b.Usuario.ID == criterios.IdUsuario.Value).ToList();
            }

            if (criterios.FechaDesde.HasValue)
            {
                resultados = resultados.Where(b => b.Fecha >= criterios.FechaDesde.Value).ToList();
            }

            if (criterios.FechaHasta.HasValue)
            {
                resultados = resultados.Where(b => b.Fecha <= criterios.FechaHasta.Value).ToList();
            }

            if (criterios.TipoEvento.HasValue && criterios.TipoEvento.Value != TipoEvento.Todos)
            {
                TipoEvento tipoFiltro = criterios.TipoEvento.Value;
                resultados = resultados.Where(b => ClasificarTipoEvento(b.Descripcion) == tipoFiltro).ToList();
            }

            if (!string.IsNullOrEmpty(criterios.Descripcion))
            {
                resultados = resultados.Where(b => b.Descripcion.ToLower().Contains(criterios.Descripcion.ToLower())).ToList();
            }

            return resultados.OrderByDescending(b => b.Fecha).ThenByDescending(b => b.ID).ToList();
        }

        public static TipoEvento ClasificarTipoEvento(string descripcion)
        {
            if (string.IsNullOrEmpty(descripcion))
                return TipoEvento.Otros;

            if (descripcion.IndexOf("desbloqueado", StringComparison.OrdinalIgnoreCase) >= 0)
                return TipoEvento.DesbloqueUsuario;
            if (descripcion.IndexOf(SV_Mensajes.GetLoginMessage(), StringComparison.OrdinalIgnoreCase) >= 0)
                return TipoEvento.Login;
            if (descripcion.IndexOf(SV_Mensajes.GetLogoutMessage(), StringComparison.OrdinalIgnoreCase) >= 0)
                return TipoEvento.Logout;
            if (descripcion.IndexOf(SV_Mensajes.GetNewUserMessage(), StringComparison.OrdinalIgnoreCase) >= 0)
                return TipoEvento.CreacionUsuario;
            if (descripcion.IndexOf("bloqueado", StringComparison.OrdinalIgnoreCase) >= 0)
                return TipoEvento.BloqueoUsuario;
            if (descripcion.IndexOf("contrasena", StringComparison.OrdinalIgnoreCase) >= 0
                || descripcion.IndexOf("contraseña", StringComparison.OrdinalIgnoreCase) >= 0)
                return TipoEvento.CambioContrasena;

            return TipoEvento.Otros;
        }

        public static string ObtenerNombreTipoEvento(string descripcion)
        {
            return ClasificarTipoEvento(descripcion).ToString();
        }

        public Dictionary<string, int> ObtenerEstadisticasPorTipo()
        {
            List<BE_BITACORA> todos = ObtenerTodosLosRegistros();
            Dictionary<string, int> estadisticas = new Dictionary<string, int>();

            estadisticas["Inicios de Sesion"] = todos.Count(b => b.Descripcion.Contains(SV_Mensajes.GetLoginMessage()));
            estadisticas["Cierres de Sesion"] = todos.Count(b => b.Descripcion.Contains(SV_Mensajes.GetLogoutMessage()));
            estadisticas["Creacion de Usuarios"] = todos.Count(b => b.Descripcion.Contains(SV_Mensajes.GetNewUserMessage()));
            estadisticas["Total Registros"] = todos.Count;

            return estadisticas;
        }

        public List<BE_BITACORA> ObtenerUltimosRegistros(int cantidad = 10)
        {
            List<BE_BITACORA> todos = ObtenerTodosLosRegistros();
            return todos.OrderByDescending(b => b.Fecha).Take(cantidad).ToList();
        }

        public List<BE_BITACORA> ObtenerActividadUsuarioPorPeriodo(BE_USUARIO usuario, DateTime fechaDesde, DateTime fechaHasta)
        {
            if (usuario == null)
                return new List<BE_BITACORA>();

            List<BE_BITACORA> actividadUsuario = ListarXID(usuario);

            return actividadUsuario.Where(b => b.Fecha >= fechaDesde && b.Fecha <= fechaHasta)
                                   .OrderByDescending(b => b.Fecha)
                                   .ToList();
        }

        public string GenerarReporteActividad(List<BE_BITACORA> registros)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("===========================================");
            sb.AppendLine("REPORTE DE ACTIVIDAD DE BITACORA");
            sb.AppendLine($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine($"Total de registros: {registros.Count}");
            sb.AppendLine("===========================================");
            sb.AppendLine();

            foreach (var registro in registros.Take(50))
            {
                sb.AppendLine($"[{registro.Fecha:dd/MM/yyyy HH:mm:ss}] {registro.Usuario.Username}");
                sb.AppendLine($"   Accion: {registro.Descripcion}");
                sb.AppendLine();
            }

            if (registros.Count > 50)
            {
                sb.AppendLine($"... y {registros.Count - 50} registros mas");
            }

            return sb.ToString();
        }
    }
}
