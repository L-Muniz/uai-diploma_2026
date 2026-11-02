# G. Descripción global del producto

En este grupo se describe el producto con características de alto nivel, para entender el negocio y el sistema de forma global. Parte de la planificación estratégica de BecoGre, ajustada a lo que efectivamente se implementó.

## G01. Propósito

BecoGre es una plataforma que conecta a las personas con el reciclaje y la reutilización de materiales, y que apoya a la empresa en sus decisiones sobre la red de acopio y los incentivos.

El sistema busca:

- Facilitar que cada persona declare los materiales que puede reciclar o reutilizar, y reciba una recomendación: reutilizarlos en casa con una guía paso a paso, o llevarlos a un punto verde cercano.
- Reconocer el reciclaje con Eco-Puntos, que se canjean por beneficios (cupones y envíos bonificados).
- Ofrecer un espacio de venta de productos elaborados con materiales reciclados, con comisión para la empresa.
- Dar a la gerencia la información para decidir sobre la red de puntos verdes, las comisiones y la tabla de conversión de Eco-Puntos.

El proyecto aporta al Objetivo de Desarrollo Sostenible 13 (acción por el clima) y, en particular, a la reducción de residuos enviados a disposición final.

## G02. Descripción funcional del producto y alcance

El producto se organiza en gestiones del negocio. Cada una se detalla en el grupo N.

| Gestión | Descripción | Proceso |
|---|---|---|
| Reconocimiento por reciclaje y canje | El ciudadano declara materiales, recibe una recomendación, entrega en un punto verde y gana Eco-Puntos. Puede canjearlos por beneficios. El costo de los beneficios es un egreso para la empresa. | N03 |
| Venta de productos sustentables | Los vendedores verificados publican productos, el área comercial los aprueba y los compradores los pagan. La empresa cobra una comisión por venta, y paga a los vendedores el saldo neto una vez confirmada la recepción. | N02 |
| Gestión de la red y apoyo a la decisión | La gerencia administra puntos verdes, materiales aceptados, comisiones y conversión de Eco-Puntos, y consulta indicadores de acopio, ventas y costo de canjes para decidir. | N01 |

**Alcance.** Hasta donde llega la solución:

- Incluye: declaración de materiales, recomendación, acopio en puntos verdes, Eco-Puntos y canjes, verificación de vendedores, publicación y aprobación de productos, compra con pago simulado, despacho, confirmación de recepción, liquidación, reclamos, indicadores con exportación a PDF y XML, estadísticas de ventas, guías de reutilización y funciones técnicas (log-in, perfiles y permisos, idiomas, bitácora, backup y dígitos verificadores).
- No incluye: aplicación móvil, feed de contenido de perfiles, programa de niveles de usuario, pasarela de pago real (el pago se simula) y servicio de envíos. Son trabajo a futuro.
- Ámbito: piloto en AMBA (CABA y GBA), tal como se planteó en la planificación.

## G03. Definiciones, acrónimos y abreviaturas

**Definiciones**

- **Punto verde:** lugar que recibe materiales reciclables. Acepta materiales determinados y tiene horario y ubicación.
- **Declaración:** registro de un material y una cantidad (en kg) que una persona declara tener para reciclar o reutilizar.
- **Recomendación:** opción que el sistema ofrece al declarar: reutilizar con una guía, o entregar en el punto verde más cercano.
- **Guía de reutilización:** instrucciones paso a paso para reutilizar un material en casa.
- **Acopio:** recepción de un material en un punto verde, con su peso.
- **Eco-Puntos:** unidad de reconocimiento. Se acreditan por reciclaje y se descuentan al canjear.
- **Canje:** cambio de Eco-Puntos por un beneficio.
- **Beneficio:** cupón o envío bonificado que se canjea con Eco-Puntos. Tiene un costo para la empresa.
- **Comisión:** porcentaje que la empresa cobra sobre cada venta, según la categoría del producto.
- **Verificación de vendedor:** aprobación previa que necesita una persona para publicar productos.
- **Liquidación:** saldo neto que la empresa paga al vendedor por una venta, después de la comisión.
- **Reclamo:** solicitud de un comprador sobre un pedido. Congela la liquidación hasta que se resuelve.
- **Dígito verificador (DVH / DVV):** código que permite detectar cambios en los datos almacenados (T08).

**Acrónimos y abreviaturas**

- **AMBA:** Área Metropolitana de Buenos Aires.
- **CABA / GBA:** Ciudad Autónoma de Buenos Aires / Gran Buenos Aires.
- **BD:** base de datos.
- **DVH / DVV:** dígito verificador horizontal / vertical.
- **ODS:** Objetivo de Desarrollo Sostenible.
- **PDF / XML:** formatos de exportación de informes.

## G04. Personas participantes y roles de usuario

**Participantes en el desarrollo.** Autora del proyecto: Lucía Muñiz, responsable de la planificación, el análisis, el diseño, la implementación, las pruebas y la documentación.

**Roles de usuario del sistema.** Cada usuario tiene un rol que define qué puede hacer.

| Rol | Quién es | Acceso principal |
|---|---|---|
| CIUDADANO | Persona que recicla y compra o vende productos sustentables | Declarar materiales, canjear Eco-Puntos, verificarse como vendedor, publicar, comprar, reclamar, ver sus estadísticas de ventas y sus materiales suscriptos |
| OPERADOR_PUNTO_VERDE | Persona que atiende un punto verde | Registrar recepciones de materiales y acreditar Eco-Puntos |
| GERENCIA | Responsable de la red y de la política comercial | Indicadores, puntos verdes, tarifas y conversión, materiales y guías de reutilización |
| AREA_COMERCIAL | Responsable de la tienda | Aprobar vendedores y productos, liquidar a vendedores, resolver reclamos y administrar beneficios |
| ADMIN | Administrador del sistema | Acceso total, usuarios, permisos, idiomas, bitácora, backups e integridad |

## G05. Otros requisitos

**Del producto**

- *Estándares aplicables.* Código en C# con programación orientada a objetos. Acceso a datos por procedimientos almacenados. Contraseñas con hash PBKDF2-SHA256 (T03).
- *Requisitos de sistema.* Windows (probado en Windows 11). .NET Framework 4.8. SQL Server (Express o instancia existente) con la base `Proyecto`.
- *Requisitos de desempeño.* Los backups automáticos se ejecutan cada 30 minutos por defecto, configurable.
- *Requisitos de entorno.* Una sola computadora con SQL Server local. Los backups se guardan en una carpeta configurable de la aplicación.

**De documentación**

- *Ayuda en línea (D02).* Se abre con la tecla F1 desde cualquier pantalla.
- *Material de apoyo al usuario final (D03).* Guías del usuario, de operación y de mantenimiento.
- *Manual de instalación (D01).* No se incluye en esta carpeta, por indicación de la cátedra.
