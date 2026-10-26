namespace GUI
{
    partial class FormBeneficios
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
            this.dgvBeneficios = new System.Windows.Forms.DataGridView();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cboTipo = new System.Windows.Forms.ComboBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.lblPuntos = new System.Windows.Forms.Label();
            this.numPuntos = new System.Windows.Forms.NumericUpDown();
            this.lblCosto = new System.Windows.Forms.Label();
            this.txtCosto = new System.Windows.Forms.TextBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnActivar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeneficios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPuntos)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(16, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(240, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Beneficios canjeables";
            //
            // dgvBeneficios
            //
            this.dgvBeneficios.AllowUserToAddRows = false;
            this.dgvBeneficios.AllowUserToDeleteRows = false;
            this.dgvBeneficios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBeneficios.Location = new System.Drawing.Point(16, 40);
            this.dgvBeneficios.MultiSelect = false;
            this.dgvBeneficios.Name = "dgvBeneficios";
            this.dgvBeneficios.ReadOnly = true;
            this.dgvBeneficios.RowHeadersVisible = false;
            this.dgvBeneficios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBeneficios.Size = new System.Drawing.Size(614, 200);
            this.dgvBeneficios.TabIndex = 1;
            //
            // lblTipo
            //
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(16, 256);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(34, 15);
            this.lblTipo.TabIndex = 2;
            this.lblTipo.Text = "Tipo:";
            //
            // cboTipo
            //
            this.cboTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipo.FormattingEnabled = true;
            this.cboTipo.Items.AddRange(new object[] { "CUPON", "ENVIO" });
            this.cboTipo.Location = new System.Drawing.Point(110, 253);
            this.cboTipo.Name = "cboTipo";
            this.cboTipo.Size = new System.Drawing.Size(140, 23);
            this.cboTipo.TabIndex = 3;
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(16, 286);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(75, 15);
            this.lblDescripcion.TabIndex = 4;
            this.lblDescripcion.Text = "Descripción:";
            //
            // txtDescripcion
            //
            this.txtDescripcion.Location = new System.Drawing.Point(110, 283);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(520, 23);
            this.txtDescripcion.TabIndex = 5;
            //
            // lblPuntos
            //
            this.lblPuntos.AutoSize = true;
            this.lblPuntos.Location = new System.Drawing.Point(16, 316);
            this.lblPuntos.Name = "lblPuntos";
            this.lblPuntos.Size = new System.Drawing.Size(92, 15);
            this.lblPuntos.TabIndex = 6;
            this.lblPuntos.Text = "Puntos requeridos:";
            //
            // numPuntos
            //
            this.numPuntos.Location = new System.Drawing.Point(140, 313);
            this.numPuntos.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.numPuntos.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numPuntos.Name = "numPuntos";
            this.numPuntos.Size = new System.Drawing.Size(110, 23);
            this.numPuntos.TabIndex = 7;
            this.numPuntos.Value = new decimal(new int[] { 100, 0, 0, 0 });
            //
            // lblCosto
            //
            this.lblCosto.AutoSize = true;
            this.lblCosto.Location = new System.Drawing.Point(270, 316);
            this.lblCosto.Name = "lblCosto";
            this.lblCosto.Size = new System.Drawing.Size(150, 15);
            this.lblCosto.TabIndex = 8;
            this.lblCosto.Text = "Costo para la empresa ($):";
            //
            // txtCosto
            //
            this.txtCosto.Location = new System.Drawing.Point(425, 313);
            this.txtCosto.Name = "txtCosto";
            this.txtCosto.Size = new System.Drawing.Size(120, 23);
            this.txtCosto.TabIndex = 9;
            this.txtCosto.Text = "0";
            //
            // btnNuevo
            //
            this.btnNuevo.Location = new System.Drawing.Point(16, 360);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(120, 28);
            this.btnNuevo.TabIndex = 10;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(150, 360);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(120, 28);
            this.btnGuardar.TabIndex = 11;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnActivar
            //
            this.btnActivar.Location = new System.Drawing.Point(284, 360);
            this.btnActivar.Name = "btnActivar";
            this.btnActivar.Size = new System.Drawing.Size(200, 28);
            this.btnActivar.TabIndex = 12;
            this.btnActivar.Text = "Activar / desactivar";
            this.btnActivar.UseVisualStyleBackColor = true;
            this.btnActivar.Click += new System.EventHandler(this.btnActivar_Click);
            //
            // FormBeneficios
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 410);
            this.Controls.Add(this.btnActivar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.txtCosto);
            this.Controls.Add(this.lblCosto);
            this.Controls.Add(this.numPuntos);
            this.Controls.Add(this.lblPuntos);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.cboTipo);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.dgvBeneficios);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormBeneficios";
            this.Text = "Beneficios canjeables";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeneficios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPuntos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvBeneficios;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cboTipo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Label lblPuntos;
        private System.Windows.Forms.NumericUpDown numPuntos;
        private System.Windows.Forms.Label lblCosto;
        private System.Windows.Forms.TextBox txtCosto;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnActivar;
    }
}
