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
            this.lblServidor = new System.Windows.Forms.Label();
            this.cboServidor = new System.Windows.Forms.ComboBox();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.lblBase = new System.Windows.Forms.Label();
            this.txtBase = new System.Windows.Forms.TextBox();
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
            // lblServidor
            //
            this.lblServidor.AutoSize = true;
            this.lblServidor.Location = new System.Drawing.Point(16, 50);
            this.lblServidor.Name = "lblServidor";
            this.lblServidor.Size = new System.Drawing.Size(125, 15);
            this.lblServidor.TabIndex = 1;
            this.lblServidor.Text = "Instancia de SQL Server:";
            //
            // cboServidor
            //
            this.cboServidor.FormattingEnabled = true;
            this.cboServidor.Location = new System.Drawing.Point(160, 47);
            this.cboServidor.Name = "cboServidor";
            this.cboServidor.Size = new System.Drawing.Size(340, 23);
            this.cboServidor.TabIndex = 2;
            //
            // btnActualizar
            //
            this.btnActualizar.Location = new System.Drawing.Point(506, 45);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(110, 27);
            this.btnActualizar.TabIndex = 3;
            this.btnActualizar.Text = "Buscar instancias";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            //
            // lblBase
            //
            this.lblBase.AutoSize = true;
            this.lblBase.Location = new System.Drawing.Point(16, 84);
            this.lblBase.Name = "lblBase";
            this.lblBase.Size = new System.Drawing.Size(117, 15);
            this.lblBase.TabIndex = 4;
            this.lblBase.Text = "Nombre de la base:";
            //
            // txtBase
            //
            this.txtBase.Location = new System.Drawing.Point(160, 81);
            this.txtBase.Name = "txtBase";
            this.txtBase.Size = new System.Drawing.Size(200, 23);
            this.txtBase.TabIndex = 5;
            this.txtBase.Text = "Proyecto";
            //
            // lblDestino
            //
            this.lblDestino.AutoSize = true;
            this.lblDestino.Location = new System.Drawing.Point(16, 118);
            this.lblDestino.Name = "lblDestino";
            this.lblDestino.Size = new System.Drawing.Size(128, 15);
            this.lblDestino.TabIndex = 6;
            this.lblDestino.Text = "Carpeta de instalación:";
            //
            // txtDestino
            //
            this.txtDestino.Location = new System.Drawing.Point(160, 115);
            this.txtDestino.Name = "txtDestino";
            this.txtDestino.Size = new System.Drawing.Size(340, 23);
            this.txtDestino.TabIndex = 7;
            //
            // btnExaminar
            //
            this.btnExaminar.Location = new System.Drawing.Point(506, 113);
            this.btnExaminar.Name = "btnExaminar";
            this.btnExaminar.Size = new System.Drawing.Size(110, 27);
            this.btnExaminar.TabIndex = 8;
            this.btnExaminar.Text = "Examinar...";
            this.btnExaminar.UseVisualStyleBackColor = true;
            this.btnExaminar.Click += new System.EventHandler(this.btnExaminar_Click);
            //
            // lblAyuda
            //
            this.lblAyuda.Location = new System.Drawing.Point(16, 152);
            this.lblAyuda.Name = "lblAyuda";
            this.lblAyuda.Size = new System.Drawing.Size(600, 40);
            this.lblAyuda.TabIndex = 9;
            this.lblAyuda.Text = "El usuario de Windows que ejecuta el instalador necesita permiso para crear bases de datos " +
                                 "(sysadmin o dbcreator en SQL Server). Si no hay instancias, instalá SQL Server Express.";
            //
            // btnInstalar
            //
            this.btnInstalar.Location = new System.Drawing.Point(16, 196);
            this.btnInstalar.Name = "btnInstalar";
            this.btnInstalar.Size = new System.Drawing.Size(160, 30);
            this.btnInstalar.TabIndex = 10;
            this.btnInstalar.Text = "Instalar";
            this.btnInstalar.UseVisualStyleBackColor = true;
            this.btnInstalar.Click += new System.EventHandler(this.btnInstalar_Click);
            //
            // txtLog
            //
            this.txtLog.Location = new System.Drawing.Point(16, 236);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(600, 300);
            this.txtLog.TabIndex = 11;
            //
            // FormInstalador
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 552);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnInstalar);
            this.Controls.Add(this.lblAyuda);
            this.Controls.Add(this.btnExaminar);
            this.Controls.Add(this.txtDestino);
            this.Controls.Add(this.lblDestino);
            this.Controls.Add(this.txtBase);
            this.Controls.Add(this.lblBase);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.cboServidor);
            this.Controls.Add(this.lblServidor);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormInstalador";
            this.Text = "Instalador BecoGre";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblServidor;
        private System.Windows.Forms.ComboBox cboServidor;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Label lblBase;
        private System.Windows.Forms.TextBox txtBase;
        private System.Windows.Forms.Label lblDestino;
        private System.Windows.Forms.TextBox txtDestino;
        private System.Windows.Forms.Button btnExaminar;
        private System.Windows.Forms.Label lblAyuda;
        private System.Windows.Forms.Button btnInstalar;
        private System.Windows.Forms.TextBox txtLog;
    }
}
