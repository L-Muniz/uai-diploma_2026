using BE;
using SERVICE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormRoles : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_ROL bllRol;
        private List<BE_Rol> rolesRaiz;
        private bool arrastrandoRol;

        public FormRoles()
        {
            InitializeComponent();

            idiomaManager = SV_IdiomaManager.GetInstance();
            bllRol = new BLL_ROL();

            CrearColumnasDataGrid();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            CargarRoles();
            this.Load += (s, e) => AplicarIdentidadVisual();
        }

        private void AplicarIdentidadVisual()
        {
            UITheme.DiferenciarModulo(
                panelBotones,
                panelIzquierdo,
                idiomaManager.Obtener("ROLES"),
                "Grupos de acceso asignables a usuarios",
                UITheme.AcentoRoles,
                UITheme.FondoRoles,
                btnNuevo, btnEditar, btnEliminar);
        }

        private void CrearColumnasDataGrid()
        {
            dgvComponentes.Columns.Clear();
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Tipo", HeaderText = "Tipo", DataPropertyName = "Tipo", Width = 80 });
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Nombre", HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 180 });
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Codigo", HeaderText = "Código", DataPropertyName = "Codigo", Width = 100 });
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Descripcion", HeaderText = "Descripción", DataPropertyName = "Descripcion", Width = 180 });
            dgvComponentes.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Origen", HeaderText = "Origen", DataPropertyName = "Origen", Width = 240 });
            dgvComponentes.AutoGenerateColumns = false;
            DataGridViewSortHelper.HabilitarOrdenamiento(dgvComponentes);
            dgvComponentes.DataBindingComplete -= DgvComponentes_DataBindingComplete;
            dgvComponentes.DataBindingComplete += DgvComponentes_DataBindingComplete;
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("ROLES");
            if (lblRoles != null) lblRoles.Text = idiomaManager.Obtener("JERARQUIA_ROLES");
            if (lblDetalles != null) lblDetalles.Text = idiomaManager.Obtener("DETALLES_ROL");
            if (lblNombreRol != null) lblNombreRol.Text = idiomaManager.Obtener("NOMBRE");
            if (lblDescripcionRol != null) lblDescripcionRol.Text = idiomaManager.Obtener("DESCRIPCION");
            if (btnNuevo != null) btnNuevo.Text = idiomaManager.Obtener("NUEVO_ROL");
            if (btnEditar != null) btnEditar.Text = idiomaManager.Obtener("EDITAR");
            if (btnEliminar != null) btnEliminar.Text = idiomaManager.Obtener("ELIMINAR");
            if (lblComponentesDisponibles != null) lblComponentesDisponibles.Text = "Agregar componente:";
            if (lblComponentes != null) lblComponentes.Text = "Componentes del rol (permisos, perfiles y sub-roles)";
            if (btnAgregarComponente != null) btnAgregarComponente.Text = "Agregar";
            if (btnDesasignarComponente != null) btnDesasignarComponente.Text = "Desasignar";
            if (IsHandleCreated)
                AplicarIdentidadVisual();
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }

        private void btnNuevo_Click(object sender, EventArgs e) => NuevoRol();
        private void btnEditar_Click(object sender, EventArgs e) => EditarRol();
        private void btnEliminar_Click(object sender, EventArgs e) => EliminarRol();
        private void tvRoles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (arrastrandoRol)
                return;
            MostrarDetallesRol();
        }

        private void tvRoles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!(e.Item is TreeNode nodo) || !(nodo.Tag is BE_Rol))
                return;

            arrastrandoRol = true;
            try
            {
                DoDragDrop(nodo, DragDropEffects.Move);
            }
            finally
            {
                arrastrandoRol = false;
                MostrarDetallesRol();
            }
        }

        private void tvRoles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(typeof(TreeNode)) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void tvRoles_DragOver(object sender, DragEventArgs e)
        {
            var destino = ObtenerNodoBajoCursor(e);
            var origen = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (!EsSoltarRolValido(origen, destino))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;
            if (tvRoles.SelectedNode != destino)
                tvRoles.SelectedNode = destino;
            if (!destino.IsExpanded)
                destino.Expand();
        }

        private void tvRoles_DragDrop(object sender, DragEventArgs e)
        {
            var destino = ObtenerNodoBajoCursor(e);
            var origen = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (!EsSoltarRolValido(origen, destino))
                return;

            var rolHijo = (BE_Rol)origen.Tag;
            var rolPadre = (BE_Rol)destino.Tag;

            try
            {
                bllRol.AsignarSubRol(rolPadre.ID, rolHijo.ID);
                CargarRoles(rolPadre.ID);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Asignación de roles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CargarRoles(rolHijo.ID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al asignar sub-rol: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CargarRoles(rolHijo.ID);
            }
        }

        private TreeNode ObtenerNodoBajoCursor(DragEventArgs e)
        {
            Point punto = tvRoles.PointToClient(new Point(e.X, e.Y));
            return tvRoles.GetNodeAt(punto);
        }

        private bool EsSoltarRolValido(TreeNode origen, TreeNode destino)
        {
            if (origen == null || destino == null || origen == destino)
                return false;

            var rolHijo = origen.Tag as BE_Rol;
            var rolPadre = destino.Tag as BE_Rol;
            if (rolHijo == null || rolPadre == null)
                return false;

            if (rolHijo.IdPadreRol == rolPadre.ID || origen.Parent == destino)
                return false;

            return !EsAncestro(origen, destino);
        }

        private static bool EsAncestro(TreeNode posibleAncestro, TreeNode nodo)
        {
            for (TreeNode actual = nodo.Parent; actual != null; actual = actual.Parent)
            {
                if (actual == posibleAncestro)
                    return true;
            }
            return false;
        }

        private void btnAgregarComponente_Click(object sender, EventArgs e) => AgregarComponente();
        private void btnDesasignarComponente_Click(object sender, EventArgs e) => DesasignarComponente();

        private void CargarRoles(int? idSeleccionar = null)
        {
            try
            {
                var expandidos = TreeViewExpansionHelper.CapturarExpandidos(tvRoles, IdDeNodoRol);
                int? idPrevio = idSeleccionar ?? TreeViewExpansionHelper.CapturarSeleccionado(tvRoles, IdDeNodoRol);

                tvRoles.BeginUpdate();
                rolesRaiz = bllRol.ObtenerRolesPrincipales();
                tvRoles.Nodes.Clear();

                foreach (var rol in rolesRaiz)
                    tvRoles.Nodes.Add(CrearNodoRol(rol, new HashSet<int>()));

                TreeViewExpansionHelper.RestaurarExpandidos(tvRoles, expandidos, IdDeNodoRol);

                if (idPrevio.HasValue)
                    SeleccionarRolEnArbol(idPrevio.Value);
                else if (tvRoles.Nodes.Count > 0)
                    tvRoles.SelectedNode = tvRoles.Nodes[0];
                else
                    LimpiarDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar roles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                tvRoles.EndUpdate();
            }
        }

        private static int? IdDeNodoRol(object tag) => (tag as BE_Rol)?.ID;

        private TreeNode CrearNodoRol(BE_Rol rol, HashSet<int> visitados)
        {
            var nodo = new TreeNode(rol.Nombre) { Tag = rol };
            if (!visitados.Add(rol.ID))
                return nodo;

            foreach (var hijo in bllRol.ObtenerRolesHijos(rol.ID))
                nodo.Nodes.Add(CrearNodoRol(hijo, visitados));

            return nodo;
        }

        private void LimpiarDetalle()
        {
            txtNombreRol.Text = "";
            txtDescripcionRol.Text = "";
            dgvComponentes.DataSource = null;
            cbComponentesDisponibles.DataSource = null;
        }

        private void MostrarDetallesRol()
        {
            if (tvRoles.SelectedNode == null)
            {
                LimpiarDetalle();
                return;
            }

            var rol = tvRoles.SelectedNode.Tag as BE_Rol;
            if (rol == null) return;

            txtNombreRol.Text = rol.Nombre;
            txtDescripcionRol.Text = rol.Descripcion ?? "";
            CargarComponentes(rol.ID);
        }

        private void CargarComponentes(int idRol)
        {
            try
            {
                DataGridViewSortHelper.Bind(dgvComponentes, bllRol.ObtenerComponentesDelRol(idRol));
                cbComponentesDisponibles.DataSource = null;
                cbComponentesDisponibles.DisplayMember = "Etiqueta";
                cbComponentesDisponibles.ValueMember = "ID";
                cbComponentesDisponibles.DataSource = bllRol.ObtenerComponentesDisponiblesParaAsignar(idRol);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar componentes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvComponentes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgvComponentes.Rows)
            {
                var componente = row.DataBoundItem as BE_ComponenteAcceso;
                if (componente == null)
                    continue;

                if (componente.EsPerfil || componente.ProvieneDePerfil)
                {
                    row.DefaultCellStyle.BackColor = UITheme.FondoPerfiles;
                    row.DefaultCellStyle.ForeColor = UITheme.AcentoPerfiles;
                }
                else if (componente.EsHeredado)
                {
                    row.DefaultCellStyle.ForeColor = Color.DimGray;
                }
            }
        }

        private void AgregarComponente()
        {
            var rol = tvRoles.SelectedNode?.Tag as BE_Rol;
            var componente = cbComponentesDisponibles.SelectedItem as BE_ComponenteAcceso;
            if (rol == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (componente == null) return;

            try
            {
                bllRol.AsignarComponente(rol.ID, componente);
                CargarRoles(rol.ID);
                string mensaje = componente.EsRol
                    ? "Rol integrante asignado"
                    : componente.EsPerfil
                        ? "Perfil asignado al rol"
                        : "Permiso agregado";
                MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Asignación de roles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar componente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DesasignarComponente()
        {
            var rol = tvRoles.SelectedNode?.Tag as BE_Rol;
            if (rol == null || dgvComponentes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un componente para desasignar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var componente = dgvComponentes.SelectedRows[0].DataBoundItem as BE_ComponenteAcceso;
            if (componente == null) return;

            try
            {
                bllRol.DesasignarComponente(rol.ID, componente);
                CargarRoles(rol.ID);
                string mensaje = componente.EsRol
                    ? "Rol integrante desasignado"
                    : componente.EsPerfil
                        ? "Perfil desasignado del rol"
                        : "Permiso removido";
                MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Desasignar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al desasignar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void NuevoRol()
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Nuevo Rol";
                dialog.Width = 450;
                dialog.Height = 350;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblNombre = new Label() { Left = 20, Top = 20, Text = "Nombre:", Width = 400 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 45, Width = 400 };
                Label lblDesc = new Label() { Left = 20, Top = 75, Text = "Descripción:", Width = 400 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 100, Width = 400, Height = 80, Multiline = true };
                Label lblPadre = new Label() { Left = 20, Top = 190, Text = "Rol Padre (opcional):", Width = 400 };
                ComboBox cbPadre = new ComboBox() { Left = 20, Top = 215, Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };
                CargarComboPadre(cbPadre, bllRol.ObtenerTodosLosRoles(), null);

                Button btnOK = new Button() { Text = "OK", Left = 250, Width = 80, Top = 260, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 340, Width = 80, Top = 260, DialogResult = DialogResult.Cancel };
                dialog.Controls.AddRange(new Control[] { lblNombre, txtNombre, lblDesc, txtDesc, lblPadre, cbPadre, btnOK, btnCancel });
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(txtNombre.Text))
                {
                    try
                    {
                        BE_Rol nuevoRol = new BE_Rol(txtNombre.Text, txtDesc.Text);
                        nuevoRol.IdPadreRol = LeerIdPadreSeleccionado(cbPadre);

                        bllRol.CrearRol(nuevoRol);
                        CargarRoles();
                        MessageBox.Show("Rol creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Asignación de roles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al crear rol: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EditarRol()
        {
            var rol = tvRoles.SelectedNode?.Tag as BE_Rol;
            if (rol == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Form dialog = new Form())
            {
                dialog.Text = "Editar Rol";
                dialog.Width = 450;
                dialog.Height = 350;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblNombre = new Label() { Left = 20, Top = 20, Text = "Nombre:", Width = 400 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 45, Width = 400, Text = rol.Nombre };
                Label lblDesc = new Label() { Left = 20, Top = 75, Text = "Descripción:", Width = 400 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 100, Width = 400, Height = 80, Multiline = true, Text = rol.Descripcion ?? "" };
                Label lblPadre = new Label() { Left = 20, Top = 190, Text = "Rol Padre (opcional):", Width = 400 };
                ComboBox cbPadre = new ComboBox() { Left = 20, Top = 215, Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };
                CargarComboPadre(cbPadre, bllRol.ObtenerRolesCandidatosComoPadre(rol.ID), rol.IdPadreRol);

                Button btnOK = new Button() { Text = "OK", Left = 250, Width = 80, Top = 260, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 340, Width = 80, Top = 260, DialogResult = DialogResult.Cancel };
                dialog.Controls.AddRange(new Control[] { lblNombre, txtNombre, lblDesc, txtDesc, lblPadre, cbPadre, btnOK, btnCancel });
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        rol.Nombre = txtNombre.Text;
                        rol.Descripcion = txtDesc.Text;
                        rol.IdPadreRol = LeerIdPadreSeleccionado(cbPadre);
                        bllRol.ActualizarRol(rol);
                        CargarRoles(rol.ID);
                        MessageBox.Show("Rol actualizado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Asignación de roles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al editar rol: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EliminarRol()
        {
            var rol = tvRoles.SelectedNode?.Tag as BE_Rol;
            if (rol == null)
            {
                MessageBox.Show("Selecciona un rol", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                    $"¿Eliminar el rol '{rol.Nombre}' y todo su árbol de sub-roles?",
                    "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            try
            {
                bllRol.EliminarRol(rol.ID);
                CargarRoles();
                MessageBox.Show("Rol eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar rol: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CargarComboPadre(ComboBox combo, List<BE_Rol> candidatos, int? idPadreActual)
        {
            var items = new List<BE_Rol> { new BE_Rol { ID = 0, Nombre = "(Sin padre)" } };
            if (candidatos != null)
                items.AddRange(candidatos);

            combo.DisplayMember = "Nombre";
            combo.ValueMember = "ID";
            combo.DataSource = items;
            combo.SelectedValue = idPadreActual.GetValueOrDefault();
        }

        private static int? LeerIdPadreSeleccionado(ComboBox combo)
        {
            if (combo.SelectedValue is int id && id > 0)
                return id;
            return null;
        }

        private void SeleccionarRolEnArbol(int idRol)
        {
            TreeNode encontrado = BuscarNodo(tvRoles.Nodes, idRol);
            if (encontrado != null)
            {
                tvRoles.SelectedNode = encontrado;
                encontrado.EnsureVisible();
            }
        }

        private TreeNode BuscarNodo(TreeNodeCollection nodos, int idRol)
        {
            foreach (TreeNode nodo in nodos)
            {
                if (nodo.Tag is BE_Rol r && r.ID == idRol)
                    return nodo;
                var hijo = BuscarNodo(nodo.Nodes, idRol);
                if (hijo != null)
                    return hijo;
            }
            return null;
        }
    }
}
