namespace GUI
{
    partial class FormPermisos
    {
        private System.ComponentModel.IContainer components = null;
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
            this.lblPermisosDisponibles = new System.Windows.Forms.Label();
            this.cbPermisosDisponibles = new System.Windows.Forms.ComboBox();
            this.lblPermisosLabel = new System.Windows.Forms.Label();
            this.dgvPermisos = new System.Windows.Forms.DataGridView();
            this.panelIzquierdo.SuspendLayout();
            this.panelDerecho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPermisos)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAgregarPermiso
            // 
            this.btnAgregarPermiso.Location = new System.Drawing.Point(441, 35);
            this.btnAgregarPermiso.Name = "btnAgregarPermiso";
            this.btnAgregarPermiso.Size = new System.Drawing.Size(103, 26);
            this.btnAgregarPermiso.TabIndex = 3;
            this.btnAgregarPermiso.Text = "Agregar Permiso";
            this.btnAgregarPermiso.UseVisualStyleBackColor = true;
            this.btnAgregarPermiso.Click += new System.EventHandler(this.btnAgregarPermiso_Click);
            // 
            // btnEliminarPermiso
            // 
            this.btnEliminarPermiso.Location = new System.Drawing.Point(441, 88);
            this.btnEliminarPermiso.Name = "btnEliminarPermiso";
            this.btnEliminarPermiso.Size = new System.Drawing.Size(103, 26);
            this.btnEliminarPermiso.TabIndex = 4;
            this.btnEliminarPermiso.Text = "Quitar Permiso";
            this.btnEliminarPermiso.UseVisualStyleBackColor = true;
            this.btnEliminarPermiso.Click += new System.EventHandler(this.btnEliminarPermiso_Click);
            // 
            // panelIzquierdo
            // 
            this.panelIzquierdo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelIzquierdo.Controls.Add(this.tvPerfiles);
            this.panelIzquierdo.Controls.Add(this.lblPerfiles);
            this.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIzquierdo.Location = new System.Drawing.Point(0, 0);
            this.panelIzquierdo.Name = "panelIzquierdo";
            this.panelIzquierdo.Size = new System.Drawing.Size(300, 561);
            this.panelIzquierdo.TabIndex = 1;
            // 
            // tvPerfiles
            // 
            this.tvPerfiles.Location = new System.Drawing.Point(9, 30);
            this.tvPerfiles.Name = "tvPerfiles";
            this.tvPerfiles.Size = new System.Drawing.Size(283, 469);
            this.tvPerfiles.TabIndex = 1;
            this.tvPerfiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPerfiles_AfterSelect);
            // 
            // lblPerfiles
            // 
            this.lblPerfiles.AutoSize = true;
            this.lblPerfiles.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblPerfiles.Location = new System.Drawing.Point(9, 9);
            this.lblPerfiles.Name = "lblPerfiles";
            this.lblPerfiles.Size = new System.Drawing.Size(140, 16);
            this.lblPerfiles.TabIndex = 0;
            this.lblPerfiles.Text = "Jerarquía de Roles";
            // 
            // panelDerecho
            // 
            this.panelDerecho.BackColor = System.Drawing.Color.White;
            this.panelDerecho.Controls.Add(this.btnEliminarPermiso);
            this.panelDerecho.Controls.Add(this.btnAgregarPermiso);
            this.panelDerecho.Controls.Add(this.lblDetalles);
            this.panelDerecho.Controls.Add(this.lblNombrePerfil);
            this.panelDerecho.Controls.Add(this.txtNombrePerfil);
            this.panelDerecho.Controls.Add(this.lblDescripcionPerfil);
            this.panelDerecho.Controls.Add(this.txtDescripcionPerfil);
            this.panelDerecho.Controls.Add(this.lblPermisosDisponibles);
            this.panelDerecho.Controls.Add(this.cbPermisosDisponibles);
            this.panelDerecho.Controls.Add(this.lblPermisosLabel);
            this.panelDerecho.Controls.Add(this.dgvPermisos);
            this.panelDerecho.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDerecho.Location = new System.Drawing.Point(300, 0);
            this.panelDerecho.Name = "panelDerecho";
            this.panelDerecho.Padding = new System.Windows.Forms.Padding(9);
            this.panelDerecho.Size = new System.Drawing.Size(643, 561);
            this.panelDerecho.TabIndex = 2;
            // 
            // lblDetalles
            // 
            this.lblDetalles.AutoSize = true;
            this.lblDetalles.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblDetalles.Location = new System.Drawing.Point(9, 9);
            this.lblDetalles.Name = "lblDetalles";
            this.lblDetalles.Size = new System.Drawing.Size(117, 16);
            this.lblDetalles.TabIndex = 0;
            this.lblDetalles.Text = "Detalles del Rol";
            // 
            // lblNombrePerfil
            // 
            this.lblNombrePerfil.AutoSize = true;
            this.lblNombrePerfil.Location = new System.Drawing.Point(9, 35);
            this.lblNombrePerfil.Name = "lblNombrePerfil";
            this.lblNombrePerfil.Size = new System.Drawing.Size(47, 13);
            this.lblNombrePerfil.TabIndex = 1;
            this.lblNombrePerfil.Text = "Nombre:";
            // 
            // txtNombrePerfil
            // 
            this.txtNombrePerfil.Location = new System.Drawing.Point(102, 35);
            this.txtNombrePerfil.Name = "txtNombrePerfil";
            this.txtNombrePerfil.Size = new System.Drawing.Size(258, 20);
            this.txtNombrePerfil.TabIndex = 2;
            // 
            // lblDescripcionPerfil
            // 
            this.lblDescripcionPerfil.AutoSize = true;
            this.lblDescripcionPerfil.Location = new System.Drawing.Point(9, 61);
            this.lblDescripcionPerfil.Name = "lblDescripcionPerfil";
            this.lblDescripcionPerfil.Size = new System.Drawing.Size(66, 13);
            this.lblDescripcionPerfil.TabIndex = 3;
            this.lblDescripcionPerfil.Text = "Descripción:";
            // 
            // txtDescripcionPerfil
            // 
            this.txtDescripcionPerfil.Location = new System.Drawing.Point(102, 61);
            this.txtDescripcionPerfil.Multiline = true;
            this.txtDescripcionPerfil.Name = "txtDescripcionPerfil";
            this.txtDescripcionPerfil.ReadOnly = true;
            this.txtDescripcionPerfil.Size = new System.Drawing.Size(258, 53);
            this.txtDescripcionPerfil.TabIndex = 4;
            // 
            // lblPermisosDisponibles
            // 
            this.lblPermisosDisponibles.AutoSize = true;
            this.lblPermisosDisponibles.Location = new System.Drawing.Point(9, 138);
            this.lblPermisosDisponibles.Name = "lblPermisosDisponibles";
            this.lblPermisosDisponibles.Size = new System.Drawing.Size(87, 13);
            this.lblPermisosDisponibles.TabIndex = 5;
            this.lblPermisosDisponibles.Text = "Agregar Permiso:";
            // 
            // cbPermisosDisponibles
            // 
            this.cbPermisosDisponibles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPermisosDisponibles.FormattingEnabled = true;
            this.cbPermisosDisponibles.Location = new System.Drawing.Point(102, 135);
            this.cbPermisosDisponibles.Name = "cbPermisosDisponibles";
            this.cbPermisosDisponibles.Size = new System.Drawing.Size(258, 21);
            this.cbPermisosDisponibles.TabIndex = 6;
            // 
            // lblPermisosLabel
            // 
            this.lblPermisosLabel.AutoSize = true;
            this.lblPermisosLabel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblPermisosLabel.Location = new System.Drawing.Point(9, 168);
            this.lblPermisosLabel.Name = "lblPermisosLabel";
            this.lblPermisosLabel.Size = new System.Drawing.Size(106, 15);
            this.lblPermisosLabel.TabIndex = 7;
            this.lblPermisosLabel.Text = "Permisos del Rol:";
            // 
            // dgvPermisos
            // 
            this.dgvPermisos.AllowUserToAddRows = false;
            this.dgvPermisos.AllowUserToDeleteRows = false;
            this.dgvPermisos.AllowUserToResizeRows = false;
            this.dgvPermisos.BackgroundColor = System.Drawing.Color.White;
            this.dgvPermisos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPermisos.Location = new System.Drawing.Point(9, 190);
            this.dgvPermisos.Name = "dgvPermisos";
            this.dgvPermisos.ReadOnly = true;
            this.dgvPermisos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPermisos.Size = new System.Drawing.Size(617, 290);
            this.dgvPermisos.TabIndex = 8;
            // 
            // FormPermisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(943, 561);
            this.Controls.Add(this.panelDerecho);
            this.Controls.Add(this.panelIzquierdo);
            this.Name = "FormPermisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Permisos";
            this.panelIzquierdo.ResumeLayout(false);
            this.panelIzquierdo.PerformLayout();
            this.panelDerecho.ResumeLayout(false);
            this.panelDerecho.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPermisos)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
