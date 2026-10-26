using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    // Estadísticas de ventas del vendedor (G05). Todo mediante stored procedures.
    public class MP_ESTADISTICAS
    {
        public DataTable ResumenVentas(int idVendedor, DateTime desde, DateTime hasta)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = Parametros(acc, idVendedor, desde, hasta);
            DataTable dt = acc.Leer("VENDEDOR_ESTADISTICAS_RESUMEN", p);
            acc.Cerrar();
            return dt;
        }

        public DataTable VentasPorProducto(int idVendedor, DateTime desde, DateTime hasta)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = Parametros(acc, idVendedor, desde, hasta);
            DataTable dt = acc.Leer("VENDEDOR_ESTADISTICAS_PRODUCTOS", p);
            acc.Cerrar();
            return dt;
        }

        private static List<SqlParameter> Parametros(ACCESO acc, int idVendedor, DateTime desde, DateTime hasta)
        {
            return new List<SqlParameter>
            {
                acc.CrearParametro("@ID_VENDEDOR", idVendedor),
                acc.CrearParametro("@DESDE", desde.Date),
                acc.CrearParametro("@HASTA", hasta.Date)
            };
        }
    }
}
