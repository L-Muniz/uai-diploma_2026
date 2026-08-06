using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE
{
    
    public class SV_DigitoVerificadorHorizontal
    {
        private const int MODULO = 11;

        public static int CalcularDV(string datos)
        {
            if (string.IsNullOrEmpty(datos))
                return 0;

            int suma = 0;
            int posicionCaracter = 1;

            foreach (char c in datos)
            {
                
                int valorCaracter = (int)c;

                int aporte = (valorCaracter * posicionCaracter) % MODULO;

                suma += aporte;
                posicionCaracter++;
            }

            int dv = (MODULO - (suma % MODULO)) % MODULO;
            return dv;
        }

        
        public static bool VerificarDV(string datos, int dvAlmacenado)
        {
            int dvCalculado = CalcularDV(datos);
            return dvCalculado == dvAlmacenado;
        }

        
        public static string ObtenerDVString(string datos)
        {
            return CalcularDV(datos).ToString();
        }
    }
}
