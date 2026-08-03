using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class MP_BITACORA : MAPPER<BE_BITACORA>
    {
        public override int Alta(BE_BITACORA obj)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "REGISTRO";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@id_user", obj.Usuario.ID));
            parametros.Add(acc.CrearParametro("@mensaje", obj.Descripcion));
            parametros.Add(acc.CrearParametro("@fecha", obj.Fecha));
            int filas = acc.Escribir(sql, parametros);
            acc.Cerrar();
            return filas;
        }

        public override int Borrar(BE_BITACORA obj)
        {
            throw new NotImplementedException();
        }

        public override int Editar(BE_BITACORA obj)
        {
            throw new NotImplementedException();
        }

        public override List<BE_BITACORA> Listar()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "BITACORA_LISTAR";
            DataTable dt = acc.Leer(sql);
            acc.Cerrar();

            List<BE_BITACORA> lista = new List<BE_BITACORA>();
            Dictionary<int, BE_USUARIO> cacheUsuarios = new Dictionary<int, BE_USUARIO>();
            foreach (DataRow row in dt.Rows)
            {
                BE_BITACORA bitacora = new BE_BITACORA();
                bitacora.ID = int.Parse(row["ID_BITACORA"].ToString());
                bitacora.Descripcion = row["DESCRIPCION"].ToString();
                bitacora.Fecha = DateTime.Parse(row["FECHA"].ToString());

                int idUsuario = int.Parse(row["ID_USUARIO"].ToString());

                if (cacheUsuarios.TryGetValue(idUsuario, out BE_USUARIO find))
                {
                    bitacora.Usuario = find;
                }
                else
                {
                    MP_USUARIO mpUsuario = new MP_USUARIO();
                    BE_USUARIO usuario = mpUsuario.BuscarPorId(idUsuario);
                    cacheUsuarios.Add(idUsuario, usuario);
                    bitacora.Usuario = usuario;
                }
                lista.Add(bitacora);
            }

            return lista;
        }

        public List<BE_BITACORA> ListarXID(BE_USUARIO user)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            string sql = "BITACORA_X_USUARIO";
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acc.CrearParametro("@id_user", user.ID));
            DataTable dt = acc.Leer(sql, parametros);
            acc.Cerrar();

            List<BE_BITACORA> lista = new List<BE_BITACORA>();
            foreach (DataRow row in dt.Rows)
            {
                BE_BITACORA bitacora = new BE_BITACORA();
                bitacora.ID = int.Parse(row["ID_BITACORA"].ToString());
                bitacora.Descripcion = row["DESCRIPCION"].ToString();
                bitacora.Fecha = DateTime.Parse(row["FECHA"].ToString());
                bitacora.Usuario = user;

                lista.Add(bitacora);
            }
            return lista;
        }
    }
}
