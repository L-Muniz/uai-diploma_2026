using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    public class MP_MEMENTO_USUARIO : MAPPER<BE_UsuarioMemento>
    {
        // Devuelve el ID generado para el memento, o 0 si no se pudo guardar.
        public override int Alta(BE_UsuarioMemento obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "MEMENTO_USUARIO_ALTA";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_USUARIO", obj.IdUsuario));
            parametros.Add(acc.CrearParametro("@DESCRIPCION", obj.Descripcion ?? ""));
            parametros.Add(acc.CrearParametro("@CONTENIDO", obj.Contenido));
            parametros.Add(acc.CrearParametro("@FECHA_CAPTURA", obj.FechaCaptura));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            if (dt.Rows.Count == 0)
                return 0;

            return int.Parse(dt.Rows[0]["ID_MEMENTO"].ToString());
        }

        public override int Borrar(BE_UsuarioMemento obj)
        {
            throw new NotImplementedException("Los mementos son inmutables; no se pueden eliminar desde la aplicación");
        }

        public override int Editar(BE_UsuarioMemento obj)
        {
            throw new NotImplementedException("Los mementos son inmutables; no se pueden editar");
        }

        public override List<BE_UsuarioMemento> Listar()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "MEMENTO_USUARIO_LISTAR";
            DataTable dt = acc.Leer(sql);
            acc.Cerrar();

            return MapearLista(dt);
        }

        public List<BE_UsuarioMemento> ListarPorUsuario(int idUsuario)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "MEMENTO_USUARIO_LISTAR_X_USUARIO";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_USUARIO", idUsuario));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            return MapearLista(dt);
        }

        public BE_UsuarioMemento ObtenerPorId(int idMemento)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "MEMENTO_USUARIO_OBTENER";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@ID_MEMENTO", idMemento));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            if (dt.Rows.Count == 0)
                return null;

            return MapearDatos(dt.Rows[0]);
        }

        private List<BE_UsuarioMemento> MapearLista(DataTable dt)
        {
            List<BE_UsuarioMemento> lista = new List<BE_UsuarioMemento>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(MapearDatos(row));
            }

            return lista;
        }

        private BE_UsuarioMemento MapearDatos(DataRow row)
        {
            return new BE_UsuarioMemento
            {
                ID = int.Parse(row["ID_MEMENTO"].ToString()),
                IdUsuario = int.Parse(row["ID_USUARIO"].ToString()),
                Descripcion = row["DESCRIPCION"].ToString(),
                FechaCaptura = DateTime.Parse(row["FECHA_CAPTURA"].ToString()),
                Contenido = row["CONTENIDO"].ToString()
            };
        }
    }
}
