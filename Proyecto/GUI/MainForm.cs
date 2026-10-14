using BLL;
using BE;
using SERVICE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class MainForm : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BE_USUARIO usuarioActual;

        public MainForm(BE_USUARIO usuario = null)
        {
            InitializeComponent();

            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;

            usuarioActual = usuario;
            idiomaManager = SV_IdiomaManager.GetInstance();
            idiomaManager.RegistrarObservador(this);

            CrearMenuIdiomas();
            if (usuarioActual != null && (usuarioActual.Rol?.ToUpper() == "ADMIN" || usuarioActual.Rol?.ToUpper() == "ADMINISTRADOR"))
            {
                CrearMenuAdministracion();
            }
            CrearMenuVentanas();
            CrearMenuSesion();
            ActualizarTextos();
            AplicarControlesDeAcceso();
            UITheme.Aplicar(this);
            this.Load += (s, e) => UITheme.Aplicar(this);
        }

        private void AbrirHijoMdi(Form form)
        {
            foreach (Form hijo in this.MdiChildren)
            {
                if (hijo.GetType() == form.GetType())
                {
                    hijo.Activate();
                    form.Dispose();
                    return;
                }
            }

            form.MdiParent = this;
            UITheme.Aplicar(form);
            form.Show();
        }

        private void AplicarControlesDeAcceso()
        {
            if (usuarioActual == null)
                return;

            string rolUsuario = usuarioActual.Rol?.ToUpper() ?? "USUARIO";

            var menuStrip = this.Controls.OfType<MenuStrip>().FirstOrDefault();
            if (menuStrip == null) return;

            foreach (ToolStripItem item in menuStrip.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                    AplicarControlesMenu(menuItem, rolUsuario);
            }

            System.Diagnostics.Debug.WriteLine($"Controles aplicados para rol: {rolUsuario}");
        }

        private void AplicarControlesMenu(ToolStripMenuItem item, string rol)
        {
            bool esAdmin = rol == "ADMIN" || rol == "ADMINISTRADOR";
            bool esAuditor = rol == "AUDITOR";

            foreach (ToolStripItem subitem in item.DropDownItems)
            {
                if (subitem is ToolStripMenuItem subMenu)
                {
                    string clave = subMenu.Tag?.ToString()?.ToUpper() ?? "";

                    if (esAuditor)
                    {
                        if (clave == "BACKUPS" || clave == "ADMINISTRACION" ||
                            clave == "GESTION_USUARIOS" || clave == "ROLES" || clave == "ADMINISTRACION_PERMISOS" ||
                            clave == "ASIGNAR_PERMISOS" || clave == "INTEGRIDAD")
                            subMenu.Visible = false;
                    }
                    else if (rol == "USUARIO")
                    {
                        if (clave == "AUDITORIA" || clave == "BACKUPS" ||
                            clave == "ADMINISTRACION" || clave == "GESTION_USUARIOS" || clave == "ROLES" ||
                            clave == "ADMINISTRACION_PERMISOS" || clave == "ASIGNAR_PERMISOS" ||
                            clave == "SNAPSHOTS" || clave == "INTEGRIDAD")
                            subMenu.Visible = false;
                    }

                    AplicarControlesMenu(subMenu, rol);
                }
            }

            if (!esAdmin && item.Tag?.ToString() == "ADMINISTRACION")
                item.Visible = false;
        }

        public BE_USUARIO ObtenerUsuario()
        {
            return usuarioActual;
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            System.Diagnostics.Debug.WriteLine($"MainForm.ActualizarIdioma: {codigoIdiomaAnterior} -> {codigoIdiomaNuevo}");
            ActualizarTextos();
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("SISTEMA_PRINCIPAL");
            ActualizarMenuTextos();
        }

        private void ActualizarMenuTextos()
        {
            var menuStrip = this.Controls.OfType<MenuStrip>().FirstOrDefault();
            if (menuStrip == null) return;

            foreach (ToolStripItem item in menuStrip.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                    ActualizarItemMenu(menuItem);
            }
        }

        private void ActualizarMenuIdiomas()
        {
            var menuStrip = this.Controls.OfType<MenuStrip>().FirstOrDefault();
            if (menuStrip == null) return;

            var menuOpciones = menuStrip.Items.OfType<ToolStripMenuItem>().FirstOrDefault(m => m.Tag?.ToString() == "OPCIONES");
            if (menuOpciones == null) return;

            var menuIdioma = menuOpciones.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(m => m.Tag?.ToString() == "CAMBIAR_IDIOMA_TITULO");
            if (menuIdioma == null) return;

            menuIdioma.DropDownItems.Clear();

            var idiomas = idiomaManager.ObtenerNombresIdiomas();
            var fuenteIdiomas = new Font("Segoe UI", 9f);
            foreach (var idioma in idiomas.OrderBy(i => i.Value))
            {
                ToolStripMenuItem itemIdioma = new ToolStripMenuItem(idioma.Value);
                itemIdioma.Tag = idioma.Key;
                itemIdioma.Font = fuenteIdiomas;
                itemIdioma.Click += (s, e) =>
                {
                    idiomaManager.CambiarIdioma(idioma.Key);
                };
                menuIdioma.DropDownItems.Add(itemIdioma);
            }

            if (this.MainMenuStrip != null)
                UITheme.EstilizarMenu(this.MainMenuStrip);
        }

        private void ActualizarItemMenu(ToolStripMenuItem item)
        {
            if (item.Tag is string clave && clave.Length > 2)
            {
                string texto = idiomaManager.Obtener(clave);
                if (!string.IsNullOrEmpty(texto))
                    item.Text = "&" + texto;
            }

            foreach (ToolStripItem subitem in item.DropDownItems)
            {
                if (subitem is ToolStripMenuItem subMenu)
                    ActualizarItemMenu(subMenu);
            }
        }

        private void CrearMenuAdministracion()
        {
            var menuStrip = this.Controls.OfType<MenuStrip>().FirstOrDefault();
            if (menuStrip == null) return;

            ToolStripMenuItem menuAdmin = new ToolStripMenuItem("&Administración");
            menuAdmin.Tag = "ADMINISTRACION";

            var itemUsuarios = new ToolStripMenuItem("&Gestión de Usuarios", null, (s, e) => AbrirHijoMdi(new FormGestionUsuarios()));
            itemUsuarios.Tag = "GESTION_USUARIOS";
            menuAdmin.DropDownItems.Add(itemUsuarios);

            var itemRoles = new ToolStripMenuItem("&Roles", null, (s, e) => AbrirHijoMdi(new FormRoles()));
            itemRoles.Tag = "ROLES";
            menuAdmin.DropDownItems.Add(itemRoles);

            var itemPermisoABM = new ToolStripMenuItem("&Catálogo de Permisos", null, (s, e) => AbrirHijoMdi(new FormPermisoABM()));
            itemPermisoABM.Tag = "ADMINISTRACION_PERMISOS";
            menuAdmin.DropDownItems.Add(itemPermisoABM);

            var itemIntegridad = new ToolStripMenuItem("&Integridad / DV", null, (s, e) => AbrirHijoMdi(new FormIntegridad()));
            itemIntegridad.Tag = "INTEGRIDAD";
            menuAdmin.DropDownItems.Add(itemIntegridad);

            menuStrip.Items.Add(menuAdmin);
        }

        private void CrearMenuVentanas()
        {
            var menuStrip = this.Controls.OfType<MenuStrip>().FirstOrDefault();
            if (menuStrip == null) return;

            ToolStripMenuItem menuVentanas = new ToolStripMenuItem("&Ventanas");
            menuVentanas.Tag = "VENTANAS";

            var itemCascada = new ToolStripMenuItem("&Cascada", null, (s, e) => this.LayoutMdi(MdiLayout.Cascade));
            itemCascada.Tag = "CASCADA";
            menuVentanas.DropDownItems.Add(itemCascada);

            var itemHorizontal = new ToolStripMenuItem("Mosaico &Horizontal", null, (s, e) => this.LayoutMdi(MdiLayout.TileHorizontal));
            itemHorizontal.Tag = "MOSAICO_HORIZONTAL";
            menuVentanas.DropDownItems.Add(itemHorizontal);

            var itemVertical = new ToolStripMenuItem("Mosaico &Vertical", null, (s, e) => this.LayoutMdi(MdiLayout.TileVertical));
            itemVertical.Tag = "MOSAICO_VERTICAL";
            menuVentanas.DropDownItems.Add(itemVertical);

            var itemCerrarTodas = new ToolStripMenuItem("&Cerrar todas", null, (s, e) =>
            {
                foreach (Form hijo in this.MdiChildren.ToArray())
                    hijo.Close();
            });
            itemCerrarTodas.Tag = "CERRAR_TODAS";
            menuVentanas.DropDownItems.Add(itemCerrarTodas);

            menuStrip.Items.Add(menuVentanas);
        }

        private void CrearMenuSesion()
        {
            var menuStrip = this.Controls.OfType<MenuStrip>().FirstOrDefault();
            if (menuStrip == null) return;

            ToolStripMenuItem menuSesion = new ToolStripMenuItem("&Sesión");
            menuSesion.Tag = "SESION";

            var itemCerrarSesion = new ToolStripMenuItem("&Cerrar Sesión", null, btnLogoff_Click);
            itemCerrarSesion.Tag = "LOGOUT";
            menuSesion.DropDownItems.Add(itemCerrarSesion);

            menuStrip.Items.Add(menuSesion);
        }

        private void CrearMenuIdiomas()
        {
            if (this.Controls.OfType<MenuStrip>().Any())
                return;

            MenuStrip menuStrip = new MenuStrip();

            ToolStripMenuItem menuVer = new ToolStripMenuItem("&Ver");
            menuVer.Tag = "VER";

            var itemAuditoria = new ToolStripMenuItem("&Auditoría", null, (s, e) => AbrirHijoMdi(new FormAuditoria()));
            itemAuditoria.Tag = "AUDITORIA";
            menuVer.DropDownItems.Add(itemAuditoria);

            var itemBitacora = new ToolStripMenuItem("&Bitácora", null, (s, e) => AbrirHijoMdi(new Bitacora()));
            itemBitacora.Tag = "BITACORA";
            menuVer.DropDownItems.Add(itemBitacora);

            var itemBackups = new ToolStripMenuItem("&Backups", null, (s, e) => AbrirHijoMdi(new FormBackups()));
            itemBackups.Tag = "BACKUPS";
            menuVer.DropDownItems.Add(itemBackups);

            var itemSnapshots = new ToolStripMenuItem("&Snapshots", null, (s, e) => AbrirHijoMdi(new FormSnapshots()));
            itemSnapshots.Tag = "SNAPSHOTS";
            menuVer.DropDownItems.Add(itemSnapshots);

            ToolStripMenuItem menuOpciones = new ToolStripMenuItem("&Opciones");
            menuOpciones.Tag = "OPCIONES";

            ToolStripMenuItem menuIdioma = new ToolStripMenuItem("&Idioma");
            menuIdioma.Tag = "CAMBIAR_IDIOMA_TITULO";

            var idiomas = idiomaManager.ObtenerNombresIdiomas();
            var fuenteIdiomas = new Font("Segoe UI", 9f);
            foreach (var idioma in idiomas.OrderBy(i => i.Value))
            {
                ToolStripMenuItem itemIdioma = new ToolStripMenuItem(idioma.Value);
                itemIdioma.Tag = idioma.Key;
                itemIdioma.Font = fuenteIdiomas;
                itemIdioma.Click += (s, e) =>
                {
                    idiomaManager.CambiarIdioma(idioma.Key);
                };
                menuIdioma.DropDownItems.Add(itemIdioma);
            }

            menuOpciones.DropDownItems.Add(menuIdioma);

            var itemAgregarIdioma = new ToolStripMenuItem("&Agregar Idioma", null, (s, e) =>
            {
                FormAgregarIdioma formIdioma = new FormAgregarIdioma();
                formIdioma.FormClosed += (sender, args) =>
                {
                    if (formIdioma.IdiomaAgregado)
                    {
                        ActualizarMenuIdiomas();
                    }
                };
                AbrirHijoMdi(formIdioma);
            });
            itemAgregarIdioma.Tag = "AGREGAR_IDIOMA";
            menuOpciones.DropDownItems.Add(itemAgregarIdioma);

            menuStrip.Items.Add(menuVer);
            menuStrip.Items.Add(menuOpciones);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
            try
            {
                BLL_USUARIO user = new BLL_USUARIO();
                user.Logout();
                this.Close();
                new LogInOut().Show();
            }
            catch (Exception ex)
            {
                ManejoErrores.MostrarError(ex);
            }
        }
    }
}
