namespace GUI
{
    partial class FormRoles
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelIzquierdo;
        private System.Windows.Forms.Panel panelBotones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Label lblRoles;
        private System.Windows.Forms.DataGridView dgvRoles;
        private System.Windows.Forms.Label lblDetalles;
        private System.Windows.Forms.Label lblNombreRol;
        private System.Windows.Forms.TextBox txtNombreRol;
        private System.Windows.Forms.Label lblDescripcionRol;
        private System.Windows.Forms.TextBox txtDescripcionRol;
        private System.Windows.Forms.Label lblComponentes;
        private System.Windows.Forms.DataGridView dgvComponentes;
        private System.Windows.Forms.Label lblComponentesDisponibles;
        private System.Windows.Forms.ComboBox cbComponentesDisponibles;
        private System.Windows.Forms.Button btnAgregarComponente;
        private System.Windows.Forms.Button btnDesasignarComponente;

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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelIzquierdo = new System.Windows.Forms.Panel();
            this.panelBotones = new System.Windows.Forms.Panel();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.lblRoles = new System.Windows.Forms.Label();
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.lblDetalles = new System.Windows.Forms.Label();
            this.lblNombreRol = new System.Windows.Forms.Label();
            this.txtNombreRol = new System.Windows.Forms.TextBox();
            this.lblDescripcionRol = new System.Windows.Forms.Label();
            this.txtDescripcionRol = new System.Windows.Forms.TextBox();
            this.lblComponentes = new System.Windows.Forms.Label();
            this.dgvComponentes = new System.Windows.Forms.DataGridView();
            this.lblComponentesDisponibles = new System.Windows.Forms.Label();
            this.cbComponentesDisponibles = new System.Windows.Forms.ComboBox();
            this.btnAgregarComponente = new System.Windows.Forms.Button();
            this.btnDesasignarComponente = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComponentes)).BeginInit();
            this.SuspendLayout();

            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Size = new System.Drawing.Size(1000, 600);
            this.splitContainer.SplitterDistance = 350;
            this.splitContainer.TabIndex = 0;

            this.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelIzquierdo.Location = new System.Drawing.Point(0, 0);
            this.panelIzquierdo.Name = "panelIzquierdo";
            this.panelIzquierdo.Size = new System.Drawing.Size(350, 600);
            this.panelIzquierdo.TabIndex = 0;

            this.lblRoles.AutoSize = true;
            this.lblRoles.Location = new System.Drawing.Point(12, 12);
            this.lblRoles.Name = "lblRoles";
            this.lblRoles.Size = new System.Drawing.Size(37, 13);
            this.lblRoles.TabIndex = 0;
            this.lblRoles.Text = "Roles:";

            this.dgvRoles.AllowUserToAddRows = false;
            this.dgvRoles.AllowUserToDeleteRows = false;
            this.dgvRoles.AutoGenerateColumns = false;
            this.dgvRoles.Location = new System.Drawing.Point(12, 30);
            this.dgvRoles.Name = "dgvRoles";
            this.dgvRoles.ReadOnly = true;
            this.dgvRoles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoles.Size = new System.Drawing.Size(326, 390);
            this.dgvRoles.TabIndex = 1;
            this.dgvRoles.SelectionChanged += new System.EventHandler(this.dgvRoles_SelectionChanged);

            this.panelBotones.Location = new System.Drawing.Point(12, 430);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new System.Drawing.Size(326, 40);
            this.panelBotones.TabIndex = 2;

            this.btnNuevo.Location = new System.Drawing.Point(0, 0);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(100, 35);
            this.btnNuevo.TabIndex = 0;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);

            this.btnEditar.Location = new System.Drawing.Point(110, 0);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(100, 35);
            this.btnEditar.TabIndex = 1;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);

            this.btnEliminar.Location = new System.Drawing.Point(220, 0);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(100, 35);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            this.panelBotones.Controls.Add(this.btnNuevo);
            this.panelBotones.Controls.Add(this.btnEditar);
            this.panelBotones.Controls.Add(this.btnEliminar);
            this.panelIzquierdo.Controls.Add(this.panelBotones);
            this.panelIzquierdo.Controls.Add(this.dgvRoles);
            this.panelIzquierdo.Controls.Add(this.lblRoles);
            this.splitContainer.Panel1.Controls.Add(this.panelIzquierdo);

            this.lblDetalles.AutoSize = true;
            this.lblDetalles.Location = new System.Drawing.Point(12, 12);
            this.lblDetalles.Name = "lblDetalles";
            this.lblDetalles.Size = new System.Drawing.Size(58, 13);
            this.lblDetalles.TabIndex = 0;
            this.lblDetalles.Text = "Detalles:";

            this.lblNombreRol.AutoSize = true;
            this.lblNombreRol.Location = new System.Drawing.Point(12, 40);
            this.lblNombreRol.Name = "lblNombreRol";
            this.lblNombreRol.Size = new System.Drawing.Size(47, 13);
            this.lblNombreRol.TabIndex = 1;
            this.lblNombreRol.Text = "Nombre:";

            this.txtNombreRol.Location = new System.Drawing.Point(12, 56);
            this.txtNombreRol.Name = "txtNombreRol";
            this.txtNombreRol.ReadOnly = true;
            this.txtNombreRol.Size = new System.Drawing.Size(400, 20);
            this.txtNombreRol.TabIndex = 2;

            this.lblDescripcionRol.AutoSize = true;
            this.lblDescripcionRol.Location = new System.Drawing.Point(12, 85);
            this.lblDescripcionRol.Name = "lblDescripcionRol";
            this.lblDescripcionRol.Size = new System.Drawing.Size(66, 13);
            this.lblDescripcionRol.TabIndex = 3;
            this.lblDescripcionRol.Text = "Descripción:";

            this.txtDescripcionRol.Location = new System.Drawing.Point(12, 101);
            this.txtDescripcionRol.Name = "txtDescripcionRol";
            this.txtDescripcionRol.ReadOnly = true;
            this.txtDescripcionRol.Multiline = true;
            this.txtDescripcionRol.Size = new System.Drawing.Size(400, 50);
            this.txtDescripcionRol.TabIndex = 4;

            this.lblComponentes.AutoSize = true;
            this.lblComponentes.Location = new System.Drawing.Point(12, 165);
            this.lblComponentes.Name = "lblComponentes";
            this.lblComponentes.Size = new System.Drawing.Size(79, 13);
            this.lblComponentes.TabIndex = 5;
            this.lblComponentes.Text = "Permisos:";

            this.dgvComponentes.AllowUserToAddRows = false;
            this.dgvComponentes.AllowUserToDeleteRows = false;
            this.dgvComponentes.AutoGenerateColumns = false;
            this.dgvComponentes.Location = new System.Drawing.Point(12, 181);
            this.dgvComponentes.Name = "dgvComponentes";
            this.dgvComponentes.ReadOnly = true;
            this.dgvComponentes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComponentes.Size = new System.Drawing.Size(400, 180);
            this.dgvComponentes.TabIndex = 6;

            this.lblComponentesDisponibles.AutoSize = true;
            this.lblComponentesDisponibles.Location = new System.Drawing.Point(12, 370);
            this.lblComponentesDisponibles.Name = "lblComponentesDisponibles";
            this.lblComponentesDisponibles.Size = new System.Drawing.Size(105, 13);
            this.lblComponentesDisponibles.TabIndex = 7;
            this.lblComponentesDisponibles.Text = "Agregar permiso:";

            this.cbComponentesDisponibles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbComponentesDisponibles.FormattingEnabled = true;
            this.cbComponentesDisponibles.Location = new System.Drawing.Point(12, 390);
            this.cbComponentesDisponibles.Name = "cbComponentesDisponibles";
            this.cbComponentesDisponibles.Size = new System.Drawing.Size(400, 21);
            this.cbComponentesDisponibles.TabIndex = 8;

            this.btnAgregarComponente.Location = new System.Drawing.Point(12, 420);
            this.btnAgregarComponente.Name = "btnAgregarComponente";
            this.btnAgregarComponente.Size = new System.Drawing.Size(195, 30);
            this.btnAgregarComponente.TabIndex = 9;
            this.btnAgregarComponente.Text = "Agregar";
            this.btnAgregarComponente.UseVisualStyleBackColor = true;
            this.btnAgregarComponente.Click += new System.EventHandler(this.btnAgregarComponente_Click);

            this.btnDesasignarComponente.Location = new System.Drawing.Point(217, 420);
            this.btnDesasignarComponente.Name = "btnDesasignarComponente";
            this.btnDesasignarComponente.Size = new System.Drawing.Size(195, 30);
            this.btnDesasignarComponente.TabIndex = 10;
            this.btnDesasignarComponente.Text = "Quitar";
            this.btnDesasignarComponente.UseVisualStyleBackColor = true;
            this.btnDesasignarComponente.Click += new System.EventHandler(this.btnDesasignarComponente_Click);

            this.splitContainer.Panel2.Controls.Add(this.btnDesasignarComponente);
            this.splitContainer.Panel2.Controls.Add(this.btnAgregarComponente);
            this.splitContainer.Panel2.Controls.Add(this.cbComponentesDisponibles);
            this.splitContainer.Panel2.Controls.Add(this.lblComponentesDisponibles);
            this.splitContainer.Panel2.Controls.Add(this.dgvComponentes);
            this.splitContainer.Panel2.Controls.Add(this.lblComponentes);
            this.splitContainer.Panel2.Controls.Add(this.txtDescripcionRol);
            this.splitContainer.Panel2.Controls.Add(this.lblDescripcionRol);
            this.splitContainer.Panel2.Controls.Add(this.txtNombreRol);
            this.splitContainer.Panel2.Controls.Add(this.lblNombreRol);
            this.splitContainer.Panel2.Controls.Add(this.lblDetalles);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.splitContainer);
            this.Name = "FormRoles";
            this.Text = "Roles";

            ((System.ComponentModel.ISupportInitialize)(this.dgvComponentes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
