using System;
using System.Data;
using BE;
using DAL;

namespace BLL
{
    // ABM de materiales, guías de reutilización y beneficios canjeables.
    public class BLL_CATALOGO
    {
        private readonly MP_CATALOGO dal = new MP_CATALOGO();
        private readonly BLL_BITACORA bllBitacora = new BLL_BITACORA();

        // ---------------- materiales ----------------

        public DataTable ListarMateriales()
        {
            return dal.ListarMateriales();
        }

        public int GuardarMaterial(BE_USUARIO usuario, BE_Material m)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_ABM_MATERIALES");
            ValidarTexto(m.Nombre, "el nombre del material", 100);
            ValidarTexto(m.Categoria, "la categoría", 50);
            if (string.IsNullOrWhiteSpace(m.UnidadMedida)) m.UnidadMedida = "kg";

            if (m.ID == 0)
            {
                m.ID = dal.AltaMaterial(m);
                bllBitacora.IngresarRegistro(usuario, $"Alta de material {m.ID} ({m.Nombre}).");
            }
            else
            {
                dal.ModificarMaterial(m);
                bllBitacora.IngresarRegistro(usuario, $"Modificó el material {m.ID} ({m.Nombre}).");
            }
            return m.ID;
        }

        public void EliminarMaterial(BE_USUARIO usuario, int id)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_ABM_MATERIALES");
            dal.EliminarMaterial(id);
            bllBitacora.IngresarRegistro(usuario, $"Eliminó el material {id}.");
        }

        // ---------------- guías de reutilización ----------------

        public DataTable ListarGuias()
        {
            return dal.ListarGuias();
        }

        public int GuardarGuia(BE_USUARIO usuario, BE_GuiaReutilizacion g)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_ABM_GUIAS");
            if (g.Material == null || g.Material.ID <= 0) throw new ArgumentException("Seleccione el material.");
            ValidarTexto(g.Titulo, "el título", 150);
            ValidarTexto(g.Pasos, "los pasos", 4000);
            if (string.IsNullOrWhiteSpace(g.Dificultad)) g.Dificultad = "Baja";

            if (g.ID == 0)
            {
                g.ID = dal.AltaGuia(g);
                bllBitacora.IngresarRegistro(usuario, $"Alta de guía {g.ID} ({g.Titulo}).");
            }
            else
            {
                dal.ModificarGuia(g);
                bllBitacora.IngresarRegistro(usuario, $"Modificó la guía {g.ID} ({g.Titulo}).");
            }
            return g.ID;
        }

        public void EliminarGuia(BE_USUARIO usuario, int id)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_ABM_GUIAS");
            dal.EliminarGuia(id);
            bllBitacora.IngresarRegistro(usuario, $"Eliminó la guía {id}.");
        }

        // ---------------- beneficios canjeables ----------------

        public DataTable ListarBeneficios()
        {
            return dal.ListarBeneficios();
        }

        public int GuardarBeneficio(BE_USUARIO usuario, BE_Beneficio b)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_ABM_BENEFICIOS");
            ValidarTexto(b.Descripcion, "la descripción", 200);
            ValidarTexto(b.Tipo, "el tipo", 20);
            if (b.PuntosRequeridos <= 0) throw new ArgumentException("Los puntos requeridos deben ser mayores a cero.");
            if (b.CostoEmpresa < 0) throw new ArgumentException("El costo para la empresa no puede ser negativo.");

            if (b.ID == 0)
            {
                b.ID = dal.AltaBeneficio(b);
                bllBitacora.IngresarRegistro(usuario, $"Alta de beneficio {b.ID} ({b.Descripcion}).");
            }
            else
            {
                dal.ModificarBeneficio(b);
                bllBitacora.IngresarRegistro(usuario, $"Modificó el beneficio {b.ID} ({b.Descripcion}).");
            }
            return b.ID;
        }

        public void CambiarEstadoBeneficio(BE_USUARIO usuario, int id, bool activo)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_ABM_BENEFICIOS");
            dal.CambiarEstadoBeneficio(id, activo);
            bllBitacora.IngresarRegistro(usuario, $"Beneficio {id} {(activo ? "activado" : "desactivado")}.");
        }

        private static void ValidarTexto(string valor, string campo, int maximo)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException($"Ingrese {campo}.");
            if (valor.Trim().Length > maximo)
                throw new ArgumentException($"{char.ToUpper(campo[0]) + campo.Substring(1)} no puede superar {maximo} caracteres.");
        }
    }
}
