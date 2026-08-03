using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
	public class MP_USUARIO:MAPPER<BE_USUARIO>
    {
        public override int Alta(BE_USUARIO obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "ALTA_USUARIO";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@user", obj.Username));
            parametros.Add(acc.CrearParametro("@pass", obj.HashedPassword));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override int Borrar(BE_USUARIO obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "USUARIO_ELIMINAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_USUARIO", obj.ID));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public BE_USUARIO BuscarPorUserName(string username)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "USUARIO_BUSCAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@user", username));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            BE_USUARIO usuario = null;
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                usuario = new BE_USUARIO();
                usuario.ID = int.Parse(row["ID_USUARIO"].ToString());
                usuario.Username = row["USERNAME"].ToString();
                usuario.HashedPassword = row["PASS"].ToString();
                usuario.Bloqueado = row.Table.Columns.Contains("BLOQUEADO") && row["BLOQUEADO"] != DBNull.Value ? bool.Parse(row["BLOQUEADO"].ToString()) : false;
                usuario.DVHorizontal = row.Table.Columns.Contains("DVH_HORIZONTAL") && row["DVH_HORIZONTAL"] != DBNull.Value ? int.Parse(row["DVH_HORIZONTAL"].ToString()) : 0;
            }

            return usuario;
        }

        public BE_USUARIO BuscarPorId(int id)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "USUARIO_BUSCAR_X_ID";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@id", id));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            BE_USUARIO usuario = null;
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                usuario = new BE_USUARIO();
                usuario.ID = int.Parse(row["ID_USUARIO"].ToString());
                usuario.Username = row["USERNAME"].ToString();
                usuario.HashedPassword = row["PASS"].ToString();
                usuario.Bloqueado = row.Table.Columns.Contains("BLOQUEADO") && row["BLOQUEADO"] != DBNull.Value ? bool.Parse(row["BLOQUEADO"].ToString()) : false;
                usuario.DVHorizontal = row.Table.Columns.Contains("DVH_HORIZONTAL") && row["DVH_HORIZONTAL"] != DBNull.Value ? int.Parse(row["DVH_HORIZONTAL"].ToString()) : 0;
            }

            return usuario;
        }

        public override int Editar(BE_USUARIO obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "USUARIO_EDITAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_USUARIO", obj.ID));
            parametros.Add(acc.CrearParametro("@USERNAME", obj.Username));
            parametros.Add(acc.CrearParametro("@PASS", obj.HashedPassword));
            parametros.Add(acc.CrearParametro("@BLOQUEADO", obj.Bloqueado));
            parametros.Add(acc.CrearParametro("@DVH_HORIZONTAL", obj.DVHorizontal));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public int AsignarRol(int idUsuario, int idRol)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "USUARIO_ASIGNAR_ROL";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_USUARIO", idUsuario));
            parametros.Add(acc.CrearParametro("@ID_ROL", idRol));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override List<BE_USUARIO> Listar()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "USUARIO_LISTAR";
            DataTable dt = acc.Leer(sql);
            acc.Cerrar();

            List<BE_USUARIO> lista = new List<BE_USUARIO>();
            foreach (DataRow row in dt.Rows)
            {
                BE_USUARIO usuario = new BE_USUARIO();
                usuario.ID = int.Parse(row["ID_USUARIO"].ToString());
                usuario.Username = row["USERNAME"].ToString();
                usuario.HashedPassword = row["PASS"].ToString();
                usuario.Bloqueado = row.Table.Columns.Contains("BLOQUEADO") && row["BLOQUEADO"] != DBNull.Value ? bool.Parse(row["BLOQUEADO"].ToString()) : false;
                usuario.DVHorizontal = row.Table.Columns.Contains("DVH_HORIZONTAL") && row["DVH_HORIZONTAL"] != DBNull.Value ? int.Parse(row["DVH_HORIZONTAL"].ToString()) : 0;
                lista.Add(usuario);
            }

            return lista;
        }

        public int BloquearUsuario(BE_USUARIO usuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "USUARIO_BLOQUEAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_USUARIO", usuario.ID));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public int DesbloquearUsuario(BE_USUARIO usuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "USUARIO_DESBLOQUEAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_USUARIO", usuario.ID));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public int ActualizarDVHorizontal(BE_USUARIO usuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "USUARIO_ACTUALIZAR_DVH";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@p1", usuario.ID));
            parametros.Add(acc.CrearParametro("@p2", usuario.DVHorizontal));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public string ObtenerRolUsuario(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "USUARIO_OBTENER_ROL";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_USUARIO", idUsuario));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            if (dt.Rows.Count > 0)
                return dt.Rows[0]["NOMBRE"].ToString();

            return "USUARIO";
        }
    }
}