using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    // Informes, decisiones, tarifas de comisión y tabla de conversión (N01). Todo mediante stored procedures.
    public class MP_GESTION
    {
        public int AltaInforme(int idUsuario, string tipo, string periodo)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_USUARIO", idUsuario),
                acc.CrearParametro("@TIPO", tipo),
                acc.CrearParametro("@PERIODO", periodo)
            };
            int id = Convert.ToInt32(acc.Escalar("INFORME_ALTA", p));
            acc.Cerrar();
            return id;
        }

        // Acopio en kg por material en el período (formato aaaa-mm).
        public DataTable IndicadorAcopioPorMaterial(string periodo)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@PERIODO", periodo) };
            DataTable dt = acc.Leer("INDICADOR_ACOPIO_POR_MATERIAL", p);
            acc.Cerrar();
            return dt;
        }

        public DataTable IndicadorVentas(string periodo)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@PERIODO", periodo) };
            DataTable dt = acc.Leer("INDICADOR_VENTAS", p);
            acc.Cerrar();
            return dt;
        }

        public decimal IndicadorCostoCanjes(string periodo)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@PERIODO", periodo) };
            object o = acc.Escalar("INDICADOR_COSTO_CANJES", p);
            acc.Cerrar();
            return Convert.ToDecimal(o);
        }

        public int AltaDecision(int? idInforme, int idUsuario, int? idPuntoVerde, string tipo, string descripcion)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametroObj("@ID_INFORME", idInforme),
                acc.CrearParametro("@ID_USUARIO", idUsuario),
                acc.CrearParametroObj("@ID_PUNTO_VERDE", idPuntoVerde),
                acc.CrearParametro("@TIPO", tipo),
                acc.CrearParametro("@DESCRIPCION", descripcion)
            };
            int id = Convert.ToInt32(acc.Escalar("DECISION_ALTA", p));
            acc.Cerrar();
            return id;
        }

        public int AltaTarifa(int? idDecision, string categoria, decimal porcentaje)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametroObj("@ID_DECISION", idDecision),
                acc.CrearParametro("@CATEGORIA", categoria),
                acc.CrearParametro("@PORCENTAJE", porcentaje)
            };
            int id = Convert.ToInt32(acc.Escalar("TARIFA_ALTA", p));
            acc.Cerrar();
            return id;
        }

        public DataTable ListarTarifasVigentes()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("TARIFAS_LISTAR_VIGENTES");
            acc.Cerrar();
            return dt;
        }

        // Porcentaje vigente para una categoría; null si no hay tarifa cargada.
        public decimal? ObtenerPorcentajeVigente(string categoria)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@CATEGORIA", categoria) };
            object o = acc.Escalar("TARIFA_PORCENTAJE_VIGENTE", p);
            acc.Cerrar();
            if (o == null || o == DBNull.Value) return null;
            return Convert.ToDecimal(o);
        }

        // ID de la tarifa vigente para una categoría; null si no hay tarifa cargada.
        public int? ObtenerIdTarifaVigente(string categoria)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@CATEGORIA", categoria) };
            object o = acc.Escalar("TARIFA_ID_VIGENTE", p);
            acc.Cerrar();
            if (o == null || o == DBNull.Value) return null;
            return Convert.ToInt32(o);
        }

        public int AltaConversion(int? idDecision, int idMaterial, decimal puntosPorKg)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametroObj("@ID_DECISION", idDecision),
                acc.CrearParametro("@ID_MATERIAL", idMaterial),
                acc.CrearParametro("@PUNTOS_POR_KG", puntosPorKg)
            };
            int id = Convert.ToInt32(acc.Escalar("CONVERSION_ALTA", p));
            acc.Cerrar();
            return id;
        }

        public DataTable ListarConversionesVigentes()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("CONVERSIONES_LISTAR_VIGENTES");
            acc.Cerrar();
            return dt;
        }

        // Puntos por kg vigentes para un material; null si no hay conversión cargada.
        public decimal? ObtenerPuntosPorKgVigente(int idMaterial)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_MATERIAL", idMaterial) };
            object o = acc.Escalar("CONVERSION_PUNTOS_POR_KG_VIGENTE", p);
            acc.Cerrar();
            if (o == null || o == DBNull.Value) return null;
            return Convert.ToDecimal(o);
        }
    }
}
