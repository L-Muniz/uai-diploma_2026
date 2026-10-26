using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace GUI
{
    // Utilidades compartidas por los formularios de los procesos de negocio (3ra entrega).
    public static class UIHelper
    {
        // Elemento para listas de selección múltiple (ID + texto visible).
        public class ItemLista
        {
            public int ID { get; set; }
            public string Texto { get; set; }

            public override string ToString()
            {
                return Texto;
            }
        }

        public static void Informar(string mensaje)
        {
            MessageBox.Show(mensaje, "Procesos de negocio", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Error(Exception ex)
        {
            // Los mensajes de SQL de negocio llegan con THROW; se muestran tal cual.
            MessageBox.Show(ex.Message, "No se pudo completar la operación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static bool Confirmar(string pregunta)
        {
            return MessageBox.Show(pregunta, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public static decimal LeerDecimal(string texto, string nombreCampo)
        {
            decimal valor;
            string normalizado = (texto ?? "").Trim().Replace(',', '.');
            if (!decimal.TryParse(normalizado, NumberStyles.Number, CultureInfo.InvariantCulture, out valor))
                throw new ArgumentException($"{nombreCampo}: ingrese un número válido.");
            return valor;
        }

        public static void CargarCombo(ComboBox combo, DataTable datos, string campoMostrar, string campoValor)
        {
            combo.DataSource = null;
            combo.DisplayMember = campoMostrar;
            combo.ValueMember = campoValor;
            combo.DataSource = datos;
            combo.SelectedIndex = datos.Rows.Count > 0 ? 0 : -1;
        }

        public static void CargarGrilla(DataGridView grilla, DataTable datos)
        {
            grilla.DataSource = null;
            grilla.AutoGenerateColumns = true;
            grilla.DataSource = datos;
        }

        public static int? ValorId(DataGridViewRow fila, string columna)
        {
            if (fila == null || fila.Cells[columna].Value == null || fila.Cells[columna].Value == DBNull.Value)
                return null;
            return Convert.ToInt32(fila.Cells[columna].Value);
        }
    }
}
