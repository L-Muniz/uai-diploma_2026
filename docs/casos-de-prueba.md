# Casos de prueba: procesos de negocio (3ra entrega)

Base de pruebas: `ProyectoD`, con los scripts 01, 02 y 03 de `Proyecto/Scripts/Entrega3` ya ejecutados.
Usuarios de prueba: uno por rol (CIUDADANO, OPERADOR_PUNTO_VERDE, GERENCIA, AREA_COMERCIAL) y un ADMIN.
Para el monto de la pasarela simulada: hasta $100.000 aprueba; por encima rechaza.

## N03. Reconocimiento por reciclaje y canje (CU01, CU02, CU03)

| ID | Caso | Precondiciones | Pasos | Resultado esperado |
|---|---|---|---|---|
| CP-01 | Declarar material válido | Rol CIUDADANO, material Papel con guía y punto verde que lo acepta | Declarar 5 kg de Papel | Se registra la declaración y se muestran la guía y el punto verde |
| CP-02 | Cantidad inválida | Rol CIUDADANO | Declarar 0, texto o número negativo | Mensaje de error y no se crea declaración |
| CP-03 | Declarar sin permiso | Rol sin PROC_DECLARAR_MATERIAL (p. ej. GERENCIA) | Abrir Declarar material | El menú no lo muestra y la operación falla con mensaje de permiso |
| CP-04 | Reutilizar en casa | Declaración de CP-01 | Confirmar reutilización | Estado REUTILIZADA, se acreditan Eco-Puntos (kg × puntos por kg) |
| CP-05 | Entrega oficial | Declaración de CP-01 | Elegir entrega en punto verde | Estado PENDIENTE_ENTREGA |
| CP-06 | Punto verde más cercano | Usuario con latitud y longitud cargadas y dos puntos que aceptan el material | Declarar material | Se sugiere el punto verde de menor distancia |
| CP-07 | Material sin guía | Material sin guía cargada | Declarar material | Botón de reutilizar deshabilitado |
| CP-08 | Recepción aceptada | Declaración PENDIENTE_ENTREGA, punto verde que acepta el material | Registrar recepción con el peso | Se crea ACOPIO, declaración ENTREGADA y se acreditan puntos al ciudadano |
| CP-09 | Recepción no aceptada | Punto verde que no acepta el material | Registrar recepción | Error "El punto verde no acepta este material", sin cambios en la base |
| CP-10 | Peso inválido en recepción | Declaración pendiente | Peso 0 o negativo | Error de validación |
| CP-11 | Canje con saldo suficiente | Ciudadano con saldo ≥ puntos requeridos | Canjear beneficio | Canje APROBADO, saldo descontado y movimiento CANJE negativo |
| CP-12 | Canje con saldo insuficiente | Saldo menor a puntos requeridos | Canjear beneficio | Error de saldo insuficiente, saldo sin cambio |
| CP-13 | Canje de beneficio inactivo | Beneficio desactivado | Canjear | Error "beneficio no disponible" |

## N01. Red de puntos verdes, indicadores y tarifas (CU04, CU05, CU06)

| ID | Caso | Precondiciones | Pasos | Resultado esperado |
|---|---|---|---|---|
| CP-14 | Indicadores de un período | Hay acopios, ventas y canjes en el período | Consultar aaaa-mm | Filas de acopio por material, ventas, comisiones y costo de canjes; se guarda el informe |
| CP-15 | Período con formato inválido | Rol GERENCIA | Consultar "2026/10" | Error de formato |
| CP-16 | Período sin datos | Rol GERENCIA | Consultar un mes vacío | Filas con valor 0, sin error |
| CP-17 | Alta de punto verde sin validación | Rol GERENCIA | Guardar sin marcar referente validó | Error y no se guarda |
| CP-18 | Alta de punto verde validado | Rol GERENCIA | Completar datos, materiales y marcar validado | Punto creado, materiales asignados y DECISION ALTA_PUNTO_VERDE registrada |
| CP-19 | Modificar materiales aceptados | Punto verde existente | Cambiar la lista de materiales | La lista anterior se reemplaza por la nueva |
| CP-20 | Comisión fuera de rango | Rol GERENCIA | Fijar 0 % o 60 % | Error de rango |
| CP-21 | Nueva comisión vigente | Rol GERENCIA | Fijar 15 % en categoría General | Tarifa vigente 15 %, sin borrar la anterior |
| CP-22 | Conversión de Eco-Puntos | Rol GERENCIA | Fijar 12 puntos por kg en Papel | La siguiente acreditación usa 12 puntos por kg |

## N02. Tienda sustentable, pagos, liquidación y reclamos (CU07 a CU11)

| ID | Caso | Precondiciones | Pasos | Resultado esperado |
|---|---|---|---|---|
| CP-23 | Solicitar verificación sin fecha de nacimiento | Perfil sin fecha | Solicitar verificación | Error: cargar fecha en Mi perfil |
| CP-24 | Solicitar verificación siendo menor de 18 | Fecha que da menos de 18 años | Solicitar | Error de edad mínima |
| CP-25 | Aprobar y rechazar vendedor | Solicitud PENDIENTE | Aprobar; en otra prueba, rechazar | Último estado APROBADA o RECHAZADA |
| CP-26 | Publicar sin estar verificado | Usuario sin APROBADA | Publicar producto | Error: debe estar verificado |
| CP-27 | Publicar y aprobar producto | Vendedor verificado | Publicar; luego aprobar como AREA_COMERCIAL | Producto PENDIENTE, luego ACTIVO y visible en el catálogo |
| CP-28 | Actualizar precio y stock propio | Producto propio ACTIVO | Cambiar precio y stock | Valores actualizados |
| CP-29 | Modificar producto ajeno | Producto de otro vendedor | Intentar modificar | Error, sin cambios |
| CP-30 | Compra exitosa | Producto ACTIVO con stock, comprador distinto del vendedor | Pagar pedido | Orden PAGADA_PENDIENTE_DESPACHO, stock descontado, comisión y liquidación RETENIDA con neto = monto − comisión |
| CP-31 | Pago rechazado | Monto mayor a $100.000 | Pagar | Error de pago rechazado, sin orden ni cambio de stock |
| CP-32 | Comprar propio producto | Vendedor sobre su publicación | Pagar | Error "no puede comprar sus propios productos" |
| CP-33 | Compra sin stock suficiente | Stock menor a cantidad pedida | Pagar | Error de stock insuficiente |
| CP-34 | Despachar por el dueño | Orden PAGADA_PENDIENTE_DESPACHO | Marcar despachada con el vendedor dueño | Estado DESPACHADA |
| CP-35 | Despachar por otro vendedor | Misma orden | Marcar despachada con otro usuario | Error, sin cambios |
| CP-36 | Confirmar recepción | Orden DESPACHADA del comprador | Confirmar recepción | Orden RECIBIDA y liquidación DISPONIBLE |
| CP-37 | Liquidar saldo | Liquidación DISPONIBLE, rol AREA_COMERCIAL | Ordenar transferencia | Liquidación PAGADA con fecha de pago |
| CP-38 | Liquidar saldo retenido | Liquidación RETENIDA | Ordenar transferencia | Error, no se paga |
| CP-39 | Reclamo sobre orden despachada | Orden DESPACHADA | Presentar reclamo | Orden EN_RECLAMO, liquidación CONGELADA |
| CP-40 | Reclamo sobre orden no despachada | Orden PAGADA_PENDIENTE_DESPACHO | Presentar reclamo | Error, sin cambios |
| CP-41 | Resolver reclamo procedente | Reclamo ABIERTO | Resolver como procedente | Orden DEVUELTA, stock restituido, liquidación CANCELADA |
| CP-42 | Resolver reclamo improcedente | Reclamo ABIERTO | Resolver como improcedente | Orden DESPACHADA, liquidación RETENIDA |

## Perfil, suscripciones, estadísticas y control de acceso

| ID | Caso | Precondiciones | Pasos | Resultado esperado |
|---|---|---|---|---|
| CP-43 | Fecha de nacimiento menor de 16 | Cualquier usuario | Guardar perfil con fecha que da 15 años | Error de edad mínima |
| CP-44 | Fecha futura | Cualquier usuario | Guardar fecha de mañana | Error |
| CP-45 | Latitud sin longitud | Cualquier usuario | Cargar solo latitud | Error |
| CP-46 | Suscribirse a materiales | Usuario con rol que tenga PROC_SUSCRIBIRSE_MATERIALES | Marcar Papel y Vidrio, guardar | Se listan las guías de esos materiales |
| CP-47 | Estadísticas de ventas | Vendedor con ventas en el período | Consultar período | Resumen y ventas por producto con los valores del período |
| CP-48 | Período invertido | Cualquier vendedor | Desde mayor que hasta | Error de período |
| CP-49 | Menú según rol | Usuarios de cada rol | Revisar el menú Procesos de negocio | Cada rol ve solo sus opciones; ADMIN ve todas |
| CP-50 | Acceso denegado en BLL | Rol sin permiso | Ejecutar una operación desde el código | UnauthorizedAccessException con el código del permiso |

## Pendientes del plan de entregas (no son casos de prueba todavía)

Según el plan de entregas, para la 3ra entrega también se evalúan: **A01 instalador** (versión final, incluye casos de instancia inexistente, motor no instalado y servicio SQL detenido), **A02 informe y exportación a PDF** con librería de terceros, **A02 serialización**, **D01 manual de instalación**, **D02 ayuda en línea**, **D03 material de apoyo al usuario**, y el balanceo entre diagramas, DER, casos de uso y secuencias con la implementación. Esos casos de prueba se agregan cuando los implementemos.
