using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class MP_CambiosEntidad : MAPPER<BE_CambiosEntidad>
    {
        public override int Alta(BE_CambiosEntidad obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "CAMBIOS_ENTIDAD_ALTA";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@tipoEntidad", obj.TipoEntidad));
            parametros.Add(acc.CrearParametro("@idEntidad", obj.IdEntidad));
            parametros.Add(acc.CrearParametro("@nombreAtributo", obj.NombreAtributo));
            parametros.Add(acc.CrearParametro("@valorAnterior", obj.ValorAnterior ?? ""));
            parametros.Add(acc.CrearParametro("@valorNuevo", obj.ValorNuevo ?? ""));
            parametros.Add(acc.CrearParametro("@idUsuarioModifico", obj.UsuarioModifico?.ID ?? 0));
            parametros.Add(acc.CrearParametro("@fechaCambio", obj.FechaCambio));
            parametros.Add(acc.CrearParametro("@descripcion", obj.Descripcion ?? ""));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override int Borrar(BE_CambiosEntidad obj)
        {
            throw new NotImplementedException("No se pueden eliminar registros de auditoría");
        }

        public override int Editar(BE_CambiosEntidad obj)
        {
            throw new NotImplementedException("No se pueden editar registros de auditoría");
        }

        public override List<BE_CambiosEntidad> Listar()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "CAMBIOS_ENTIDAD_LISTAR";
            DataTable dt = acc.Leer(sql);
            acc.Cerrar();

            List<BE_CambiosEntidad> lista = new List<BE_CambiosEntidad>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearDatos(row));
            }

            return lista;
        }

        public List<BE_CambiosEntidad> ListarPorEntidad(string tipoEntidad, int idEntidad)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "CAMBIOS_ENTIDAD_LISTAR_X_ENTIDAD";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@tipoEntidad", tipoEntidad));
            parametros.Add(acc.CrearParametro("@idEntidad", idEntidad));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            List<BE_CambiosEntidad> lista = new List<BE_CambiosEntidad>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearDatos(row));
            }

            return lista;
        }

        public List<BE_CambiosEntidad> ListarPorUsuario(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "CAMBIOS_ENTIDAD_LISTAR_X_USUARIO";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@idUsuario", idUsuario));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            List<BE_CambiosEntidad> lista = new List<BE_CambiosEntidad>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearDatos(row));
            }

            return lista;
        }

        public List<BE_CambiosEntidad> ListarPorFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "CAMBIOS_ENTIDAD_LISTAR_X_FECHAS";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@fechaDesde", fechaDesde));
            parametros.Add(acc.CrearParametro("@fechaHasta", fechaHasta));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            List<BE_CambiosEntidad> lista = new List<BE_CambiosEntidad>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearDatos(row));
            }

            return lista;
        }

        private BE_CambiosEntidad MapearDatos(DataRow row)
        {
            BE_CambiosEntidad cambio = new BE_CambiosEntidad();
            cambio.ID = int.Parse(row["ID_CAMBIO"].ToString());
            cambio.TipoEntidad = row["TIPO_ENTIDAD"].ToString();
            cambio.IdEntidad = int.Parse(row["ID_ENTIDAD"].ToString());
            cambio.NombreAtributo = row["NOMBRE_ATRIBUTO"].ToString();
            cambio.ValorAnterior = row["VALOR_ANTERIOR"].ToString();
            cambio.ValorNuevo = row["VALOR_NUEVO"].ToString();
            cambio.FechaCambio = DateTime.Parse(row["FECHA_CAMBIO"].ToString());
            cambio.Descripcion = row["DESCRIPCION"].ToString();

            // Obtener usuario que realizó el cambio
            int idUsuario = int.Parse(row["ID_USUARIO_MODIFICO"].ToString());
            if (idUsuario > 0)
            {
                MP_USUARIO mpUsuario = new MP_USUARIO();
                cambio.UsuarioModifico = mpUsuario.BuscarPorId(idUsuario);
            }

            return cambio;
        }
    }
}
