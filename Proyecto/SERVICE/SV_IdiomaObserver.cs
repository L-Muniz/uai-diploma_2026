using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE
{
    /// <summary>
    /// Interfaz observadora para implementar el patrón Observer
    /// Los observadores se notifican cuando cambia el idioma del sistema
    /// </summary>
    public interface ISV_IdiomaObserver
    {
        /// <summary>
        /// Se ejecuta cuando cambia el idioma del sistema
        /// </summary>
        void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo);
    }

    /// <summary>
    /// Observador base que puede ser heredado por componentes que necesitan reaccionar a cambios de idioma
    /// </summary>
    public abstract class SV_IdiomaObserverBase : ISV_IdiomaObserver
    {
        protected string nombreObservador;

        public SV_IdiomaObserverBase(string nombre)
        {
            nombreObservador = nombre;
        }

        public abstract void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo);

        public virtual string ObtenerNombre()
        {
            return nombreObservador;
        }
    }
}
