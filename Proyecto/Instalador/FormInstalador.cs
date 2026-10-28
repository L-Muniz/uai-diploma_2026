using System;
using System.IO;
using System.Windows.Forms;

namespace Instalador
{
    public partial class FormInstalador : Form
    {
        private readonly string carpetaAplicacion = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Aplicacion");
        private readonly string carpetaScripts = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts");

        public FormInstalador()
        {
            InitializeComponent();
            txtDestino.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BecoGre");
            BuscarInstancias();
        }

        private void BuscarInstancias()
        {
            cboServidor.Items.Clear();
            try
            {
                cboServidor.Items.AddRange(InstaladorLogica.ObtenerInstancias());
            }
            catch (Exception ex)
            {
                Informar("No se pudieron listar las instancias: " + ex.Message);
            }

            if (cboServidor.Items.Count == 0)
            {
                Informar("No se encontró ninguna instancia de SQL Server. Instalá SQL Server Express " +
                         "(https://www.microsoft.com/sql-server/sql-server-downloads) o escribí el nombre de la instancia a mano.");
            }
            else
            {
                cboServidor.SelectedIndex = 0;
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            BuscarInstancias();
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog { Description = "Elegí la carpeta de instalación", SelectedPath = txtDestino.Text })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    txtDestino.Text = dlg.SelectedPath;
            }
        }

        private void btnInstalar_Click(object sender, EventArgs e)
        {
            btnInstalar.Enabled = false;
            txtLog.Clear();
            try
            {
                InstaladorLogica.Instalar(cboServidor.Text.Trim(), txtBase.Text.Trim(), txtDestino.Text.Trim(),
                                          carpetaAplicacion, carpetaScripts, Informar);
                MessageBox.Show(this, "La instalación se completó correctamente.", "Instalador BecoGre",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ErrorInstalacion ex)
            {
                Informar("ERROR: " + ex.Message);
                MessageBox.Show(this, ex.Message, "No se pudo instalar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                Informar("ERROR inesperado: " + ex.Message);
                MessageBox.Show(this, ex.Message, "Error inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnInstalar.Enabled = true;
            }
        }

        private void Informar(string mensaje)
        {
            txtLog.AppendText(mensaje + Environment.NewLine);
            Application.DoEvents();
        }
    }
}
