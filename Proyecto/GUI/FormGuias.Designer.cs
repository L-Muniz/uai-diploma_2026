namespace GUI
{
    partial class FormGuias
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
            this.dgvGuias = new System.Windows.Forms.DataGridView();
            this.lblMaterial = new System.Windows.Forms.Label();
            this.cboMaterial = new System.Windows.Forms.ComboBox();
            this.lblTituloGuia = new System.Windows.Forms.Label();
            this.txtTitulo = new System.Windows.Forms.TextBox();
            this.lblDificultad = new System.Windows.Forms.Label();
            this.cboDificultad = new System.Windows.Forms.ComboBox();
            this.lblPasos = new System.Windows.Forms.Label();
            this.txtPasos = new System.Windows.Forms.TextBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
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
            this.lblTitulo.Text = "Guías de reutilización";
            //
            // dgvGuias
            //
            this.dgvGuias.AllowUserToAddRows = false;
            this.dgvGuias.AllowUserToDeleteRows = false;
            this.dgvGuias.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvGuias.Location = new System.Drawing.Point(16, 40);
            this.dgvGuias.MultiSelect = false;
            this.dgvGuias.Name = "dgvGuias";
            this.dgvGuias.ReadOnly = true;
            this.dgvGuias.RowHeadersVisible = false;
            this.dgvGuias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGuias.Size = new System.Drawing.Size(614, 200);
            this.dgvGuias.TabIndex = 1;
            //
            // lblMaterial
            //
            this.lblMaterial.AutoSize = true;
            this.lblMaterial.Location = new System.Drawing.Point(16, 256);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(52, 15);
            this.lblMaterial.TabIndex = 2;
            this.lblMaterial.Text = "Material:";
            //
            // cboMaterial
            //
            this.cboMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaterial.FormattingEnabled = true;
            this.cboMaterial.Location = new System.Drawing.Point(110, 253);
            this.cboMaterial.Name = "cboMaterial";
            this.cboMaterial.Size = new System.Drawing.Size(250, 23);
            this.cboMaterial.TabIndex = 3;
            //
            // lblTituloGuia
            //
            this.lblTituloGuia.AutoSize = true;
            this.lblTituloGuia.Location = new System.Drawing.Point(16, 286);
            this.lblTituloGuia.Name = "lblTituloGuia";
            this.lblTituloGuia.Size = new System.Drawing.Size(40, 15);
            this.lblTituloGuia.TabIndex = 4;
            this.lblTituloGuia.Text = "Título:";
            //
            // txtTitulo
            //
            this.txtTitulo.Location = new System.Drawing.Point(110, 283);
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.Size = new System.Drawing.Size(520, 23);
            this.txtTitulo.TabIndex = 5;
            //
            // lblDificultad
            //
            this.lblDificultad.AutoSize = true;
            this.lblDificultad.Location = new System.Drawing.Point(16, 316);
            this.lblDificultad.Name = "lblDificultad";
            this.lblDificultad.Size = new System.Drawing.Size(65, 15);
            this.lblDificultad.TabIndex = 6;
            this.lblDificultad.Text = "Dificultad:";
            //
            // cboDificultad
            //
            this.cboDificultad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDificultad.FormattingEnabled = true;
            this.cboDificultad.Items.AddRange(new object[] { "Baja", "Media", "Alta" });
            this.cboDificultad.Location = new System.Drawing.Point(110, 313);
            this.cboDificultad.Name = "cboDificultad";
            this.cboDificultad.Size = new System.Drawing.Size(120, 23);
            this.cboDificultad.TabIndex = 7;
            //
            // lblPasos
            //
            this.lblPasos.AutoSize = true;
            this.lblPasos.Location = new System.Drawing.Point(16, 346);
            this.lblPasos.Name = "lblPasos";
            this.lblPasos.Size = new System.Drawing.Size(42, 15);
            this.lblPasos.TabIndex = 8;
            this.lblPasos.Text = "Pasos:";
            //
            // txtPasos
            //
            this.txtPasos.Location = new System.Drawing.Point(110, 343);
            this.txtPasos.Multiline = true;
            this.txtPasos.Name = "txtPasos";
            this.txtPasos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPasos.Size = new System.Drawing.Size(520, 90);
            this.txtPasos.TabIndex = 9;
            //
            // btnNuevo
            //
            this.btnNuevo.Location = new System.Drawing.Point(16, 452);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(120, 28);
            this.btnNuevo.TabIndex = 10;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(150, 452);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(120, 28);
            this.btnGuardar.TabIndex = 11;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnEliminar
            //
            this.btnEliminar.Location = new System.Drawing.Point(284, 452);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(120, 28);
            this.btnEliminar.TabIndex = 12;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            //
            // FormGuias
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 500);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.txtPasos);
            this.Controls.Add(this.lblPasos);
            this.Controls.Add(this.cboDificultad);
            this.Controls.Add(this.lblDificultad);
            this.Controls.Add(this.txtTitulo);
            this.Controls.Add(this.lblTituloGuia);
            this.Controls.Add(this.cboMaterial);
            this.Controls.Add(this.lblMaterial);
            this.Controls.Add(this.dgvGuias);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormGuias";
            this.Text = "Guías de reutilización";
            ((System.ComponentModel.ISupportInitialize)(this.dgvGuias)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvGuias;
        private System.Windows.Forms.Label lblMaterial;
        private System.Windows.Forms.ComboBox cboMaterial;
        private System.Windows.Forms.Label lblTituloGuia;
        private System.Windows.Forms.TextBox txtTitulo;
        private System.Windows.Forms.Label lblDificultad;
        private System.Windows.Forms.ComboBox cboDificultad;
        private System.Windows.Forms.Label lblPasos;
        private System.Windows.Forms.TextBox txtPasos;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEliminar;
    }
}
