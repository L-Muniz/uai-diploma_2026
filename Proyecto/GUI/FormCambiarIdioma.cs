using SERVICE;
using System;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormCambiarIdioma : Form
    {
        private ComboBox cbIdiomas;

        public FormCambiarIdioma()
        {
            this.Text = "Cambiar Idioma";
            this.Size = new System.Drawing.Size(300, 150);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblIdioma = new Label() { Text = "Selecciona un idioma:", Location = new System.Drawing.Point(10, 10), AutoSize = true };
            cbIdiomas = new ComboBox() { Name = "cbIdiomas", Location = new System.Drawing.Point(10, 35), Width = 270, DropDownStyle = ComboBoxStyle.DropDownList };
            Button btnAceptar = new Button() { Text = "Aceptar", Location = new System.Drawing.Point(100, 70), Width = 80 };
            btnAceptar.Click += (s, e) => btnAceptar_Click(s, e);
            Button btnCancelar = new Button() { Text = "Cancelar", Location = new System.Drawing.Point(190, 70), Width = 80 };
            btnCancelar.Click += (s, e) => btnCancelar_Click(s, e);

            this.Controls.AddRange(new Control[] { lblIdioma, cbIdiomas, btnAceptar, btnCancelar });

            CargarIdiomas();
        }

        private void CargarIdiomas()
        {
            var idiomaManager = SV_IdiomaManager.GetInstance();
            var idiomas = idiomaManager.ObtenerNombresIdiomas()
                .Select(kv => new { Codigo = kv.Key, Nombre = kv.Value })
                .OrderBy(i => i.Nombre)
                .ToList();

            cbIdiomas.DisplayMember = "Nombre";
            cbIdiomas.ValueMember = "Codigo";
            cbIdiomas.DataSource = idiomas;
            cbIdiomas.SelectedValue = idiomaManager.ObtenerIdiomaActual();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var idiomaManager = SV_IdiomaManager.GetInstance();
            string idiomaSeleccionado = cbIdiomas.SelectedValue?.ToString();

            if (!string.IsNullOrWhiteSpace(idiomaSeleccionado)
                && !string.Equals(idiomaSeleccionado, idiomaManager.ObtenerIdiomaActual(), StringComparison.OrdinalIgnoreCase))
            {
                idiomaManager.CambiarIdioma(idiomaSeleccionado);
                MessageBox.Show("Idioma cambiado exitosamente. Los cambios se aplicarán de inmediato.");
            }

            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
