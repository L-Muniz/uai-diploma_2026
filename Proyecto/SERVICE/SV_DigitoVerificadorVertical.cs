using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE
{
    /// <summary>
    /// Clase para gestionar Dígitos Verificadores Verticales (DVV)
    /// Los DVV se almacenan en una entidad adicional para verificar integridad a nivel de atributos
    /// Permite detectar cambios en columnas específicas de las entidades
    /// </summary>
    public class SV_DigitoVerificadorVertical
    {
        private const int MODULO = 11;

        /// <summary>
        /// Calcula DV vertical para un atributo
        /// Considera la posición del atributo en la entidad
        /// </summary>
        public static int CalcularDVAtributo(string valorAtributo, int posicionAtributo)
        {
            if (string.IsNullOrEmpty(valorAtributo))
                return 0;

            int suma = 0;

            for (int i = 0; i < valorAtributo.Length; i++)
            {
                int valorCaracter = (int)valorAtributo[i];
                // Multiplicar por posición del atributo + posición del carácter
                int aporte = (valorCaracter * (posicionAtributo + i + 1)) % MODULO;
                suma += aporte;
            }

            int dv = (MODULO - (suma % MODULO)) % MODULO;
            return dv;
        }

        /// <summary>
        /// Verifica integridad de un atributo
        /// </summary>
        public static bool VerificarDVAtributo(string valorAtributo, int posicionAtributo, int dvAlmacenado)
        {
            int dvCalculado = CalcularDVAtributo(valorAtributo, posicionAtributo);
            return dvCalculado == dvAlmacenado;
        }

        /// <summary>
        /// Calcula DVV para múltiples atributos de una entidad
        /// Devuelve diccionario: nombreAtributo -> DVV
        /// </summary>
        public static Dictionary<string, int> CalcularDVVEntidad(Dictionary<string, string> atributos)
        {
            Dictionary<string, int> dvvResultados = new Dictionary<string, int>();
            int posicion = 1;

            foreach (var atributo in atributos)
            {
                int dvv = CalcularDVAtributo(atributo.Value, posicion);
                dvvResultados[atributo.Key] = dvv;
                posicion++;
            }

            return dvvResultados;
        }

        /// <summary>
        /// Verifica todos los DVV de una entidad
        /// Devuelve: true si todos son válidos, false si alguno falló
        /// </summary>
        public static bool VerificarDVVEntidad(Dictionary<string, string> atributos, Dictionary<string, int> dvvAlmacenados)
        {
            int posicion = 1;

            foreach (var atributo in atributos)
            {
                if (!dvvAlmacenados.ContainsKey(atributo.Key))
                    return false;

                int dvvAlmacenado = dvvAlmacenados[atributo.Key];
                int dvvCalculado = CalcularDVAtributo(atributo.Value, posicion);

                if (dvvCalculado != dvvAlmacenado)
                    return false;

                posicion++;
            }

            return true;
        }
    }
}
