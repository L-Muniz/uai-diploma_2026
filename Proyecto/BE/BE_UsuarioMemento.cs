using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BE
{
    // Memento del usuario: estado capturado en un momento dado.
    // Lo crea BE_USUARIO.CrearMemento y lo aplica BE_USUARIO.RestaurarDesde.
    // Se trata como inmutable: una vez creado no se modifica.
    public class BE_UsuarioMemento
    {
        [XmlIgnore]
        public int ID { get; set; }

        public int IdUsuario { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public bool Bloqueado { get; set; }
        public List<int> IdRoles { get; set; }

        [XmlIgnore]
        public string Descripcion { get; set; }

        [XmlIgnore]
        public DateTime FechaCaptura { get; set; }

        // Forma persistida del estado (XML encriptado). Solo la usa la BLL.
        [XmlIgnore]
        public string Contenido { get; set; }

        public BE_UsuarioMemento()
        {
            IdRoles = new List<int>();
        }
    }
}
