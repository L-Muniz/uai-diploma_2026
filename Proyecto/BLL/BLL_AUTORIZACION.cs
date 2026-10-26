using System;
using System.Linq;
using BE;

namespace BLL
{
    // Control de acceso por permiso para los procesos de negocio.
    // El permiso se asigna al ROL del usuario (catálogo PERMISO / ROL_PERMISO). El rol ADMIN conserva acceso total.
    public static class BLL_AUTORIZACION
    {
        public static bool TienePermiso(BE_USUARIO usuario, string codigo)
        {
            if (usuario == null || string.IsNullOrWhiteSpace(usuario.Rol))
                return false;
            if (BLL_USUARIO.EsRolAdministrador(usuario.Rol))
                return true;

            BLL_ROL bllRol = new BLL_ROL();
            BE_Rol rol = bllRol.ObtenerTodosLosRoles()
                .FirstOrDefault(r => string.Equals(r.Nombre, usuario.Rol, StringComparison.OrdinalIgnoreCase));
            if (rol == null)
                return false;

            return bllRol.ObtenerPermisosDelRol(rol.ID)
                .Any(p => string.Equals(p.Codigo, codigo, StringComparison.OrdinalIgnoreCase));
        }

        // Lanza una excepción si el usuario no tiene el permiso. Se llama al inicio de cada operación de negocio.
        public static void Exigir(BE_USUARIO usuario, string codigo)
        {
            if (!TienePermiso(usuario, codigo))
                throw new UnauthorizedAccessException($"Su rol no tiene permiso para esta operación ({codigo}).");
        }
    }
}
