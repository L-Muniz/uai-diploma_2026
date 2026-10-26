using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BE;
using DAL;

namespace BLL
{
    // CU01 (declarar material y recibir recomendación) y CU02 (registrar recepción en punto verde).
    public class BLL_RECICLAJE
    {
        private readonly MP_RECICLAJE dal = new MP_RECICLAJE();
        private readonly MP_CATALOGO dalCatalogo = new MP_CATALOGO();
        private readonly MP_GESTION dalGestion = new MP_GESTION();
        private readonly BLL_ECOPUNTOS bllEcoPuntos = new BLL_ECOPUNTOS();
        private readonly BLL_BITACORA bllBitacora = new BLL_BITACORA();

        public class OpcionesRecomendacion
        {
            public int IDDeclaracion { get; set; }
            public int IDMaterial { get; set; }
            public string Material { get; set; }
            public decimal Cantidad { get; set; }
            public int? IDGuia { get; set; }
            public string TituloGuia { get; set; }
            public string PasosGuia { get; set; }
            public int? IDPuntoVerde { get; set; }
            public string NombrePuntoVerde { get; set; }
            public string DireccionPuntoVerde { get; set; }
        }

        public DataTable ListarMateriales()
        {
            return dalCatalogo.ListarMateriales();
        }

        public DataTable ListarPuntosVerdes()
        {
            return dal.ListarPuntosVerdes();
        }

        public DataTable ListarDeclaracionesDeUsuario(int idUsuario)
        {
            return dal.ListarDeclaracionesDeUsuario(idUsuario);
        }

        public DataTable ListarDeclaracionesPendientes()
        {
            return dal.ListarDeclaracionesPendientes();
        }

        // CU01 paso 1-4: valida la declaración, la registra y devuelve la mejor guía y el punto verde más cercano disponibles.
        public OpcionesRecomendacion DeclararMaterial(BE_USUARIO usuario, int idMaterial, decimal cantidad)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_DECLARAR_MATERIAL");
            if (usuario == null) throw new ArgumentException("Debe iniciar sesión.");
            if (idMaterial <= 0) throw new ArgumentException("Seleccione un material.");
            if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");

            string nombreMaterial = dalCatalogo.ListarMateriales().AsEnumerable()
                .Where(r => Convert.ToInt32(r["ID"]) == idMaterial)
                .Select(r => r["Nombre"].ToString())
                .FirstOrDefault();

            int idDeclaracion = dal.AltaDeclaracion(usuario.ID, idMaterial, cantidad);
            bllBitacora.IngresarRegistro(usuario, $"Declaró {cantidad} kg de {nombreMaterial} (declaración {idDeclaracion}).");

            var opciones = new OpcionesRecomendacion
            {
                IDDeclaracion = idDeclaracion,
                IDMaterial = idMaterial,
                Material = nombreMaterial,
                Cantidad = cantidad
            };

            DataTable guias = dal.ListarGuiasPorMaterial(idMaterial);
            if (guias.Rows.Count > 0)
            {
                opciones.IDGuia = Convert.ToInt32(guias.Rows[0]["ID"]);
                opciones.TituloGuia = guias.Rows[0]["Titulo"].ToString();
                opciones.PasosGuia = guias.Rows[0]["Pasos"].ToString();
            }

            // Punto verde más cercano a la ubicación del usuario; sin ubicación, el primero que acepta el material.
            DataTable puntos = dal.ListarPuntosVerdesPorMaterial(idMaterial);
            DataRow mejor = MasCercano(usuario, puntos);
            if (mejor != null)
            {
                opciones.IDPuntoVerde = Convert.ToInt32(mejor["ID"]);
                opciones.NombrePuntoVerde = mejor["Nombre"].ToString();
                opciones.DireccionPuntoVerde = mejor["Direccion"].ToString();
            }

            return opciones;
        }

        // CU01 paso 5-8: el usuario reutiliza el material en casa. Se acreditan los Eco-Puntos al confirmar.
        public int ConfirmarReutilizacion(BE_USUARIO usuario, OpcionesRecomendacion opciones)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_DECLARAR_MATERIAL");
            if (opciones.IDGuia == null)
                throw new InvalidOperationException("No hay una guía de reutilización para este material.");

            dal.GuardarRecomendacion(opciones.IDDeclaracion, opciones.IDGuia, null, "REUTILIZAR", "REUTILIZADA");
            int puntos = CalcularPuntos(opciones.IDMaterial, opciones.Cantidad);
            int saldo = bllEcoPuntos.Acreditar(usuario.ID, puntos, "RECICLAJE",
                                               $"Reutilización de {opciones.Cantidad} kg de {opciones.Material}");
            bllBitacora.IngresarRegistro(usuario, $"Confirmó reutilización de declaración {opciones.IDDeclaracion}: +{puntos} Eco-Puntos.");
            return saldo;
        }

        // CU01 flujo alternativo 2: el usuario elige entrega oficial. Queda pendiente hasta que el punto verde la registre (CU02).
        public void ElegirEntregaOficial(BE_USUARIO usuario, OpcionesRecomendacion opciones)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_DECLARAR_MATERIAL");
            if (opciones.IDPuntoVerde == null)
                throw new InvalidOperationException("No hay un punto verde disponible para este material.");

            dal.GuardarRecomendacion(opciones.IDDeclaracion, opciones.IDGuia, opciones.IDPuntoVerde, "ENTREGA", "PENDIENTE_ENTREGA");
            bllBitacora.IngresarRegistro(usuario, $"Eligió entrega oficial en {opciones.NombrePuntoVerde} (declaración {opciones.IDDeclaracion}).");
        }

        public DataTable ListarMaterialesAceptados(int idPuntoVerde)
        {
            return dal.ListarMaterialesAceptados(idPuntoVerde);
        }

        // CU02: el operador del punto verde registra el peso de la entrega. Acredita los Eco-Puntos al ciudadano.
        public int RegistrarRecepcion(BE_USUARIO operador, int idPuntoVerde, int? idDeclaracion, int idUsuarioCiudadano,
                                      int idMaterial, decimal pesoKg)
        {
            BLL_AUTORIZACION.Exigir(operador, "PROC_REGISTRAR_RECEPCION");
            if (pesoKg <= 0) throw new ArgumentException("El peso debe ser mayor a cero.");

            bool acepta = dal.ListarMaterialesAceptados(idPuntoVerde).AsEnumerable()
                .Any(r => Convert.ToInt32(r["ID"]) == idMaterial);
            if (!acepta)
                throw new InvalidOperationException("El punto verde no acepta este material. Se rechaza la carga.");

            int idAcopio = dal.RegistrarAcopio(idDeclaracion, idPuntoVerde, idMaterial, pesoKg);
            int puntos = CalcularPuntos(idMaterial, pesoKg);
            bllEcoPuntos.Acreditar(idUsuarioCiudadano, puntos, "ACOPIO", $"Acopio {idAcopio}: {pesoKg} kg");
            bllBitacora.IngresarRegistro(operador, $"Registró acopio {idAcopio} de {pesoKg} kg (+{puntos} Eco-Puntos).");
            return puntos;
        }

        private DataRow MasCercano(BE_USUARIO usuario, DataTable puntos)
        {
            if (puntos.Rows.Count == 0) return null;
            BE_DatosUsuario datos = new BLL_DATOS_USUARIO().Obtener(usuario);
            if (datos.Latitud == null || datos.Longitud == null) return puntos.Rows[0];

            DataRow mejor = null;
            double mejorDistancia = double.MaxValue;
            foreach (DataRow pv in puntos.Rows)
            {
                if (pv["Latitud"] == DBNull.Value || pv["Longitud"] == DBNull.Value) continue;
                double d = BLL_DATOS_USUARIO.DistanciaKm((double)datos.Latitud.Value, (double)datos.Longitud.Value,
                                                         Convert.ToDouble(pv["Latitud"]), Convert.ToDouble(pv["Longitud"]));
                if (d < mejorDistancia)
                {
                    mejorDistancia = d;
                    mejor = pv;
                }
            }
            return mejor ?? puntos.Rows[0];
        }

        // Puntos = kilos por puntos por kg vigentes (tabla de conversión). Sin conversión cargada no se acredita nada.
        private int CalcularPuntos(int idMaterial, decimal kg)
        {
            decimal? ppk = dalGestion.ObtenerPuntosPorKgVigente(idMaterial);
            if (ppk == null) return 0;
            return (int)Math.Floor(kg * ppk.Value);
        }
    }
}
