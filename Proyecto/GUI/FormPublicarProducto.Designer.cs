namespace GUI
{
    partial class FormPublicarProducto
    {
        private System.ComponentModel.IContainer components = null;

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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.txtCategoria = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblPrecio = new System.Windows.Forms.Label();
            this.txtPrecio = new System.Windows.Forms.TextBox();
            this.lblStock = new System.Windows.Forms.Label();
            this.numStock = new System.Windows.Forms.NumericUpDown();
            this.lblComision = new System.Windows.Forms.Label();
            this.btnPublicar = new System.Windows.Forms.Button();
            this.lblMisProductos = new System.Windows.Forms.Label();
            this.dgvMisProductos = new System.Windows.Forms.DataGridView();
            this.lblPendientes = new System.Windows.Forms.Label();
            this.dgvPendientes = new System.Windows.Forms.DataGridView();
            this.btnAprobar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMisProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendientes)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(16, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(280, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Publicar producto sustentable";
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(16, 46);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(56, 15);
            this.lblNombre.TabIndex = 1;
            this.lblNombre.Text = "Nombre:";
            //
            // txtNombre
            //
            this.txtNombre.Location = new System.Drawing.Point(110, 43);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(250, 23);
            this.txtNombre.TabIndex = 2;
            //
            // lblCategoria
            //
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Location = new System.Drawing.Point(16, 76);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(65, 15);
            this.lblCategoria.TabIndex = 3;
            this.lblCategoria.Text = "Categoría:";
            //
            // txtCategoria
            //
            this.txtCategoria.Location = new System.Drawing.Point(110, 73);
            this.txtCategoria.Name = "txtCategoria";
            this.txtCategoria.Size = new System.Drawing.Size(250, 23);
            this.txtCategoria.TabIndex = 4;
            this.txtCategoria.Text = "General";
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(16, 106);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(75, 15);
            this.lblDescripcion.TabIndex = 5;
            this.lblDescripcion.Text = "Descripción:";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(110, 103);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(520, 23);
            this.txtDescripcion.TabIndex = 6;
            //
            // lblPrecio
            //
            this.lblPrecio.AutoSize = true;
            this.lblPrecio.Location = new System.Drawing.Point(16, 136);
            this.lblPrecio.Name = "lblPrecio";
            this.lblPrecio.Size = new System.Drawing.Size(45, 15);
            this.lblPrecio.TabIndex = 7;
            this.lblPrecio.Text = "Precio:";
            //
            // txtPrecio
            //
            this.txtPrecio.Location = new System.Drawing.Point(110, 133);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Size = new System.Drawing.Size(110, 23);
            this.txtPrecio.TabIndex = 8;
            //
            // lblStock
            //
            this.lblStock.AutoSize = true;
            this.lblStock.Location = new System.Drawing.Point(240, 136);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(44, 15);
            this.lblStock.TabIndex = 9;
            this.lblStock.Text = "Stock:";
            //
            // numStock
            //
            this.numStock.Location = new System.Drawing.Point(290, 133);
            this.numStock.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            this.numStock.Name = "numStock";
            this.numStock.Size = new System.Drawing.Size(90, 23);
            this.numStock.TabIndex = 10;
            this.numStock.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // lblComision
            //
            this.lblComision.AutoSize = true;
            this.lblComision.Location = new System.Drawing.Point(110, 166);
            this.lblComision.Name = "lblComision";
            this.lblComision.Size = new System.Drawing.Size(140, 15);
            this.lblComision.TabIndex = 11;
            this.lblComision.Text = "Comisión estimada: —";
            //
            // btnPublicar
            //
            this.btnPublicar.Location = new System.Drawing.Point(110, 190);
            this.btnPublicar.Name = "btnPublicar";
            this.btnPublicar.Size = new System.Drawing.Size(160, 28);
            this.btnPublicar.TabIndex = 12;
            this.btnPublicar.Text = "Publicar";
            this.btnPublicar.UseVisualStyleBackColor = true;
            this.btnPublicar.Click += new System.EventHandler(this.btnPublicar_Click);
            //
            // lblMisProductos
            //
            this.lblMisProductos.AutoSize = true;
            this.lblMisProductos.Location = new System.Drawing.Point(16, 232);
            this.lblMisProductos.Name = "lblMisProductos";
            this.lblMisProductos.Size = new System.Drawing.Size(110, 15);
            this.lblMisProductos.TabIndex = 13;
            this.lblMisProductos.Text = "Mis publicaciones:";
            //
            // dgvMisProductos
            //
            this.dgvMisProductos.AllowUserToAddRows = false;
            this.dgvMisProductos.AllowUserToDeleteRows = false;
            this.dgvMisProductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMisProductos.Location = new System.Drawing.Point(16, 252);
            this.dgvMisProductos.Name = "dgvMisProductos";
            this.dgvMisProductos.ReadOnly = true;
            this.dgvMisProductos.RowHeadersVisible = false;
            this.dgvMisProductos.Size = new System.Drawing.Size(614, 150);
            this.dgvMisProductos.TabIndex = 14;
            //
            // lblPendientes
            //
            this.lblPendientes.AutoSize = true;
            this.lblPendientes.Location = new System.Drawing.Point(16, 414);
            this.lblPendientes.Name = "lblPendientes";
            this.lblPendientes.Size = new System.Drawing.Size(220, 15);
            this.lblPendientes.TabIndex = 15;
            this.lblPendientes.Text = "Pendientes de aprobación (área comercial):";
            //
            // dgvPendientes
            //
            this.dgvPendientes.AllowUserToAddRows = false;
            this.dgvPendientes.AllowUserToDeleteRows = false;
            this.dgvPendientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPendientes.Location = new System.Drawing.Point(16, 434);
            this.dgvPendientes.MultiSelect = false;
            this.dgvPendientes.Name = "dgvPendientes";
            this.dgvPendientes.ReadOnly = true;
            this.dgvPendientes.RowHeadersVisible = false;
            this.dgvPendientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPendientes.Size = new System.Drawing.Size(614, 150);
            this.dgvPendientes.TabIndex = 16;
            //
            // btnAprobar
            //
            this.btnAprobar.Location = new System.Drawing.Point(16, 592);
            this.btnAprobar.Name = "btnAprobar";
            this.btnAprobar.Size = new System.Drawing.Size(200, 28);
            this.btnAprobar.TabIndex = 17;
            this.btnAprobar.Text = "Aprobar producto seleccionado";
            this.btnAprobar.UseVisualStyleBackColor = true;
            this.btnAprobar.Click += new System.EventHandler(this.btnAprobar_Click);
            //
            // btnActualizar
            //
            this.btnActualizar.Location = new System.Drawing.Point(290, 190);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(200, 28);
            this.btnActualizar.TabIndex = 18;
            this.btnActualizar.Text = "Actualizar precio y stock";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            //
            // FormPublicarProducto
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 640);
            this.Controls.Add(this.btnAprobar);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.dgvPendientes);
            this.Controls.Add(this.lblPendientes);
            this.Controls.Add(this.dgvMisProductos);
            this.Controls.Add(this.lblMisProductos);
            this.Controls.Add(this.btnPublicar);
            this.Controls.Add(this.lblComision);
            this.Controls.Add(this.numStock);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.txtPrecio);
            this.Controls.Add(this.lblPrecio);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.txtCategoria);
            this.Controls.Add(this.lblCategoria);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormPublicarProducto";
            this.Text = "Publicar producto";
            ((System.ComponentModel.ISupportInitialize)(this.numStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMisProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendientes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.TextBox txtCategoria;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblPrecio;
        private System.Windows.Forms.TextBox txtPrecio;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.NumericUpDown numStock;
        private System.Windows.Forms.Label lblComision;
        private System.Windows.Forms.Button btnPublicar;
        private System.Windows.Forms.Label lblMisProductos;
        private System.Windows.Forms.DataGridView dgvMisProductos;
        private System.Windows.Forms.Label lblPendientes;
        private System.Windows.Forms.DataGridView dgvPendientes;
        private System.Windows.Forms.Button btnAprobar;
        private System.Windows.Forms.Button btnActualizar;
    }
}
