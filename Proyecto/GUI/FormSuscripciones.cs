using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // Suscripción a materiales (G02.6): el usuario sigue materiales y ve las guías de los que sigue.
    public partial class FormSuscripciones : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_DATOS_USUARIO bllDatos = new BLL_DATOS_USUARIO();

        public FormSuscripciones(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            CargarMateriales();
            Refrescar();
        }

        private void CargarMateriales()
        {
            clbMateriales.Items.Clear();
            foreach (DataRow r in bllDatos.ListarSuscripciones(usuario).Rows)
            {
                var item = new UIHelper.ItemLista { ID = Convert.ToInt32(r["ID"]), Texto = r["Material"].ToString() };
                clbMateriales.Items.Add(item, Convert.ToInt32(r["Suscripto"]) == 1);
            }
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvGuias, bllDatos.ListarGuiasSuscriptas(usuario));
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> ids = clbMateriales.CheckedItems.Cast<UIHelper.ItemLista>().Select(i => i.ID).ToList();
                bllDatos.GuardarSuscripciones(usuario, ids);
                UIHelper.Informar("Suscripciones guardadas.");
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
