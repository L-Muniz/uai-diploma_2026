using BLL;
using BE;
using SERVICE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormIntegridad : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private TextBox txtResultado;
        private Button btnVerificar, btnRecalcular, btnCerrar;
        private Label lblTitulo;

        public FormIntegridad()
        {
            idiomaManager = SV_IdiomaManager.GetInstance();
            idiomaManager.RegistrarObservador(this);
            CrearControles();
            ActualizarTextos();
        }

        private void CrearControles()
        {
            this.Text = "Integridad de datos / Dígitos Verificadores";
            this.Size = new Size(750, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTitulo = new Label()
            {
                Location = new Point(12, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Text = "Verificación y restauración de DVH / DVV"
            };

            txtResultado = new TextBox()
            {
                Location = new Point(12, 45),
                Size = new Size(710, 380),
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                ReadOnly = true,
                Font = new Font("Consolas", 9)
            };

            btnVerificar = new Button() { Text = "Verificar", Location = new Point(12, 440), Width = 120 };
            btnVerificar.Click += (s, e) => EjecutarVerificacion();

            btnRecalcular = new Button() { Text = "Recalcular DV", Location = new Point(140, 440), Width = 140 };
            btnRecalcular.Click += (s, e) => EjecutarRecalculo();

            btnCerrar = new Button() { Text = "Cerrar", Location = new Point(600, 440), Width = 120 };
            btnCerrar.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblTitulo, txtResultado, btnVerificar, btnRecalcular, btnCerrar });
        }

        private void ActualizarTextos()
        {
            this.Text = "Integridad / DV";
            if (btnVerificar != null) btnVerificar.Text = "Verificar";
            if (btnRecalcular != null) btnRecalcular.Text = "Recalcular DV";
            if (btnCerrar != null) btnCerrar.Text = idiomaManager.Obtener("CERRAR");
        }

        private void EjecutarVerificacion()
        {
            try
            {
                var resultado = BLL_IntegridadDatos.VerificarTodasLasEntidades();
                txtResultado.Text = resultado.detalle;
                if (!resultado.esValido)
                    MessageBox.Show("Se detectaron inconsistencias de integridad.", "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Integridad OK.", "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ManejoErrores.MostrarError(ex);
            }
        }

        private void EjecutarRecalculo()
        {
            try
            {
                var confirm = MessageBox.Show(
                    "Esto recalculará todos los dígitos verificadores a partir del estado actual de la BD.\n" +
                    "Úselo solo si confirma que los datos actuales son correctos.\n\n¿Continuar?",
                    "Recalcular DV", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                var resultado = BLL_IntegridadDatos.RecalcularTodosLosDV();
                txtResultado.Text = resultado.detalle;
                Program.IntegridadFallo = false;
                Program.MensajeIntegridad = "";
                MessageBox.Show(resultado.mensaje, "Recálculo DV", MessageBoxButtons.OK,
                    resultado.exito ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                ManejoErrores.MostrarError(ex);
            }
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }
    }
}
