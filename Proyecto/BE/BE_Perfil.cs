using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Entidad que implementa patrón Composite para jerarquía de perfiles
    /// Un perfil puede contener permisos y otros perfiles (subperfiles)
    /// </summary>
    public class BE_Perfil
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

        private int? idPadrePerfil;
        public int? IdPadrePerfil
        {
            get { return idPadrePerfil; }
            set { idPadrePerfil = value; }
        }

        private BE_Perfil perfilPadre;
        public BE_Perfil PerfilPadre
        {
            get { return perfilPadre; }
            set { perfilPadre = value; }
        }

        private List<BE_Permiso> permisos;
        public List<BE_Permiso> Permisos
        {
            get { return permisos; }
            set { permisos = value; }
        }

        private List<BE_Perfil> subperfiles;
        public List<BE_Perfil> Subperfiles
        {
            get { return subperfiles; }
            set { subperfiles = value; }
        }

        public BE_Perfil()
        {
            permisos = new List<BE_Permiso>();
            subperfiles = new List<BE_Perfil>();
        }

        public BE_Perfil(string nombre, string descripcion = "", int? idPadrePerfil = null)
            : this()
        {
            this.Nombre = nombre;
            this.Descripcion = descripcion;
            this.IdPadrePerfil = idPadrePerfil;
        }

        /// <summary>
        /// Agrega un permiso a este perfil
        /// </summary>
        public void AgregarPermiso(BE_Permiso permiso)
        {
            if (permiso != null && !Permisos.Contains(permiso))
            {
                Permisos.Add(permiso);
            }
        }

        /// <summary>
        /// Agrega un subperfil (patrón Composite)
        /// </summary>
        public void AgregarSubperfil(BE_Perfil subperfil)
        {
            if (subperfil != null && subperfil.ID != this.ID)
            {
                Subperfiles.Add(subperfil);
                subperfil.PerfilPadre = this;
                subperfil.IdPadrePerfil = this.ID;
            }
        }

        /// <summary>
        /// Obtiene todos los permisos incluyendo los heredados de perfiles padres
        /// </summary>
        public HashSet<BE_Permiso> ObtenerPermisosHeredados()
        {
            HashSet<BE_Permiso> permisosTotal = new HashSet<BE_Permiso>();

            // Agregar permisos propios
            foreach (var permiso in Permisos)
            {
                permisosTotal.Add(permiso);
            }

            // Agregar permisos del perfil padre
            if (PerfilPadre != null)
            {
                var permisosHeredados = PerfilPadre.ObtenerPermisosHeredados();
                foreach (var permiso in permisosHeredados)
                {
                    permisosTotal.Add(permiso);
                }
            }

            return permisosTotal;
        }

        /// <summary>
        /// Obtiene todos los permisos incluyendo subperfiles (recursivo)
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
            if (PerfilPadre != null)
            {
                var permisosHeredados = PerfilPadre.ObtenerPermisosHeredados();
                foreach (var permiso in permisosHeredados)
                {
                    permisosTotal.Add(permiso);
                }
            }

            // Agregar permisos de subperfiles
            foreach (var subperfil in Subperfiles)
            {
                var permisosSubperfil = subperfil.ObtenerTodosLosPermisos();
                foreach (var permiso in permisosSubperfil)
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
            if (PerfilPadre == null)
                return 0;
            return PerfilPadre.ObtenerProfundidad() + 1;
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
