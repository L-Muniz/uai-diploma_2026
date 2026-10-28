# Scripts de la 3ra entrega

Ejecutar sobre la base `Proyecto`, en este orden. Son idempotentes: si se vuelven a ejecutar no duplican datos.

1. `01_TABLAS_ENTREGA3.sql`: tablas de N01, N02 y N03 y datos semilla (materiales, conversión, tarifa general, beneficios).
2. `02_STORED_PROCEDURES_ENTREGA3.sql`: permisos `PROC_*`, roles de negocio (CIUDADANO, OPERADOR_PUNTO_VERDE, GERENCIA, AREA_COMERCIAL) con sus permisos y los stored procedures de los procesos.
3. `03_PERFIL_SUSCRIPCIONES_ESTADISTICAS.sql`: datos del perfil (fecha de nacimiento y ubicación), suscripciones a materiales, estadísticas de ventas del vendedor y sus permisos.
4. `04_AMPLIAR_BITACORA.sql`: amplía `BITACORA.DESCRIPCION` a 500 caracteres (los mensajes de los procesos superan los 50 originales).

5. `05_DATOS_DE_PRUEBA.sql`: usuarios de prueba por rol y datos de ejemplo (materiales, guías, puntos verdes, productos, una entrega pendiente). Contraseña de todos los usuarios de prueba: `Prueba2026!`. Usuarios: `admin_demo` (ADMIN), `ciudadano1` y `ciudadano2` (CIUDADANO; ciudadano2 es vendedor verificado), `operador` (OPERADOR_PUNTO_VERDE), `gerente` (GERENCIA) y `area_comercial` (AREA_COMERCIAL).

**Instalación completa:** en lugar de ejecutar los scripts a mano, usá el instalador (`Proyecto/Instalador`). Ejecuta el script original de la base (sin la creación de la base), los scripts 01 a 05 y copia la aplicación con la conexión configurada.

Requisitos: SQL Server 2016 SP1 o posterior, con compatibilidad 130 o más (usa `CREATE OR ALTER` y `STRING_SPLIT`). Las tablas `USUARIO`, `ROL`, `PERMISO` y `ROL_PERMISO` deben existir (son de la base actual).

Después de ejecutar, asignar los roles de negocio a los usuarios desde la pantalla de usuarios de la aplicación. El rol `USUARIO` ya recibe los permisos del ciudadano.
