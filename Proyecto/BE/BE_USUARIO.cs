using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	public class BE_USUARIO
	{
        private string user;

        public string Username
        {
            get { return user; }
            set { user = value; }
        }

        private string pass;

        public string HashedPassword
        {
            get { return pass; }
            set { pass = value; }
        }

        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private bool bloqueado;

        public bool Bloqueado
        {
            get { return bloqueado; }
            set { bloqueado = value; }
        }

        private int dvHorizontal;

        public int DVHorizontal
        {
            get { return dvHorizontal; }
            set { dvHorizontal = value; }
        }

        private string rol;

        public string Rol
        {
            get { return rol; }
            set { rol = value; }
        }

        private string codigoIdioma;

        /// <summary>Código del idioma preferido del usuario (ej. "ES"). Null si no tiene preferencia.</summary>
        public string CodigoIdioma
        {
            get { return codigoIdioma; }
            set { codigoIdioma = value; }
        }

        public BE_USUARIO()
        {
            Bloqueado = false;
            DVHorizontal = 0;
            Rol = "usuario";
        }

        public BE_USUARIO(string user, string pass)
        {
            HashedPassword = pass;
            Username = user;
        }

        public override string ToString()
        {
            return Username;
        }
    }
}
