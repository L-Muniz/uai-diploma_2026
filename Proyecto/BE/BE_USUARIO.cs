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

        // Originator: crea un memento con el estado actual del usuario.
        public BE_UsuarioMemento CrearMemento(List<int> idRoles, string descripcion)
        {
            return new BE_UsuarioMemento
            {
                IdUsuario = ID,
                Username = Username,
                HashedPassword = HashedPassword,
                Bloqueado = Bloqueado,
                IdRoles = new List<int>(idRoles ?? new List<int>()),
                Descripcion = descripcion,
                FechaCaptura = DateTime.Now
            };
        }

        // Originator: aplica el estado guardado en el memento. Los roles los restaura la BLL.
        public void RestaurarDesde(BE_UsuarioMemento memento)
        {
            if (memento == null)
                throw new ArgumentNullException(nameof(memento));

            if (memento.IdUsuario != ID)
                throw new InvalidOperationException("El memento pertenece a otro usuario.");

            Username = memento.Username;
            HashedPassword = memento.HashedPassword;
            Bloqueado = memento.Bloqueado;
        }

        public override string ToString()
        {
            return Username;
        }
    }
}
