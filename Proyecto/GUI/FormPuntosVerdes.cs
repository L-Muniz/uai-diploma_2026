using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU05: la gerencia administra los puntos verdes y los materiales que cada uno acepta.
    public partial class FormPuntosVerdes : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_GESTION bllGestion = new BLL_GESTION();
        private int idSeleccionado;

        public FormPuntosVerdes(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            dgvPuntos.SelectionChanged += (s, e) => CargarSeleccion();
            CargarMateriales();
            Refrescar();
            Nuevo();
        }

        private void CargarMateriales()
        {
            clbMateriales.Items.Clear();
            foreach (DataRow r in bllGestion.ListarMateriales().Rows)
                clbMateriales.Items.Add(new UIHelper.ItemLista { ID = Convert.ToInt32(r["ID"]), Texto = r["Nombre"].ToString() });
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvPuntos, bllGestion.ListarPuntosVerdes());
        }

        private void CargarSeleccion()
        {
            if (dgvPuntos.CurrentRow == null) return;
            idSeleccionado = Convert.ToInt32(dgvPuntos.CurrentRow.Cells["ID"].Value);
            txtNombre.Text = dgvPuntos.CurrentRow.Cells["Nombre"].Value.ToString();
            txtDireccion.Text = dgvPuntos.CurrentRow.Cells["Direccion"].Value.ToString();
            txtLatitud.Text = dgvPuntos.CurrentRow.Cells["Latitud"].Value?.ToString() ?? "";
            txtLongitud.Text = dgvPuntos.CurrentRow.Cells["Longitud"].Value?.ToString() ?? "";
            txtHorario.Text = dgvPuntos.CurrentRow.Cells["Horario"].Value?.ToString() ?? "";
            chkActivo.Checked = dgvPuntos.CurrentRow.Cells["Activo"].Value.ToString() == "Sí";

            DataTable aceptados = bllGestion.ListarMaterialesAceptados(idSeleccionado);
            for (int i = 0; i < clbMateriales.Items.Count; i++)
            {
                var item = (UIHelper.ItemLista)clbMateriales.Items[i];
                clbMateriales.SetItemChecked(i, aceptados.AsEnumerable().Any(r => Convert.ToInt32(r["ID"]) == item.ID));
            }
        }

        private void Nuevo()
        {
            idSeleccionado = 0;
            txtNombre.Clear();
            txtDireccion.Clear();
            txtLatitud.Clear();
            txtLongitud.Clear();
            txtHorario.Clear();
            chkActivo.Checked = true;
            chkValidado.Checked = false;
            for (int i = 0; i < clbMateriales.Items.Count; i++)
                clbMateriales.SetItemChecked(i, false);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Nuevo();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var pv = new BE_PuntoVerde
                {
                    ID = idSeleccionado,
                    Nombre = txtNombre.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    Latitud = string.IsNullOrWhiteSpace(txtLatitud.Text) ? (decimal?)null : UIHelper.LeerDecimal(txtLatitud.Text, "Latitud"),
                    Longitud = string.IsNullOrWhiteSpace(txtLongitud.Text) ? (decimal?)null : UIHelper.LeerDecimal(txtLongitud.Text, "Longitud"),
                    Horario = txtHorario.Text.Trim(),
                    Activo = chkActivo.Checked
                };

                var materiales = new List<int>();
                foreach (var item in clbMateriales.CheckedItems.Cast<UIHelper.ItemLista>())
                    materiales.Add(item.ID);

                int id = bllGestion.GuardarPuntoVerde(usuario, pv, materiales, chkValidado.Checked);
                UIHelper.Informar($"Punto verde guardado (ID {id}). La decisión quedó registrada.");
                Refrescar();
                chkValidado.Checked = false;
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
