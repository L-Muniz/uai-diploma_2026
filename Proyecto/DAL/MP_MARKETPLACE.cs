using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    // Verificación de vendedores, productos, órdenes, pagos, comisiones, liquidaciones y reclamos (N02).
    // Todo mediante stored procedures.
    public class MP_MARKETPLACE
    {
        // ---------------- verificación de vendedores (CU07) ----------------

        public int AltaVerificacion(int idUsuario, string estado, string documentoRef)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_USUARIO", idUsuario),
                acc.CrearParametro("@ESTADO", estado),
                acc.CrearParametroObj("@DOCUMENTO", documentoRef)
            };
            int id = Convert.ToInt32(acc.Escalar("VERIFICACION_ALTA", p));
            acc.Cerrar();
            return id;
        }

        // Estado de la última verificación del usuario; null si nunca solicitó.
        public string UltimoEstadoVerificacion(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_USUARIO", idUsuario) };
            object o = acc.Escalar("VERIFICACION_ULTIMO_ESTADO", p);
            acc.Cerrar();
            return o == null || o == DBNull.Value ? null : o.ToString();
        }

        public DataTable ListarSolicitudesVerificacion()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("VERIFICACIONES_PENDIENTES_LISTAR");
            acc.Cerrar();
            return dt;
        }

        // ---------------- productos (CU08) ----------------

        public int AltaProducto(BE_Producto producto)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_VENDEDOR", producto.IDVendedor),
                acc.CrearParametroObj("@ID_TARIFA", producto.IDTarifa),
                acc.CrearParametro("@NOMBRE", producto.Nombre),
                acc.CrearParametroObj("@DESCRIPCION", producto.Descripcion),
                acc.CrearParametro("@CATEGORIA", producto.Categoria),
                acc.CrearParametro("@PRECIO", producto.Precio),
                acc.CrearParametro("@STOCK", producto.Stock)
            };
            int id = Convert.ToInt32(acc.Escalar("PRODUCTO_ALTA", p));
            acc.Cerrar();
            return id;
        }

        // Precio y stock de un producto propio.
        public void ModificarProducto(int idProducto, int idVendedor, decimal precio, int stock)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_PRODUCTO", idProducto),
                acc.CrearParametro("@ID_VENDEDOR", idVendedor),
                acc.CrearParametro("@PRECIO", precio),
                acc.CrearParametro("@STOCK", stock)
            };
            acc.Escribir("PRODUCTO_MODIFICAR", p);
            acc.Cerrar();
        }

        public DataTable ListarProductosPendientes()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("PRODUCTOS_PENDIENTES_LISTAR");
            acc.Cerrar();
            return dt;
        }

        public void AprobarProducto(int idProducto)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_PRODUCTO", idProducto) };
            acc.Escribir("PRODUCTO_APROBAR", p);
            acc.Cerrar();
        }

        public DataTable ListarCatalogo()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("CATALOGO_LISTAR");
            acc.Cerrar();
            return dt;
        }

        public DataTable ListarProductosDeVendedor(int idVendedor)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_VENDEDOR", idVendedor) };
            DataTable dt = acc.Leer("PRODUCTOS_VENDEDOR_LISTAR", p);
            acc.Cerrar();
            return dt;
        }

        public BE_Producto ObtenerProducto(int idProducto)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_PRODUCTO", idProducto) };
            DataTable dt = acc.Leer("PRODUCTO_OBTENER", p);
            acc.Cerrar();
            if (dt.Rows.Count == 0) return null;
            DataRow r = dt.Rows[0];
            return new BE_Producto
            {
                ID = Convert.ToInt32(r["ID_PRODUCTO"]),
                IDVendedor = Convert.ToInt32(r["ID_VENDEDOR"]),
                IDTarifa = r["ID_TARIFA"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["ID_TARIFA"]),
                Nombre = r["NOMBRE"].ToString(),
                Descripcion = r["DESCRIPCION"].ToString(),
                Categoria = r["CATEGORIA"].ToString(),
                Precio = Convert.ToDecimal(r["PRECIO"]),
                Stock = Convert.ToInt32(r["STOCK"]),
                Estado = r["ESTADO"].ToString()
            };
        }

        // ---------------- compra, pago y registro de la venta (CU09) ----------------

        // Registra la orden ya cobrada. Devuelve el ID de la orden.
        public int RegistrarVentaAprobada(BE_ItemOrden item, int idComprador, int idVendedor, string direccion,
                                          string referenciaPasarela, decimal porcentajeComision)
        {
            decimal monto = item.Cantidad * item.PrecioUnitario;
            decimal montoComision = Math.Round(monto * porcentajeComision / 100m, 2);
            decimal neto = monto - montoComision;

            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_PRODUCTO", item.Producto.ID),
                acc.CrearParametro("@CANTIDAD", item.Cantidad),
                acc.CrearParametro("@PRECIO", item.PrecioUnitario),
                acc.CrearParametro("@MONTO", monto),
                acc.CrearParametro("@ID_COMPRADOR", idComprador),
                acc.CrearParametro("@ID_VENDEDOR", idVendedor),
                acc.CrearParametroObj("@DIRECCION", direccion),
                acc.CrearParametroObj("@REFERENCIA", referenciaPasarela),
                acc.CrearParametro("@PORCENTAJE", porcentajeComision),
                acc.CrearParametro("@COMISION", montoComision),
                acc.CrearParametro("@NETO", neto)
            };
            int id = Convert.ToInt32(acc.Escalar("VENTA_REGISTRAR", p));
            acc.Cerrar();
            return id;
        }

        public DataTable ListarOrdenesComprador(int idComprador)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_COMPRADOR", idComprador) };
            DataTable dt = acc.Leer("ORDENES_COMPRADOR_LISTAR", p);
            acc.Cerrar();
            return dt;
        }

        public DataTable ListarOrdenesVendedor(int idVendedor)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_VENDEDOR", idVendedor) };
            DataTable dt = acc.Leer("ORDENES_VENDEDOR_LISTAR", p);
            acc.Cerrar();
            return dt;
        }

        // ---------------- despacho, recepción y liquidación (CU10) ----------------

        public void MarcarDespachada(int idOrden, int idVendedor)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_ORDEN", idOrden),
                acc.CrearParametro("@ID_VENDEDOR", idVendedor)
            };
            acc.Escribir("ORDEN_DESPACHAR", p);
            acc.Cerrar();
        }

        public void ConfirmarRecepcion(int idOrden, int idComprador)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_ORDEN", idOrden),
                acc.CrearParametro("@ID_COMPRADOR", idComprador)
            };
            acc.Escribir("ORDEN_CONFIRMAR_RECEPCION", p);
            acc.Cerrar();
        }

        public DataTable ListarLiquidaciones(string estado)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ESTADO", estado) };
            DataTable dt = acc.Leer("LIQUIDACIONES_LISTAR", p);
            acc.Cerrar();
            return dt;
        }

        public void PagarLiquidacion(int idLiquidacion)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_LIQUIDACION", idLiquidacion) };
            acc.Escribir("LIQUIDACION_PAGAR", p);
            acc.Cerrar();
        }

        // ---------------- reclamos (CU11) ----------------

        public int RegistrarReclamo(int idOrden, int idUsuario, string motivo)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_ORDEN", idOrden),
                acc.CrearParametro("@ID_USUARIO", idUsuario),
                acc.CrearParametro("@MOTIVO", motivo)
            };
            int id = Convert.ToInt32(acc.Escalar("RECLAMO_REGISTRAR", p));
            acc.Cerrar();
            return id;
        }

        public DataTable ListarReclamosAbiertos()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("RECLAMOS_ABIERTOS_LISTAR");
            acc.Cerrar();
            return dt;
        }

        public void ResolverReclamo(int idReclamo, bool procedente)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_RECLAMO", idReclamo),
                acc.CrearParametro("@PROCEDENTE", procedente)
            };
            acc.Escribir("RECLAMO_RESOLVER", p);
            acc.Cerrar();
        }
    }
}
