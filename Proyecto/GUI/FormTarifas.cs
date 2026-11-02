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
            dgvConversiones.SelectionChanged += (s, e) => CargarConversionSeleccionada();
            CargarConversionSeleccionada();
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvTarifas, bllGestion.ListarTarifas());
            UIHelper.CargarGrilla(dgvConversiones, bllGestion.ListarConversiones());
        }

        // Al elegir una fila de conversiones, el material y los puntos por kg pasan a los controles de abajo.
        private void CargarConversionSeleccionada()
        {
            if (dgvConversiones.CurrentRow == null) return;

            string material = dgvConversiones.CurrentRow.Cells["Material"].Value?.ToString();
            for (int i = 0; i < cboMaterial.Items.Count; i++)
            {
                if (string.Equals(cboMaterial.GetItemText(cboMaterial.Items[i]), material, StringComparison.OrdinalIgnoreCase))
                {
                    cboMaterial.SelectedIndex = i;
                    break;
                }
            }

            txtPuntosKg.Text = Convert.ToDecimal(dgvConversiones.CurrentRow.Cells["PuntosPorKg"].Value)
                                      .ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
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
