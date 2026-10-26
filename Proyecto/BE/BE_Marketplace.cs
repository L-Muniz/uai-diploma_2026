using System;
using System.Collections.Generic;

namespace BE
{
    public class BE_VerificacionVendedor
    {
        public int ID { get; set; }
        public int IDUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public string DocumentoRef { get; set; }
    }

    public class BE_Producto
    {
        public int ID { get; set; }
        public int IDVendedor { get; set; }
        public int? IDTarifa { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Estado { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }

    public class BE_ItemOrden
    {
        public BE_Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

    public class BE_OrdenVenta
    {
        public int ID { get; set; }
        public int IDComprador { get; set; }
        public int IDVendedor { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public decimal MontoBruto { get; set; }
        public string DireccionEnvio { get; set; }
        public List<BE_ItemOrden> Items { get; set; } = new List<BE_ItemOrden>();
    }

    public class BE_Pago
    {
        public int ID { get; set; }
        public int IDOrden { get; set; }
        public string ReferenciaPasarela { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class BE_Comision
    {
        public int ID { get; set; }
        public int IDOrden { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class BE_LiquidacionVendedor
    {
        public int ID { get; set; }
        public int IDOrden { get; set; }
        public decimal MontoNeto { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaDisponible { get; set; }
        public DateTime? FechaPago { get; set; }
    }

    public class BE_Reclamo
    {
        public int ID { get; set; }
        public int IDOrden { get; set; }
        public int IDUsuario { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
    }
}
