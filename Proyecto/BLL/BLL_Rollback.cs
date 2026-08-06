using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE;
using DAL;
using SERVICE;

namespace BLL
{
    public class BLL_Rollback
    {
        public static (bool exito, string mensaje) GuardarSnapshot(string nombre, string descripcion, string contenido = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(contenido))
                    return (false, "El contenido del snapshot no puede ser nulo o vacío");

                string contenidoEncriptado = SV_Encriptacion.Encriptar(contenido);

                ACCESO acc = new ACCESO();
                acc.Abrir();

                string sql = "INSERT INTO SNAPSHOTS (NOMBRE, DESCRIPCION, CONTENIDO, FECHA_CREACION) VALUES (@nombre, @desc, @contenido, @fecha)";
                SqlCommand cmd = new SqlCommand(sql, acc.GetConexion());
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@desc", descripcion ?? "");
                cmd.Parameters.AddWithValue("@contenido", contenidoEncriptado);
                cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                cmd.ExecuteNonQuery();

                acc.Cerrar();
                return (true, "Snapshot guardado (contenido encriptado AES)");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public static (bool exito, string mensaje) RevertirASnapshot(int idSnapshot)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();

                string sqlSelect = "SELECT CONTENIDO FROM SNAPSHOTS WHERE ID_SNAPSHOT = @id";
                SqlCommand cmdSelect = new SqlCommand(sqlSelect, acc.GetConexion());
                cmdSelect.Parameters.AddWithValue("@id", idSnapshot);
                object result = cmdSelect.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    acc.Cerrar();
                    return (false, "Snapshot sin contenido restaurable");
                }

                string contenido = SV_Encriptacion.Desencriptar(result.ToString());
                SqlCommand cmd = new SqlCommand(contenido, acc.GetConexion());
                cmd.CommandTimeout = 300;
                cmd.ExecuteNonQuery();

                acc.Cerrar();
                return (true, "Datos revertidos exitosamente");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public static List<BE_Snapshot> ListarSnapshots()
        {
            var snapshots = new List<BE_Snapshot>();
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();

                string sql = "SELECT ID_SNAPSHOT, NOMBRE, DESCRIPCION, FECHA_CREACION FROM SNAPSHOTS ORDER BY FECHA_CREACION DESC";
                SqlCommand cmd = new SqlCommand(sql, acc.GetConexion());
                DataTable dt = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    snapshots.Add(new BE_Snapshot
                    {
                        ID = Convert.ToInt32(row["ID_SNAPSHOT"]),
                        Nombre = row["NOMBRE"]?.ToString() ?? "",
                        Descripcion = row["DESCRIPCION"] == DBNull.Value ? "" : row["DESCRIPCION"].ToString(),
                        Fecha = row["FECHA_CREACION"] == DBNull.Value
                            ? DateTime.MinValue
                            : Convert.ToDateTime(row["FECHA_CREACION"])
                    });
                }

                acc.Cerrar();
            }
            catch { }
            return snapshots;
        }

        public static (bool exito, string mensaje) EliminarSnapshot(int idSnapshot)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();

                string sql = "DELETE FROM SNAPSHOTS WHERE ID_SNAPSHOT = @id";
                SqlCommand cmd = new SqlCommand(sql, acc.GetConexion());
                cmd.Parameters.AddWithValue("@id", idSnapshot);
                cmd.ExecuteNonQuery();

                acc.Cerrar();
                return (true, "Snapshot eliminado");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public static (bool exito, string mensaje) GuardarSnapshotUsuario(BE_USUARIO usuario, string descripcion)
        {
            try
            {
                if (usuario == null)
                    return (false, "Usuario nulo");

                MP_USUARIO mp = new MP_USUARIO();
                var actual = mp.BuscarPorId(usuario.ID) ?? usuario;

                string bloqueadoSql = actual.Bloqueado ? "1" : "0";
                string contenido = $"UPDATE USUARIO SET USERNAME='{actual.Username.Replace("'", "''")}', BLOQUEADO={bloqueadoSql}, PASS='{actual.HashedPassword.Replace("'", "''")}', DVH_HORIZONTAL={actual.DVHorizontal} WHERE ID_USUARIO={actual.ID};";
                string nombre = $"Usuario: {actual.Username}";

                return GuardarSnapshot(nombre, descripcion, contenido);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
