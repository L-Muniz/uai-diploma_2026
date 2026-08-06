using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE
{
    
    public interface ISV_IdiomaObserver
    {

        void ActualizarIdioma(string codigoIdiomaAnterior, string codigoIdiomaNuevo);
    }

    
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
