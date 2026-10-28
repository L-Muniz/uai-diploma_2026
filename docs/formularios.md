# Formularios de los procesos de negocio

Todos se abren desde el menú **Procesos de negocio** (o **Ayuda en línea**, con F1). El menú muestra solo las opciones a las que el rol del usuario tiene permiso. ADMIN ve todo.

Usuarios de prueba (script 05, contraseña `Prueba2026!`): `admin_demo`, `ciudadano1`, `ciudadano2`, `operador`, `gerente`, `area_comercial`.

## Reciclaje y Eco-Puntos (N03)

| Formulario | Rol | Qué hace |
|---|---|---|
| **FormDeclararMaterial** (CU01) | CIUDADANO | Declara un material y una cantidad en kilos. Muestra la guía para reutilizarlo y el punto verde más cercano que lo acepta. Permite confirmar la reutilización (acredita Eco-Puntos) o dejar la entrega pendiente en un punto verde. Muestra el saldo y el historial de declaraciones. |
| **FormRecepcionAcopio** (CU02) | OPERADOR_PUNTO_VERDE | El operador elige su punto verde y una declaración pendiente, carga el peso y registra la recepción. Acredita los Eco-Puntos. Rechaza el material si el punto verde no lo acepta. |
| **FormCanjeEcoPuntos** (CU03) | CIUDADANO | Muestra el saldo, los beneficios activos (cupones y envíos bonificados) y los movimientos de la cuenta. Canjea un beneficio si hay saldo suficiente. |

## Gestión (N01)

| Formulario | Rol | Qué hace |
|---|---|---|
| **FormIndicadores** (CU04) | GERENCIA | Consulta los indicadores de un período (aaaa-mm): acopio por material, ventas, comisiones y costo de canjes. Permite exportar a PDF, guardar el informe en XML y abrir un XML guardado. |
| **FormPuntosVerdes** (CU05) | GERENCIA | Alta y modificación de puntos verdes (datos, horario, activo) con sus materiales aceptados. Solo guarda si se marca que el referente de cooperativas validó la locación. Cada cambio queda registrado como decisión. |
| **FormTarifas** (CU06) | GERENCIA | Fija la comisión por categoría (0,01 % a 50 %) y los puntos por kilo de cada material. Muestra las tarifas y conversiones vigentes. Cada cambio queda registrado como decisión. |

## Tienda sustentable (N02)

| Formulario | Rol | Qué hace |
|---|---|---|
| **FormVerificacionVendedor** (CU07) | CIUDADANO solicita, AREA_COMERCIAL resuelve | El ciudadano solicita la verificación para vender con una referencia de documentación. Requiere fecha de nacimiento de 18 años o más en Mi perfil. El área comercial aprueba o rechaza las solicitudes pendientes. |
| **FormPublicarProducto** (CU08) | CIUDADANO publica, AREA_COMERCIAL aprueba | El vendedor verificado publica productos con precio, categoría y stock, y actualiza precio y stock de sus publicaciones. El área comercial aprueba los productos pendientes para que aparezcan en el catálogo. |
| **FormCompraMarketplace** (CU09) | CIUDADANO | Muestra el catálogo con stock. El comprador elige cantidad y domicilio, y paga con la pasarela (simulada: aprueba hasta $100.000). Genera la orden, la comisión y la liquidación retenida del vendedor. |
| **FormLiquidacion** (CU10) | CIUDADANO (compras y ventas), AREA_COMERCIAL (liquidaciones) | Tres pestañas según el permiso: **Mis compras** (confirmar recepción conforme), **Mis ventas** (marcar despachada) y **Liquidaciones** (ordenar la transferencia del saldo disponible). |
| **FormReclamos** (CU11) | CIUDADANO presenta, AREA_COMERCIAL resuelve | El comprador reclama un pedido despachado (congela el saldo del vendedor). El área comercial resuelve el reclamo como procedente (devuelve la orden y restituye el stock) o improcedente (libera el saldo). |

## Catálogos (ABM)

| Formulario | Rol | Qué hace |
|---|---|---|
| **FormMateriales** | GERENCIA | Alta, modificación y eliminación de materiales. No se puede eliminar un material que está en uso. |
| **FormGuias** | GERENCIA | Alta, modificación y eliminación de guías de reutilización por material. No se puede eliminar una guía ya recomendada. |
| **FormBeneficios** | GERENCIA | Alta y modificación de beneficios canjeables, y activación o desactivación. |

## Cuenta del usuario

| Formulario | Rol | Qué hace |
|---|---|---|
| **FormMiPerfil** | Todos | Carga fecha de nacimiento (mínimo 16 años), localidad y ubicación (latitud y longitud). La ubicación se usa para sugerir el punto verde más cercano. |
| **FormSuscripciones** | CIUDADANO | Elige los materiales que sigue y muestra las guías de reutilización de esos materiales. |
| **FormEstadisticasVentas** | CIUDADANO (vendedor) | Estadísticas de ventas propias por período: ventas, monto bruto, comisiones, neto liquidado y pendiente, y ventas por producto. Exporta a PDF. |

## Ayuda

| Formulario | Rol | Qué hace |
|---|---|---|
| **FormAyuda** | Todos | Ayuda en línea con un tema por funcionalidad. Se abre desde el menú Ayuda o con F1. |
