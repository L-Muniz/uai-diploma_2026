using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    
    public class BE_CambiosEntidad
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string tipoEntidad;
        public string TipoEntidad
        {
            get { return tipoEntidad; }
            set { tipoEntidad = value; }
        }

        private int idEntidad;
        public int IdEntidad
        {
            get { return idEntidad; }
            set { idEntidad = value; }
        }

        private string nombreAtributo;
        public string NombreAtributo
        {
            get { return nombreAtributo; }
            set { nombreAtributo = value; }
        }

        private string valorAnterior;
        public string ValorAnterior
        {
            get { return valorAnterior; }
            set { valorAnterior = value; }
        }

        private string valorNuevo;
        public string ValorNuevo
        {
            get { return valorNuevo; }
            set { valorNuevo = value; }
        }

        /// <summary>Valor de ValorAnterior apto para mostrar: los mementos (con hashes) se ocultan.</summary>
        public string ValorAnteriorMostrado => EsMemento ? "[estado guardado]" : valorAnterior;

        /// <summary>Valor de ValorNuevo apto para mostrar: los mementos (con hashes) se ocultan.</summary>
        public string ValorNuevoMostrado => EsMemento ? "[estado guardado]" : valorNuevo;

        private bool EsMemento => string.Equals(nombreAtributo, "MEMENTO", StringComparison.OrdinalIgnoreCase);

        private BE_USUARIO usuarioModifico;
        public BE_USUARIO UsuarioModifico
        {
            get { return usuarioModifico; }
            set { usuarioModifico = value; }
        }

        private DateTime fechaCambio;
        public DateTime FechaCambio
        {
            get { return fechaCambio; }
            set { fechaCambio = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public BE_CambiosEntidad() { }

        public BE_CambiosEntidad(string tipoEntidad, int idEntidad, string nombreAtributo,
            string valorAnterior, string valorNuevo, BE_USUARIO usuarioModifico, string descripcion)
        {
            this.TipoEntidad = tipoEntidad;
            this.IdEntidad = idEntidad;
            this.NombreAtributo = nombreAtributo;
            this.ValorAnterior = valorAnterior;
            this.ValorNuevo = valorNuevo;
            this.UsuarioModifico = usuarioModifico;
            this.FechaCambio = DateTime.Now;
            this.Descripcion = descripcion;
        }

        public override string ToString()
        {
            return $"[{FechaCambio:dd/MM/yyyy HH:mm:ss}] {TipoEntidad}(ID:{IdEntidad}) - {NombreAtributo}: '{ValorAnterior}' → '{ValorNuevo}'";
        }
    }
}
