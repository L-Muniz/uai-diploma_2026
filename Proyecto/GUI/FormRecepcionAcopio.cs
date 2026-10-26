using System;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU02: el operador del punto verde registra el peso del material entregado y acredita los Eco-Puntos.
    public partial class FormRecepcionAcopio : Form
    {
        private readonly BE_USUARIO operador;
        private readonly BLL_RECICLAJE bllReciclaje = new BLL_RECICLAJE();

        public FormRecepcionAcopio(BE_USUARIO operador)
        {
            InitializeComponent();
            this.operador = operador;
            UIHelper.CargarCombo(cboPuntoVerde, bllReciclaje.ListarPuntosVerdes(), "Nombre", "ID");
            Refrescar();
            dgvPendientes.SelectionChanged += (s, e) => ActualizarPesoSugerido();
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvPendientes, bllReciclaje.ListarDeclaracionesPendientes());
            ActualizarPesoSugerido();
        }

        private void ActualizarPesoSugerido()
        {
            if (dgvPendientes.CurrentRow != null && dgvPendientes.CurrentRow.Cells["Cantidad"].Value != null)
                txtPeso.Text = Convert.ToDecimal(dgvPendientes.CurrentRow.Cells["Cantidad"].Value).ToString("0.00");
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPendientes.CurrentRow == null)
                    throw new ArgumentException("Seleccione una declaración pendiente.");
                if (cboPuntoVerde.SelectedValue == null)
                    throw new ArgumentException("Seleccione el punto verde.");

                DataGridViewRow fila = dgvPendientes.CurrentRow;
                int idPuntoVerde = Convert.ToInt32(cboPuntoVerde.SelectedValue);
                int idDeclaracion = Convert.ToInt32(fila.Cells["ID"].Value);
                int idUsuario = Convert.ToInt32(fila.Cells["IDUsuario"].Value);
                int idMaterial = Convert.ToInt32(fila.Cells["IDMaterial"].Value);
                decimal peso = UIHelper.LeerDecimal(txtPeso.Text, "Peso");

                int puntos = bllReciclaje.RegistrarRecepcion(operador, idPuntoVerde, idDeclaracion, idUsuario, idMaterial, peso);
                UIHelper.Informar($"Recepción registrada. Se acreditaron {puntos} Eco-Puntos al ciudadano.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
