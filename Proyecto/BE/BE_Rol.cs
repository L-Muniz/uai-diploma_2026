using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class BE_Rol
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private List<I_ComponenteAcceso> componentes;
        public List<I_ComponenteAcceso> Componentes
        {
            get { return componentes; }
            set { componentes = value; }
        }

        public BE_Rol()
        {
            componentes = new List<I_ComponenteAcceso>();
        }

        public BE_Rol(string nombre, string descripcion = "")
            : this()
        {
            this.Nombre = nombre;
            this.Descripcion = descripcion;
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

        public int ContarPermisosTotal()
        {
            return ObtenerTodosLosPermisos().Count;
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
