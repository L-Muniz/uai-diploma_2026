using BLL;
using BE;
using SERVICE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class Bitacora : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private readonly BLL_USUARIO user = new BLL_USUARIO();
        private readonly BLL_BITACORA bitacora = new BLL_BITACORA();
        private BE_USUARIO opcionTodos;
        private readonly BindingSource bindingSource = new BindingSource();

        private class BitacoraFila
        {
            public int ID { get; set; }
            public string Usuario { get; set; }
            public string TipoEvento { get; set; }
            public string Descripcion { get; set; }
            public DateTime Fecha { get; set; }
        }

        public Bitacora()
        {
            InitializeComponent();
            idiomaManager = SV_IdiomaManager.GetInstance();
            idiomaManager.RegistrarObservador(this);

            dataGridView1.DataSource = bindingSource;
            dataGridView1.AllowUserToOrderColumns = false;
            ActualizarTextos();
        }

        private void Bitacora_Load(object sender, EventArgs e)
        {
            CargarFiltrosPorDefecto();
            CargarTodos();
        }

        private void CargarFiltrosPorDefecto()
        {
            opcionTodos = new BE_USUARIO
            {
                ID = 0,
                Username = idiomaManager.Obtener("TODOS")
            };
            if (string.IsNullOrWhiteSpace(opcionTodos.Username) || opcionTodos.Username.StartsWith("["))
                opcionTodos.Username = "Todos";

            var usuarios = new List<BE_USUARIO> { opcionTodos };
            usuarios.AddRange(user.ListarUsuarios());

            comboBox1.DataSource = null;
            comboBox1.DisplayMember = "Username";
            comboBox1.ValueMember = "ID";
            comboBox1.DataSource = usuarios;
            comboBox1.SelectedIndex = 0;

            var tiposEventos = new List<BLL_BITACORA.TipoEvento> { BLL_BITACORA.TipoEvento.Todos };
            tiposEventos.AddRange(
                Enum.GetValues(typeof(BLL_BITACORA.TipoEvento))
                    .Cast<BLL_BITACORA.TipoEvento>()
                    .Where(t => t != BLL_BITACORA.TipoEvento.Todos));

            cbTipoEvento.DataSource = null;
            cbTipoEvento.DataSource = tiposEventos;
            cbTipoEvento.SelectedIndex = 0;

            dtFechaDesde.Value = DateTime.Now.AddDays(-30);
            dtFechaHasta.Value = DateTime.Now;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltrosCombinados();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            CargarFiltrosPorDefecto();
            CargarTodos();
        }

        private void AplicarFiltrosCombinados()
        {
            try
            {
                if (dtFechaHasta.Value < dtFechaDesde.Value)
                {
                    MessageBox.Show("La fecha hasta no puede ser menor que la fecha desde.", "Bitácora",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var criterios = new BLL_BITACORA.CriteriosBusqueda
                {
                    FechaDesde = dtFechaDesde.Value,
                    FechaHasta = dtFechaHasta.Value,
                    TipoEvento = cbTipoEvento.SelectedItem is BLL_BITACORA.TipoEvento tipo
                        ? tipo
                        : BLL_BITACORA.TipoEvento.Todos
                };

                if (comboBox1.SelectedItem is BE_USUARIO usuario && usuario.ID > 0)
                    criterios.IdUsuario = usuario.ID;

                MostrarEnGrilla(bitacora.FiltrarAvanzado(criterios));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar: {ex.Message}", "Bitácora",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CargarTodos()
        {
            MostrarEnGrilla(bitacora.Listar());
        }

        private void MostrarEnGrilla(List<BE_BITACORA> registros)
        {
            var filas = (registros ?? new List<BE_BITACORA>())
                .OrderByDescending(b => b.Fecha)
                .ThenByDescending(b => b.ID)
                .Select(b => new BitacoraFila
                {
                    ID = b.ID,
                    Usuario = b.Usuario?.Username ?? "",
                    TipoEvento = BLL_BITACORA.ObtenerNombreTipoEvento(b.Descripcion),
                    Descripcion = b.Descripcion,
                    Fecha = b.Fecha
                })
                .ToList();

            bindingSource.DataSource = new SortableBindingList<BitacoraFila>(filas);
            ConfigurarColumnas();
            DataGridViewSortHelper.HabilitarOrdenamiento(dataGridView1);
        }

        private void ConfigurarColumnas()
        {
            if (dataGridView1.Columns.Count == 0)
                return;

            if (dataGridView1.Columns["ID"] != null)
            {
                dataGridView1.Columns["ID"].HeaderText = "ID";
                dataGridView1.Columns["ID"].FillWeight = 10;
            }
            if (dataGridView1.Columns["Usuario"] != null)
            {
                dataGridView1.Columns["Usuario"].HeaderText = idiomaManager.Obtener("USUARIO");
                dataGridView1.Columns["Usuario"].FillWeight = 15;
            }
            if (dataGridView1.Columns["TipoEvento"] != null)
            {
                string headerTipo = idiomaManager.Obtener("TIPO_EVENTO");
                dataGridView1.Columns["TipoEvento"].HeaderText =
                    string.IsNullOrWhiteSpace(headerTipo) || headerTipo.StartsWith("[")
                        ? "Tipo Evento"
                        : headerTipo;
                dataGridView1.Columns["TipoEvento"].FillWeight = 18;
            }
            if (dataGridView1.Columns["Descripcion"] != null)
            {
                dataGridView1.Columns["Descripcion"].HeaderText = idiomaManager.Obtener("DESCRIPCION");
                dataGridView1.Columns["Descripcion"].FillWeight = 37;
            }
            if (dataGridView1.Columns["Fecha"] != null)
            {
                dataGridView1.Columns["Fecha"].HeaderText = idiomaManager.Obtener("FECHA");
                dataGridView1.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                dataGridView1.Columns["Fecha"].FillWeight = 20;
            }

            foreach (DataGridViewColumn col in dataGridView1.Columns)
                col.SortMode = DataGridViewColumnSortMode.Automatic;
        }

        private void Bitacora_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
            if (mainForm != null)
                mainForm.Show();
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
            if (opcionTodos != null)
            {
                string todos = idiomaManager.Obtener("TODOS");
                opcionTodos.Username = string.IsNullOrWhiteSpace(todos) || todos.StartsWith("[") ? "Todos" : todos;
                comboBox1.Refresh();
            }
            ConfigurarColumnas();
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("VER_BITACORA");
            if (label1 != null) label1.Text = idiomaManager.Obtener("USUARIO") + ":";
            if (lblFechaDesde != null) lblFechaDesde.Text = idiomaManager.Obtener("FECHA_DESDE");
            if (lblFechaHasta != null) lblFechaHasta.Text = idiomaManager.Obtener("FECHA_HASTA");
            if (lblTipoEvento != null) lblTipoEvento.Text = idiomaManager.Obtener("TIPO_EVENTO");
            if (btnBuscar != null) btnBuscar.Text = idiomaManager.Obtener("BUSCAR");
            if (string.IsNullOrWhiteSpace(btnBuscar?.Text) || btnBuscar.Text.StartsWith("["))
                btnBuscar.Text = "Buscar";
            if (btnLimpiar != null) btnLimpiar.Text = idiomaManager.Obtener("LIMPIAR");
        }
    }
}
