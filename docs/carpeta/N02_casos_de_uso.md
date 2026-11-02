# N02. Especificación de casos de uso

Cada caso de uso tiene un flujo principal y sus flujos alternativos. Los pasos alternan una acción del usuario y una respuesta del sistema, tal como se ve en la pantalla. Lo que el sistema hace por dentro (consultas, registros de bitácora) figura en el diagrama de secuencia, no en el flujo.

## CU01 Declarar material

**Actor principal:** Ciudadano.
**Precondiciones:** el usuario tiene sesión iniciada con el rol CIUDADANO.
**Postcondiciones:** queda registrada la declaración y su recomendación (reutilizar o entregar).

**Flujo principal**

1. Usuario: abre "Declarar material" y elige el material en la lista.
2. Sistema: muestra el material seleccionado y habilita el campo de cantidad.
3. Usuario: escribe la cantidad en kilogramos y presiona "Declarar".
4. Sistema: muestra la opción 1 (reutilizar en casa, con la guía del material) y la opción 2 (entregar en el punto verde más cercano).
5. Usuario: presiona "Reutilizar en casa".
6. Sistema: muestra "Reutilización confirmada" y los Eco-Puntos acreditados.

**Flujos alternativos**

- 4a. El material no tiene guía cargada.
  - 4a.1 Sistema: muestra "No hay una guía de reutilización para este material" y deshabilita "Reutilizar en casa".
  - 4a.2 Usuario: presiona "Entregar en punto verde".
  - 4a.3 Sistema: muestra "Entrega registrada: pendiente de recepción en {punto verde}".
- 5a. Usuario presiona "Entregar en punto verde" (en lugar de reutilizar).
  - 5a.1 Sistema: muestra "Entrega registrada: pendiente de recepción en {punto verde}".
- 3a. Cantidad no válida (vacía, cero o negativa).
  - 3a.1 Sistema: muestra "La cantidad debe ser mayor a cero" y mantiene los datos cargados.
- 1a. Usuario presiona "Declarar" sin elegir material.
  - 1a.1 Sistema: muestra "Seleccione un material".

## CU02 Registrar recepción de material

**Actor principal:** Operador de punto verde.
**Precondiciones:** el operador tiene sesión iniciada con el rol OPERADOR_PUNTO_VERDE y existe al menos una declaración pendiente en el punto verde.
**Postcondiciones:** la declaración queda entregada y el ciudadano recibe los Eco-Puntos del peso registrado.

**Flujo principal**

1. Usuario: abre "Recepción en punto verde" y elige el punto verde en la lista.
2. Sistema: muestra las declaraciones pendientes de ese punto verde.
3. Usuario: selecciona una declaración.
4. Sistema: muestra el peso declarado por el ciudadano en el campo "Peso".
5. Usuario: confirma o corrige el peso y presiona "Registrar recepción".
6. Sistema: muestra "Recepción registrada. Se acreditaron {puntos} Eco-Puntos al ciudadano." y la declaración sale de la lista.

**Flujos alternativos**

- 3a. El punto verde no tiene declaraciones pendientes.
  - 3a.1 Sistema: la lista queda vacía y no habilita el registro.
- 5a. Usuario presiona "Registrar recepción" sin seleccionar una declaración.
  - 5a.1 Sistema: muestra "Seleccione una declaración pendiente".
- 5b. El punto verde no acepta el material de la declaración.
  - 5b.1 Sistema: muestra "El punto verde no acepta este material" y no registra la recepción.
- 5c. Peso no válido.
  - 5c.1 Sistema: muestra "Peso: ingrese un número válido" y mantiene la declaración seleccionada.

## CU03 Canjear Eco-Puntos

**Actor principal:** Ciudadano.
**Precondiciones:** el usuario tiene sesión iniciada y una cuenta de Eco-Puntos.
**Postcondiciones:** se descuentan los puntos del beneficio y queda registrado el canje.

**Flujo principal**

1. Usuario: abre "Canjear Eco-Puntos".
2. Sistema: muestra su saldo y la lista de beneficios disponibles (cupón o envío bonificado, con sus puntos requeridos).
3. Usuario: selecciona un beneficio y presiona "Canjear".
4. Sistema: pregunta "¿Canjear {beneficio}?".
5. Usuario: presiona "Sí".
6. Sistema: muestra "Canje realizado. Su cupón o envío bonificado quedó registrado." y actualiza el saldo.

**Flujos alternativos**

- 3a. Usuario presiona "Canjear" sin seleccionar un beneficio.
  - 3a.1 Sistema: muestra "Seleccione un beneficio".
- 5a. Usuario presiona "No".
  - 5a.1 Sistema: vuelve a la lista sin cambios.
- 5b. Saldo insuficiente.
  - 5b.1 Sistema: muestra "Saldo insuficiente para este beneficio".

## CU04 Consultar indicadores

**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** se muestran los indicadores del período y queda registrado el informe.

**Flujo principal**

1. Usuario: abre "Indicadores".
2. Sistema: muestra el campo "Período" con el mes actual.
3. Usuario: escribe el período (formato aaaa-mm) y presiona "Consultar indicadores".
4. Sistema: muestra la grilla con el acopio por material, las ventas y el costo de los canjes del período.

**Flujos alternativos**

- 3a. Período con formato incorrecto.
  - 3a.1 Sistema: muestra "Ingrese el período en formato aaaa-mm" y no cambia la grilla.
- 4a. No hay datos para el período.
  - 4a.1 Sistema: muestra la grilla con todos los valores en cero.

## CU05 Exportar informe de indicadores

**Actor principal:** Gerencia.
**Precondiciones:** se consultaron los indicadores de un período (CU04).
**Postcondiciones:** queda guardado el archivo PDF o XML en la ubicación elegida.

**Flujo principal (PDF)**

1. Usuario: presiona "Exportar PDF".
2. Sistema: muestra el diálogo para guardar el archivo con el nombre "Indicadores_{período}.pdf".
3. Usuario: elige la carpeta y presiona "Guardar".
4. Sistema: muestra "Informe guardado en PDF".

**Flujo alternativo (XML)**

- 1a. Usuario presiona "Exportar XML".
  - 1a.1 Sistema: muestra el diálogo para guardar el archivo "Indicadores_{período}.xml".
  - 1a.2 Usuario: elige la carpeta y presiona "Guardar".
  - 1a.3 Sistema: muestra "Informe guardado en XML".

**Flujos alternativos comunes**

- 3a. Usuario presiona "Cancelar" en el diálogo.
  - 3a.1 Sistema: cierra el diálogo y no guarda el archivo.

## CU06 Gestionar puntos verdes

**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** el punto verde queda dado de alta o modificado, con sus materiales aceptados.

**Flujo principal**

1. Usuario: abre "Red de puntos verdes".
2. Sistema: muestra la lista de puntos verdes.
3. Usuario: completa nombre, dirección, horario, marca los materiales que acepta y marca "Validado por el referente de cooperativas".
4. Usuario: presiona "Guardar".
5. Sistema: muestra "Punto verde guardado" y actualiza la lista.

**Flujos alternativos**

- 2a. Usuario elige un punto verde de la lista.
  - 2a.1 Sistema: carga sus datos y materiales en el formulario.
  - 2a.2 Usuario: modifica los datos y continúa en el paso 4.
- 4a. La casilla de validación del referente no está marcada.
  - 4a.1 Sistema: muestra "El referente debe validar la locación" y no guarda.
- 4b. Falta un dato obligatorio (nombre o dirección).
  - 4b.1 Sistema: muestra "Complete el dato obligatorio: {campo}".

## CU07 Fijar comisión por categoría

**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** la comisión de la categoría queda vigente desde la fecha de hoy.

**Flujo principal**

1. Usuario: abre "Tarifas y conversión".
2. Sistema: muestra la lista de comisiones vigentes.
3. Usuario: escribe la categoría y el porcentaje, y presiona "Fijar comisión".
4. Sistema: muestra "Comisión fijada. Rige desde hoy."

**Flujos alternativos**

- 3a. Porcentaje fuera del rango permitido (0,01 % a 50 %).
  - 3a.1 Sistema: muestra "El porcentaje debe estar entre 0,01 % y 50 %".
- 3b. Categoría vacía.
  - 3b.1 Sistema: muestra "Ingrese la categoría".

## CU08 Fijar conversión de puntos

**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** la conversión de puntos por kilo del material queda vigente desde hoy.

**Flujo principal**

1. Usuario: abre "Tarifas y conversión".
2. Sistema: muestra la grilla de conversiones vigentes.
3. Usuario: selecciona una fila de la grilla.
4. Sistema: carga el material y sus puntos por kilo en los controles de abajo.
5. Usuario: corrige los puntos por kilo y presiona "Fijar conversión".
6. Sistema: muestra "Conversión fijada. Queda registrada como decisión."

**Flujos alternativos**

- 5a. Puntos por kilo no válidos.
  - 5a.1 Sistema: muestra "Puntos por kg: ingrese un número válido".
- 3a. Usuario elige el material en la lista desplegable sin seleccionar la grilla.
  - 3a.1 Sistema: muestra los puntos vigentes de ese material en el campo correspondiente.

## CU09 Solicitar verificación de vendedor

**Actor principal:** Ciudadano.
**Precondiciones:** el usuario tiene sesión iniciada y es mayor de 18 años según la fecha de nacimiento de su perfil.
**Postcondiciones:** queda una solicitud pendiente de revisión por el área comercial.

**Flujo principal**

1. Usuario: abre "Verificación de vendedor".
2. Sistema: muestra su estado actual (sin solicitud, pendiente, aprobada o rechazada) y el campo "Documentación (ref.)".
3. Usuario: escribe la referencia de la documentación y presiona "Solicitar verificación".
4. Sistema: muestra "Solicitud enviada. El área comercial la revisará." y el estado pasa a pendiente.

**Flujos alternativos**

- 2a. El estado es pendiente o aprobada.
  - 2a.1 Sistema: deshabilita "Solicitar verificación".
- 3a. Referencia vacía.
  - 3a.1 Sistema: muestra "Ingrese la referencia de la documentación".
- 3b. El usuario no tiene la edad mínima.
  - 3b.1 Sistema: muestra "Debe tener 18 años para vender".

## CU10 Resolver verificación de vendedor

**Actor principal:** Área comercial.
**Precondiciones:** el usuario tiene sesión iniciada con el rol AREA_COMERCIAL y existe al menos una solicitud pendiente.
**Postcondiciones:** la solicitud queda aprobada o rechazada, y sale de la lista de pendientes.

**Flujo principal**

1. Usuario: abre "Verificación de vendedor".
2. Sistema: muestra la lista de solicitudes pendientes (usuario, fecha y referencia de documentación).
3. Usuario: selecciona una solicitud y presiona "Aprobar seleccionada".
4. Sistema: muestra "Vendedor verificado." y quita la solicitud de la lista.

**Flujos alternativos**

- 3a. Usuario presiona "Rechazar seleccionada".
  - 3a.1 Sistema: muestra "Solicitud rechazada." y quita la solicitud de la lista.
- 3b. Usuario presiona los botones sin seleccionar una solicitud.
  - 3b.1 Sistema: muestra "Seleccione una solicitud".
- 2a. No hay solicitudes pendientes.
  - 2a.1 Sistema: muestra la lista vacía.

## CU11 Publicar producto

**Actor principal:** Vendedor verificado.
**Precondiciones:** el usuario tiene el estado de vendedor aprobado (CU10).
**Postcondiciones:** el producto queda publicado en estado pendiente de aprobación.

**Flujo principal**

1. Usuario: abre "Publicar producto".
2. Sistema: muestra el formulario de publicación, la comisión estimada para la categoría y la lista de sus publicaciones.
3. Usuario: completa nombre, categoría, descripción, precio y stock, y presiona "Publicar".
4. Sistema: muestra "Producto {id} publicado. Queda pendiente de aprobación del área comercial." y lo agrega a la lista de sus publicaciones.

**Flujos alternativos**

- 3a. Usuario selecciona una publicación propia de la lista.
  - 3a.1 Sistema: carga su nombre, categoría, precio y stock.
  - 3a.2 Usuario: modifica precio o stock y presiona "Actualizar".
  - 3a.3 Sistema: muestra "Precio y stock actualizados".
- 3b. El vendedor no está verificado.
  - 3b.1 Sistema: deshabilita "Publicar".
- 3c. Precio igual o menor a cero, o stock negativo.
  - 3c.1 Sistema: muestra "El precio debe ser mayor a cero" o "El stock no puede ser negativo".

## CU12 Aprobar producto

**Actor principal:** Área comercial.
**Precondiciones:** el usuario tiene sesión iniciada con el rol AREA_COMERCIAL y existe al menos un producto pendiente.
**Postcondiciones:** el producto queda visible en el catálogo de compra.

**Flujo principal**

1. Usuario: abre "Aprobar productos".
2. Sistema: muestra la lista de productos pendientes (producto, categoría, precio, stock y vendedor).
3. Usuario: selecciona un producto y presiona "Aprobar".
4. Sistema: muestra "Producto aprobado. Ya está visible en el catálogo." y lo quita de la lista.

**Flujos alternativos**

- 3a. Usuario presiona "Aprobar" sin seleccionar un producto.
  - 3a.1 Sistema: muestra "Seleccione un producto pendiente".

## CU13 Comprar producto

**Actor principal:** Comprador (ciudadano).
**Precondiciones:** el usuario tiene sesión iniciada y existe al menos un producto aprobado con stock.
**Postcondiciones:** queda registrada la orden, con el pago aprobado, el stock descontado y la liquidación del vendedor retenida.

**Flujo principal**

1. Usuario: abre "Comprar".
2. Sistema: muestra el catálogo de productos aprobados con stock.
3. Usuario: selecciona un producto, elige la cantidad y escribe el domicilio de entrega.
4. Sistema: muestra el resumen "{cantidad} x {producto} = {total}".
5. Usuario: presiona "Pagar pedido".
6. Sistema: pregunta "¿Confirmar el pedido?" con el resumen.
7. Usuario: presiona "Sí".
8. Sistema: muestra "Pago aprobado. Orden {id} registrada. Se le notificará cuando el vendedor la despache." y actualiza el catálogo.

**Flujos alternativos**

- 5a. Usuario presiona "Pagar pedido" sin seleccionar un producto.
  - 5a.1 Sistema: muestra "Seleccione un producto".
- 5b. Domicilio vacío.
  - 5b.1 Sistema: muestra "Ingrese el domicilio de entrega".
- 7a. Usuario presiona "No".
  - 7a.1 Sistema: vuelve al catálogo sin registrar el pedido.
- 8a. El cobro es rechazado.
  - 8a.1 Sistema: muestra "El pago fue rechazado. No se registró el pedido."
- 3a. Cantidad mayor al stock disponible.
  - 3a.1 Sistema: muestra "Stock insuficiente".
- 3b. El producto es propio del usuario.
  - 3b.1 Sistema: muestra "No puede comprar sus propios productos".

## CU14 Despachar pedido

**Actor principal:** Vendedor.
**Precondiciones:** el vendedor tiene una venta confirmada y pagada (CU13).
**Postcondiciones:** la orden queda despachada y espera la confirmación del comprador.

**Flujo principal**

1. Usuario: abre "Pedidos y liquidaciones" y elige la pestaña de ventas.
2. Sistema: muestra la lista de ventas del vendedor con su estado.
3. Usuario: selecciona una venta pendiente de despacho y presiona "Marcar como despachada".
4. Sistema: muestra "Orden marcada como despachada." y actualiza su estado.

**Flujos alternativos**

- 3a. Usuario presiona "Marcar como despachada" sin seleccionar una venta.
  - 3a.1 Sistema: muestra "Seleccione una venta".

## CU15 Confirmar recepción de pedido

**Actor principal:** Comprador (ciudadano).
**Precondiciones:** la orden fue despachada (CU14).
**Postcondiciones:** la orden queda recibida y la liquidación del vendedor pasa a disponible.

**Flujo principal**

1. Usuario: abre "Pedidos y liquidaciones" y elige la pestaña de compras.
2. Sistema: muestra la lista de compras con su estado.
3. Usuario: selecciona una compra despachada y presiona "Confirmar recepción".
4. Sistema: muestra "Recepción confirmada. El saldo quedó disponible para liquidar." y actualiza el estado.

**Flujos alternativos**

- 3a. Usuario presiona "Confirmar recepción" sin seleccionar una compra.
  - 3a.1 Sistema: muestra "Seleccione una compra".

## CU16 Liquidar vendedor

**Actor principal:** Área comercial.
**Precondiciones:** existe una liquidación disponible (CU15).
**Postcondiciones:** la liquidación queda pagada.

**Flujo principal**

1. Usuario: abre "Pedidos y liquidaciones" y elige la pestaña de liquidaciones.
2. Sistema: muestra las liquidaciones disponibles, con el monto neto de cada una.
3. Usuario: selecciona una liquidación y presiona "Pagar".
4. Sistema: pregunta "¿Ordenar la transferencia al vendedor por el saldo neto?".
5. Usuario: presiona "Sí".
6. Sistema: muestra "Transferencia ordenada. La liquidación quedó pagada." y la quita de la lista.

**Flujos alternativos**

- 3a. Usuario presiona "Pagar" sin seleccionar una liquidación.
  - 3a.1 Sistema: muestra "Seleccione una liquidación".
- 5a. Usuario presiona "No".
  - 5a.1 Sistema: vuelve a la lista sin cambios.

## CU17 Presentar reclamo

**Actor principal:** Comprador (ciudadano).
**Precondiciones:** la orden fue despachada y el comprador no confirmó la recepción.
**Postcondiciones:** queda registrado el reclamo y la liquidación del vendedor queda congelada.

**Flujo principal**

1. Usuario: abre "Reclamos" y elige la pestaña "Presentar".
2. Sistema: muestra sus órdenes despachadas.
3. Usuario: selecciona una orden, escribe el motivo y presiona "Presentar reclamo".
4. Sistema: muestra "Reclamo {id} registrado."

**Flujos alternativos**

- 3a. Motivo vacío.
  - 3a.1 Sistema: muestra "Describa el motivo del reclamo".
- 3b. Usuario presiona "Presentar reclamo" sin seleccionar una orden.
  - 3b.1 Sistema: muestra "Seleccione una orden".

## CU18 Resolver reclamo

**Actor principal:** Área comercial.
**Precondiciones:** existe al menos un reclamo abierto (CU17).
**Postcondiciones:** el reclamo queda resuelto como procedente o improcedente, y la liquidación se actualiza según la resolución.

**Flujo principal**

1. Usuario: abre "Reclamos" y elige la pestaña "Resolver".
2. Sistema: muestra la lista de reclamos abiertos (orden, comprador, motivo y fecha).
3. Usuario: selecciona un reclamo y presiona "Procedente" o "Improcedente".
4. Sistema: muestra "Reclamo resuelto" y lo quita de la lista.

**Flujos alternativos**

- 3a. Usuario presiona un botón sin seleccionar un reclamo.
  - 3a.1 Sistema: muestra "Seleccione un reclamo".
- 2a. No hay reclamos abiertos.
  - 2a.1 Sistema: muestra la lista vacía.
