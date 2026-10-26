using System;
using System.Data;
using BE;
using DAL;

namespace BLL
{
    // Estadísticas de ventas propias del vendedor (G05).
    public class BLL_ESTADISTICAS
    {
        private readonly MP_ESTADISTICAS dal = new MP_ESTADISTICAS();

        // Devuelve una fila con: Ventas, MontoBruto, NetoLiquidado, NetoPendiente, Comisiones.
        public DataTable Resumen(BE_USUARIO vendedor, DateTime desde, DateTime hasta)
        {
            BLL_AUTORIZACION.Exigir(vendedor, "PROC_VER_ESTADISTICAS_VENTAS");
            ValidarPeriodo(desde, hasta);
            return dal.ResumenVentas(vendedor.ID, desde, hasta);
        }

        public DataTable VentasPorProducto(BE_USUARIO vendedor, DateTime desde, DateTime hasta)
        {
            BLL_AUTORIZACION.Exigir(vendedor, "PROC_VER_ESTADISTICAS_VENTAS");
            ValidarPeriodo(desde, hasta);
            return dal.VentasPorProducto(vendedor.ID, desde, hasta);
        }

        private static void ValidarPeriodo(DateTime desde, DateTime hasta)
        {
            if (desde.Date > hasta.Date)
                throw new ArgumentException("La fecha 'desde' no puede ser posterior a 'hasta'.");
        }
    }
}
