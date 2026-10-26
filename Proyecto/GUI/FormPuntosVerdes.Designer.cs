namespace GUI
{
    partial class FormPuntosVerdes
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
            this.dgvPuntos = new System.Windows.Forms.DataGridView();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblDireccion = new System.Windows.Forms.Label();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.lblLatitud = new System.Windows.Forms.Label();
            this.txtLatitud = new System.Windows.Forms.TextBox();
            this.lblLongitud = new System.Windows.Forms.Label();
            this.txtLongitud = new System.Windows.Forms.TextBox();
            this.lblHorario = new System.Windows.Forms.Label();
            this.txtHorario = new System.Windows.Forms.TextBox();
            this.chkActivo = new System.Windows.Forms.CheckBox();
            this.lblMateriales = new System.Windows.Forms.Label();
            this.clbMateriales = new System.Windows.Forms.CheckedListBox();
            this.chkValidado = new System.Windows.Forms.CheckBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPuntos)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(16, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(300, 21);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Red de puntos verdes y materiales";
            //
            // dgvPuntos
            //
            this.dgvPuntos.AllowUserToAddRows = false;
            this.dgvPuntos.AllowUserToDeleteRows = false;
            this.dgvPuntos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPuntos.Location = new System.Drawing.Point(16, 40);
            this.dgvPuntos.MultiSelect = false;
            this.dgvPuntos.Name = "dgvPuntos";
            this.dgvPuntos.ReadOnly = true;
            this.dgvPuntos.RowHeadersVisible = false;
            this.dgvPuntos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPuntos.Size = new System.Drawing.Size(754, 170);
            this.dgvPuntos.TabIndex = 1;
            //
            // lblNombre
            //
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(16, 225);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(56, 15);
            this.lblNombre.TabIndex = 2;
            this.lblNombre.Text = "Nombre:";
            //
            // txtNombre
            //
            this.txtNombre.Location = new System.Drawing.Point(110, 222);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(260, 23);
            this.txtNombre.TabIndex = 3;
            //
            // lblDireccion
            //
            this.lblDireccion.AutoSize = true;
            this.lblDireccion.Location = new System.Drawing.Point(16, 255);
            this.lblDireccion.Name = "lblDireccion";
            this.lblDireccion.Size = new System.Drawing.Size(65, 15);
            this.lblDireccion.TabIndex = 4;
            this.lblDireccion.Text = "Dirección:";
            //
            // txtDireccion
            //
            this.txtDireccion.Location = new System.Drawing.Point(110, 252);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(660, 23);
            this.txtDireccion.TabIndex = 5;
            //
            // lblLatitud
            //
            this.lblLatitud.AutoSize = true;
            this.lblLatitud.Location = new System.Drawing.Point(16, 285);
            this.lblLatitud.Name = "lblLatitud";
            this.lblLatitud.Size = new System.Drawing.Size(50, 15);
            this.lblLatitud.TabIndex = 6;
            this.lblLatitud.Text = "Latitud:";
            //
            // txtLatitud
            //
            this.txtLatitud.Location = new System.Drawing.Point(110, 282);
            this.txtLatitud.Name = "txtLatitud";
            this.txtLatitud.Size = new System.Drawing.Size(120, 23);
            this.txtLatitud.TabIndex = 7;
            //
            // lblLongitud
            //
            this.lblLongitud.AutoSize = true;
            this.lblLongitud.Location = new System.Drawing.Point(250, 285);
            this.lblLongitud.Name = "lblLongitud";
            this.lblLongitud.Size = new System.Drawing.Size(58, 15);
            this.lblLongitud.TabIndex = 8;
            this.lblLongitud.Text = "Longitud:";
            //
            // txtLongitud
            //
            this.txtLongitud.Location = new System.Drawing.Point(314, 282);
            this.txtLongitud.Name = "txtLongitud";
            this.txtLongitud.Size = new System.Drawing.Size(120, 23);
            this.txtLongitud.TabIndex = 9;
            //
            // lblHorario
            //
            this.lblHorario.AutoSize = true;
            this.lblHorario.Location = new System.Drawing.Point(16, 315);
            this.lblHorario.Name = "lblHorario";
            this.lblHorario.Size = new System.Drawing.Size(51, 15);
            this.lblHorario.TabIndex = 10;
            this.lblHorario.Text = "Horario:";
            //
            // txtHorario
            //
            this.txtHorario.Location = new System.Drawing.Point(110, 312);
            this.txtHorario.Name = "txtHorario";
            this.txtHorario.Size = new System.Drawing.Size(660, 23);
            this.txtHorario.TabIndex = 11;
            //
            // chkActivo
            //
            this.chkActivo.AutoSize = true;
            this.chkActivo.Checked = true;
            this.chkActivo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActivo.Location = new System.Drawing.Point(110, 345);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new System.Drawing.Size(60, 19);
            this.chkActivo.TabIndex = 12;
            this.chkActivo.Text = "Activo";
            this.chkActivo.UseVisualStyleBackColor = true;
            //
            // lblMateriales
            //
            this.lblMateriales.AutoSize = true;
            this.lblMateriales.Location = new System.Drawing.Point(16, 375);
            this.lblMateriales.Name = "lblMateriales";
            this.lblMateriales.Size = new System.Drawing.Size(125, 15);
            this.lblMateriales.TabIndex = 13;
            this.lblMateriales.Text = "Materiales aceptados:";
            //
            // clbMateriales
            //
            this.clbMateriales.CheckOnClick = true;
            this.clbMateriales.FormattingEnabled = true;
            this.clbMateriales.Location = new System.Drawing.Point(150, 372);
            this.clbMateriales.Name = "clbMateriales";
            this.clbMateriales.Size = new System.Drawing.Size(260, 112);
            this.clbMateriales.TabIndex = 14;
            //
            // chkValidado
            //
            this.chkValidado.AutoSize = true;
            this.chkValidado.Location = new System.Drawing.Point(430, 375);
            this.chkValidado.Name = "chkValidado";
            this.chkValidado.Size = new System.Drawing.Size(340, 19);
            this.chkValidado.TabIndex = 15;
            this.chkValidado.Text = "Locación validada por el referente de cooperativas";
            this.chkValidado.UseVisualStyleBackColor = true;
            //
            // btnNuevo
            //
            this.btnNuevo.Location = new System.Drawing.Point(16, 510);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(120, 28);
            this.btnNuevo.TabIndex = 16;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(150, 510);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(180, 28);
            this.btnGuardar.TabIndex = 17;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // FormPuntosVerdes
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 560);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.chkValidado);
            this.Controls.Add(this.clbMateriales);
            this.Controls.Add(this.lblMateriales);
            this.Controls.Add(this.chkActivo);
            this.Controls.Add(this.txtHorario);
            this.Controls.Add(this.lblHorario);
            this.Controls.Add(this.txtLongitud);
            this.Controls.Add(this.lblLongitud);
            this.Controls.Add(this.txtLatitud);
            this.Controls.Add(this.lblLatitud);
            this.Controls.Add(this.txtDireccion);
            this.Controls.Add(this.lblDireccion);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.dgvPuntos);
            this.Controls.Add(this.lblTitulo);
            this.Name = "FormPuntosVerdes";
            this.Text = "Red de puntos verdes";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPuntos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvPuntos;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblDireccion;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Label lblLatitud;
        private System.Windows.Forms.TextBox txtLatitud;
        private System.Windows.Forms.Label lblLongitud;
        private System.Windows.Forms.TextBox txtLongitud;
        private System.Windows.Forms.Label lblHorario;
        private System.Windows.Forms.TextBox txtHorario;
        private System.Windows.Forms.CheckBox chkActivo;
        private System.Windows.Forms.Label lblMateriales;
        private System.Windows.Forms.CheckedListBox clbMateriales;
        private System.Windows.Forms.CheckBox chkValidado;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
    }
}
