# N. Procesos de negocio

Los tres procesos de negocio que cubre el sistema: N01 (apoyo a la toma de decisiones), N02 (ingreso por venta y comisión) y N03 (egreso por reconocimiento y canje). Cada proceso incluye roles, descripción funcional, diagramas y especificación de casos de uso.

## Figuras generales

- Diagrama de clases por capa: ver G06.
- Modelo de datos (DER técnico y de negocio): ver G07.

## Resumen

| Código | Proceso | Naturaleza | Gestiones de G02 que cubre |
|---|---|---|---|
| N01 | Gestión de la red de puntos verdes y de las tarifas | Apoyo a la toma de decisiones | G02.5, G02.8, G02.9 |
| N02 | Venta de productos sustentables y cobro de comisión | Ingreso | G02.7, G02.8 |
| N03 | Reconocimiento por reciclaje y canje de Eco-Puntos | Egreso | G02.2, G02.3, G02.9 |

Casos de uso (numeración global):

| Caso de uso | Proceso | Descripción breve |
|---|---|---|
| CU01 | N03 | Declarar material y recibir recomendación (existente, reformulado) |
| CU02 | N03 | Registrar recepción de material en punto verde (extiende a CU01) |
| CU03 | N03 | Canjear Eco-Puntos por cupón o envío bonificado |
| CU04 | N01 | Consultar indicadores de acopio y ventas |
| CU05 | N01 | Administrar puntos verdes y materiales |
| CU06 | N01 | Configurar comisiones y tabla de conversión de Eco-Puntos |
| CU07 | N02 | Verificar identidad del vendedor (incluido en CU08) |
| CU08 | N02 | Publicar producto en el marketplace |
| CU09 | N02 | Comprar y pagar pedido |
| CU10 | N02 | Confirmar recepción y liquidar saldo al vendedor |
| CU11 | N02 | Gestionar reclamo de pedido (extiende a CU10) |

---

## N01. Gestión de la red de puntos verdes y de las tarifas

**Naturaleza:** apoyo a la toma de decisiones.

### 1. Roles intervinientes

- **Gerencia:** analiza la información disponible y decide altas y bajas de puntos verdes, materiales aceptados y tarifas.
- **Referente de cooperativas y puntos verdes:** aporta el conocimiento de campo y valida los datos de las locaciones.
- **Área comercial:** aporta datos de ventas, comisiones cobradas y productos publicados.

### 2. Descripción funcional del proceso

- **Entrada:** informes de acopio por material y zona, informes de ventas y canjes, solicitudes de nuevos puntos verdes o de cambio de tarifas.
- **Comportamiento:** una vez por mes, la gerencia revisa el acopio por material y zona, el volumen de ventas y el costo de los canjes. Con esa información evalúa si conviene incorporar o dar de baja un punto verde, habilitar o deshabilitar materiales, o ajustar la comisión por categoría y la tabla de conversión de Eco-Puntos. El referente de cooperativas valida las locaciones propuestas. Las decisiones se formalizan y se comunican a las áreas involucradas.
- **Salida:** decisiones formalizadas sobre la red de puntos verdes y las tarifas vigentes.

### 3. Diagrama de actividad

![Figura N01-1. Diagrama de actividad del proceso N01](diagramas/procesos/N01_diagrama_actividad.svg)

### 4. Modelo conceptual

![Figura N01-2. Modelo conceptual del proceso N01](diagramas/procesos/N01_modelo_conceptual.svg)

### 5. Especificación de casos de uso

![Figura N01-0. Diagrama de casos de uso del proceso N01](diagramas/casos_de_uso/N01.svg)

### CU04 Consultar indicadores

**Descripción:** Permite a gerencia consultar los indicadores de acopio, ventas y costo de canjes de un período.
**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** se muestran los indicadores del período y queda registrado el informe.

**Flujo principal**

1. La gerencia abre "Indicadores de acopio y ventas".
2. El sistema muestra el campo "Período (aaaa-mm):".
3. La gerencia escribe el período y presiona "Consultar indicadores".
4. El sistema muestra la grilla de indicadores del período (acopio por material, ventas y costo de canjes).

**Flujos alternativos**

- 3a. Período con formato incorrecto.
  - 3a.1 El sistema muestra "El período debe tener el formato aaaa-mm."

![Figura CU04-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU04.svg)

![Figura CU04-a. Diagrama de clases](diagramas/cu/CU04_clases.svg)

![Figura CU04-b. Modelo de datos](diagramas/cu/CU04_der.svg)

![Figura CU04-c. Diagrama de secuencia](diagramas/secuencias_png/CU04.png)


### CU05 Exportar informe de indicadores

**Descripción:** Permite a gerencia exportar el informe de indicadores a PDF o XML.
**Actor principal:** Gerencia.
**Precondiciones:** se consultaron los indicadores de un período (CU04).
**Postcondiciones:** queda guardado el archivo PDF o XML en la ubicación elegida.

**Flujo principal (PDF)**

1. La gerencia presiona "Exportar a PDF".
2. El sistema muestra el diálogo para guardar con el nombre "Indicadores_{período}.pdf".
3. La gerencia elige la carpeta y presiona "Guardar".
4. El sistema muestra "Informe PDF generado."

**Flujo alternativo (XML)**

- 1a. La gerencia presiona "Guardar XML".
  - 1a.1 El sistema muestra el diálogo para guardar con el nombre "Indicadores_{período}.xml".
  - 1a.2 La gerencia elige la carpeta y presiona "Guardar".
  - 1a.3 El sistema muestra "Informe guardado en XML."

**Flujos alternativos comunes**

- 2a. La gerencia cierra el diálogo sin guardar.
  - 2a.1 El sistema no guarda el archivo y vuelve a la pantalla.

![Figura CU05-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU05.svg)

![Figura CU05-a. Diagrama de clases](diagramas/cu/CU05_clases.svg)

![Figura CU05-b. Modelo de datos](diagramas/cu/CU05_der.svg)

![Figura CU05-c. Diagrama de secuencia](diagramas/secuencias_png/CU05.png)


### CU06 Gestionar puntos verdes

**Descripción:** Permite a gerencia dar de alta o modificar puntos verdes y sus materiales aceptados.
**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** el punto verde queda dado de alta o modificado, con sus materiales aceptados.

**Flujo principal**

1. La gerencia abre "Red de puntos verdes y materiales".
2. La gerencia completa "Nombre", "Dirección", "Latitud", "Longitud", "Horario" y marca los "Materiales aceptados".
3. La gerencia marca "Locación validada por el referente de cooperativas" y presiona "Guardar".
4. El sistema muestra "Punto verde guardado (ID {id}). La decisión quedó registrada."

**Flujos alternativos**

- 2a. La gerencia presiona "Nuevo" para cargar otro punto verde.
  - 2a.1 El sistema limpia el formulario.
- 3a. La gerencia presiona "Guardar" sin marcar la validación del referente.
  - 3a.1 El sistema muestra "La locación debe ser validada por el referente de cooperativas."
- 3b. Nombre vacío.
  - 3b.1 El sistema muestra "Ingrese el nombre del punto verde."
- 3c. Dirección vacía.
  - 3c.1 El sistema muestra "Ingrese la dirección."

![Figura CU06-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU06.svg)

![Figura CU06-a. Diagrama de clases](diagramas/cu/CU06_clases.svg)

![Figura CU06-b. Modelo de datos](diagramas/cu/CU06_der.svg)

![Figura CU06-c. Diagrama de secuencia](diagramas/secuencias_png/CU06.png)


### CU07 Fijar comisión por categoría

**Descripción:** Permite a gerencia fijar la comisión por categoría de producto.
**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** la comisión de la categoría queda vigente desde la fecha de hoy.

**Flujo principal**

1. La gerencia abre "Tarifas de comisión y conversión".
2. El sistema muestra "Comisión vigente por categoría:".
3. La gerencia escribe la "Categoría" y la "Comisión %" y presiona "Fijar comisión".
4. El sistema muestra "Comisión fijada (tarifa {id}). Rige desde hoy y queda registrada como decisión."

**Flujos alternativos**

- 3a. Comisión menor o igual a 0 o mayor a 50.
  - 3a.1 El sistema muestra "La comisión debe estar entre 0,01% y 50%."
- 3b. Categoría vacía.
  - 3b.1 El sistema muestra "Ingrese la categoría."

![Figura CU07-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU07.svg)

![Figura CU07-a. Diagrama de clases](diagramas/cu/CU07_clases.svg)

![Figura CU07-b. Modelo de datos](diagramas/cu/CU07_der.svg)

![Figura CU07-c. Diagrama de secuencia](diagramas/secuencias_png/CU07.png)


### CU08 Fijar conversión de puntos

**Descripción:** Permite a gerencia fijar los puntos por kilo de cada material.
**Actor principal:** Gerencia.
**Precondiciones:** el usuario tiene sesión iniciada con el rol GERENCIA.
**Postcondiciones:** la conversión de puntos por kilo del material queda vigente desde hoy.

**Flujo principal**

1. La gerencia abre "Tarifas de comisión y conversión".
2. El sistema muestra la grilla "Conversión vigente de Eco-Puntos".
3. La gerencia selecciona una fila de la grilla.
4. El sistema carga el material en "Material" y sus puntos en "Puntos por kg".
5. La gerencia corrige "Puntos por kg" y presiona "Fijar conversión".
6. El sistema muestra "Conversión fijada (ID {id}). Queda registrada como decisión."

**Flujos alternativos**

- 5a. Puntos por kg vacíos, cero o no numéricos.
  - 5a.1 El sistema muestra "Los puntos por kg deben ser mayores a cero." o "Puntos por kg: ingrese un número válido.".
- 5b. No hay material seleccionado.
  - 5b.1 El sistema muestra "Seleccione un material."

![Figura CU08-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU08.svg)

![Figura CU08-a. Diagrama de clases](diagramas/cu/CU08_clases.svg)

![Figura CU08-b. Modelo de datos](diagramas/cu/CU08_der.svg)

![Figura CU08-c. Diagrama de secuencia](diagramas/secuencias_png/CU08.png)


### 6. Diagrama de secuencia

Cada caso de uso tiene su secuencia (figuras de CU04, CU05 y CU06 arriba).

### 7. Modelo de datos del proceso

![Figura N01-13. Modelo de datos del proceso N01](diagramas/procesos/N01_modelo_datos.svg)

---

## N02. Venta de productos sustentables y cobro de comisión

**Naturaleza:** ingreso (monetización por intermediación).

### 1. Roles intervinientes

- **Vendedor sustentable:** publica productos, mantiene el stock y despacha los pedidos.
- **Comprador:** elige productos, paga y confirma la recepción.
- **Área comercial:** verifica publicaciones y vendedores, controla las ventas, retiene la comisión y libera el saldo neto.
- **Pasarela de pagos (externo):** procesa el cobro con tarjeta o transferencia.

### 2. Descripción funcional del proceso

- **Entrada:** producto publicado por el vendedor y pedido confirmado y pagado por el comprador.
- **Comportamiento:** el vendedor publica su producto. El área comercial verifica la publicación y la identidad del vendedor; si no está aprobada, el vendedor la corrige. El comprador paga el pedido a través de la pasarela. Si el cobro es aprobado, el área comercial registra la venta, retiene la comisión que corresponde a la categoría y registra el saldo neto del vendedor. El vendedor despacha el producto y el comprador confirma la recepción. Si la recepción es conforme, el área comercial libera el saldo neto y ordena la transferencia al vendedor. Si hay un reclamo, el comprador lo presenta, el saldo queda congelado y se audita.
- **Salida:** producto entregado, comisión ingresada a la empresa y saldo neto transferido al vendedor.

### 3. Diagrama de actividad

![Figura N02-1. Diagrama de actividad del proceso N02](diagramas/procesos/N02_diagrama_actividad.svg)

### 4. Modelo conceptual

![Figura N02-2. Modelo conceptual del proceso N02](diagramas/procesos/N02_modelo_conceptual.svg)

### 5. Especificación de casos de uso

![Figura N02-0. Diagrama de casos de uso del proceso N02](diagramas/casos_de_uso/N02.svg)

### CU09 Solicitar verificación de vendedor

**Descripción:** Permite al ciudadano pedir la verificación para vender.
**Actor principal:** Ciudadano.
**Precondiciones:** el usuario tiene sesión iniciada y cargó su fecha de nacimiento en "Mi perfil".
**Postcondiciones:** queda una solicitud pendiente de revisión por el área comercial.

**Flujo principal**

1. El ciudadano abre "Verificación de vendedor".
2. El sistema muestra "Estado: {estado}" y el campo "Documentación (ref.):".
3. El ciudadano escribe la referencia de la documentación y presiona "Solicitar verificación".
4. El sistema muestra "Solicitud enviada. El área comercial la revisará."

**Flujos alternativos**

- 2a. El estado es pendiente o aprobado.
  - 2a.1 El sistema deshabilita "Solicitar verificación".
- 3a. Referencia vacía.
  - 3a.1 El sistema muestra "Ingrese la referencia de la documentación."
- 3b. No cargó la fecha de nacimiento.
  - 3b.1 El sistema muestra "Cargue su fecha de nacimiento en Mi perfil para solicitar la verificación."
- 3c. Tiene menos de 18 años.
  - 3c.1 El sistema muestra "Para vender debe tener al menos 18 años."

![Figura CU09-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU09.svg)

![Figura CU09-a. Diagrama de clases](diagramas/cu/CU09_clases.svg)

![Figura CU09-b. Modelo de datos](diagramas/cu/CU09_der.svg)

![Figura CU09-c. Diagrama de secuencia](diagramas/secuencias_png/CU09.png)


### CU10 Resolver verificación de vendedor

**Descripción:** Permite al área comercial aprobar o rechazar la verificación de un vendedor.
**Actor principal:** Área comercial.
**Precondiciones:** el usuario tiene sesión iniciada con el rol AREA_COMERCIAL y existe al menos una solicitud pendiente.
**Postcondiciones:** la solicitud queda aprobada o rechazada, y sale de la lista de pendientes.

**Flujo principal**

1. El área comercial abre "Verificación de vendedor".
2. El sistema muestra "Solicitudes pendientes (área comercial):" con la lista de solicitudes.
3. El área comercial selecciona una solicitud y presiona "Aprobar seleccionada".
4. El sistema muestra "Vendedor verificado." y quita la solicitud de la lista.

**Flujos alternativos**

- 3a. El área comercial presiona "Rechazar seleccionada".
  - 3a.1 El sistema muestra "Solicitud rechazada." y quita la solicitud de la lista.
- 3b. El área comercial presiona "Aprobar seleccionada" o "Rechazar seleccionada" sin seleccionar una solicitud.
  - 3b.1 El sistema muestra "Seleccione una solicitud."

![Figura CU10-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU10.svg)

![Figura CU10-a. Diagrama de clases](diagramas/cu/CU10_clases.svg)

![Figura CU10-b. Modelo de datos](diagramas/cu/CU10_der.svg)

![Figura CU10-c. Diagrama de secuencia](diagramas/secuencias_png/CU10.png)


### CU11 Publicar producto

**Descripción:** Permite a un vendedor verificado publicar un producto.
**Actor principal:** Vendedor verificado.
**Precondiciones:** el vendedor tiene el estado de verificado (CU10).
**Postcondiciones:** el producto queda publicado en estado pendiente de aprobación.

**Flujo principal**

1. El vendedor verificado abre "Publicar producto sustentable".
2. El sistema muestra la comisión estimada ("Comisión estimada: {porcentaje}%") y "Mis publicaciones:".
3. El vendedor verificado completa "Nombre", "Categoría", "Descripción", "Precio" y "Stock", y presiona "Publicar".
4. El sistema muestra "Producto {id} publicado. Queda pendiente de aprobación del área comercial."

**Flujos alternativos**

- 3a. El vendedor verificado selecciona una publicación propia de "Mis publicaciones".
  - 3a.1 El sistema carga su nombre, categoría, precio y stock.
  - 3a.2 El vendedor verificado modifica "Precio" o "Stock" y presiona "Actualizar precio y stock".
  - 3a.3 El sistema muestra "Precio y stock actualizados."
- 3b. El vendedor no está verificado.
  - 3b.1 El sistema deshabilita "Publicar".
- 3c. Nombre o categoría vacíos.
  - 3c.1 El sistema muestra "Ingrese el nombre del producto." o "Ingrese la categoría.".
- 3d. Precio igual o menor a cero, o stock negativo.
  - 3d.1 El sistema muestra "El precio debe ser mayor a cero." o "El stock no puede ser negativo.".

![Figura CU11-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU11.svg)

![Figura CU11-a. Diagrama de clases](diagramas/cu/CU11_clases.svg)

![Figura CU11-b. Modelo de datos](diagramas/cu/CU11_der.svg)

![Figura CU11-c. Diagrama de secuencia](diagramas/secuencias_png/CU11.png)


### CU12 Aprobar producto

**Descripción:** Permite al área comercial aprobar un producto pendiente para su venta.
**Actor principal:** Área comercial.
**Precondiciones:** el usuario tiene sesión iniciada con el rol AREA_COMERCIAL y existe al menos un producto pendiente.
**Postcondiciones:** el producto queda visible en el catálogo de compra.

**Flujo principal**

1. El área comercial abre "Publicar producto sustentable".
2. El sistema muestra "Pendientes de aprobación (área comercial):" con la lista de productos.
3. El área comercial selecciona un producto y presiona "Aprobar producto seleccionado".
4. El sistema muestra "Producto aprobado. Ya está visible en el catálogo." y quita el producto de la lista.

**Flujos alternativos**

- 3a. El área comercial presiona "Aprobar producto seleccionado" sin seleccionar un producto.
  - 3a.1 El sistema muestra "Seleccione un producto pendiente."

![Figura CU12-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU12.svg)

![Figura CU12-a. Diagrama de clases](diagramas/cu/CU12_clases.svg)

![Figura CU12-b. Modelo de datos](diagramas/cu/CU12_der.svg)

![Figura CU12-c. Diagrama de secuencia](diagramas/secuencias_png/CU12.png)


### CU13 Comprar producto

**Descripción:** Permite al comprador comprar un producto y pagarlo.
**Actor principal:** Comprador (ciudadano).
**Precondiciones:** el usuario tiene sesión iniciada y existe al menos un producto aprobado con stock.
**Postcondiciones:** queda registrada la orden, con el pago aprobado, el stock descontado y la liquidación del vendedor retenida.

**Flujo principal**

1. El comprador abre "Comprar en la tienda sustentable".
2. El sistema muestra el catálogo de productos aprobados con stock.
3. El comprador selecciona un producto, elige "Cantidad" y escribe el "Domicilio envío".
4. El sistema muestra el resumen "{cantidad} x {producto} = {total}".
5. El comprador presiona "Pagar pedido".
6. El sistema pregunta "¿Confirmar el pedido?" con el resumen, con los botones "Sí" y "No".
7. El comprador presiona "Sí".
8. El sistema muestra "Pago aprobado. Orden {id} registrada. Se le notificará cuando el vendedor la despache." y actualiza el catálogo.

**Flujos alternativos**

- 5a. El comprador presiona "Pagar pedido" sin seleccionar un producto.
  - 5a.1 El sistema muestra "Seleccione un producto."
- 5b. Domicilio vacío.
  - 5b.1 El sistema muestra "Ingrese el domicilio de entrega."
- 7a. El comprador presiona "No".
  - 7a.1 El sistema vuelve al catálogo sin registrar el pedido.
- 7b. El cobro es rechazado.
  - 7b.1 El sistema muestra "El pago fue rechazado: {motivo}".
- 7c. Cantidad mayor al stock disponible.
  - 7c.1 El sistema muestra "Stock insuficiente para completar la compra."
- 7d. El producto es propio del comprador.
  - 7d.1 El sistema muestra "No puede comprar sus propios productos."

![Figura CU13-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU13.svg)

![Figura CU13-a. Diagrama de clases](diagramas/cu/CU13_clases.svg)

![Figura CU13-b. Modelo de datos](diagramas/cu/CU13_der.svg)

![Figura CU13-c. Diagrama de secuencia](diagramas/secuencias_png/CU13.png)


### CU14 Despachar pedido

**Descripción:** Permite al vendedor marcar una venta como despachada.
**Actor principal:** Vendedor.
**Precondiciones:** el vendedor tiene una venta pagada (CU13).
**Postcondiciones:** la orden queda despachada y espera la confirmación del comprador.

**Flujo principal**

1. El vendedor abre "Pedidos y liquidaciones" y elige la pestaña "Mis ventas".
2. El sistema muestra la lista de ventas del vendedor.
3. El vendedor selecciona una venta y presiona "Marcar como despachada".
4. El sistema muestra "Orden marcada como despachada."

**Flujos alternativos**

- 3a. El vendedor presiona "Marcar como despachada" sin seleccionar una venta.
  - 3a.1 El sistema muestra "Seleccione una venta."
- 3b. La orden ya no está pendiente de despacho.
  - 3b.1 El sistema muestra "La orden no está pendiente de despacho o no le pertenece."

![Figura CU14-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU14.svg)

![Figura CU14-a. Diagrama de clases](diagramas/cu/CU14_clases.svg)

![Figura CU14-b. Modelo de datos](diagramas/cu/CU14_der.svg)

![Figura CU14-c. Diagrama de secuencia](diagramas/secuencias_png/CU14.png)


### CU15 Confirmar recepción de pedido

**Descripción:** Permite al comprador confirmar la recepción conforme de un pedido.
**Actor principal:** Comprador (ciudadano).
**Precondiciones:** la orden fue despachada (CU14).
**Postcondiciones:** la orden queda recibida y la liquidación del vendedor pasa a disponible.

**Flujo principal**

1. El comprador abre "Pedidos y liquidaciones" y elige la pestaña "Mis compras".
2. El sistema muestra la lista de compras.
3. El comprador selecciona una compra y presiona "Confirmar recepción conforme".
4. El sistema muestra "Recepción confirmada. El saldo quedó disponible para liquidar."

**Flujos alternativos**

- 3a. El comprador presiona "Confirmar recepción conforme" sin seleccionar una compra.
  - 3a.1 El sistema muestra "Seleccione una compra."
- 3b. La orden no está despachada.
  - 3b.1 El sistema muestra "La orden no está despachada o no le pertenece."

![Figura CU15-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU15.svg)

![Figura CU15-a. Diagrama de clases](diagramas/cu/CU15_clases.svg)

![Figura CU15-b. Modelo de datos](diagramas/cu/CU15_der.svg)

![Figura CU15-c. Diagrama de secuencia](diagramas/secuencias_png/CU15.png)


### CU16 Liquidar vendedor

**Descripción:** Permite al área comercial ordenar la transferencia del saldo neto al vendedor.
**Actor principal:** Área comercial.
**Precondiciones:** existe una liquidación disponible (CU15).
**Postcondiciones:** la liquidación queda pagada.

**Flujo principal**

1. El área comercial abre "Pedidos y liquidaciones" y elige la pestaña "Liquidaciones".
2. El sistema muestra las liquidaciones disponibles.
3. El área comercial selecciona una liquidación y presiona "Ordenar transferencia al vendedor".
4. El sistema pregunta "¿Ordenar la transferencia al vendedor por el saldo neto?" con los botones "Sí" y "No".
5. El área comercial presiona "Sí".
6. El sistema muestra "Transferencia ordenada. La liquidación quedó pagada." y quita la liquidación de la lista.

**Flujos alternativos**

- 3a. El área comercial presiona "Ordenar transferencia al vendedor" sin seleccionar una liquidación.
  - 3a.1 El sistema muestra "Seleccione una liquidación."
- 5a. El área comercial presiona "No".
  - 5a.1 El sistema vuelve a la lista sin cambios.
- 5b. La liquidación ya no está disponible.
  - 5b.1 El sistema muestra "La liquidación no está disponible para pagar."

![Figura CU16-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU16.svg)

![Figura CU16-a. Diagrama de clases](diagramas/cu/CU16_clases.svg)

![Figura CU16-b. Modelo de datos](diagramas/cu/CU16_der.svg)

![Figura CU16-c. Diagrama de secuencia](diagramas/secuencias_png/CU16.png)


### CU17 Presentar reclamo

**Descripción:** Permite al comprador presentar un reclamo sobre un pedido despachado.
**Actor principal:** Comprador (ciudadano).
**Precondiciones:** el comprador tiene una orden despachada.
**Postcondiciones:** queda registrado el reclamo y la liquidación del vendedor queda congelada.

**Flujo principal**

1. El comprador abre "Reclamos" y elige la pestaña "Presentar reclamo".
2. El sistema muestra las órdenes del comprador.
3. El comprador selecciona una orden, escribe el "Motivo:" y presiona "Presentar reclamo".
4. El sistema muestra "Reclamo {id} presentado. El saldo del vendedor quedó congelado hasta su resolución."

**Flujos alternativos**

- 3a. El comprador presiona "Presentar reclamo" sin seleccionar una orden.
  - 3a.1 El sistema muestra "Seleccione la orden a reclamar."
- 3b. Motivo vacío.
  - 3b.1 El sistema muestra "Describa el motivo del reclamo."
- 3c. La orden no es despachada o no es propia.
  - 3c.1 El sistema muestra "Solo puede reclamar órdenes despachadas propias."

![Figura CU17-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU17.svg)

![Figura CU17-a. Diagrama de clases](diagramas/cu/CU17_clases.svg)

![Figura CU17-b. Modelo de datos](diagramas/cu/CU17_der.svg)

![Figura CU17-c. Diagrama de secuencia](diagramas/secuencias_png/CU17.png)


### CU18 Resolver reclamo

**Descripción:** Permite al área comercial resolver un reclamo como procedente o improcedente.
**Actor principal:** Área comercial.
**Precondiciones:** existe al menos un reclamo abierto (CU17).
**Postcondiciones:** el reclamo queda resuelto como procedente o improcedente, y la liquidación se actualiza según la resolución.

**Flujo principal**

1. El área comercial abre "Reclamos" y elige la pestaña "Resolver reclamos".
2. El sistema muestra la lista de reclamos abiertos.
3. El área comercial selecciona un reclamo y presiona "Reclamo procedente".
4. El sistema muestra "Reclamo procedente: la orden se devolvió, el stock se restituyó y la liquidación se canceló." y quita el reclamo de la lista.

**Flujos alternativos**

- 3a. El área comercial presiona "Reclamo improcedente".
  - 3a.1 El sistema muestra "Reclamo improcedente: la liquidación volvió a retenida hasta la confirmación del comprador."
- 3b. El área comercial presiona un botón sin seleccionar un reclamo.
  - 3b.1 El sistema muestra "Seleccione un reclamo."
- 3c. El reclamo ya no está abierto.
  - 3c.1 El sistema muestra "El reclamo no está abierto."

![Figura CU18-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU18.svg)

![Figura CU18-a. Diagrama de clases](diagramas/cu/CU18_clases.svg)

![Figura CU18-b. Modelo de datos](diagramas/cu/CU18_der.svg)

![Figura CU18-c. Diagrama de secuencia](diagramas/secuencias_png/CU18.png)


### 6. Diagrama de secuencia

Cada caso de uso tiene su secuencia (figuras de CU07 a CU11 arriba).

### 7. Modelo de datos del proceso

![Figura N02-19. Modelo de datos del proceso N02](diagramas/procesos/N02_modelo_datos.svg)

---

## N03. Reconocimiento por reciclaje y canje de Eco-Puntos

**Naturaleza:** egreso (costo que asume la empresa para incentivar el reciclaje).

### 1. Roles intervinientes

- **Ciudadano:** declara material, lo entrega o lo reutiliza, y canjea sus Eco-Puntos.
- **Operador de punto verde:** recibe el material, lo pesa y lo registra.
- **Área de administración:** acredita los Eco-Puntos y autoriza los canjes.

### 2. Descripción funcional del proceso

- **Entrada:** declaración de material del ciudadano y, si corresponde, entrega física en un punto verde; solicitud de canje de Eco-Puntos.
- **Comportamiento:** el ciudadano declara el material y recibe la recomendación: reutilizarlo en el hogar o entregarlo en un punto verde. Si reutiliza, confirma la acción y el área de administración acredita los Eco-Puntos. Si entrega, el operador recibe, pesa y registra el material, notifica el peso y el área de administración acredita los Eco-Puntos. Cuando el ciudadano pide un canje, el área de administración verifica el saldo: si alcanza, entrega el cupón o el envío bonificado y registra el costo; si no, informa la situación al ciudadano.
- **Salida:** Eco-Puntos acreditados, canjes entregados y costo registrado.

### 3. Diagrama de actividad

![Figura N03-1. Diagrama de actividad del proceso N03](diagramas/procesos/N03_diagrama_actividad.svg)

### 4. Modelo conceptual

![Figura N03-2. Modelo conceptual del proceso N03](diagramas/procesos/N03_modelo_conceptual.svg)

### 5. Especificación de casos de uso

![Figura N03-0. Diagrama de casos de uso del proceso N03](diagramas/casos_de_uso/N03.svg)

### CU01 Declarar material

**Descripción:** Permite al ciudadano declarar un material y elegir cómo reciclarlo.
**Actor principal:** Ciudadano.
**Precondiciones:** el usuario tiene sesión iniciada con el rol CIUDADANO.
**Postcondiciones:** queda registrada la declaración y su recomendación (reutilizar o entregar).

**Flujo principal**

1. El ciudadano elige el material en la lista "Material" y escribe la cantidad en "Cantidad (kg)".
2. El ciudadano presiona "Declarar".
3. El sistema muestra la "Opción 1: reutilizar en casa" con la guía del material y la "Opción 2: entregar en el punto verde más cercano".
4. El ciudadano presiona "Reutilizar en casa (confirmar)".
5. El sistema muestra "Reutilización confirmada. Saldo actual: {saldo} Eco-Puntos."

**Flujos alternativos**

- 1a. El material no tiene guía cargada.
  - 1a.1 El sistema muestra "Opción 1: reutilizar en casa (declare un material primero)." y deshabilita "Reutilizar en casa (confirmar)".
  - 1a.2 El ciudadano presiona "Entregar en punto verde".
  - 1a.3 El sistema muestra "Entrega oficial registrada. Los Eco-Puntos se acreditarán cuando el punto verde confirme el peso."
- 4a. El ciudadano presiona "Entregar en punto verde" en lugar de reutilizar.
  - 4a.1 El sistema muestra "Entrega oficial registrada. Los Eco-Puntos se acreditarán cuando el punto verde confirme el peso."
- 2a. Cantidad vacía, cero o negativa.
  - 2a.1 El sistema muestra "La cantidad debe ser mayor a cero."
- 2b. El ciudadano presiona "Declarar" sin elegir material.
  - 2b.1 El sistema muestra "Seleccione un material."

![Figura CU01-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU01.svg)

![Figura CU01-a. Diagrama de clases](diagramas/cu/CU01_clases.svg)

![Figura CU01-b. Modelo de datos](diagramas/cu/CU01_der.svg)

![Figura CU01-c. Diagrama de secuencia](diagramas/secuencias_png/CU01.png)


### CU02 Registrar recepción de material

**Descripción:** Permite al operador de punto verde registrar el peso recibido y acreditar los Eco-Puntos.
**Actor principal:** Operador de punto verde.
**Precondiciones:** el operador tiene sesión iniciada con el rol OPERADOR_PUNTO_VERDE y existe al menos una declaración pendiente en el punto verde.
**Postcondiciones:** la declaración queda entregada y el ciudadano recibe los Eco-Puntos del peso registrado.

**Flujo principal**

1. El operador de punto verde elige el punto verde en la lista "Punto verde".
2. El sistema muestra en "Declaraciones pendientes de entrega" las declaraciones de ese punto verde.
3. El operador de punto verde selecciona una declaración.
4. El sistema muestra en "Peso pesado (kg)" el peso que declaró el ciudadano.
5. El operador de punto verde confirma o corrige el peso y presiona "Registrar recepción".
6. El sistema muestra "Recepción registrada. Se acreditaron {puntos} Eco-Puntos al ciudadano." y la declaración sale de la lista.

**Flujos alternativos**

- 2a. El punto verde no tiene declaraciones pendientes.
  - 2a.1 El sistema la lista queda vacía.
- 5a. El operador de punto verde presiona "Registrar recepción" sin seleccionar una declaración.
  - 5a.1 El sistema muestra "Seleccione una declaración pendiente."
- 5b. El punto verde no acepta el material de la declaración.
  - 5b.1 El sistema muestra "El punto verde no acepta este material. Se rechaza la carga."
- 5c. Peso vacío, cero o no numérico.
  - 5c.1 El sistema muestra "El peso debe ser mayor a cero." o "Peso: ingrese un número válido.".

![Figura CU02-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU02.svg)

![Figura CU02-a. Diagrama de clases](diagramas/cu/CU02_clases.svg)

![Figura CU02-b. Modelo de datos](diagramas/cu/CU02_der.svg)

![Figura CU02-c. Diagrama de secuencia](diagramas/secuencias_png/CU02.png)


### CU03 Canjear Eco-Puntos

**Descripción:** Permite al ciudadano canjear sus Eco-Puntos por un beneficio.
**Actor principal:** Ciudadano.
**Precondiciones:** el usuario tiene sesión iniciada y una cuenta de Eco-Puntos.
**Postcondiciones:** se descuentan los puntos del beneficio y queda registrado el canje.

**Flujo principal**

1. El ciudadano abre "Canjear Eco-Puntos".
2. El sistema muestra "Saldo: {saldo} Eco-Puntos", la lista "Beneficios disponibles" y los "Movimientos de mi cuenta".
3. El ciudadano selecciona un beneficio y presiona "Canjear beneficio seleccionado".
4. El sistema pregunta "¿Canjear \"{descripción}\"?" con los botones "Sí" y "No".
5. El ciudadano presiona "Sí".
6. El sistema muestra "Canje realizado. Su cupón o envío bonificado quedó registrado." y actualiza el saldo y los movimientos.

**Flujos alternativos**

- 3a. El ciudadano presiona "Canjear beneficio seleccionado" sin seleccionar un beneficio.
  - 3a.1 El sistema muestra "Seleccione un beneficio."
- 5a. El ciudadano presiona "No".
  - 5a.1 El sistema vuelve a la pantalla sin cambios.
- 5b. Saldo insuficiente.
  - 5b.1 El sistema muestra "Saldo insuficiente de Eco-Puntos."
- 5c. El beneficio dejó de estar disponible.
  - 5c.1 El sistema muestra "El beneficio no está disponible."

![Figura CU03-0. Diagrama de casos de uso](diagramas/casos_de_uso/CU03.svg)

![Figura CU03-a. Diagrama de clases](diagramas/cu/CU03_clases.svg)

![Figura CU03-b. Modelo de datos](diagramas/cu/CU03_der.svg)

![Figura CU03-c. Diagrama de secuencia](diagramas/secuencias_png/CU03.png)


### 6. Diagrama de secuencia

Cada caso de uso tiene su secuencia (figuras de CU01 a CU03 arriba).

### 7. Modelo de datos del proceso

![Figura N03-13. Modelo de datos del proceso N03](diagramas/procesos/N03_modelo_datos.svg)

---

