using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GUI
{
    // Ayuda en línea (D02): explica cada funcionalidad, agrupada como el menú (Reciclar, Tienda, Mi cuenta, Gestión).
    public partial class FormAyuda : Form
    {
        private static readonly List<KeyValuePair<string, string>> Temas = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Primeros pasos",
                "Inicie sesión con su usuario. En la barra superior aparecen los grupos Reciclar, Tienda, Mi cuenta y Gestión, " +
                "y dentro de cada uno solo las opciones que su rol puede usar. Si no ve una opción esperada, consulte con el " +
                "administrador qué rol tiene asignado.\r\n\r\n" +
                "Roles: CIUDADANO (declarar, canjear, comprar, vender, reclamar), OPERADOR_PUNTO_VERDE (registrar recepciones), " +
                "GERENCIA (indicadores, puntos verdes, tarifas, materiales y guías) y AREA_COMERCIAL (verificaciones, productos, " +
                "liquidaciones, reclamos y beneficios)."),
            new KeyValuePair<string, string>("Mi perfil",
                "Cargue su fecha de nacimiento y su ubicación (latitud y longitud). Debe tener al menos 16 años para usar la plataforma " +
                "y 18 para vender. La ubicación se usa para sugerirle el punto verde más cercano."),
            new KeyValuePair<string, string>("Declarar material",
                "Elija el material y la cantidad en kilogramos y presione «Declarar». El sistema le muestra una guía para reutilizarlo en casa " +
                "y el punto verde más cercano que lo acepta.\r\n\r\n" +
                "«Reutilizar en casa» confirma la acción y acredita Eco-Puntos. «Entregar en punto verde» deja la declaración pendiente " +
                "hasta que el operador del punto verde registre el peso."),
            new KeyValuePair<string, string>("Materiales que sigo",
                "Marque los materiales que sigue y presione «Guardar suscripciones». Abajo verá las guías aprobadas de esos materiales; " +
                "al seleccionar una, sus pasos aparecen en el cuadro inferior."),
            new KeyValuePair<string, string>("Guías de reutilización",
                "Las guías aprobadas por la gerencia para cada material. Quienes administran las guías pueden crear, modificar y eliminarlas. " +
                "Los pasos se leen en el cuadro inferior al seleccionar una guía."),
            new KeyValuePair<string, string>("Canjear Eco-Puntos",
                "Muestra su saldo y los beneficios disponibles (cupones y envíos bonificados). Seleccione un beneficio y presione " +
                "«Canjear». El canje requiere saldo suficiente y los puntos se descuentan al confirmar. Los movimientos aparecen abajo."),
            new KeyValuePair<string, string>("Publicar producto",
                "Un vendedor verificado publica productos con precio, categoría y stock. Los productos quedan pendientes hasta que el " +
                "área comercial los aprueba. Puede actualizar precio y stock de sus publicaciones."),
            new KeyValuePair<string, string>("Comprar",
                "Seleccione un producto del catálogo, la cantidad y el domicilio de entrega, y presione «Pagar pedido». El cobro se " +
                "procesa con la pasarela de pagos. Si el pago es rechazado no se genera la orden."),
            new KeyValuePair<string, string>("Verificación de vendedor",
                "Para vender debe solicitar la verificación con una referencia de documentación. Mientras tiene una solicitud pendiente " +
                "no puede enviar otra. Si la solicitud fue rechazada, puede volver a pedirla con más documentación. El área comercial " +
                "aprueba o rechaza la solicitud. Se revisa la edad con la fecha de nacimiento de su perfil."),
            new KeyValuePair<string, string>("Pedidos y liquidaciones",
                "Compradores: confirme la recepción conforme de los pedidos despachados. Vendedores: marque como despachada cada venta " +
                "y vea sus liquidaciones. Área comercial: ordene la transferencia de las liquidaciones disponibles."),
            new KeyValuePair<string, string>("Reclamos",
                "Puede reclamar un pedido despachado describiendo el motivo. Mientras el reclamo está abierto, el saldo del vendedor " +
                "queda congelado. El área comercial resuelve el reclamo como procedente o improcedente."),
            new KeyValuePair<string, string>("Estadísticas de mis ventas",
                "Elija un período y consulte cantidad de ventas, montos, comisiones, neto liquidado y pendiente, y las ventas por producto. " +
                "Puede exportar el resumen a PDF."),
            new KeyValuePair<string, string>("Recepción en punto verde",
                "Para operadores de punto verde. Seleccione el punto verde: la lista muestra las declaraciones pendientes de ese punto. " +
                "Elija una declaración, confirme el peso y presione «Registrar recepción». Si el punto verde no acepta el material, la carga se rechaza."),
            new KeyValuePair<string, string>("Indicadores",
                "Ingrese el período en formato aaaa-mm y presione «Consultar indicadores». Puede exportar el informe a PDF o guardarlo " +
                "como XML para abrirlo luego sin recalcular."),
            new KeyValuePair<string, string>("Red de puntos verdes",
                "Cargue o modifique puntos verdes con sus datos y los materiales que aceptan. Solo se guarda si el referente de " +
                "cooperativas validó la locación (marque la casilla). Cada alta o cambio queda registrado como decisión."),
            new KeyValuePair<string, string>("Tarifas y conversión",
                "Fije la comisión por categoría (entre 0,01 % y 50 %) y los puntos por kilo de cada material. Al elegir una fila de " +
                "conversiones, el material y los puntos pasan a los controles de abajo. Cada cambio queda registrado como decisión."),
            new KeyValuePair<string, string>("Materiales y beneficios",
                "Administre los materiales aceptados y los beneficios canjeables. Un material con movimientos no puede eliminarse; " +
                "en ese caso use la modificación."),
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
