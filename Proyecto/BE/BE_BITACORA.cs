using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BE_BITACORA
    {
		private int id;

		public int ID
		{
			get { return id; }
			set { id = value; }
		}

		private BE_USUARIO user;

		public BE_USUARIO Usuario
		{
			get { return user; }
			set { user = value; }
		}

		private string descripcion;

		public string Descripcion
		{
			get { return descripcion; }
			set { descripcion = value; }
		}

		private DateTime fecha;

		public DateTime Fecha
		{
			get { return fecha; }
			set { fecha = value; }
		}

		public override string ToString()
		{
			return $"Usuario: {Usuario.Username}, Descripción: {Descripcion}, Fecha: {Fecha}";
		}
	}
}
