using System;

namespace BE
{
    public class BE_InformeGestion
    {
        public int ID { get; set; }
        public int IDUsuario { get; set; }
        public string Tipo { get; set; }
        public string Periodo { get; set; }
        public DateTime FechaGeneracion { get; set; }
    }

    public class BE_Decision
    {
        public int ID { get; set; }
        public int? IDInforme { get; set; }
        public int IDUsuario { get; set; }
        public int? IDPuntoVerde { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class BE_TarifaComision
    {
        public int ID { get; set; }
        public int? IDDecision { get; set; }
        public string Categoria { get; set; }
        public decimal Porcentaje { get; set; }
        public DateTime VigenteDesde { get; set; }
    }

    public class BE_TablaConversion
    {
        public int ID { get; set; }
        public int? IDDecision { get; set; }
        public BE_Material Material { get; set; }
        public decimal PuntosPorKg { get; set; }
        public DateTime VigenteDesde { get; set; }
    }
}
