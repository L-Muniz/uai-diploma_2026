# D. Documentación adicional

En esta sección se describen los documentos que acompañan al sistema al finalizar el desarrollo. El manual de instalación (D01) no se incluye en esta carpeta, por indicación de la cátedra.

## D02. Ayuda en línea

**Objetivo.** Brindar ayuda sobre las funcionalidades más relevantes, sin salir de la aplicación.

**Cómo funciona.** Desde el menú **Ayuda → Ayuda en línea**, o con la tecla **F1**, se abre una ventana con una lista de temas a la izquierda y el texto del tema seleccionado a la derecha. Los temas corresponden a las funciones de cada grupo del menú: Reciclar (declarar material, materiales que sigo, guías), Tienda (canje, publicar, comprar), Mi cuenta (perfil, verificación, pedidos y liquidaciones, reclamos, estadísticas), Gestión (recepción, indicadores, puntos verdes, tarifas, catálogos) y primeros pasos.

## D03. Material de apoyo al usuario final

El material se organiza en tres guías.

### Guía del usuario

Describe cómo usar el sistema según el rol. El detalle de cada pantalla está en el anexo de formularios.

- **Ciudadano:** declarar un material y elegir entre reutilizarlo con una guía o entregarlo en un punto verde. Materiales que sigo, para ver las guías de los materiales suscriptos. Canjear Eco-Puntos. Verificarse como vendedor, publicar productos y comprar. Pedidos y liquidaciones, reclamos y estadísticas de ventas desde Mi cuenta.
- **Operador de punto verde:** elegir el punto verde, seleccionar la declaración pendiente y registrar el peso recibido. Los Eco-Puntos se acreditan al ciudadano al confirmar.
- **Gerencia:** consultar indicadores por período y exportarlos. Administrar puntos verdes, tarifas y conversión de Eco-Puntos, materiales y guías de reutilización.
- **Área comercial:** aprobar o rechazar solicitudes de vendedores, aprobar productos, liquidar a vendedores, resolver reclamos y administrar beneficios.
- **Administrador:** usuarios, permisos, idiomas, bitácora, backups e integridad de datos.

### Guía de operación

Describe el uso diario del sistema por parte del administrador.

- **Ingreso.** Usuario y contraseña. Tres intentos fallidos bloquean la cuenta; solo el administrador la desbloquea.
- **Backups.** Se hacen automáticamente cada 30 minutos. Para hacer uno manual, desde Ver → Backups, "Realizar backup". Para restaurar, elegir el archivo y confirmar: los datos actuales se reemplazan.
- **Integridad.** Al iniciar, el sistema verifica los dígitos verificadores. Si detecta un error, el administrador decide entre restaurar desde backup o recalcular.
- **Bitácora.** Desde Ver → Bitácora, consultar las operaciones con filtros por usuario, fechas, tipo de evento y texto.

### Guía de mantenimiento

Describe las tareas técnicas que mantienen el sistema en funcionamiento.

- **Base de datos.** Los scripts de la 3ra entrega (01 a 06) son idempotentes y pueden volver a ejecutarse. Los procedimientos se actualizan con `CREATE OR ALTER`.
- **Permisos.** Si cambia la asignación de permisos de un rol, se ejecuta de nuevo el script 02.
- **Dígitos verificadores.** Si se modifican datos fuera de la aplicación, el sistema lo detecta al iniciar. Para recalcularlos, desde el menú de administración: Integridad / DV.
- **Copias de seguridad.** Guardar periódicamente la carpeta de backups fuera de la computadora.

## Anexo: formularios

El detalle de cada formulario (propósito, roles y controles principales) está en `docs/formularios.md`.
