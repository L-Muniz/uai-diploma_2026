using SERVICE;
using BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    internal static class Program
    {
        public static bool IntegridadFallo { get; set; } = false;
        public static string MensajeIntegridad { get; set; } = "";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                VerificarIntegridadAlInicio();

                var resultadoBackup = BLL_BackupRestore.RealizarBackup();
                if (!resultadoBackup.exito)
                {
                    System.Diagnostics.Debug.WriteLine($"Advertencia: {resultadoBackup.mensaje}");
                }

                int intervalo = 30;
                string cfgIntervalo = ConfigurationManager.AppSettings["BackupIntervalMinutes"];
                if (!string.IsNullOrWhiteSpace(cfgIntervalo))
                    int.TryParse(cfgIntervalo, out intervalo);
                if (intervalo <= 0) intervalo = 30;

                BLL_BackupRestore.ConfigurarBackupAutomatico(intervalo);

                Application.Run(new LogInOut());
            }
            catch (Exception ex)
            {
                ManejarExcepcion(ex, "Error fatal al iniciar la aplicación");
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ManejarExcepcion(e.Exception, "Error en el hilo de la aplicación");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                ManejarExcepcion(ex, "Error no manejado en la aplicación");
            }
        }

        private static void VerificarIntegridadAlInicio()
        {
            try
            {
                var resultado = BLL_IntegridadDatos.DetectarFalloIntegridadPrioritario();
                if (!resultado.esValido)
                {
                    IntegridadFallo = true;
                    MensajeIntegridad = resultado.mensaje;
                    // El detalle técnico solo se muestra a administradores, después del login
                    // (ver LogInOut). Antes de autenticar se enmascara el error.
                    MessageBox.Show(SV_Mensajes.GetIntegridadNoDisponibleMessage(),
                        "Integridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                System.Diagnostics.Debug.WriteLine("✓ Verificación de integridad completada: OK");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Advertencia en verificación de integridad: {ex.Message}");
            }
        }

        private static void ManejarExcepcion(Exception ex, string titulo)
        {
            string mensaje = $"{titulo}\n\n{ex.GetType().Name}: {ex.Message}\n\nDetalles:\n{ex.StackTrace}";

            System.Diagnostics.Debug.WriteLine($"EXCEPCIÓN: {mensaje}");

            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
