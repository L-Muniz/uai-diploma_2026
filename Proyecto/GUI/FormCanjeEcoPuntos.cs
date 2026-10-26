using System;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU03: el ciudadano canjea Eco-Puntos por un cupón o un envío bonificado.
    public partial class FormCanjeEcoPuntos : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_ECOPUNTOS bllEcoPuntos = new BLL_ECOPUNTOS();

        public FormCanjeEcoPuntos(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            Refrescar();
        }

        private void Refrescar()
        {
            lblSaldo.Text = $"Saldo: {bllEcoPuntos.ObtenerSaldo(usuario.ID)} Eco-Puntos";
            UIHelper.CargarGrilla(dgvBeneficios, bllEcoPuntos.ListarBeneficios());
            UIHelper.CargarGrilla(dgvMovimientos, bllEcoPuntos.ListarMovimientos(usuario.ID));
        }

        private void btnCanjear_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBeneficios.CurrentRow == null)
                    throw new ArgumentException("Seleccione un beneficio.");

                int idBeneficio = Convert.ToInt32(dgvBeneficios.CurrentRow.Cells["ID"].Value);
                string descripcion = dgvBeneficios.CurrentRow.Cells["Descripcion"].Value.ToString();
                if (!UIHelper.Confirmar($"¿Canjear \"{descripcion}\"?"))
                    return;

                bllEcoPuntos.Canjear(usuario, idBeneficio);
                UIHelper.Informar("Canje realizado. Su cupón o envío bonificado quedó registrado.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
