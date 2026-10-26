namespace GUI
{
    partial class FormReclamos
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPresentar = new System.Windows.Forms.TabPage();
            this.dgvOrdenes = new System.Windows.Forms.DataGridView();
            this.lblMotivo = new System.Windows.Forms.Label();
            this.txtMotivo = new System.Windows.Forms.TextBox();
            this.btnPresentar = new System.Windows.Forms.Button();
            this.tabResolver = new System.Windows.Forms.TabPage();
            this.dgvReclamos = new System.Windows.Forms.DataGridView();
            this.btnProcedente = new System.Windows.Forms.Button();
            this.btnImprocedente = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPresentar.SuspendLayout();
            this.tabResolver.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReclamos)).BeginInit();
            this.SuspendLayout();
            //
            // tabControl
            //
            this.tabControl.Controls.Add(this.tabPresentar);
            this.tabControl.Controls.Add(this.tabResolver);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(626, 460);
            this.tabControl.TabIndex = 0;
            //
            // tabPresentar
            //
            this.tabPresentar.Controls.Add(this.dgvOrdenes);
            this.tabPresentar.Controls.Add(this.lblMotivo);
            this.tabPresentar.Controls.Add(this.txtMotivo);
            this.tabPresentar.Controls.Add(this.btnPresentar);
            this.tabPresentar.Location = new System.Drawing.Point(4, 24);
            this.tabPresentar.Name = "tabPresentar";
            this.tabPresentar.Padding = new System.Windows.Forms.Padding(3);
            this.tabPresentar.Size = new System.Drawing.Size(618, 432);
            this.tabPresentar.TabIndex = 0;
            this.tabPresentar.Text = "Presentar reclamo";
            this.tabPresentar.UseVisualStyleBackColor = true;
            //
            // dgvOrdenes
            //
            this.dgvOrdenes.AllowUserToAddRows = false;
            this.dgvOrdenes.AllowUserToDeleteRows = false;
            this.dgvOrdenes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrdenes.Location = new System.Drawing.Point(10, 10);
            this.dgvOrdenes.MultiSelect = false;
            this.dgvOrdenes.Name = "dgvOrdenes";
            this.dgvOrdenes.ReadOnly = true;
            this.dgvOrdenes.RowHeadersVisible = false;
            this.dgvOrdenes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrdenes.Size = new System.Drawing.Size(596, 280);
            this.dgvOrdenes.TabIndex = 0;
            //
            // lblMotivo
            //
            this.lblMotivo.AutoSize = true;
            this.lblMotivo.Location = new System.Drawing.Point(10, 302);
            this.lblMotivo.Name = "lblMotivo";
            this.lblMotivo.Size = new System.Drawing.Size(48, 15);
            this.lblMotivo.TabIndex = 1;
            this.lblMotivo.Text = "Motivo:";
            //
            // txtMotivo
            //
            this.txtMotivo.Location = new System.Drawing.Point(70, 299);
            this.txtMotivo.Name = "txtMotivo";
            this.txtMotivo.Size = new System.Drawing.Size(536, 23);
            this.txtMotivo.TabIndex = 2;
            //
            // btnPresentar
            //
            this.btnPresentar.Location = new System.Drawing.Point(10, 336);
            this.btnPresentar.Name = "btnPresentar";
            this.btnPresentar.Size = new System.Drawing.Size(200, 28);
            this.btnPresentar.TabIndex = 3;
            this.btnPresentar.Text = "Presentar reclamo";
            this.btnPresentar.UseVisualStyleBackColor = true;
            this.btnPresentar.Click += new System.EventHandler(this.btnPresentar_Click);
            //
            // tabResolver
            //
            this.tabResolver.Controls.Add(this.dgvReclamos);
            this.tabResolver.Controls.Add(this.btnProcedente);
            this.tabResolver.Controls.Add(this.btnImprocedente);
            this.tabResolver.Location = new System.Drawing.Point(4, 24);
            this.tabResolver.Name = "tabResolver";
            this.tabResolver.Padding = new System.Windows.Forms.Padding(3);
            this.tabResolver.Size = new System.Drawing.Size(618, 432);
            this.tabResolver.TabIndex = 1;
            this.tabResolver.Text = "Resolver reclamos";
            this.tabResolver.UseVisualStyleBackColor = true;
            //
            // dgvReclamos
            //
            this.dgvReclamos.AllowUserToAddRows = false;
            this.dgvReclamos.AllowUserToDeleteRows = false;
            this.dgvReclamos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReclamos.Location = new System.Drawing.Point(10, 10);
            this.dgvReclamos.MultiSelect = false;
            this.dgvReclamos.Name = "dgvReclamos";
            this.dgvReclamos.ReadOnly = true;
            this.dgvReclamos.RowHeadersVisible = false;
            this.dgvReclamos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReclamos.Size = new System.Drawing.Size(596, 350);
            this.dgvReclamos.TabIndex = 0;
            //
            // btnProcedente
            //
            this.btnProcedente.Location = new System.Drawing.Point(10, 370);
            this.btnProcedente.Name = "btnProcedente";
            this.btnProcedente.Size = new System.Drawing.Size(180, 28);
            this.btnProcedente.TabIndex = 1;
            this.btnProcedente.Text = "Reclamo procedente";
            this.btnProcedente.UseVisualStyleBackColor = true;
            this.btnProcedente.Click += new System.EventHandler(this.btnProcedente_Click);
            //
            // btnImprocedente
            //
            this.btnImprocedente.Location = new System.Drawing.Point(200, 370);
            this.btnImprocedente.Name = "btnImprocedente";
            this.btnImprocedente.Size = new System.Drawing.Size(180, 28);
            this.btnImprocedente.TabIndex = 2;
            this.btnImprocedente.Text = "Reclamo improcedente";
            this.btnImprocedente.UseVisualStyleBackColor = true;
            this.btnImprocedente.Click += new System.EventHandler(this.btnImprocedente_Click);
            //
            // FormReclamos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 484);
            this.Controls.Add(this.tabControl);
            this.Name = "FormReclamos";
            this.Text = "Reclamos de pedidos";
            this.tabControl.ResumeLayout(false);
            this.tabPresentar.ResumeLayout(false);
            this.tabPresentar.PerformLayout();
            this.tabResolver.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdenes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReclamos)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPresentar;
        private System.Windows.Forms.DataGridView dgvOrdenes;
        private System.Windows.Forms.Label lblMotivo;
        private System.Windows.Forms.TextBox txtMotivo;
        private System.Windows.Forms.Button btnPresentar;
        private System.Windows.Forms.TabPage tabResolver;
        private System.Windows.Forms.DataGridView dgvReclamos;
        private System.Windows.Forms.Button btnProcedente;
        private System.Windows.Forms.Button btnImprocedente;
    }
}
