using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using DAL;
using SERVICE;

namespace BLL
{
    public class BLL_BackupRestore
    {
        private static string GetRutaBackup()
        {
            string appBasePath = AppDomain.CurrentDomain.BaseDirectory;
            string folder = ConfigurationManager.AppSettings["BackupFolder"];
            if (string.IsNullOrWhiteSpace(folder))
                folder = "Backups";
            if (Path.IsPathRooted(folder))
                return folder;
            return Path.Combine(appBasePath, folder);
        }

        private static string rutaBackup = GetRutaBackup();

        static BLL_BackupRestore()
        {
            if (!Directory.Exists(rutaBackup))
            {
                try
                {
                    Directory.CreateDirectory(rutaBackup);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"No se pudo crear carpeta de backups: {ex.Message}");
                }
            }
        }

        public static (bool exito, string mensaje) RealizarBackup()
        {
            try
            {
                string nombreArchivo = $"BD_BACKUP_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.sql";
                string rutaCompleta = Path.Combine(rutaBackup, nombreArchivo);

                ACCESO acc = new ACCESO();
                acc.Abrir();

                StringBuilder scriptSQL = new StringBuilder();
                scriptSQL.AppendLine("-- Backup de Base de Datos Proyecto");
                scriptSQL.AppendLine($"-- Generado: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                scriptSQL.AppendLine();

                SqlConnection conn = acc.GetConexion();

                string queryTablas = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME";
                SqlCommand cmdTablas = new SqlCommand(queryTablas, conn);
                DataTable tablas = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmdTablas);
                adapter.Fill(tablas);

                foreach (DataRow tablaRow in tablas.Rows)
                {
                    string nombreTabla = tablaRow["TABLE_NAME"].ToString();
                    scriptSQL.AppendLine($"ALTER TABLE [{nombreTabla}] NOCHECK CONSTRAINT ALL;");
                }
                scriptSQL.AppendLine();

                foreach (DataRow tablaRow in tablas.Rows)
                {
                    string nombreTabla = tablaRow["TABLE_NAME"].ToString();

                    // No borrar TRADUCCION para preservar las traducciones cargadas
                    if (nombreTabla == "TRADUCCION")
                        continue;

                    scriptSQL.AppendLine($"-- Tabla: [{nombreTabla}]");
                    scriptSQL.AppendLine($"DELETE FROM [{nombreTabla}];");

                    // Verificar si la tabla tiene una columna de identidad
                    string queryIdentidad = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{nombreTabla}' AND COLUMNPROPERTY(OBJECT_ID(TABLE_SCHEMA+'.'+TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1";
                    SqlCommand cmdIdentidad = new SqlCommand(queryIdentidad, conn);
                    object columnaIdentidad = cmdIdentidad.ExecuteScalar();

                    bool tieneIdentidad = columnaIdentidad != null;
                    if (tieneIdentidad)
                        scriptSQL.AppendLine($"SET IDENTITY_INSERT [{nombreTabla}] ON;");

                    string queryData = $"SELECT * FROM [{nombreTabla}]";
                    SqlCommand cmdData = new SqlCommand(queryData, conn);
                    DataTable datos = new DataTable();
                    SqlDataAdapter adapterData = new SqlDataAdapter(cmdData);
                    adapterData.Fill(datos);

                    if (datos.Rows.Count > 0)
                    {
                        foreach (DataRow row in datos.Rows)
                        {
                            List<string> columnas = new List<string>();
                            List<string> valores = new List<string>();

                            foreach (DataColumn col in datos.Columns)
                            {
                                columnas.Add($"[{col.ColumnName}]");
                                object valor = row[col.ColumnName];
                                if (valor == DBNull.Value)
                                    valores.Add("NULL");
                                else if (col.DataType == typeof(string))
                                    valores.Add($"N'{valor.ToString().Replace("'", "''")}'");
                                else if (col.DataType == typeof(bool))
                                    valores.Add(((bool)valor) ? "1" : "0");
                                else if (col.DataType == typeof(DateTime))
                                    valores.Add($"'{((DateTime)valor):yyyy-MM-dd HH:mm:ss.fff}'");
                                else
                                    valores.Add(valor.ToString());
                            }

                            scriptSQL.AppendLine($"INSERT INTO [{nombreTabla}] ({string.Join(", ", columnas)}) VALUES ({string.Join(", ", valores)});");
                        }
                    }

                    if (tieneIdentidad)
                        scriptSQL.AppendLine($"SET IDENTITY_INSERT [{nombreTabla}] OFF;");

                    scriptSQL.AppendLine();
                }

                foreach (DataRow tablaRow in tablas.Rows)
                {
                    string nombreTabla = tablaRow["TABLE_NAME"].ToString();
                    scriptSQL.AppendLine($"ALTER TABLE [{nombreTabla}] WITH CHECK CHECK CONSTRAINT ALL;");
                }

                acc.Cerrar();

                string contenidoPlano = scriptSQL.ToString();
                File.WriteAllText(rutaCompleta, SV_Encriptacion.Encriptar(contenidoPlano), Encoding.UTF8);

                return (true, $"Backup realizado (encriptado AES): {nombreArchivo}");
            }
            catch (Exception ex)
            {
                return (false, $"Error en backup: {ex.Message}");
            }
        }

        public static string ObtenerUltimoBackup()
        {
            try
            {
                var archivos = Directory.GetFiles(rutaBackup, "BD_BACKUP_*.sql")
                    .OrderByDescending(f => File.GetCreationTime(f))
                    .FirstOrDefault();

                return archivos;
            }
            catch
            {
                return null;
            }
        }

        public static List<string> ListarBackups()
        {
            try
            {
                return Directory.GetFiles(rutaBackup, "BD_BACKUP_*.sql")
                    .OrderByDescending(f => File.GetCreationTime(f))
                    .ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        public static (bool exito, string mensaje) RestaurarDesdeBackup(string rutaArchivo)
        {
            try
            {
                if (!File.Exists(rutaArchivo))
                    return (false, "Archivo de backup no encontrado");

                ACCESO acc = new ACCESO();
                acc.Abrir();

                string scriptSQL = SV_Encriptacion.LeerArchivoDesencriptado(rutaArchivo);

                SqlConnection conn = acc.GetConexion();
                SqlTransaction tx = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = tx;
                cmd.CommandTimeout = 300;

                string sentenciaActual = "";
                int lineasEjecutadas = 0;

                try
                {
                    foreach (string linea in scriptSQL.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
                    {
                        if (string.IsNullOrWhiteSpace(linea) || linea.Trim().StartsWith("--"))
                            continue;

                        sentenciaActual += " " + linea;

                        if (linea.Trim().EndsWith(";"))
                        {
                            cmd.CommandText = sentenciaActual.Trim();
                            cmd.ExecuteNonQuery();
                            lineasEjecutadas++;
                            sentenciaActual = "";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(sentenciaActual))
                    {
                        cmd.CommandText = sentenciaActual.Trim();
                        cmd.ExecuteNonQuery();
                        lineasEjecutadas++;
                    }

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    try { tx.Rollback(); } catch { }
                    acc.Cerrar();
                    return (false, $"Error en restore (se revirtió la transacción): {ex.Message}");
                }

                acc.Cerrar();
                return (true, $"Base de datos restaurada. Sentencias ejecutadas: {lineasEjecutadas}");
            }
            catch (Exception ex)
            {
                return (false, $"Error en restore: {ex.Message}");
            }
        }

        public static (bool exito, string mensaje) RestaurarUltimoBackup()
        {
            string ultimoBackup = ObtenerUltimoBackup();
            if (ultimoBackup == null)
                return (false, "No hay backups disponibles");

            return RestaurarDesdeBackup(ultimoBackup);
        }

        public static Dictionary<string, object> ObtenerInfoBackups()
        {
            var info = new Dictionary<string, object>();
            var archivos = ListarBackups();

            info["TotalBackups"] = archivos.Count;
            info["RutaBackups"] = rutaBackup;
            info["UltimoBackup"] = ObtenerUltimoBackup();

            if (archivos.Count > 0)
            {
                var ultimoArchivo = archivos.First();
                var fileInfo = new FileInfo(ultimoArchivo);
                info["FechaUltimoBackup"] = fileInfo.CreationTime;
                info["TamañoMB"] = Math.Round(fileInfo.Length / (1024.0 * 1024.0), 2);
            }

            return info;
        }

        public static void ConfigurarBackupAutomatico(int intervalMinutos)
        {
            var timer = new System.Timers.Timer(intervalMinutos * 60 * 1000);
            timer.Elapsed += (s, e) =>
            {
                var resultado = RealizarBackup();
                System.Diagnostics.Debug.WriteLine($"[Backup Automatico] {resultado.mensaje}");
            };
            timer.Start();
        }
    }
}
