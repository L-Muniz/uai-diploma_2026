using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class BE_PermisoSimple : I_ComponenteAcceso
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Codigo { get; set; }

        public BE_PermisoSimple() { }

        public BE_PermisoSimple(string codigo, string nombre, string descripcion = "")
        {
            Codigo = codigo;
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public HashSet<BE_PermisoSimple> ObtenerTodosLosPermisos()
        {
            return new HashSet<BE_PermisoSimple> { this };
        }

        public bool TienePermiso(BE_PermisoSimple permiso)
        {
            return this.ID == permiso.ID;
        }

        public bool TienePermisoPorCodigo(string codigo)
        {
            return this.Codigo == codigo;
        }

        public override string ToString()
        {
            return Nombre;
        }

        public override bool Equals(object obj)
        {
            if (obj is BE_PermisoSimple permiso)
                return this.ID == permiso.ID;
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
