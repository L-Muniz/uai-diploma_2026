using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    // Cuenta de Eco-Puntos, movimientos, beneficios activos y canjes (N03). Todo mediante stored procedures.
    public class MP_ECOPUNTOS
    {
        // Acredita puntos (negativos para descontar) y devuelve el nuevo saldo.
        public int AcreditarPuntos(int idUsuario, int puntos, string tipo, string motivo)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_USUARIO", idUsuario),
                acc.CrearParametro("@PUNTOS", puntos),
                acc.CrearParametro("@TIPO", tipo),
                acc.CrearParametroObj("@MOTIVO", motivo)
            };
            int saldo = Convert.ToInt32(acc.Escalar("CUENTA_ACREDITAR", p));
            acc.Cerrar();
            return saldo;
        }

        public int ObtenerSaldo(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_USUARIO", idUsuario) };
            object o = acc.Escalar("CUENTA_SALDO", p);
            acc.Cerrar();
            return Convert.ToInt32(o);
        }

        public DataTable ListarMovimientos(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_USUARIO", idUsuario) };
            DataTable dt = acc.Leer("MOVIMIENTOS_LISTAR", p);
            acc.Cerrar();
            return dt;
        }

        public DataTable ListarBeneficiosActivos()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("BENEFICIO_LISTAR_ACTIVOS");
            acc.Cerrar();
            return dt;
        }

        // Devuelve el ID del canje registrado.
        public int RegistrarCanje(int idUsuario, int idBeneficio)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_USUARIO", idUsuario),
                acc.CrearParametro("@ID_BENEFICIO", idBeneficio)
            };
            int id = Convert.ToInt32(acc.Escalar("CANJE_REGISTRAR", p));
            acc.Cerrar();
            return id;
        }

        public DataTable ListarCanjes(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID_USUARIO", idUsuario) };
            DataTable dt = acc.Leer("CANJES_LISTAR", p);
            acc.Cerrar();
            return dt;
        }
    }
}
