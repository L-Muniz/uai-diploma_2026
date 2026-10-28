namespace GUI
{
    partial class FormEstadisticasVentas
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
            this.lblDesde = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblHasta = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.lblResumen = new System.Windows.Forms.Label();
            this.lblProductos = new System.Windows.Forms.Label();
            this.dgvProductos = new System.Windows.Forms.DataGridView();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(16, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(260, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Estadísticas de mis ventas";
            //
            // lblDesde
            //
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(16, 50);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(42, 15);
            this.lblDesde.TabIndex = 1;
            this.lblDesde.Text = "Desde:";
            //
            // dtpDesde
            //
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(64, 47);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(130, 23);
            this.dtpDesde.TabIndex = 2;
            //
            // lblHasta
            //
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(214, 50);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(39, 15);
            this.lblHasta.TabIndex = 3;
            this.lblHasta.Text = "Hasta:";
            //
            // dtpHasta
            //
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(259, 47);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(130, 23);
            this.dtpHasta.TabIndex = 4;
            //
            // btnConsultar
            //
            this.btnConsultar.Location = new System.Drawing.Point(410, 45);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(140, 27);
            this.btnConsultar.TabIndex = 5;
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.UseVisualStyleBackColor = true;
            this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
            //
            // lblResumen
            //
            this.lblResumen.Location = new System.Drawing.Point(16, 86);
            this.lblResumen.Name = "lblResumen";
            this.lblResumen.Size = new System.Drawing.Size(614, 80);
            this.lblResumen.TabIndex = 6;
            this.lblResumen.Text = "";
            //
            // lblProductos
            //
            this.lblProductos.AutoSize = true;
            this.lblProductos.Location = new System.Drawing.Point(16, 172);
            this.lblProductos.Name = "lblProductos";
            this.lblProductos.Size = new System.Drawing.Size(120, 15);
            this.lblProductos.TabIndex = 7;
            this.lblProductos.Text = "Ventas por producto:";
            //
            // dgvProductos
            //
            this.dgvProductos.AllowUserToAddRows = false;
            this.dgvProductos.AllowUserToDeleteRows = false;
            this.dgvProductos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductos.Location = new System.Drawing.Point(16, 192);
            this.dgvProductos.Name = "dgvProductos";
            this.dgvProductos.ReadOnly = true;
            this.dgvProductos.RowHeadersVisible = false;
            this.dgvProductos.Size = new System.Drawing.Size(614, 240);
            //
            // btnExportarPdf
            //
            this.btnExportarPdf.Location = new System.Drawing.Point(16, 444);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(160, 28);
            this.btnExportarPdf.TabIndex = 9;
            this.btnExportarPdf.Text = "Exportar a PDF";
            this.btnExportarPdf.UseVisualStyleBackColor = true;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            this.dgvProductos.TabIndex = 8;
            //
            // FormEstadisticasVentas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 490);
            this.Controls.Add(this.dgvProductos);
            this.Controls.Add(this.btnExportarPdf);
            this.Controls.Add(this.lblProductos);
            this.Controls.Add(this.lblResumen);
            this.Controls.Add(this.btnConsultar);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.lblHasta);
            this.Controls.Add(this.dtpDesde);
            this.Controls.Add(this.lblDesde);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormEstadisticasVentas";
            this.Text = "Estadísticas de ventas";
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.Label lblResumen;
        private System.Windows.Forms.Label lblProductos;
        private System.Windows.Forms.DataGridView dgvProductos;
        private System.Windows.Forms.Button btnExportarPdf;
    }
}
