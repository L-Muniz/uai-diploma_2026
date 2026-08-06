using SERVICE;
using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormAgregarIdioma : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_Idioma bllIdioma;
        private Dictionary<string, string> traduccionesIngresadas;
        private Label lblCodigo, lblNombre, lblTraduccion, lblInstruccion;
        private Button btnCargarClaves, btnGuardar, btnCancelar;
        private DataGridView dgv;
        public bool IdiomaAgregado { get; set; } = false;

        public FormAgregarIdioma()
        {
            idiomaManager = SV_IdiomaManager.GetInstance();
            bllIdioma = new BLL_Idioma();
            traduccionesIngresadas = new Dictionary<string, string>();

            CrearControles();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
        }

        private class TraduccionFila
        {
            public string Clave { get; set; }
            public string Traduccion { get; set; }
        }

        private void CrearControles()
        {
            this.Size = new Size(800, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblInstruccion = new Label() { Location = new Point(10, 10), AutoSize = true, Text = "Instrucciones" };

            lblCodigo = new Label() { Location = new Point(10, 45), AutoSize = true, Text = "Código (ej: FR):" };
            TextBox txtCodigo = new TextBox() { Name = "txtCodigo", Location = new Point(150, 45), Width = 80 };

            lblNombre = new Label() { Location = new Point(10, 75), AutoSize = true, Text = "Nombre:" };
            TextBox txtNombre = new TextBox() { Name = "txtNombre", Location = new Point(150, 75), Width = 150 };

            btnCargarClaves = new Button() { Location = new Point(10, 105), Width = 230, Height = 30, Text = "CARGAR CLAVES" };
            btnCargarClaves.Click += (s, e) => CargarClavesPendientes(dgv, txtCodigo.Text);

            lblTraduccion = new Label() { Location = new Point(10, 145), AutoSize = true };
            dgv = new DataGridView()
            {
                Name = "dgvTraduccion",
                Location = new Point(10, 170),
                Size = new Size(770, 340),
                AllowUserToAddRows = false,
                AutoGenerateColumns = false
            };
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Clave", DataPropertyName = "Clave", Width = 300, ReadOnly = true });
            dgv.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Traducción", DataPropertyName = "Traduccion", Width = 450 });
            DataGridViewSortHelper.HabilitarOrdenamiento(dgv);

            btnGuardar = new Button() { Location = new Point(10, 520), Width = 200, Height = 35, Text = "GUARDAR IDIOMA" };
            btnGuardar.Click += (s, e) => GuardarIdioma(txtCodigo.Text, txtNombre.Text, dgv);

            btnCancelar = new Button() { Location = new Point(620, 520), Width = 160, Height = 35, Text = "Cancelar" };
            btnCancelar.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] { lblInstruccion, lblCodigo, txtCodigo, lblNombre, txtNombre, btnCargarClaves, lblTraduccion, dgv, btnGuardar, btnCancelar });
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("AGREGAR_IDIOMA");
            if (lblInstruccion != null) lblInstruccion.Text = idiomaManager.Obtener("INSTRUCCIONES_AGREGAR_IDIOMA");
            if (lblCodigo != null) lblCodigo.Text = idiomaManager.Obtener("CODIGO_IDIOMA");
            if (lblNombre != null) lblNombre.Text = idiomaManager.Obtener("NOMBRE_IDIOMA");
            if (lblTraduccion != null) lblTraduccion.Text = idiomaManager.Obtener("INSTRUCCIONES_TRADUCIR");
            if (btnCargarClaves != null) btnCargarClaves.Text = idiomaManager.Obtener("CARGAR_CLAVES");
            if (btnGuardar != null) btnGuardar.Text = idiomaManager.Obtener("GUARDAR_IDIOMA");
            if (btnCancelar != null) btnCancelar.Text = idiomaManager.Obtener("CANCELAR");
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }

        private void CargarClavesPendientes(DataGridView dgv, string codigoIdioma)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigoIdioma))
                {
                    MessageBox.Show("Ingresa un código de idioma", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var todasLasClaves = ObtenerTodasLasClaves();
                var filas = todasLasClaves
                    .Select(clave => new TraduccionFila { Clave = clave, Traduccion = "" })
                    .ToList();

                DataGridViewSortHelper.Bind(dgv, filas);

                MessageBox.Show($"Se cargaron {todasLasClaves.Count} claves pendientes de traducir", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar claves: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<string> ObtenerTodasLasClaves()
        {
            return bllIdioma.ObtenerTodasLasClaves();
        }

        private void GuardarIdioma(string codigo, string nombre, DataGridView dgv)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(nombre))
                {
                    MessageBox.Show("Completa el código y nombre del idioma", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (codigo.Length != 2)
                {
                    MessageBox.Show("El código debe tener exactamente 2 caracteres (ej: ES, EN, FR)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnGuardar.Enabled = false;
                btnGuardar.Text = "Guardando...";

                if (bllIdioma.AgregarIdioma(codigo, nombre))
                {
                    int contadorTraduccionesGuardadas = 0;
                    int contadorTraduccionesFallidas = 0;
                    int contadorSinTraduccion = 0;
                    List<string> fallos = new List<string>();

                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        var fila = row.DataBoundItem as TraduccionFila;
                        string clave = fila?.Clave ?? row.Cells[0].Value?.ToString() ?? "";
                        string traduccion = fila?.Traduccion ?? row.Cells[1].Value?.ToString() ?? "";

                        if (string.IsNullOrWhiteSpace(clave))
                            continue;

                        if (string.IsNullOrWhiteSpace(traduccion))
                        {
                            traduccion = $"[{clave}]";
                            contadorSinTraduccion++;
                        }

                        try
                        {
                            if (bllIdioma.AgregarTraduccion(clave, codigo, traduccion))
                            {
                                contadorTraduccionesGuardadas++;
                            }
                            else
                            {
                                contadorTraduccionesFallidas++;
                                fallos.Add($"- {clave}");
                            }
                        }
                        catch (Exception ex)
                        {
                            contadorTraduccionesFallidas++;
                            fallos.Add($"- {clave}: {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"Error guardando traducción {clave}: {ex.Message}");
                        }
                    }

                    string mensaje = $"Idioma '{nombre}' guardado exitosamente.\nTraduciones guardadas: {contadorTraduccionesGuardadas}/{dgv.Rows.Count}";

                    if (contadorSinTraduccion > 0)
                        mensaje += $"\n⚠️ Claves sin traducción: {contadorSinTraduccion}";

                    if (contadorTraduccionesFallidas > 0)
                        mensaje += $"\n❌ Claves con error: {contadorTraduccionesFallidas}\n{string.Join("\n", fallos.Take(5))}";

                    IdiomaAgregado = true;
                    MessageBox.Show(mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    var txtCodigo = this.Controls.Cast<Control>().FirstOrDefault(c => c.Name == "txtCodigo") as TextBox;
                    var txtNombre = this.Controls.Cast<Control>().FirstOrDefault(c => c.Name == "txtNombre") as TextBox;
                    if (txtCodigo != null) txtCodigo.Clear();
                    if (txtNombre != null) txtNombre.Clear();
                    DataGridViewSortHelper.Bind(dgv, new List<TraduccionFila>());
                }
                else
                {
                    MessageBox.Show("Error al guardar el idioma. Verifica que:\n- El código no exista\n- La base de datos esté disponible", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnGuardar.Enabled = true;
                    btnGuardar.Text = idiomaManager.Obtener("GUARDAR_IDIOMA");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = true;
                btnGuardar.Text = idiomaManager.Obtener("GUARDAR_IDIOMA");
                System.Diagnostics.Debug.WriteLine($"Error en GuardarIdioma: {ex}");
            }
        }
    }
}
