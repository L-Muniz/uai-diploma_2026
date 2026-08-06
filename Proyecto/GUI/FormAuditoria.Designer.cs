namespace GUI
{
    partial class FormAuditoria
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelFiltros;
        private System.Windows.Forms.Label lblEntidad;
        private System.Windows.Forms.ComboBox cbEntidad;
        private System.Windows.Forms.Label lblFechaDesde;
        private System.Windows.Forms.DateTimePicker dtDesde;
        private System.Windows.Forms.Label lblFechaHasta;
        private System.Windows.Forms.DateTimePicker dtHasta;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnDeshacer;
        private System.Windows.Forms.Label lblActualizar;
        private System.Windows.Forms.Label lblTotalCambios;
        private System.Windows.Forms.DataGridView dgvCambios;

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
            this.components = new System.ComponentModel.Container();
            this.panelFiltros = new System.Windows.Forms.Panel();
            this.lblEntidad = new System.Windows.Forms.Label();
            this.cbEntidad = new System.Windows.Forms.ComboBox();
            this.lblFechaDesde = new System.Windows.Forms.Label();
            this.dtDesde = new System.Windows.Forms.DateTimePicker();
            this.lblFechaHasta = new System.Windows.Forms.Label();
            this.dtHasta = new System.Windows.Forms.DateTimePicker();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnExportar = new System.Windows.Forms.Button();
            this.btnDeshacer = new System.Windows.Forms.Button();
            this.lblActualizar = new System.Windows.Forms.Label();
            this.lblTotalCambios = new System.Windows.Forms.Label();
            this.dgvCambios = new System.Windows.Forms.DataGridView();
            this.panelFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCambios)).BeginInit();
            this.SuspendLayout();

            this.panelFiltros.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelFiltros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFiltros.Controls.Add(this.lblEntidad);
            this.panelFiltros.Controls.Add(this.cbEntidad);
            this.panelFiltros.Controls.Add(this.lblFechaDesde);
            this.panelFiltros.Controls.Add(this.dtDesde);
            this.panelFiltros.Controls.Add(this.lblFechaHasta);
            this.panelFiltros.Controls.Add(this.dtHasta);
            this.panelFiltros.Controls.Add(this.btnFiltrar);
            this.panelFiltros.Controls.Add(this.btnExportar);
            this.panelFiltros.Controls.Add(this.btnDeshacer);
            this.panelFiltros.Controls.Add(this.lblActualizar);
            this.panelFiltros.Controls.Add(this.lblTotalCambios);
            this.panelFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFiltros.Height = 90;
            this.panelFiltros.Location = new System.Drawing.Point(0, 0);
            this.panelFiltros.Name = "panelFiltros";
            this.panelFiltros.Size = new System.Drawing.Size(1200, 90);
            this.panelFiltros.TabIndex = 0;

            this.lblEntidad.AutoSize = true;
            this.lblEntidad.Location = new System.Drawing.Point(12, 15);
            this.lblEntidad.Name = "lblEntidad";
            this.lblEntidad.Text = "Tipo de Entidad:";

            this.cbEntidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEntidad.FormattingEnabled = true;
            this.cbEntidad.Items.AddRange(new object[] { "Todos", "USUARIO", "PERFIL" });
            this.cbEntidad.Location = new System.Drawing.Point(120, 12);
            this.cbEntidad.Name = "cbEntidad";
            this.cbEntidad.Size = new System.Drawing.Size(150, 21);
            this.cbEntidad.TabIndex = 1;
            this.cbEntidad.SelectedIndex = 0;

            this.lblFechaDesde.AutoSize = true;
            this.lblFechaDesde.Location = new System.Drawing.Point(12, 50);
            this.lblFechaDesde.Name = "lblFechaDesde";
            this.lblFechaDesde.Text = "Fecha Desde:";

            this.dtDesde.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDesde.Location = new System.Drawing.Point(98, 46);
            this.dtDesde.Name = "dtDesde";
            this.dtDesde.Size = new System.Drawing.Size(160, 20);
            this.dtDesde.TabIndex = 3;

            this.lblFechaHasta.AutoSize = true;
            this.lblFechaHasta.Location = new System.Drawing.Point(280, 50);
            this.lblFechaHasta.Name = "lblFechaHasta";
            this.lblFechaHasta.Text = "Fecha Hasta:";

            this.dtHasta.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtHasta.Location = new System.Drawing.Point(360, 46);
            this.dtHasta.Name = "dtHasta";
            this.dtHasta.Size = new System.Drawing.Size(160, 20);
            this.dtHasta.TabIndex = 5;

            this.btnFiltrar.Location = new System.Drawing.Point(640, 15);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(80, 23);
            this.btnFiltrar.TabIndex = 6;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);

            this.btnExportar.Location = new System.Drawing.Point(730, 15);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(80, 23);
            this.btnExportar.TabIndex = 7;
            this.btnExportar.Text = "Exportar";
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);

            this.btnDeshacer.Location = new System.Drawing.Point(820, 15);
            this.btnDeshacer.Name = "btnDeshacer";
            this.btnDeshacer.Size = new System.Drawing.Size(80, 23);
            this.btnDeshacer.TabIndex = 8;
            this.btnDeshacer.Text = "Deshacer";
            this.btnDeshacer.UseVisualStyleBackColor = true;
            this.btnDeshacer.Click += new System.EventHandler(this.btnDeshacer_Click);

            this.lblActualizar.AutoSize = true;
            this.lblActualizar.Location = new System.Drawing.Point(540, 50);
            this.lblActualizar.Name = "lblActualizar";
            this.lblActualizar.TabIndex = 8;
            this.lblActualizar.Text = "Total cambios:";

            this.lblTotalCambios.AutoSize = true;
            this.lblTotalCambios.Font = new System.Drawing.Font(this.Font, System.Drawing.FontStyle.Bold);
            this.lblTotalCambios.Location = new System.Drawing.Point(640, 50);
            this.lblTotalCambios.Name = "lblTotalCambios";
            this.lblTotalCambios.TabIndex = 9;
            this.lblTotalCambios.Text = "0";

            this.dgvCambios.AllowUserToAddRows = false;
            this.dgvCambios.AllowUserToDeleteRows = false;
            this.dgvCambios.AllowUserToResizeRows = false;
            this.dgvCambios.AutoGenerateColumns = false;
            this.dgvCambios.BackgroundColor = System.Drawing.Color.White;
            this.dgvCambios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCambios.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCambios.Location = new System.Drawing.Point(0, 90);
            this.dgvCambios.Name = "dgvCambios";
            this.dgvCambios.ReadOnly = true;
            this.dgvCambios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCambios.Size = new System.Drawing.Size(1200, 563);
            this.dgvCambios.TabIndex = 1;

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1200, 653);
            this.Controls.Add(this.dgvCambios);
            this.Controls.Add(this.panelFiltros);
            this.Name = "FormAuditoria";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Auditoria";
            this.panelFiltros.ResumeLayout(false);
            this.panelFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCambios)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
