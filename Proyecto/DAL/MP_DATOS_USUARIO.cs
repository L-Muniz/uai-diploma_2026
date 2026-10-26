using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    // Perfil del usuario (fecha de nacimiento y ubicación) y suscripciones a materiales. Todo mediante stored procedures.
    public class MP_DATOS_USUARIO
    {
        public BE_DatosUsuario Obtener(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_USUARIO", idUsuario) };
            DataTable dt = acc.Leer("DATOS_USUARIO_OBTENER", p);
            acc.Cerrar();

            if (dt.Rows.Count == 0)
                return new BE_DatosUsuario { IDUsuario = idUsuario };

            DataRow r = dt.Rows[0];
            return new BE_DatosUsuario
            {
                IDUsuario = idUsuario,
                FechaNacimiento = r["FECHA_NACIMIENTO"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["FECHA_NACIMIENTO"]),
                Latitud = r["LATITUD"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(r["LATITUD"]),
                Longitud = r["LONGITUD"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(r["LONGITUD"]),
                Localidad = r["LOCALIDAD"] == DBNull.Value ? null : r["LOCALIDAD"].ToString()
            };
        }

        public void Guardar(BE_DatosUsuario datos)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_USUARIO", datos.IDUsuario),
                acc.CrearParametroObj("@FECHA_NACIMIENTO", datos.FechaNacimiento?.Date),
                acc.CrearParametroObj("@LATITUD", datos.Latitud),
                acc.CrearParametroObj("@LONGITUD", datos.Longitud),
                acc.CrearParametroObj("@LOCALIDAD", datos.Localidad)
            };
            acc.Escribir("DATOS_USUARIO_GUARDAR", p);
            acc.Cerrar();
        }

        public DataTable ListarSuscripciones(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_USUARIO", idUsuario) };
            DataTable dt = acc.Leer("SUSCRIPCIONES_LISTAR", p);
            acc.Cerrar();
            return dt;
        }

        public void GuardarSuscripciones(int idUsuario, List<int> idMateriales)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_USUARIO", idUsuario),
                acc.CrearParametro("@MATERIALES", string.Join(",", idMateriales))
            };
            acc.Escribir("SUSCRIPCIONES_GUARDAR", p);
            acc.Cerrar();
        }

        public DataTable ListarGuiasSuscriptas(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_USUARIO", idUsuario) };
            DataTable dt = acc.Leer("GUIAS_SUSCRIPTAS_LISTAR", p);
            acc.Cerrar();
            return dt;
        }
    }
}
