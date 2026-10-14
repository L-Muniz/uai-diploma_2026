using System.Collections.Generic;

namespace BE
{
    public interface I_ComponenteAcceso
    {
        int ID { get; set; }
        string Nombre { get; set; }
        string Descripcion { get; set; }
        string Codigo { get; set; }

        HashSet<BE_PermisoSimple> ObtenerTodosLosPermisos();
        bool TienePermiso(BE_PermisoSimple permiso);
        bool TienePermisoPorCodigo(string codigo);
    }
}
