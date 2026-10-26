using System;
using System.Collections.Generic;
using System.Data;
using BE;
using DAL;

namespace BLL
{
    // N01: indicadores de gestión, red de puntos verdes (CU04, CU05) y tarifas de comisión y conversión (CU06).
    public class BLL_GESTION
    {
        private readonly MP_GESTION dal = new MP_GESTION();
        private readonly MP_RECICLAJE dalReciclaje = new MP_RECICLAJE();
        private readonly MP_CATALOGO dalCatalogo = new MP_CATALOGO();
        private readonly BLL_BITACORA bllBitacora = new BLL_BITACORA();

        public DataTable ListarMateriales()
        {
            return dalCatalogo.ListarMateriales();
        }

        public DataTable ListarPuntosVerdes()
        {
            return dalReciclaje.ListarPuntosVerdes();
        }

        public DataTable ListarMaterialesAceptados(int idPuntoVerde)
        {
            return dalReciclaje.ListarMaterialesAceptados(idPuntoVerde);
        }

        public DataTable ListarTarifas()
        {
            return dal.ListarTarifasVigentes();
        }

        public DataTable ListarConversiones()
        {
            return dal.ListarConversionesVigentes();
        }

        // CU04: arma los indicadores del período (formato yyyy-MM) y deja registrado el informe.
        public DataTable ConsultarIndicadores(BE_USUARIO usuario, string periodo)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_VER_INDICADORES");
            if (usuario == null) throw new ArgumentException("Debe iniciar sesión.");
            if (!System.Text.RegularExpressions.Regex.IsMatch(periodo ?? "", @"^\d{4}-\d{2}$"))
                throw new ArgumentException("El período debe tener el formato aaaa-mm.");

            DataTable resultado = new DataTable();
            resultado.Columns.Add("Indicador", typeof(string));
            resultado.Columns.Add("Valor", typeof(string));

            foreach (DataRow fila in dal.IndicadorAcopioPorMaterial(periodo).Rows)
                resultado.Rows.Add($"Acopio de {fila["Material"]} (kg)", Convert.ToDecimal(fila["Kg"]).ToString("0.00"));

            DataTable ventas = dal.IndicadorVentas(periodo);
            if (ventas.Rows.Count > 0)
            {
                resultado.Rows.Add("Órdenes de venta", ventas.Rows[0]["Ordenes"].ToString());
                resultado.Rows.Add("Monto bruto vendido", Convert.ToDecimal(ventas.Rows[0]["MontoBruto"]).ToString("0.00"));
                resultado.Rows.Add("Comisiones retenidas", Convert.ToDecimal(ventas.Rows[0]["Comisiones"]).ToString("0.00"));
            }

            resultado.Rows.Add("Costo de canjes", dal.IndicadorCostoCanjes(periodo).ToString("0.00"));

            int idInforme = dal.AltaInforme(usuario.ID, "INDICADORES_MENSUALES", periodo);
            bllBitacora.IngresarRegistro(usuario, $"Generó el informe de indicadores {idInforme} del período {periodo}.");
            return resultado;
        }

        // CU05: alta o modificación de un punto verde con sus materiales aceptados.
        // Solo se guarda si el referente de cooperativas validó la locación.
        public int GuardarPuntoVerde(BE_USUARIO usuario, BE_PuntoVerde pv, List<int> materiales, bool validadoPorReferente)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_GESTIONAR_PUNTOS_VERDES");
            if (usuario == null) throw new ArgumentException("Debe iniciar sesión.");
            if (string.IsNullOrWhiteSpace(pv.Nombre)) throw new ArgumentException("Ingrese el nombre del punto verde.");
            if (string.IsNullOrWhiteSpace(pv.Direccion)) throw new ArgumentException("Ingrese la dirección.");
            if (!validadoPorReferente)
                throw new InvalidOperationException("La locación debe ser validada por el referente de cooperativas.");

            bool esAlta = pv.ID == 0;
            int id = pv.ID;
            if (esAlta)
                id = dalReciclaje.AltaPuntoVerde(pv);
            else
                dalReciclaje.ModificarPuntoVerde(pv);

            pv.ID = id;
            dalReciclaje.GuardarMaterialesAceptados(id, materiales ?? new List<int>());

            string tipo = esAlta ? "ALTA_PUNTO_VERDE" : "MODIFICACION_PUNTO_VERDE";
            dal.AltaDecision(null, usuario.ID, id, tipo,
                             $"{pv.Nombre}: {(materiales?.Count ?? 0)} materiales aceptados, activo={pv.Activo}.");
            bllBitacora.IngresarRegistro(usuario, $"{tipo} {id} ({pv.Nombre}).");
            return id;
        }

        // CU06: nueva tarifa de comisión por categoría. Queda registrada como decisión.
        public int ProponerTarifa(BE_USUARIO usuario, string categoria, decimal porcentaje)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_GESTIONAR_TARIFAS");
            if (usuario == null) throw new ArgumentException("Debe iniciar sesión.");
            if (string.IsNullOrWhiteSpace(categoria)) throw new ArgumentException("Ingrese la categoría.");
            if (porcentaje <= 0 || porcentaje > 50) throw new ArgumentException("La comisión debe estar entre 0,01% y 50%.");

            int idDecision = dal.AltaDecision(null, usuario.ID, null, "CAMBIO_TARIFA",
                                              $"Comisión de {categoria} fijada en {porcentaje}%.");
            int idTarifa = dal.AltaTarifa(idDecision, categoria, porcentaje);
            bllBitacora.IngresarRegistro(usuario, $"Fijó la comisión de {categoria} en {porcentaje}% (tarifa {idTarifa}).");
            return idTarifa;
        }

        // CU06: nueva tabla de conversión de Eco-Puntos por material. Queda registrada como decisión.
        public int ProponerConversion(BE_USUARIO usuario, int idMaterial, decimal puntosPorKg)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_GESTIONAR_TARIFAS");
            if (usuario == null) throw new ArgumentException("Debe iniciar sesión.");
            if (idMaterial <= 0) throw new ArgumentException("Seleccione un material.");
            if (puntosPorKg <= 0) throw new ArgumentException("Los puntos por kg deben ser mayores a cero.");

            int idDecision = dal.AltaDecision(null, usuario.ID, null, "CAMBIO_CONVERSION",
                                              $"Material {idMaterial}: {puntosPorKg} puntos por kg.");
            int idConversion = dal.AltaConversion(idDecision, idMaterial, puntosPorKg);
            bllBitacora.IngresarRegistro(usuario, $"Fijó {puntosPorKg} puntos por kg para el material {idMaterial} (conversión {idConversion}).");
            return idConversion;
        }
    }
}
