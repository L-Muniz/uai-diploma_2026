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
    public class MP_DigitoVerificadorVertical : MAPPER<BE_DigitoVerificadorVertical>
    {
        public override int Alta(BE_DigitoVerificadorVertical obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "DVV_ALTA";
            List<SqlParameter> parametros = new List<SqlParameter>();

            SqlParameter p1 = new SqlParameter("@p1", System.Data.SqlDbType.Int);
            p1.Value = DBNull.Value;
            parametros.Add(p1);

            parametros.Add(acc.CrearParametro("@p2", obj.TipoEntidad));
            parametros.Add(acc.CrearParametro("@p3", obj.DVUsername));
            parametros.Add(acc.CrearParametro("@p4", obj.DVHashedPassword));
            parametros.Add(acc.CrearParametro("@p5", obj.DVBloqueado));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override int Borrar(BE_DigitoVerificadorVertical obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "DVV_BORRAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@id", obj.ID));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override int Editar(BE_DigitoVerificadorVertical obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "DVV_EDITAR";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@id", obj.ID));
            parametros.Add(acc.CrearParametro("@dvUsername", obj.DVUsername));
            parametros.Add(acc.CrearParametro("@dvPassword", obj.DVHashedPassword));
            parametros.Add(acc.CrearParametro("@dvBloqueado", obj.DVBloqueado));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override List<BE_DigitoVerificadorVertical> Listar()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "DVV_LISTAR";
            DataTable dt = acc.Leer(sql);
            acc.Cerrar();

            List<BE_DigitoVerificadorVertical> lista = new List<BE_DigitoVerificadorVertical>();
            foreach (DataRow row in dt.Rows)
            {
                BE_DigitoVerificadorVertical dvv = new BE_DigitoVerificadorVertical();
                dvv.ID = int.Parse(row["ID_DVV"].ToString());
                if (row["ID_ENTIDAD"] != DBNull.Value)
                    dvv.IdEntidad = int.Parse(row["ID_ENTIDAD"].ToString());
                dvv.TipoEntidad = row["TIPO_ENTIDAD"].ToString();
                dvv.DVUsername = int.Parse(row["DV_USERNAME"].ToString());
                dvv.DVHashedPassword = int.Parse(row["DV_PASSWORD"].ToString());
                dvv.DVBloqueado = int.Parse(row["DV_BLOQUEADO"].ToString());
                dvv.FechaCreacion = DateTime.Parse(row["FECHA_CREACION"].ToString());
                lista.Add(dvv);
            }

            return lista;
        }

        public BE_DigitoVerificadorVertical BuscarPorTipoEntidad(string tipoEntidad)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "DVV_BUSCAR_X_TIPO";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@tipoEntidad", tipoEntidad));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            BE_DigitoVerificadorVertical dvv = null;
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                dvv = new BE_DigitoVerificadorVertical();
                dvv.ID = int.Parse(row["ID_DVV"].ToString());
                if (row["ID_ENTIDAD"] != DBNull.Value)
                    dvv.IdEntidad = int.Parse(row["ID_ENTIDAD"].ToString());
                dvv.TipoEntidad = row["TIPO_ENTIDAD"].ToString();
                dvv.DVUsername = int.Parse(row["DV_USERNAME"].ToString());
                dvv.DVHashedPassword = int.Parse(row["DV_PASSWORD"].ToString());
                dvv.DVBloqueado = int.Parse(row["DV_BLOQUEADO"].ToString());
                dvv.FechaCreacion = DateTime.Parse(row["FECHA_CREACION"].ToString());
            }

            return dvv;
        }

        public BE_DigitoVerificadorVertical BuscarPorIdEntidad(int idEntidad, string tipoEntidad)
        {
            return BuscarPorTipoEntidad(tipoEntidad);
        }
    }
}
