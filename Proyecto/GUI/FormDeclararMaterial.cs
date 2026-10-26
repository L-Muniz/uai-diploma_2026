using System;
using System.Windows.Forms;
using BE;
using BLL;

namespace GUI
{
    // CU01: el ciudadano declara un material y recibe las dos mejores opciones de disposición.
    public partial class FormDeclararMaterial : Form
    {
        private readonly BE_USUARIO usuario;
        private readonly BLL_RECICLAJE bllReciclaje = new BLL_RECICLAJE();
        private readonly BLL_ECOPUNTOS bllEcoPuntos = new BLL_ECOPUNTOS();
        private BLL_RECICLAJE.OpcionesRecomendacion opciones;

        public FormDeclararMaterial(BE_USUARIO usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            UIHelper.CargarCombo(cboMaterial, bllReciclaje.ListarMateriales(), "Nombre", "ID");
            Refrescar();
        }

        private void Refrescar()
        {
            UIHelper.CargarGrilla(dgvDeclaraciones, bllReciclaje.ListarDeclaracionesDeUsuario(usuario.ID));
            lblSaldo.Text = $"Saldo: {bllEcoPuntos.ObtenerSaldo(usuario.ID)} Eco-Puntos";
        }

        private void btnDeclarar_Click(object sender, EventArgs e)
        {
            try
            {
                int idMaterial = Convert.ToInt32(cboMaterial.SelectedValue);
                decimal cantidad = UIHelper.LeerDecimal(txtCantidad.Text, "Cantidad");
                opciones = bllReciclaje.DeclararMaterial(usuario, idMaterial, cantidad);

                lblGuia.Text = opciones.IDGuia == null
                    ? "Opción 1: no hay guía de reutilización cargada para este material."
                    : $"Opción 1: reutilizar en casa – {opciones.TituloGuia}.\r\n{opciones.PasosGuia}";
                lblPuntoVerde.Text = opciones.IDPuntoVerde == null
                    ? "Opción 2: no hay punto verde que acepte este material."
                    : $"Opción 2: entregar en {opciones.NombrePuntoVerde} ({opciones.DireccionPuntoVerde}).";
                btnReutilizar.Enabled = opciones.IDGuia != null;
                btnEntregar.Enabled = opciones.IDPuntoVerde != null;
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnReutilizar_Click(object sender, EventArgs e)
        {
            try
            {
                int saldo = bllReciclaje.ConfirmarReutilizacion(usuario, opciones);
                UIHelper.Informar($"Reutilización confirmada. Saldo actual: {saldo} Eco-Puntos.");
                btnReutilizar.Enabled = false;
                btnEntregar.Enabled = false;
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void btnEntregar_Click(object sender, EventArgs e)
        {
            try
            {
                bllReciclaje.ElegirEntregaOficial(usuario, opciones);
                UIHelper.Informar("Entrega oficial registrada. Los Eco-Puntos se acreditarán cuando el punto verde confirme el peso.");
                btnReutilizar.Enabled = false;
                btnEntregar.Enabled = false;
                Refrescar();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }
    }
}
