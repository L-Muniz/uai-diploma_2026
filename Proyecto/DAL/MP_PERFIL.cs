using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class MP_PERFIL : MAPPER<BE_Perfil>
    {
        public override int Alta(BE_Perfil obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "PERFIL_CREAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@NOMBRE", obj.Nombre));
            parametros.Add(acc.CrearParametro("@DESCRIPCION", obj.Descripcion ?? ""));
            parametros.Add(acc.CrearParametro("@ID_PERFIL_PADRE", obj.IdPadrePerfil ?? 0));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override int Borrar(BE_Perfil obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "PERFIL_ELIMINAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_PERFIL", obj.ID));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override int Editar(BE_Perfil obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "PERFIL_ACTUALIZAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_PERFIL", obj.ID));
            parametros.Add(acc.CrearParametro("@NOMBRE", obj.Nombre));
            parametros.Add(acc.CrearParametro("@DESCRIPCION", obj.Descripcion ?? ""));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override List<BE_Perfil> Listar()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "PERFIL_LISTAR_PRINCIPALES";
            DataTable dt = acc.Leer(sql);
            acc.Cerrar();

            List<BE_Perfil> lista = new List<BE_Perfil>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearDatos(row));
            }
            return lista;
        }

        public List<BE_Perfil> ObtenerPerfilesHijos(int idPerfilPadre)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "PERFIL_LISTAR_HIJOS";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_PERFIL_PADRE", idPerfilPadre));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            List<BE_Perfil> lista = new List<BE_Perfil>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearDatos(row));
            }
            return lista;
        }

        public List<BE_Permiso> ObtenerPermisos(int idPerfil)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "PERFIL_OBTENER_PERMISOS";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_PERFIL", idPerfil));
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

        public void AgregarPermiso(int idPerfil, int idPermiso)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "PERFIL_AGREGAR_PERMISO";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_PERFIL", idPerfil));
            parametros.Add(acc.CrearParametro("@ID_PERMISO", idPermiso));
            acc.Escribir(sql, parametros);
            acc.Cerrar();
        }

        public void QuitarPermiso(int idPerfil, int idPermiso)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "PERFIL_QUITAR_PERMISO";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_PERFIL", idPerfil));
            parametros.Add(acc.CrearParametro("@ID_PERMISO", idPermiso));
            acc.Escribir(sql, parametros);
            acc.Cerrar();
        }

        private BE_Perfil MapearDatos(DataRow row)
        {
            BE_Perfil perfil = new BE_Perfil();
            perfil.ID = int.Parse(row["ID_PERFIL"].ToString());
            perfil.Nombre = row["NOMBRE"].ToString();
            perfil.Descripcion = row["DESCRIPCION"].ToString();

            if (row["ID_PERFIL_PADRE"] != DBNull.Value)
                perfil.IdPadrePerfil = int.Parse(row["ID_PERFIL_PADRE"].ToString());

            return perfil;
        }
    }
}
