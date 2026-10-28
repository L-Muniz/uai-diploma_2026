using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.ServiceProcess;

namespace Instalador
{
    // Error esperado de la instalación: el mensaje se muestra tal cual al usuario.
    public class ErrorInstalacion : Exception
    {
        public ErrorInstalacion(string mensaje) : base(mensaje) { }
    }

    // Instalación completa del sistema: verifica SQL Server, crea la base, ejecuta los scripts
    // y copia la aplicación configurada para usar esa base.
    public static class InstaladorLogica
    {
        private const string NombreBaseOriginal = "Proyecto";

        // Sentencias del script original que no deben ejecutarse en una instalación nueva
        // (crean o configuran la base con rutas de otra computadora).
        private static readonly string[] SentenciasOmitidas =
        {
            @"ALTER\s+DATABASE", @"CREATE\s+DATABASE", @"(?m)^\s*USE\s+\[", @"FULLTEXT", @"sp_fulltext",
            @"ALTER\s+AUTHORIZATION", @"CREATE\s+USER", @"CREATE\s+LOGIN", @"ALTER\s+LOGIN"
        };

        public static string[] ObtenerInstancias()
        {
            DataTable tabla = SqlDataSourceEnumerator.Instance.GetDataSources();
            var instancias = new List<string>();
            foreach (DataRow fila in tabla.Rows)
            {
                string servidor = fila["ServerName"].ToString();
                string instancia = fila["InstanceName"] == DBNull.Value ? "" : fila["InstanceName"].ToString();
                instancias.Add(string.IsNullOrEmpty(instancia) ? servidor : servidor + "\\" + instancia);
            }
            return instancias.Distinct().OrderBy(x => x).ToArray();
        }

        public static void Instalar(string servidor, string nombreBase, string carpetaDestino,
                                    string carpetaOrigen, string carpetaScripts, Action<string> informar)
        {
            ValidarNombreBase(nombreBase);
            if (string.IsNullOrWhiteSpace(servidor)) throw new ErrorInstalacion("Indique la instancia de SQL Server.");
            if (string.IsNullOrWhiteSpace(carpetaDestino)) throw new ErrorInstalacion("Indique la carpeta donde se instala la aplicación.");
            if (!Directory.Exists(carpetaOrigen))
                throw new ErrorInstalacion("No se encontró la aplicación compilada en '" + carpetaOrigen + "'. Compilá la solución completa antes de instalar.");

            informar("1/6 Verificando el motor de SQL Server...");
            VerificarMotorYServicio(servidor);

            informar("2/6 Conectando a la instancia " + servidor + "...");
            ProbarConexion(servidor);

            informar("3/6 Creando la base de datos '" + nombreBase + "' (si no existe)...");
            CrearBaseSiNoExiste(servidor, nombreBase);

            string cadenaBase = CadenaBase(servidor, nombreBase).ConnectionString;

            informar("4/6 Creando tablas, procedimientos y datos base...");
            string scriptBase = Path.Combine(carpetaScripts, "BD_SCRIPT_OCT.sql");
            if (!ExisteTabla(cadenaBase, "USUARIO"))
                EjecutarScript(cadenaBase, scriptBase, nombreBase, informar);
            else
                informar("   La base ya tiene tablas: se omite el script original.");

            informar("5/6 Ejecutando los scripts de la 3ra entrega...");
            string carpetaEntrega = Path.Combine(carpetaScripts, "Entrega3");
            foreach (string script in new[]
            {
                "01_TABLAS_ENTREGA3.sql", "02_STORED_PROCEDURES_ENTREGA3.sql",
                "03_PERFIL_SUSCRIPCIONES_ESTADISTICAS.sql", "04_AMPLIAR_BITACORA.sql", "05_DATOS_DE_PRUEBA.sql"
            })
            {
                informar("   " + script);
                EjecutarScript(cadenaBase, Path.Combine(carpetaEntrega, script), nombreBase, informar);
            }

            informar("6/6 Copiando la aplicación y configurando la conexión...");
            CopiarAplicacion(carpetaOrigen, carpetaDestino);
            ConfigurarConexion(carpetaDestino, CadenaBase(servidor, nombreBase).ConnectionString);
            informar("Instalación completa. La aplicación está en: " + carpetaDestino);
        }

        // ---------------------------------------------------------------- verificaciones

        // Caso: el motor no está instalado o el servicio está detenido.
        public static void VerificarMotorYServicio(string servidor)
        {
            string nombreServicio = NombreServicio(servidor);
            ServiceController servicio = ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName.Equals(nombreServicio, StringComparison.OrdinalIgnoreCase));

            if (servicio == null)
                throw new ErrorInstalacion(
                    "No se encontró el motor de SQL Server '" + nombreServicio + "' en esta computadora.\n\n" +
                    "Instalá SQL Server Express (https://www.microsoft.com/sql-server/sql-server-downloads) " +
                    "o revisá el nombre de la instancia, y volvé a intentar.");

            if (servicio.Status == ServiceControllerStatus.Running)
                return;

            try
            {
                servicio.Start();
                servicio.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
            }
            catch (Exception ex)
            {
                throw new ErrorInstalacion(
                    "El servicio '" + servicio.DisplayName + "' está detenido y no se pudo iniciar automáticamente.\n\n" +
                    "Iniciá el servicio desde 'Servicios' de Windows o ejecutá el instalador como administrador.\n\n" +
                    "Detalle: " + ex.Message);
            }
        }

        // Caso: la instancia elegida no responde (nombre incorrecto o red).
        public static void ProbarConexion(string servidor)
        {
            try
            {
                using (var cn = new SqlConnection(CadenaBase(servidor, "master").ConnectionString))
                {
                    cn.Open();
                }
            }
            catch (Exception ex)
            {
                throw new ErrorInstalacion(
                    "No se pudo conectar a la instancia '" + servidor + "'.\n\n" +
                    "Verificá el nombre (elegilo de la lista) y que SQL Server esté en ejecución.\n\n" +
                    "Detalle: " + ex.Message);
            }
        }

        public static bool ExisteTabla(string cadenaBase, string tabla)
        {
            using (var cn = new SqlConnection(cadenaBase))
            using (var cmd = new SqlCommand("SELECT CASE WHEN OBJECT_ID(@t, 'U') IS NULL THEN 0 ELSE 1 END", cn))
            {
                cmd.Parameters.AddWithValue("@t", "dbo." + tabla);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        // ---------------------------------------------------------------- base y scripts

        public static void CrearBaseSiNoExiste(string servidor, string nombreBase)
        {
            using (var cn = new SqlConnection(CadenaBase(servidor, "master").ConnectionString))
            using (var cmd = new SqlCommand(
                "IF DB_ID(@n) IS NULL BEGIN DECLARE @sql NVARCHAR(300) = N'CREATE DATABASE ' + QUOTENAME(@n); EXEC(@sql); END", cn))
            {
                cmd.Parameters.AddWithValue("@n", nombreBase);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Ejecuta un script separando los lotes por GO y omitiendo las sentencias de creación de la base.
        public static void EjecutarScript(string cadenaBase, string ruta, string nombreBase, Action<string> informar)
        {
            if (!File.Exists(ruta))
                throw new ErrorInstalacion("Falta el script '" + ruta + "'. Volvé a compilar el instalador.");

            string texto = File.ReadAllText(ruta).Replace("[" + NombreBaseOriginal + "]", "[" + nombreBase + "]");
            string[] lotes = Regex.Split(texto, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (var cn = new SqlConnection(cadenaBase))
            {
                cn.Open();
                foreach (string lote in lotes)
                {
                    if (string.IsNullOrWhiteSpace(lote) || Omitir(lote))
                        continue;

                    try
                    {
                        using (var cmd = new SqlCommand(lote, cn) { CommandTimeout = 300 })
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ErrorInstalacion(
                            "Error al ejecutar '" + Path.GetFileName(ruta) + "':\n\n" + ex.Message +
                            "\n\nSentencia: " + Resumir(lote));
                    }
                }
            }
        }

        private static bool Omitir(string lote)
        {
            return SentenciasOmitidas.Any(patron => Regex.IsMatch(lote, patron, RegexOptions.IgnoreCase));
        }

        private static string Resumir(string lote)
        {
            string limpio = Regex.Replace(lote.Trim(), @"\s+", " ");
            return limpio.Length > 200 ? limpio.Substring(0, 200) + "..." : limpio;
        }

        // ---------------------------------------------------------------- aplicación y configuración

        public static void CopiarAplicacion(string origen, string destino)
        {
            Directory.CreateDirectory(destino);
            foreach (string archivo in Directory.GetFiles(origen, "*.*", SearchOption.AllDirectories))
            {
                string relativo = archivo.Substring(origen.Length).TrimStart('\\');
                string destinoArchivo = Path.Combine(destino, relativo);
                Directory.CreateDirectory(Path.GetDirectoryName(destinoArchivo));
                File.Copy(archivo, destinoArchivo, true);
            }
        }

        // Reemplaza la cadena de conexión "Proyecto" del App.config instalado por la de la instalación.
        public static void ConfigurarConexion(string carpetaDestino, string cadena)
        {
            string config = Path.Combine(carpetaDestino, "GUI.exe.config");
            if (!File.Exists(config))
                throw new ErrorInstalacion("No se encontró GUI.exe.config en la carpeta de instalación.");

            string xml = File.ReadAllText(config);
            string nuevo = Regex.Replace(xml,
                "(<add\\s+name=\"Proyecto\"\\s+connectionString=\")[^\"]*(\")",
                m => m.Groups[1].Value + cadena.Replace("&", "&amp;") + m.Groups[2].Value,
                RegexOptions.Singleline);
            File.WriteAllText(config, nuevo);
        }

        // ---------------------------------------------------------------- utilidades

        public static SqlConnectionStringBuilder CadenaBase(string servidor, string nombreBase)
        {
            return new SqlConnectionStringBuilder
            {
                DataSource = servidor,
                InitialCatalog = nombreBase,
                IntegratedSecurity = true,
                ConnectTimeout = 10
            };
        }

        // Nombre del servicio de Windows: MSSQLSERVER para la instancia por defecto, MSSQL$NOMBRE para una nombrada.
        public static string NombreServicio(string servidor)
        {
            int barra = servidor.IndexOf('\\');
            if (barra < 0)
                return "MSSQLSERVER";
            string instancia = servidor.Substring(barra + 1);
            return instancia.Equals("MSSQLSERVER", StringComparison.OrdinalIgnoreCase) ? "MSSQLSERVER" : "MSSQL$" + instancia;
        }

        public static void ValidarNombreBase(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre) || !Regex.IsMatch(nombre, @"^[A-Za-z][A-Za-z0-9_]{0,99}$"))
                throw new ErrorInstalacion("El nombre de la base debe empezar con letra y tener solo letras, números o guion bajo.");
        }
    }
}
