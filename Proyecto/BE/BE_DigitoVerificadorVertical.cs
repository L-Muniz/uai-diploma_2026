using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Entidad para almacenar D�gitos Verificadores Verticales
    /// Relacionada 1:1 con BE_USUARIO (pero extensible a otras entidades)
    /// </summary>
    public class BE_DigitoVerificadorVertical
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idEntidad; // FK a la entidad relacionada (ej: ID_USUARIO)

        public int IdEntidad
        {
            get { return idEntidad; }
            set { idEntidad = value; }
        }

        private string tipoEntidad; // Nombre de la entidad (ej: "USUARIO")

        public string TipoEntidad
        {
            get { return tipoEntidad; }
            set { tipoEntidad = value; }
        }

        private int dvUsername; // DV del atributo USERNAME

        public int DVUsername
        {
            get { return dvUsername; }
            set { dvUsername = value; }
        }

        private int dvHashedPassword; // DV del atributo PASS

        public int DVHashedPassword
        {
            get { return dvHashedPassword; }
            set { dvHashedPassword = value; }
        }

        private int dvBloqueado; // DV del atributo BLOQUEADO

        public int DVBloqueado
        {
            get { return dvBloqueado; }
            set { dvBloqueado = value; }
        }

        private DateTime fechaCalculoDV; // Cu�ndo se calcul� el DV

        public DateTime FechaCreacion
        {
            get { return fechaCalculoDV; }
            set { fechaCalculoDV = value; }
        }

        public BE_DigitoVerificadorVertical()
        {
            FechaCreacion = DateTime.Now;
        }

        public override string ToString()
        {
            return $"DVV - Entidad: {TipoEntidad}, ID: {IdEntidad}, Fecha: {FechaCreacion}";
        }
    }
}
