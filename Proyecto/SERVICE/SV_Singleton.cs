using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace SERVICE
{
    public class SV_Singleton
    {
        private static SV_Singleton singleton;

        public static SV_Singleton GetInstance()
        {
            if (singleton == null)
            {
                singleton = new SV_Singleton();
            }
            return singleton;
        }

        private BE_USUARIO usuario;

        public BE_USUARIO Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public void LimpiarSesion()
        {
            usuario = null;
            singleton = null;
        }
    }
}
