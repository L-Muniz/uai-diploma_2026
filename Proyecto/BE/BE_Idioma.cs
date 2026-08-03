using System;

namespace BE
{
    public class BE_Idioma
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }

        public override string ToString()
        {
            return Nombre ?? Codigo ?? "";
        }
    }
}
