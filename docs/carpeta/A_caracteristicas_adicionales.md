# A. Características y funcionalidades adicionales

En esta sección se describen las características que el software tiene al final del proceso de desarrollo.

## A01. Instalador

**Objetivo.** Instalar el sistema completo con la mínima intervención del usuario: motor de base de datos, base, tablas, procedimientos, datos de prueba y la aplicación.

**Cómo funciona.** El instalador es un único ejecutable (`Instalador.exe`) que incluye la aplicación y los scripts de la base. Sigue estos pasos:

1. Verifica la carpeta de destino (que se pueda escribir en ella).
2. Verifica el motor de SQL Server y su servicio. Si el servicio está detenido, intenta iniciarlo.
3. Se conecta a la instancia y crea la base `Proyecto` si no existe.
4. Crea las tablas y los datos base con el script original, solo si la base no tiene tablas todavía.
5. Ejecuta los scripts de la 3ra entrega (01 a 05): tablas de negocio, procedimientos, datos de prueba y cálculo de dígitos verificadores.
6. Copia la aplicación a la carpeta de destino, configura la cadena de conexión y crea un acceso directo en el escritorio con el ícono de la aplicación.

El instalador muestra el avance paso a paso en su ventana.

**Casos especiales.**

- *No hay ninguna instancia de SQL Server.* El instalador lo informa y ofrece abrir la descarga oficial de SQL Server Express.
- *No existe el motor de base de datos.* Es el mismo caso: se informa y se ofrece la descarga.
- *El servicio de SQL no está levantado.* El instalador intenta iniciarlo durante 30 segundos. Si no puede, pide iniciarlo desde Servicios de Windows o ejecutar el instalador como administrador.
- *La instancia no responde.* Informa el nombre de la instancia y el detalle del error.
- *La carpeta de destino no se puede escribir.* Pide elegir otra carpeta o ejecutar como administrador.
- *Un script falla.* La instalación se detiene e informa el script y la sentencia que falló.

**Qué no contempla.** Instalar el motor de SQL Server, revertir una instalación parcial, o verificar que el usuario tenga permisos para crear bases.

## A02. Informe y exportación a PDF

**Objetivo.** Generar reportes en PDF con una librería de terceros.

**Cómo funciona.** Se usa PdfSharp (versión 1.50.5147), que dibuja el documento directamente, sin impresora virtual. El generador (`SV_GeneradorPdf`) crea una tabla con título, subtítulo, columnas, filas y pie, y pasa a una página nueva cuando la tabla no entra.

Hay dos reportes:

- **Indicadores de gestión.** Desde Gestión → Indicadores se elige el período y se exporta a PDF. El reporte incluye el acopio por material, las ventas y el costo de los canjes.
- **Estadísticas de mis ventas.** Desde Mi cuenta → Estadísticas de mis ventas, el vendedor elige un rango de fechas y exporta un resumen y el detalle por producto.

## A03. Serialización

**Objetivo.** Generar un archivo serializado con la información del informe de indicadores, y leerlo de vuelta. Si el archivo no es válido, mostrar una excepción específica.

**Cómo funciona.**

- *Exportar XML.* Desde Gestión → Indicadores, **Exportar XML** guarda el informe con `XmlSerializer`. La raíz es `BE_InformeIndicadores` y contiene la versión, el período, la fecha de generación y la lista de indicadores (nombre y valor).
- *Importar XML.* **Importar XML** lee el archivo, verifica que tenga período e indicadores, y muestra los indicadores en la grilla. El dato se muestra tal como viene en el archivo, sin recalcularlo.
- *Archivo inválido.* Si el archivo no es un XML de informe, se lanza `SV_ArchivoInvalidoException` con el mensaje "El archivo no es un informe de indicadores válido". Si existe pero le faltan datos, el mensaje es "El archivo no contiene un informe de indicadores completo".
