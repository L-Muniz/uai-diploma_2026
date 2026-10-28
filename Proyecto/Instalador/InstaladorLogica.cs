using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text.RegularExpressions;

namespace Instalador
{
    // Error esperado de la instalación: el mensaje se muestra tal cual al usuario.
    public class ErrorInstalacion : Exception
    {
        public ErrorInstalacion(string mensaje) : base(mensaje) { }
    }

    // No hay motor de SQL Server instalado (o no hay instancias): se ofrece abrir la descarga oficial.
    public class ErrorMotorNoInstalado : ErrorInstalacion
    {
        public ErrorMotorNoInstalado(string mensaje) : base(mensaje) { }
    }

    // Instalación completa en un solo paso: la aplicación y los scripts van incrustados en el .exe.
    public static class InstaladorLogica
    {
        public const string UrlSqlExpress = "https://www.microsoft.com/sql-server/sql-server-downloads";
        // La aplicación y los scripts usan siempre este nombre de base.
        public const string NombreBase = "Proyecto";
        private const string NombreBaseOriginal = "Proyecto";

        // Sentencias del script original que no deben ejecutarse en una instalación nueva
        // (crean o configuran la base con rutas de otra computadora).
        private static readonly string[] SentenciasOmitidas =
        {
            @"ALTER\s+DATABASE", @"CREATE\s+DATABASE", @"(?m)^\s*USE\s+\[", @"FULLTEXT", @"sp_fulltext",
            @"ALTER\s+AUTHORIZATION", @"CREATE\s+USER", @"CREATE\s+LOGIN", @"ALTER\s+LOGIN"
        };

        // Busca instancias de SQL Server por tres vías: el enumerador de ADO.NET (requiere SQL Browser),
        // el registro de Windows y los servicios MSSQL$. Así se detectan aunque SQL Browser esté apagado.
        public static string[] ObtenerInstancias()
        {
            var instancias = new List<string>();

            try
            {
                DataTable tabla = SqlDataSourceEnumerator.Instance.GetDataSources();
                foreach (DataRow fila in tabla.Rows)
                {
                    string servidor = fila["ServerName"].ToString();
                    string instancia = fila["InstanceName"] == DBNull.Value ? "" : fila["InstanceName"].ToString();
                    instancias.Add(string.IsNullOrEmpty(instancia) ? servidor : servidor + "\\" + instancia);
                }
            }
            catch (Exception)
            {
                // Si el enumerador falla, se sigue con las otras fuentes.
            }

            try
            {
                using (var clave = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL"))
                {
                    if (clave != null)
                    {
                        foreach (string nombre in clave.GetValueNames())
                            instancias.Add(NombreDesdeInstancia(nombre));
                    }
                }
            }
            catch (Exception)
            {
                // Sin acceso al registro no se agregan instancias desde esta fuente.
            }

            try
            {
                foreach (ServiceController servicio in ServiceController.GetServices())
                {
                    if (servicio.ServiceName == "MSSQLSERVER")
                        instancias.Add("localhost");
                    else if (servicio.ServiceName.StartsWith("MSSQL$", StringComparison.OrdinalIgnoreCase))
                        instancias.Add(NombreDesdeInstancia(servicio.ServiceName.Substring(6)));
                }
            }
            catch (Exception)
            {
                // Sin acceso a los servicios no se agregan instancias desde esta fuente.
            }

            return instancias.Where(x => !string.IsNullOrWhiteSpace(x))
                             .Select(x => x.Trim())
                             .Distinct(StringComparer.OrdinalIgnoreCase)
                             .OrderBy(x => x)
                             .ToArray();
        }

        // "MSSQLSERVER" es la instancia por defecto (se usa "localhost"); el resto se escribe como localhost\NOMBRE.
        private static string NombreDesdeInstancia(string instancia)
        {
            return instancia.Equals("MSSQLSERVER", StringComparison.OrdinalIgnoreCase) ? "localhost" : "localhost\\" + instancia;
        }

        public static void Instalar(string servidor, string carpetaDestino, Action<string> informar)
        {
            const string nombreBase = NombreBase;
            if (string.IsNullOrWhiteSpace(servidor))
                throw new ErrorMotorNoInstalado("No hay una instancia de SQL Server seleccionada.\n\n" +
                                                "Si no tenés SQL Server, instalá SQL Server Express y volvé a intentar.");
            VerificarCarpetaDestino(carpetaDestino);

            informar("1/6 Verificando el motor de SQL Server...");
            VerificarMotorYServicio(servidor);

            informar("2/6 Conectando a la instancia " + servidor + "...");
            ProbarConexion(servidor);

            informar("3/6 Creando la base de datos '" + nombreBase + "' (si no existe)...");
            CrearBaseSiNoExiste(servidor, nombreBase);

            string cadenaBase = CadenaBase(servidor, nombreBase).ConnectionString;

            informar("4/6 Creando tablas, procedimientos y datos base...");
            if (!ExisteTabla(cadenaBase, "USUARIO"))
                EjecutarScript(cadenaBase, "BD_SCRIPT_OCT.sql", nombreBase, informar);
            else
                informar("   La base ya tiene tablas: se omite el script original.");

            informar("5/6 Ejecutando los scripts de la 3ra entrega...");
            foreach (string script in new[]
            {
                "01_TABLAS_ENTREGA3.sql", "02_STORED_PROCEDURES_ENTREGA3.sql",
                "03_PERFIL_SUSCRIPCIONES_ESTADISTICAS.sql", "04_AMPLIAR_BITACORA.sql", "05_DATOS_DE_PRUEBA.sql"
            })
            {
                informar("   " + script);
                EjecutarScript(cadenaBase, script, nombreBase, informar);
            }

            informar("6/6 Instalando la aplicación y configurando la conexión...");
            ExtraerAplicacion(carpetaDestino);
            ConfigurarConexion(carpetaDestino, cadenaBase);
            CrearAccesoDirecto(Path.Combine(carpetaDestino, "GUI.exe"));

            informar("Instalación completa. La aplicación está en: " + carpetaDestino);
        }

        // ---------------------------------------------------------------- verificaciones

        public static void VerificarCarpetaDestino(string carpeta)
        {
            if (string.IsNullOrWhiteSpace(carpeta))
                throw new ErrorInstalacion("Indique la carpeta donde se instala la aplicación.");
            try
            {
                Directory.CreateDirectory(carpeta);
                string prueba = Path.Combine(carpeta, ".escritura.tmp");
                File.WriteAllText(prueba, "ok");
                File.Delete(prueba);
            }
            catch (Exception ex)
            {
                throw new ErrorInstalacion(
                    "No se puede escribir en la carpeta '" + carpeta + "'.\n\n" +
                    "Elegí otra carpeta (por ejemplo, Documentos) o ejecutá el instalador como administrador.\n\nDetalle: " + ex.Message);
            }
        }

        // Caso: el motor no está instalado o el servicio está detenido.
        public static void VerificarMotorYServicio(string servidor)
        {
            string nombreServicio = NombreServicio(servidor);
            ServiceController servicio = ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName.Equals(nombreServicio, StringComparison.OrdinalIgnoreCase));

            if (servicio == null)
                throw new ErrorMotorNoInstalado(
                    "No se encontró el motor de SQL Server '" + nombreServicio + "' en esta computadora.\n\n" +
                    "Instalá SQL Server Express, o revisá el nombre de la instancia.");

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

        // Ejecuta un script incrustado separando los lotes por GO y omitiendo las sentencias de creación de la base.
        public static void EjecutarScript(string cadenaBase, string nombreRecurso, string nombreBase, Action<string> informar)
        {
            string texto = LeerRecursoTexto(nombreRecurso).Replace("[" + NombreBaseOriginal + "]", "[" + nombreBase + "]");
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
                            "Error al ejecutar '" + nombreRecurso + "':\n\n" + ex.Message +
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

        public static void ExtraerAplicacion(string carpetaDestino)
        {
            using (Stream recurso = LeerRecurso("aplicacion.zip"))
            using (var zip = new ZipArchive(recurso, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entrada in zip.Entries)
                {
                    if (string.IsNullOrEmpty(entrada.Name)) continue;
                    string destino = Path.Combine(carpetaDestino, entrada.FullName.Replace('/', '\\'));
                    Directory.CreateDirectory(Path.GetDirectoryName(destino));
                    entrada.ExtractToFile(destino, true);
                }
            }
        }

        // Reemplaza la cadena de conexión "Proyecto" del GUI.exe.config instalado por la de esta instalación.
        public static void ConfigurarConexion(string carpetaDestino, string cadena)
        {
            string config = Path.Combine(carpetaDestino, "GUI.exe.config");
            if (!File.Exists(config))
                throw new ErrorInstalacion("No se encontró GUI.exe.config en la aplicación instalada.");

            string xml = File.ReadAllText(config);
            string nuevo = Regex.Replace(xml,
                "(<add\\s+name=\"Proyecto\"\\s+connectionString=\")[^\"]*(\")",
                m => m.Groups[1].Value + cadena.Replace("&", "&amp;") + m.Groups[2].Value,
                RegexOptions.Singleline);
            File.WriteAllText(config, nuevo);
        }

        // Acceso directo en el escritorio (usa WScript.Shell; si falla, la instalación sigue igual).
        public static void CrearAccesoDirecto(string ejecutable)
        {
            try
            {
                Type tipo = Type.GetTypeFromProgID("WScript.Shell");
                object shell = Activator.CreateInstance(tipo);
                string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                object acceso = tipo.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, shell,
                                                  new object[] { Path.Combine(escritorio, "BecoGre.lnk") });
                acceso.GetType().InvokeMember("TargetPath", BindingFlags.SetProperty, null, acceso, new object[] { ejecutable });
                acceso.GetType().InvokeMember("WorkingDirectory", BindingFlags.SetProperty, null, acceso,
                                              new object[] { Path.GetDirectoryName(ejecutable) });
                acceso.GetType().InvokeMember("Save", BindingFlags.InvokeMethod, null, acceso, null);
            }
            catch (Exception)
            {
                // El acceso directo es opcional.
            }
        }

        // ---------------------------------------------------------------- recursos incrustados

        private static Stream LeerRecurso(string nombre)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(nombre);
            if (stream == null)
                throw new ErrorInstalacion("Falta el recurso '" + nombre + "' dentro del instalador. Volvé a compilarlo.");
            return stream;
        }

        private static string LeerRecursoTexto(string nombre)
        {
            using (Stream stream = LeerRecurso(nombre))
            using (var lector = new StreamReader(stream, true))
            {
                return lector.ReadToEnd();
            }
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
