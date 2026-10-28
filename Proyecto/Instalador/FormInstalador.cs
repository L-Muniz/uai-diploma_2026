using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Instalador
{
    public partial class FormInstalador : Form
    {
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
                Informar("No se encontró ninguna instancia de SQL Server. Podés instalar SQL Server Express " +
                         "o escribir el nombre de la instancia a mano.");
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
                InstaladorLogica.Instalar(cboServidor.Text.Trim(), txtDestino.Text.Trim(), Informar);
                MessageBox.Show(this,
                    "La instalación se completó correctamente.\n\nSe creó el acceso directo BecoGre en el escritorio.",
                    "Instalador BecoGre", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ErrorMotorNoInstalado ex)
            {
                Informar("ERROR: " + ex.Message);
                OfrecerDescargaSqlExpress(ex.Message);
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

        private void OfrecerDescargaSqlExpress(string mensaje)
        {
            DialogResult r = MessageBox.Show(this,
                mensaje + "\n\n¿Abrir la página oficial de descarga de SQL Server Express?",
                "Falta SQL Server", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                Process.Start(InstaladorLogica.UrlSqlExpress);
                Informar("Instalá SQL Server Express, volvé a abrir el instalador y presioná Buscar instancias.");
            }
        }

        private void Informar(string mensaje)
        {
            txtLog.AppendText(mensaje + Environment.NewLine);
            Application.DoEvents();
        }
    }
}
