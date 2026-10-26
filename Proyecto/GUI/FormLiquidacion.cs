using System;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU10: el comprador confirma la recepción, el vendedor despacha y el área comercial liquida el saldo neto.
    public partial class FormLiquidacion : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_MARKETPLACE bllMarketplace = new BLL_MARKETPLACE();

        public FormLiquidacion(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            if (!BLL_AUTORIZACION.TienePermiso(usuario, "PROC_CONFIRMAR_RECEPCION")) tabControl.TabPages.Remove(tabCompras);
            if (!BLL_AUTORIZACION.TienePermiso(usuario, "PROC_DESPACHAR")) tabControl.TabPages.Remove(tabVentas);
            if (!BLL_AUTORIZACION.TienePermiso(usuario, "PROC_LIQUIDAR")) tabControl.TabPages.Remove(tabLiquidaciones);
            Refrescar();
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvCompras, bllMarketplace.ListarOrdenesComprador(usuario.ID));
            UIHelper.CargarGrilla(dgvVentas, bllMarketplace.ListarOrdenesVendedor(usuario.ID));
            UIHelper.CargarGrilla(dgvLiquidaciones, bllMarketplace.ListarLiquidaciones("DISPONIBLE"));
        }

        private int? IdSeleccionado(DataGridView grilla)
        {
            return grilla.CurrentRow == null ? (int?)null : Convert.ToInt32(grilla.CurrentRow.Cells["ID"].Value);
        }

        private void btnConfirmarRecepcion_Click(object sender, EventArgs e)
        {
            try
            {
                int? id = IdSeleccionado(dgvCompras) ?? throw new ArgumentException("Seleccione una compra.");
                bllMarketplace.ConfirmarRecepcion(usuario, id.Value);
                UIHelper.Informar("Recepción confirmada. El saldo quedó disponible para liquidar.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnDespachar_Click(object sender, EventArgs e)
        {
            try
            {
                int? id = IdSeleccionado(dgvVentas) ?? throw new ArgumentException("Seleccione una venta.");
                bllMarketplace.MarcarDespachada(usuario, id.Value);
                UIHelper.Informar("Orden marcada como despachada.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnPagar_Click(object sender, EventArgs e)
        {
            try
            {
                int? id = IdSeleccionado(dgvLiquidaciones) ?? throw new ArgumentException("Seleccione una liquidación.");
                if (!UIHelper.Confirmar("¿Ordenar la transferencia al vendedor por el saldo neto?"))
                    return;

                bllMarketplace.PagarLiquidacion(usuario, id.Value);
                UIHelper.Informar("Transferencia ordenada. La liquidación quedó pagada.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
