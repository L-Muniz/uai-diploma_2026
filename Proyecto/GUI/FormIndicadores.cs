using System;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU04: la gerencia consulta los indicadores del período para decidir sobre la red y las tarifas.
    public partial class FormIndicadores : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_GESTION bllGestion = new BLL_GESTION();

        public FormIndicadores(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            txtPeriodo.Text = DateTime.Now.ToString("yyyy-MM");
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                UIHelper.CargarGrilla(dgvIndicadores, bllGestion.ConsultarIndicadores(usuario, txtPeriodo.Text.Trim()));
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
