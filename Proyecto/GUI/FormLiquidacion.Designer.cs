namespace GUI
{
    partial class FormLiquidacion
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabCompras = new System.Windows.Forms.TabPage();
            this.dgvCompras = new System.Windows.Forms.DataGridView();
            this.btnConfirmarRecepcion = new System.Windows.Forms.Button();
            this.tabVentas = new System.Windows.Forms.TabPage();
            this.dgvVentas = new System.Windows.Forms.DataGridView();
            this.btnDespachar = new System.Windows.Forms.Button();
            this.tabLiquidaciones = new System.Windows.Forms.TabPage();
            this.dgvLiquidaciones = new System.Windows.Forms.DataGridView();
            this.btnPagar = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabCompras.SuspendLayout();
            this.tabVentas.SuspendLayout();
            this.tabLiquidaciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompras)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLiquidaciones)).BeginInit();
            this.SuspendLayout();
            //
            // tabControl
            //
            this.tabControl.Controls.Add(this.tabCompras);
            this.tabControl.Controls.Add(this.tabVentas);
            this.tabControl.Controls.Add(this.tabLiquidaciones);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(626, 470);
            this.tabControl.TabIndex = 0;
            //
            // tabCompras
            //
            this.tabCompras.Controls.Add(this.dgvCompras);
            this.tabCompras.Controls.Add(this.btnConfirmarRecepcion);
            this.tabCompras.Location = new System.Drawing.Point(4, 24);
            this.tabCompras.Name = "tabCompras";
            this.tabCompras.Padding = new System.Windows.Forms.Padding(3);
            this.tabCompras.Size = new System.Drawing.Size(618, 442);
            this.tabCompras.TabIndex = 0;
            this.tabCompras.Text = "Mis compras";
            this.tabCompras.UseVisualStyleBackColor = true;
            //
            // dgvCompras
            //
            this.dgvCompras.AllowUserToAddRows = false;
            this.dgvCompras.AllowUserToDeleteRows = false;
            this.dgvCompras.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCompras.Location = new System.Drawing.Point(10, 10);
            this.dgvCompras.MultiSelect = false;
            this.dgvCompras.Name = "dgvCompras";
            this.dgvCompras.ReadOnly = true;
            this.dgvCompras.RowHeadersVisible = false;
            this.dgvCompras.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCompras.Size = new System.Drawing.Size(596, 380);
            this.dgvCompras.TabIndex = 0;
            //
            // btnConfirmarRecepcion
            //
            this.btnConfirmarRecepcion.Location = new System.Drawing.Point(10, 400);
            this.btnConfirmarRecepcion.Name = "btnConfirmarRecepcion";
            this.btnConfirmarRecepcion.Size = new System.Drawing.Size(220, 28);
            this.btnConfirmarRecepcion.TabIndex = 1;
            this.btnConfirmarRecepcion.Text = "Confirmar recepción conforme";
            this.btnConfirmarRecepcion.UseVisualStyleBackColor = true;
            this.btnConfirmarRecepcion.Click += new System.EventHandler(this.btnConfirmarRecepcion_Click);
            //
            // tabVentas
            //
            this.tabVentas.Controls.Add(this.dgvVentas);
            this.tabVentas.Controls.Add(this.btnDespachar);
            this.tabVentas.Location = new System.Drawing.Point(4, 24);
            this.tabVentas.Name = "tabVentas";
            this.tabVentas.Padding = new System.Windows.Forms.Padding(3);
            this.tabVentas.Size = new System.Drawing.Size(618, 442);
            this.tabVentas.TabIndex = 1;
            this.tabVentas.Text = "Mis ventas";
            this.tabVentas.UseVisualStyleBackColor = true;
            //
            // dgvVentas
            //
            this.dgvVentas.AllowUserToAddRows = false;
            this.dgvVentas.AllowUserToDeleteRows = false;
            this.dgvVentas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVentas.Location = new System.Drawing.Point(10, 10);
            this.dgvVentas.MultiSelect = false;
            this.dgvVentas.Name = "dgvVentas";
            this.dgvVentas.ReadOnly = true;
            this.dgvVentas.RowHeadersVisible = false;
            this.dgvVentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVentas.Size = new System.Drawing.Size(596, 380);
            this.dgvVentas.TabIndex = 0;
            //
            // btnDespachar
            //
            this.btnDespachar.Location = new System.Drawing.Point(10, 400);
            this.btnDespachar.Name = "btnDespachar";
            this.btnDespachar.Size = new System.Drawing.Size(220, 28);
            this.btnDespachar.TabIndex = 1;
            this.btnDespachar.Text = "Marcar como despachada";
            this.btnDespachar.UseVisualStyleBackColor = true;
            this.btnDespachar.Click += new System.EventHandler(this.btnDespachar_Click);
            //
            // tabLiquidaciones
            //
            this.tabLiquidaciones.Controls.Add(this.dgvLiquidaciones);
            this.tabLiquidaciones.Controls.Add(this.btnPagar);
            this.tabLiquidaciones.Location = new System.Drawing.Point(4, 24);
            this.tabLiquidaciones.Name = "tabLiquidaciones";
            this.tabLiquidaciones.Padding = new System.Windows.Forms.Padding(3);
            this.tabLiquidaciones.Size = new System.Drawing.Size(618, 442);
            this.tabLiquidaciones.TabIndex = 2;
            this.tabLiquidaciones.Text = "Liquidaciones";
            this.tabLiquidaciones.UseVisualStyleBackColor = true;
            //
            // dgvLiquidaciones
            //
            this.dgvLiquidaciones.AllowUserToAddRows = false;
            this.dgvLiquidaciones.AllowUserToDeleteRows = false;
            this.dgvLiquidaciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLiquidaciones.Location = new System.Drawing.Point(10, 10);
            this.dgvLiquidaciones.MultiSelect = false;
            this.dgvLiquidaciones.Name = "dgvLiquidaciones";
            this.dgvLiquidaciones.ReadOnly = true;
            this.dgvLiquidaciones.RowHeadersVisible = false;
            this.dgvLiquidaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLiquidaciones.Size = new System.Drawing.Size(596, 380);
            this.dgvLiquidaciones.TabIndex = 0;
            //
            // btnPagar
            //
            this.btnPagar.Location = new System.Drawing.Point(10, 400);
            this.btnPagar.Name = "btnPagar";
            this.btnPagar.Size = new System.Drawing.Size(260, 28);
            this.btnPagar.TabIndex = 1;
            this.btnPagar.Text = "Ordenar transferencia al vendedor";
            this.btnPagar.UseVisualStyleBackColor = true;
            this.btnPagar.Click += new System.EventHandler(this.btnPagar_Click);
            //
            // FormLiquidacion
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 494);
            this.Controls.Add(this.tabControl);
            this.Name = "FormLiquidacion";
            this.Text = "Pedidos y liquidaciones";
            this.tabControl.ResumeLayout(false);
            this.tabCompras.ResumeLayout(false);
            this.tabVentas.ResumeLayout(false);
            this.tabLiquidaciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCompras)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLiquidaciones)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabCompras;
        private System.Windows.Forms.DataGridView dgvCompras;
        private System.Windows.Forms.Button btnConfirmarRecepcion;
        private System.Windows.Forms.TabPage tabVentas;
        private System.Windows.Forms.DataGridView dgvVentas;
        private System.Windows.Forms.Button btnDespachar;
        private System.Windows.Forms.TabPage tabLiquidaciones;
        private System.Windows.Forms.DataGridView dgvLiquidaciones;
        private System.Windows.Forms.Button btnPagar;
    }
}
