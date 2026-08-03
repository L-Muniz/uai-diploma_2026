using System;
using System.Collections.Generic;
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

        public void CrearPerfil(string nombre, string descripcion)
        {
            BE_Perfil perfil = new BE_Perfil();
            perfil.Nombre = nombre;
            perfil.Descripcion = descripcion;
            mp.Alta(perfil);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Perfil '{nombre}' creado");
            }
        }

        public void ActualizarPerfil(BE_Perfil perfil)
        {
            mp.Editar(perfil);

            var usuario = ObtenerUsuarioActual();
            if (usuario != null)
            {
                BLL_BITACORA bitacora = new BLL_BITACORA();
                bitacora.IngresarRegistro(usuario, $"Perfil '{perfil.Nombre}' actualizado");
            }
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
    }
}
