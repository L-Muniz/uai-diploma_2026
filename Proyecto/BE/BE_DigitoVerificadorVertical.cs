using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    
    public class BE_DigitoVerificadorVertical
    {
        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idEntidad; 

        public int IdEntidad
        {
            get { return idEntidad; }
            set { idEntidad = value; }
        }

        private string tipoEntidad; 

        public string TipoEntidad
        {
            get { return tipoEntidad; }
            set { tipoEntidad = value; }
        }

        private int dvUsername; 

        public int DVUsername
        {
            get { return dvUsername; }
            set { dvUsername = value; }
        }

        private int dvHashedPassword; 

        public int DVHashedPassword
        {
            get { return dvHashedPassword; }
            set { dvHashedPassword = value; }
        }

        private int dvBloqueado; 

        public int DVBloqueado
        {
            get { return dvBloqueado; }
            set { dvBloqueado = value; }
        }

        private DateTime fechaCalculoDV; 

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
