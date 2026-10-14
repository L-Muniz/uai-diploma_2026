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

        private int? idPadreRol;
        public int? IdPadreRol
        {
            get { return idPadreRol; }
            set { idPadreRol = value; }
        }

        private BE_Rol rolPadre;
        public BE_Rol RolPadre
        {
            get { return rolPadre; }
            set { rolPadre = value; }
        }

        private List<I_ComponenteAcceso> componentes;
        public List<I_ComponenteAcceso> Componentes
        {
            get { return componentes; }
            set { componentes = value; }
        }

        private List<BE_Rol> subRoles;
        public List<BE_Rol> SubRoles
        {
            get { return subRoles; }
            set { subRoles = value; }
        }

        public BE_Rol()
        {
            componentes = new List<I_ComponenteAcceso>();
            subRoles = new List<BE_Rol>();
        }

        public BE_Rol(string nombre, string descripcion = "", int? idPadreRol = null)
            : this()
        {
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.IdPadreRol = idPadreRol;
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

        public void AgregarSubRol(BE_Rol subRol)
        {
            if (subRol != null && subRol.ID != this.ID && !SubRoles.Contains(subRol))
            {
                SubRoles.Add(subRol);
                subRol.RolPadre = this;
                subRol.IdPadreRol = this.ID;
            }
        }

        public void QuitarSubRol(BE_Rol subRol)
        {
            if (subRol == null)
                return;

            if (SubRoles.Remove(subRol))
            {
                if (subRol.RolPadre == this)
                    subRol.RolPadre = null;
                if (subRol.IdPadreRol == this.ID)
                    subRol.IdPadreRol = null;
            }
        }

        public HashSet<BE_PermisoSimple> ObtenerPermisosHeredados()
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

            if (RolPadre != null)
            {
                var permisosHeredados = RolPadre.ObtenerPermisosHeredados();
                foreach (var permiso in permisosHeredados)
                {
                    permisosTotal.Add(permiso);
                }
            }

            return permisosTotal;
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

            if (RolPadre != null)
            {
                var permisosHeredados = RolPadre.ObtenerPermisosHeredados();
                foreach (var permiso in permisosHeredados)
                {
                    permisosTotal.Add(permiso);
                }
            }

            foreach (var subRol in SubRoles)
            {
                var permisosSubRol = subRol.ObtenerTodosLosPermisos();
                foreach (var permiso in permisosSubRol)
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

        public int ObtenerProfundidad()
        {
            if (RolPadre == null)
                return 0;
            return RolPadre.ObtenerProfundidad() + 1;
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
