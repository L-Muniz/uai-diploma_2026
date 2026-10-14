using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class BE_PermisoCompuesto : I_ComponenteAcceso
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Codigo { get; set; }

        private List<I_ComponenteAcceso> componentes;
        public List<I_ComponenteAcceso> Componentes
        {
            get { return componentes; }
            set { componentes = value; }
        }

        public BE_PermisoCompuesto()
        {
            componentes = new List<I_ComponenteAcceso>();
        }

        public BE_PermisoCompuesto(string codigo, string nombre, string descripcion = "")
            : this()
        {
            Codigo = codigo;
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public void AgregarComponente(I_ComponenteAcceso componente)
        {
            if (componente != null && !Componentes.Contains(componente))
            {
                Componentes.Add(componente);
            }
        }

        public void QuitarComponente(I_ComponenteAcceso componente)
        {
            if (componente != null)
            {
                Componentes.Remove(componente);
            }
        }

        public HashSet<BE_PermisoSimple> ObtenerTodosLosPermisos()
        {
            HashSet<BE_PermisoSimple> permisosTotal = new HashSet<BE_PermisoSimple>();

            foreach (var componente in Componentes)
            {
                var permisos = componente.ObtenerTodosLosPermisos();
                foreach (var permiso in permisos)
                {
                    permisosTotal.Add(permiso);
                }
            }

            return permisosTotal;
        }

        public bool TienePermiso(BE_PermisoSimple permiso)
        {
            return ObtenerTodosLosPermisos().Contains(permiso);
        }

        public bool TienePermisoPorCodigo(string codigo)
        {
            return ObtenerTodosLosPermisos().Any(p => p.Codigo == codigo);
        }

        public override string ToString()
        {
            return Nombre;
        }

        public override bool Equals(object obj)
        {
            if (obj is BE_PermisoCompuesto permiso)
                return this.ID == permiso.ID;
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}
