using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using SERVICE;

namespace BLL
{
	public class BLL_GESTOR
	{
		public static string HashPassword(string password)
		{
			return SV_Hash.HashPassword(password);
		}

		public static bool VerifyPassword(string password, string storedHash)
		{
			return SV_Hash.VerifyPassword(password, storedHash);
		}

		public static bool EsHashValido(string storedHash)
		{
			return SV_Hash.EsHashValido(storedHash);
		}
	}
}
