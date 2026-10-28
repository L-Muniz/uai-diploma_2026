using System;
using System.Collections.Generic;

namespace BE
{
    // Informe de indicadores de gestión (CU04) en forma serializable. Se guarda y se carga como XML.
    public class BE_InformeIndicadores
    {
        public string Version { get; set; } = "1.0";
        public string Periodo { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string Generado { get; set; }
        public List<BE_IndicadorItem> Indicadores { get; set; } = new List<BE_IndicadorItem>();
    }

    public class BE_IndicadorItem
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
}
