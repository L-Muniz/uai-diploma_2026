using System;
using System.Collections.Generic;
using BE;
using DAL;
using SERVICE;

namespace BLL
{
    public class BLL_ROL
    {
        private MP_ROL mp = new MP_ROL();

        private BE_USUARIO ObtenerUsuarioActual()
        {
            return SV_Singleton.GetInstance().Usuario;
        }

        public List<BE_Rol> ObtenerRolesPrincipales()
        {
            return mp.Listar();
        }

        public List<BE_Rol> ObtenerRolesHijos(int idRolPadre)
        {
            return mp.ObtenerRolesHijos(idRolPadre);
        }

        public List<BE_Permiso> ObtenerPermisosDelRol(int idRol)
        {
            return mp.ObtenerPermisos(idRol);
        }

        public List<BE_Permiso> ObtenerPermisosConHerencia(int idRol)
        {
            var rol = mp.ObtenerPorId(idRol);
            if (rol == null) return new List<BE_Permiso>();

            var permisosDirectos = ObtenerPermisosDelRol(idRol);
            var permisos = new Dictionary<int, BE_Permiso>();

            foreach (var p in permisosDirectos)
            {
                permisos[p.ID] = p;
            }

            if (rol.IdPadreRol.HasValue && rol.IdPadreRol.Value > 0)
            {
                var permisosDelPadre = ObtenerPermisosConHerencia(rol.IdPadreRol.Value);
                foreach (var p in permisosDelPadre)
                {
                    if (!permisos.ContainsKey(p.ID))
                    {
                        permisos[p.ID] = p;
                    }
                }
            }

            return new List<BE_Permiso>(permisos.Values);
        }

        public List<BE_Permiso> ObtenerTodosLosPermisos()
        {
            return mp.ObtenerTodosLosPermisos();
        }

        public void CrearRol(BE_Rol rol)
        {
            mp.Alta(rol);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Rol '{rol.Nombre}' creado");
            }
        }

        public void CrearRol(string nombre, string descripcion)
        {
            BE_Rol rol = new BE_Rol(nombre, descripcion);
            CrearRol(rol);
        }

        public void ActualizarRol(BE_Rol rol)
        {
            mp.Editar(rol);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Rol '{rol.Nombre}' actualizado");
            }
        }

        public void EliminarRol(int idRol)
        {
            BE_Rol rol = new BE_Rol();
            rol.ID = idRol;
            var nombreRol = rol.Nombre ?? $"ID {idRol}";
            mp.Borrar(rol);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Rol '{nombreRol}' eliminado");
            }
        }

        public void AgregarPermisoAlRol(int idRol, int idPermiso)
        {
            mp.AgregarPermiso(idRol, idPermiso);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Permiso agregado a rol ID {idRol}");
            }
        }

        public void QuitarPermisoDelRol(int idRol, int idPermiso)
        {
            mp.QuitarPermiso(idRol, idPermiso);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Permiso quitado del rol ID {idRol}");
            }
        }

        public BE_Rol ObtenerRolPorID(int idRol)
        {
            return mp.ObtenerPorID(idRol);
        }
    }
}
