namespace GUI
{
    partial class FormIndicadores
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
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.txtPeriodo = new System.Windows.Forms.TextBox();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.dgvIndicadores = new System.Windows.Forms.DataGridView();
            this.btnExportarPdf = new System.Windows.Forms.Button();
            this.btnExportarXml = new System.Windows.Forms.Button();
            this.btnImportarXml = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndicadores)).BeginInit();
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
            this.lblTitulo.Text = "Indicadores de acopio y ventas";
            //
            // lblPeriodo
            //
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Location = new System.Drawing.Point(16, 50);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(120, 15);
            this.lblPeriodo.TabIndex = 1;
            this.lblPeriodo.Text = "Período (aaaa-mm):";
            //
            // txtPeriodo
            //
            this.txtPeriodo.Location = new System.Drawing.Point(140, 47);
            this.txtPeriodo.Name = "txtPeriodo";
            this.txtPeriodo.Size = new System.Drawing.Size(100, 23);
            this.txtPeriodo.TabIndex = 2;
            //
            // btnConsultar
            //
            this.btnConsultar.Location = new System.Drawing.Point(255, 45);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(160, 27);
            this.btnConsultar.TabIndex = 3;
            this.btnConsultar.Text = "Consultar indicadores";
            this.btnConsultar.UseVisualStyleBackColor = true;
            this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
            //
            // dgvIndicadores
            //
            this.dgvIndicadores.AllowUserToAddRows = false;
            this.dgvIndicadores.AllowUserToDeleteRows = false;
            this.dgvIndicadores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIndicadores.Location = new System.Drawing.Point(16, 90);
            this.dgvIndicadores.Name = "dgvIndicadores";
            this.dgvIndicadores.ReadOnly = true;
            this.dgvIndicadores.RowHeadersVisible = false;
            this.dgvIndicadores.Size = new System.Drawing.Size(614, 350);
            this.dgvIndicadores.TabIndex = 4;
            //
            // btnExportarPdf
            //
            this.btnExportarPdf.Location = new System.Drawing.Point(16, 455);
            this.btnExportarPdf.Name = "btnExportarPdf";
            this.btnExportarPdf.Size = new System.Drawing.Size(150, 28);
            this.btnExportarPdf.TabIndex = 5;
            this.btnExportarPdf.Text = "Exportar a PDF";
            this.btnExportarPdf.UseVisualStyleBackColor = true;
            this.btnExportarPdf.Click += new System.EventHandler(this.btnExportarPdf_Click);
            //
            // btnExportarXml
            //
            this.btnExportarXml.Location = new System.Drawing.Point(180, 455);
            this.btnExportarXml.Name = "btnExportarXml";
            this.btnExportarXml.Size = new System.Drawing.Size(150, 28);
            this.btnExportarXml.TabIndex = 6;
            this.btnExportarXml.Text = "Guardar XML";
            this.btnExportarXml.UseVisualStyleBackColor = true;
            this.btnExportarXml.Click += new System.EventHandler(this.btnExportarXml_Click);
            //
            // btnImportarXml
            //
            this.btnImportarXml.Location = new System.Drawing.Point(344, 455);
            this.btnImportarXml.Name = "btnImportarXml";
            this.btnImportarXml.Size = new System.Drawing.Size(150, 28);
            this.btnImportarXml.TabIndex = 7;
            this.btnImportarXml.Text = "Abrir XML guardado";
            this.btnImportarXml.UseVisualStyleBackColor = true;
            this.btnImportarXml.Click += new System.EventHandler(this.btnImportarXml_Click);
            //
            // FormIndicadores
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 510);
            this.Controls.Add(this.dgvIndicadores);
            this.Controls.Add(this.btnExportarPdf);
            this.Controls.Add(this.btnExportarXml);
            this.Controls.Add(this.btnImportarXml);
            this.Controls.Add(this.btnConsultar);
            this.Controls.Add(this.txtPeriodo);
            this.Controls.Add(this.lblPeriodo);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormIndicadores";
            this.Text = "Indicadores de gestión";
            ((System.ComponentModel.ISupportInitialize)(this.dgvIndicadores)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.TextBox txtPeriodo;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.DataGridView dgvIndicadores;
        private System.Windows.Forms.Button btnExportarPdf;
        private System.Windows.Forms.Button btnExportarXml;
        private System.Windows.Forms.Button btnImportarXml;
    }
}
