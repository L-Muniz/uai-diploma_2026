using System;
using System.Data;
using BE;
using DAL;

namespace BLL
{
    // CU03 (canjear Eco-Puntos por cupón o envío bonificado) y saldo de Eco-Puntos de los usuarios.
    public class BLL_ECOPUNTOS
    {
        private readonly MP_ECOPUNTOS dal = new MP_ECOPUNTOS();
        private readonly BLL_BITACORA bllBitacora = new BLL_BITACORA();

        public int ObtenerSaldo(int idUsuario)
        {
            return dal.ObtenerSaldo(idUsuario);
        }

        public DataTable ListarMovimientos(int idUsuario)
        {
            return dal.ListarMovimientos(idUsuario);
        }

        public DataTable ListarBeneficios()
        {
            return dal.ListarBeneficiosActivos();
        }

        public DataTable ListarCanjes(int idUsuario)
        {
            return dal.ListarCanjes(idUsuario);
        }

        // Acredita (o descuenta con puntos negativos) y devuelve el nuevo saldo. Sin puntos no toca la cuenta.
        public int Acreditar(int idUsuario, int puntos, string tipo, string motivo)
        {
            if (puntos == 0) return dal.ObtenerSaldo(idUsuario);
            return dal.AcreditarPuntos(idUsuario, puntos, tipo, motivo);
        }

        // CU03: canje de un beneficio por el saldo del usuario. Devuelve el ID del canje.
        public int Canjear(BE_USUARIO usuario, int idBeneficio)
        {
            BLL_AUTORIZACION.Exigir(usuario, "PROC_CANJEAR_PUNTOS");
            if (usuario == null) throw new ArgumentException("Debe iniciar sesión.");
            if (idBeneficio <= 0) throw new ArgumentException("Seleccione un beneficio.");

            int idCanje = dal.RegistrarCanje(usuario.ID, idBeneficio);
            bllBitacora.IngresarRegistro(usuario, $"Canjeó el beneficio {idBeneficio} (canje {idCanje}).");
            return idCanje;
        }
    }
}
