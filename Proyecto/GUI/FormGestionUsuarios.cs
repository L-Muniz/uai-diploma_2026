using BLL;
using BE;
using SERVICE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormGestionUsuarios : Form, ISV_IdiomaObserver
    {
        private SV_IdiomaManager idiomaManager;
        private BLL_USUARIO bllUsuario;
        private BLL_ROL bllRol;
        private DataGridView dgvUsuarios;
        private Button btnBloquearDesbloquear, btnNuevo, btnEliminar, btnAsignarRol;
        private Label lblUsuarios;

        public FormGestionUsuarios()
        {
            idiomaManager = SV_IdiomaManager.GetInstance();
            idiomaManager.RegistrarObservador(this);
            bllUsuario = new BLL_USUARIO();
            bllRol = new BLL_ROL();

            CrearControles();
            ActualizarTextos();
            CargarUsuarios();
        }

        private void CrearControles()
        {
            this.Text = "Gestión de Usuarios";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblUsuarios = new Label() { Location = new Point(10, 10), AutoSize = true };

            dgvUsuarios = new DataGridView()
            {
                Location = new Point(10, 35),
                Size = new Size(770, 390),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn() { Name = "ID", HeaderText = "ID", DataPropertyName = "ID", FillWeight = 10 });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Username", HeaderText = "Usuario", DataPropertyName = "Username", FillWeight = 30 });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn() { Name = "Rol", HeaderText = "Rol", DataPropertyName = "Rol", FillWeight = 25 });
            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "Bloqueado", HeaderText = "Bloqueado", DataPropertyName = "Bloqueado", FillWeight = 15 });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn() { Name = "DVHorizontal", HeaderText = "DVH", DataPropertyName = "DVHorizontal", FillWeight = 20 });
            dgvUsuarios.SelectionChanged += (s, e) => ActualizarEstadoBotones();
            DataGridViewSortHelper.HabilitarOrdenamiento(dgvUsuarios);

            const int btnY = 445;
            const int btnH = 32;
            btnNuevo = CrearBotonAccion("Nuevo", 10, btnY, 90, btnH, (s, e) => NuevoUsuario());
            Button btnEditar = CrearBotonAccion("Editar", 108, btnY, 90, btnH, (s, e) => EditarUsuario());
            btnBloquearDesbloquear = CrearBotonAccion("Bloquear / Desbloquear", 206, btnY, 165, btnH, (s, e) => AlternarBloqueoUsuario());
            btnEliminar = CrearBotonAccion("Eliminar", 379, btnY, 95, btnH, (s, e) => EliminarUsuario());
            btnAsignarRol = CrearBotonAccion("Asignar Rol", 482, btnY, 110, btnH, (s, e) => AsignarRolUsuario());

            this.Controls.AddRange(new Control[] { lblUsuarios, dgvUsuarios, btnNuevo, btnEditar, btnBloquearDesbloquear, btnEliminar, btnAsignarRol });
        }

        private static Button CrearBotonAccion(string texto, int x, int y, int ancho, int alto, EventHandler onClick)
        {
            var boton = new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(ancho, alto)
            };
            boton.Click += onClick;
            return boton;
        }

        private void ActualizarTextos()
        {
            this.Text = idiomaManager.Obtener("GESTION_USUARIOS_TITULO");
            if (lblUsuarios != null) lblUsuarios.Text = idiomaManager.Obtener("USUARIOS_REGISTRADOS");
            if (btnNuevo != null) btnNuevo.Text = idiomaManager.Obtener("NUEVO");
            if (btnBloquearDesbloquear != null) btnBloquearDesbloquear.Text = "Bloquear / Desbloquear";
            if (btnEliminar != null) btnEliminar.Text = idiomaManager.Obtener("ELIMINAR");
            if (btnAsignarRol != null) btnAsignarRol.Text = "Asignar Rol";

            if (dgvUsuarios.Columns.Count >= 5)
            {
                dgvUsuarios.Columns["ID"].HeaderText = "ID";
                dgvUsuarios.Columns["Username"].HeaderText = idiomaManager.Obtener("USUARIO");
                dgvUsuarios.Columns["Rol"].HeaderText = idiomaManager.Obtener("ROLES");
                if (string.IsNullOrWhiteSpace(dgvUsuarios.Columns["Rol"].HeaderText) || dgvUsuarios.Columns["Rol"].HeaderText.StartsWith("["))
                    dgvUsuarios.Columns["Rol"].HeaderText = "Rol";
                dgvUsuarios.Columns["Bloqueado"].HeaderText = idiomaManager.Obtener("BLOQUEADO");
                dgvUsuarios.Columns["DVHorizontal"].HeaderText = "DVH";
            }
        }

        private void CargarUsuarios()
        {
            try
            {
                var usuarios = bllUsuario.ListarUsuarios();
                DataGridViewSortHelper.Bind(dgvUsuarios, usuarios);
                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                ManejoErrores.MostrarError(ex);
            }
        }

        private void ActualizarEstadoBotones()
        {
            BE_USUARIO usuario = ObtenerUsuarioSeleccionado();
            bool esAdmin = usuario != null && BLL_USUARIO.EsRolAdministrador(usuario.Rol);

            if (btnBloquearDesbloquear != null)
                btnBloquearDesbloquear.Enabled = usuario != null && !esAdmin;
            if (btnEliminar != null)
                btnEliminar.Enabled = usuario != null && !esAdmin;
        }

        private BE_USUARIO ObtenerUsuarioSeleccionado()
        {
            if (dgvUsuarios == null || dgvUsuarios.SelectedRows.Count == 0)
                return null;

            return dgvUsuarios.SelectedRows[0].DataBoundItem as BE_USUARIO;
        }

        private void AlternarBloqueoUsuario()
        {
            try
            {
                BE_USUARIO usuario = ObtenerUsuarioSeleccionado();
                if (usuario == null)
                {
                    MessageBox.Show(idiomaManager.Obtener("SELECCIONA_USUARIO"), "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (BLL_USUARIO.EsRolAdministrador(usuario.Rol))
                {
                    MessageBox.Show("No se puede bloquear ni desbloquear a un usuario administrador.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                usuario.Bloqueado = !usuario.Bloqueado;
                bllUsuario.ActualizarUsuario(usuario);
                MessageBox.Show(
                    usuario.Bloqueado ? idiomaManager.Obtener("USUARIO_BLOQUEADO_OK") : idiomaManager.Obtener("USUARIO_DESBLOQUEADO_OK"),
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarUsuarios();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Bloquear / Desbloquear", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                ManejoErrores.MostrarError(ex);
                CargarUsuarios();
            }
        }

        private void NuevoUsuario()
        {
            try
            {
                using (var dialog = new Form())
                {
                    dialog.Text = "Nuevo usuario";
                    dialog.Width = 420;
                    dialog.Height = 220;
                    dialog.StartPosition = FormStartPosition.CenterParent;
                    dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                    dialog.MaximizeBox = false;
                    dialog.MinimizeBox = false;

                    Label lblUser = new Label() { Left = 20, Top = 20, Text = "Usuario:", Width = 100 };
                    TextBox txtUser = new TextBox() { Left = 130, Top = 18, Width = 250 };
                    Label lblPass = new Label() { Left = 20, Top = 55, Text = "Contraseña:", Width = 100 };
                    TextBox txtPass = new TextBox() { Left = 130, Top = 53, Width = 250, UseSystemPasswordChar = true };
                    Button btnOK = new Button() { Text = "Crear", Left = 200, Width = 80, Top = 110, DialogResult = DialogResult.OK };
                    Button btnCancelar = new Button() { Text = "Cancelar", Left = 290, Width = 80, Top = 110, DialogResult = DialogResult.Cancel };

                    dialog.Controls.AddRange(new Control[] { lblUser, txtUser, lblPass, txtPass, btnOK, btnCancelar });
                    dialog.AcceptButton = btnOK;
                    dialog.CancelButton = btnCancelar;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        if (string.IsNullOrWhiteSpace(txtUser.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
                        {
                            MessageBox.Show("Usuario y contraseña son obligatorios", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (txtPass.Text.Length <= 4)
                        {
                            MessageBox.Show("La contraseña debe tener más de 4 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        bllUsuario.CrearUsuario(txtUser.Text.Trim(), txtPass.Text);
                        MessageBox.Show("Usuario creado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarUsuarios();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                ManejoErrores.MostrarError(ex);
            }
        }

        private void EditarUsuario()
        {
            try
            {
                BE_USUARIO usuario = ObtenerUsuarioSeleccionado();
                if (usuario == null)
                {
                    MessageBox.Show(idiomaManager.Obtener("SELECCIONA_USUARIO"), "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var dialog = new Form())
                {
                    dialog.Text = $"Editar usuario: {usuario.Username}";
                    dialog.Width = 420;
                    dialog.Height = 240;
                    dialog.StartPosition = FormStartPosition.CenterParent;
                    dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                    dialog.MaximizeBox = false;
                    dialog.MinimizeBox = false;

                    Label lblUser = new Label() { Left = 20, Top = 20, Text = "Usuario:", Width = 100 };
                    TextBox txtUser = new TextBox() { Left = 130, Top = 18, Width = 250, Text = usuario.Username };
                    Label lblPass = new Label() { Left = 20, Top = 55, Text = "Nueva contraseña:", Width = 110 };
                    TextBox txtPass = new TextBox() { Left = 130, Top = 53, Width = 250, UseSystemPasswordChar = true };
                    Label lblHint = new Label() { Left = 130, Top = 80, Width = 250, Text = "(vacío = no cambiar)", ForeColor = Color.Gray };
                    Button btnOK = new Button() { Text = "Guardar", Left = 200, Width = 80, Top = 130, DialogResult = DialogResult.OK };
                    Button btnCancelar = new Button() { Text = "Cancelar", Left = 290, Width = 80, Top = 130, DialogResult = DialogResult.Cancel };

                    dialog.Controls.AddRange(new Control[] { lblUser, txtUser, lblPass, txtPass, lblHint, btnOK, btnCancelar });
                    dialog.AcceptButton = btnOK;
                    dialog.CancelButton = btnCancelar;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        bllUsuario.EditarUsuario(usuario.ID, txtUser.Text.Trim(),
                            string.IsNullOrWhiteSpace(txtPass.Text) ? null : txtPass.Text);
                        MessageBox.Show("Usuario actualizado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarUsuarios();
                    }
                }
            }
            catch (Exception ex)
            {
                ManejoErrores.MostrarError(ex);
            }
        }

        private void EliminarUsuario()
        {
            try
            {
                BE_USUARIO usuario = ObtenerUsuarioSeleccionado();
                if (usuario == null)
                {
                    MessageBox.Show(idiomaManager.Obtener("SELECCIONA_USUARIO"), "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (BLL_USUARIO.EsRolAdministrador(usuario.Rol))
                {
                    MessageBox.Show("No se puede eliminar a un usuario administrador.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show($"¿Eliminar al usuario '{usuario.Username}'?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;

                bllUsuario.EliminarUsuario(usuario.ID);
                MessageBox.Show("Usuario eliminado", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarUsuarios();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Eliminar usuario", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                ManejoErrores.MostrarError(ex);
            }
        }

        private void AsignarRolUsuario()
        {
            try
            {
                BE_USUARIO usuario = ObtenerUsuarioSeleccionado();
                if (usuario == null)
                {
                    MessageBox.Show("Selecciona un usuario", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var roles = bllRol.ObtenerRolesPrincipales();

                using (var dialog = new Form())
                {
                    dialog.Text = $"Asignar Rol a {usuario.Username}";
                    dialog.Width = 400;
                    dialog.Height = 200;
                    dialog.StartPosition = FormStartPosition.CenterParent;
                    dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                    dialog.MaximizeBox = false;
                    dialog.MinimizeBox = false;

                    Label lblRol = new Label() { Left = 20, Top = 20, Text = "Rol:", Width = 350 };
                    ComboBox cbRol = new ComboBox() { Left = 20, Top = 45, Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };

                    cbRol.DataSource = roles;
                    cbRol.DisplayMember = "Nombre";
                    cbRol.ValueMember = "ID";

                    if (!string.IsNullOrEmpty(usuario.Rol))
                    {
                        var rolActual = roles.FirstOrDefault(r => r.Nombre == usuario.Rol);
                        if (rolActual != null)
                            cbRol.SelectedValue = rolActual.ID;
                    }

                    Button btnOK = new Button() { Text = "Asignar", Left = 200, Width = 80, Top = 100, DialogResult = DialogResult.OK };
                    Button btnCancelar = new Button() { Text = "Cancelar", Left = 290, Width = 80, Top = 100, DialogResult = DialogResult.Cancel };

                    dialog.Controls.Add(lblRol);
                    dialog.Controls.Add(cbRol);
                    dialog.Controls.Add(btnOK);
                    dialog.Controls.Add(btnCancelar);
                    dialog.AcceptButton = btnOK;
                    dialog.CancelButton = btnCancelar;

                    if (dialog.ShowDialog() == DialogResult.OK && cbRol.SelectedValue is int idRol)
                    {
                        var rol = roles.FirstOrDefault(r => r.ID == idRol);
                        if (rol != null)
                        {
                            bllUsuario.AsignarRolUsuario(usuario.ID, rol.ID, rol.Nombre);
                            MessageBox.Show($"Rol '{rol.Nombre}' asignado a {usuario.Username}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarUsuarios();
                        }
                    }
                }
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
