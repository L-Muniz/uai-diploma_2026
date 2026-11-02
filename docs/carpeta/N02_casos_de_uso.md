# N02. Especificación de casos de uso

Cada caso de uso tiene un flujo principal y sus flujos alternativos. Los pasos alternan una acción del usuario y una respuesta del sistema, tal como se ve en la pantalla. Los rótulos de botones y los mensajes son los que muestra la aplicación. Lo que el sistema hace por dentro (consultas, registros de bitácora) figura en el diagrama de secuencia.

## CU01 Declarar material

**Descripción:** Permite al ciudadano declarar un material y elegir cómo reciclarlo.
**Actor principal:** Ciudadano.
**Precondiciones:** el usuario tiene sesión iniciada con el rol CIUDADANO.
**Postcondiciones:** queda registrada la declaración y su recomendación (reutilizar o entregar).

**Flujo principal**

1. Usuario: elige el material en la lista "Material" y escribe la cantidad en "Cantidad (kg)".
2. Usuario: presiona "Declarar".
3. Sistema: muestra la "Opción 1: reutilizar en casa" con la guía del material y la "Opción 2: entregar en el punto verde más cercano".
4. Usuario: presiona "Reutilizar en casa (confirmar)".
5. Sistema: muestra "Reutilización confirmada. Saldo actual: {saldo} Eco-Puntos."

**Flujos alternativos**

- 1a. El material no tiene guía cargada.
  - 1a.1 Sistema: muestra "Opción 1: reutilizar en casa (declare un material primero)." y deshabilita "Reutilizar en casa (confirmar)".
  - 1a.2 Usuario: presiona "Entregar en punto verde".
  - 1a.3 Sistema: muestra "Entrega oficial registrada. Los Eco-Puntos se acreditarán cuando el punto verde confirme el peso."
- 4a. Usuario presiona "Entregar en punto verde" en lugar de reutilizar.
  - 4a.1 Sistema: muestra "Entrega oficial registrada. Los Eco-Puntos se acreditarán cuando el punto verde confirme el peso."
- 2a. Cantidad vacía, cero o negativa.
  - 2a.1 Sistema: muestra "La cantidad debe ser mayor a cero."
- 2b. Usuario presiona "Declarar" sin elegir material.
  - 2b.1 Sistema: muestra "Seleccione un material."

## CU02 Registrar recepción de material

**Descripción:** Permite al operador de punto verde registrar el peso recibido y acreditar los Eco-Puntos.
**Actor principal:** Operador de punto verde.
**Precondiciones:** el operador tiene sesión iniciada con el rol OPERADOR_PUNTO_VERDE y existe al menos una declaración pendiente en el punto verde.
**Postcondiciones:** la declaración queda entregada y el ciudadano recibe los Eco-Puntos del peso registrado.

**Flujo principal**

1. Usuario: elige el punto verde en la lista "Punto verde".
2. Sistema: muestra en "Declaraciones pendientes de entrega" las declaraciones de ese punto verde.
3. Usuario: selecciona una declaración.
4. Sistema: muestra en "Peso pesado (kg)" el peso que declaró el ciudadano.
5. Usuario: confirma o corrige el peso y presiona "Registrar recepción".
6. Sistema: muestra "Recepción registrada. Se acreditaron {puntos} Eco-Puntos al ciudadano." y la declaración sale de la lista.

**Flujos alternativos**

- 2a. El punto verde no tiene declaraciones pendientes.
  - 2a.1 Sistema: la lista queda vacía.
- 5a. Usuario presiona "Registrar recepción" sin seleccionar una declaración.
  - 5a.1 Sistema: muestra "Seleccione una declaración pendiente."
- 5b. El punto verde no acepta el material de la declaración.
  - 5b.1 Sistema: muestra "El punto verde no acepta este material. Se rechaza la carga."
- 5c. Peso vacío, cero o no numérico.
  - 5c.1 Sistema: muestra "El peso debe ser mayor a cero." o "Peso: ingrese un número válido.".

## CU03 Canjear Eco-Puntos

**Descripción:** Permite al ciudadano canjear sus Eco-Puntos por un beneficio.
**Actor principal:** Ciudadano.
**Precondiciones:** el usuario tiene sesión iniciada y una cuenta de Eco-Puntos.
**Postcondiciones:** se descuentan los puntos del beneficio y queda registrado el canje.

**Flujo principal**

1. Usuario: abre "Canjear Eco-Puntos".
2. Sistema: muestra "Saldo: {saldo} Eco-Puntos", la lista "Beneficios disponibles" y los "Movimientos de mi cuenta".
3. Usuario: selecciona un beneficio y presiona "Canjear beneficio seleccionado".
4. Sistema: pregunta "¿Canjear \"{descripción}\"?" con los botones "Sí" y "No".
5. Usuario: presiona "Sí".
6. Sistema: muestra "Canje realizado. Su cupón o envío bonificado quedó registrado." y actualiza el saldo y los movimientos.

**Flujos alternativos**

- 3a. Usuario presiona "Canjear beneficio seleccionado" sin seleccionar un beneficio.
  - 3a.1 Sistema: muestra "Seleccione un beneficio."
- 5a. Usuario presiona "No".
  - 5a.1 Sistema: vuelve a la pantalla sin cambios.
- 5b. Saldo insuficiente.
  - 5b.1 Sistema: muestra "Saldo insuficiente de Eco-Puntos."
- 5c. El beneficio dejó de estar disponible.
  - 5c.1 Sistema: muestra "El beneficio no está disponible."

## CU04 Consultar indicadores

**Descripción:** Permite a gerencia consultar los indicadores de acopio, ventas y costo de canjes de un período.
**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** se muestran los indicadores del período y queda registrado el informe.

**Flujo principal**

1. Usuario: abre "Indicadores de acopio y ventas".
2. Sistema: muestra el campo "Período (aaaa-mm):".
3. Usuario: escribe el período y presiona "Consultar indicadores".
4. Sistema: muestra la grilla de indicadores del período (acopio por material, ventas y costo de canjes).

**Flujos alternativos**

- 3a. Período con formato incorrecto.
  - 3a.1 Sistema: muestra "El período debe tener el formato aaaa-mm."

## CU05 Exportar informe de indicadores

**Descripción:** Permite a gerencia exportar el informe de indicadores a PDF o XML.
**Actor principal:** Gerencia.
**Precondiciones:** se consultaron los indicadores de un período (CU04).
**Postcondiciones:** queda guardado el archivo PDF o XML en la ubicación elegida.

**Flujo principal (PDF)**

1. Usuario: presiona "Exportar a PDF".
2. Sistema: muestra el diálogo para guardar con el nombre "Indicadores_{período}.pdf".
3. Usuario: elige la carpeta y presiona "Guardar".
4. Sistema: muestra "Informe PDF generado."

**Flujo alternativo (XML)**

- 1a. Usuario presiona "Guardar XML".
  - 1a.1 Sistema: muestra el diálogo para guardar con el nombre "Indicadores_{período}.xml".
  - 1a.2 Usuario: elige la carpeta y presiona "Guardar".
  - 1a.3 Sistema: muestra "Informe guardado en XML."

**Flujos alternativos comunes**

- 2a. Usuario cierra el diálogo sin guardar.
  - 2a.1 Sistema: no guarda el archivo y vuelve a la pantalla.

## CU06 Gestionar puntos verdes

**Descripción:** Permite a gerencia dar de alta o modificar puntos verdes y sus materiales aceptados.
**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** el punto verde queda dado de alta o modificado, con sus materiales aceptados.

**Flujo principal**

1. Usuario: abre "Red de puntos verdes y materiales".
2. Usuario: completa "Nombre", "Dirección", "Latitud", "Longitud", "Horario" y marca los "Materiales aceptados".
3. Usuario: marca "Locación validada por el referente de cooperativas" y presiona "Guardar".
4. Sistema: muestra "Punto verde guardado (ID {id}). La decisión quedó registrada."

**Flujos alternativos**

- 2a. Usuario presiona "Nuevo" para cargar otro punto verde.
  - 2a.1 Sistema: limpia el formulario.
- 3a. Usuario presiona "Guardar" sin marcar la validación del referente.
  - 3a.1 Sistema: muestra "La locación debe ser validada por el referente de cooperativas."
- 3b. Nombre vacío.
  - 3b.1 Sistema: muestra "Ingrese el nombre del punto verde."
- 3c. Dirección vacía.
  - 3c.1 Sistema: muestra "Ingrese la dirección."

## CU07 Fijar comisión por categoría

**Descripción:** Permite a gerencia fijar la comisión por categoría de producto.
**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** la comisión de la categoría queda vigente desde la fecha de hoy.

**Flujo principal**

1. Usuario: abre "Tarifas de comisión y conversión".
2. Sistema: muestra "Comisión vigente por categoría:".
3. Usuario: escribe la "Categoría" y la "Comisión %" y presiona "Fijar comisión".
4. Sistema: muestra "Comisión fijada (tarifa {id}). Rige desde hoy y queda registrada como decisión."

**Flujos alternativos**

- 3a. Comisión menor o igual a 0 o mayor a 50.
  - 3a.1 Sistema: muestra "La comisión debe estar entre 0,01% y 50%."
- 3b. Categoría vacía.
  - 3b.1 Sistema: muestra "Ingrese la categoría."

## CU08 Fijar conversión de puntos

**Descripción:** Permite a gerencia fijar los puntos por kilo de cada material.
**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** la conversión de puntos por kilo del material queda vigente desde hoy.

**Flujo principal**

1. Usuario: abre "Tarifas de comisión y conversión".
2. Sistema: muestra la grilla "Conversión vigente de Eco-Puntos".
3. Usuario: selecciona una fila de la grilla.
4. Sistema: carga el material en "Material" y sus puntos en "Puntos por kg".
5. Usuario: corrige "Puntos por kg" y presiona "Fijar conversión".
6. Sistema: muestra "Conversión fijada (ID {id}). Queda registrada como decisión."

**Flujos alternativos**

- 5a. Puntos por kg vacíos, cero o no numéricos.
  - 5a.1 Sistema: muestra "Los puntos por kg deben ser mayores a cero." o "Puntos por kg: ingrese un número válido.".
- 5b. No hay material seleccionado.
  - 5b.1 Sistema: muestra "Seleccione un material."

## CU09 Solicitar verificación de vendedor

**Descripción:** Permite al ciudadano pedir la verificación para vender.
**Actor principal:** Ciudadano.
**Precondiciones:** el usuario tiene sesión iniciada y cargó su fecha de nacimiento en "Mi perfil".
**Postcondiciones:** queda una solicitud pendiente de revisión por el área comercial.

**Flujo principal**

1. Usuario: abre "Verificación de vendedor".
2. Sistema: muestra "Estado: {estado}" y el campo "Documentación (ref.):".
3. Usuario: escribe la referencia de la documentación y presiona "Solicitar verificación".
4. Sistema: muestra "Solicitud enviada. El área comercial la revisará."

**Flujos alternativos**

- 2a. El estado es pendiente o aprobado.
  - 2a.1 Sistema: deshabilita "Solicitar verificación".
- 3a. Referencia vacía.
  - 3a.1 Sistema: muestra "Ingrese la referencia de la documentación."
- 3b. No cargó la fecha de nacimiento.
  - 3b.1 Sistema: muestra "Cargue su fecha de nacimiento en Mi perfil para solicitar la verificación."
- 3c. Tiene menos de 18 años.
  - 3c.1 Sistema: muestra "Para vender debe tener al menos 18 años."

## CU10 Resolver verificación de vendedor

**Descripción:** Permite al área comercial aprobar o rechazar la verificación de un vendedor.
**Actor principal:** Área comercial.
**Precondiciones:** el usuario tiene sesión iniciada con el rol AREA_COMERCIAL y existe al menos una solicitud pendiente.
**Postcondiciones:** la solicitud queda aprobada o rechazada, y sale de la lista de pendientes.

**Flujo principal**

1. Usuario: abre "Verificación de vendedor".
2. Sistema: muestra "Solicitudes pendientes (área comercial):" con la lista de solicitudes.
3. Usuario: selecciona una solicitud y presiona "Aprobar seleccionada".
4. Sistema: muestra "Vendedor verificado." y quita la solicitud de la lista.

**Flujos alternativos**

- 3a. Usuario presiona "Rechazar seleccionada".
  - 3a.1 Sistema: muestra "Solicitud rechazada." y quita la solicitud de la lista.
- 3b. Usuario presiona "Aprobar seleccionada" o "Rechazar seleccionada" sin seleccionar una solicitud.
  - 3b.1 Sistema: muestra "Seleccione una solicitud."

## CU11 Publicar producto

**Descripción:** Permite a un vendedor verificado publicar un producto.
**Actor principal:** Vendedor verificado.
**Precondiciones:** el vendedor tiene el estado de verificado (CU10).
**Postcondiciones:** el producto queda publicado en estado pendiente de aprobación.

**Flujo principal**

1. Usuario: abre "Publicar producto sustentable".
2. Sistema: muestra la comisión estimada ("Comisión estimada: {porcentaje}%") y "Mis publicaciones:".
3. Usuario: completa "Nombre", "Categoría", "Descripción", "Precio" y "Stock", y presiona "Publicar".
4. Sistema: muestra "Producto {id} publicado. Queda pendiente de aprobación del área comercial."

**Flujos alternativos**

- 3a. Usuario selecciona una publicación propia de "Mis publicaciones".
  - 3a.1 Sistema: carga su nombre, categoría, precio y stock.
  - 3a.2 Usuario: modifica "Precio" o "Stock" y presiona "Actualizar precio y stock".
  - 3a.3 Sistema: muestra "Precio y stock actualizados."
- 3b. El vendedor no está verificado.
  - 3b.1 Sistema: deshabilita "Publicar".
- 3c. Nombre o categoría vacíos.
  - 3c.1 Sistema: muestra "Ingrese el nombre del producto." o "Ingrese la categoría.".
- 3d. Precio igual o menor a cero, o stock negativo.
  - 3d.1 Sistema: muestra "El precio debe ser mayor a cero." o "El stock no puede ser negativo.".

## CU12 Aprobar producto

**Descripción:** Permite al área comercial aprobar un producto pendiente para su venta.
**Actor principal:** Área comercial.
**Precondiciones:** el usuario tiene sesión iniciada con el rol AREA_COMERCIAL y existe al menos un producto pendiente.
**Postcondiciones:** el producto queda visible en el catálogo de compra.

**Flujo principal**

1. Usuario: abre "Publicar producto sustentable".
2. Sistema: muestra "Pendientes de aprobación (área comercial):" con la lista de productos.
3. Usuario: selecciona un producto y presiona "Aprobar producto seleccionado".
4. Sistema: muestra "Producto aprobado. Ya está visible en el catálogo." y quita el producto de la lista.

**Flujos alternativos**

- 3a. Usuario presiona "Aprobar producto seleccionado" sin seleccionar un producto.
  - 3a.1 Sistema: muestra "Seleccione un producto pendiente."

## CU13 Comprar producto

**Descripción:** Permite al comprador comprar un producto y pagarlo.
**Actor principal:** Comprador (ciudadano).
**Precondiciones:** el usuario tiene sesión iniciada y existe al menos un producto aprobado con stock.
**Postcondiciones:** queda registrada la orden, con el pago aprobado, el stock descontado y la liquidación del vendedor retenida.

**Flujo principal**

1. Usuario: abre "Comprar en la tienda sustentable".
2. Sistema: muestra el catálogo de productos aprobados con stock.
3. Usuario: selecciona un producto, elige "Cantidad" y escribe el "Domicilio envío".
4. Sistema: muestra el resumen "{cantidad} x {producto} = {total}".
5. Usuario: presiona "Pagar pedido".
6. Sistema: pregunta "¿Confirmar el pedido?" con el resumen, con los botones "Sí" y "No".
7. Usuario: presiona "Sí".
8. Sistema: muestra "Pago aprobado. Orden {id} registrada. Se le notificará cuando el vendedor la despache." y actualiza el catálogo.

**Flujos alternativos**

- 5a. Usuario presiona "Pagar pedido" sin seleccionar un producto.
  - 5a.1 Sistema: muestra "Seleccione un producto."
- 5b. Domicilio vacío.
  - 5b.1 Sistema: muestra "Ingrese el domicilio de entrega."
- 7a. Usuario presiona "No".
  - 7a.1 Sistema: vuelve al catálogo sin registrar el pedido.
- 7b. El cobro es rechazado.
  - 7b.1 Sistema: muestra "El pago fue rechazado: {motivo}".
- 7c. Cantidad mayor al stock disponible.
  - 7c.1 Sistema: muestra "Stock insuficiente para completar la compra."
- 7d. El producto es propio del usuario.
  - 7d.1 Sistema: muestra "No puede comprar sus propios productos."

## CU14 Despachar pedido

**Descripción:** Permite al vendedor marcar una venta como despachada.
**Actor principal:** Vendedor.
**Precondiciones:** el vendedor tiene una venta pagada (CU13).
**Postcondiciones:** la orden queda despachada y espera la confirmación del comprador.

**Flujo principal**

1. Usuario: abre "Pedidos y liquidaciones" y elige la pestaña "Mis ventas".
2. Sistema: muestra la lista de ventas del vendedor.
3. Usuario: selecciona una venta y presiona "Marcar como despachada".
4. Sistema: muestra "Orden marcada como despachada."

**Flujos alternativos**

- 3a. Usuario presiona "Marcar como despachada" sin seleccionar una venta.
  - 3a.1 Sistema: muestra "Seleccione una venta."
- 3b. La orden ya no está pendiente de despacho.
  - 3b.1 Sistema: muestra "La orden no está pendiente de despacho o no le pertenece."

## CU15 Confirmar recepción de pedido

**Descripción:** Permite al comprador confirmar la recepción conforme de un pedido.
**Actor principal:** Comprador (ciudadano).
**Precondiciones:** la orden fue despachada (CU14).
**Postcondiciones:** la orden queda recibida y la liquidación del vendedor pasa a disponible.

**Flujo principal**

1. Usuario: abre "Pedidos y liquidaciones" y elige la pestaña "Mis compras".
2. Sistema: muestra la lista de compras.
3. Usuario: selecciona una compra y presiona "Confirmar recepción conforme".
4. Sistema: muestra "Recepción confirmada. El saldo quedó disponible para liquidar."

**Flujos alternativos**

- 3a. Usuario presiona "Confirmar recepción conforme" sin seleccionar una compra.
  - 3a.1 Sistema: muestra "Seleccione una compra."
- 3b. La orden no está despachada.
  - 3b.1 Sistema: muestra "La orden no está despachada o no le pertenece."

## CU16 Liquidar vendedor

**Descripción:** Permite al área comercial ordenar la transferencia del saldo neto al vendedor.
**Actor principal:** Área comercial.
**Precondiciones:** existe una liquidación disponible (CU15).
**Postcondiciones:** la liquidación queda pagada.

**Flujo principal**

1. Usuario: abre "Pedidos y liquidaciones" y elige la pestaña "Liquidaciones".
2. Sistema: muestra las liquidaciones disponibles.
3. Usuario: selecciona una liquidación y presiona "Ordenar transferencia al vendedor".
4. Sistema: pregunta "¿Ordenar la transferencia al vendedor por el saldo neto?" con los botones "Sí" y "No".
5. Usuario: presiona "Sí".
6. Sistema: muestra "Transferencia ordenada. La liquidación quedó pagada." y quita la liquidación de la lista.

**Flujos alternativos**

- 3a. Usuario presiona "Ordenar transferencia al vendedor" sin seleccionar una liquidación.
  - 3a.1 Sistema: muestra "Seleccione una liquidación."
- 5a. Usuario presiona "No".
  - 5a.1 Sistema: vuelve a la lista sin cambios.
- 5b. La liquidación ya no está disponible.
  - 5b.1 Sistema: muestra "La liquidación no está disponible para pagar."

## CU17 Presentar reclamo

**Descripción:** Permite al comprador presentar un reclamo sobre un pedido despachado.
**Actor principal:** Comprador (ciudadano).
**Precondiciones:** el comprador tiene una orden despachada.
**Postcondiciones:** queda registrado el reclamo y la liquidación del vendedor queda congelada.

**Flujo principal**

1. Usuario: abre "Reclamos" y elige la pestaña "Presentar reclamo".
2. Sistema: muestra las órdenes del comprador.
3. Usuario: selecciona una orden, escribe el "Motivo:" y presiona "Presentar reclamo".
4. Sistema: muestra "Reclamo {id} presentado. El saldo del vendedor quedó congelado hasta su resolución."

**Flujos alternativos**

- 3a. Usuario presiona "Presentar reclamo" sin seleccionar una orden.
  - 3a.1 Sistema: muestra "Seleccione la orden a reclamar."
- 3b. Motivo vacío.
  - 3b.1 Sistema: muestra "Describa el motivo del reclamo."
- 3c. La orden no es despachada o no es propia.
  - 3c.1 Sistema: muestra "Solo puede reclamar órdenes despachadas propias."

## CU18 Resolver reclamo

**Descripción:** Permite al área comercial resolver un reclamo como procedente o improcedente.
**Actor principal:** Área comercial.
**Precondiciones:** existe al menos un reclamo abierto (CU17).
**Postcondiciones:** el reclamo queda resuelto como procedente o improcedente, y la liquidación se actualiza según la resolución.

**Flujo principal**

1. Usuario: abre "Reclamos" y elige la pestaña "Resolver reclamos".
2. Sistema: muestra la lista de reclamos abiertos.
3. Usuario: selecciona un reclamo y presiona "Reclamo procedente".
4. Sistema: muestra "Reclamo procedente: la orden se devolvió, el stock se restituyó y la liquidación se canceló." y quita el reclamo de la lista.

**Flujos alternativos**

- 3a. Usuario presiona "Reclamo improcedente".
  - 3a.1 Sistema: muestra "Reclamo improcedente: la liquidación volvió a retenida hasta la confirmación del comprador."
- 3b. Usuario presiona un botón sin seleccionar un reclamo.
  - 3b.1 Sistema: muestra "Seleccione un reclamo."
- 3c. El reclamo ya no está abierto.
  - 3c.1 Sistema: muestra "El reclamo no está abierto."
