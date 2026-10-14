using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<BE_Rol> ObtenerTodosLosRoles()
        {
            return mp.ListarTodos();
        }

        public List<BE_Permiso> ObtenerPermisosDelRol(int idRol)
        {
            return mp.ObtenerPermisos(idRol);
        }

        public List<BE_ComponenteAcceso> ObtenerComponentesDelRol(int idRol)
        {
            var componentes = new List<BE_ComponenteAcceso>();
            var idsPermisosDirectos = new HashSet<int>(ObtenerPermisosDelRol(idRol).Select(p => p.ID));

            foreach (var p in ObtenerPermisosDelRol(idRol))
            {
                componentes.Add(new BE_ComponenteAcceso
                {
                    ID = p.ID,
                    Tipo = BE_ComponenteAcceso.TipoPermiso,
                    Nombre = p.Nombre,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    EsHeredado = false,
                    Origen = BE_ComponenteAcceso.OrigenDirecto
                });
            }

            return componentes;
        }

        public List<BE_ComponenteAcceso> ObtenerComponentesDisponiblesParaAsignar(int idRol)
        {
            var disponibles = new List<BE_ComponenteAcceso>();
            var idsPermisosDirectos = new HashSet<int>(ObtenerPermisosDelRol(idRol).Select(p => p.ID));

            foreach (var p in ObtenerTodosLosPermisos())
            {
                if (idsPermisosDirectos.Contains(p.ID))
                    continue;

                disponibles.Add(new BE_ComponenteAcceso
                {
                    ID = p.ID,
                    Tipo = BE_ComponenteAcceso.TipoPermiso,
                    Nombre = p.Nombre,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion
                });
            }

            return disponibles;
        }

        public List<BE_Permiso> ObtenerTodosLosPermisos()
        {
            return mp.ObtenerTodosLosPermisos();
        }

        public void CrearRol(BE_Rol rol)
        {
            if (rol == null)
                throw new ArgumentNullException(nameof(rol));

            int resultado = mp.Alta(rol);
            if (resultado < 1)
                throw new Exception("Error al crear el rol.");

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Rol '{rol.Nombre}' creado");
            }
        }

        public void ActualizarRol(BE_Rol rol)
        {
            if (rol == null)
                throw new ArgumentNullException(nameof(rol));

            mp.Editar(rol);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Rol '{rol.Nombre}' actualizado");
            }
        }

        public void DesasignarComponente(int idRol, BE_ComponenteAcceso componente)
        {
            if (componente == null)
                throw new ArgumentNullException(nameof(componente));

            if (componente.EsPermiso)
                QuitarPermisoDelRol(idRol, componente.ID);
            else
                throw new InvalidOperationException("Tipo de componente no reconocido.");
        }

        public void AsignarComponente(int idRol, BE_ComponenteAcceso componente)
        {
            if (componente == null)
                throw new ArgumentNullException(nameof(componente));

            if (componente.EsPermiso)
                AgregarPermisoAlRol(idRol, componente.ID);
            else
                throw new InvalidOperationException("Tipo de componente no reconocido.");
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
