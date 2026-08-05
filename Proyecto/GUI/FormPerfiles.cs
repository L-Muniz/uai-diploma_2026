using BE;
using SERVICE;
using BLL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormPerfiles : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_PERFIL bllPerfil;
        private bool arrastrandoPerfil;

        public FormPerfiles()
        {
            InitializeComponent();

            idiomaManager = SV_IdiomaManager.GetInstance();
            bllPerfil = new BLL_PERFIL();

            CrearColumnasDataGrid();
            idiomaManager.RegistrarObservador(this);
            ActualizarTextos();
            CargarPerfiles();
            this.Load += (s, e) => AplicarIdentidadVisual();
        }

        private void AplicarIdentidadVisual()
        {
            UITheme.DiferenciarModulo(
                panelBotones,
                panelIzquierdo,
                idiomaManager.Obtener("PERFILES"),
                "Paquetes de permisos reutilizables",
                UITheme.AcentoPerfiles,
                UITheme.FondoPerfiles,
                btnNuevo, btnEditar, btnEliminar);
        }

        private void CrearColumnasDataGrid()
        {
            dgvPermisos.Columns.Clear();
            dgvPermisos.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Tipo", HeaderText = "Tipo", DataPropertyName = "Tipo", Width = 80 });
            dgvPermisos.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Nombre", HeaderText = "Nombre", DataPropertyName = "Nombre", Width = 180 });
            dgvPermisos.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Codigo", HeaderText = "Código", DataPropertyName = "Codigo", Width = 100 });
            dgvPermisos.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Descripcion", HeaderText = "Descripción", DataPropertyName = "Descripcion", Width = 220 });
            dgvPermisos.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "EsHeredado", HeaderText = "Heredado", DataPropertyName = "EsHeredado", Width = 70 });
            dgvPermisos.AutoGenerateColumns = false;
            DataGridViewSortHelper.HabilitarOrdenamiento(dgvPermisos);
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("PERFILES");
            if (btnNuevo != null) btnNuevo.Text = idiomaManager.Obtener("NUEVO_PERFIL");
            if (btnEditar != null) btnEditar.Text = idiomaManager.Obtener("EDITAR");
            if (btnEliminar != null) btnEliminar.Text = idiomaManager.Obtener("ELIMINAR");
            if (btnAgregarPermiso != null) btnAgregarPermiso.Text = "Agregar";
            if (btnEliminarPermiso != null) btnEliminarPermiso.Text = "Desasignar";
            if (lblPerfiles != null) lblPerfiles.Text = idiomaManager.Obtener("JERARQUIA_PERFILES");
            if (lblDetalles != null) lblDetalles.Text = idiomaManager.Obtener("DETALLES_PERFIL");
            if (lblNombrePerfil != null) lblNombrePerfil.Text = idiomaManager.Obtener("NOMBRE");
            if (lblDescripcionPerfil != null) lblDescripcionPerfil.Text = idiomaManager.Obtener("DESCRIPCION");
            if (lblPermisosLabel != null) lblPermisosLabel.Text = "Componentes del perfil (permisos y sub-perfiles)";
            if (lblPermisosDisponibles != null) lblPermisosDisponibles.Text = "Agregar componente:";
            if (IsHandleCreated)
                AplicarIdentidadVisual();
        }

        public void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo)
        {
            ActualizarTextos();
        }

        private void btnNuevo_Click(object sender, EventArgs e) => NuevoPerfil();
        private void btnEditar_Click(object sender, EventArgs e) => EditarPerfil();
        private void btnEliminar_Click(object sender, EventArgs e) => EliminarPerfil();
        private void btnAgregarPermiso_Click(object sender, EventArgs e) => AgregarComponente();
        private void btnEliminarPermiso_Click(object sender, EventArgs e) => DesasignarComponente();
        private void tvPerfiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (arrastrandoPerfil)
                return;
            MostrarDetallesPerfil();
        }

        private void tvPerfiles_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (!(e.Item is TreeNode nodo) || !(nodo.Tag is BE_Perfil))
                return;

            arrastrandoPerfil = true;
            try
            {
                DoDragDrop(nodo, DragDropEffects.Move);
            }
            finally
            {
                arrastrandoPerfil = false;
                BeginInvoke(new Action(MostrarDetallesPerfil));
            }
        }

        private void tvPerfiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(typeof(TreeNode)) ? DragDropEffects.Move : DragDropEffects.None;
        }

        private void tvPerfiles_DragOver(object sender, DragEventArgs e)
        {
            var destino = ObtenerNodoBajoCursor(e);
            var origen = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (!EsSoltarPerfilValido(origen, destino))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;
            if (tvPerfiles.SelectedNode != destino)
                tvPerfiles.SelectedNode = destino;
            if (!destino.IsExpanded)
                destino.Expand();
        }

        private void tvPerfiles_DragDrop(object sender, DragEventArgs e)
        {
            var destino = ObtenerNodoBajoCursor(e);
            var origen = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (!EsSoltarPerfilValido(origen, destino))
                return;

            var perfilHijo = (BE_Perfil)origen.Tag;
            var perfilPadre = (BE_Perfil)destino.Tag;
            int idDestino = perfilPadre.ID;
            int idOrigen = perfilHijo.ID;

            try
            {
                bllPerfil.AsignarSubPerfil(idDestino, idOrigen);
                RefrescarTrasArrastre(idDestino);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Asignación de perfiles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                RefrescarTrasArrastre(idOrigen);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al asignar sub-perfil: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RefrescarTrasArrastre(idOrigen);
            }
        }

        private void RefrescarTrasArrastre(int idPerfil)
        {
            arrastrandoPerfil = false;
            BeginInvoke(new Action(() =>
            {
                CargarPerfiles(idPerfil);
                MostrarDetallesPerfil();
            }));
        }

        private TreeNode ObtenerNodoBajoCursor(DragEventArgs e)
        {
            Point punto = tvPerfiles.PointToClient(new Point(e.X, e.Y));
            return tvPerfiles.GetNodeAt(punto);
        }

        private bool EsSoltarPerfilValido(TreeNode origen, TreeNode destino)
        {
            if (origen == null || destino == null || origen == destino)
                return false;

            var perfilHijo = origen.Tag as BE_Perfil;
            var perfilPadre = destino.Tag as BE_Perfil;
            if (perfilHijo == null || perfilPadre == null)
                return false;

            if (perfilHijo.IdPadrePerfil == perfilPadre.ID || origen.Parent == destino)
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

        private void CargarPerfiles(int? idSeleccionar = null)
        {
            try
            {
                var expandidos = TreeViewExpansionHelper.CapturarExpandidos(tvPerfiles, IdDeNodoPerfil);
                int? idPrevio = idSeleccionar ?? TreeViewExpansionHelper.CapturarSeleccionado(tvPerfiles, IdDeNodoPerfil);

                tvPerfiles.BeginUpdate();
                tvPerfiles.Nodes.Clear();
                foreach (var perfil in bllPerfil.ObtenerPerfilesPrincipales())
                    tvPerfiles.Nodes.Add(CrearNodoPerfil(perfil, new HashSet<int>()));

                TreeViewExpansionHelper.RestaurarExpandidos(tvPerfiles, expandidos, IdDeNodoPerfil);

                if (idPrevio.HasValue)
                    SeleccionarPerfilEnArbol(idPrevio.Value);
                else if (tvPerfiles.Nodes.Count > 0)
                    tvPerfiles.SelectedNode = tvPerfiles.Nodes[0];
                else
                    LimpiarDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar perfiles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                tvPerfiles.EndUpdate();
            }
        }

        private static int? IdDeNodoPerfil(object tag) => (tag as BE_Perfil)?.ID;

        private TreeNode CrearNodoPerfil(BE_Perfil perfil, HashSet<int> visitados)
        {
            var nodo = new TreeNode(perfil.Nombre) { Tag = perfil };
            if (!visitados.Add(perfil.ID))
                return nodo;

            foreach (var hijo in bllPerfil.ObtenerPerfilesHijos(perfil.ID))
                nodo.Nodes.Add(CrearNodoPerfil(hijo, visitados));

            return nodo;
        }

        private void LimpiarDetalle()
        {
            txtNombrePerfil.Text = "";
            txtDescripcionPerfil.Text = "";
            dgvPermisos.DataSource = null;
            cbPermisosDisponibles.DataSource = null;
        }

        private void MostrarDetallesPerfil()
        {
            var perfil = tvPerfiles.SelectedNode?.Tag as BE_Perfil;
            if (perfil == null)
            {
                LimpiarDetalle();
                return;
            }

            txtNombrePerfil.Text = perfil.Nombre;
            txtDescripcionPerfil.Text = perfil.Descripcion ?? "";
            CargarComponentes(perfil.ID);
        }

        private void CargarComponentes(int idPerfil)
        {
            try
            {
                DataGridViewSortHelper.Bind(dgvPermisos, bllPerfil.ObtenerComponentesDelPerfil(idPerfil));
                cbPermisosDisponibles.DataSource = null;
                cbPermisosDisponibles.DataSource = bllPerfil.ObtenerComponentesDisponiblesParaAsignar(idPerfil);
                cbPermisosDisponibles.DisplayMember = "Etiqueta";
                cbPermisosDisponibles.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar componentes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AgregarComponente()
        {
            var perfil = tvPerfiles.SelectedNode?.Tag as BE_Perfil;
            var componente = cbPermisosDisponibles.SelectedItem as BE_ComponenteAcceso;
            if (perfil == null)
            {
                MessageBox.Show("Selecciona un perfil", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (componente == null) return;

            try
            {
                bllPerfil.AsignarComponente(perfil.ID, componente);
                CargarPerfiles(perfil.ID);
                MessageBox.Show(componente.EsPerfil ? "Perfil integrante asignado" : "Permiso agregado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Asignación de perfiles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar componente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DesasignarComponente()
        {
            var perfil = tvPerfiles.SelectedNode?.Tag as BE_Perfil;
            if (perfil == null || dgvPermisos.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un componente para desasignar", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var componente = dgvPermisos.SelectedRows[0].DataBoundItem as BE_ComponenteAcceso;
            if (componente == null) return;

            try
            {
                bllPerfil.DesasignarComponente(perfil.ID, componente);
                CargarPerfiles(perfil.ID);
                MessageBox.Show(componente.EsPerfil ? "Perfil integrante desasignado" : "Permiso removido", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void NuevoPerfil()
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Nuevo Perfil";
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
                Label lblPadre = new Label() { Left = 20, Top = 190, Text = "Perfil Padre (opcional):", Width = 400 };
                ComboBox cbPadre = new ComboBox() { Left = 20, Top = 215, Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };
                CargarComboPadre(cbPadre, bllPerfil.ObtenerTodosLosPerfiles(), null);

                Button btnOK = new Button() { Text = "OK", Left = 250, Width = 80, Top = 260, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 340, Width = 80, Top = 260, DialogResult = DialogResult.Cancel };
                dialog.Controls.AddRange(new Control[] { lblNombre, txtNombre, lblDesc, txtDesc, lblPadre, cbPadre, btnOK, btnCancel });
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(txtNombre.Text))
                {
                    try
                    {
                        var nuevo = new BE_Perfil(txtNombre.Text, txtDesc.Text);
                        nuevo.IdPadrePerfil = LeerIdPadreSeleccionado(cbPadre);
                        bllPerfil.CrearPerfil(nuevo);
                        CargarPerfiles();
                        MessageBox.Show("Perfil creado exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Asignación de perfiles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al crear perfil: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EditarPerfil()
        {
            var perfil = tvPerfiles.SelectedNode?.Tag as BE_Perfil;
            if (perfil == null)
            {
                MessageBox.Show("Selecciona un perfil", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Form dialog = new Form())
            {
                dialog.Text = "Editar Perfil";
                dialog.Width = 450;
                dialog.Height = 350;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lblNombre = new Label() { Left = 20, Top = 20, Text = "Nombre:", Width = 400 };
                TextBox txtNombre = new TextBox() { Left = 20, Top = 45, Width = 400, Text = perfil.Nombre };
                Label lblDesc = new Label() { Left = 20, Top = 75, Text = "Descripción:", Width = 400 };
                TextBox txtDesc = new TextBox() { Left = 20, Top = 100, Width = 400, Height = 80, Multiline = true, Text = perfil.Descripcion ?? "" };
                Label lblPadre = new Label() { Left = 20, Top = 190, Text = "Perfil Padre (opcional):", Width = 400 };
                ComboBox cbPadre = new ComboBox() { Left = 20, Top = 215, Width = 400, DropDownStyle = ComboBoxStyle.DropDownList };
                CargarComboPadre(cbPadre, bllPerfil.ObtenerPerfilesCandidatosComoPadre(perfil.ID), perfil.IdPadrePerfil);

                Button btnOK = new Button() { Text = "OK", Left = 250, Width = 80, Top = 260, DialogResult = DialogResult.OK };
                Button btnCancel = new Button() { Text = "Cancelar", Left = 340, Width = 80, Top = 260, DialogResult = DialogResult.Cancel };
                dialog.Controls.AddRange(new Control[] { lblNombre, txtNombre, lblDesc, txtDesc, lblPadre, cbPadre, btnOK, btnCancel });
                dialog.AcceptButton = btnOK;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        perfil.Nombre = txtNombre.Text;
                        perfil.Descripcion = txtDesc.Text;
                        perfil.IdPadrePerfil = LeerIdPadreSeleccionado(cbPadre);
                        bllPerfil.ActualizarPerfil(perfil);
                        CargarPerfiles(perfil.ID);
                        MessageBox.Show("Perfil actualizado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Asignación de perfiles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al editar perfil: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void EliminarPerfil()
        {
            var perfil = tvPerfiles.SelectedNode?.Tag as BE_Perfil;
            if (perfil == null)
            {
                MessageBox.Show("Selecciona un perfil", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                    $"¿Eliminar el perfil '{perfil.Nombre}' y todo su árbol de sub-perfiles?",
                    "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            try
            {
                bllPerfil.EliminarPerfil(perfil.ID);
                CargarPerfiles();
                MessageBox.Show("Perfil eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar perfil: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CargarComboPadre(ComboBox combo, List<BE_Perfil> candidatos, int? idPadreActual)
        {
            var items = new List<BE_Perfil> { new BE_Perfil { ID = 0, Nombre = "(Sin padre)" } };
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

        private void SeleccionarPerfilEnArbol(int idPerfil)
        {
            TreeNode encontrado = BuscarNodo(tvPerfiles.Nodes, idPerfil);
            if (encontrado != null)
            {
                tvPerfiles.SelectedNode = encontrado;
                encontrado.EnsureVisible();
            }
        }

        private TreeNode BuscarNodo(TreeNodeCollection nodos, int idPerfil)
        {
            foreach (TreeNode nodo in nodos)
            {
                if (nodo.Tag is BE_Perfil p && p.ID == idPerfil)
                    return nodo;
                var hijo = BuscarNodo(nodo.Nodes, idPerfil);
                if (hijo != null)
                    return hijo;
            }
            return null;
        }
    }
}
