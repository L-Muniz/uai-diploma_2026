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

        /// <summary>Valor de ValorAnterior legible para mostrar en auditoría.</summary>
        public string ValorAnteriorMostrado => FormatearValor(valorAnterior);

        /// <summary>Valor de ValorNuevo legible para mostrar en auditoría.</summary>
        public string ValorNuevoMostrado => FormatearValor(valorNuevo);

        private bool EsMemento => string.Equals(nombreAtributo, "MEMENTO", StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Atributo legible: para los mementos lista los campos que cambiaron (ej. "NOMBRE, DESCRIPCION").
        /// Para el resto de los registros devuelve el atributo tal cual.
        /// </summary>
        public string AtributoMostrado => EsMemento ? DescribirCamposCambiados() : nombreAtributo;

        private string DescribirCamposCambiados()
        {
            try
            {
                var campos = new List<string>();

                if (string.Equals(tipoEntidad, "ROL", StringComparison.OrdinalIgnoreCase))
                {
                    var antes = BE_MementoRol.FromSerializedString(valorAnterior);
                    var despues = BE_MementoRol.FromSerializedString(valorNuevo);
                    if (antes != null && despues != null)
                    {
                        if (antes.Nombre != despues.Nombre) campos.Add("NOMBRE");
                        if (antes.Descripcion != despues.Descripcion) campos.Add("DESCRIPCION");
                        if (!antes.IdsPermisos.OrderBy(i => i).SequenceEqual(despues.IdsPermisos.OrderBy(i => i))) campos.Add("PERMISOS");
                    }
                }
                else if (string.Equals(tipoEntidad, "USUARIO", StringComparison.OrdinalIgnoreCase))
                {
                    var antes = BE_MementoUsuario.FromSerializedString(valorAnterior);
                    var despues = BE_MementoUsuario.FromSerializedString(valorNuevo);
                    if (antes != null && despues != null)
                    {
                        if (antes.Username != despues.Username) campos.Add("USERNAME");
                        if (antes.HashedPassword != despues.HashedPassword) campos.Add("PASS");
                    }
                }

                return campos.Count > 0 ? string.Join(", ", campos) : "ESTADO";
            }
            catch (FormatException)
            {
                return "ESTADO";
            }
        }

        /// <summary>
        /// Convierte un memento en texto legible. El hash de contraseña de usuario nunca se muestra.
        /// Los valores que no son memento se devuelven tal cual.
        /// </summary>
        private string FormatearValor(string valor)
        {
            if (!EsMemento || string.IsNullOrEmpty(valor))
                return valor;

            try
            {
                if (string.Equals(tipoEntidad, "ROL", StringComparison.OrdinalIgnoreCase))
                {
                    var rol = BE_MementoRol.FromSerializedString(valor);
                    if (rol == null) return "[estado guardado]";
                    return $"{rol.Nombre} | {rol.Descripcion} | {rol.IdsPermisos.Count} permiso(s)";
                }

                if (string.Equals(tipoEntidad, "USUARIO", StringComparison.OrdinalIgnoreCase))
                {
                    var usuario = BE_MementoUsuario.FromSerializedString(valor);
                    if (usuario == null) return "[estado guardado]";
                    return $"{usuario.Username} | (contraseña oculta)";
                }
            }
            catch (FormatException)
            {
                // Registro con formato inesperado: se muestra genérico en lugar de romper la grilla.
            }

            return "[estado guardado]";
        }

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
