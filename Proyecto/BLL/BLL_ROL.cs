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

        /// <summary>
        /// Devuelve los permisos asignados al rol (directos) y, de forma recursiva, todos los
        /// componentes de cada permiso compuesto asignado, a cualquier nivel de profundidad.
        /// Los componentes que llegan por un compuesto se marcan como heredados.
        /// </summary>
        public List<BE_ComponenteAcceso> ObtenerComponentesDelRol(int idRol)
        {
            var componentes = new List<BE_ComponenteAcceso>();
            var visitados = new HashSet<int>();
            var bllPermiso = new BLL_PERMISO();

            foreach (var p in ObtenerPermisosDelRol(idRol))
            {
                if (!visitados.Add(p.ID))
                    continue;

                componentes.Add(ConvertirPermisoAComponente(p, false, BE_ComponenteAcceso.OrigenDirecto));
                AgregarComponentesRecursivos(p.ID, p.Nombre, componentes, visitados, bllPermiso);
            }

            return componentes;
        }

        private void AgregarComponentesRecursivos(int idPadre, string nombrePadre, List<BE_ComponenteAcceso> destino,
            HashSet<int> visitados, BLL_PERMISO bllPermiso)
        {
            foreach (var hijo in bllPermiso.ObtenerComponentes(idPadre))
            {
                if (!visitados.Add(hijo.ID))
                    continue;

                destino.Add(ConvertirPermisoAComponente(hijo, true, "Compuesto: " + nombrePadre));
                AgregarComponentesRecursivos(hijo.ID, hijo.Nombre, destino, visitados, bllPermiso);
            }
        }

        private static BE_ComponenteAcceso ConvertirPermisoAComponente(BE_Permiso p, bool heredado, string origen)
        {
            return new BE_ComponenteAcceso
            {
                ID = p.ID,
                Tipo = BE_ComponenteAcceso.TipoPermiso,
                Nombre = p.Nombre,
                Codigo = p.Codigo,
                Descripcion = p.Descripcion,
                EsHeredado = heredado,
                Origen = origen
            };
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
            BLL_ControlCambios.RegistrarCambio("ROL", rol.ID, "CREACION", "", rol.Nombre, usuario, "Rol creado");
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

            var antes = CrearMemento(rol.ID);
            mp.Editar(rol);
            var despues = CrearMemento(rol.ID);

            var usuario = ObtenerUsuarioActual();
            BLL_ControlCambios.RegistrarMementoRol(antes, despues, usuario, "Rol actualizado");

            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Rol '{rol.Nombre}' actualizado");
            }
        }

        /// <summary>
        /// Originator del patrón Memento: captura el estado del rol (nombre, descripción y permisos directos).
        /// </summary>
        public BE_MementoRol CrearMemento(int idRol)
        {
            var rol = mp.ObtenerPorId(idRol);
            if (rol == null)
                throw new Exception($"Rol ID {idRol} no encontrado.");

            return new BE_MementoRol
            {
                ID = rol.ID,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion,
                IdsPermisos = ObtenerPermisosDelRol(idRol).Select(p => p.ID).OrderBy(i => i).ToList()
            };
        }

        /// <summary>
        /// Originator del patrón Memento: restaura el estado guardado en el memento.
        /// Solo modifica datos; quien llama es responsable de registrar el cambio en el control de cambios.
        /// </summary>
        public void RestaurarMemento(BE_MementoRol memento)
        {
            if (memento == null)
                throw new ArgumentNullException(nameof(memento));

            var rol = mp.ObtenerPorId(memento.ID);
            if (rol == null)
                throw new Exception($"Rol ID {memento.ID} no encontrado.");

            rol.Nombre = memento.Nombre;
            rol.Descripcion = memento.Descripcion;
            mp.Editar(rol);

            var actuales = ObtenerPermisosDelRol(memento.ID).Select(p => p.ID).ToList();
            foreach (int idPermiso in actuales.Except(memento.IdsPermisos).ToList())
                mp.QuitarPermiso(memento.ID, idPermiso);

            foreach (int idPermiso in memento.IdsPermisos.Except(actuales).ToList())
                mp.AgregarPermiso(memento.ID, idPermiso);
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
            BE_Rol rol = mp.ObtenerPorId(idRol) ?? new BE_Rol { ID = idRol };
            var nombreRol = rol.Nombre ?? $"ID {idRol}";
            mp.Borrar(rol);

            var usuario = ObtenerUsuarioActual();
            // La eliminación se audita pero no es reversible: el registro no conserva el ID original del rol.
            BLL_ControlCambios.RegistrarCambio("ROL", idRol, "ELIMINACION", nombreRol, "", usuario, "Rol eliminado");
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Rol '{nombreRol}' eliminado");
            }
        }

        public void AgregarPermisoAlRol(int idRol, int idPermiso)
        {
            var antes = CrearMemento(idRol);
            mp.AgregarPermiso(idRol, idPermiso);
            var despues = CrearMemento(idRol);

            var usuario = ObtenerUsuarioActual();
            BLL_ControlCambios.RegistrarMementoRol(antes, despues, usuario, "Permiso agregado");

            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Permiso agregado a rol ID {idRol}");
            }
        }

        public void QuitarPermisoDelRol(int idRol, int idPermiso)
        {
            var antes = CrearMemento(idRol);
            mp.QuitarPermiso(idRol, idPermiso);
            var despues = CrearMemento(idRol);

            var usuario = ObtenerUsuarioActual();
            BLL_ControlCambios.RegistrarMementoRol(antes, despues, usuario, "Permiso quitado");

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
