using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace SERVICE
{
    public class SV_PerfilComposite
    {
        public static List<BE_Perfil> ConstructorArbolPerfiles(List<BE_Perfil> perfilesPlanos)
        {
            List<BE_Perfil> raices = new List<BE_Perfil>();

            foreach (var perfil in perfilesPlanos)
            {
                if (!perfil.IdPadrePerfil.HasValue)
                {
                    raices.Add(perfil);
                }
            }

            foreach (var perfil in perfilesPlanos)
            {
                if (perfil.IdPadrePerfil.HasValue)
                {
                    var padre = perfilesPlanos.FirstOrDefault(p => p.ID == perfil.IdPadrePerfil.Value);
                    if (padre != null)
                    {
                        padre.AgregarSubperfil(perfil);
                    }
                }
            }

            return raices;
        }

        public static string VisualizarArbolPerfiles(List<BE_Perfil> raices)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var perfil in raices)
            {
                VisualizarArbolRecursivo(perfil, 0, sb);
            }

            return sb.ToString();
        }

        private static void VisualizarArbolRecursivo(BE_Perfil perfil, int nivel, StringBuilder sb)
        {
            string indentacion = new string(' ', nivel * 2) + "├─ ";
            sb.AppendLine($"{indentacion}{perfil.Nombre} ({perfil.Permisos.Count} permisos)");

            foreach (var permiso in perfil.Permisos)
            {
                string indentPermiso = new string(' ', (nivel + 1) * 2) + "└─ ";
                sb.AppendLine($"{indentPermiso}[{permiso.Codigo}] {permiso.Nombre}");
            }

            foreach (var subperfil in perfil.Subperfiles)
            {
                VisualizarArbolRecursivo(subperfil, nivel + 1, sb);
            }
        }

        public static string ObtenerRutaPerfil(BE_Perfil perfil)
        {
            Stack<string> ruta = new Stack<string>();
            BE_Perfil actual = perfil;

            while (actual != null)
            {
                ruta.Push(actual.Nombre);
                actual = actual.PerfilPadre;
            }

            return string.Join(" > ", ruta);
        }

        public static int ContarPermisosEnArbol(List<BE_Perfil> raices)
        {
            HashSet<BE_Permiso> permisosUnicos = new HashSet<BE_Permiso>();

            foreach (var perfil in raices)
            {
                ContarPermisosRecursivo(perfil, permisosUnicos);
            }

            return permisosUnicos.Count;
        }

        private static void ContarPermisosRecursivo(BE_Perfil perfil, HashSet<BE_Permiso> permisosUnicos)
        {
            foreach (var permiso in perfil.Permisos)
            {
                permisosUnicos.Add(permiso);
            }

            foreach (var subperfil in perfil.Subperfiles)
            {
                ContarPermisosRecursivo(subperfil, permisosUnicos);
            }
        }

        public static BE_Perfil BuscarPerfilPorNombre(List<BE_Perfil> raices, string nombre)
        {
            foreach (var perfil in raices)
            {
                var encontrado = BuscarPerfilRecursivo(perfil, nombre);
                if (encontrado != null)
                    return encontrado;
            }
            return null;
        }

        private static BE_Perfil BuscarPerfilRecursivo(BE_Perfil perfil, string nombre)
        {
            if (perfil.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase))
                return perfil;

            foreach (var subperfil in perfil.Subperfiles)
            {
                var encontrado = BuscarPerfilRecursivo(subperfil, nombre);
                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }

        public static List<BE_Perfil> ObtenerTodosLosPerfiles(List<BE_Perfil> raices)
        {
            List<BE_Perfil> todosLosPerfiles = new List<BE_Perfil>();

            foreach (var perfil in raices)
            {
                ObtenerTodosLosPerfilesRecursivo(perfil, todosLosPerfiles);
            }

            return todosLosPerfiles;
        }

        private static void ObtenerTodosLosPerfilesRecursivo(BE_Perfil perfil, List<BE_Perfil> lista)
        {
            lista.Add(perfil);

            foreach (var subperfil in perfil.Subperfiles)
            {
                ObtenerTodosLosPerfilesRecursivo(subperfil, lista);
            }
        }

        public static int ObtenerProfundidadMaxima(List<BE_Perfil> raices)
        {
            int profundidadMaxima = 0;

            foreach (var perfil in raices)
            {
                int profundidad = ObtenerProfundidadRecursiva(perfil);
                if (profundidad > profundidadMaxima)
                    profundidadMaxima = profundidad;
            }

            return profundidadMaxima;
        }

        private static int ObtenerProfundidadRecursiva(BE_Perfil perfil)
        {
            if (perfil.Subperfiles.Count == 0)
                return 0;

            int maxSubperfil = 0;
            foreach (var subperfil in perfil.Subperfiles)
            {
                int profundidad = ObtenerProfundidadRecursiva(subperfil);
                if (profundidad > maxSubperfil)
                    maxSubperfil = profundidad;
            }

            return maxSubperfil + 1;
        }

        public static Dictionary<string, object> ObtenerEstadisticasArbol(List<BE_Perfil> raices)
        {
            var estadisticas = new Dictionary<string, object>();
            var todosPerfiles = ObtenerTodosLosPerfiles(raices);

            estadisticas["TotalPerfiles"] = todosPerfiles.Count;
            estadisticas["TotalPermisos"] = ContarPermisosEnArbol(raices);
            estadisticas["ProfundidadMaxima"] = ObtenerProfundidadMaxima(raices);
            estadisticas["PerfilesRaiz"] = raices.Count;

            return estadisticas;
        }

    }
}
