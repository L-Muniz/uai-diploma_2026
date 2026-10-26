using System;
using System.Globalization;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU09: el comprador elige un producto, indica el domicilio y paga el pedido con la pasarela.
    public partial class FormCompraMarketplace : Form
    {
        private readonly BE_USUARIO comprador;
        private readonly BLL_MARKETPLACE bllMarketplace = new BLL_MARKETPLACE();
        private readonly IPasarelaPago pasarela = new PasarelaSimulada();

        public FormCompraMarketplace(BE_USUARIO comprador)
        {
            InitializeComponent();
            this.comprador = comprador;
            dgvCatalogo.SelectionChanged += (s, e) => ActualizarResumen();
            UIHelper.CargarGrilla(dgvCatalogo, bllMarketplace.ListarCatalogo());
            ActualizarResumen();
        }

        private void numCantidad_ValueChanged(object sender, EventArgs e)
        {
            ActualizarResumen();
        }

        private void ActualizarResumen()
        {
            if (dgvCatalogo.CurrentRow == null)
            {
                lblResumen.Text = "Seleccione un producto.";
                return;
            }

            decimal precio = Convert.ToDecimal(dgvCatalogo.CurrentRow.Cells["Precio"].Value);
            string nombre = dgvCatalogo.CurrentRow.Cells["Producto"].Value.ToString();
            decimal total = precio * numCantidad.Value;
            lblResumen.Text = $"{numCantidad.Value} x {nombre} = {total.ToString("0.00", CultureInfo.InvariantCulture)}";
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCatalogo.CurrentRow == null)
                    throw new ArgumentException("Seleccione un producto.");

                int idProducto = Convert.ToInt32(dgvCatalogo.CurrentRow.Cells["ID"].Value);
                if (!UIHelper.Confirmar($"¿Confirmar el pedido?\r\n{lblResumen.Text}"))
                    return;

                int idOrden = bllMarketplace.Comprar(comprador, idProducto, (int)numCantidad.Value,
                                                     txtDireccion.Text.Trim(), pasarela);
                UIHelper.Informar($"Pago aprobado. Orden {idOrden} registrada. Se le notificará cuando el vendedor la despache.");
                UIHelper.CargarGrilla(dgvCatalogo, bllMarketplace.ListarCatalogo());
                ActualizarResumen();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
