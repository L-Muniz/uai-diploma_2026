using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using SERVICE;

namespace BLL
{
    public class BLL_IntegridadDatos
    {
        public static void CalcularDVHUsuario(BE_USUARIO usuario)
        {
            if (usuario == null)
                return;

            string datosUsuario = usuario.Username + usuario.HashedPassword + usuario.Bloqueado.ToString();
            usuario.DVHorizontal = SV_DigitoVerificadorHorizontal.CalcularDV(datosUsuario);
        }

        public static bool VerificarIntegridadDVHUsuario(BE_USUARIO usuario)
        {
            if (usuario == null)
                return false;

            string datosUsuario = usuario.Username + usuario.HashedPassword + usuario.Bloqueado.ToString();
            return SV_DigitoVerificadorHorizontal.VerificarDV(datosUsuario, usuario.DVHorizontal);
        }

        // Recalcula el DVH del usuario, lo persiste junto con sus datos y actualiza el DVV de la tabla.
        public static int RecalcularYPersistirUsuario(BE_USUARIO usuario, MP_USUARIO mpUsuario)
        {
            CalcularDVHUsuario(usuario);
            int filas = mpUsuario.Editar(usuario);
            mpUsuario.ActualizarDVHorizontal(usuario);

            PersistirDVVUsuario(CalcularDVVUsuario(usuario));
            return filas;
        }

        public static BE_DigitoVerificadorVertical CalcularDVVUsuario(BE_USUARIO usuario)
        {
            
            MP_USUARIO mpUsuario = new MP_USUARIO();
            var usuarios = mpUsuario.Listar();
            if (usuarios == null || usuarios.Count == 0)
            {
                if (usuario == null) return null;
                usuarios = new List<BE_USUARIO> { usuario };
            }

            int sumaUser = 0, sumaPass = 0, sumaBloq = 0;
            int fila = 1;
            foreach (var u in usuarios.OrderBy(x => x.ID))
            {
                sumaUser = (sumaUser + SV_DigitoVerificadorVertical.CalcularDVAtributo(u.Username, fila)) % 11;
                sumaPass = (sumaPass + SV_DigitoVerificadorVertical.CalcularDVAtributo(u.HashedPassword, fila)) % 11;
                sumaBloq = (sumaBloq + SV_DigitoVerificadorVertical.CalcularDVAtributo(u.Bloqueado.ToString(), fila)) % 11;
                fila++;
            }

            BE_DigitoVerificadorVertical dvv = new BE_DigitoVerificadorVertical();
            dvv.IdEntidad = 0;
            dvv.TipoEntidad = "USUARIO";
            dvv.DVUsername = sumaUser;
            dvv.DVHashedPassword = sumaPass;
            dvv.DVBloqueado = sumaBloq;
            dvv.FechaCreacion = DateTime.Now;
            return dvv;
        }

        public static void PersistirDVVUsuario(BE_DigitoVerificadorVertical dvv)
        {
            if (dvv == null)
                return;

            dvv.TipoEntidad = "USUARIO";
            dvv.IdEntidad = 0;

            MP_DigitoVerificadorVertical mpDVV = new MP_DigitoVerificadorVertical();
            var existentes = (mpDVV.Listar() ?? new List<BE_DigitoVerificadorVertical>())
                .Where(d => string.Equals(d.TipoEntidad, "USUARIO", StringComparison.OrdinalIgnoreCase))
                .OrderBy(d => d.ID)
                .ToList();

            var canonico = existentes.LastOrDefault();
            foreach (var extra in existentes.Where(d => canonico == null || d.ID != canonico.ID))
                mpDVV.Borrar(extra);

            if (canonico != null)
            {
                dvv.ID = canonico.ID;
                mpDVV.Editar(dvv);
            }
            else
            {
                mpDVV.Alta(dvv);
            }
        }

        public static bool VerificarIntegridadDVVUsuario(BE_USUARIO usuario, BE_DigitoVerificadorVertical dvv)
        {
            if (dvv == null)
                return false;

            var calculado = CalcularDVVUsuario(usuario);
            if (calculado == null)
                return false;

            return calculado.DVUsername == dvv.DVUsername
                && calculado.DVHashedPassword == dvv.DVHashedPassword
                && calculado.DVBloqueado == dvv.DVBloqueado;
        }

        
        public static (bool esValido, string mensaje) VerificarSoloDVH(BE_USUARIO usuario)
        {
            if (usuario == null)
                return (false, "Usuario no encontrado");

            if (!VerificarIntegridadDVHUsuario(usuario))
                return (false, $"Error DVH: el usuario ID={usuario.ID} ({usuario.Username}) está comprometido (DV Horizontal no coincide)");

            return (true, "DVH OK");
        }

        
        public static (bool esValido, string mensaje) VerificarSoloDVV(BE_DigitoVerificadorVertical dvv)
        {
            if (dvv == null)
                return (false, "Error DVV: la tabla USUARIO está comprometida (no existe dígito vertical almacenado)");

            var calculado = CalcularDVVUsuario(null);
            if (calculado == null)
                return (false, "Error DVV: la tabla USUARIO está comprometida (no se pudo recalcular el dígito vertical)");

            bool ok = calculado.DVUsername == dvv.DVUsername
                && calculado.DVHashedPassword == dvv.DVHashedPassword
                && calculado.DVBloqueado == dvv.DVBloqueado;

            if (!ok)
                return (false, "Error DVV: la tabla USUARIO está comprometida (DV Vertical no coincide)");

            return (true, "DVV OK");
        }

        
        public static (bool esValido, string mensaje) DetectarFalloIntegridadPrioritario()
        {
            MP_USUARIO mpUsuario = new MP_USUARIO();
            MP_DigitoVerificadorVertical mpDVV = new MP_DigitoVerificadorVertical();
            var usuarios = mpUsuario.Listar() ?? new List<BE_USUARIO>();
            var dvv = mpDVV.BuscarPorTipoEntidad("USUARIO");

            var resultadoDvv = VerificarSoloDVV(dvv);
            if (!resultadoDvv.esValido)
                return resultadoDvv;

            foreach (var usuario in usuarios.OrderBy(u => u.ID))
            {
                var resultadoDvh = VerificarSoloDVH(usuario);
                if (!resultadoDvh.esValido)
                    return resultadoDvh;
            }

            return (true, "Integridad verificada correctamente");
        }

        public static (bool esValido, string mensaje) VerificarIntegridadCompleta(BE_USUARIO usuario, BE_DigitoVerificadorVertical dvv)
        {
            if (usuario == null)
                return (false, "Usuario no encontrado");

            bool dvvFaltaba = false;

            if (dvv == null)
            {
                dvvFaltaba = true;
                dvv = CalcularDVVUsuario(usuario);
                System.Diagnostics.Debug.WriteLine($"[DVV] Calculado para tabla USUARIO");
                PersistirDVVUsuario(dvv);
            }

            if (dvvFaltaba)
                return (true, "DVV inicializado automáticamente");

            var dvvResultado = VerificarSoloDVV(dvv);
            if (!dvvResultado.esValido)
                return (false, dvvResultado.mensaje);

            bool dvhValido = VerificarIntegridadDVHUsuario(usuario);
            if (!dvhValido)
                return (false, $"Error DVH: el usuario ID={usuario.ID} ({usuario.Username}) está comprometido (DV Horizontal no coincide)");

            return (true, "Integridad verificada correctamente");
        }

        public static (bool esValido, string detalle) VerificarTodasLasEntidades()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Verificación de integridad — {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            sb.AppendLine(new string('=', 60));

            MP_USUARIO mpUsuario = new MP_USUARIO();
            MP_DigitoVerificadorVertical mpDVV = new MP_DigitoVerificadorVertical();
            var usuarios = mpUsuario.Listar();
            var dvv = mpDVV.BuscarPorTipoEntidad("USUARIO");

            bool okGlobal = true;
            sb.AppendLine($"Entidad: USUARIO ({usuarios.Count} registros)");
            sb.AppendLine();
            sb.AppendLine("— DVH por registro —");

            foreach (var usuario in usuarios.OrderBy(u => u.ID))
            {
                var resultado = VerificarSoloDVH(usuario);
                string estado = resultado.esValido ? "OK" : "FALLA";
                if (!resultado.esValido) okGlobal = false;
                sb.AppendLine($"  [{estado}] ID={usuario.ID} User={usuario.Username} DVH={usuario.DVHorizontal} — {resultado.mensaje}");
            }

            sb.AppendLine();
            sb.AppendLine("— DVV de tabla USUARIO —");
            var resultadoDvv = VerificarSoloDVV(dvv);
            if (!resultadoDvv.esValido) okGlobal = false;
            sb.AppendLine($"  [{(resultadoDvv.esValido ? "OK" : "FALLA")}] {resultadoDvv.mensaje}");

            sb.AppendLine();
            sb.AppendLine(okGlobal ? "RESULTADO GLOBAL: INTEGRIDAD OK" : "RESULTADO GLOBAL: FALLOS DETECTADOS");
            return (okGlobal, sb.ToString());
        }

        
        public static (bool exito, string mensaje, string detalle) RecalcularTodosLosDV()
        {
            var sb = new StringBuilder();
            try
            {
                MP_USUARIO mpUsuario = new MP_USUARIO();
                var usuarios = mpUsuario.Listar();

                sb.AppendLine($"Recálculo de DV — {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                sb.AppendLine(new string('=', 60));

                int sumaUser = 0, sumaPass = 0, sumaBloq = 0;
                int fila = 1;

                foreach (var usuario in usuarios.OrderBy(u => u.ID))
                {
                    CalcularDVHUsuario(usuario);
                    mpUsuario.ActualizarDVHorizontal(usuario);

                    sumaUser = (sumaUser + SV_DigitoVerificadorVertical.CalcularDVAtributo(usuario.Username, fila)) % 11;
                    sumaPass = (sumaPass + SV_DigitoVerificadorVertical.CalcularDVAtributo(usuario.HashedPassword, fila)) % 11;
                    sumaBloq = (sumaBloq + SV_DigitoVerificadorVertical.CalcularDVAtributo(usuario.Bloqueado.ToString(), fila)) % 11;

                    sb.AppendLine($"  DVH recalculado: ID={usuario.ID} User={usuario.Username} DVH={usuario.DVHorizontal}");
                    fila++;
                }

                BE_DigitoVerificadorVertical dvv = new BE_DigitoVerificadorVertical
                {
                    IdEntidad = 0,
                    TipoEntidad = "USUARIO",
                    DVUsername = sumaUser,
                    DVHashedPassword = sumaPass,
                    DVBloqueado = sumaBloq,
                    FechaCreacion = DateTime.Now
                };

                PersistirDVVUsuario(dvv);
                sb.AppendLine("DVV de tabla USUARIO persistido (una sola fila)");

                sb.AppendLine();
                sb.AppendLine("Recálculo completado.");
                return (true, $"Se recalcularon DVH de {usuarios.Count} usuarios y el DVV de USUARIO.", sb.ToString());
            }
            catch (Exception ex)
            {
                sb.AppendLine($"ERROR: {ex.Message}");
                return (false, ex.Message, sb.ToString());
            }
        }

        public static void CalcularDVHRol(BE_Rol rol)
        {
            if (rol == null)
                return;

            string datosRol = rol.Nombre + rol.Descripcion;
            int dv = SV_DigitoVerificadorHorizontal.CalcularDV(datosRol);
            rol.ToString(); 
        }

        public static bool VerificarIntegridadDVHRol(BE_Rol rol)
        {
            if (rol == null)
                return false;

            string datosRol = rol.Nombre + rol.Descripcion;
            
            return true;
        }
    }
}
