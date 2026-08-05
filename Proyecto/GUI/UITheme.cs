using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    /// <summary>
    /// Tema visual de oficina suave (azules/grises fríos) para WinForms.
    /// </summary>
    public static class UITheme
    {
        public static readonly Color FondoApp = Color.FromArgb(244, 246, 249);
        public static readonly Color FondoPanel = Color.FromArgb(228, 234, 241);
        public static readonly Color FondoTarjeta = Color.FromArgb(255, 255, 255);
        public static readonly Color Acento = Color.FromArgb(91, 124, 153);
        public static readonly Color AcentoOscuro = Color.FromArgb(70, 100, 128);
        public static readonly Color AcentoClaro = Color.FromArgb(210, 222, 234);
        public static readonly Color Texto = Color.FromArgb(51, 65, 85);
        public static readonly Color TextoSuave = Color.FromArgb(100, 116, 139);
        public static readonly Color Borde = Color.FromArgb(197, 208, 220);
        public static readonly Color GridHeader = Color.FromArgb(213, 222, 232);
        public static readonly Color GridAlt = Color.FromArgb(248, 250, 252);
        public static readonly Color GridLinea = Color.FromArgb(226, 232, 240);
        public static readonly Color Seleccion = Color.FromArgb(186, 205, 222);
        public static readonly Color MenuFondo = Color.FromArgb(91, 124, 153);
        public static readonly Color MenuTexto = Color.White;
        public static readonly Color BotonDeshabilitadoFondo = Color.FromArgb(232, 236, 241);
        public static readonly Color BotonDeshabilitadoTexto = Color.FromArgb(148, 163, 184);
        public static readonly Color BotonDeshabilitadoBorde = Color.FromArgb(210, 218, 228);

        public static readonly Font FuenteNormal = new Font("Segoe UI", 9f, FontStyle.Regular);
        public static readonly Font FuenteTitulo = new Font("Segoe UI", 11f, FontStyle.Bold);
        public static readonly Font FuenteMenu = new Font("Segoe UI", 9f, FontStyle.Regular);

        public static readonly Color AcentoRoles = Color.FromArgb(70, 100, 128);
        public static readonly Color FondoRoles = Color.FromArgb(232, 239, 246);
        public static readonly Color AcentoPerfiles = Color.FromArgb(46, 125, 122);
        public static readonly Color FondoPerfiles = Color.FromArgb(230, 244, 243);

        public static void Aplicar(Form form)
        {
            if (form == null)
                return;

            form.Font = FuenteNormal;
            form.ForeColor = Texto;
            form.BackColor = FondoApp;

            AplicarControles(form.Controls);
            EstilizarMdiClient(form);

            if (form.MainMenuStrip != null)
                EstilizarMenu(form.MainMenuStrip);
        }

        public static void DiferenciarModulo(
            Panel panelBotones,
            Panel panelIzquierdo,
            string titulo,
            string subtitulo,
            Color acento,
            Color fondoSuave,
            params Button[] botonesAccion)
        {
            if (panelBotones == null || panelIzquierdo == null)
                return;

            panelBotones.Height = Math.Max(panelBotones.Height, 58);
            panelIzquierdo.BackColor = fondoSuave;

            Panel acentoBarra = null;
            foreach (Control control in panelIzquierdo.Controls)
            {
                if (control is Panel panel && panel.Name == "panelAcentoModulo")
                {
                    acentoBarra = panel;
                    break;
                }
            }

            if (acentoBarra == null)
            {
                acentoBarra = new Panel
                {
                    Name = "panelAcentoModulo",
                    Dock = DockStyle.Left,
                    Width = 8,
                    BackColor = acento
                };
                panelIzquierdo.Controls.Add(acentoBarra);
            }

            acentoBarra.BackColor = acento;
            acentoBarra.BringToFront();

            Label lblTitulo = ObtenerOCrearLabel(panelBotones, "lblModuloTitulo");
            Label lblAyuda = ObtenerOCrearLabel(panelBotones, "lblModuloAyuda");
            lblTitulo.Text = titulo ?? "";
            lblTitulo.ForeColor = acento;
            lblTitulo.Font = new Font(FuenteTitulo.FontFamily, 12f, FontStyle.Bold);
            lblAyuda.Text = subtitulo ?? "";
            lblAyuda.ForeColor = TextoSuave;
            lblAyuda.Font = FuenteNormal;

            void Ubicar()
            {
                lblTitulo.Size = new Size(430, 24);
                lblAyuda.Size = new Size(430, 18);
                lblTitulo.Location = new Point(Math.Max(12, panelBotones.ClientSize.Width - lblTitulo.Width - 14), 8);
                lblAyuda.Location = new Point(Math.Max(12, panelBotones.ClientSize.Width - lblAyuda.Width - 14), 32);

                int x = 12;
                int yCentro = Math.Max(8, (panelBotones.Height - 32) / 2);
                foreach (Button boton in botonesAccion)
                {
                    if (boton == null)
                        continue;

                    int textoAncho = TextRenderer.MeasureText(boton.Text ?? " ", boton.Font).Width + 24;
                    if (boton.Width < textoAncho)
                        boton.Width = textoAncho;

                    boton.Location = new Point(x, yCentro);
                    x += boton.Width + 8;
                }
            }

            Ubicar();
            panelBotones.Resize -= PanelModulo_Resize;
            panelBotones.Tag = new Action(Ubicar);
            panelBotones.Resize += PanelModulo_Resize;
        }

        private static void PanelModulo_Resize(object sender, EventArgs e)
        {
            if (sender is Panel panel && panel.Tag is Action ubicar)
                ubicar();
        }

        private static Label ObtenerOCrearLabel(Panel panel, string nombre)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is Label label && label.Name == nombre)
                    return label;
            }

            var creado = new Label
            {
                Name = nombre,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.Transparent
            };
            panel.Controls.Add(creado);
            creado.BringToFront();
            return creado;
        }

        public static void AplicarLogin(Form form)
        {
            Aplicar(form);
            form.BackColor = FondoPanel;

            foreach (Control c in form.Controls)
            {
                if (c is GroupBox grp)
                {
                    grp.BackColor = FondoTarjeta;
                    grp.ForeColor = Texto;
                    grp.Font = FuenteTitulo;
                    EstilizarControlesHijos(grp);
                }
            }
        }

        private static void AplicarControles(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                EstilizarControl(control);

                if (control.HasChildren)
                    AplicarControles(control.Controls);
            }
        }

        private static void EstilizarControlesHijos(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                EstilizarControl(control);
                if (control.HasChildren)
                    EstilizarControlesHijos(control);
            }
        }

        private static void EstilizarControl(Control control)
        {
            if (control == null)
                return;

            if (!(control is DataGridView) && !(control is MenuStrip) && !(control is ToolStrip))
                control.Font = FuenteNormal;

            if (control is Panel panel)
            {
                if (panel.Dock == DockStyle.Top || string.Equals(panel.Name, "panelFiltros", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(panel.Name, "panelIzquierdo", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(panel.Name, "panelDerecho", StringComparison.OrdinalIgnoreCase))
                {
                    panel.BackColor = FondoPanel;
                    panel.ForeColor = Texto;
                }
                else
                {
                    panel.BackColor = FondoApp;
                }
            }
            else if (control is GroupBox groupBox)
            {
                groupBox.ForeColor = Texto;
                groupBox.BackColor = FondoTarjeta;
            }
            else if (control is Label label)
            {
                label.ForeColor = Texto;
                if (label.Font.Bold)
                    label.Font = new Font(FuenteNormal, FontStyle.Bold);
            }
            else if (control is Button button)
            {
                EstilizarBoton(button);
            }
            else if (control is TextBox textBox)
            {
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.BackColor = FondoTarjeta;
                textBox.ForeColor = Texto;
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.FlatStyle = FlatStyle.Flat;
                comboBox.BackColor = FondoTarjeta;
                comboBox.ForeColor = Texto;
            }
            else if (control is DateTimePicker dtp)
            {
                dtp.CalendarForeColor = Texto;
                dtp.CalendarMonthBackground = FondoTarjeta;
                dtp.CalendarTitleBackColor = Acento;
                dtp.CalendarTitleForeColor = MenuTexto;
                dtp.CalendarTrailingForeColor = TextoSuave;
            }
            else if (control is DataGridView grid)
            {
                EstilizarGrid(grid);
            }
            else if (control is TreeView tree)
            {
                tree.BackColor = FondoTarjeta;
                tree.ForeColor = Texto;
                tree.BorderStyle = BorderStyle.FixedSingle;
                tree.Font = FuenteNormal;
            }
            else if (control is ListBox listBox)
            {
                listBox.BackColor = FondoTarjeta;
                listBox.ForeColor = Texto;
                listBox.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is MenuStrip menu)
            {
                EstilizarMenu(menu);
            }
            else if (control is StatusStrip status)
            {
                status.BackColor = FondoPanel;
                status.ForeColor = Texto;
            }
            else if (control is ProgressBar)
            {
                // deja el estilo del sistema
            }
            else if (control is LinkLabel link)
            {
                link.LinkColor = AcentoOscuro;
                link.ActiveLinkColor = Acento;
                link.VisitedLinkColor = AcentoOscuro;
            }
        }

        private static void EstilizarBoton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.Font = FuenteNormal;
            button.UseCompatibleTextRendering = false;
            button.UseVisualStyleBackColor = false;
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.Padding = new Padding(4, 2, 4, 2);

            // Evita texto cortado con Segoe UI en botones de altura chica.
            const int minAlto = 32;
            if (button.Height < minAlto)
                button.Height = minAlto;

            int textoAncho = TextRenderer.MeasureText(button.Text ?? " ", button.Font).Width + 20;
            if (button.Width > 0 && button.Width < textoAncho)
                button.Width = textoAncho;

            AplicarColoresBoton(button);

            button.EnabledChanged -= Boton_EnabledChanged;
            button.EnabledChanged += Boton_EnabledChanged;
        }

        private static void Boton_EnabledChanged(object sender, EventArgs e)
        {
            if (sender is Button button)
                AplicarColoresBoton(button);
        }

        private static void AplicarColoresBoton(Button button)
        {
            if (!button.Enabled)
            {
                button.BackColor = BotonDeshabilitadoFondo;
                button.ForeColor = BotonDeshabilitadoTexto;
                button.FlatAppearance.BorderColor = BotonDeshabilitadoBorde;
                button.FlatAppearance.MouseOverBackColor = BotonDeshabilitadoFondo;
                button.FlatAppearance.MouseDownBackColor = BotonDeshabilitadoFondo;
                button.Cursor = Cursors.Default;
                return;
            }

            button.Cursor = Cursors.Hand;

            if (EsBotonPrimario(button))
            {
                button.BackColor = Acento;
                button.ForeColor = MenuTexto;
                button.FlatAppearance.BorderColor = AcentoOscuro;
                button.FlatAppearance.MouseOverBackColor = AcentoOscuro;
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(55, 82, 108);
            }
            else
            {
                button.BackColor = AcentoClaro;
                button.ForeColor = Texto;
                button.FlatAppearance.BorderColor = Borde;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(198, 210, 222);
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(180, 195, 210);
            }
        }

        private static bool EsBotonPrimario(Button button)
        {
            string name = (button.Name ?? "").ToLowerInvariant();
            string text = (button.Text ?? "").ToLowerInvariant();

            string[] primarios =
            {
                "buscar", "filtrar", "guardar", "ingresar", "aceptar", "crear", "nuevo",
                "realizar", "backup", "restaurar", "recalcular", "verificar", "asignar"
            };

            string[] secundarios =
            {
                "cancelar", "cerrar", "limpiar", "eliminar", "quitar", "deshacer", "exportar"
            };

            foreach (string s in secundarios)
            {
                if (name.Contains(s) || text.Contains(s))
                    return false;
            }

            foreach (string p in primarios)
            {
                if (name.Contains(p) || text.Contains(p))
                    return true;
            }

            return false;
        }

        public static void EstilizarGrid(DataGridView grid)
        {
            if (grid == null)
                return;

            grid.BackgroundColor = FondoTarjeta;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = GridLinea;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = GridHeader;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Texto;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font(FuenteNormal, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = GridHeader;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Texto;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersHeight = 28;
            grid.RowHeadersVisible = false;
            grid.DefaultCellStyle.BackColor = FondoTarjeta;
            grid.DefaultCellStyle.ForeColor = Texto;
            grid.DefaultCellStyle.SelectionBackColor = Seleccion;
            grid.DefaultCellStyle.SelectionForeColor = Texto;
            grid.DefaultCellStyle.Font = FuenteNormal;
            grid.AlternatingRowsDefaultCellStyle.BackColor = GridAlt;
            grid.AlternatingRowsDefaultCellStyle.ForeColor = Texto;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Seleccion;
            grid.AlternatingRowsDefaultCellStyle.SelectionForeColor = Texto;
            grid.RowTemplate.Height = 24;
        }

        public static void EstilizarMenu(MenuStrip menu)
        {
            if (menu == null)
                return;

            menu.BackColor = MenuFondo;
            menu.ForeColor = MenuTexto;
            menu.Font = FuenteMenu;
            menu.Renderer = new OficinaMenuRenderer();

            foreach (ToolStripItem item in menu.Items)
                EstilizarMenuItem(item);
        }

        private static void EstilizarMenuItem(ToolStripItem item)
        {
            item.ForeColor = MenuTexto;
            item.Font = FuenteMenu;

            if (item is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem sub in menuItem.DropDownItems)
                    EstilizarMenuItemDropDown(sub);
            }
        }

        private static void EstilizarMenuItemDropDown(ToolStripItem item)
        {
            item.Font = FuenteMenu;
            if (item is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem sub in menuItem.DropDownItems)
                    EstilizarMenuItemDropDown(sub);
            }
        }

        private static void EstilizarMdiClient(Form form)
        {
            foreach (Control control in form.Controls)
            {
                if (control is MdiClient mdi)
                {
                    mdi.BackColor = FondoApp;
                }
            }
        }

        private sealed class OficinaMenuRenderer : ToolStripProfessionalRenderer
        {
            public OficinaMenuRenderer() : base(new OficinaColorTable())
            {
            }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                if (e.Item.OwnerItem == null && e.ToolStrip is MenuStrip)
                    e.TextColor = MenuTexto;
                else
                    e.TextColor = Texto;

                base.OnRenderItemText(e);
            }
        }

        private sealed class OficinaColorTable : ProfessionalColorTable
        {
            public override Color MenuStripGradientBegin => MenuFondo;
            public override Color MenuStripGradientEnd => MenuFondo;
            public override Color MenuItemSelected => AcentoClaro;
            public override Color MenuItemSelectedGradientBegin => AcentoClaro;
            public override Color MenuItemSelectedGradientEnd => AcentoClaro;
            public override Color MenuItemBorder => Borde;
            public override Color MenuBorder => AcentoOscuro;
            public override Color MenuItemPressedGradientBegin => AcentoOscuro;
            public override Color MenuItemPressedGradientEnd => AcentoOscuro;
            public override Color ToolStripDropDownBackground => FondoTarjeta;
            public override Color ImageMarginGradientBegin => FondoPanel;
            public override Color ImageMarginGradientMiddle => FondoPanel;
            public override Color ImageMarginGradientEnd => FondoPanel;
            public override Color SeparatorDark => Borde;
            public override Color SeparatorLight => FondoTarjeta;
        }
    }
}
