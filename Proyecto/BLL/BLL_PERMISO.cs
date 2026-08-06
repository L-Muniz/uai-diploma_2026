using System;
using System.Collections.Generic;
using BE;
using DAL;

namespace BLL
{
    public class BLL_PERMISO
    {
        private MP_PERMISO mpPermiso;
        private BLL_BITACORA bllBitacora;

        public BLL_PERMISO()
        {
            mpPermiso = new MP_PERMISO();
            bllBitacora = new BLL_BITACORA();
        }

        public void CrearPermiso(BE_Permiso permiso)
        {
            ValidarPermiso(permiso);

            if (mpPermiso.ExisteCodigoDuplicado(permiso.Codigo))
                throw new Exception("El código de permiso ya existe");

            try
            {
                int idPermiso = mpPermiso.Alta(permiso);
                permiso.ID = idPermiso;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear permiso: " + ex.Message);
            }
        }

        public void ActualizarPermiso(BE_Permiso permiso)
        {
            ValidarPermiso(permiso);

            if (mpPermiso.ExisteCodigoDuplicado(permiso.Codigo, permiso.ID))
                throw new Exception("El código de permiso ya existe");

            try
            {
                mpPermiso.Editar(permiso);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar permiso: " + ex.Message);
            }
        }

        public void EliminarPermiso(int idPermiso)
        {
            try
            {
                BE_Permiso permiso = mpPermiso.ObtenerPorId(idPermiso);
                if (permiso == null)
                    throw new Exception("Permiso no encontrado");

                mpPermiso.Borrar(idPermiso);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar permiso: " + ex.Message);
            }
        }

        public List<BE_Permiso> ObtenerTodosLosPermisos()
        {
            try
            {
                return mpPermiso.Listar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener permisos: " + ex.Message);
            }
        }

        public BE_Permiso ObtenerPermisoPorId(int idPermiso)
        {
            try
            {
                return mpPermiso.ObtenerPorId(idPermiso);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener permiso: " + ex.Message);
            }
        }

        private void ValidarPermiso(BE_Permiso permiso)
        {
            if (permiso == null)
                throw new ArgumentNullException(nameof(permiso));

            if (string.IsNullOrWhiteSpace(permiso.Codigo))
                throw new Exception("El código del permiso no puede estar vacío");

            if (string.IsNullOrWhiteSpace(permiso.Nombre))
                throw new Exception("El nombre del permiso no puede estar vacío");

            if (permiso.Codigo.Length > 50)
                throw new Exception("El código no puede exceder 50 caracteres");

            if (permiso.Nombre.Length > 100)
                throw new Exception("El nombre no puede exceder 100 caracteres");
        }
    }
}
