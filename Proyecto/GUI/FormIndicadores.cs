using System;
using System.Data;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU04: la gerencia consulta los indicadores del período para decidir sobre la red y las tarifas.
    public partial class FormIndicadores : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_GESTION bllGestion = new BLL_GESTION();
        private readonly BLL_REPORTES bllReportes = new BLL_REPORTES();

        public FormIndicadores(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            txtPeriodo.Text = DateTime.Now.ToString("yyyy-MM");
        }

        private void btnExportarPdf_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog { Filter = "PDF (*.pdf)|*.pdf", FileName = $"Indicadores_{txtPeriodo.Text.Trim()}.pdf" })
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                    bllReportes.ExportarIndicadoresPdf(usuario, txtPeriodo.Text.Trim(), dlg.FileName);
                    UIHelper.Informar("Informe PDF generado.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnExportarXml_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog { Filter = "Informe XML (*.xml)|*.xml", FileName = $"Indicadores_{txtPeriodo.Text.Trim()}.xml" })
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                    bllReportes.ExportarIndicadoresXml(usuario, txtPeriodo.Text.Trim(), dlg.FileName);
                    UIHelper.Informar("Informe guardado en XML.");
                }
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnImportarXml_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dlg = new OpenFileDialog { Filter = "Informe XML (*.xml)|*.xml" })
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                    BE_InformeIndicadores informe = bllReportes.ImportarIndicadoresXml(dlg.FileName);
                    var tabla = new DataTable();
                    tabla.Columns.Add("Indicador");
                    tabla.Columns.Add("Valor");
                    foreach (BE_IndicadorItem item in informe.Indicadores)
                        tabla.Rows.Add(item.Nombre, item.Valor);
                    txtPeriodo.Text = informe.Periodo;
                    UIHelper.CargarGrilla(dgvIndicadores, tabla);
                    UIHelper.Informar($"Informe del período {informe.Periodo} generado el {informe.FechaGeneracion:dd/MM/yyyy HH:mm} (archivo cargado, no recalculado).");
                }
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            try
            {
                UIHelper.CargarGrilla(dgvIndicadores, bllGestion.ConsultarIndicadores(usuario, txtPeriodo.Text.Trim()));
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
