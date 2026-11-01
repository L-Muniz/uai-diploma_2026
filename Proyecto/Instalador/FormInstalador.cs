using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Instalador
{
    public partial class FormInstalador : Form
    {
        public FormInstalador()
        {
            InitializeComponent();
            txtDestino.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BecoGre");
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog { Description = "Elegí la carpeta de instalación", SelectedPath = txtDestino.Text })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    txtDestino.Text = dlg.SelectedPath;
            }
        }

        private async void btnInstalar_Click(object sender, EventArgs e)
        {
            btnInstalar.Enabled = false;
            btnExaminar.Enabled = false;
            txtLog.Clear();
            string destino = txtDestino.Text.Trim();
            try
            {
                // Corre en segundo plano para que la ventana siga respondiendo y el log avance en vivo.
                await Task.Run(() =>
                {
                    string servidor = InstaladorLogica.ElegirInstancia();
                    if (servidor == null)
                        throw new ErrorMotorNoInstalado("No se encontró ninguna instancia de SQL Server en esta computadora.\n\n" +
                                                        "Instalá SQL Server Express y volvé a ejecutar el instalador.");
                    InstaladorLogica.Instalar(servidor, destino, Informar);
                });
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
                btnExaminar.Enabled = true;
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
                Informar("Instalá SQL Server Express, volvé a abrir el instalador y ejecutalo de nuevo.");
            }
        }

        // Puede llamarse desde el hilo de la instalación: vuelve al hilo de la ventana antes de escribir.
        private void Informar(string mensaje)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.BeginInvoke(new Action(() => Informar(mensaje)));
                return;
            }
            txtLog.AppendText(mensaje + Environment.NewLine);
        }
    }
}
