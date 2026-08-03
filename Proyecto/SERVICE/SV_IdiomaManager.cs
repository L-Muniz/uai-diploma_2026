using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE
{
    public interface ISV_ProveedorTraduccion
    {
        string ObtenerTraduccion(string clave, string codigoIdioma);
        List<string> ObtenerIdiomasDisponibles();
        /// <summary>Código → nombre nativo del idioma (desde BD).</summary>
        Dictionary<string, string> ObtenerNombresIdiomas();
    }

    public class SV_IdiomaManager
    {
        private static SV_IdiomaManager instancia;
        private string codigoIdiomaActual = "ES";
        private List<ISV_IdiomaObserver> observadores = new List<ISV_IdiomaObserver>();
        private ISV_ProveedorTraduccion proveedorTraduccion;

        private SV_IdiomaManager()
        {
        }

        public static SV_IdiomaManager GetInstance()
        {
            if (instancia == null)
            {
                instancia = new SV_IdiomaManager();
            }
            return instancia;
        }

        public void EstablecerProveedorTraduccion(ISV_ProveedorTraduccion proveedor)
        {
            proveedorTraduccion = proveedor;
        }

        public string ObtenerIdiomaActual()
        {
            return codigoIdiomaActual;
        }

        public List<string> ObtenerIdiomasSoportados()
        {
            if (proveedorTraduccion != null)
                return proveedorTraduccion.ObtenerIdiomasDisponibles();
            return new List<string>();
        }

        public Dictionary<string, string> ObtenerNombresIdiomas()
        {
            if (proveedorTraduccion != null)
                return proveedorTraduccion.ObtenerNombresIdiomas();
            return new Dictionary<string, string>();
        }

        public void CambiarIdioma(string codigoIdioma)
        {
            codigoIdioma = codigoIdioma.ToUpper();

            if (proveedorTraduccion != null)
            {
                var idiomas = proveedorTraduccion.ObtenerIdiomasDisponibles();
                if (!idiomas.Any(i => string.Equals(i, codigoIdioma, StringComparison.OrdinalIgnoreCase)))
                    throw new Exception($"Idioma '{codigoIdioma}' no soportado");
            }

            if (codigoIdioma == codigoIdiomaActual)
                return;

            string idiomaAnterior = codigoIdiomaActual;
            codigoIdiomaActual = codigoIdioma;

            NotificarCambioIdioma(idiomaAnterior, codigoIdioma);
        }

        public string Obtener(string clave)
        {
            if (proveedorTraduccion != null)
            {
                return proveedorTraduccion.ObtenerTraduccion(clave, codigoIdiomaActual);
            }

            return $"[{clave}]";
        }

        public string Obtener(string clave, params object[] args)
        {
            string cadena = Obtener(clave);
            try
            {
                return string.Format(cadena, args);
            }
            catch
            {
                return cadena;
            }
        }

        public void RegistrarObservador(ISV_IdiomaObserver observador)
        {
            if (observador != null && !observadores.Contains(observador))
            {
                observadores.Add(observador);
            }
        }

        public void DesregistrarObservador(ISV_IdiomaObserver observador)
        {
            if (observador != null)
            {
                observadores.Remove(observador);
            }
        }

        private void NotificarCambioIdioma(string idiomaAnterior, string idiomaNuevo)
        {
            foreach (var observador in observadores)
            {
                try
                {
                    observador.ActualizarIdioma(idiomaAnterior, idiomaNuevo);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error notificando observador: {ex.Message}");
                }
            }
        }

        public int ContarObservadores()
        {
            return observadores.Count;
        }
    }
}
