using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BE;

namespace DAL
{
    // Puntos verdes, declaraciones, recomendaciones y acopios (N03 y N01). Todo mediante stored procedures.
    public class MP_RECICLAJE
    {
        public DataTable ListarPuntosVerdes()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("PUNTO_VERDE_LISTAR");
            acc.Cerrar();
            return dt;
        }

        public DataTable ListarMaterialesAceptados(int idPuntoVerde)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_PUNTO_VERDE", idPuntoVerde) };
            DataTable dt = acc.Leer("PUNTO_VERDE_MATERIALES_LISTAR", p);
            acc.Cerrar();
            return dt;
        }

        public int AltaPuntoVerde(BE_PuntoVerde pv)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@NOMBRE", pv.Nombre),
                acc.CrearParametro("@DIRECCION", pv.Direccion),
                acc.CrearParametroObj("@LATITUD", pv.Latitud),
                acc.CrearParametroObj("@LONGITUD", pv.Longitud),
                acc.CrearParametroObj("@HORARIO", pv.Horario),
                acc.CrearParametro("@ACTIVO", pv.Activo)
            };
            int id = Convert.ToInt32(acc.Escalar("PUNTO_VERDE_ALTA", p));
            acc.Cerrar();
            return id;
        }

        public void ModificarPuntoVerde(BE_PuntoVerde pv)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID", pv.ID),
                acc.CrearParametro("@NOMBRE", pv.Nombre),
                acc.CrearParametro("@DIRECCION", pv.Direccion),
                acc.CrearParametroObj("@LATITUD", pv.Latitud),
                acc.CrearParametroObj("@LONGITUD", pv.Longitud),
                acc.CrearParametroObj("@HORARIO", pv.Horario),
                acc.CrearParametro("@ACTIVO", pv.Activo)
            };
            acc.Escribir("PUNTO_VERDE_MODIFICAR", p);
            acc.Cerrar();
        }

        // Reemplaza los materiales aceptados del punto verde. Los IDs viajan como lista separada por comas.
        public void GuardarMaterialesAceptados(int idPuntoVerde, List<int> idMateriales)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_PUNTO_VERDE", idPuntoVerde),
                acc.CrearParametro("@MATERIALES", string.Join(",", idMateriales))
            };
            acc.Escribir("PUNTO_VERDE_MATERIALES_GUARDAR", p);
            acc.Cerrar();
        }

        public DataTable ListarPuntosVerdesPorMaterial(int idMaterial)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_MATERIAL", idMaterial) };
            DataTable dt = acc.Leer("PUNTO_VERDE_LISTAR_POR_MATERIAL", p);
            acc.Cerrar();
            return dt;
        }

        public DataTable ListarGuiasPorMaterial(int idMaterial)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_MATERIAL", idMaterial) };
            DataTable dt = acc.Leer("GUIA_LISTAR_POR_MATERIAL", p);
            acc.Cerrar();
            return dt;
        }

        public int AltaDeclaracion(int idUsuario, int idMaterial, decimal cantidad)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_USUARIO", idUsuario),
                acc.CrearParametro("@ID_MATERIAL", idMaterial),
                acc.CrearParametro("@CANTIDAD", cantidad)
            };
            int id = Convert.ToInt32(acc.Escalar("DECLARACION_ALTA", p));
            acc.Cerrar();
            return id;
        }

        public DataTable ListarDeclaracionesPendientes()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("DECLARACION_LISTAR_PENDIENTES");
            acc.Cerrar();
            return dt;
        }

        public DataTable ListarDeclaracionesDeUsuario(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_USUARIO", idUsuario) };
            DataTable dt = acc.Leer("DECLARACION_LISTAR_POR_USUARIO", p);
            acc.Cerrar();
            return dt;
        }

        // Guarda la opción elegida (guía o punto verde) y el nuevo estado de la declaración (transaccional en el SP).
        public void GuardarRecomendacion(int idDeclaracion, int? idGuia, int? idPuntoVerde, string opcionElegida, string nuevoEstado)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_DECLARACION", idDeclaracion),
                acc.CrearParametroObj("@ID_GUIA", idGuia),
                acc.CrearParametroObj("@ID_PUNTO_VERDE", idPuntoVerde),
                acc.CrearParametro("@OPCION", opcionElegida),
                acc.CrearParametro("@ESTADO", nuevoEstado)
            };
            acc.Escribir("RECOMENDACION_GUARDAR", p);
            acc.Cerrar();
        }

        // Registra el acopio y marca la declaración como entregada. Devuelve el ID del acopio.
        public int RegistrarAcopio(int? idDeclaracion, int idPuntoVerde, int idMaterial, decimal pesoKg)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametroObj("@ID_DECLARACION", idDeclaracion),
                acc.CrearParametro("@ID_PUNTO_VERDE", idPuntoVerde),
                acc.CrearParametro("@ID_MATERIAL", idMaterial),
                acc.CrearParametro("@PESO", pesoKg)
            };
            int id = Convert.ToInt32(acc.Escalar("ACOPIO_REGISTRAR", p));
            acc.Cerrar();
            return id;
        }
    }
}
