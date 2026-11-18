using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BE
{
    /// <summary>
    /// Memento del estado de un rol: nombre, descripción y permisos asignados directamente.
    /// Es un objeto de valor inmutable en la práctica: lo crea BLL_ROL.CrearMemento, lo guarda
    /// el control de cambios (BLL_ControlCambios) y lo consume BLL_ROL.RestaurarMemento.
    /// La serialización usa texto plano (Base64 para los textos libres) para no depender de
    /// referencias extra y para que quepa en la columna de valores de CAMBIOS_ENTIDAD.
    /// </summary>
    public class BE_MementoRol
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<int> IdsPermisos { get; set; } = new List<int>();

        /// <summary>Formato: ID:{id}|N:{base64 nombre}|D:{base64 descripción}|P:{ids separados por coma}</summary>
        public string ToSerializedString()
        {
            return string.Join("|",
                "ID:" + ID,
                "N:" + Codificar(Nombre),
                "D:" + Codificar(Descripcion),
                "P:" + string.Join(",", (IdsPermisos ?? new List<int>()).OrderBy(i => i)));
        }

        public static BE_MementoRol FromSerializedString(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;

            var memento = new BE_MementoRol();
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
                    case "N":
                        memento.Nombre = Decodificar(valor);
                        break;
                    case "D":
                        memento.Descripcion = Decodificar(valor);
                        break;
                    case "P":
                        memento.IdsPermisos = valor.Length == 0
                            ? new List<int>()
                            : valor.Split(',').Select(int.Parse).ToList();
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
