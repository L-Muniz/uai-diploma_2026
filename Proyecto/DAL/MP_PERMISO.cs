using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class MP_PERMISO
    {
        public int Alta(BE_Permiso permiso)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sql = "PERMISO_CREAR";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@CODIGO", permiso.Codigo ?? ""));
                parametros.Add(acc.CrearParametro("@NOMBRE", permiso.Nombre ?? ""));
                parametros.Add(acc.CrearParametro("@DESCRIPCION", permiso.Descripcion ?? ""));
                parametros.Add(acc.CrearParametro("@ES_COMPUESTO", 0));

                int resultado = acc.Verificar(sql, parametros);
                acc.Cerrar();
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear permiso: " + ex.Message);
            }
        }

        public void Editar(BE_Permiso permiso)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sql = "PERMISO_ACTUALIZAR";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@ID_PERMISO", permiso.ID));
                parametros.Add(acc.CrearParametro("@CODIGO", permiso.Codigo ?? ""));
                parametros.Add(acc.CrearParametro("@NOMBRE", permiso.Nombre ?? ""));
                parametros.Add(acc.CrearParametro("@DESCRIPCION", permiso.Descripcion ?? ""));
                parametros.Add(acc.CrearParametro("@ES_COMPUESTO", 0));

                acc.Escribir(sql, parametros);
                acc.Cerrar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar permiso: " + ex.Message);
            }
        }

        public void Borrar(int idPermiso)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sql = "PERMISO_ELIMINAR";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@ID_PERMISO", idPermiso));

                acc.Escribir(sql, parametros);
                acc.Cerrar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar permiso: " + ex.Message);
            }
        }

        public List<BE_Permiso> Listar()
        {
            List<BE_Permiso> permisos = new List<BE_Permiso>();

            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sql = "PERMISO_LISTAR";
                DataTable dt = acc.Leer(sql);
                acc.Cerrar();

                foreach (DataRow row in dt.Rows)
                {
                    permisos.Add(MapearDatos(row));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar permisos: " + ex.Message);
            }

            return permisos;
        }

        public BE_Permiso ObtenerPorId(int idPermiso)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sql = "PERMISO_OBTENER_POR_ID";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@ID_PERMISO", idPermiso));

                DataTable dt = acc.Leer(sql, parametros);
                acc.Cerrar();

                if (dt.Rows.Count > 0)
                {
                    return MapearDatos(dt.Rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener permiso: " + ex.Message);
            }

            return null;
        }

        public bool ExisteCodigoDuplicado(string codigo, int idPermisoExcluir = 0)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sql = "PERMISO_EXISTE_CODIGO";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@CODIGO", codigo));
                parametros.Add(acc.CrearParametro("@ID_EXCLUSION", idPermisoExcluir));

                int resultado = acc.Verificar(sql, parametros);
                acc.Cerrar();
                return resultado == 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar código duplicado: " + ex.Message);
            }
        }

        public void AgregarComponente(int idPermisoCompuesto, int idPermisoComponente)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sql = "PERMISO_COMPONENTE_AGREGAR";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@ID_PERMISO_PADRE", idPermisoCompuesto));
                parametros.Add(acc.CrearParametro("@ID_PERMISO_HIJO", idPermisoComponente));

                acc.Escribir(sql, parametros);
                acc.Cerrar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar componente: " + ex.Message);
            }
        }

        public void QuitarComponente(int idPermisoCompuesto, int idPermisoComponente)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sql = "PERMISO_COMPONENTE_QUITAR";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@ID_PERMISO_PADRE", idPermisoCompuesto));
                parametros.Add(acc.CrearParametro("@ID_PERMISO_HIJO", idPermisoComponente));

                acc.Escribir(sql, parametros);
                acc.Cerrar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al quitar componente: " + ex.Message);
            }
        }

        public List<BE_Permiso> ObtenerComponentes(int idPermisoCompuesto)
        {
            List<BE_Permiso> componentes = new List<BE_Permiso>();

            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sql = "PERMISO_COMPONENTE_LISTAR";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@ID_PERMISO_PADRE", idPermisoCompuesto));

                DataTable dt = acc.Leer(sql, parametros);
                acc.Cerrar();

                foreach (DataRow row in dt.Rows)
                {
                    componentes.Add(MapearDatos(row));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener componentes: " + ex.Message);
            }

            return componentes;
        }

        private BE_Permiso MapearDatos(DataRow row)
        {
            BE_Permiso permiso = new BE_Permiso();
            permiso.ID = Convert.ToInt32(row["ID_PERMISO"]);
            permiso.Codigo = row["CODIGO"].ToString();
            permiso.Nombre = row["NOMBRE"].ToString();
            permiso.Descripcion = row["DESCRIPCION"] != DBNull.Value ? row["DESCRIPCION"].ToString() : "";
            return permiso;
        }
    }
}
