using BE;
using BLL;
using SERVICE;
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
    public partial class FormAuditoria : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private List<BE_CambiosEntidad> cambiosActuales;
        private BLL_Rollback bllRollback;
        private ComboBox cbVista;
        private bool mostrandoSnapshots = false;

        public FormAuditoria()
        {
            InitializeComponent();

            idiomaManager = SV_IdiomaManager.GetInstance();
            bllRollback = new BLL_Rollback();

            CrearComboVista();

            dtDesde.Value = DateTime.Now.AddDays(-30);
            dtHasta.Value = DateTime.Now;
            dtDesde.CustomFormat = "dd/MM/yyyy HH:mm";
            dtDesde.Format = DateTimePickerFormat.Custom;
            dtHasta.CustomFormat = "dd/MM/yyyy HH:mm";
            dtHasta.Format = DateTimePickerFormat.Custom;

            CrearColumnasDataGrid();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            CargarCambios();
        }

        private void CrearComboVista()
        {
            cbVista = new ComboBox();
            cbVista.Location = new Point(10, 10);
            cbVista.Width = 150;
            cbVista.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVista.Items.Add("Auditoría");
            cbVista.Items.Add("Snapshots");
            cbVista.SelectedIndex = 0;
            cbVista.SelectedIndexChanged += (s, e) => CambiarVista();
            this.Controls.Add(cbVista);
        }

        private void CambiarVista()
        {
            mostrandoSnapshots = cbVista.SelectedIndex == 1;

            if (mostrandoSnapshots)
            {
                dtDesde.Visible = false;
                dtHasta.Visible = false;
                cbEntidad.Visible = false;
                btnFiltrar.Visible = false;
                lblEntidad.Visible = false;
                lblFechaDesde.Visible = false;
                lblFechaHasta.Visible = false;

                CargarSnapshots();
            }
            else
            {
                dtDesde.Visible = true;
                dtHasta.Visible = true;
                cbEntidad.Visible = true;
                btnFiltrar.Visible = true;
                lblEntidad.Visible = true;
                lblFechaDesde.Visible = true;
                lblFechaHasta.Visible = true;

                CargarCambios();
            }

            ActualizarTextos();
        }

        private void btnDeshacer_Click(object sender, EventArgs e)
        {
            if (mostrandoSnapshots)
            {
                RevertirSnapshot();
            }
            else
            {
                RevertirCambio();
            }
        }

        private void RevertirSnapshot()
        {
            if (dgvCambios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un snapshot para deshacer", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvCambios.SelectedRows[0];
            int idSnapshot = Convert.ToInt32(row.Cells["ID"].Value);
            string nombre = row.Cells["Nombre"].Value?.ToString() ?? "desconocido";

            if (MessageBox.Show($"¿Deshacer a '{nombre}'?\nEsta acción es irreversible.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var resultado = BLL_Rollback.RevertirASnapshot(idSnapshot);
                if (resultado.exito)
                {
                    MessageBox.Show($"Snapshot restaurado: {resultado.mensaje}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarSnapshots();
                }
                else
                {
                    MessageBox.Show($"Error al restaurar: {resultado.mensaje}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RevertirCambio()
        {
            if (dgvCambios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un cambio para deshacer", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cambiosActuales == null || cambiosActuales.Count == 0)
                return;

            var row = dgvCambios.SelectedRows[0];
            int indiceRow = dgvCambios.SelectedRows[0].Index;

            if (indiceRow >= 0 && indiceRow < cambiosActuales.Count)
            {
                var cambio = cambiosActuales[indiceRow];
                string descripcion = $"{cambio.TipoEntidad}({cambio.IdEntidad}).{cambio.NombreAtributo}: '{cambio.ValorNuevo}' -> '{cambio.ValorAnterior}'";

                if (MessageBox.Show($"¿Revertir este cambio?\n{descripcion}\n\nEsta acción es irreversible.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var usuario = SV_Singleton.GetInstance().Usuario;
                    var resultado = BLL_ControlCambios.RevertirCambio(cambio, usuario);

                    if (resultado.exito)
                    {
                        MessageBox.Show($"Cambio revertido: {resultado.mensaje}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarCambios();
                    }
                    else
                    {
                        MessageBox.Show($"Error al revertir: {resultado.mensaje}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CrearColumnasDataGrid()
        {
            dgvCambios.AutoGenerateColumns = false;
            dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ID", DataPropertyName = "IdEntidad", Width = 50 });
            dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Fecha", DataPropertyName = "FechaCambio", Width = 150, DefaultCellStyle = new DataGridViewCellStyle() { Format = "dd/MM/yyyy HH:mm:ss" } });
            dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Entidad", DataPropertyName = "TipoEntidad", Width = 100 });
            dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Atributo", DataPropertyName = "NombreAtributo", Width = 120 });
            dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Valor Anterior", DataPropertyName = "ValorAnterior", Width = 150 });
            dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Valor Nuevo", DataPropertyName = "ValorNuevo", Width = 150 });
            dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Usuario", DataPropertyName = "Usuario", Width = 100 });
            dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Descripción", DataPropertyName = "Descripcion", Width = 200 });
            DataGridViewSortHelper.HabilitarOrdenamiento(dgvCambios);
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("AUDITORIA");
            if (lblEntidad != null) lblEntidad.Text = idiomaManager.Obtener("TIPO_ENTIDAD");
            if (lblFechaDesde != null) lblFechaDesde.Text = idiomaManager.Obtener("FECHA_DESDE");
            if (lblFechaHasta != null) lblFechaHasta.Text = idiomaManager.Obtener("FECHA_HASTA");
            if (btnFiltrar != null) btnFiltrar.Text = idiomaManager.Obtener("FILTRAR");
            if (btnExportar != null) btnExportar.Text = idiomaManager.Obtener("EXPORTAR");
            if (lblActualizar != null) lblActualizar.Text = idiomaManager.Obtener("TOTAL_CAMBIOS");

            if (dgvCambios.Columns.Count >= 8)
            {
                dgvCambios.Columns[0].HeaderText = "ID";
                dgvCambios.Columns[1].HeaderText = idiomaManager.Obtener("FECHA");
                dgvCambios.Columns[2].HeaderText = idiomaManager.Obtener("ENTIDAD");
                dgvCambios.Columns[3].HeaderText = idiomaManager.Obtener("NOMBRE");
                dgvCambios.Columns[7].HeaderText = idiomaManager.Obtener("DESCRIPCION");
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            FiltrarCambios();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            ExportarReporte();
        }

        private void CargarCambios()
        {
            try
            {
                cambiosActuales = BLL_ControlCambios.ObtenerCambiosRecientes(500);
                MostrarCambios(cambiosActuales);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar cambios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FiltrarCambios()
        {
            try
            {
                if (dtHasta.Value < dtDesde.Value)
                {
                    MessageBox.Show("La fecha hasta no puede ser menor que la fecha desde.", "Auditoría",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string tipoEntidad = cbEntidad.SelectedItem.ToString();
                DateTime fechaDesde = dtDesde.Value;
                DateTime fechaHasta = dtHasta.Value;

                List<BE_CambiosEntidad> cambiosFiltrados = BLL_ControlCambios.ObtenerCambiosPorFechas(fechaDesde, fechaHasta);

                if (tipoEntidad != "Todos")
                {
                    cambiosFiltrados = cambiosFiltrados.Where(c => c.TipoEntidad == tipoEntidad).ToList();
                }

                MostrarCambios(cambiosFiltrados);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarCambios(List<BE_CambiosEntidad> cambios)
        {
            var filas = cambios.Select(c => new
            {
                c.FechaCambio,
                c.TipoEntidad,
                c.IdEntidad,
                c.NombreAtributo,
                c.ValorAnterior,
                c.ValorNuevo,
                Usuario = c.UsuarioModifico?.Username ?? "Sistema",
                c.Descripcion
            }).ToList();

            DataGridViewSortHelper.Bind(dgvCambios, filas);
            lblTotalCambios.Text = cambios.Count.ToString();
        }

        private void ExportarReporte()
        {
            try
            {
                string reporte = BLL_ControlCambios.GenerarReporteCambios(cambiosActuales);

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Archivo de texto|*.txt";
                sfd.FileName = $"Auditoria_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, reporte);
                    MessageBox.Show("Reporte exportado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                dgvCambios.DataSource = null;

                // Limpiar columnas anteriores y crear nuevas
                dgvCambios.Columns.Clear();
                dgvCambios.AutoGenerateColumns = false;
                dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "ID", DataPropertyName = "ID", Width = 50 });
                dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 200 });
                dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Descripción", DataPropertyName = "Descripcion", Width = 400 });
                dgvCambios.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Fecha", DataPropertyName = "Fecha", Width = 150, DefaultCellStyle = new DataGridViewCellStyle() { Format = "dd/MM/yyyy HH:mm:ss" } });

                DataGridViewSortHelper.Bind(dgvCambios, snapshots);

                lblTotalCambios.Text = snapshots.Count.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar snapshots: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
