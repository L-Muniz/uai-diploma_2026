# T. Documentos de aspectos técnicos que provee el sistema de información

Las gestiones de este grupo son parte integral del sistema. Para cada una se describe el objetivo, cómo funciona, el diagrama de clases, el DER y, si corresponde, la secuencia.

## T02. Gestión de log-in y log-out

**Objetivo.** Verificar la identidad del usuario con su nombre de usuario y su contraseña, y asignarle el perfil que tiene en el sistema. Define la política de ingreso y de salida, y los procesos que se ejecutan al arrancar, al ingresar y al salir.

**Cómo funciona.**

- *Arranque.* Antes de mostrar la ventana de ingreso, la aplicación verifica la integridad de la base (dígitos verificadores, T08) y hace un backup inicial. Si detecta un error de integridad, informa al administrador y le ofrece restaurar desde backup o recalcular los dígitos verificadores.
- *Ingreso.* El usuario escribe su nombre de usuario y su contraseña. El sistema:
  1. Rechaza a los usuarios bloqueados.
  2. Verifica la contraseña contra el hash guardado (T03).
  3. Si es correcta, carga la sesión en `SV_Singleton` y muestra el menú que corresponde a sus permisos (T04).
- *Política de intentos.* Tres intentos fallidos consecutivos bloquean la cuenta. El bloqueo queda registrado en la bitácora y en el control de cambios (T06). Solo un administrador puede desbloquear la cuenta, desde Gestión de usuarios.
- *Salida.* El usuario elige "Cerrar sesión" y confirma. El sistema limpia la sesión, registra el evento en la bitácora y vuelve a la ventana de ingreso.

**Patrón.** La sesión se guarda en una única instancia (singleton, `SV_Singleton`), a la que acceden las capas de negocio y de interfaz.

**Diagrama de clases y secuencia.** Ver G06 (capa BLL: `BLL_USUARIO`) y la secuencia de ingreso en el anexo de diagramas.

## T03. Gestión de encriptado

**Objetivo.** Proteger los datos sensibles del sistema. Las contraseñas no se guardan en texto plano: se guardan como hash con sal.

**Cómo funciona.**

- *Hash de contraseñas.* Se usa PBKDF2 con SHA-256, 100.000 iteraciones y una sal aleatoria de 16 bytes por usuario. El valor guardado tiene 48 bytes codificados en base 64 (`BLL_GESTOR.HashPassword`).
- *Verificación en el ingreso.* Se recalcula el hash con la sal guardada y se compara con el valor almacenado. La contraseña nunca se compara en texto plano.
- *Datos sensibles.* Las contraseñas son el dato sensible que el sistema protege. No se aplica encriptado simétrico a otros datos porque la base y los backups quedan bajo control del administrador.
- *Políticas de recupero.* Si un usuario olvida su contraseña, no se puede recuperar el valor original: el administrador la restablece. Ante un fallo de la base, la recuperación es por backup (T07) y por verificación de integridad (T08).

## T04. Gestión de perfiles de usuario

**Objetivo.** Asignar permisos de forma rápida y definir perfiles desde el sistema.

**Cómo funciona.**

- *Permisos atómicos.* Cada funcionalidad tiene un permiso con un código único (por ejemplo, `PROC_COMPRAR` para comprar o `PROC_DECLARAR_MATERIAL` para declarar material).
- *Permisos compuestos.* Un permiso puede agrupar otros permisos (patrón composite: `BE_PermisoCompuesto` y `BE_PermisoSimple`, ambos con la interfaz `I_ComponenteAcceso`). La asignación de permisos a los perfiles se hace desde la pantalla de administración de permisos.
- *Perfiles.* Un perfil es un permiso compuesto que se asigna a un usuario. Los perfiles también pueden tener un perfil padre del que heredan permisos.
- *Administración.* Desde el menú de administración, el administrador asigna y quita permisos a un perfil. Cada cambio queda registrado en la bitácora (T06).
- *Roles de la 3ra entrega.* Los procesos de negocio usan roles (CIUDADANO, OPERADOR_PUNTO_VERDE, GERENCIA, AREA_COMERCIAL). El menú se arma con los permisos del rol del usuario.

**Diagrama de clases.** Ver G06 (capas BE y BLL: `BE_Permiso`, `BLL_PERMISO`, `BLL_PERFIL`).

## T05. Gestión de múltiples idiomas

**Objetivo.** Cambiar el idioma de todas las leyendas y títulos de la interfaz, de forma dinámica, y permitir agregar idiomas desde el sistema.

**Cómo funciona.**

- Las traducciones están en la base de datos, con una clave por leyenda y un código de idioma. No hay archivos de recursos estáticos.
- `SV_IdiomaManager` (singleton) mantiene el idioma activo y avisa a los formularios registrados (patrón observer, `ISV_IdiomaObserver`). Cada formulario se actualiza al cambiar el idioma, sin reiniciar la aplicación.
- Si una clave no tiene traducción en el idioma activo, se muestra la clave entre corchetes (por ejemplo, `[LOGIN_TITULO]`).
- Agregar un idioma (nombre y código, y las traducciones) es una función de administración. Los usuarios sin permiso de administración no ven esa opción.

## T06. Gestión de bitácora y control de cambios

### T06a. Bitácora

**Objetivo.** Registrar las operaciones que realizan los usuarios, para trazar su actividad dentro de la aplicación.

**Cómo funciona.** Cada operación relevante genera un registro con fecha, hora, usuario, actividad y descripción (hasta 500 caracteres). La bitácora se consulta desde el menú Ver, con filtros combinados por usuario, rango de fechas, tipo de evento y texto de la descripción. Si una operación no puede registrarse en la bitácora, la operación principal no se cancela: el error queda en el registro de diagnóstico.

### T06b. Control de cambios

**Objetivo.** Trazar los cambios de una entidad durante su ciclo de vida: quién, cuándo y qué cambió, con posibilidad de recomponer el estado anterior.

**Cómo funciona.** Cada cambio sobre una entidad registra el valor anterior y el nuevo, el usuario y la fecha (`BLL_ControlCambios.RegistrarCambio`). Desde el formulario de auditoría se consultan los cambios de una entidad elegida. Antes de cambios críticos (por ejemplo, un bloqueo) se guarda un snapshot que permite volver al estado anterior.

## T07. Gestión de backup

**Objetivo.** Administrar las copias de seguridad: el catálogo de backups y los archivos que contienen la información.

**Cómo funciona.**

- *Realizar backup.* El sistema genera un script SQL con los datos de la base y lo guarda como `BD_BACKUP_AAAA-MM-DD_HH-mm-ss.sql`. El nombre incluye la fecha y la hora.
- *Backup automático.* La aplicación hace un backup cada 30 minutos por defecto. El intervalo se configura en `BackupIntervalMinutes`.
- *Catálogo.* El formulario de backups lista los archivos disponibles y muestra su tamaño en MB.
- *Restaurar.* El administrador elige un backup y el sistema ejecuta el script. La restauración reemplaza los datos actuales, por eso se pide confirmación. Al terminar, se recomienda reiniciar la aplicación.
- *Error al restaurar.* Si el script falla a mitad de camino, la base puede quedar inconsistente. El administrador debe volver a restaurar o informar a soporte.

## T08. Gestión de dígitos verificadores

**Objetivo.** Comprobar la integridad de los datos de la base: detectar datos agregados o quitados fuera del sistema, y datos intercambiados de posición.

**Cómo funciona.**

- *Dígito verificador horizontal (DVH).* Se guarda en cada usuario. Se calcula sobre el nombre de usuario, la contraseña (hash) y el estado de bloqueo. Cada carácter aporta su código multiplicado por su posición, en módulo 11. El dígito es `(11 - suma mod 11) mod 11`.
- *Dígito verificador vertical (DVV).* Se guarda en una tabla propia (`DIGITO_VERIFICADOR_VERTICAL`). Por cada atributo, se suman los dígitos de todas las filas (en orden de ID), con la posición de la fila y la del carácter, en módulo 11. Así se detecta un cambio de posición de datos.
- *Verificación al arranque.* Antes del ingreso, se verifican los DVH y los DVV. Si hay un error, el administrador elige restaurar desde backup o recalcular los dígitos verificadores.
- *Recálculo.* Al crear usuarios de prueba, el script de datos calcula sus DVH y el DVV de la tabla. Así la tabla queda íntegra desde la instalación.
