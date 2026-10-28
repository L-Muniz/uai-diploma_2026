namespace GUI
{
    partial class FormAyuda
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
            this.lblTemas = new System.Windows.Forms.Label();
            this.lstTemas = new System.Windows.Forms.ListBox();
            this.txtContenido = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // lblTemas
            //
            this.lblTemas.AutoSize = true;
            this.lblTemas.Location = new System.Drawing.Point(12, 12);
            this.lblTemas.Name = "lblTemas";
            this.lblTemas.Size = new System.Drawing.Size(45, 15);
            this.lblTemas.TabIndex = 0;
            this.lblTemas.Text = "Temas:";
            //
            // lstTemas
            //
            this.lstTemas.FormattingEnabled = true;
            this.lstTemas.ItemHeight = 15;
            this.lstTemas.Location = new System.Drawing.Point(12, 32);
            this.lstTemas.Name = "lstTemas";
            this.lstTemas.Size = new System.Drawing.Size(230, 439);
            this.lstTemas.TabIndex = 1;
            this.lstTemas.SelectedIndexChanged += new System.EventHandler(this.lstTemas_SelectedIndexChanged);
            //
            // txtContenido
            //
            this.txtContenido.Location = new System.Drawing.Point(256, 32);
            this.txtContenido.Multiline = true;
            this.txtContenido.Name = "txtContenido";
            this.txtContenido.ReadOnly = true;
            this.txtContenido.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContenido.Size = new System.Drawing.Size(480, 439);
            this.txtContenido.TabIndex = 2;
            //
            // FormAyuda
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 486);
            this.Controls.Add(this.txtContenido);
            this.Controls.Add(this.lstTemas);
            this.Controls.Add(this.lblTemas);
            this.Name = "FormAyuda";
            this.Text = "Ayuda en línea";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTemas;
        private System.Windows.Forms.ListBox lstTemas;
        private System.Windows.Forms.TextBox txtContenido;
    }
}
