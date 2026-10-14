namespace GUI
{
    partial class FormPermisoABM
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView tvPermisos;
        private System.Windows.Forms.Panel panelDetalles;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.CheckBox chkCompuesto;
        private System.Windows.Forms.Label lblComponentes;
        private System.Windows.Forms.DataGridView dgvComponentes;
        private System.Windows.Forms.Button btnAgregarComponente;
        private System.Windows.Forms.Button btnQuitarComponente;
        private System.Windows.Forms.Button btnNuevoCompuesto;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Panel panelBotones;

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
            this.tvPermisos = new System.Windows.Forms.TreeView();
            this.panelDetalles = new System.Windows.Forms.Panel();
            this.panelBotones = new System.Windows.Forms.Panel();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.chkCompuesto = new System.Windows.Forms.CheckBox();
            this.lblComponentes = new System.Windows.Forms.Label();
            this.dgvComponentes = new System.Windows.Forms.DataGridView();
            this.btnAgregarComponente = new System.Windows.Forms.Button();
            this.btnQuitarComponente = new System.Windows.Forms.Button();
            this.btnNuevoCompuesto = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComponentes)).BeginInit();
            this.panelDetalles.SuspendLayout();
            this.SuspendLayout();

            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Size = new System.Drawing.Size(1000, 600);
            this.splitContainer.SplitterDistance = 300;
            this.splitContainer.TabIndex = 0;

            this.tvPermisos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvPermisos.Location = new System.Drawing.Point(0, 0);
            this.tvPermisos.Name = "tvPermisos";
            this.tvPermisos.Size = new System.Drawing.Size(300, 600);
            this.tvPermisos.TabIndex = 0;
            this.tvPermisos.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPermisos_AfterSelect);
            this.splitContainer.Panel1.Controls.Add(this.tvPermisos);

            this.panelDetalles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetalles.Location = new System.Drawing.Point(0, 0);
            this.panelDetalles.Name = "panelDetalles";
            this.panelDetalles.Size = new System.Drawing.Size(696, 600);
            this.panelDetalles.TabIndex = 1;
            this.panelDetalles.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Panel2.Controls.Add(this.panelDetalles);

            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(12, 12);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(47, 13);
            this.lblNombre.TabIndex = 0;
            this.lblNombre.Text = "Nombre:";

            this.txtNombre.Location = new System.Drawing.Point(12, 28);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.ReadOnly = true;
            this.txtNombre.Size = new System.Drawing.Size(400, 20);
            this.txtNombre.TabIndex = 1;

            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(12, 56);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(66, 13);
            this.lblDescripcion.TabIndex = 2;
            this.lblDescripcion.Text = "Descripción:";

            this.txtDescripcion.Location = new System.Drawing.Point(12, 72);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.ReadOnly = true;
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Size = new System.Drawing.Size(400, 60);
            this.txtDescripcion.TabIndex = 3;

            this.chkCompuesto.AutoSize = true;
            this.chkCompuesto.Location = new System.Drawing.Point(12, 140);
            this.chkCompuesto.Name = "chkCompuesto";
            this.chkCompuesto.Size = new System.Drawing.Size(130, 17);
            this.chkCompuesto.TabIndex = 4;
            this.chkCompuesto.Text = "Es permiso compuesto";
            this.chkCompuesto.Enabled = false;

            this.lblComponentes.AutoSize = true;
            this.lblComponentes.Location = new System.Drawing.Point(12, 165);
            this.lblComponentes.Name = "lblComponentes";
            this.lblComponentes.Size = new System.Drawing.Size(79, 13);
            this.lblComponentes.TabIndex = 5;
            this.lblComponentes.Text = "Componentes:";

            this.dgvComponentes.AllowUserToAddRows = false;
            this.dgvComponentes.AllowUserToDeleteRows = false;
            this.dgvComponentes.AutoGenerateColumns = false;
            this.dgvComponentes.Location = new System.Drawing.Point(12, 181);
            this.dgvComponentes.Name = "dgvComponentes";
            this.dgvComponentes.ReadOnly = true;
            this.dgvComponentes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComponentes.Size = new System.Drawing.Size(520, 250);
            this.dgvComponentes.TabIndex = 6;

            this.btnAgregarComponente.Location = new System.Drawing.Point(12, 440);
            this.btnAgregarComponente.Name = "btnAgregarComponente";
            this.btnAgregarComponente.Size = new System.Drawing.Size(110, 23);
            this.btnAgregarComponente.TabIndex = 7;
            this.btnAgregarComponente.Text = "Agregar";
            this.btnAgregarComponente.UseVisualStyleBackColor = true;
            this.btnAgregarComponente.Click += new System.EventHandler(this.btnAgregarComponente_Click);

            this.btnQuitarComponente.Location = new System.Drawing.Point(128, 440);
            this.btnQuitarComponente.Name = "btnQuitarComponente";
            this.btnQuitarComponente.Size = new System.Drawing.Size(110, 23);
            this.btnQuitarComponente.TabIndex = 8;
            this.btnQuitarComponente.Text = "Quitar";
            this.btnQuitarComponente.UseVisualStyleBackColor = true;
            this.btnQuitarComponente.Click += new System.EventHandler(this.btnQuitarComponente_Click);

            this.btnNuevoCompuesto.Location = new System.Drawing.Point(12, 470);
            this.btnNuevoCompuesto.Name = "btnNuevoCompuesto";
            this.btnNuevoCompuesto.Size = new System.Drawing.Size(140, 35);
            this.btnNuevoCompuesto.TabIndex = 9;
            this.btnNuevoCompuesto.Text = "Crear Permiso\nCompuesto";
            this.btnNuevoCompuesto.UseVisualStyleBackColor = true;
            this.btnNuevoCompuesto.Click += new System.EventHandler(this.btnNuevoCompuesto_Click);

            this.btnEliminar.Location = new System.Drawing.Point(158, 470);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(80, 35);
            this.btnEliminar.TabIndex = 10;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            this.panelDetalles.Controls.Add(this.btnEliminar);
            this.panelDetalles.Controls.Add(this.btnNuevoCompuesto);
            this.panelDetalles.Controls.Add(this.btnQuitarComponente);
            this.panelDetalles.Controls.Add(this.btnAgregarComponente);
            this.panelDetalles.Controls.Add(this.dgvComponentes);
            this.panelDetalles.Controls.Add(this.lblComponentes);
            this.panelDetalles.Controls.Add(this.chkCompuesto);
            this.panelDetalles.Controls.Add(this.txtDescripcion);
            this.panelDetalles.Controls.Add(this.lblDescripcion);
            this.panelDetalles.Controls.Add(this.txtNombre);
            this.panelDetalles.Controls.Add(this.lblNombre);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.splitContainer);
            this.Name = "FormPermisoABM";
            this.Text = "Administración de Permisos";

            ((System.ComponentModel.ISupportInitialize)(this.dgvComponentes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelDetalles.ResumeLayout(false);
            this.panelDetalles.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
