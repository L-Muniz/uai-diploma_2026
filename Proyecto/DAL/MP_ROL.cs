using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class MP_ROL : MAPPER<BE_Rol>
    {
        public override int Alta(BE_Rol obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "ROL_CREAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@NOMBRE", obj.Nombre));
            parametros.Add(acc.CrearParametro("@DESCRIPCION", obj.Descripcion ?? ""));
            int idNuevoRol = acc.Verificar(sql, parametros);
            acc.Cerrar();

            if (idNuevoRol > 0)
            {
                obj.ID = idNuevoRol;
            }

            return idNuevoRol;
        }

        public override int Borrar(BE_Rol obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "ROL_ELIMINAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_ROL", obj.ID));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override int Editar(BE_Rol obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "ROL_ACTUALIZAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_ROL", obj.ID));
            parametros.Add(acc.CrearParametro("@NOMBRE", obj.Nombre));
            parametros.Add(acc.CrearParametro("@DESCRIPCION", obj.Descripcion ?? ""));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override List<BE_Rol> Listar()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "ROL_LISTAR_TODOS";
            DataTable dt = acc.Leer(sql);
            acc.Cerrar();

            List<BE_Rol> lista = new List<BE_Rol>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearDatos(row));
            }
            return lista;
        }

        public List<BE_Rol> ListarTodos()
        {
            return Listar();
        }

        public BE_Rol ObtenerPorId(int idRol)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "ROL_OBTENER_POR_ID";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_ROL", idRol));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            if (dt.Rows.Count > 0)
            {
                return MapearDatos(dt.Rows[0]);
            }
            return null;
        }

        public List<BE_Permiso> ObtenerPermisos(int idRol)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "ROL_OBTENER_PERMISOS";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_ROL", idRol));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            List<BE_Permiso> lista = new List<BE_Permiso>();
            foreach (DataRow row in dt.Rows)
            {
                BE_Permiso permiso = new BE_Permiso();
                permiso.ID = int.Parse(row["ID_PERMISO"].ToString());
                permiso.Codigo = row["CODIGO"].ToString();
                permiso.Nombre = row["NOMBRE"].ToString();
                permiso.Descripcion = row["DESCRIPCION"].ToString();
                lista.Add(permiso);
            }
            return lista;
        }

        public List<BE_Permiso> ObtenerTodosLosPermisos()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "PERMISO_LISTAR_TODOS";
            DataTable dt = acc.Leer(sql);
            acc.Cerrar();

            List<BE_Permiso> lista = new List<BE_Permiso>();
            foreach (DataRow row in dt.Rows)
            {
                BE_Permiso permiso = new BE_Permiso();
                permiso.ID = int.Parse(row["ID_PERMISO"].ToString());
                permiso.Codigo = row["CODIGO"].ToString();
                permiso.Nombre = row["NOMBRE"].ToString();
                permiso.Descripcion = row["DESCRIPCION"].ToString();
                lista.Add(permiso);
            }
            return lista;
        }

        public void AgregarPermiso(int idRol, int idPermiso)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "ROL_AGREGAR_PERMISO";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_ROL", idRol));
            parametros.Add(acc.CrearParametro("@ID_PERMISO", idPermiso));
            acc.Escribir(sql, parametros);
            acc.Cerrar();
        }

        public void QuitarPermiso(int idRol, int idPermiso)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "ROL_QUITAR_PERMISO";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_ROL", idRol));
            parametros.Add(acc.CrearParametro("@ID_PERMISO", idPermiso));
            acc.Escribir(sql, parametros);
            acc.Cerrar();
        }

        public BE_Rol ObtenerPorID(int idRol)
        {
            return ObtenerPorId(idRol);
        }

        private BE_Rol MapearDatos(DataRow row)
        {
            BE_Rol rol = new BE_Rol();
            rol.ID = int.Parse(row["ID_ROL"].ToString());
            rol.Nombre = row["NOMBRE"].ToString();
            rol.Descripcion = row["DESCRIPCION"].ToString();
            return rol;
        }
    }
}
