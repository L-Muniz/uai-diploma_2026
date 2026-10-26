using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // Estadísticas de ventas propias del vendedor (G05), por período.
    public partial class FormEstadisticasVentas : Form
    {
        private readonly BE_USUARIO vendedor;
        private readonly BLL_ESTADISTICAS bllEstadisticas = new BLL_ESTADISTICAS();

        public FormEstadisticasVentas(BE_USUARIO vendedor)
        {
            InitializeComponent();
            this.vendedor = vendedor;
            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;
            Consultar();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            Consultar();
        }

        private void Consultar()
        {
            try
            {
                DataTable resumen = bllEstadisticas.Resumen(vendedor, dtpDesde.Value, dtpHasta.Value);
                if (resumen.Rows.Count > 0)
                {
                    DataRow r = resumen.Rows[0];
                    lblResumen.Text =
                        $"Ventas: {r["Ventas"]}     Monto bruto: {Money(r["MontoBruto"])}     Comisiones: {Money(r["Comisiones"])}\r\n" +
                        $"Neto liquidado: {Money(r["NetoLiquidado"])}     Neto pendiente de liquidar: {Money(r["NetoPendiente"])}";
                }
                UIHelper.CargarGrilla(dgvProductos, bllEstadisticas.VentasPorProducto(vendedor, dtpDesde.Value, dtpHasta.Value));
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private static string Money(object valor)
        {
            return Convert.ToDecimal(valor).ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
}
