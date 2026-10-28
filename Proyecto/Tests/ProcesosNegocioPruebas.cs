using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BE;
using BLL;

namespace Tests
{
    // Casos de prueba CP-01 a CP-50 de docs/casos-de-prueba.md.
    // Ejecutar con:  Tests.exe negocio   (contra la base de pruebas indicada en Tests/App.config).
    // Crea sus propios datos (usuarios TEST_*, puntos verdes TEST_*) y los elimina al terminar.
    internal static class ProcesosNegocioPruebas
    {
        private static int _ok;
        private static int _fail;
        private static string _connStr;

        private static BE_USUARIO _ger, _op, _ac, _c1, _c2, _c3;
        private static int _papel, _vidrio, _pv1, _pv2, _pv3, _guia;
        private static int _declaracionReutilizar, _declaracionEntrega, _declaracionPendiente;
        private static int _productoA, _productoCaro, _orden1, _orden2, _orden3, _reclamo1, _reclamo2;
        private static int _liquidacion1, _liquidacion2;
        private static int _beneficioEnvio, _beneficioCupon;

        public static void Ejecutar()
        {
            _connStr = ConfigurationManager.ConnectionStrings["Proyecto"].ConnectionString;
            Console.WriteLine("=== Pruebas de procesos de negocio (base: " + new SqlConnectionStringBuilder(_connStr).InitialCatalog + ") ===");
            Console.WriteLine();
            try
            {
                Limpiar();
                Preparar();
                EjecutarCasos();
            }
            catch (Exception ex)
            {
                Fail("PREP", "Preparación de datos", ex.Message);
            }
            finally
            {
                try { Limpiar(); }
                catch (Exception ex) { Fail("LIMP", "Limpieza de datos de prueba", ex.Message); }
                Console.WriteLine();
                Console.WriteLine($"Resultado: {_ok} OK, {_fail} FALLAS");
            }
        }

        // ------------------------------------------------------------ preparación

        private static void Preparar()
        {
            _papel = Convert.ToInt32(Scalar("SELECT ID_MATERIAL FROM MATERIAL WHERE NOMBRE = N'Papel'"));
            _vidrio = Convert.ToInt32(Scalar("SELECT ID_MATERIAL FROM MATERIAL WHERE NOMBRE = N'Vidrio'"));
            _beneficioEnvio = Convert.ToInt32(Scalar("SELECT ID_BENEFICIO FROM BENEFICIO WHERE DESCRIPCION LIKE N'Envío%'"));
            _beneficioCupon = Convert.ToInt32(Scalar("SELECT ID_BENEFICIO FROM BENEFICIO WHERE DESCRIPCION LIKE N'Cupón%'"));

            _ger = CrearUsuario("GER", "GERENCIA");
            _op = CrearUsuario("OP", "OPERADOR_PUNTO_VERDE");
            _ac = CrearUsuario("AC", "AREA_COMERCIAL");
            _c1 = CrearUsuario("C1", "CIUDADANO");
            _c2 = CrearUsuario("C2", "CIUDADANO");
            _c3 = CrearUsuario("C3", "CIUDADANO");

            var bllDatos = new BLL_DATOS_USUARIO();
            bllDatos.Guardar(_c1, new BE_DatosUsuario { FechaNacimiento = new DateTime(1990, 5, 10), Latitud = -34.60m, Longitud = -58.38m, Localidad = "Test" });
            bllDatos.Guardar(_c3, new BE_DatosUsuario { FechaNacimiento = new DateTime(1990, 5, 10) });

            var bllGestion = new BLL_GESTION();
            _pv1 = bllGestion.GuardarPuntoVerde(_ger, new BE_PuntoVerde { Nombre = "TEST_PV1", Direccion = "Av. Test 1", Latitud = -34.60m, Longitud = -58.40m, Horario = "8-16", Activo = true },
                                                new List<int> { _papel }, true);
            _pv2 = bllGestion.GuardarPuntoVerde(_ger, new BE_PuntoVerde { Nombre = "TEST_PV2", Direccion = "Av. Test 2", Latitud = -31.40m, Longitud = -64.20m, Horario = "8-16", Activo = true },
                                                new List<int> { _papel }, true);
            _pv3 = bllGestion.GuardarPuntoVerde(_ger, new BE_PuntoVerde { Nombre = "TEST_PV3", Direccion = "Av. Test 3", Latitud = -34.60m, Longitud = -58.40m, Horario = "8-16", Activo = true },
                                                new List<int> { _vidrio }, true);

            _guia = new BLL_CATALOGO().GuardarGuia(_ger, new BE_GuiaReutilizacion
            {
                Material = new BE_Material { ID = _papel },
                Titulo = "TEST_Guia de papel",
                Pasos = "1. Doblar. 2. Reutilizar.",
                Dificultad = "Baja"
            });
        }

        private static BE_USUARIO CrearUsuario(string nombre, string rol)
        {
            string user = "TEST_" + nombre;
            Exec("IF NOT EXISTS (SELECT 1 FROM USUARIO WHERE USERNAME = @u) EXEC ALTA_USUARIO @u, N'TEST'", P("@u", user));
            int id = Convert.ToInt32(Scalar("SELECT ID_USUARIO FROM USUARIO WHERE USERNAME = @u", P("@u", user)));
            Exec("DECLARE @rid INT = (SELECT ID_ROL FROM ROL WHERE NOMBRE = @r); " +
                 "IF NOT EXISTS (SELECT 1 FROM USUARIO_ROL WHERE ID_USUARIO = @id) EXEC USUARIO_ASIGNAR_ROL @id, @rid",
                 P("@id", id), P("@r", rol));
            return new BE_USUARIO { ID = id, Username = user, Rol = rol };
        }

        // ------------------------------------------------------------ casos

        private static void EjecutarCasos()
        {
            string periodo = DateTime.Now.ToString("yyyy-MM");
            var bllReciclaje = new BLL_RECICLAJE();
            var bllEco = new BLL_ECOPUNTOS();
            var bllGestion = new BLL_GESTION();
            var bllCatalogo = new BLL_CATALOGO();
            var bllMkt = new BLL_MARKETPLACE();
            var bllPerfil = new BLL_DATOS_USUARIO();
            var bllEst = new BLL_ESTADISTICAS();
            var pasarela = new PasarelaSimulada();

            Console.WriteLine("-- N03: reconocimiento y canje");

            BLL_RECICLAJE.OpcionesRecomendacion opc = null;
            Caso("CP-01", "Declarar material válido", () =>
            {
                opc = bllReciclaje.DeclararMaterial(_c1, _papel, 5);
                Verificar(opc.IDGuia != null && opc.IDDeclaracion > 0, "declaración con guía");
                _declaracionReutilizar = opc.IDDeclaracion;
            });
            Error("CP-02", "Cantidad inválida (0)", () => bllReciclaje.DeclararMaterial(_c1, _papel, 0), "mayor a cero");
            Error("CP-03", "Declarar sin permiso (GERENCIA)", () => bllReciclaje.DeclararMaterial(_ger, _papel, 1), "permiso");
            Caso("CP-06", "Sugerencia del punto verde más cercano", () =>
                Verificar(opc != null && opc.IDPuntoVerde == _pv1, "esperado PV1", $"obtenido {opc?.IDPuntoVerde}"));
            Caso("CP-04", "Reutilizar en casa acredita Eco-Puntos (5 kg x 10)", () =>
            {
                int antes = bllEco.ObtenerSaldo(_c1.ID);
                bllReciclaje.ConfirmarReutilizacion(_c1, opc);
                int despues = bllEco.ObtenerSaldo(_c1.ID);
                Verificar(despues - antes == 50, "saldo +50", $"diferencia {despues - antes}");
                Verificar(Estado("DECLARACION_MATERIAL", "ID_DECLARACION", _declaracionReutilizar) == "REUTILIZADA", "estado REUTILIZADA");
            });
            BLL_RECICLAJE.OpcionesRecomendacion opc2 = null;
            Caso("CP-05", "Entrega oficial deja la declaración pendiente", () =>
            {
                opc2 = bllReciclaje.DeclararMaterial(_c1, _papel, 3);
                bllReciclaje.ElegirEntregaOficial(_c1, opc2);
                _declaracionEntrega = opc2.IDDeclaracion;
                Verificar(Estado("DECLARACION_MATERIAL", "ID_DECLARACION", _declaracionEntrega) == "PENDIENTE_ENTREGA", "estado PENDIENTE_ENTREGA");
            });
            Caso("CP-07", "Material sin guía no permite reutilizar", () =>
            {
                var sinGuia = bllReciclaje.DeclararMaterial(_c1, _vidrio, 1);
                Error("CP-07b", "Reutilizar sin guía falla", () => bllReciclaje.ConfirmarReutilizacion(_c1, sinGuia), "guía");
            });

            Caso("CP-08", "Recepción aceptada acredita puntos y marca ENTREGADA", () =>
            {
                int antes = bllEco.ObtenerSaldo(_c1.ID);
                int puntos = bllReciclaje.RegistrarRecepcion(_op, _pv1, _declaracionEntrega, _c1.ID, _papel, 3);
                Verificar(puntos == 30, "30 puntos", $"puntos {puntos}");
                Verificar(bllEco.ObtenerSaldo(_c1.ID) - antes == 30, "saldo +30");
                Verificar(Estado("DECLARACION_MATERIAL", "ID_DECLARACION", _declaracionEntrega) == "ENTREGADA", "estado ENTREGADA");
            });
            Error("CP-09", "Punto verde que no acepta el material rechaza la carga", () =>
                bllReciclaje.RegistrarRecepcion(_op, _pv3, null, _c1.ID, _papel, 1), "no acepta");
            Error("CP-10", "Peso inválido en recepción", () => bllReciclaje.RegistrarRecepcion(_op, _pv1, null, _c1.ID, _papel, 0), "mayor a cero");

            Caso("CP-11", "Canje con saldo suficiente (Envío 50 pts)", () =>
            {
                int antes = bllEco.ObtenerSaldo(_c1.ID);
                bllEco.Canjear(_c1, _beneficioEnvio);
                Verificar(antes - bllEco.ObtenerSaldo(_c1.ID) == 50, "saldo -50");
            });
            Error("CP-12", "Canje con saldo insuficiente (Cupón 100 pts)", () => bllEco.Canjear(_c1, _beneficioCupon), "Saldo insuficiente");
            Caso("CP-13", "Canje de beneficio inactivo", () =>
            {
                bllCatalogo.CambiarEstadoBeneficio(_ger, _beneficioEnvio, false);
                try { Error("CP-13b", "Beneficio desactivado no se canjea", () => bllEco.Canjear(_c1, _beneficioEnvio), "no está disponible"); }
                finally { bllCatalogo.CambiarEstadoBeneficio(_ger, _beneficioEnvio, true); }
            });

            Console.WriteLine("-- N01: indicadores, red de puntos verdes y tarifas");

            Caso("CP-14", "Indicadores del período actual", () =>
                Verificar(bllGestion.ConsultarIndicadores(_ger, periodo).Rows.Count > 0, "al menos una fila"));
            Error("CP-15", "Período con formato inválido", () => bllGestion.ConsultarIndicadores(_ger, "2026/10"), "formato");
            Caso("CP-16", "Período sin datos no falla", () =>
                Verificar(bllGestion.ConsultarIndicadores(_ger, "1999-01").Rows.Count > 0, "filas con valor 0"));
            Caso("CP-17b", "Informe registrado en INFORME_GESTION", () =>
                Verificar(Count("SELECT COUNT(*) FROM INFORME_GESTION WHERE ID_USUARIO = @u", P("@u", _ger.ID)) >= 1, "informe registrado"));
            Error("CP-17", "Alta de punto verde sin validación del referente", () =>
                bllGestion.GuardarPuntoVerde(_ger, new BE_PuntoVerde { Nombre = "TEST_PV4", Direccion = "X", Activo = true }, new List<int>(), false), "referente");
            Caso("CP-18", "Alta de punto verde validada queda registrada como decisión", () =>
                Verificar(Count("SELECT COUNT(*) FROM DECISION WHERE ID_PUNTO_VERDE = @p AND TIPO = N'ALTA_PUNTO_VERDE'", P("@p", _pv1)) == 1, "decisión ALTA_PUNTO_VERDE"));
            Caso("CP-19", "Modificar materiales aceptados reemplaza la lista", () =>
            {
                bllGestion.GuardarPuntoVerde(_ger, new BE_PuntoVerde { ID = _pv2, Nombre = "TEST_PV2", Direccion = "Av. Test 2", Latitud = -31.40m, Longitud = -64.20m, Activo = true },
                                             new List<int> { _papel, _vidrio }, true);
                Verificar(Count("SELECT COUNT(*) FROM PUNTO_VERDE_MATERIAL WHERE ID_PUNTO_VERDE = @p", P("@p", _pv2)) == 2, "2 materiales");
                bllGestion.GuardarPuntoVerde(_ger, new BE_PuntoVerde { ID = _pv2, Nombre = "TEST_PV2", Direccion = "Av. Test 2", Latitud = -31.40m, Longitud = -64.20m, Activo = true },
                                             new List<int> { _papel }, true);
                Verificar(Count("SELECT COUNT(*) FROM PUNTO_VERDE_MATERIAL WHERE ID_PUNTO_VERDE = @p", P("@p", _pv2)) == 1, "vuelve a 1 material");
            });
            Error("CP-20a", "Comisión 0 % fuera de rango", () => bllGestion.ProponerTarifa(_ger, "General", 0), "entre");
            Error("CP-20b", "Comisión 60 % fuera de rango", () => bllGestion.ProponerTarifa(_ger, "General", 60), "entre");
            Caso("CP-21", "Nueva comisión 15 % queda vigente", () =>
            {
                bllGestion.ProponerTarifa(_ger, "General", 15);
                Verificar(bllMkt.ObtenerComisionEstimada("General") == 15m, "comisión 15");
            });
            Caso("CP-22", "Conversión de Vidrio a 7,5 puntos por kg", () =>
            {
                bllGestion.ProponerConversion(_ger, _vidrio, 7.5m);
                Verificar(Scalar("SELECT TOP 1 PUNTOS_POR_KG FROM TABLA_CONVERSION WHERE ID_MATERIAL = @m ORDER BY VIGENTE_DESDE DESC, ID_CONVERSION DESC", P("@m", _vidrio)) is decimal d && d == 7.5m, "7,5 vigente");
            });

            Console.WriteLine("-- N02: tienda, pagos, liquidación y reclamos");

            Error("CP-23", "Solicitar verificación sin fecha de nacimiento", () => bllMkt.SolicitarVerificacion(_c2, "DOC-1"), "fecha de nacimiento");
            bllPerfil.Guardar(_c2, new BE_DatosUsuario { FechaNacimiento = DateTime.Today.AddYears(-17) });
            Error("CP-24", "Solicitar verificación siendo menor de 18", () => bllMkt.SolicitarVerificacion(_c2, "DOC-1"), "18 años");
            bllPerfil.Guardar(_c2, new BE_DatosUsuario { FechaNacimiento = new DateTime(1990, 1, 1) });
            Caso("CP-25", "Aprobar y rechazar vendedores", () =>
            {
                bllMkt.SolicitarVerificacion(_c2, "DOC-2");
                bllMkt.ResolverVerificacion(_ac, _c2.ID, true);
                Verificar(bllMkt.EsVendedorVerificado(_c2.ID), "C2 verificado");
                bllMkt.SolicitarVerificacion(_c3, "DOC-3");
                bllMkt.ResolverVerificacion(_ac, _c3.ID, false);
                Verificar(bllMkt.ObtenerEstadoVerificacion(_c3.ID) == "RECHAZADA", "C3 rechazado");
            });
            Error("CP-26", "Publicar sin estar verificado", () =>
                bllMkt.PublicarProducto(_c3, "TEST_NoVerificado", "x", "General", 10, 1), "verificado");
            Caso("CP-27", "Publicar y aprobar producto", () =>
            {
                _productoA = bllMkt.PublicarProducto(_c2, "TEST_Producto A", "Bolsa reciclada", "General", 100, 10);
                bool enCatalogoAntes = ContieneId(bllMkt.ListarCatalogo(), _productoA);
                Verificar(!enCatalogoAntes, "no visible antes de aprobar");
                bllMkt.AprobarProducto(_ac, _productoA);
                Verificar(ContieneId(bllMkt.ListarCatalogo(), _productoA), "visible después de aprobar");
            });
            Caso("CP-28", "Actualizar precio y stock propio", () =>
            {
                bllMkt.ModificarProducto(_c2, _productoA, 120, 8);
                Verificar(Count("SELECT STOCK FROM PRODUCTO WHERE ID_PRODUCTO = @p", P("@p", _productoA)) == 8, "stock 8");
            });
            Error("CP-29", "Modificar producto ajeno", () => bllMkt.ModificarProducto(_c3, _productoA, 1, 1), "pertenece");

            Caso("CP-30", "Compra exitosa (2 unidades)", () =>
            {
                _orden1 = bllMkt.Comprar(_c1, _productoA, 2, "Calle 123", pasarela);
                Verificar(Estado("ORDEN_VENTA", "ID_ORDEN", _orden1) == "PAGADA_PENDIENTE_DESPACHO", "orden pagada pendiente");
                Verificar(Count("SELECT STOCK FROM PRODUCTO WHERE ID_PRODUCTO = @p", P("@p", _productoA)) == 6, "stock 6");
                decimal monto = 240m;
                decimal pct = bllMkt.ObtenerComisionEstimada("General");
                decimal neto = monto - Math.Round(monto * pct / 100m, 2);
                decimal liq = Convert.ToDecimal(Scalar("SELECT MONTO_NETO FROM LIQUIDACION_VENDEDOR WHERE ID_ORDEN = @o", P("@o", _orden1)));
                Verificar(liq == neto, "neto correcto", $"neto {liq} esperado {neto}");
            });
            Caso("CP-31", "Pago rechazado por monto superior a 100.000", () =>
            {
                _productoCaro = bllMkt.PublicarProducto(_c2, "TEST_Caro", "Caro", "General", 200000, 1);
                bllMkt.AprobarProducto(_ac, _productoCaro);
                Error("CP-31b", "Compra rechazada no genera orden", () => bllMkt.Comprar(_c1, _productoCaro, 1, "Calle 1", pasarela), "rechazado");
                Verificar(Count("SELECT STOCK FROM PRODUCTO WHERE ID_PRODUCTO = @p", P("@p", _productoCaro)) == 1, "stock intacto");
            });
            Error("CP-32", "Comprar un producto propio", () => bllMkt.Comprar(_c2, _productoA, 1, "Calle 1", pasarela), "propios");
            Error("CP-33", "Compra sin stock suficiente", () => bllMkt.Comprar(_c1, _productoA, 100, "Calle 1", pasarela), "Stock insuficiente");

            Caso("CP-34", "Despachar por el vendedor dueño", () =>
            {
                bllMkt.MarcarDespachada(_c2, _orden1);
                Verificar(Estado("ORDEN_VENTA", "ID_ORDEN", _orden1) == "DESPACHADA", "DESPACHADA");
            });
            Error("CP-35", "Despachar por otro vendedor", () => bllMkt.MarcarDespachada(_c3, _orden1), "pendiente de despacho");
            Caso("CP-36", "Confirmar recepción conforme libera la liquidación", () =>
            {
                bllMkt.ConfirmarRecepcion(_c1, _orden1);
                Verificar(Estado("ORDEN_VENTA", "ID_ORDEN", _orden1) == "RECIBIDA", "RECIBIDA");
                _liquidacion1 = Convert.ToInt32(Scalar("SELECT ID_LIQUIDACION FROM LIQUIDACION_VENDEDOR WHERE ID_ORDEN = @o", P("@o", _orden1)));
                Verificar(Estado("LIQUIDACION_VENDEDOR", "ID_LIQUIDACION", _liquidacion1) == "DISPONIBLE", "DISPONIBLE");
            });
            Caso("CP-37", "Liquidar saldo disponible", () =>
            {
                bllMkt.PagarLiquidacion(_ac, _liquidacion1);
                Verificar(Estado("LIQUIDACION_VENDEDOR", "ID_LIQUIDACION", _liquidacion1) == "PAGADA", "PAGADA");
            });
            _orden2 = 0;
            Caso("CP-38", "Liquidar saldo retenido falla", () =>
            {
                _orden2 = bllMkt.Comprar(_c1, _productoA, 1, "Calle 2", pasarela);
                _liquidacion2 = Convert.ToInt32(Scalar("SELECT ID_LIQUIDACION FROM LIQUIDACION_VENDEDOR WHERE ID_ORDEN = @o", P("@o", _orden2)));
                Error("CP-38b", "Pagar liquidación retenida", () => bllMkt.PagarLiquidacion(_ac, _liquidacion2), "no está disponible");
            });

            Console.WriteLine("-- N02: reclamos");

            Error("CP-40", "Reclamo sobre orden no despachada", () => bllMkt.PresentarReclamo(_c1, _orden2, "Dañado"), "despachadas");
            Caso("CP-39", "Reclamo sobre orden despachada congela la liquidación", () =>
            {
                bllMkt.MarcarDespachada(_c2, _orden2);
                _reclamo1 = bllMkt.PresentarReclamo(_c1, _orden2, "Producto dañado");
                Verificar(Estado("ORDEN_VENTA", "ID_ORDEN", _orden2) == "EN_RECLAMO", "EN_RECLAMO");
                Verificar(Estado("LIQUIDACION_VENDEDOR", "ID_LIQUIDACION", _liquidacion2) == "CONGELADA", "CONGELADA");
            });
            Caso("CP-41", "Resolver reclamo procedente devuelve la orden y restituye stock", () =>
            {
                int stockAntes = Convert.ToInt32(Scalar("SELECT STOCK FROM PRODUCTO WHERE ID_PRODUCTO = @p", P("@p", _productoA)));
                bllMkt.ResolverReclamo(_ac, _reclamo1, true);
                Verificar(Estado("ORDEN_VENTA", "ID_ORDEN", _orden2) == "DEVUELTA", "DEVUELTA");
                Verificar(Estado("LIQUIDACION_VENDEDOR", "ID_LIQUIDACION", _liquidacion2) == "CANCELADA", "CANCELADA");
                int stockDespues = Convert.ToInt32(Scalar("SELECT STOCK FROM PRODUCTO WHERE ID_PRODUCTO = @p", P("@p", _productoA)));
                Verificar(stockDespues == stockAntes + 1, "stock restituido", $"{stockAntes} -> {stockDespues}");
            });
            Caso("CP-42", "Resolver reclamo improcedente devuelve la liquidación a retenida", () =>
            {
                _orden3 = bllMkt.Comprar(_c1, _productoA, 1, "Calle 3", pasarela);
                bllMkt.MarcarDespachada(_c2, _orden3);
                _reclamo2 = bllMkt.PresentarReclamo(_c1, _orden3, "Sin motivo real");
                bllMkt.ResolverReclamo(_ac, _reclamo2, false);
                Verificar(Estado("ORDEN_VENTA", "ID_ORDEN", _orden3) == "DESPACHADA", "DESPACHADA");
                int liq3 = Convert.ToInt32(Scalar("SELECT ID_LIQUIDACION FROM LIQUIDACION_VENDEDOR WHERE ID_ORDEN = @o", P("@o", _orden3)));
                Verificar(Estado("LIQUIDACION_VENDEDOR", "ID_LIQUIDACION", liq3) == "RETENIDA", "RETENIDA");
            });

            Console.WriteLine("-- Perfil, suscripciones, estadísticas y control de acceso");

            Error("CP-43", "Fecha de nacimiento menor de 16", () =>
                bllPerfil.Guardar(_c1, new BE_DatosUsuario { FechaNacimiento = DateTime.Today.AddYears(-15) }), "16 años");
            Error("CP-44", "Fecha de nacimiento futura", () =>
                bllPerfil.Guardar(_c1, new BE_DatosUsuario { FechaNacimiento = DateTime.Today.AddDays(1) }), "futura");
            Error("CP-45", "Latitud sin longitud", () =>
                bllPerfil.Guardar(_c1, new BE_DatosUsuario { FechaNacimiento = new DateTime(1990, 5, 10), Latitud = -34.6m }), "latitud y longitud");
            Caso("CP-46", "Suscribirse a materiales y ver sus guías", () =>
            {
                bllPerfil.GuardarSuscripciones(_c1, new List<int> { _papel, _vidrio });
                Verificar(Count("SELECT COUNT(*) FROM SUSCRIPCION_MATERIAL WHERE ID_USUARIO = @u", P("@u", _c1.ID)) == 2, "2 suscripciones");
                Verificar(bllPerfil.ListarGuiasSuscriptas(_c1).Rows.Count >= 1, "guía de papel visible");
            });
            Caso("CP-47", "Estadísticas de ventas del vendedor", () =>
            {
                DataTable r = bllEst.Resumen(_c2, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));
                Verificar(r.Rows.Count > 0 && Convert.ToInt32(r.Rows[0]["Ventas"]) >= 1, "al menos una venta");
            });
            Error("CP-48", "Período de estadísticas invertido", () =>
                bllEst.Resumen(_c2, DateTime.Today, DateTime.Today.AddDays(-1)), "posterior");
            Caso("CP-50", "Usuario sin permiso no ejecuta la operación", () =>
            {
                Verificar(!BLL_AUTORIZACION.TienePermiso(_c1, "PROC_VER_INDICADORES"), "CIUDADANO sin indicadores");
                Error("CP-50b", "Exigir lanza UnauthorizedAccess", () => BLL_AUTORIZACION.Exigir(_c1, "PROC_VER_INDICADORES"), "permiso");
            });
            Console.WriteLine("   CP-49 (menú según rol) se verifica manualmente en la interfaz.");
        }

        // ------------------------------------------------------------ helpers de casos

        private static void Caso(string id, string desc, Action accion)
        {
            try
            {
                accion();
                Ok(id, desc);
            }
            catch (Exception ex)
            {
                Fail(id, desc, ex.Message);
            }
        }

        private static void Error(string id, string desc, Action accion, string contiene)
        {
            try
            {
                accion();
                Fail(id, desc, "no lanzó error");
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(contiene, StringComparison.OrdinalIgnoreCase) >= 0)
                    Ok(id, desc);
                else
                    Fail(id, desc, "mensaje inesperado: " + ex.Message);
            }
        }

        private static void Verificar(bool condicion, string desc, string detalle = "")
        {
            if (!condicion) throw new Exception(desc + (detalle == "" ? "" : " (" + detalle + ")"));
        }

        private static bool ContieneId(DataTable tabla, int id)
        {
            foreach (DataRow r in tabla.Rows)
                if (Convert.ToInt32(r["ID"]) == id) return true;
            return false;
        }

        private static void Ok(string id, string desc)
        {
            _ok++;
            Console.WriteLine($"[OK]    {id} {desc}");
        }

        private static void Fail(string id, string desc, string detalle)
        {
            _fail++;
            Console.WriteLine($"[FALLA] {id} {desc} -> {detalle}");
        }

        // ------------------------------------------------------------ acceso a datos de prueba (solo lectura/limpieza)

        private static SqlParameter P(string nombre, object valor)
        {
            return new SqlParameter(nombre, valor ?? DBNull.Value);
        }

        private static object Scalar(string sql, params SqlParameter[] parametros)
        {
            using (var cn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(parametros);
                cn.Open();
                return cmd.ExecuteScalar();
            }
        }

        private static int Count(string sql, params SqlParameter[] parametros)
        {
            return Convert.ToInt32(Scalar(sql, parametros));
        }

        private static string Estado(string tabla, string columnaId, int id)
        {
            return Convert.ToString(Scalar($"SELECT ESTADO FROM {tabla} WHERE {columnaId} = @id", P("@id", id)));
        }

        private static void Exec(string sql, params SqlParameter[] parametros)
        {
            using (var cn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(parametros);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Borra todo lo creado por las pruebas (usuarios TEST_*, puntos verdes TEST_*, guías TEST_*) y sus dependencias.
        private static void Limpiar()
        {
            const string sql = @"
DECLARE @U TABLE (ID INT);
INSERT INTO @U SELECT ID_USUARIO FROM USUARIO WHERE USERNAME LIKE 'TEST[_]%';
DECLARE @P TABLE (ID INT);
INSERT INTO @P SELECT ID_PUNTO_VERDE FROM PUNTO_VERDE WHERE NOMBRE LIKE 'TEST[_]%';
DECLARE @O TABLE (ID INT);
INSERT INTO @O SELECT ID_ORDEN FROM ORDEN_VENTA WHERE ID_COMPRADOR IN (SELECT ID FROM @U) OR ID_VENDEDOR IN (SELECT ID FROM @U);
DECLARE @D TABLE (ID INT);
INSERT INTO @D SELECT ID_DECLARACION FROM DECLARACION_MATERIAL WHERE ID_USUARIO IN (SELECT ID FROM @U);
DECLARE @C TABLE (ID INT);
INSERT INTO @C SELECT ID_CUENTA FROM CUENTA_ECOPUNTOS WHERE ID_USUARIO IN (SELECT ID FROM @U);
DECLARE @DEC TABLE (ID INT);
INSERT INTO @DEC SELECT ID_DECISION FROM DECISION WHERE ID_USUARIO IN (SELECT ID FROM @U) OR ID_PUNTO_VERDE IN (SELECT ID FROM @P);
DELETE FROM RECLAMO WHERE ID_ORDEN IN (SELECT ID FROM @O);
DELETE FROM LIQUIDACION_VENDEDOR WHERE ID_ORDEN IN (SELECT ID FROM @O);
DELETE FROM COMISION WHERE ID_ORDEN IN (SELECT ID FROM @O);
DELETE FROM PAGO WHERE ID_ORDEN IN (SELECT ID FROM @O);
DELETE FROM ORDEN_DETALLE WHERE ID_ORDEN IN (SELECT ID FROM @O);
DELETE FROM ORDEN_VENTA WHERE ID_ORDEN IN (SELECT ID FROM @O);
DELETE FROM PRODUCTO WHERE ID_VENDEDOR IN (SELECT ID FROM @U);
DELETE FROM VERIFICACION_VENDEDOR WHERE ID_USUARIO IN (SELECT ID FROM @U);
DELETE FROM RECOMENDACION WHERE ID_DECLARACION IN (SELECT ID FROM @D);
DELETE FROM ACOPIO WHERE ID_DECLARACION IN (SELECT ID FROM @D) OR ID_PUNTO_VERDE IN (SELECT ID FROM @P);
DELETE FROM DECLARACION_MATERIAL WHERE ID_USUARIO IN (SELECT ID FROM @U);
DELETE FROM MOVIMIENTO_ECOPUNTOS WHERE ID_CUENTA IN (SELECT ID FROM @C);
DELETE FROM CANJE WHERE ID_USUARIO IN (SELECT ID FROM @U);
DELETE FROM CUENTA_ECOPUNTOS WHERE ID_USUARIO IN (SELECT ID FROM @U);
DELETE FROM SUSCRIPCION_MATERIAL WHERE ID_USUARIO IN (SELECT ID FROM @U);
DELETE FROM DATOS_USUARIO WHERE ID_USUARIO IN (SELECT ID FROM @U);
DELETE FROM GUIA_REUTILIZACION WHERE TITULO LIKE 'TEST[_]%';
DELETE FROM PUNTO_VERDE_MATERIAL WHERE ID_PUNTO_VERDE IN (SELECT ID FROM @P);
DELETE FROM TARIFA_COMISION WHERE ID_DECISION IN (SELECT ID FROM @DEC);
DELETE FROM TABLA_CONVERSION WHERE ID_DECISION IN (SELECT ID FROM @DEC);
DELETE FROM DECISION WHERE ID_DECISION IN (SELECT ID FROM @DEC);
DELETE FROM INFORME_GESTION WHERE ID_USUARIO IN (SELECT ID FROM @U);
DELETE FROM PUNTO_VERDE WHERE ID_PUNTO_VERDE IN (SELECT ID FROM @P);
DELETE FROM BITACORA WHERE ID_USUARIO IN (SELECT ID FROM @U);
DELETE FROM USUARIO_ROL WHERE ID_USUARIO IN (SELECT ID FROM @U);
DELETE FROM USUARIO WHERE ID_USUARIO IN (SELECT ID FROM @U);";
            Exec(sql);
        }
    }
}
