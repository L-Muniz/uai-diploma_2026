namespace GUI
{
    partial class FormDeclararMaterial
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
            this.lblMaterial = new System.Windows.Forms.Label();
            this.cboMaterial = new System.Windows.Forms.ComboBox();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.btnDeclarar = new System.Windows.Forms.Button();
            this.gbOpciones = new System.Windows.Forms.GroupBox();
            this.lblGuia = new System.Windows.Forms.Label();
            this.lblPuntoVerde = new System.Windows.Forms.Label();
            this.btnReutilizar = new System.Windows.Forms.Button();
            this.btnEntregar = new System.Windows.Forms.Button();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblHistorial = new System.Windows.Forms.Label();
            this.dgvDeclaraciones = new System.Windows.Forms.DataGridView();
            this.gbOpciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeclaraciones)).BeginInit();
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
            this.lblTitulo.Text = "Declarar material y recibir recomendación";
            //
            // lblMaterial
            //
            this.lblMaterial.AutoSize = true;
            this.lblMaterial.Location = new System.Drawing.Point(16, 50);
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(52, 15);
            this.lblMaterial.TabIndex = 1;
            this.lblMaterial.Text = "Material:";
            //
            // cboMaterial
            //
            this.cboMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaterial.FormattingEnabled = true;
            this.cboMaterial.Location = new System.Drawing.Point(90, 47);
            this.cboMaterial.Name = "cboMaterial";
            this.cboMaterial.Size = new System.Drawing.Size(200, 23);
            this.cboMaterial.TabIndex = 2;
            //
            // lblCantidad
            //
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(306, 50);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(93, 15);
            this.lblCantidad.TabIndex = 3;
            this.lblCantidad.Text = "Cantidad (kg):";
            //
            // txtCantidad
            //
            this.txtCantidad.Location = new System.Drawing.Point(405, 47);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Size = new System.Drawing.Size(80, 23);
            this.txtCantidad.TabIndex = 4;
            this.txtCantidad.Text = "1";
            //
            // btnDeclarar
            //
            this.btnDeclarar.Location = new System.Drawing.Point(500, 45);
            this.btnDeclarar.Name = "btnDeclarar";
            this.btnDeclarar.Size = new System.Drawing.Size(130, 27);
            this.btnDeclarar.TabIndex = 5;
            this.btnDeclarar.Text = "Declarar";
            this.btnDeclarar.UseVisualStyleBackColor = true;
            this.btnDeclarar.Click += new System.EventHandler(this.btnDeclarar_Click);
            //
            // gbOpciones
            //
            this.gbOpciones.Controls.Add(this.lblGuia);
            this.gbOpciones.Controls.Add(this.lblPuntoVerde);
            this.gbOpciones.Controls.Add(this.btnReutilizar);
            this.gbOpciones.Controls.Add(this.btnEntregar);
            this.gbOpciones.Location = new System.Drawing.Point(16, 90);
            this.gbOpciones.Name = "gbOpciones";
            this.gbOpciones.Size = new System.Drawing.Size(614, 170);
            this.gbOpciones.TabIndex = 6;
            this.gbOpciones.TabStop = false;
            this.gbOpciones.Text = "Recomendación";
            //
            // lblGuia
            //
            this.lblGuia.Location = new System.Drawing.Point(14, 24);
            this.lblGuia.Name = "lblGuia";
            this.lblGuia.Size = new System.Drawing.Size(580, 60);
            this.lblGuia.TabIndex = 0;
            this.lblGuia.Text = "Opción 1: reutilizar en casa (declare un material primero).";
            //
            // lblPuntoVerde
            //
            this.lblPuntoVerde.Location = new System.Drawing.Point(14, 90);
            this.lblPuntoVerde.Name = "lblPuntoVerde";
            this.lblPuntoVerde.Size = new System.Drawing.Size(580, 30);
            this.lblPuntoVerde.TabIndex = 1;
            this.lblPuntoVerde.Text = "Opción 2: entregar en el punto verde más cercano.";
            //
            // btnReutilizar
            //
            this.btnReutilizar.Enabled = false;
            this.btnReutilizar.Location = new System.Drawing.Point(14, 128);
            this.btnReutilizar.Name = "btnReutilizar";
            this.btnReutilizar.Size = new System.Drawing.Size(200, 30);
            this.btnReutilizar.TabIndex = 2;
            this.btnReutilizar.Text = "Reutilizar en casa (confirmar)";
            this.btnReutilizar.UseVisualStyleBackColor = true;
            this.btnReutilizar.Click += new System.EventHandler(this.btnReutilizar_Click);
            //
            // btnEntregar
            //
            this.btnEntregar.Enabled = false;
            this.btnEntregar.Location = new System.Drawing.Point(230, 128);
            this.btnEntregar.Name = "btnEntregar";
            this.btnEntregar.Size = new System.Drawing.Size(200, 30);
            this.btnEntregar.TabIndex = 3;
            this.btnEntregar.Text = "Entregar en punto verde";
            this.btnEntregar.UseVisualStyleBackColor = true;
            this.btnEntregar.Click += new System.EventHandler(this.btnEntregar_Click);
            //
            // lblSaldo
            //
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblSaldo.Location = new System.Drawing.Point(16, 272);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(120, 17);
            this.lblSaldo.TabIndex = 7;
            this.lblSaldo.Text = "Saldo: 0 Eco-Puntos";
            //
            // lblHistorial
            //
            this.lblHistorial.AutoSize = true;
            this.lblHistorial.Location = new System.Drawing.Point(16, 300);
            this.lblHistorial.Name = "lblHistorial";
            this.lblHistorial.Size = new System.Drawing.Size(120, 15);
            this.lblHistorial.TabIndex = 8;
            this.lblHistorial.Text = "Mis declaraciones:";
            //
            // dgvDeclaraciones
            //
            this.dgvDeclaraciones.AllowUserToAddRows = false;
            this.dgvDeclaraciones.AllowUserToDeleteRows = false;
            this.dgvDeclaraciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDeclaraciones.Location = new System.Drawing.Point(16, 320);
            this.dgvDeclaraciones.MultiSelect = false;
            this.dgvDeclaraciones.Name = "dgvDeclaraciones";
            this.dgvDeclaraciones.ReadOnly = true;
            this.dgvDeclaraciones.RowHeadersVisible = false;
            this.dgvDeclaraciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeclaraciones.Size = new System.Drawing.Size(614, 200);
            this.dgvDeclaraciones.TabIndex = 9;
            //
            // FormDeclararMaterial
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 540);
            this.Controls.Add(this.dgvDeclaraciones);
            this.Controls.Add(this.lblHistorial);
            this.Controls.Add(this.lblSaldo);
            this.Controls.Add(this.gbOpciones);
            this.Controls.Add(this.btnDeclarar);
            this.Controls.Add(this.txtCantidad);
            this.Controls.Add(this.lblCantidad);
            this.Controls.Add(this.cboMaterial);
            this.Controls.Add(this.lblMaterial);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormDeclararMaterial";
            this.Text = "Declarar material";
            this.gbOpciones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeclaraciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblMaterial;
        private System.Windows.Forms.ComboBox cboMaterial;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.Button btnDeclarar;
        private System.Windows.Forms.GroupBox gbOpciones;
        private System.Windows.Forms.Label lblGuia;
        private System.Windows.Forms.Label lblPuntoVerde;
        private System.Windows.Forms.Button btnReutilizar;
        private System.Windows.Forms.Button btnEntregar;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblHistorial;
        private System.Windows.Forms.DataGridView dgvDeclaraciones;
    }
}
