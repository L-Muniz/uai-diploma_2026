namespace GUI
{
    partial class FormBackups
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelBotones;
        private System.Windows.Forms.Button btnBackupAhora;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnRefrescar;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTotalBackups;
        private System.Windows.Forms.Label lblUltimo;
        private System.Windows.Forms.Label lblUltimoBackup;
        private System.Windows.Forms.Label lblUbicacionLabel;
        private System.Windows.Forms.TextBox txtUbicacion;
        private System.Windows.Forms.DataGridView dgvBackups;

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
            this.panelBotones = new System.Windows.Forms.Panel();
            this.btnBackupAhora = new System.Windows.Forms.Button();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnRefrescar = new System.Windows.Forms.Button();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotalBackups = new System.Windows.Forms.Label();
            this.lblUltimo = new System.Windows.Forms.Label();
            this.lblUltimoBackup = new System.Windows.Forms.Label();
            this.lblUbicacionLabel = new System.Windows.Forms.Label();
            this.txtUbicacion = new System.Windows.Forms.TextBox();
            this.dgvBackups = new System.Windows.Forms.DataGridView();
            this.panelBotones.SuspendLayout();
            this.panelInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).BeginInit();
            this.SuspendLayout();

            this.panelBotones.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelBotones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBotones.Controls.Add(this.btnBackupAhora);
            this.panelBotones.Controls.Add(this.btnRestaurar);
            this.panelBotones.Controls.Add(this.btnEliminar);
            this.panelBotones.Controls.Add(this.btnRefrescar);
            this.panelBotones.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBotones.Location = new System.Drawing.Point(0, 0);
            this.panelBotones.Name = "panelBotones";
            this.panelBotones.Size = new System.Drawing.Size(771, 52);
            this.panelBotones.TabIndex = 0;

            this.btnBackupAhora.Location = new System.Drawing.Point(9, 13);
            this.btnBackupAhora.Name = "btnBackupAhora";
            this.btnBackupAhora.Size = new System.Drawing.Size(86, 26);
            this.btnBackupAhora.TabIndex = 0;
            this.btnBackupAhora.Text = "Backup Ahora";
            this.btnBackupAhora.UseVisualStyleBackColor = true;
            this.btnBackupAhora.Click += new System.EventHandler(this.btnBackupAhora_Click);

            this.btnRestaurar.Location = new System.Drawing.Point(103, 13);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(86, 26);
            this.btnRestaurar.TabIndex = 1;
            this.btnRestaurar.Text = "Restaurar";
            this.btnRestaurar.UseVisualStyleBackColor = true;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);

            this.btnEliminar.Location = new System.Drawing.Point(197, 13);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(86, 26);
            this.btnEliminar.TabIndex = 2;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            this.btnRefrescar.Location = new System.Drawing.Point(291, 13);
            this.btnRefrescar.Name = "btnRefrescar";
            this.btnRefrescar.Size = new System.Drawing.Size(86, 26);
            this.btnRefrescar.TabIndex = 3;
            this.btnRefrescar.Text = "Refrescar";
            this.btnRefrescar.UseVisualStyleBackColor = true;
            this.btnRefrescar.Click += new System.EventHandler(this.btnRefrescar_Click);

            this.panelInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInfo.Controls.Add(this.lblInfo);
            this.panelInfo.Controls.Add(this.lblTotal);
            this.panelInfo.Controls.Add(this.lblTotalBackups);
            this.panelInfo.Controls.Add(this.lblUltimo);
            this.panelInfo.Controls.Add(this.lblUltimoBackup);
            this.panelInfo.Controls.Add(this.lblUbicacionLabel);
            this.panelInfo.Controls.Add(this.txtUbicacion);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Location = new System.Drawing.Point(0, 52);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(771, 104);
            this.panelInfo.TabIndex = 1;

            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblInfo.Location = new System.Drawing.Point(9, 9);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(176, 16);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.Text = "Información de Backups";

            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Arial", 9F);
            this.lblTotal.Location = new System.Drawing.Point(9, 30);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(104, 15);
            this.lblTotal.TabIndex = 1;
            this.lblTotal.Text = "Total de Backups:";

            this.lblTotalBackups.AutoSize = true;
            this.lblTotalBackups.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalBackups.Location = new System.Drawing.Point(129, 30);
            this.lblTotalBackups.Name = "lblTotalBackups";
            this.lblTotalBackups.Size = new System.Drawing.Size(14, 15);
            this.lblTotalBackups.TabIndex = 2;
            this.lblTotalBackups.Text = "0";

            this.lblUltimo.AutoSize = true;
            this.lblUltimo.Font = new System.Drawing.Font("Arial", 9F);
            this.lblUltimo.Location = new System.Drawing.Point(9, 48);
            this.lblUltimo.Name = "lblUltimo";
            this.lblUltimo.Size = new System.Drawing.Size(90, 15);
            this.lblUltimo.TabIndex = 3;
            this.lblUltimo.Text = "Último Backup:";

            this.lblUltimoBackup.AutoSize = true;
            this.lblUltimoBackup.Font = new System.Drawing.Font("Arial", 9F);
            this.lblUltimoBackup.Location = new System.Drawing.Point(129, 48);
            this.lblUltimoBackup.Name = "lblUltimoBackup";
            this.lblUltimoBackup.Size = new System.Drawing.Size(26, 15);
            this.lblUltimoBackup.TabIndex = 4;
            this.lblUltimoBackup.Text = "N/A";

            this.lblUbicacionLabel.AutoSize = true;
            this.lblUbicacionLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.lblUbicacionLabel.Location = new System.Drawing.Point(9, 65);
            this.lblUbicacionLabel.Name = "lblUbicacionLabel";
            this.lblUbicacionLabel.Size = new System.Drawing.Size(65, 15);
            this.lblUbicacionLabel.TabIndex = 5;
            this.lblUbicacionLabel.Text = "Ubicación:";

            this.txtUbicacion.Location = new System.Drawing.Point(129, 65);
            this.txtUbicacion.Name = "txtUbicacion";
            this.txtUbicacion.ReadOnly = true;
            this.txtUbicacion.Size = new System.Drawing.Size(558, 20);
            this.txtUbicacion.TabIndex = 6;

            this.dgvBackups.AllowUserToAddRows = false;
            this.dgvBackups.AllowUserToDeleteRows = false;
            this.dgvBackups.AllowUserToResizeRows = false;
            this.dgvBackups.BackgroundColor = System.Drawing.Color.White;
            this.dgvBackups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBackups.GridColor = System.Drawing.Color.LightGray;
            this.dgvBackups.Location = new System.Drawing.Point(0, 156);
            this.dgvBackups.Name = "dgvBackups";
            this.dgvBackups.MultiSelect = true;
            this.dgvBackups.ReadOnly = true;
            this.dgvBackups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBackups.Size = new System.Drawing.Size(771, 410);
            this.dgvBackups.TabIndex = 2;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(771, 566);
            this.Controls.Add(this.dgvBackups);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panelBotones);
            this.Name = "FormBackups";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backups";
            this.panelBotones.ResumeLayout(false);
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
