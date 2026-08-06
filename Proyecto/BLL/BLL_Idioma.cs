using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE;
using DAL;

namespace BLL
{
    public class BLL_Idioma
    {
        public List<BE_Idioma> ObtenerIdiomas()
        {
            var idiomas = new List<BE_Idioma>();

            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                DataTable dt = acc.Leer("SELECT_IDIOMA_TODOS");
                acc.Cerrar();

                foreach (DataRow row in dt.Rows)
                {
                    string codigo = row["CODIGO"]?.ToString()?.Trim() ?? "";
                    string nombre = dt.Columns.Contains("NOMBRE")
                        ? (row["NOMBRE"]?.ToString()?.Trim() ?? codigo)
                        : codigo;

                    if (string.IsNullOrWhiteSpace(codigo))
                        continue;

                    idiomas.Add(new BE_Idioma
                    {
                        Codigo = codigo.ToUpperInvariant(),
                        Nombre = string.IsNullOrWhiteSpace(nombre) ? codigo.ToUpperInvariant() : nombre
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error ObtenerIdiomas: {ex.Message}");
            }

            return idiomas;
        }

        public List<string> ObtenerIdiomasDisponibles()
        {
            return ObtenerIdiomas().Select(i => i.Codigo).ToList();
        }

        public string ObtenerTraduccion(string clave, string codigoIdioma)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();

                string claveLimpia = clave.Trim().ToUpper();
                string codigoLimpio = codigoIdioma.Trim().ToUpper();

                System.Diagnostics.Debug.WriteLine($"BLL_Idioma.ObtenerTraduccion: buscando {claveLimpia} para {codigoLimpio}");

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@CODIGO", codigoLimpio));

                DataTable dtIdioma = acc.Leer("IDIOMA_OBTENER_ID", parametros);
                if (dtIdioma.Rows.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"  -> Idioma {codigoLimpio} NO encontrado");
                    acc.Cerrar();
                    return $"[{clave}]";
                }

                int idIdioma = (int)dtIdioma.Rows[0]["ID_IDIOMA"];
                System.Diagnostics.Debug.WriteLine($"  -> ID_IDIOMA={idIdioma}");

                parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@CLAVE", claveLimpia));
                parametros.Add(acc.CrearParametro("@ID_IDIOMA", idIdioma));

                DataTable dt = acc.Leer("SELECT_TRADUCCION", parametros);
                acc.Cerrar();

                if (dt.Rows.Count > 0)
                {
                    string valor = dt.Rows[0]["VALOR"].ToString();
                    System.Diagnostics.Debug.WriteLine($"  -> ENCONTRADO: {valor}");
                    return valor;
                }

                System.Diagnostics.Debug.WriteLine($"  -> Clave {claveLimpia} NO encontrada para idioma {codigoLimpio}");
                return $"[{clave}]";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error ObtenerTraduccion: {ex.Message}");
                return $"[{clave}]";
            }
        }

        public Dictionary<string, string> ObtenerTodosCatalogo(string codigoIdioma)
        {
            Dictionary<string, string> catalogo = new Dictionary<string, string>();

            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sql = @"SELECT CLAVE, VALOR FROM TRADUCCION
                             WHERE ID_IDIOMA = (SELECT ID_IDIOMA FROM IDIOMA WHERE CODIGO = @CODIGO)";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@CODIGO", codigoIdioma.ToUpper()));

                DataTable dt = acc.Leer(sql, parametros);
                acc.Cerrar();

                foreach (DataRow row in dt.Rows)
                {
                    string clave = row["CLAVE"].ToString().ToLower();
                    string valor = row["VALOR"].ToString();
                    catalogo[clave] = valor;
                }
            }
            catch { }

            return catalogo;
        }

        public bool AgregarIdioma(string codigo, string nombre)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();
                string sp = "IDIOMA_AGREGAR";

                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@CODIGO", codigo.ToUpper()));
                parametros.Add(acc.CrearParametro("@NOMBRE", nombre));

                System.Diagnostics.Debug.WriteLine($"[IDIOMA] Insertando idioma: {codigo} - {nombre}");
                int filas = acc.Escribir(sp, parametros);
                System.Diagnostics.Debug.WriteLine($"[IDIOMA] Filas insertadas: {filas}");
                acc.Cerrar();

                if (filas > 0 && codigo.ToUpper() == "ES")
                {
                    
                    AgregarTraduccionesBasicas(codigo);

                    try
                    {
                        var usuario = SERVICE.SV_Singleton.GetInstance().Usuario;
                        if (usuario != null)
                        {
                            BLL_BITACORA bitacora = new BLL_BITACORA();
                            bitacora.IngresarRegistro(usuario, $"Idioma '{nombre}' ({codigo}) agregado");
                        }
                    }
                    catch { }
                }

                return filas > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[IDIOMA] Error al agregar: {ex.Message}");
                return false;
            }
        }

        private void AgregarTraduccionesBasicas(string codigoIdioma)
        {
            var traducciones = new Dictionary<string, string>
            {
                { "LOGIN_TITULO", "Iniciar Sesión" },
                { "LOGIN_USUARIO", "Usuario:" },
                { "LOGIN_CONTRASENA", "Contraseña:" },
                { "LOGIN_INGRESAR", "Ingresar" },
                { "LOGIN_REGISTRARSE", "Registrarse" },
                { "SISTEMA_PRINCIPAL", "Sistema Principal" },
                { "VER_BITACORA", "Ver Bitácora" },
                { "LOGOUT", "Cerrar Sesión" },
                { "OPCIONES", "Opciones" },
                { "VER", "Ver" },
                { "AUDITORIA", "Auditoría" },
                { "BITACORA", "Bitácora" },
                { "PERFILES", "Perfiles" },
                { "BACKUPS", "Backups" },
                { "NUEVO", "Nuevo" },
                { "EDITAR", "Editar" },
                { "ELIMINAR", "Eliminar" },
                { "AGREGAR_PERMISO", "Agregar Permiso" },
                { "ELIMINAR_PERMISO", "Eliminar Permiso" },
                { "NOMBRE", "Nombre" },
                { "DESCRIPCION", "Descripción" },
                { "PERMISOS", "Permisos" },
                { "REALIZAR_BACKUP", "Realizar Backup" },
                { "RESTAURAR", "Restaurar" },
                { "REFRESCAR", "Refrescar" },
                { "CAMBIAR_IDIOMA_TITULO", "Cambiar Idioma" },
                { "CANCELAR", "Cancelar" },
                { "ACEPTAR", "Aceptar" },
                { "CODIGO_IDIOMA", "Código:" },
                { "NOMBRE_IDIOMA", "Nombre:" },
                { "AGREGAR_IDIOMA", "Agregar Idioma" },
                { "CARGAR_CLAVES", "Cargar Claves" },
                { "CLAVES_TRADUCIR", "Claves a traducir:" }
            };

            foreach (var kvp in traducciones)
            {
                AgregarTraduccion(kvp.Key, codigoIdioma, kvp.Value);
            }
        }

        public List<string> ObtenerTodasLasClaves()
        {
            var claves = new List<string>();

            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();

                string sp = "TRADUCCION_OBTENER_CLAVES";
                List<SqlParameter> parametros = new List<SqlParameter>();

                DataTable dt = acc.Leer(sp, parametros);
                acc.Cerrar();

                foreach (DataRow row in dt.Rows)
                {
                    claves.Add(row["CLAVE"].ToString());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error obteniendo claves: {ex.Message}");
            }

            return claves;
        }

        public bool AgregarTraduccion(string clave, string codigoIdioma, string valor)
        {
            try
            {
                ACCESO acc = new ACCESO();
                acc.Abrir();

                string sp = "TRADUCCION_AGREGAR";
                List<SqlParameter> parametros = new List<SqlParameter>();
                parametros.Add(acc.CrearParametro("@CLAVE", clave.ToUpper()));
                parametros.Add(acc.CrearParametro("@CODIGO", codigoIdioma.ToUpper()));
                parametros.Add(acc.CrearParametro("@VALOR", valor));

                System.Diagnostics.Debug.WriteLine($"[TRADUCCION] Insertando: {clave.ToUpper()} - {codigoIdioma.ToUpper()} - {valor}");
                int filas = acc.Escribir(sp, parametros);
                System.Diagnostics.Debug.WriteLine($"[TRADUCCION] Filas: {filas}");
                acc.Cerrar();

                if (filas > 0)
                {
                    try
                    {
                        var usuario = SERVICE.SV_Singleton.GetInstance().Usuario;
                        if (usuario != null)
                        {
                            BLL_BITACORA bitacora = new BLL_BITACORA();
                            bitacora.IngresarRegistro(usuario, $"Traducción agregada: {clave} ({codigoIdioma})");
                        }
                    }
                    catch { }
                }

                return filas > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error AgregarTraduccion: {ex.Message}");
                return false;
            }
        }
    }
}
