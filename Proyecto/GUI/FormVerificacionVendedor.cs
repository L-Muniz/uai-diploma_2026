using System;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU07: el usuario solicita verificación para vender; el área comercial aprueba o rechaza.
    public partial class FormVerificacionVendedor : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_MARKETPLACE bllMarketplace = new BLL_MARKETPLACE();

        public FormVerificacionVendedor(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            bool puedeSolicitar = BLL_AUTORIZACION.TienePermiso(usuario, "PROC_SOLICITAR_VERIFICACION");
            bool puedeResolver = BLL_AUTORIZACION.TienePermiso(usuario, "PROC_RESOLVER_VERIFICACION");
            txtDocumento.Enabled = puedeSolicitar;
            btnSolicitar.Enabled = puedeSolicitar;
            dgvSolicitudes.Enabled = puedeResolver;
            btnAprobar.Enabled = puedeResolver;
            btnRechazar.Enabled = puedeResolver;
            Refrescar();
        }

        private void Refrescar()
        {
            string estado = bllMarketplace.ObtenerEstadoVerificacion(usuario.ID) ?? "sin solicitud registrada";
            lblEstado.Text = $"Estado: {estado}";
            UIHelper.CargarGrilla(dgvSolicitudes, bllMarketplace.ListarSolicitudesVerificacion());
        }

        private void btnSolicitar_Click(object sender, EventArgs e)
        {
            try
            {
                bllMarketplace.SolicitarVerificacion(usuario, txtDocumento.Text.Trim());
                UIHelper.Informar("Solicitud enviada. El área comercial la revisará.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            ResolverSeleccion(true);
        }

        private void btnRechazar_Click(object sender, EventArgs e)
        {
            ResolverSeleccion(false);
        }

        private void ResolverSeleccion(bool aprobada)
        {
            try
            {
                if (dgvSolicitudes.CurrentRow == null)
                    throw new ArgumentException("Seleccione una solicitud.");

                int idSolicitante = Convert.ToInt32(dgvSolicitudes.CurrentRow.Cells["IDUsuario"].Value);
                bllMarketplace.ResolverVerificacion(usuario, idSolicitante, aprobada);
                UIHelper.Informar(aprobada ? "Vendedor verificado." : "Solicitud rechazada.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
