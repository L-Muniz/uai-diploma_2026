namespace GUI
{
    partial class FormRoles
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelBotones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Panel panelIzquierdo;
        private System.Windows.Forms.Label lblRoles;
        private System.Windows.Forms.TreeView tvRoles;
        private System.Windows.Forms.Panel panelDerecho;
        private System.Windows.Forms.Label lblDetalles;
        private System.Windows.Forms.Label lblNombreRol;
        private System.Windows.Forms.TextBox txtNombreRol;
        private System.Windows.Forms.Label lblDescripcionRol;
        private System.Windows.Forms.TextBox txtDescripcionRol;
        private System.Windows.Forms.Label lblComponentesDisponibles;
        private System.Windows.Forms.ComboBox cbComponentesDisponibles;
        private System.Windows.Forms.Button btnAgregarComponente;
        private System.Windows.Forms.Button btnDesasignarComponente;
        private System.Windows.Forms.Label lblComponentes;
        private System.Windows.Forms.DataGridView dgvComponentes;

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
            this.panelIzquierdo = new System.Windows.Forms.Panel();
            this.tvRoles = new System.Windows.Forms.TreeView();
            this.lblRoles = new System.Windows.Forms.Label();
            this.panelDerecho = new System.Windows.Forms.Panel();
            this.lblDetalles = new System.Windows.Forms.Label();
            this.lblNombreRol = new System.Windows.Forms.Label();
            this.txtNombreRol = new System.Windows.Forms.TextBox();
            this.lblDescripcionRol = new System.Windows.Forms.Label();
            this.txtDescripcionRol = new System.Windows.Forms.TextBox();
            this.lblComponentesDisponibles = new System.Windows.Forms.Label();
            this.cbComponentesDisponibles = new System.Windows.Forms.ComboBox();
            this.btnAgregarComponente = new System.Windows.Forms.Button();
            this.btnDesasignarComponente = new System.Windows.Forms.Button();
            this.lblComponentes = new System.Windows.Forms.Label();
            this.dgvComponentes = new System.Windows.Forms.DataGridView();
            this.panelBotones.SuspendLayout();
            this.panelIzquierdo.SuspendLayout();
            this.panelDerecho.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComponentes)).BeginInit();
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
            this.btnNuevo.Size = new System.Drawing.Size(110, 32);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Nuevo Rol";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);

            this.btnEditar.Location = new System.Drawing.Point(130, 13);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(80, 32);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);

            this.btnEliminar.Location = new System.Drawing.Point(218, 13);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(80, 32);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            this.panelIzquierdo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelIzquierdo.Controls.Add(this.tvRoles);
            this.panelIzquierdo.Controls.Add(this.lblRoles);
            this.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIzquierdo.Location = new System.Drawing.Point(0, 52);
            this.panelIzquierdo.Name = "panelIzquierdo";
            this.panelIzquierdo.Size = new System.Drawing.Size(300, 509);
            this.panelIzquierdo.TabIndex = 1;

            this.tvRoles.AllowDrop = true;
            this.tvRoles.HideSelection = false;
            this.tvRoles.Location = new System.Drawing.Point(9, 30);
            this.tvRoles.Name = "tvRoles";
            this.tvRoles.Size = new System.Drawing.Size(283, 469);
            this.tvRoles.TabIndex = 1;
            this.tvRoles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvRoles_AfterSelect);
            this.tvRoles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvRoles_ItemDrag);
            this.tvRoles.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvRoles_DragEnter);
            this.tvRoles.DragOver += new System.Windows.Forms.DragEventHandler(this.tvRoles_DragOver);
            this.tvRoles.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvRoles_DragDrop);

            this.lblRoles.AutoSize = true;
            this.lblRoles.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblRoles.Location = new System.Drawing.Point(9, 9);
            this.lblRoles.Name = "lblRoles";
            this.lblRoles.Size = new System.Drawing.Size(140, 16);
            this.lblRoles.TabIndex = 0;
            this.lblRoles.Text = "Jerarquía de Roles";

            this.panelDerecho.BackColor = System.Drawing.Color.White;
            this.panelDerecho.Controls.Add(this.lblDetalles);
            this.panelDerecho.Controls.Add(this.lblNombreRol);
            this.panelDerecho.Controls.Add(this.txtNombreRol);
            this.panelDerecho.Controls.Add(this.lblDescripcionRol);
            this.panelDerecho.Controls.Add(this.txtDescripcionRol);
            this.panelDerecho.Controls.Add(this.lblComponentesDisponibles);
            this.panelDerecho.Controls.Add(this.cbComponentesDisponibles);
            this.panelDerecho.Controls.Add(this.btnAgregarComponente);
            this.panelDerecho.Controls.Add(this.btnDesasignarComponente);
            this.panelDerecho.Controls.Add(this.lblComponentes);
            this.panelDerecho.Controls.Add(this.dgvComponentes);
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
            this.lblDetalles.Size = new System.Drawing.Size(118, 16);
            this.lblDetalles.TabIndex = 0;
            this.lblDetalles.Text = "Detalles del Rol";

            this.lblNombreRol.AutoSize = true;
            this.lblNombreRol.Location = new System.Drawing.Point(9, 35);
            this.lblNombreRol.Name = "lblNombreRol";
            this.lblNombreRol.Size = new System.Drawing.Size(47, 13);
            this.lblNombreRol.TabIndex = 1;
            this.lblNombreRol.Text = "Nombre:";

            this.txtNombreRol.Location = new System.Drawing.Point(86, 35);
            this.txtNombreRol.Name = "txtNombreRol";
            this.txtNombreRol.Size = new System.Drawing.Size(258, 20);
            this.txtNombreRol.TabIndex = 2;
            this.txtNombreRol.ReadOnly = true;

            this.lblDescripcionRol.AutoSize = true;
            this.lblDescripcionRol.Location = new System.Drawing.Point(9, 61);
            this.lblDescripcionRol.Name = "lblDescripcionRol";
            this.lblDescripcionRol.Size = new System.Drawing.Size(66, 13);
            this.lblDescripcionRol.TabIndex = 3;
            this.lblDescripcionRol.Text = "Descripción:";

            this.txtDescripcionRol.Location = new System.Drawing.Point(86, 61);
            this.txtDescripcionRol.Multiline = true;
            this.txtDescripcionRol.Name = "txtDescripcionRol";
            this.txtDescripcionRol.ReadOnly = true;
            this.txtDescripcionRol.Size = new System.Drawing.Size(258, 53);
            this.txtDescripcionRol.TabIndex = 4;

            this.lblComponentesDisponibles.AutoSize = true;
            this.lblComponentesDisponibles.Location = new System.Drawing.Point(9, 128);
            this.lblComponentesDisponibles.Name = "lblComponentesDisponibles";
            this.lblComponentesDisponibles.Size = new System.Drawing.Size(108, 13);
            this.lblComponentesDisponibles.TabIndex = 5;
            this.lblComponentesDisponibles.Text = "Agregar componente:";

            this.cbComponentesDisponibles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbComponentesDisponibles.FormattingEnabled = true;
            this.cbComponentesDisponibles.Location = new System.Drawing.Point(12, 146);
            this.cbComponentesDisponibles.Name = "cbComponentesDisponibles";
            this.cbComponentesDisponibles.Size = new System.Drawing.Size(332, 21);
            this.cbComponentesDisponibles.TabIndex = 6;

            this.btnAgregarComponente.Location = new System.Drawing.Point(350, 144);
            this.btnAgregarComponente.Name = "btnAgregarComponente";
            this.btnAgregarComponente.Size = new System.Drawing.Size(90, 26);
            this.btnAgregarComponente.TabIndex = 7;
            this.btnAgregarComponente.Text = "Agregar";
            this.btnAgregarComponente.UseVisualStyleBackColor = true;
            this.btnAgregarComponente.Click += new System.EventHandler(this.btnAgregarComponente_Click);

            this.btnDesasignarComponente.Location = new System.Drawing.Point(446, 144);
            this.btnDesasignarComponente.Name = "btnDesasignarComponente";
            this.btnDesasignarComponente.Size = new System.Drawing.Size(100, 26);
            this.btnDesasignarComponente.TabIndex = 8;
            this.btnDesasignarComponente.Text = "Desasignar";
            this.btnDesasignarComponente.UseVisualStyleBackColor = true;
            this.btnDesasignarComponente.Click += new System.EventHandler(this.btnDesasignarComponente_Click);

            this.lblComponentes.AutoSize = true;
            this.lblComponentes.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblComponentes.Location = new System.Drawing.Point(9, 180);
            this.lblComponentes.Name = "lblComponentes";
            this.lblComponentes.Size = new System.Drawing.Size(200, 15);
            this.lblComponentes.TabIndex = 9;
            this.lblComponentes.Text = "Componentes del rol:";

            this.dgvComponentes.AllowUserToAddRows = false;
            this.dgvComponentes.AllowUserToDeleteRows = false;
            this.dgvComponentes.AllowUserToResizeRows = false;
            this.dgvComponentes.BackgroundColor = System.Drawing.Color.White;
            this.dgvComponentes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComponentes.Location = new System.Drawing.Point(9, 200);
            this.dgvComponentes.Name = "dgvComponentes";
            this.dgvComponentes.ReadOnly = true;
            this.dgvComponentes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComponentes.Size = new System.Drawing.Size(617, 290);
            this.dgvComponentes.TabIndex = 10;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(943, 561);
            this.Controls.Add(this.panelDerecho);
            this.Controls.Add(this.panelIzquierdo);
            this.Controls.Add(this.panelBotones);
            this.Name = "FormRoles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Roles";
            this.panelBotones.ResumeLayout(false);
            this.panelIzquierdo.ResumeLayout(false);
            this.panelIzquierdo.PerformLayout();
            this.panelDerecho.ResumeLayout(false);
            this.panelDerecho.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComponentes)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
