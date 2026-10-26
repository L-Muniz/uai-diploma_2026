namespace GUI
{
    partial class FormSuscripciones
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
            this.lblMateriales = new System.Windows.Forms.Label();
            this.clbMateriales = new System.Windows.Forms.CheckedListBox();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.lblGuias = new System.Windows.Forms.Label();
            this.dgvGuias = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGuias)).BeginInit();
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
            this.lblTitulo.Text = "Mis materiales (suscripciones)";
            //
            // lblMateriales
            //
            this.lblMateriales.AutoSize = true;
            this.lblMateriales.Location = new System.Drawing.Point(16, 44);
            this.lblMateriales.Name = "lblMateriales";
            this.lblMateriales.Size = new System.Drawing.Size(200, 15);
            this.lblMateriales.TabIndex = 1;
            this.lblMateriales.Text = "Seleccione los materiales que sigue:";
            //
            // clbMateriales
            //
            this.clbMateriales.CheckOnClick = true;
            this.clbMateriales.FormattingEnabled = true;
            this.clbMateriales.Location = new System.Drawing.Point(16, 64);
            this.clbMateriales.Name = "clbMateriales";
            this.clbMateriales.Size = new System.Drawing.Size(300, 130);
            this.clbMateriales.TabIndex = 2;
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(16, 204);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(180, 28);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar suscripciones";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // lblGuias
            //
            this.lblGuias.AutoSize = true;
            this.lblGuias.Location = new System.Drawing.Point(16, 248);
            this.lblGuias.Name = "lblGuias";
            this.lblGuias.Size = new System.Drawing.Size(260, 15);
            this.lblGuias.TabIndex = 4;
            this.lblGuias.Text = "Guías de reutilización de sus materiales:";
            //
            // dgvGuias
            //
            this.dgvGuias.AllowUserToAddRows = false;
            this.dgvGuias.AllowUserToDeleteRows = false;
            this.dgvGuias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGuias.Location = new System.Drawing.Point(16, 268);
            this.dgvGuias.Name = "dgvGuias";
            this.dgvGuias.ReadOnly = true;
            this.dgvGuias.RowHeadersVisible = false;
            this.dgvGuias.Size = new System.Drawing.Size(614, 240);
            this.dgvGuias.TabIndex = 5;
            //
            // FormSuscripciones
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 530);
            this.Controls.Add(this.dgvGuias);
            this.Controls.Add(this.lblGuias);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.clbMateriales);
            this.Controls.Add(this.lblMateriales);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormSuscripciones";
            this.Text = "Mis materiales";
            ((System.ComponentModel.ISupportInitialize)(this.dgvGuias)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblMateriales;
        private System.Windows.Forms.CheckedListBox clbMateriales;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Label lblGuias;
        private System.Windows.Forms.DataGridView dgvGuias;
    }
}
