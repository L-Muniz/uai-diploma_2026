using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using SERVICE;

namespace Tests
{
    
    internal static class Program
    {
        private static int _passed;
        private static int _failed;

        private static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "negocio")
            {
                ProcesosNegocioPruebas.Ejecutar();
                return;
            }

            Console.WriteLine("=== Pruebas de arquitectura y servicios ===");
            Console.WriteLine();

            Run("GUI no referencia DAL", () => AssertFalse(References(Load("GUI"), "DAL")));
            Run("DAL referencia BE", () => AssertTrue(References(Load("DAL"), "BE")));
            Run("DAL no referencia BLL", () => AssertFalse(References(Load("DAL"), "BLL")));
            Run("DAL no referencia GUI", () => AssertFalse(References(Load("DAL"), "GUI")));
            Run("DAL no referencia SERVICE", () => AssertFalse(References(Load("DAL"), "SERVICE")));
            Run("BE no referencia DAL", () => AssertFalse(References(Load("BE"), "DAL")));
            Run("BE no referencia BLL", () => AssertFalse(References(Load("BE"), "BLL")));
            Run("BE no referencia GUI", () => AssertFalse(References(Load("BE"), "GUI")));
            Run("BE no referencia SERVICE", () => AssertFalse(References(Load("BE"), "SERVICE")));
            Run("SERVICE no referencia BLL", () => AssertFalse(References(Load("SERVICE"), "BLL")));
            Run("SERVICE no referencia DAL", () => AssertFalse(References(Load("SERVICE"), "DAL")));
            Run("SERVICE no referencia GUI", () => AssertFalse(References(Load("SERVICE"), "GUI")));
            Run("BLL referencia BE/DAL/SERVICE", () =>
            {
                var bll = Load("BLL");
                AssertTrue(References(bll, "BE"));
                AssertTrue(References(bll, "DAL"));
                AssertTrue(References(bll, "SERVICE"));
                AssertFalse(References(bll, "GUI"));
            });

            Run("Hash VerifyPassword", () =>
            {
                string hash = SV_Hash.HashPassword("ClaveSegura123!");
                AssertTrue(SV_Hash.VerifyPassword("ClaveSegura123!", hash));
                AssertFalse(SV_Hash.VerifyPassword("otra", hash));
            });

            Run("DVH estable", () =>
            {
                int a = SV_DigitoVerificadorHorizontal.CalcularDV("adminfalsehash");
                int b = SV_DigitoVerificadorHorizontal.CalcularDV("adminfalsehash");
                AssertTrue(a == b);
                AssertTrue(SV_DigitoVerificadorHorizontal.VerificarDV("adminfalsehash", a));
                AssertFalse(SV_DigitoVerificadorHorizontal.VerificarDV("adminXfalsehash", a));
            });

            Run("DVV detecta posición", () =>
            {
                int dv1 = SV_DigitoVerificadorVertical.CalcularDVAtributo("valor", 1);
                int dv2 = SV_DigitoVerificadorVertical.CalcularDVAtributo("valor", 2);
                AssertTrue(dv1 != dv2);
            });

            Run("Encriptacion AES round-trip", () =>
            {
                string original = "UPDATE USUARIO SET BLOQUEADO=0 WHERE ID_USUARIO=1;";
                string enc = SV_Encriptacion.Encriptar(original);
                AssertTrue(SV_Encriptacion.EstaEncriptado(enc));
                AssertTrue(SV_Encriptacion.Desencriptar(enc) == original);
            });

            Console.WriteLine();
            Console.WriteLine($"Resultado: {_passed} OK, {_failed} FAIL");
            Environment.ExitCode = _failed == 0 ? 0 : 1;
        }

        private static void Run(string name, Action action)
        {
            try
            {
                action();
                _passed++;
                Console.WriteLine($"[PASS] {name}");
            }
            catch (Exception ex)
            {
                _failed++;
                Console.WriteLine($"[FAIL] {name}: {ex.Message}");
            }
        }

        private static Assembly Load(string name)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dll = Path.Combine(baseDir, name + ".dll");
            string exe = Path.Combine(baseDir, name + ".exe");
            string path = File.Exists(dll) ? dll : exe;
            if (!File.Exists(path))
                throw new FileNotFoundException($"No se encontró {name}.dll/.exe en {baseDir}");
            return Assembly.LoadFrom(path);
        }

        private static bool References(Assembly from, string assemblyName)
        {
            return from.GetReferencedAssemblies().Any(a =>
                a.Name.Equals(assemblyName, StringComparison.OrdinalIgnoreCase));
        }

        private static void AssertTrue(bool condition)
        {
            if (!condition) throw new Exception("Se esperaba true");
        }

        private static void AssertFalse(bool condition)
        {
            if (condition) throw new Exception("Se esperaba false");
        }
    }
}
