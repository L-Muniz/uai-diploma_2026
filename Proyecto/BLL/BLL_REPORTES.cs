using System;
using System.Collections.Generic;
using System.Data;
using BE;
using SERVICE;

namespace BLL
{
    // Informes exportables (A02): PDF de indicadores y de ventas, y serialización XML de indicadores.
    public class BLL_REPORTES
    {
        private readonly BLL_GESTION bllGestion = new BLL_GESTION();
        private readonly BLL_ESTADISTICAS bllEstadisticas = new BLL_ESTADISTICAS();
        private readonly BLL_BITACORA bllBitacora = new BLL_BITACORA();

        // CU04 en PDF: consulta los indicadores del período y los escribe en el archivo indicado.
        public void ExportarIndicadoresPdf(BE_USUARIO usuario, string periodo, string ruta)
        {
            DataTable indicadores = bllGestion.ConsultarIndicadores(usuario, periodo);
            List<string[]> filas = new List<string[]>();
            foreach (DataRow r in indicadores.Rows)
                filas.Add(new[] { r["Indicador"].ToString(), r["Valor"].ToString() });

            SV_GeneradorPdf.GenerarTabla(ruta, "Indicadores de gestión", $"Período: {periodo}",
                                         new[] { "Indicador", "Valor" }, filas,
                                         $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm} por {usuario.Username}.");
            bllBitacora.IngresarRegistro(usuario, $"Exportó a PDF los indicadores del período {periodo}.");
        }

        // CU04 serializado: guarda el informe de indicadores como XML para poder recuperarlo después.
        public void ExportarIndicadoresXml(BE_USUARIO usuario, string periodo, string ruta)
        {
            DataTable indicadores = bllGestion.ConsultarIndicadores(usuario, periodo);
            var informe = new BE_InformeIndicadores
            {
                Periodo = periodo,
                FechaGeneracion = DateTime.Now,
                Generado = usuario.Username
            };
            foreach (DataRow r in indicadores.Rows)
                informe.Indicadores.Add(new BE_IndicadorItem { Nombre = r["Indicador"].ToString(), Valor = r["Valor"].ToString() });

            SV_Serializacion.GuardarInforme(ruta, informe);
            bllBitacora.IngresarRegistro(usuario, $"Exportó a XML el informe de indicadores del período {periodo}.");
        }

        public BE_InformeIndicadores ImportarIndicadoresXml(string ruta)
        {
            return SV_Serializacion.CargarInforme(ruta);
        }

        // Estadísticas de ventas del vendedor en PDF.
        public void ExportarEstadisticasPdf(BE_USUARIO vendedor, DateTime desde, DateTime hasta, string ruta)
        {
            DataTable resumen = bllEstadisticas.Resumen(vendedor, desde, hasta);
            DataTable productos = bllEstadisticas.VentasPorProducto(vendedor, desde, hasta);

            var filas = new List<string[]>();
            if (resumen.Rows.Count > 0)
            {
                DataRow r = resumen.Rows[0];
                filas.Add(new[] { "Ventas", r["Ventas"].ToString() });
                filas.Add(new[] { "Monto bruto", Convert.ToDecimal(r["MontoBruto"]).ToString("0.00") });
                filas.Add(new[] { "Comisiones", Convert.ToDecimal(r["Comisiones"]).ToString("0.00") });
                filas.Add(new[] { "Neto liquidado", Convert.ToDecimal(r["NetoLiquidado"]).ToString("0.00") });
                filas.Add(new[] { "Neto pendiente", Convert.ToDecimal(r["NetoPendiente"]).ToString("0.00") });
            }
            filas.Add(new[] { "", "" });
            filas.Add(new[] { "Ventas por producto", "" });
            foreach (DataRow p in productos.Rows)
                filas.Add(new[] { p["Producto"].ToString(), $"{p["Unidades"]} u. - {Convert.ToDecimal(p["MontoBruto"]):0.00}" });

            SV_GeneradorPdf.GenerarTabla(ruta, "Estadísticas de ventas", $"Período: {desde:dd/MM/yyyy} al {hasta:dd/MM/yyyy}",
                                         new[] { "Concepto", "Valor" }, filas,
                                         $"Vendedor: {vendedor.Username}. Generado el {DateTime.Now:dd/MM/yyyy HH:mm}.");
            bllBitacora.IngresarRegistro(vendedor, $"Exportó a PDF sus estadísticas de ventas ({desde:dd/MM/yyyy} a {hasta:dd/MM/yyyy}).");
        }
    }
}
