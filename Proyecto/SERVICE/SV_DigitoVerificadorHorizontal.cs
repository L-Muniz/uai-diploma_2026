using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE
{
    /// <summary>
    /// Clase genérica para calcular Dígitos Verificadores Horizontales (DVH)
    /// Implementa algoritmo modular configurable para detectar:
    /// 1. Datos agregados o quitados de BD
    /// 2. Intercambio de posiciones de caracteres/atributos
    /// </summary>
    public class SV_DigitoVerificadorHorizontal
    {
        private const int MODULO = 11;

        /// <summary>
        /// Calcula el DV horizontal para una entidad
        /// Considera: valor del dato + posición del dato + posición del atributo
        /// </summary>
        public static int CalcularDV(string datos)
        {
            if (string.IsNullOrEmpty(datos))
                return 0;

            int suma = 0;
            int posicionCaracter = 1;

            foreach (char c in datos)
            {
                // Convertir carácter a valor ASCII
                int valorCaracter = (int)c;

                // Multiplicar por posición del carácter
                int aporte = (valorCaracter * posicionCaracter) % MODULO;

                suma += aporte;
                posicionCaracter++;
            }

            // Calcular DV: (MODULO - (suma % MODULO)) % MODULO
            int dv = (MODULO - (suma % MODULO)) % MODULO;
            return dv;
        }

        /// <summary>
        /// Verifica la integridad del DV horizontal
        /// </summary>
        public static bool VerificarDV(string datos, int dvAlmacenado)
        {
            int dvCalculado = CalcularDV(datos);
            return dvCalculado == dvAlmacenado;
        }

        /// <summary>
        /// Genera representación en string del DV para BD
        /// </summary>
        public static string ObtenerDVString(string datos)
        {
            return CalcularDV(datos).ToString();
        }
    }
}
