using System;
using System.Data;
using BE;
using DAL;

namespace BLL
{
    // Perfil del usuario (G02.1: edad mínima 16 y ubicación), suscripciones a materiales (G02.6)
    // y utilidades de distancia usadas para sugerir el punto verde más cercano (CU01).
    public class BLL_DATOS_USUARIO
    {
        public const int EDAD_MINIMA_REGISTRO = 16;
        public const int EDAD_MINIMA_VENDEDOR = 18;

        private readonly MP_DATOS_USUARIO dal = new MP_DATOS_USUARIO();
        private readonly BLL_BITACORA bllBitacora = new BLL_BITACORA();

        public BE_DatosUsuario Obtener(BE_USUARIO usuario)
        {
            if (usuario == null) throw new ArgumentException("Debe iniciar sesión.");
            return dal.Obtener(usuario.ID);
        }

        public void Guardar(BE_USUARIO usuario, BE_DatosUsuario datos)
        {
            if (usuario == null) throw new ArgumentException("Debe iniciar sesión.");
            datos.IDUsuario = usuario.ID;
            if (datos.FechaNacimiento != null)
            {
                if (datos.FechaNacimiento.Value.Date > DateTime.Today)
                    throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
                if (EdadEn(datos.FechaNacimiento.Value) < EDAD_MINIMA_REGISTRO)
                    throw new ArgumentException($"Debe tener al menos {EDAD_MINIMA_REGISTRO} años para usar la plataforma.");
            }
            if ((datos.Latitud == null) != (datos.Longitud == null))
                throw new ArgumentException("Ingrese latitud y longitud juntas, o ninguna.");
            if (datos.Latitud != null && (datos.Latitud < -90 || datos.Latitud > 90))
                throw new ArgumentException("La latitud debe estar entre -90 y 90.");
            if (datos.Longitud != null && (datos.Longitud < -180 || datos.Longitud > 180))
                throw new ArgumentException("La longitud debe estar entre -180 y 180.");

            dal.Guardar(datos);
            bllBitacora.IngresarRegistro(usuario, "Actualizó sus datos de perfil.");
        }

        // CU07: el vendedor debe tener al menos 18 años según su fecha de nacimiento cargada en el perfil.
        public void ValidarMayorDeEdadVendedor(BE_USUARIO usuario)
        {
            BE_DatosUsuario datos = Obtener(usuario);
            if (datos.FechaNacimiento == null)
                throw new InvalidOperationException("Cargue su fecha de nacimiento en Mi perfil para solicitar la verificación.");
            if (EdadEn(datos.FechaNacimiento.Value) < EDAD_MINIMA_VENDEDOR)
                throw new InvalidOperationException($"Para vender debe tener al menos {EDAD_MINIMA_VENDEDOR} años.");
        }

        public DataTable ListarSuscripciones(BE_USUARIO usuario)
        {
            return dal.ListarSuscripciones(usuario.ID);
        }

        public void GuardarSuscripciones(BE_USUARIO usuario, System.Collections.Generic.List<int> materiales)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_SUSCRIBIRSE_MATERIALES");
            dal.GuardarSuscripciones(usuario.ID, materiales ?? new System.Collections.Generic.List<int>());
            bllBitacora.IngresarRegistro(usuario, $"Actualizó sus suscripciones a materiales ({materiales?.Count ?? 0}).");
        }

        public DataTable ListarGuiasSuscriptas(BE_USUARIO usuario)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_SUSCRIBIRSE_MATERIALES");
            return dal.ListarGuiasSuscriptas(usuario.ID);
        }

        public static int EdadEn(DateTime nacimiento)
        {
            DateTime hoy = DateTime.Today;
            int edad = hoy.Year - nacimiento.Year;
            if (nacimiento.Date > hoy.AddYears(-edad)) edad--;
            return edad;
        }

        // Distancia aproximada en kilómetros entre dos coordenadas (fórmula de Haversine).
        public static double DistanciaKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double radioTierra = 6371.0;
            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLon = (lon2 - lon1) * Math.PI / 180.0;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return 2 * radioTierra * Math.Asin(Math.Min(1, Math.Sqrt(a)));
        }
    }
}
