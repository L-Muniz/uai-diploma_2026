namespace Instalador
{
    partial class FormInstalador
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
            this.lblDestino = new System.Windows.Forms.Label();
            this.txtDestino = new System.Windows.Forms.TextBox();
            this.btnExaminar = new System.Windows.Forms.Button();
            this.lblAyuda = new System.Windows.Forms.Label();
            this.btnInstalar = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
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
            this.lblTitulo.Text = "Instalador BecoGre";
            //
            // lblDestino
            //
            this.lblDestino.AutoSize = true;
            this.lblDestino.Location = new System.Drawing.Point(16, 52);
            this.lblDestino.Name = "lblDestino";
            this.lblDestino.Size = new System.Drawing.Size(128, 15);
            this.lblDestino.TabIndex = 6;
            this.lblDestino.Text = "Carpeta de instalación:";
            //
            // txtDestino
            //
            this.txtDestino.Location = new System.Drawing.Point(160, 49);
            this.txtDestino.Name = "txtDestino";
            this.txtDestino.Size = new System.Drawing.Size(340, 23);
            this.txtDestino.TabIndex = 7;
            //
            // btnExaminar
            //
            this.btnExaminar.Location = new System.Drawing.Point(506, 47);
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.Size = new System.Drawing.Size(110, 27);
            this.btnExaminar.TabIndex = 8;
            this.btnExaminar.Text = "Examinar...";
            this.btnExaminar.UseVisualStyleBackColor = true;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            //
            // lblAyuda
            //
            this.lblAyuda.Location = new System.Drawing.Point(16, 86);
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.Size = new System.Drawing.Size(600, 40);
            this.lblAyuda.TabIndex = 9;
            this.lblAyuda.Text = "El usuario de Windows que ejecuta el instalador necesita permiso para crear bases de datos " +
                                 "(sysadmin o dbcreator en SQL Server). Si no hay instancias, instalá SQL Server Express.";
            //
            // btnInstalar
            //
            this.btnInstalar.Location = new System.Drawing.Point(16, 130);
            this.btnInstalar.Name = "btnInstalar";
            this.btnInstalar.Size = new System.Drawing.Size(160, 30);
            this.btnInstalar.TabIndex = 10;
            this.btnInstalar.Text = "Instalar";
            this.btnInstalar.UseVisualStyleBackColor = true;
            this.btnInstalar.Click += new System.EventHandler(this.btnInstalar_Click);
            //
            // txtLog
            //
            this.txtLog.Location = new System.Drawing.Point(16, 170);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(600, 340);
            this.txtLog.TabIndex = 11;
            //
            // FormInstalador
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 490);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnInstalar);
            this.Controls.Add(this.lblAyuda);
            this.Controls.Add(this.btnExaminar);
            this.Controls.Add(this.txtDestino);
            this.Controls.Add(this.lblDestino);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormInstalador";
            this.Text = "Instalador BecoGre";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDestino;
        private System.Windows.Forms.TextBox txtDestino;
        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.Label lblAyuda;
        private System.Windows.Forms.Button btnInstalar;
        private System.Windows.Forms.TextBox txtLog;
    }
}
