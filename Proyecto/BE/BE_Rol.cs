using System;
using System.Collections.Generic;
using System.Linq;

namespace BE
{
    /// <summary>
    /// Entidad que implementa patrón Composite para jerarquía de roles
    /// Un rol puede contener permisos y otros roles (sub-roles)
    /// </summary>
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

        private List<BE_Permiso> permisos;
        public List<BE_Permiso> Permisos
        {
            get { return permisos; }
            set { permisos = value; }
        }

        private List<BE_Rol> subRoles;
        public List<BE_Rol> SubRoles
        {
            get { return subRoles; }
            set { subRoles = value; }
        }

        public BE_Rol()
        {
            permisos = new List<BE_Permiso>();
            subRoles = new List<BE_Rol>();
        }

        public BE_Rol(string nombre, string descripcion = "", int? idPadreRol = null)
            : this()
        {
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.IdPadreRol = idPadreRol;
        }

        /// <summary>
        /// Agrega un permiso a este rol
        /// </summary>
        public void AgregarPermiso(BE_Permiso permiso)
        {
            if (permiso != null && !Permisos.Contains(permiso))
            {
                Permisos.Add(permiso);
            }
        }

        /// <summary>
        /// Agrega un sub-rol (patrón Composite). No permite auto-referencia.
        /// La detección de ciclos A→B→A se valida en BLL al persistir.
        /// </summary>
        public void AgregarSubRol(BE_Rol subRol)
        {
            if (subRol != null && subRol.ID != this.ID && !SubRoles.Contains(subRol))
            {
                SubRoles.Add(subRol);
                subRol.RolPadre = this;
                subRol.IdPadreRol = this.ID;
            }
        }

        /// <summary>
        /// Quita un sub-rol integrante sin eliminarlo del sistema (desasignar).
        /// </summary>
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

        /// <summary>
        /// Obtiene todos los permisos incluyendo los heredados de roles padres
        /// </summary>
        public HashSet<BE_Permiso> ObtenerPermisosHeredados()
        {
            HashSet<BE_Permiso> permisosTotal = new HashSet<BE_Permiso>();

            // Agregar permisos propios
            foreach (var permiso in Permisos)
            {
                permisosTotal.Add(permiso);
            }

            // Agregar permisos del rol padre
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

        /// <summary>
        /// Obtiene todos los permisos incluyendo sub-roles (recursivo)
        /// </summary>
        public HashSet<BE_Permiso> ObtenerTodosLosPermisos()
        {
            HashSet<BE_Permiso> permisosTotal = new HashSet<BE_Permiso>();

            // Agregar permisos propios
            foreach (var permiso in Permisos)
            {
                permisosTotal.Add(permiso);
            }

            // Agregar permisos heredados del padre
            if (RolPadre != null)
            {
                var permisosHeredados = RolPadre.ObtenerPermisosHeredados();
                foreach (var permiso in permisosHeredados)
                {
                    permisosTotal.Add(permiso);
                }
            }

            // Agregar permisos de sub-roles
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

        /// <summary>
        /// Verifica si tiene un permiso específico
        /// </summary>
        public bool TienePermiso(BE_Permiso permiso)
        {
            return ObtenerTodosLosPermisos().Contains(permiso);
        }

        /// <summary>
        /// Verifica si tiene permiso por código
        /// </summary>
        public bool TienePermisoPorCodigo(string codigo)
        {
            return ObtenerTodosLosPermisos().Any(p => p.Codigo == codigo);
        }

        /// <summary>
        /// Obtiene la profundidad en la jerarquía
        /// </summary>
        public int ObtenerProfundidad()
        {
            if (RolPadre == null)
                return 0;
            return RolPadre.ObtenerProfundidad() + 1;
        }

        /// <summary>
        /// Cuenta total de permisos (incluyendo heredados)
        /// </summary>
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
