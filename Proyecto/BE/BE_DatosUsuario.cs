using System;

namespace BE
{
    // Datos personales del usuario que el sistema necesita: edad (G02.1) y ubicación (recomendación).
    public class BE_DatosUsuario
    {
        public int IDUsuario { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string Localidad { get; set; }
    }
}
