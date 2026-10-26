using System;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU06: la gerencia fija la comisión por categoría y la tabla de conversión de Eco-Puntos.
    public partial class FormTarifas : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_GESTION bllGestion = new BLL_GESTION();

        public FormTarifas(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            UIHelper.CargarCombo(cboMaterial, bllGestion.ListarMateriales(), "Nombre", "ID");
            Refrescar();
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvTarifas, bllGestion.ListarTarifas());
            UIHelper.CargarGrilla(dgvConversiones, bllGestion.ListarConversiones());
        }

        private void btnProponerTarifa_Click(object sender, EventArgs e)
        {
            try
            {
                int id = bllGestion.ProponerTarifa(usuario, txtCategoria.Text.Trim(), numPorcentaje.Value);
                UIHelper.Informar($"Comisión fijada (tarifa {id}). Rige desde hoy y queda registrada como decisión.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnProponerConversion_Click(object sender, EventArgs e)
        {
            try
            {
                int idMaterial = Convert.ToInt32(cboMaterial.SelectedValue);
                decimal puntos = UIHelper.LeerDecimal(txtPuntosKg.Text, "Puntos por kg");
                int id = bllGestion.ProponerConversion(usuario, idMaterial, puntos);
                UIHelper.Informar($"Conversión fijada (ID {id}). Queda registrada como decisión.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
