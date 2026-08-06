namespace GUI
{
    partial class FormPerfiles
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelBotones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnAgregarPermiso;
        private System.Windows.Forms.Button btnEliminarPermiso;
        private System.Windows.Forms.Panel panelIzquierdo;
        private System.Windows.Forms.Label lblPerfiles;
        private System.Windows.Forms.TreeView tvPerfiles;
        private System.Windows.Forms.Panel panelDerecho;
        private System.Windows.Forms.Label lblDetalles;
        private System.Windows.Forms.Label lblNombrePerfil;
        private System.Windows.Forms.TextBox txtNombrePerfil;
        private System.Windows.Forms.Label lblDescripcionPerfil;
        private System.Windows.Forms.TextBox txtDescripcionPerfil;
        private System.Windows.Forms.Label lblPermisosLabel;
        private System.Windows.Forms.DataGridView dgvPermisos;
        private System.Windows.Forms.Label lblPermisosDisponibles;
        private System.Windows.Forms.ComboBox cbPermisosDisponibles;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelBotones = new System.Windows.Forms.Panel();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnAgregarPermiso = new System.Windows.Forms.Button();
            this.btnEliminarPermiso = new System.Windows.Forms.Button();
            this.panelIzquierdo = new System.Windows.Forms.Panel();
            this.tvPerfiles = new System.Windows.Forms.TreeView();
            this.lblPerfiles = new System.Windows.Forms.Label();
            this.panelDerecho = new System.Windows.Forms.Panel();
            this.lblDetalles = new System.Windows.Forms.Label();
            this.lblNombrePerfil = new System.Windows.Forms.Label();
            this.txtNombrePerfil = new System.Windows.Forms.TextBox();
            this.lblDescripcionPerfil = new System.Windows.Forms.Label();
            this.txtDescripcionPerfil = new System.Windows.Forms.TextBox();
            this.lblPermisosLabel = new System.Windows.Forms.Label();
            this.dgvPermisos = new System.Windows.Forms.DataGridView();
            this.lblPermisosDisponibles = new System.Windows.Forms.Label();
            this.cbPermisosDisponibles = new System.Windows.Forms.ComboBox();
            this.panelBotones.SuspendLayout();
            this.panelIzquierdo.SuspendLayout();
            this.panelDerecho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPermisos)).BeginInit();
            this.SuspendLayout();

            this.panelBotones.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelBotones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBotones.Controls.Add(this.btnNuevo);
            this.panelBotones.Controls.Add(this.btnEditar);
            this.panelBotones.Controls.Add(this.btnEliminar);
            this.panelBotones.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBotones.Location = new System.Drawing.Point(0, 0);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new System.Drawing.Size(943, 58);
            this.panelBotones.TabIndex = 0;

            this.btnNuevo.Location = new System.Drawing.Point(12, 13);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(120, 32);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Nuevo Perfil";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);

            this.btnEditar.Location = new System.Drawing.Point(140, 13);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(80, 32);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);

            this.btnEliminar.Location = new System.Drawing.Point(228, 13);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(80, 32);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            this.btnAgregarPermiso.Location = new System.Drawing.Point(350, 144);
            this.btnAgregarPermiso.Name = "btnAgregarPermiso";
            this.btnAgregarPermiso.Size = new System.Drawing.Size(90, 26);
            this.btnAgregarPermiso.TabIndex = 3;
            this.btnAgregarPermiso.Text = "Agregar";
            this.btnAgregarPermiso.UseVisualStyleBackColor = true;
            this.btnAgregarPermiso.Click += new System.EventHandler(this.btnAgregarPermiso_Click);

            this.btnEliminarPermiso.Location = new System.Drawing.Point(446, 144);
            this.btnEliminarPermiso.Name = "btnEliminarPermiso";
            this.btnEliminarPermiso.Size = new System.Drawing.Size(100, 26);
            this.btnEliminarPermiso.TabIndex = 4;
            this.btnEliminarPermiso.Text = "Desasignar";
            this.btnEliminarPermiso.UseVisualStyleBackColor = true;
            this.btnEliminarPermiso.Click += new System.EventHandler(this.btnEliminarPermiso_Click);

            this.panelIzquierdo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelIzquierdo.Controls.Add(this.tvPerfiles);
            this.panelIzquierdo.Controls.Add(this.lblPerfiles);
            this.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIzquierdo.Location = new System.Drawing.Point(0, 52);
            this.panelIzquierdo.Name = "panelIzquierdo";
            this.panelIzquierdo.Size = new System.Drawing.Size(300, 509);
            this.panelIzquierdo.TabIndex = 1;

            this.tvPerfiles.AllowDrop = true;
            this.tvPerfiles.HideSelection = false;
            this.tvPerfiles.Location = new System.Drawing.Point(9, 30);
            this.tvPerfiles.Name = "tvPerfiles";
            this.tvPerfiles.Size = new System.Drawing.Size(283, 469);
            this.tvPerfiles.TabIndex = 1;
            this.tvPerfiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPerfiles_AfterSelect);
            this.tvPerfiles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvPerfiles_ItemDrag);
            this.tvPerfiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvPerfiles_DragEnter);
            this.tvPerfiles.DragOver += new System.Windows.Forms.DragEventHandler(this.tvPerfiles_DragOver);
            this.tvPerfiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvPerfiles_DragDrop);

            this.lblPerfiles.AutoSize = true;
            this.lblPerfiles.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblPerfiles.Location = new System.Drawing.Point(9, 9);
            this.lblPerfiles.Name = "lblPerfiles";
            this.lblPerfiles.Size = new System.Drawing.Size(154, 16);
            this.lblPerfiles.TabIndex = 0;
            this.lblPerfiles.Text = "Jerarquía de Perfiles";

            this.panelDerecho.BackColor = System.Drawing.Color.White;
            this.panelDerecho.Controls.Add(this.lblDetalles);
            this.panelDerecho.Controls.Add(this.lblNombrePerfil);
            this.panelDerecho.Controls.Add(this.txtNombrePerfil);
            this.panelDerecho.Controls.Add(this.lblDescripcionPerfil);
            this.panelDerecho.Controls.Add(this.txtDescripcionPerfil);
            this.panelDerecho.Controls.Add(this.lblPermisosDisponibles);
            this.panelDerecho.Controls.Add(this.cbPermisosDisponibles);
            this.panelDerecho.Controls.Add(this.btnAgregarPermiso);
            this.panelDerecho.Controls.Add(this.btnEliminarPermiso);
            this.panelDerecho.Controls.Add(this.lblPermisosLabel);
            this.panelDerecho.Controls.Add(this.dgvPermisos);
            this.panelDerecho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDerecho.Location = new System.Drawing.Point(300, 52);
            this.panelDerecho.Name = "panelDerecho";
            this.panelDerecho.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
            this.panelDerecho.Size = new System.Drawing.Size(643, 509);
            this.panelDerecho.TabIndex = 2;

            this.lblDetalles.AutoSize = true;
            this.lblDetalles.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblDetalles.Location = new System.Drawing.Point(9, 9);
            this.lblDetalles.Name = "lblDetalles";
            this.lblDetalles.Size = new System.Drawing.Size(131, 16);
            this.lblDetalles.TabIndex = 0;
            this.lblDetalles.Text = "Detalles del Perfil";

            this.lblNombrePerfil.AutoSize = true;
            this.lblNombrePerfil.Location = new System.Drawing.Point(9, 35);
            this.lblNombrePerfil.Name = "lblNombrePerfil";
            this.lblNombrePerfil.Size = new System.Drawing.Size(47, 13);
            this.lblNombrePerfil.TabIndex = 1;
            this.lblNombrePerfil.Text = "Nombre:";

            this.txtNombrePerfil.Location = new System.Drawing.Point(86, 35);
            this.txtNombrePerfil.Name = "txtNombrePerfil";
            this.txtNombrePerfil.ReadOnly = true;
            this.txtNombrePerfil.Size = new System.Drawing.Size(258, 20);
            this.txtNombrePerfil.TabIndex = 2;

            this.lblDescripcionPerfil.AutoSize = true;
            this.lblDescripcionPerfil.Location = new System.Drawing.Point(9, 61);
            this.lblDescripcionPerfil.Name = "lblDescripcionPerfil";
            this.lblDescripcionPerfil.Size = new System.Drawing.Size(66, 13);
            this.lblDescripcionPerfil.TabIndex = 3;
            this.lblDescripcionPerfil.Text = "Descripción:";

            this.txtDescripcionPerfil.Location = new System.Drawing.Point(86, 61);
            this.txtDescripcionPerfil.Multiline = true;
            this.txtDescripcionPerfil.Name = "txtDescripcionPerfil";
            this.txtDescripcionPerfil.ReadOnly = true;
            this.txtDescripcionPerfil.Size = new System.Drawing.Size(258, 53);
            this.txtDescripcionPerfil.TabIndex = 4;

            this.lblPermisosDisponibles.AutoSize = true;
            this.lblPermisosDisponibles.Location = new System.Drawing.Point(9, 128);
            this.lblPermisosDisponibles.Name = "lblPermisosDisponibles";
            this.lblPermisosDisponibles.Size = new System.Drawing.Size(87, 13);
            this.lblPermisosDisponibles.TabIndex = 5;
            this.lblPermisosDisponibles.Text = "Agregar Permiso:";

            this.cbPermisosDisponibles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPermisosDisponibles.FormattingEnabled = true;
            this.cbPermisosDisponibles.Location = new System.Drawing.Point(12, 146);
            this.cbPermisosDisponibles.Name = "cbPermisosDisponibles";
            this.cbPermisosDisponibles.Size = new System.Drawing.Size(332, 21);
            this.cbPermisosDisponibles.TabIndex = 6;

            this.lblPermisosLabel.AutoSize = true;
            this.lblPermisosLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblPermisosLabel.Location = new System.Drawing.Point(9, 180);
            this.lblPermisosLabel.Name = "lblPermisosLabel";
            this.lblPermisosLabel.Size = new System.Drawing.Size(118, 15);
            this.lblPermisosLabel.TabIndex = 7;
            this.lblPermisosLabel.Text = "Permisos del Perfil:";

            this.dgvPermisos.AllowUserToAddRows = false;
            this.dgvPermisos.AllowUserToDeleteRows = false;
            this.dgvPermisos.AllowUserToResizeRows = false;
            this.dgvPermisos.BackgroundColor = System.Drawing.Color.White;
            this.dgvPermisos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPermisos.Location = new System.Drawing.Point(9, 200);
            this.dgvPermisos.Name = "dgvPermisos";
            this.dgvPermisos.ReadOnly = true;
            this.dgvPermisos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPermisos.Size = new System.Drawing.Size(617, 290);
            this.dgvPermisos.TabIndex = 8;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(943, 561);
            this.Controls.Add(this.panelDerecho);
            this.Controls.Add(this.panelIzquierdo);
            this.Controls.Add(this.panelBotones);
            this.Name = "FormPerfiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Perfiles";
            this.panelBotones.ResumeLayout(false);
            this.panelIzquierdo.ResumeLayout(false);
            this.panelIzquierdo.PerformLayout();
            this.panelDerecho.ResumeLayout(false);
            this.panelDerecho.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPermisos)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
