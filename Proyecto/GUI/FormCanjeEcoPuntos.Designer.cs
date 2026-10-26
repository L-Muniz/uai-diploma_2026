namespace GUI
{
    partial class FormCanjeEcoPuntos
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
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblBeneficios = new System.Windows.Forms.Label();
            this.dgvBeneficios = new System.Windows.Forms.DataGridView();
            this.btnCanjear = new System.Windows.Forms.Button();
            this.lblMovimientos = new System.Windows.Forms.Label();
            this.dgvMovimientos = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeneficios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).BeginInit();
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
            this.lblTitulo.Text = "Canjear Eco-Puntos";
            //
            // lblSaldo
            //
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblSaldo.Location = new System.Drawing.Point(16, 42);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(120, 17);
            this.lblSaldo.TabIndex = 1;
            this.lblSaldo.Text = "Saldo: 0 Eco-Puntos";
            //
            // lblBeneficios
            //
            this.lblBeneficios.AutoSize = true;
            this.lblBeneficios.Location = new System.Drawing.Point(16, 72);
            this.lblBeneficios.Name = "lblBeneficios";
            this.lblBeneficios.Size = new System.Drawing.Size(120, 15);
            this.lblBeneficios.TabIndex = 2;
            this.lblBeneficios.Text = "Beneficios disponibles:";
            //
            // dgvBeneficios
            //
            this.dgvBeneficios.AllowUserToAddRows = false;
            this.dgvBeneficios.AllowUserToDeleteRows = false;
            this.dgvBeneficios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBeneficios.Location = new System.Drawing.Point(16, 92);
            this.dgvBeneficios.MultiSelect = false;
            this.dgvBeneficios.Name = "dgvBeneficios";
            this.dgvBeneficios.ReadOnly = true;
            this.dgvBeneficios.RowHeadersVisible = false;
            this.dgvBeneficios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBeneficios.Size = new System.Drawing.Size(614, 150);
            this.dgvBeneficios.TabIndex = 3;
            //
            // btnCanjear
            //
            this.btnCanjear.Location = new System.Drawing.Point(16, 250);
            this.btnCanjear.Name = "btnCanjear";
            this.btnCanjear.Size = new System.Drawing.Size(200, 28);
            this.btnCanjear.TabIndex = 4;
            this.btnCanjear.Text = "Canjear beneficio seleccionado";
            this.btnCanjear.UseVisualStyleBackColor = true;
            this.btnCanjear.Click += new System.EventHandler(this.btnCanjear_Click);
            //
            // lblMovimientos
            //
            this.lblMovimientos.AutoSize = true;
            this.lblMovimientos.Location = new System.Drawing.Point(16, 292);
            this.lblMovimientos.Name = "lblMovimientos";
            this.lblMovimientos.Size = new System.Drawing.Size(150, 15);
            this.lblMovimientos.TabIndex = 5;
            this.lblMovimientos.Text = "Movimientos de mi cuenta:";
            //
            // dgvMovimientos
            //
            this.dgvMovimientos.AllowUserToAddRows = false;
            this.dgvMovimientos.AllowUserToDeleteRows = false;
            this.dgvMovimientos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMovimientos.Location = new System.Drawing.Point(16, 312);
            this.dgvMovimientos.Name = "dgvMovimientos";
            this.dgvMovimientos.ReadOnly = true;
            this.dgvMovimientos.RowHeadersVisible = false;
            this.dgvMovimientos.Size = new System.Drawing.Size(614, 200);
            this.dgvMovimientos.TabIndex = 6;
            //
            // FormCanjeEcoPuntos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 530);
            this.Controls.Add(this.dgvMovimientos);
            this.Controls.Add(this.lblMovimientos);
            this.Controls.Add(this.btnCanjear);
            this.Controls.Add(this.dgvBeneficios);
            this.Controls.Add(this.lblBeneficios);
            this.Controls.Add(this.lblSaldo);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormCanjeEcoPuntos";
            this.Text = "Canje de Eco-Puntos";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBeneficios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovimientos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblBeneficios;
        private System.Windows.Forms.DataGridView dgvBeneficios;
        private System.Windows.Forms.Button btnCanjear;
        private System.Windows.Forms.Label lblMovimientos;
        private System.Windows.Forms.DataGridView dgvMovimientos;
    }
}
