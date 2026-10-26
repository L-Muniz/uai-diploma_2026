using System;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU11: el comprador reclama una orden despachada; el área comercial audita y resuelve el reclamo.
    public partial class FormReclamos : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_MARKETPLACE bllMarketplace = new BLL_MARKETPLACE();

        public FormReclamos(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            if (!BLL_AUTORIZACION.TienePermiso(usuario, "PROC_RECLAMAR")) tabControl.TabPages.Remove(tabPresentar);
            if (!BLL_AUTORIZACION.TienePermiso(usuario, "PROC_RESOLVER_RECLAMO")) tabControl.TabPages.Remove(tabResolver);
            Refrescar();
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvOrdenes, bllMarketplace.ListarOrdenesComprador(usuario.ID));
            UIHelper.CargarGrilla(dgvReclamos, bllMarketplace.ListarReclamosAbiertos());
        }

        private void btnPresentar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvOrdenes.CurrentRow == null)
                    throw new ArgumentException("Seleccione la orden a reclamar.");

                int idOrden = Convert.ToInt32(dgvOrdenes.CurrentRow.Cells["ID"].Value);
                int idReclamo = bllMarketplace.PresentarReclamo(usuario, idOrden, txtMotivo.Text.Trim());
                UIHelper.Informar($"Reclamo {idReclamo} presentado. El saldo del vendedor quedó congelado hasta su resolución.");
                txtMotivo.Clear();
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnProcedente_Click(object sender, EventArgs e)
        {
            Resolver(true);
        }

        private void btnImprocedente_Click(object sender, EventArgs e)
        {
            Resolver(false);
        }

        private void Resolver(bool procedente)
        {
            try
            {
                if (dgvReclamos.CurrentRow == null)
                    throw new ArgumentException("Seleccione un reclamo.");

                int idReclamo = Convert.ToInt32(dgvReclamos.CurrentRow.Cells["ID"].Value);
                bllMarketplace.ResolverReclamo(usuario, idReclamo, procedente);
                UIHelper.Informar(procedente
                    ? "Reclamo procedente: la orden se devolvió, el stock se restituyó y la liquidación se canceló."
                    : "Reclamo improcedente: la liquidación volvió a retenida hasta la confirmación del comprador.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
