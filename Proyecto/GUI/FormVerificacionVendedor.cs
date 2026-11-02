using System;
using System.Drawing;
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

            // Cada rol ve solo su parte: el vendedor solicita y el área comercial resuelve.
            lblEstado.Visible = puedeSolicitar;
            lblDocumento.Visible = puedeSolicitar;
            txtDocumento.Visible = puedeSolicitar;
            btnSolicitar.Visible = puedeSolicitar;
            lblSolicitudes.Visible = puedeResolver;
            dgvSolicitudes.Visible = puedeResolver;
            btnAprobar.Visible = puedeResolver;
            btnRechazar.Visible = puedeResolver;

            if (!puedeSolicitar)
            {
                // El área comercial no solicita: la lista de pendientes sube y los botones van debajo.
                lblSolicitudes.Location = new Point(16, 44);
                dgvSolicitudes.Location = new Point(16, 66);
                btnAprobar.Location = new Point(16, dgvSolicitudes.Bottom + 10);
                btnRechazar.Location = new Point(180, dgvSolicitudes.Bottom + 10);
            }
            Refrescar();
        }

        private void Refrescar()
        {
            string estado = bllMarketplace.ObtenerEstadoVerificacion(usuario.ID) ?? "sin solicitud registrada";
            lblEstado.Text = $"Estado: {estado}";
            // Con una solicitud pendiente no se puede enviar otra (evita duplicados).
            btnSolicitar.Enabled = btnSolicitar.Visible && estado != "PENDIENTE";
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
