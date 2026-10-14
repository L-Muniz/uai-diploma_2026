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

        public List<BE_Rol> ObtenerRolesHijos(int idRolPadre)
        {
            return mp.ObtenerRolesHijos(idRolPadre);
        }

        public List<BE_Permiso> ObtenerPermisosDelRol(int idRol)
        {
            return mp.ObtenerPermisos(idRol);
        }

        public List<BE_ComponenteAcceso> ObtenerComponentesDelRol(int idRol)
        {
            var componentes = new List<BE_ComponenteAcceso>();
            var idsPermisosDirectos = new HashSet<int>(ObtenerPermisosDelRol(idRol).Select(p => p.ID));

            var rol = mp.ObtenerPorId(idRol);
            string nombrePadre = null;
            var idsPermisosPadre = new HashSet<int>();
            if (rol?.IdPadreRol.HasValue == true && rol.IdPadreRol.Value > 0)
            {
                var padre = mp.ObtenerPorId(rol.IdPadreRol.Value);
                nombrePadre = padre?.Nombre;
                foreach (var permisoPadre in ObtenerPermisosDelRol(rol.IdPadreRol.Value))
                    idsPermisosPadre.Add(permisoPadre.ID);
            }

            foreach (var p in ObtenerPermisosDelRol(idRol))
            {
                bool esDirecto = idsPermisosDirectos.Contains(p.ID);
                var origenes = new List<string>();

                if (esDirecto)
                    origenes.Add(BE_ComponenteAcceso.OrigenDirecto);

                if (idsPermisosPadre.Contains(p.ID))
                    origenes.Add(BE_ComponenteAcceso.OrigenDesdeRolPadre(nombrePadre));

                componentes.Add(new BE_ComponenteAcceso
                {
                    ID = p.ID,
                    Tipo = BE_ComponenteAcceso.TipoPermiso,
                    Nombre = p.Nombre,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    EsHeredado = !esDirecto,
                    Origen = BE_ComponenteAcceso.CombinarOrigenes(origenes)
                });
            }

            foreach (var hijo in ObtenerRolesHijos(idRol))
            {
                componentes.Add(new BE_ComponenteAcceso
                {
                    ID = hijo.ID,
                    Tipo = BE_ComponenteAcceso.TipoRol,
                    Nombre = hijo.Nombre,
                    Codigo = "ROL",
                    Descripcion = string.IsNullOrWhiteSpace(hijo.Descripcion)
                        ? "Rol integrante (composite)"
                        : hijo.Descripcion,
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
            var idsHijos = new HashSet<int>(ObtenerRolesHijos(idRol).Select(r => r.ID));

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

            foreach (var candidato in ObtenerTodosLosRoles())
            {
                if (candidato.ID == idRol || idsHijos.Contains(candidato.ID))
                    continue;
                if (FormariaReferenciaCircular(candidato.ID, idRol))
                    continue;

                disponibles.Add(new BE_ComponenteAcceso
                {
                    ID = candidato.ID,
                    Tipo = BE_ComponenteAcceso.TipoRol,
                    Nombre = candidato.Nombre,
                    Codigo = "ROL",
                    Descripcion = candidato.Descripcion
                });
            }

            return disponibles;
        }

        public List<BE_Permiso> ObtenerTodosLosPermisos()
        {
            return mp.ObtenerTodosLosPermisos();
        }

        public bool FormariaReferenciaCircular(int idRol, int? idNuevoPadre)
        {
            if (!idNuevoPadre.HasValue || idNuevoPadre.Value <= 0)
                return false;

            if (idRol > 0 && idRol == idNuevoPadre.Value)
                return true;

            var visitados = new HashSet<int>();
            int? actual = idNuevoPadre;

            while (actual.HasValue && actual.Value > 0)
            {
                if (idRol > 0 && actual.Value == idRol)
                    return true;

                if (!visitados.Add(actual.Value))
                    return true;

                var nodo = mp.ObtenerPorId(actual.Value);
                actual = nodo?.IdPadreRol;
            }

            return false;
        }

        public void ValidarAsignacionPadre(int idRol, int? idNuevoPadre)
        {
            if (FormariaReferenciaCircular(idRol, idNuevoPadre))
                throw new InvalidOperationException("No se permite referencia circular en la asignación de roles.");
        }

        public void CrearRol(BE_Rol rol)
        {
            if (rol == null)
                throw new ArgumentNullException(nameof(rol));

            ValidarAsignacionPadre(rol.ID, rol.IdPadreRol);

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

            ValidarAsignacionPadre(rol.ID, rol.IdPadreRol);
            mp.Editar(rol);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Rol '{rol.Nombre}' actualizado");
            }
        }

        public void AsignarSubRol(int idRolPadre, int idRolHijo)
        {
            if (idRolPadre <= 0 || idRolHijo <= 0)
                throw new ArgumentException("IDs de rol inválidos.");

            ValidarAsignacionPadre(idRolHijo, idRolPadre);
            mp.AsignarPadre(idRolHijo, idRolPadre);

            var hijo = mp.ObtenerPorId(idRolHijo);
            if (hijo == null || hijo.IdPadreRol != idRolPadre)
                throw new Exception("Error al asignar el rol integrante.");

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Rol ID {idRolHijo} asignado como integrante de rol ID {idRolPadre}");
            }
        }

        public void DesasignarSubRol(int idRolPadre, int idRolHijo)
        {
            var hijo = mp.ObtenerPorId(idRolHijo);
            if (hijo == null)
                throw new Exception("Rol integrante no encontrado.");

            if (!hijo.IdPadreRol.HasValue || hijo.IdPadreRol.Value != idRolPadre)
                throw new InvalidOperationException("El rol seleccionado no es integrante directo de este rol.");

            mp.AsignarPadre(idRolHijo, null);

            var actualizado = mp.ObtenerPorId(idRolHijo);
            if (actualizado == null || actualizado.IdPadreRol.HasValue)
                throw new Exception("Error al desasignar el rol integrante.");

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Rol ID {idRolHijo} desasignado del rol ID {idRolPadre}");
            }
        }

        public void DesasignarComponente(int idRol, BE_ComponenteAcceso componente)
        {
            if (componente == null)
                throw new ArgumentNullException(nameof(componente));

            if (!componente.SePuedeDesasignar)
            {
                if (componente.ProvieneDeRolPadre)
                    throw new InvalidOperationException("No se puede desasignar un permiso heredado. Quítelo del rol padre.");
            }

            if (componente.EsRol)
                DesasignarSubRol(idRol, componente.ID);
            else if (componente.EsPermiso)
                QuitarPermisoDelRol(idRol, componente.ID);
            else
                throw new InvalidOperationException("Tipo de componente no reconocido.");
        }

        public void AsignarComponente(int idRol, BE_ComponenteAcceso componente)
        {
            if (componente == null)
                throw new ArgumentNullException(nameof(componente));

            if (componente.EsRol)
                AsignarSubRol(idRol, componente.ID);
            else if (componente.EsPermiso)
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

        public List<BE_Rol> ObtenerRolesCandidatosComoPadre(int idRolHijo)
        {
            return ObtenerTodosLosRoles()
                .Where(r => r.ID != idRolHijo && !FormariaReferenciaCircular(idRolHijo, r.ID))
                .OrderBy(r => r.Nombre)
                .ToList();
        }
    }
}
