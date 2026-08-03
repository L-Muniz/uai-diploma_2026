namespace GUI
{
    partial class FormSnapshots
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvSnapshots;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Label lblSnapshots;

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
            this.dgvSnapshots = new System.Windows.Forms.DataGridView();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.lblSnapshots = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSnapshots)).BeginInit();
            this.SuspendLayout();

            this.lblSnapshots.AutoSize = true;
            this.lblSnapshots.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.lblSnapshots.Location = new System.Drawing.Point(12, 9);
            this.lblSnapshots.Name = "lblSnapshots";
            this.lblSnapshots.Size = new System.Drawing.Size(100, 16);
            this.lblSnapshots.TabIndex = 0;
            this.lblSnapshots.Text = "Snapshots";

            this.dgvSnapshots.AllowUserToAddRows = false;
            this.dgvSnapshots.AllowUserToDeleteRows = false;
            this.dgvSnapshots.AllowUserToResizeRows = false;
            this.dgvSnapshots.BackgroundColor = System.Drawing.Color.White;
            this.dgvSnapshots.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSnapshots.Location = new System.Drawing.Point(12, 35);
            this.dgvSnapshots.Name = "dgvSnapshots";
            this.dgvSnapshots.ReadOnly = true;
            this.dgvSnapshots.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSnapshots.Size = new System.Drawing.Size(760, 400);
            this.dgvSnapshots.TabIndex = 1;

            this.btnRestaurar.Location = new System.Drawing.Point(12, 445);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(100, 26);
            this.btnRestaurar.TabIndex = 2;
            this.btnRestaurar.Text = "Restaurar";
            this.btnRestaurar.UseVisualStyleBackColor = true;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);

            this.btnEliminar.Location = new System.Drawing.Point(118, 445);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(100, 26);
            this.btnEliminar.TabIndex = 3;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 481);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnRestaurar);
            this.Controls.Add(this.dgvSnapshots);
            this.Controls.Add(this.lblSnapshots);
            this.Name = "FormSnapshots";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Snapshots";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSnapshots)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
