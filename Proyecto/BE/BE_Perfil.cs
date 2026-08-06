using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    
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

        
        public void AgregarPermiso(BE_Permiso permiso)
        {
            if (permiso != null && !Permisos.Contains(permiso))
            {
                Permisos.Add(permiso);
            }
        }

        
        public void AgregarSubperfil(BE_Perfil subperfil)
        {
            if (subperfil != null && subperfil.ID != this.ID && !Subperfiles.Contains(subperfil))
            {
                Subperfiles.Add(subperfil);
                subperfil.PerfilPadre = this;
                subperfil.IdPadrePerfil = this.ID;
            }
        }

        public void QuitarSubperfil(BE_Perfil subperfil)
        {
            if (subperfil == null)
                return;

            if (Subperfiles.Remove(subperfil))
            {
                if (subperfil.PerfilPadre == this)
                    subperfil.PerfilPadre = null;
                if (subperfil.IdPadrePerfil == this.ID)
                    subperfil.IdPadrePerfil = null;
            }
        }

        
        public HashSet<BE_Permiso> ObtenerPermisosHeredados()
        {
            HashSet<BE_Permiso> permisosTotal = new HashSet<BE_Permiso>();

            foreach (var permiso in Permisos)
            {
                permisosTotal.Add(permiso);
            }

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

        
        public HashSet<BE_Permiso> ObtenerTodosLosPermisos()
        {
            HashSet<BE_Permiso> permisosTotal = new HashSet<BE_Permiso>();

            foreach (var permiso in Permisos)
            {
                permisosTotal.Add(permiso);
            }

            if (PerfilPadre != null)
            {
                var permisosHeredados = PerfilPadre.ObtenerPermisosHeredados();
                foreach (var permiso in permisosHeredados)
                {
                    permisosTotal.Add(permiso);
                }
            }

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

        
        public bool TienePermiso(BE_Permiso permiso)
        {
            return ObtenerTodosLosPermisos().Contains(permiso);
        }

        
        public bool TienePermisoPorCodigo(string codigo)
        {
            return ObtenerTodosLosPermisos().Any(p => p.Codigo == codigo);
        }

        
        public int ObtenerProfundidad()
        {
            if (PerfilPadre == null)
                return 0;
            return PerfilPadre.ObtenerProfundidad() + 1;
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
