using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE
{
    public class SV_DigitoVerificadorVertical
    {
        private const int MODULO = 11;

        public static int CalcularDVAtributo(string valorAtributo, int posicionAtributo)
        {
            if (string.IsNullOrEmpty(valorAtributo))
                return 0;

            int suma = 0;

            for (int i = 0; i < valorAtributo.Length; i++)
            {
                int valorCaracter = (int)valorAtributo[i];
                
                int aporte = (valorCaracter * (posicionAtributo + i + 1)) % MODULO;
                suma += aporte;
            }

            int dv = (MODULO - (suma % MODULO)) % MODULO;
            return dv;
        }

        
        public static bool VerificarDVAtributo(string valorAtributo, int posicionAtributo, int dvAlmacenado)
        {
            int dvCalculado = CalcularDVAtributo(valorAtributo, posicionAtributo);
            return dvCalculado == dvAlmacenado;
        }

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
