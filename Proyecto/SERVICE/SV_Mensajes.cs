using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICE
{
    
    public class SV_Mensajes
    {
        private static SV_IdiomaManager idiomaManager = SV_IdiomaManager.GetInstance();

        public static string GetLoginMessage()
        {
            return "Inicio de sesión";
        }

        public static string GetLogoutMessage()
        {
            return "Cierre de sesión";
        }

        public static string GetNewUserMessage()
        {
            return "Creación del Usuario";
        }

        public static string GetUsuarioBloqueadoMessage()
        {
            return idiomaManager.Obtener("usuario.bloqueado", "El usuario está bloqueado. Contacte al administrador.");
        }

        public static string GetIntentoFallidoMessage(int intento, int intentosMaximos)
        {
            return $"Usuario o contraseña incorrectos. Intento {intento} de {intentosMaximos}.";
        }

        public static string GetUsuarioBloqueadoPorIntentosMessage()
        {
            return "Ha excedido el número máximo de intentos. El usuario ha sido bloqueado.";
        }

        public static string GetErrorIntegridadDatos()
        {
            return idiomaManager.Obtener("error.integridad", "ALERTA CRÍTICA: Se ha detectado una alteración en la integridad de datos.");
        }

        public static string GetErrorIntegridadDVH()
        {
            return "Error de integridad DVH: Los datos del usuario han sido modificados.";
        }

        public static string GetErrorIntegridadDVV()
        {
            return "Error de integridad DVV: Atributos específicos del usuario han sido comprometidos.";
        }

        public static string GetVerificacionIntegridadExitosa()
        {
            return "Verificación de integridad completada exitosamente.";
        }

        public static string Obtener(string clave)
        {
            return idiomaManager.Obtener(clave);
        }

        public static string Obtener(string clave, params object[] args)
        {
            return idiomaManager.Obtener(clave, args);
        }
    }
}
