using BE;
using SERVICE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormSnapshots : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;

        public FormSnapshots()
        {
            InitializeComponent();

            idiomaManager = SV_IdiomaManager.GetInstance();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            CargarSnapshots();
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("SNAPSHOTS");
            if (lblSnapshots != null) lblSnapshots.Text = idiomaManager.Obtener("SNAPSHOTS");
            if (btnRestaurar != null) btnRestaurar.Text = idiomaManager.Obtener("RESTAURAR");
            if (btnEliminar != null) btnEliminar.Text = idiomaManager.Obtener("ELIMINAR");
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }

        private void CargarSnapshots()
        {
            try
            {
                var snapshots = BLL_Rollback.ListarSnapshots();
                dgvSnapshots.DataSource = null;

                dgvSnapshots.Columns.Clear();
                dgvSnapshots.AutoGenerateColumns = false;
                dgvSnapshots.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ID", DataPropertyName = "ID", Width = 50 });
                dgvSnapshots.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 200 });
                dgvSnapshots.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Descripción", DataPropertyName = "Descripcion", Width = 350 });
                dgvSnapshots.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Fecha", DataPropertyName = "Fecha", Width = 150, DefaultCellStyle = new DataGridViewCellStyle() { Format = "dd/MM/yyyy HH:mm:ss" } });

                DataGridViewSortHelper.Bind(dgvSnapshots, snapshots);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar snapshots: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            RestaurarSnapshot();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarSnapshot();
        }

        private void RestaurarSnapshot()
        {
            if (dgvSnapshots.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un snapshot para restaurar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvSnapshots.SelectedRows[0];
            if (selectedRow.DataBoundItem == null) return;

            dynamic snapshot = selectedRow.DataBoundItem;
            int idSnapshot = snapshot.ID;

            if (MessageBox.Show($"¿Restaurar el snapshot '{snapshot.Nombre}'?\nEsta acción no se puede deshacer.", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                var (exito, mensaje) = BLL_Rollback.RevertirASnapshot(idSnapshot);

                if (exito)
                {
                    MessageBox.Show($"Snapshot restaurado: {mensaje}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarSnapshots();
                }
                else
                {
                    MessageBox.Show($"Error al restaurar: {mensaje}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al restaurar snapshot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarSnapshot()
        {
            if (dgvSnapshots.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un snapshot para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvSnapshots.SelectedRows[0];
            if (selectedRow.DataBoundItem == null) return;

            dynamic snapshot = selectedRow.DataBoundItem;
            int idSnapshot = snapshot.ID;

            if (MessageBox.Show($"¿Eliminar el snapshot '{snapshot.Nombre}'?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                var (exito, mensaje) = BLL_Rollback.EliminarSnapshot(idSnapshot);

                if (exito)
                {
                    MessageBox.Show($"Snapshot eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarSnapshots();
                }
                else
                {
                    MessageBox.Show($"Error al eliminar: {mensaje}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar snapshot: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
