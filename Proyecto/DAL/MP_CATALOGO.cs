using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BE;

namespace DAL
{
    // ABM de catálogos: materiales, guías de reutilización y beneficios canjeables. Todo mediante stored procedures.
    public class MP_CATALOGO
    {
        // ---------------- materiales ----------------

        public DataTable ListarMateriales()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("MATERIAL_LISTAR");
            acc.Cerrar();
            return dt;
        }

        public int AltaMaterial(BE_Material m)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@NOMBRE", m.Nombre),
                acc.CrearParametro("@CATEGORIA", m.Categoria),
                acc.CrearParametro("@UNIDAD", m.UnidadMedida)
            };
            int id = Convert.ToInt32(acc.Escalar("MATERIAL_ALTA", p));
            acc.Cerrar();
            return id;
        }

        public void ModificarMaterial(BE_Material m)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID", m.ID),
                acc.CrearParametro("@NOMBRE", m.Nombre),
                acc.CrearParametro("@CATEGORIA", m.Categoria),
                acc.CrearParametro("@UNIDAD", m.UnidadMedida)
            };
            acc.Escribir("MATERIAL_MODIFICAR", p);
            acc.Cerrar();
        }

        public void EliminarMaterial(int id)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID", id) };
            acc.Escribir("MATERIAL_ELIMINAR", p);
            acc.Cerrar();
        }

        // ---------------- guías de reutilización ----------------

        public DataTable ListarGuias()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("GUIA_LISTAR");
            acc.Cerrar();
            return dt;
        }

        public int AltaGuia(BE_GuiaReutilizacion g)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID_MATERIAL", g.Material.ID),
                acc.CrearParametro("@TITULO", g.Titulo),
                acc.CrearParametro("@PASOS", g.Pasos),
                acc.CrearParametro("@DIFICULTAD", g.Dificultad)
            };
            int id = Convert.ToInt32(acc.Escalar("GUIA_ALTA", p));
            acc.Cerrar();
            return id;
        }

        public void ModificarGuia(BE_GuiaReutilizacion g)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID", g.ID),
                acc.CrearParametro("@ID_MATERIAL", g.Material.ID),
                acc.CrearParametro("@TITULO", g.Titulo),
                acc.CrearParametro("@PASOS", g.Pasos),
                acc.CrearParametro("@DIFICULTAD", g.Dificultad)
            };
            acc.Escribir("GUIA_MODIFICAR", p);
            acc.Cerrar();
        }

        public void EliminarGuia(int id)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter> { acc.CrearParametro("@ID", id) };
            acc.Escribir("GUIA_ELIMINAR", p);
            acc.Cerrar();
        }

        // ---------------- beneficios canjeables ----------------

        public DataTable ListarBeneficios()
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            DataTable dt = acc.Leer("BENEFICIO_LISTAR");
            acc.Cerrar();
            return dt;
        }

        public int AltaBeneficio(BE_Beneficio b)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@TIPO", b.Tipo),
                acc.CrearParametro("@DESCRIPCION", b.Descripcion),
                acc.CrearParametro("@PUNTOS", b.PuntosRequeridos),
                acc.CrearParametro("@COSTO", b.CostoEmpresa)
            };
            int id = Convert.ToInt32(acc.Escalar("BENEFICIO_ALTA", p));
            acc.Cerrar();
            return id;
        }

        public void ModificarBeneficio(BE_Beneficio b)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID", b.ID),
                acc.CrearParametro("@TIPO", b.Tipo),
                acc.CrearParametro("@DESCRIPCION", b.Descripcion),
                acc.CrearParametro("@PUNTOS", b.PuntosRequeridos),
                acc.CrearParametro("@COSTO", b.CostoEmpresa)
            };
            acc.Escribir("BENEFICIO_MODIFICAR", p);
            acc.Cerrar();
        }

        public void CambiarEstadoBeneficio(int id, bool activo)
        {
            ACCESO acc = new ACCESO();
            acc.Abrir();
            List<SqlParameter> p = new List<SqlParameter>
            {
                acc.CrearParametro("@ID", id),
                acc.CrearParametro("@ACTIVO", activo)
            };
            acc.Escribir("BENEFICIO_CAMBIAR_ESTADO", p);
            acc.Cerrar();
        }
    }
}
