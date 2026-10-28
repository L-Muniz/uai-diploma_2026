using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GUI
{
    // Ayuda en línea (D02): explica cada funcionalidad de los procesos de negocio.
    public partial class FormAyuda : Form
    {
        private static readonly List<KeyValuePair<string, string>> Temas = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Primeros pasos",
                "Inicie sesión con su usuario. El menú «Procesos de negocio» muestra solo las opciones que su rol puede usar. " +
                "Si no ve una opción esperada, consulte con el administrador qué rol tiene asignado.\r\n\r\n" +
                "Roles: CIUDADANO (declarar, canjear, comprar, vender, reclamar), OPERADOR_PUNTO_VERDE (registrar recepciones), " +
                "GERENCIA (indicadores, puntos verdes, tarifas, catálogos) y AREA_COMERCIAL (verificaciones, aprobaciones, liquidaciones y reclamos)."),
            new KeyValuePair<string, string>("Mi perfil",
                "Cargue su fecha de nacimiento y su ubicación (latitud y longitud). Debe tener al menos 16 años para usar la plataforma " +
                "y 18 para vender. La ubicación se usa para sugerirle el punto verde más cercano."),
            new KeyValuePair<string, string>("Declarar material",
                "Elija el material y la cantidad en kilogramos y presione «Declarar». El sistema le muestra una guía para reutilizarlo en casa " +
                "y el punto verde más cercano que lo acepta.\r\n\r\n" +
                "«Reutilizar en casa» confirma la acción y acredita Eco-Puntos. «Entregar en punto verde» deja la declaración pendiente " +
                "hasta que el operador del punto verde registre el peso."),
            new KeyValuePair<string, string>("Recepción en punto verde",
                "Para operadores de punto verde. Seleccione el punto verde, elija la declaración pendiente, confirme el peso " +
                "y presione «Registrar recepción». Si el punto verde no acepta el material, la carga se rechaza."),
            new KeyValuePair<string, string>("Canjear Eco-Puntos",
                "Muestra su saldo y los beneficios disponibles (cupones y envíos bonificados). Seleccione un beneficio y presione " +
                "«Canjear». El canje requiere saldo suficiente y los puntos se descuentan al confirmar."),
            new KeyValuePair<string, string>("Indicadores",
                "Ingrese el período en formato aaaa-mm y presione «Consultar indicadores». Puede exportar el informe a PDF o guardarlo " +
                "como XML para abrirlo luego sin recalcular."),
            new KeyValuePair<string, string>("Red de puntos verdes",
                "Cargue o modifique puntos verdes con sus datos y los materiales que aceptan. Solo se guarda si el referente de " +
                "cooperativas validó la locación (marque la casilla). Cada alta o cambio queda registrado como decisión."),
            new KeyValuePair<string, string>("Tarifas y conversión",
                "Fije la comisión por categoría (entre 0,01 % y 50 %) y los puntos por kilo de cada material. Las tarifas rigen desde hoy " +
                "y cada cambio queda registrado como decisión."),
            new KeyValuePair<string, string>("Catálogos",
                "Administre materiales, guías de reutilización y beneficios canjeables. Un material con movimientos o una guía ya " +
                "recomendada no puede eliminarse; en ese caso use la modificación."),
            new KeyValuePair<string, string>("Verificación de vendedor",
                "Para vender debe solicitar la verificación con una referencia de documentación. El área comercial aprueba o rechaza " +
                "la solicitud. Se revisa la edad con la fecha de nacimiento de su perfil."),
            new KeyValuePair<string, string>("Publicar producto",
                "Un vendedor verificado publica productos con precio, categoría y stock. Los productos quedan pendientes hasta que el " +
                "área comercial los aprueba. Puede actualizar precio y stock de sus publicaciones."),
            new KeyValuePair<string, string>("Comprar",
                "Seleccione un producto del catálogo, la cantidad y el domicilio de entrega, y presione «Pagar pedido». El cobro se " +
                "procesa con la pasarela de pagos. Si el pago es rechazado no se genera la orden."),
            new KeyValuePair<string, string>("Pedidos y liquidaciones",
                "Compradores: confirme la recepción conforme de los pedidos despachados. Vendedores: marque como despachada cada venta. " +
                "Área comercial: ordene la transferencia de las liquidaciones disponibles."),
            new KeyValuePair<string, string>("Reclamos",
                "Puede reclamar un pedido despachado describiendo el motivo. Mientras el reclamo está abierto, el saldo del vendedor " +
                "queda congelado. El área comercial resuelve el reclamo como procedente o improcedente."),
            new KeyValuePair<string, string>("Mis materiales",
                "Marque los materiales que sigue para ver sus guías de reutilización."),
            new KeyValuePair<string, string>("Estadísticas de mis ventas",
                "Elija un período y consulte cantidad de ventas, montos, comisiones, neto liquidado y pendiente, y las ventas por producto. " +
                "Puede exportar el resumen a PDF."),
        };

        public FormAyuda()
        {
            InitializeComponent();
            foreach (KeyValuePair<string, string> tema in Temas)
                lstTemas.Items.Add(tema.Key);
            if (lstTemas.Items.Count > 0)
                lstTemas.SelectedIndex = 0;
        }

        private void lstTemas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTemas.SelectedIndex < 0) return;
            txtContenido.Text = Temas[lstTemas.SelectedIndex].Value;
        }
    }
}
