using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ACCESO
    {
        SqlConnection conn;

        private static string ObtenerConnectionString()
        {
            var cs = ConfigurationManager.ConnectionStrings["Proyecto"];
            if (cs == null || string.IsNullOrWhiteSpace(cs.ConnectionString))
                throw new ConfigurationErrorsException(
                    "No se encontró la connection string 'Proyecto' en App.config. " +
                    "Agregue <connectionStrings><add name=\"Proyecto\" connectionString=\"...\" /></connectionStrings>.");
            return cs.ConnectionString;
        }

        public void Abrir()
        {
            conn = new SqlConnection(ObtenerConnectionString());
            conn.Open();
        }

        public void Cerrar()
        {
            conn.Close();
            conn = null;
            GC.Collect();
        }

        public int Escribir(string sql, List<SqlParameter> parametros = null)
        {
            try
            {
                int filas;
                SqlCommand cmd = CrearComando(sql, parametros);
                filas = cmd.ExecuteNonQuery();
                return filas;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public int Verificar(string sql, List<SqlParameter> parametros = null)
        {
            try
            {
                int filas;
                SqlCommand cmd = CrearComando(sql, parametros);
                filas = int.Parse(cmd.ExecuteScalar().ToString());
                return filas;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public DataTable Leer(string sql, List<SqlParameter> parametros = null)
        {
            try
            {
                DataTable tab = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = CrearComando(sql, parametros);
                da.SelectCommand = cmd;
                da.Fill(tab);
                return tab;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public SqlCommand CrearComando(string sql, List<SqlParameter> parametros)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.StoredProcedure;

            if (parametros != null)
            {
                cmd.Parameters.AddRange(parametros.ToArray());
            }

            return cmd;
        }

        public SqlParameter CrearParametro(string key, string value)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = key;
            param.Value = value;
            param.DbType = DbType.String;
            return param;
        }

        public SqlParameter CrearParametro(string key, int value)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = key;
            param.Value = value;
            param.DbType = DbType.Int32;
            return param;
        }

        public SqlParameter CrearParametro(string key, DateTime value)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = key;
            param.Value = value;
            param.SqlDbType = SqlDbType.DateTime;
            return param;
        }

        public SqlParameter CrearParametro(string key, bool value)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = key;
            param.Value = value;
            param.DbType = DbType.Boolean;
            return param;
        }

        public SqlConnection GetConexion()
        {
            return conn;
        }
    }
}
