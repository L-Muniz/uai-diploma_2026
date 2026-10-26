using System;
using System.Collections.Generic;

namespace BE
{
    public class BE_Material
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string UnidadMedida { get; set; } = "kg";

        public override string ToString()
        {
            return Nombre;
        }
    }

    public class BE_PuntoVerde
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string Horario { get; set; }
        public bool Activo { get; set; } = true;
        public List<BE_Material> MaterialesAceptados { get; set; } = new List<BE_Material>();

        public override string ToString()
        {
            return Nombre;
        }
    }

    public class BE_GuiaReutilizacion
    {
        public int ID { get; set; }
        public BE_Material Material { get; set; }
        public string Titulo { get; set; }
        public string Pasos { get; set; }
        public string Dificultad { get; set; } = "Baja";
    }

    public class BE_DeclaracionMaterial
    {
        public int ID { get; set; }
        public int IDUsuario { get; set; }
        public BE_Material Material { get; set; }
        public decimal Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
    }

    public class BE_Recomendacion
    {
        public int ID { get; set; }
        public BE_DeclaracionMaterial Declaracion { get; set; }
        public BE_GuiaReutilizacion Guia { get; set; }
        public BE_PuntoVerde PuntoVerde { get; set; }
        public string TipoOpcionElegida { get; set; }
        public DateTime FechaGeneracion { get; set; }
    }

    public class BE_Acopio
    {
        public int ID { get; set; }
        public int? IDDeclaracion { get; set; }
        public BE_PuntoVerde PuntoVerde { get; set; }
        public BE_Material Material { get; set; }
        public decimal PesoKg { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public string Estado { get; set; }
    }
}
