namespace GUI
{
    partial class FormRecepcionAcopio
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
            this.lblPuntoVerde = new System.Windows.Forms.Label();
            this.cboPuntoVerde = new System.Windows.Forms.ComboBox();
            this.lblPendientes = new System.Windows.Forms.Label();
            this.dgvPendientes = new System.Windows.Forms.DataGridView();
            this.lblPeso = new System.Windows.Forms.Label();
            this.txtPeso = new System.Windows.Forms.TextBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.lblAyuda = new System.Windows.Forms.Label();
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
            this.lblTitulo.Text = "Registrar recepción de material";
            //
            // lblPuntoVerde
            //
            this.lblPuntoVerde.AutoSize = true;
            this.lblPuntoVerde.Location = new System.Drawing.Point(16, 50);
            this.lblPuntoVerde.Name = "lblPuntoVerde";
            this.lblPuntoVerde.Size = new System.Drawing.Size(80, 15);
            this.lblPuntoVerde.TabIndex = 1;
            this.lblPuntoVerde.Text = "Punto verde:";
            //
            // cboPuntoVerde
            //
            this.cboPuntoVerde.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPuntoVerde.FormattingEnabled = true;
            this.cboPuntoVerde.Location = new System.Drawing.Point(110, 47);
            this.cboPuntoVerde.Name = "cboPuntoVerde";
            this.cboPuntoVerde.Size = new System.Drawing.Size(260, 23);
            this.cboPuntoVerde.TabIndex = 2;
            //
            // lblPendientes
            //
            this.lblPendientes.AutoSize = true;
            this.lblPendientes.Location = new System.Drawing.Point(16, 84);
            this.lblPendientes.Name = "lblPendientes";
            this.lblPendientes.Size = new System.Drawing.Size(250, 15);
            this.lblPendientes.TabIndex = 3;
            this.lblPendientes.Text = "Declaraciones pendientes de entrega:";
            //
            // dgvPendientes
            //
            this.dgvPendientes.AllowUserToAddRows = false;
            this.dgvPendientes.AllowUserToDeleteRows = false;
            this.dgvPendientes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPendientes.Location = new System.Drawing.Point(16, 104);
            this.dgvPendientes.MultiSelect = false;
            this.dgvPendientes.Name = "dgvPendientes";
            this.dgvPendientes.ReadOnly = true;
            this.dgvPendientes.RowHeadersVisible = false;
            this.dgvPendientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPendientes.Size = new System.Drawing.Size(614, 280);
            this.dgvPendientes.TabIndex = 4;
            //
            // lblPeso
            //
            this.lblPeso.AutoSize = true;
            this.lblPeso.Location = new System.Drawing.Point(16, 402);
            this.lblPeso.Name = "lblPeso";
            this.lblPeso.Size = new System.Drawing.Size(93, 15);
            this.lblPeso.TabIndex = 5;
            this.lblPeso.Text = "Peso pesado (kg):";
            //
            // txtPeso
            //
            this.txtPeso.Location = new System.Drawing.Point(115, 399);
            this.txtPeso.Name = "txtPeso";
            this.txtPeso.Size = new System.Drawing.Size(90, 23);
            this.txtPeso.TabIndex = 6;
            //
            // btnRegistrar
            //
            this.btnRegistrar.Location = new System.Drawing.Point(220, 397);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(200, 28);
            this.btnRegistrar.TabIndex = 7;
            this.btnRegistrar.Text = "Registrar recepción";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            //
            // lblAyuda
            //
            this.lblAyuda.Location = new System.Drawing.Point(16, 436);
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.Size = new System.Drawing.Size(614, 40);
            this.lblAyuda.TabIndex = 8;
            this.lblAyuda.Text = "Seleccione la declaración del ciudadano, ingrese el peso y registre la recepción. " +
                                 "Si el punto verde no acepta el material, la carga se rechaza.";
            //
            // FormRecepcionAcopio
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 490);
            this.Controls.Add(this.lblAyuda);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.txtPeso);
            this.Controls.Add(this.lblPeso);
            this.Controls.Add(this.dgvPendientes);
            this.Controls.Add(this.lblPendientes);
            this.Controls.Add(this.cboPuntoVerde);
            this.Controls.Add(this.lblPuntoVerde);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormRecepcionAcopio";
            this.Text = "Recepción de material";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPendientes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblPuntoVerde;
        private System.Windows.Forms.ComboBox cboPuntoVerde;
        private System.Windows.Forms.Label lblPendientes;
        private System.Windows.Forms.DataGridView dgvPendientes;
        private System.Windows.Forms.Label lblPeso;
        private System.Windows.Forms.TextBox txtPeso;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Label lblAyuda;
    }
}
