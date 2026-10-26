using System;
using System.Globalization;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU08: el vendedor verificado publica productos; el área comercial los aprueba para la venta.
    public partial class FormPublicarProducto : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_MARKETPLACE bllMarketplace = new BLL_MARKETPLACE();
        private int idProductoSeleccionado;

        public FormPublicarProducto(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            bool puedePublicar = BLL_AUTORIZACION.TienePermiso(usuario, "PROC_PUBLICAR_PRODUCTO");
            bool puedeAprobar = BLL_AUTORIZACION.TienePermiso(usuario, "PROC_APROBAR_PRODUCTO");
            btnPublicar.Enabled = puedePublicar;
            btnActualizar.Enabled = puedePublicar;
            dgvPendientes.Enabled = puedeAprobar;
            btnAprobar.Enabled = puedeAprobar;
            dgvMisProductos.SelectionChanged += (s, e) => CargarProductoSeleccionado();
            txtCategoria.TextChanged += (s, e) => MostrarComision();
            MostrarComision();
            Refrescar();
        }

        private void MostrarComision()
        {
            decimal pct = bllMarketplace.ObtenerComisionEstimada(txtCategoria.Text.Trim());
            lblComision.Text = $"Comisión estimada: {pct.ToString("0.00", CultureInfo.InvariantCulture)}% sobre cada venta.";
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvMisProductos, bllMarketplace.ListarProductosDeVendedor(usuario.ID));
            UIHelper.CargarGrilla(dgvPendientes, bllMarketplace.ListarProductosPendientes());
        }

        private void btnPublicar_Click(object sender, EventArgs e)
        {
            try
            {
                decimal precio = UIHelper.LeerDecimal(txtPrecio.Text, "Precio");
                int id = bllMarketplace.PublicarProducto(usuario, txtNombre.Text.Trim(), txtDescripcion.Text.Trim(),
                                                         txtCategoria.Text.Trim(), precio, (int)numStock.Value);
                UIHelper.Informar($"Producto {id} publicado. Queda pendiente de aprobación del área comercial.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void CargarProductoSeleccionado()
        {
            if (dgvMisProductos.CurrentRow == null) return;
            idProductoSeleccionado = Convert.ToInt32(dgvMisProductos.CurrentRow.Cells["ID"].Value);
            txtNombre.Text = dgvMisProductos.CurrentRow.Cells["Producto"].Value.ToString();
            txtCategoria.Text = dgvMisProductos.CurrentRow.Cells["Categoria"].Value.ToString();
            txtPrecio.Text = Convert.ToDecimal(dgvMisProductos.CurrentRow.Cells["Precio"].Value).ToString("0.00", CultureInfo.InvariantCulture);
            numStock.Value = Convert.ToDecimal(dgvMisProductos.CurrentRow.Cells["Stock"].Value);
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (idProductoSeleccionado == 0)
                    throw new ArgumentException("Seleccione una publicación propia en la lista.");
                decimal precio = UIHelper.LeerDecimal(txtPrecio.Text, "Precio");
                bllMarketplace.ModificarProducto(usuario, idProductoSeleccionado, precio, (int)numStock.Value);
                UIHelper.Informar("Precio y stock actualizados.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnAprobar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPendientes.CurrentRow == null)
                    throw new ArgumentException("Seleccione un producto pendiente.");

                int idProducto = Convert.ToInt32(dgvPendientes.CurrentRow.Cells["ID"].Value);
                bllMarketplace.AprobarProducto(usuario, idProducto);
                UIHelper.Informar("Producto aprobado. Ya está visible en el catálogo.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
