using System;

namespace BE
{
    public class BE_CuentaEcoPuntos
    {
        public int ID { get; set; }
        public int IDUsuario { get; set; }
        public int Saldo { get; set; }
        public int Nivel { get; set; } = 1;
    }

    public class BE_MovimientoEcoPuntos
    {
        public int ID { get; set; }
        public int IDCuenta { get; set; }
        public string Tipo { get; set; }
        public int Puntos { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
    }

    public class BE_Beneficio
    {
        public int ID { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public int PuntosRequeridos { get; set; }
        public decimal CostoEmpresa { get; set; }
        public bool Activo { get; set; } = true;

        public override string ToString()
        {
            return Descripcion;
        }
    }

    public class BE_Canje
    {
        public int ID { get; set; }
        public int IDUsuario { get; set; }
        public BE_Beneficio Beneficio { get; set; }
        public int PuntosUsados { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
    }
}
