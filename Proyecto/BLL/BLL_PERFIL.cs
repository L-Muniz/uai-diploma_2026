using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using DAL;
using SERVICE;

namespace BLL
{
    public class BLL_PERFIL
    {
        private MP_PERFIL mp = new MP_PERFIL();

        private BE_USUARIO ObtenerUsuarioActual()
        {
            return SV_Singleton.GetInstance().Usuario;
        }

        public List<BE_Perfil> ObtenerPerfilesPrincipales()
        {
            return mp.Listar();
        }

        public List<BE_Perfil> ObtenerTodosLosPerfiles()
        {
            return mp.ListarTodos();
        }

        public List<BE_Perfil> ObtenerPerfilesHijos(int idPerfilPadre)
        {
            return mp.ObtenerPerfilesHijos(idPerfilPadre);
        }

        public List<BE_Permiso> ObtenerPermisosDelPerfil(int idPerfil)
        {
            return mp.ObtenerPermisos(idPerfil);
        }

        public List<BE_Permiso> ObtenerTodosLosPermisos()
        {
            return mp.ObtenerTodosLosPermisos();
        }

        public List<BE_Permiso> ObtenerPermisosConHerencia(int idPerfil)
        {
            return ObtenerPermisosConHerencia(idPerfil, new HashSet<int>());
        }

        private List<BE_Permiso> ObtenerPermisosConHerencia(int idPerfil, HashSet<int> visitados)
        {
            if (!visitados.Add(idPerfil))
                return new List<BE_Permiso>();

            var perfil = mp.ObtenerPorId(idPerfil);
            if (perfil == null) return new List<BE_Permiso>();

            var permisos = new Dictionary<int, BE_Permiso>();
            foreach (var p in ObtenerPermisosDelPerfil(idPerfil))
                permisos[p.ID] = p;

            if (perfil.IdPadrePerfil.HasValue && perfil.IdPadrePerfil.Value > 0)
            {
                foreach (var p in ObtenerPermisosConHerencia(perfil.IdPadrePerfil.Value, visitados))
                {
                    if (!permisos.ContainsKey(p.ID))
                        permisos[p.ID] = p;
                }
            }

            return new List<BE_Permiso>(permisos.Values);
        }

        public List<BE_ComponenteAcceso> ObtenerComponentesDelPerfil(int idPerfil)
        {
            var componentes = new List<BE_ComponenteAcceso>();
            var idsPermisosDirectos = new HashSet<int>(ObtenerPermisosDelPerfil(idPerfil).Select(p => p.ID));

            foreach (var p in ObtenerPermisosConHerencia(idPerfil))
            {
                componentes.Add(new BE_ComponenteAcceso
                {
                    ID = p.ID,
                    Tipo = BE_ComponenteAcceso.TipoPermiso,
                    Nombre = p.Nombre,
                    Codigo = p.Codigo,
                    Descripcion = p.Descripcion,
                    EsHeredado = !idsPermisosDirectos.Contains(p.ID)
                });
            }

            foreach (var hijo in ObtenerPerfilesHijos(idPerfil))
            {
                componentes.Add(new BE_ComponenteAcceso
                {
                    ID = hijo.ID,
                    Tipo = BE_ComponenteAcceso.TipoPerfil,
                    Nombre = hijo.Nombre,
                    Codigo = "PERFIL",
                    Descripcion = string.IsNullOrWhiteSpace(hijo.Descripcion)
                        ? "Perfil integrante (composite)"
                        : hijo.Descripcion,
                    EsHeredado = false
                });
            }

            return componentes;
        }

        public List<BE_ComponenteAcceso> ObtenerComponentesDisponiblesParaAsignar(int idPerfil)
        {
            var disponibles = new List<BE_ComponenteAcceso>();
            var idsPermisosDirectos = new HashSet<int>(ObtenerPermisosDelPerfil(idPerfil).Select(p => p.ID));
            var idsHijos = new HashSet<int>(ObtenerPerfilesHijos(idPerfil).Select(p => p.ID));

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

            foreach (var candidato in ObtenerTodosLosPerfiles())
            {
                if (candidato.ID == idPerfil || idsHijos.Contains(candidato.ID))
                    continue;
                if (FormariaReferenciaCircular(candidato.ID, idPerfil))
                    continue;

                disponibles.Add(new BE_ComponenteAcceso
                {
                    ID = candidato.ID,
                    Tipo = BE_ComponenteAcceso.TipoPerfil,
                    Nombre = candidato.Nombre,
                    Codigo = "PERFIL",
                    Descripcion = candidato.Descripcion
                });
            }

            return disponibles;
        }

        public bool FormariaReferenciaCircular(int idPerfil, int? idNuevoPadre)
        {
            if (!idNuevoPadre.HasValue || idNuevoPadre.Value <= 0)
                return false;

            if (idPerfil > 0 && idPerfil == idNuevoPadre.Value)
                return true;

            var visitados = new HashSet<int>();
            int? actual = idNuevoPadre;

            while (actual.HasValue && actual.Value > 0)
            {
                if (idPerfil > 0 && actual.Value == idPerfil)
                    return true;

                if (!visitados.Add(actual.Value))
                    return true;

                var nodo = mp.ObtenerPorId(actual.Value);
                actual = nodo?.IdPadrePerfil;
            }

            return false;
        }

        public void ValidarAsignacionPadre(int idPerfil, int? idNuevoPadre)
        {
            if (FormariaReferenciaCircular(idPerfil, idNuevoPadre))
                throw new InvalidOperationException("No se permite referencia circular en la asignación de perfiles.");
        }

        public void CrearPerfil(string nombre, string descripcion)
        {
            CrearPerfil(new BE_Perfil(nombre, descripcion));
        }

        public void CrearPerfil(BE_Perfil perfil)
        {
            if (perfil == null)
                throw new ArgumentNullException(nameof(perfil));

            ValidarAsignacionPadre(perfil.ID, perfil.IdPadrePerfil);

            int filas = mp.Alta(perfil);
            if (filas < 1)
                throw new Exception("Error al crear el perfil.");

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Perfil '{perfil.Nombre}' creado");
            }
        }

        public void ActualizarPerfil(BE_Perfil perfil)
        {
            if (perfil == null)
                throw new ArgumentNullException(nameof(perfil));

            ValidarAsignacionPadre(perfil.ID, perfil.IdPadrePerfil);
            mp.Editar(perfil);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Perfil '{perfil.Nombre}' actualizado");
            }
        }

        public void AsignarSubPerfil(int idPerfilPadre, int idPerfilHijo)
        {
            ValidarAsignacionPadre(idPerfilHijo, idPerfilPadre);
            mp.AsignarPadre(idPerfilHijo, idPerfilPadre);

            var hijo = mp.ObtenerPorId(idPerfilHijo);
            if (hijo == null || hijo.IdPadrePerfil != idPerfilPadre)
                throw new Exception("Error al asignar el perfil integrante.");

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Perfil ID {idPerfilHijo} asignado como integrante de perfil ID {idPerfilPadre}");
            }
        }

        public void DesasignarSubPerfil(int idPerfilPadre, int idPerfilHijo)
        {
            var hijo = mp.ObtenerPorId(idPerfilHijo);
            if (hijo == null)
                throw new Exception("Perfil integrante no encontrado.");

            if (!hijo.IdPadrePerfil.HasValue || hijo.IdPadrePerfil.Value != idPerfilPadre)
                throw new InvalidOperationException("El perfil seleccionado no es integrante directo de este perfil.");

            mp.AsignarPadre(idPerfilHijo, null);

            var actualizado = mp.ObtenerPorId(idPerfilHijo);
            if (actualizado == null || actualizado.IdPadrePerfil.HasValue)
                throw new Exception("Error al desasignar el perfil integrante.");

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Perfil ID {idPerfilHijo} desasignado del perfil ID {idPerfilPadre}");
            }
        }

        public void AsignarComponente(int idPerfil, BE_ComponenteAcceso componente)
        {
            if (componente == null)
                throw new ArgumentNullException(nameof(componente));

            if (componente.EsPerfil)
                AsignarSubPerfil(idPerfil, componente.ID);
            else if (componente.EsPermiso)
                AgregarPermisoAlPerfil(idPerfil, componente.ID);
            else
                throw new InvalidOperationException("Tipo de componente no reconocido.");
        }

        public void DesasignarComponente(int idPerfil, BE_ComponenteAcceso componente)
        {
            if (componente == null)
                throw new ArgumentNullException(nameof(componente));

            if (!componente.SePuedeDesasignar)
                throw new InvalidOperationException("No se puede desasignar un permiso heredado. Quitelo del perfil padre.");

            if (componente.EsPerfil)
                DesasignarSubPerfil(idPerfil, componente.ID);
            else if (componente.EsPermiso)
                QuitarPermisoDelPerfil(idPerfil, componente.ID);
            else
                throw new InvalidOperationException("Tipo de componente no reconocido.");
        }

        public void EliminarPerfil(int idPerfil)
        {
            BE_Perfil perfil = new BE_Perfil();
            perfil.ID = idPerfil;
            var nombrePerfil = perfil.Nombre ?? $"ID {idPerfil}";
            mp.Borrar(perfil);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Perfil '{nombrePerfil}' eliminado");
            }
        }

        public void AgregarPermisoAlPerfil(int idPerfil, int idPermiso)
        {
            mp.AgregarPermiso(idPerfil, idPermiso);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Permiso agregado a perfil ID {idPerfil}");
            }
        }

        public void QuitarPermisoDelPerfil(int idPerfil, int idPermiso)
        {
            mp.QuitarPermiso(idPerfil, idPermiso);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Permiso quitado del perfil ID {idPerfil}");
            }
        }

        public List<BE_Perfil> ObtenerPerfilesCandidatosComoPadre(int idPerfilHijo)
        {
            return ObtenerTodosLosPerfiles()
                .Where(p => p.ID != idPerfilHijo && !FormariaReferenciaCircular(idPerfilHijo, p.ID))
                .OrderBy(p => p.Nombre)
                .ToList();
        }
    }
}
