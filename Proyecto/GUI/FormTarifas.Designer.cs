namespace GUI
{
    partial class FormTarifas
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
            this.lblTarifas = new System.Windows.Forms.Label();
            this.dgvTarifas = new System.Windows.Forms.DataGridView();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.txtCategoria = new System.Windows.Forms.TextBox();
            this.lblPorcentaje = new System.Windows.Forms.Label();
            this.numPorcentaje = new System.Windows.Forms.NumericUpDown();
            this.btnProponerTarifa = new System.Windows.Forms.Button();
            this.lblConversiones = new System.Windows.Forms.Label();
            this.dgvConversiones = new System.Windows.Forms.DataGridView();
            this.lblMaterial = new System.Windows.Forms.Label();
            this.cboMaterial = new System.Windows.Forms.ComboBox();
            this.lblPuntos = new System.Windows.Forms.Label();
            this.txtPuntosKg = new System.Windows.Forms.TextBox();
            this.btnProponerConversion = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarifas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConversiones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPorcentaje)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(16, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(320, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Tarifas de comisión y conversión";
            //
            // lblTarifas
            //
            this.lblTarifas.AutoSize = true;
            this.lblTarifas.Location = new System.Drawing.Point(16, 44);
            this.lblTarifas.Name = "lblTarifas";
            this.lblTarifas.Size = new System.Drawing.Size(180, 15);
            this.lblTarifas.TabIndex = 1;
            this.lblTarifas.Text = "Comisión vigente por categoría:";
            //
            // dgvTarifas
            //
            this.dgvTarifas.AllowUserToAddRows = false;
            this.dgvTarifas.AllowUserToDeleteRows = false;
            this.dgvTarifas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTarifas.Location = new System.Drawing.Point(16, 64);
            this.dgvTarifas.Name = "dgvTarifas";
            this.dgvTarifas.ReadOnly = true;
            this.dgvTarifas.RowHeadersVisible = false;
            this.dgvTarifas.Size = new System.Drawing.Size(614, 120);
            this.dgvTarifas.TabIndex = 2;
            //
            // lblCategoria
            //
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Location = new System.Drawing.Point(16, 200);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(65, 15);
            this.lblCategoria.TabIndex = 3;
            this.lblCategoria.Text = "Categoría:";
            //
            // txtCategoria
            //
            this.txtCategoria.Location = new System.Drawing.Point(90, 197);
            this.txtCategoria.Name = "txtCategoria";
            this.txtCategoria.Size = new System.Drawing.Size(200, 23);
            this.txtCategoria.TabIndex = 4;
            this.txtCategoria.Text = "General";
            //
            // lblPorcentaje
            //
            this.lblPorcentaje.AutoSize = true;
            this.lblPorcentaje.Location = new System.Drawing.Point(306, 200);
            this.lblPorcentaje.Name = "lblPorcentaje";
            this.lblPorcentaje.Size = new System.Drawing.Size(74, 15);
            this.lblPorcentaje.TabIndex = 5;
            this.lblPorcentaje.Text = "Comisión %:";
            //
            // numPorcentaje
            //
            this.numPorcentaje.DecimalPlaces = 2;
            this.numPorcentaje.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            this.numPorcentaje.Location = new System.Drawing.Point(386, 197);
            this.numPorcentaje.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            this.numPorcentaje.Name = "numPorcentaje";
            this.numPorcentaje.Size = new System.Drawing.Size(90, 23);
            this.numPorcentaje.TabIndex = 6;
            this.numPorcentaje.Value = new decimal(new int[] { 12, 0, 0, 0 });
            //
            // btnProponerTarifa
            //
            this.btnProponerTarifa.Location = new System.Drawing.Point(490, 195);
            this.btnProponerTarifa.Name = "btnProponerTarifa";
            this.btnProponerTarifa.Size = new System.Drawing.Size(140, 27);
            this.btnProponerTarifa.TabIndex = 7;
            this.btnProponerTarifa.Text = "Fijar comisión";
            this.btnProponerTarifa.UseVisualStyleBackColor = true;
            this.btnProponerTarifa.Click += new System.EventHandler(this.btnProponerTarifa_Click);
            //
            // lblConversiones
            //
            this.lblConversiones.AutoSize = true;
            this.lblConversiones.Location = new System.Drawing.Point(16, 240);
            this.lblConversiones.Name = "lblConversiones";
            this.lblConversiones.Size = new System.Drawing.Size(200, 15);
            this.lblConversiones.TabIndex = 8;
            this.lblConversiones.Text = "Conversión vigente de Eco-Puntos:";
            //
            // dgvConversiones
            //
            this.dgvConversiones.AllowUserToAddRows = false;
            this.dgvConversiones.AllowUserToDeleteRows = false;
            this.dgvConversiones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvConversiones.Location = new System.Drawing.Point(16, 260);
            this.dgvConversiones.Name = "dgvConversiones";
            this.dgvConversiones.ReadOnly = true;
            this.dgvConversiones.RowHeadersVisible = false;
            this.dgvConversiones.Size = new System.Drawing.Size(614, 120);
            this.dgvConversiones.TabIndex = 9;
            //
            // lblMaterial
            //
            this.lblMaterial.AutoSize = true;
            this.lblMaterial.Location = new System.Drawing.Point(16, 396);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(52, 15);
            this.lblMaterial.TabIndex = 10;
            this.lblMaterial.Text = "Material:";
            //
            // cboMaterial
            //
            this.cboMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaterial.FormattingEnabled = true;
            this.cboMaterial.Location = new System.Drawing.Point(90, 393);
            this.cboMaterial.Name = "cboMaterial";
            this.cboMaterial.Size = new System.Drawing.Size(200, 23);
            this.cboMaterial.TabIndex = 11;
            //
            // lblPuntos
            //
            this.lblPuntos.AutoSize = true;
            this.lblPuntos.Location = new System.Drawing.Point(306, 396);
            this.lblPuntos.Name = "lblPuntos";
            this.lblPuntos.Size = new System.Drawing.Size(75, 15);
            this.lblPuntos.TabIndex = 12;
            this.lblPuntos.Text = "Puntos por kg:";
            //
            // txtPuntosKg
            //
            this.txtPuntosKg.Location = new System.Drawing.Point(386, 393);
            this.txtPuntosKg.Name = "txtPuntosKg";
            this.txtPuntosKg.Size = new System.Drawing.Size(90, 23);
            this.txtPuntosKg.TabIndex = 13;
            this.txtPuntosKg.Text = "10";
            //
            // btnProponerConversion
            //
            this.btnProponerConversion.Location = new System.Drawing.Point(490, 391);
            this.btnProponerConversion.Name = "btnProponerConversion";
            this.btnProponerConversion.Size = new System.Drawing.Size(140, 27);
            this.btnProponerConversion.TabIndex = 14;
            this.btnProponerConversion.Text = "Fijar conversión";
            this.btnProponerConversion.UseVisualStyleBackColor = true;
            this.btnProponerConversion.Click += new System.EventHandler(this.btnProponerConversion_Click);
            //
            // FormTarifas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 440);
            this.Controls.Add(this.btnProponerConversion);
            this.Controls.Add(this.txtPuntosKg);
            this.Controls.Add(this.lblPuntos);
            this.Controls.Add(this.cboMaterial);
            this.Controls.Add(this.lblMaterial);
            this.Controls.Add(this.dgvConversiones);
            this.Controls.Add(this.lblConversiones);
            this.Controls.Add(this.btnProponerTarifa);
            this.Controls.Add(this.numPorcentaje);
            this.Controls.Add(this.lblPorcentaje);
            this.Controls.Add(this.txtCategoria);
            this.Controls.Add(this.lblCategoria);
            this.Controls.Add(this.dgvTarifas);
            this.Controls.Add(this.lblTarifas);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormTarifas";
            this.Text = "Tarifas de comisión y conversión";
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarifas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConversiones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPorcentaje)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblTarifas;
        private System.Windows.Forms.DataGridView dgvTarifas;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.TextBox txtCategoria;
        private System.Windows.Forms.Label lblPorcentaje;
        private System.Windows.Forms.NumericUpDown numPorcentaje;
        private System.Windows.Forms.Button btnProponerTarifa;
        private System.Windows.Forms.Label lblConversiones;
        private System.Windows.Forms.DataGridView dgvConversiones;
        private System.Windows.Forms.Label lblMaterial;
        private System.Windows.Forms.ComboBox cboMaterial;
        private System.Windows.Forms.Label lblPuntos;
        private System.Windows.Forms.TextBox txtPuntosKg;
        private System.Windows.Forms.Button btnProponerConversion;
    }
}
