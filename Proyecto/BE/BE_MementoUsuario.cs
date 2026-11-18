using System;
using System.Text;

namespace BE
{
    /// <summary>
    /// Memento del estado de un usuario en lo que se edita desde la interfaz: nombre de usuario y hash de contraseña.
    /// El hash es necesario para poder restaurar la contraseña anterior; por eso el registro de auditoría
    /// de tipo MEMENTO nunca se muestra en claro (ver FormAuditoria).
    /// </summary>
    public class BE_MementoUsuario
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }

        /// <summary>Formato: ID:{id}|U:{base64 username}|H:{base64 hash}</summary>
        public string ToSerializedString()
        {
            return string.Join("|",
                "ID:" + ID,
                "U:" + Codificar(Username),
                "H:" + Codificar(HashedPassword));
        }

        public static BE_MementoUsuario FromSerializedString(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;

            var memento = new BE_MementoUsuario();
            foreach (var parte in texto.Split('|'))
            {
                int separador = parte.IndexOf(':');
                if (separador < 0)
                    continue;

                string clave = parte.Substring(0, separador);
                string valor = parte.Substring(separador + 1);

                switch (clave)
                {
                    case "ID":
                        memento.ID = int.Parse(valor);
                        break;
                    case "U":
                        memento.Username = Decodificar(valor);
                        break;
                    case "H":
                        memento.HashedPassword = Decodificar(valor);
                        break;
                }
            }

            return memento.ID > 0 ? memento : null;
        }

        private static string Codificar(string texto)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(texto ?? ""));
        }

        private static string Decodificar(string base64)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }
    }
}
