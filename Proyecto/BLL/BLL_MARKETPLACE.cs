using System;
using System.Data;
using BE;
using DAL;

namespace BLL
{
    // Resultado de un cobro en la pasarela de pagos externa.
    public class ResultadoCobro
    {
        public bool Aprobado { get; set; }
        public string Referencia { get; set; }
        public string Motivo { get; set; }
    }

    // La pasarela de pagos es un proveedor externo (fuera de alcance). Se abstrae para poder reemplazarla.
    public interface IPasarelaPago
    {
        ResultadoCobro Cobrar(decimal monto, string referencia);
    }

    // Implementación de prueba: aprueba montos hasta 100.000 y rechaza el resto. No mueve dinero real.
    public class PasarelaSimulada : IPasarelaPago
    {
        public ResultadoCobro Cobrar(decimal monto, string referencia)
        {
            if (monto <= 100000m)
                return new ResultadoCobro { Aprobado = true, Referencia = "SIM-" + Guid.NewGuid().ToString("N").Substring(0, 12), Motivo = "" };
            return new ResultadoCobro { Aprobado = false, Referencia = referencia, Motivo = "Fondos insuficientes (simulado)." };
        }
    }

    // N02: verificación de vendedores (CU07), productos (CU08), compra y pago (CU09),
    // despacho, recepción y liquidación (CU10) y reclamos (CU11).
    public class BLL_MARKETPLACE
    {
        private readonly MP_MARKETPLACE dal = new MP_MARKETPLACE();
        private readonly MP_GESTION dalGestion = new MP_GESTION();
        private readonly BLL_BITACORA bllBitacora = new BLL_BITACORA();

        // ---------------- CU07 ----------------

        public void SolicitarVerificacion(BE_USUARIO usuario, string documentoRef)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_SOLICITAR_VERIFICACION");
            if (usuario == null) throw new ArgumentException("Debe iniciar sesión.");
            new BLL_DATOS_USUARIO().ValidarMayorDeEdadVendedor(usuario);
            if (string.IsNullOrWhiteSpace(documentoRef))
                throw new ArgumentException("Ingrese la referencia de la documentación.");

            dal.AltaVerificacion(usuario.ID, "PENDIENTE", documentoRef);
            bllBitacora.IngresarRegistro(usuario, "Solicitó verificación para vender en la tienda.");
        }

        public DataTable ListarSolicitudesVerificacion()
        {
            return dal.ListarSolicitudesVerificacion();
        }

        // El área comercial aprueba o rechaza la solicitud (genera un nuevo estado en el historial).
        public void ResolverVerificacion(BE_USUARIO responsable, int idUsuarioSolicitante, bool aprobada)
        {
            BLL_AUTORIZACION.Exigir(responsable, "PROC_RESOLVER_VERIFICACION");
            dal.AltaVerificacion(idUsuarioSolicitante, aprobada ? "APROBADA" : "RECHAZADA", null);
            bllBitacora.IngresarRegistro(responsable, $"Verificación de vendedor {idUsuarioSolicitante}: {(aprobada ? "aprobada" : "rechazada")}.");
        }

        public string ObtenerEstadoVerificacion(int idUsuario)
        {
            return dal.UltimoEstadoVerificacion(idUsuario);
        }

        public bool EsVendedorVerificado(int idUsuario)
        {
            return dal.UltimoEstadoVerificacion(idUsuario) == "APROBADA";
        }

        // ---------------- CU08 ----------------

        public int PublicarProducto(BE_USUARIO vendedor, string nombre, string descripcion, string categoria,
                                    decimal precio, int stock)
        {
            BLL_AUTORIZACION.Exigir(vendedor, "PROC_PUBLICAR_PRODUCTO");
            if (vendedor == null) throw new ArgumentException("Debe iniciar sesión.");
            if (!EsVendedorVerificado(vendedor.ID))
                throw new InvalidOperationException("Debe estar verificado como vendedor para publicar (CU07).");
            if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Ingrese el nombre del producto.");
            if (string.IsNullOrWhiteSpace(categoria)) throw new ArgumentException("Ingrese la categoría.");
            if (precio <= 0) throw new ArgumentException("El precio debe ser mayor a cero.");
            if (stock < 0) throw new ArgumentException("El stock no puede ser negativo.");

            int? idTarifa = dalGestion.ObtenerIdTarifaVigente(categoria) ?? dalGestion.ObtenerIdTarifaVigente("General");
            int id = dal.AltaProducto(new BE_Producto
            {
                IDVendedor = vendedor.ID,
                IDTarifa = idTarifa,
                Nombre = nombre,
                Descripcion = descripcion,
                Categoria = categoria,
                Precio = precio,
                Stock = stock
            });
            bllBitacora.IngresarRegistro(vendedor, $"Publicó el producto {id} ({nombre}) pendiente de aprobación.");
            return id;
        }

        // CU08 (administración del producto): el vendedor cambia precio y stock de sus publicaciones.
        public void ModificarProducto(BE_USUARIO vendedor, int idProducto, decimal precio, int stock)
        {
            BLL_AUTORIZACION.Exigir(vendedor, "PROC_PUBLICAR_PRODUCTO");
            if (precio <= 0) throw new ArgumentException("El precio debe ser mayor a cero.");
            if (stock < 0) throw new ArgumentException("El stock no puede ser negativo.");
            dal.ModificarProducto(idProducto, vendedor.ID, precio, stock);
            bllBitacora.IngresarRegistro(vendedor, $"Modificó precio y stock del producto {idProducto}.");
        }

        // Comisión estimada para mostrar al vendedor (la tarifa de la categoria o la general).
        public decimal ObtenerComisionEstimada(string categoria)
        {
            return dalGestion.ObtenerPorcentajeVigente(categoria) ?? dalGestion.ObtenerPorcentajeVigente("General") ?? 0m;
        }

        public DataTable ListarProductosPendientes()
        {
            return dal.ListarProductosPendientes();
        }

        public void AprobarProducto(BE_USUARIO responsable, int idProducto)
        {
            BLL_AUTORIZACION.Exigir(responsable, "PROC_APROBAR_PRODUCTO");
            dal.AprobarProducto(idProducto);
            bllBitacora.IngresarRegistro(responsable, $"Aprobó el producto {idProducto} para la venta.");
        }

        public DataTable ListarCatalogo()
        {
            return dal.ListarCatalogo();
        }

        public DataTable ListarProductosDeVendedor(int idVendedor)
        {
            return dal.ListarProductosDeVendedor(idVendedor);
        }

        // ---------------- CU09 ----------------

        // Cobra con la pasarela y, si aprueba, registra la orden, el pago, la comisión y la liquidación retenida.
        public int Comprar(BE_USUARIO comprador, int idProducto, int cantidad, string direccion, IPasarelaPago pasarela)
        {
            BLL_AUTORIZACION.Exigir(comprador, "PROC_COMPRAR");
            if (comprador == null) throw new ArgumentException("Debe iniciar sesión.");
            if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(direccion)) throw new ArgumentException("Ingrese el domicilio de entrega.");

            BE_Producto producto = dal.ObtenerProducto(idProducto);
            if (producto == null || producto.Estado != "ACTIVO")
                throw new InvalidOperationException("El producto no está disponible.");
            if (producto.IDVendedor == comprador.ID)
                throw new InvalidOperationException("No puede comprar sus propios productos.");
            if (producto.Stock < cantidad)
                throw new InvalidOperationException("Stock insuficiente.");

            decimal monto = producto.Precio * cantidad;
            ResultadoCobro cobro = pasarela.Cobrar(monto, "PED-" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            if (!cobro.Aprobado)
            {
                bllBitacora.IngresarRegistro(comprador, $"Pago rechazado por la pasarela para el producto {idProducto}: {cobro.Motivo}");
                throw new InvalidOperationException("El pago fue rechazado: " + cobro.Motivo);
            }

            var item = new BE_ItemOrden { Producto = producto, Cantidad = cantidad, PrecioUnitario = producto.Precio };
            int idOrden = dal.RegistrarVentaAprobada(item, comprador.ID, producto.IDVendedor, direccion,
                                                     cobro.Referencia, ObtenerComisionEstimada(producto.Categoria));
            bllBitacora.IngresarRegistro(comprador, $"Compró {cantidad} x {producto.Nombre} (orden {idOrden}).");
            return idOrden;
        }

        // ---------------- CU10 ----------------

        public DataTable ListarOrdenesComprador(int idComprador)
        {
            return dal.ListarOrdenesComprador(idComprador);
        }

        public DataTable ListarOrdenesVendedor(int idVendedor)
        {
            return dal.ListarOrdenesVendedor(idVendedor);
        }

        public void MarcarDespachada(BE_USUARIO vendedor, int idOrden)
        {
            BLL_AUTORIZACION.Exigir(vendedor, "PROC_DESPACHAR");
            dal.MarcarDespachada(idOrden, vendedor.ID);
            bllBitacora.IngresarRegistro(vendedor, $"Despachó la orden {idOrden}.");
        }

        public void ConfirmarRecepcion(BE_USUARIO comprador, int idOrden)
        {
            BLL_AUTORIZACION.Exigir(comprador, "PROC_CONFIRMAR_RECEPCION");
            dal.ConfirmarRecepcion(idOrden, comprador.ID);
            bllBitacora.IngresarRegistro(comprador, $"Confirmó la recepción conforme de la orden {idOrden}.");
        }

        public DataTable ListarLiquidaciones(string estado)
        {
            return dal.ListarLiquidaciones(estado);
        }

        public void PagarLiquidacion(BE_USUARIO responsable, int idLiquidacion)
        {
            BLL_AUTORIZACION.Exigir(responsable, "PROC_LIQUIDAR");
            dal.PagarLiquidacion(idLiquidacion);
            bllBitacora.IngresarRegistro(responsable, $"Ordenó la transferencia de la liquidación {idLiquidacion}.");
        }

        // ---------------- CU11 ----------------

        public int PresentarReclamo(BE_USUARIO comprador, int idOrden, string motivo)
        {
            BLL_AUTORIZACION.Exigir(comprador, "PROC_RECLAMAR");
            if (comprador == null) throw new ArgumentException("Debe iniciar sesión.");
            if (string.IsNullOrWhiteSpace(motivo)) throw new ArgumentException("Describa el motivo del reclamo.");

            int idReclamo = dal.RegistrarReclamo(idOrden, comprador.ID, motivo);
            bllBitacora.IngresarRegistro(comprador, $"Presentó el reclamo {idReclamo} sobre la orden {idOrden}.");
            return idReclamo;
        }

        public DataTable ListarReclamosAbiertos()
        {
            return dal.ListarReclamosAbiertos();
        }

        public void ResolverReclamo(BE_USUARIO responsable, int idReclamo, bool procedente)
        {
            BLL_AUTORIZACION.Exigir(responsable, "PROC_RESOLVER_RECLAMO");
            dal.ResolverReclamo(idReclamo, procedente);
            bllBitacora.IngresarRegistro(responsable, $"Resolvió el reclamo {idReclamo}: {(procedente ? "procedente" : "improcedente")}.");
        }
    }
}
