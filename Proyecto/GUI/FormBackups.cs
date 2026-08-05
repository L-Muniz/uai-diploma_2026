using SERVICE;
using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormBackups : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private Label lblStatus;
        private ProgressBar progressBar;

        public FormBackups()
        {
            InitializeComponent();

            idiomaManager = SV_IdiomaManager.GetInstance();

            CrearColumnasDataGrid();
            CrearElementosAdicionales();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            CargarBackups();
        }

        private void CrearElementosAdicionales()
        {
            lblStatus = new Label() { Location = new Point(10, 510), AutoSize = true, ForeColor = System.Drawing.Color.Green };
            progressBar = new ProgressBar() { Location = new Point(10, 530), Size = new Size(770, 20), Visible = false };

            this.Controls.Add(lblStatus);
            this.Controls.Add(progressBar);
        }

        private void CrearColumnasDataGrid()
        {
            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Archivo", DataPropertyName = "Nombre", Width = 350 });
            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Fecha", DataPropertyName = "Fecha", Width = 180 });
            dgvBackups.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Tamaño (MB)", DataPropertyName = "TamanoMB", Width = 100 });
            dgvBackups.AutoGenerateColumns = false;
            DataGridViewSortHelper.HabilitarOrdenamiento(dgvBackups);
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("BACKUPS");
            if (btnBackupAhora != null) btnBackupAhora.Text = idiomaManager.Obtener("REALIZAR_BACKUP");
            if (btnRestaurar != null) btnRestaurar.Text = idiomaManager.Obtener("RESTAURAR");
            if (btnEliminar != null) btnEliminar.Text = idiomaManager.Obtener("ELIMINAR");
            if (btnRefrescar != null) btnRefrescar.Text = idiomaManager.Obtener("REFRESCAR");
            if (lblInfo != null) lblInfo.Text = idiomaManager.Obtener("INFO_BACKUPS");
            if (lblTotal != null) lblTotal.Text = idiomaManager.Obtener("TOTAL_BACKUPS");
            if (lblUltimo != null) lblUltimo.Text = idiomaManager.Obtener("ULTIMO_BACKUP");
            if (lblUbicacionLabel != null) lblUbicacionLabel.Text = idiomaManager.Obtener("UBICACION");

            if (dgvBackups.Columns.Count >= 3)
            {
                dgvBackups.Columns[0].HeaderText = idiomaManager.Obtener("NOMBRE");
                dgvBackups.Columns[1].HeaderText = idiomaManager.Obtener("FECHA");
            }
        }

        private void btnBackupAhora_Click(object sender, EventArgs e)
        {
            RealizarBackupAhora();
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            RestaurarBackup();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EliminarBackup();
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarBackups();
        }

        private void CargarBackups()
        {
            try
            {
                var backups = BLL_BackupRestore.ListarBackups();
                var infoBackups = BLL_BackupRestore.ObtenerInfoBackups();

                lblTotalBackups.Text = infoBackups["TotalBackups"].ToString();

                if (infoBackups.ContainsKey("FechaUltimoBackup"))
                    lblUltimoBackup.Text = ((DateTime)infoBackups["FechaUltimoBackup"]).ToString("dd/MM/yyyy HH:mm:ss");

                txtUbicacion.Text = infoBackups["RutaBackups"].ToString();

                dgvBackups.DataSource = null;
                var filas = backups.Select(b =>
                {
                    var fileInfo = new FileInfo(b);
                    return new
                    {
                        Nombre = Path.GetFileName(b),
                        Ruta = b,
                        Fecha = fileInfo.CreationTime.ToString("dd/MM/yyyy HH:mm:ss"),
                        TamanoMB = Math.Round(fileInfo.Length / (1024.0 * 1024.0), 2)
                    };
                }).ToList();

                DataGridViewSortHelper.Bind(dgvBackups, filas);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar backups: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RealizarBackupAhora()
        {
            try
            {
                btnBackupAhora.Enabled = false;
                lblStatus.Text = "Generando backup...";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                this.Refresh();

                var resultado = BLL_BackupRestore.RealizarBackup();

                progressBar.Visible = false;
                btnBackupAhora.Enabled = true;

                if (resultado.exito)
                {
                    lblStatus.Text = $"✓ {resultado.mensaje}";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    MessageBox.Show(resultado.mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarBackups();
                }
                else
                {
                    lblStatus.Text = $"✗ Error: {resultado.mensaje}";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show($"Error en backup: {resultado.mensaje}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                progressBar.Visible = false;
                btnBackupAhora.Enabled = true;
                lblStatus.Text = $"✗ Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestaurarBackup()
        {
            if (dgvBackups.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un backup para restaurar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string rutaBackup = ObtenerRutaBackup(dgvBackups.SelectedRows[0].DataBoundItem);
            if (string.IsNullOrWhiteSpace(rutaBackup))
            {
                MessageBox.Show("No se pudo obtener la ruta del backup seleccionado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string nombreArchivo = Path.GetFileName(rutaBackup);

            if (MessageBox.Show($"⚠️ ADVERTENCIA: Se restaurarán todos los datos a la fecha de este backup.\n\nArchivo: {nombreArchivo}\n\n¿Estás seguro?",
                "Confirmación de Restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            try
            {
                btnRestaurar.Enabled = false;
                lblStatus.Text = "Restaurando base de datos...";
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                progressBar.Visible = true;
                progressBar.Style = ProgressBarStyle.Marquee;
                this.Refresh();

                var resultado = BLL_BackupRestore.RestaurarDesdeBackup(rutaBackup);

                progressBar.Visible = false;

                if (resultado.exito)
                {
                    lblStatus.Text = "✓ Base de datos restaurada";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    MessageBox.Show("✓ Base de datos restaurada exitosamente.\n\nDebe reiniciar la aplicación.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                else
                {
                    btnRestaurar.Enabled = true;
                    lblStatus.Text = $"✗ Error: {resultado.mensaje}";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show($"Error en restore: {resultado.mensaje}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                btnRestaurar.Enabled = true;
                progressBar.Visible = false;
                lblStatus.Text = $"✗ Error: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EliminarBackup()
        {
            var rutas = new List<string>();
            foreach (DataGridViewRow row in dgvBackups.SelectedRows)
            {
                string ruta = ObtenerRutaBackup(row.DataBoundItem);
                if (!string.IsNullOrWhiteSpace(ruta) && !rutas.Contains(ruta))
                    rutas.Add(ruta);
            }

            if (rutas.Count == 0)
            {
                MessageBox.Show("Selecciona al menos un backup para eliminar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mensaje = rutas.Count == 1
                ? $"¿Eliminar este backup?\n{Path.GetFileName(rutas[0])}"
                : $"¿Eliminar los {rutas.Count} backups seleccionados?";

            if (MessageBox.Show(mensaje, "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int eliminados = 0;
            var errores = new List<string>();
            foreach (string ruta in rutas)
            {
                try
                {
                    File.Delete(ruta);
                    eliminados++;
                }
                catch (Exception ex)
                {
                    errores.Add($"{Path.GetFileName(ruta)}: {ex.Message}");
                }
            }

            CargarBackups();

            if (errores.Count == 0)
            {
                MessageBox.Show(
                    eliminados == 1 ? "Backup eliminado" : $"{eliminados} backups eliminados",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show(
                $"Eliminados: {eliminados} de {rutas.Count}\n\nErrores:\n{string.Join("\n", errores)}",
                "Eliminar backups", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static string ObtenerRutaBackup(object item)
        {
            if (item == null)
                return null;

            var propiedad = item.GetType().GetProperty("Ruta");
            return propiedad?.GetValue(item)?.ToString();
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }
    }
}
