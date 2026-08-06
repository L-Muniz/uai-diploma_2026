using System;
using System.Collections.Generic;

namespace BE
{
    public class BE_ComponenteAcceso
    {
        public const string TipoPermiso = "PERMISO";
        public const string TipoRol = "ROL";
        public const string TipoPerfil = "PERFIL";

        public int ID { get; set; }
        public string Tipo { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public bool EsHeredado { get; set; }
        public string Origen { get; set; }

        public bool EsPermiso => string.Equals(Tipo, TipoPermiso, StringComparison.OrdinalIgnoreCase);
        public bool EsRol => string.Equals(Tipo, TipoRol, StringComparison.OrdinalIgnoreCase);
        public bool EsPerfil => string.Equals(Tipo, TipoPerfil, StringComparison.OrdinalIgnoreCase);
        public bool SePuedeDesasignar => !EsHeredado;
        public bool ProvieneDePerfil =>
            !string.IsNullOrEmpty(Origen)
            && Origen.IndexOf("Perfil:", StringComparison.OrdinalIgnoreCase) >= 0;
        public bool ProvieneDeRolPadre =>
            !string.IsNullOrEmpty(Origen)
            && Origen.IndexOf("Rol padre", StringComparison.OrdinalIgnoreCase) >= 0;

        public static string OrigenDirecto => "Directo";

        public static string OrigenDesdePerfil(string nombrePerfil)
        {
            return string.IsNullOrWhiteSpace(nombrePerfil)
                ? "Perfil"
                : $"Perfil: {nombrePerfil}";
        }

        public static string OrigenDesdePerfiles(IList<string> nombresPerfil)
        {
            if (nombresPerfil == null || nombresPerfil.Count == 0)
                return "Perfil";

            return "Perfil: " + string.Join(", ", nombresPerfil);
        }

        public static string OrigenDesdeRolPadre(string nombreRolPadre)
        {
            return string.IsNullOrWhiteSpace(nombreRolPadre)
                ? "Rol padre"
                : $"Rol padre: {nombreRolPadre}";
        }

        public static string CombinarOrigenes(IList<string> origenes)
        {
            if (origenes == null || origenes.Count == 0)
                return "Heredado";

            return string.Join(" + ", origenes);
        }

        public string Etiqueta => $"[{Tipo}] {Nombre}";

        public override string ToString()
        {
            return Etiqueta;
        }
    }
}
