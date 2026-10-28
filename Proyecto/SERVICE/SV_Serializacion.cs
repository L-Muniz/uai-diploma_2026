using System;
using System.IO;
using System.Xml.Serialization;
using BE;

namespace SERVICE
{
    // Error específico cuando un archivo serializado no tiene el formato esperado.
    public class SV_ArchivoInvalidoException : Exception
    {
        public SV_ArchivoInvalidoException(string mensaje, Exception interna = null) : base(mensaje, interna) { }
    }

    // Serialización XML de informes (A02 serialización): guardar y recuperar informes de indicadores.
    public static class SV_Serializacion
    {
        public static void GuardarInforme(string ruta, BE_InformeIndicadores informe)
        {
            if (string.IsNullOrWhiteSpace(ruta)) throw new ArgumentException("Indique la ruta del archivo.");
            if (informe == null) throw new ArgumentNullException(nameof(informe));

            using (FileStream fs = new FileStream(ruta, FileMode.Create, FileAccess.Write))
            {
                new XmlSerializer(typeof(BE_InformeIndicadores)).Serialize(fs, informe);
            }
        }

        public static BE_InformeIndicadores CargarInforme(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta) || !File.Exists(ruta))
                throw new SV_ArchivoInvalidoException("El archivo indicado no existe.");

            BE_InformeIndicadores informe;
            try
            {
                using (FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))
                {
                    informe = (BE_InformeIndicadores)new XmlSerializer(typeof(BE_InformeIndicadores)).Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                throw new SV_ArchivoInvalidoException("El archivo no es un informe de indicadores válido.", ex);
            }

            if (informe == null || string.IsNullOrWhiteSpace(informe.Periodo) || informe.Indicadores == null)
                throw new SV_ArchivoInvalidoException("El archivo no contiene un informe de indicadores completo.");
            return informe;
        }
    }
}
