-- =====================================================================
-- 3ra entrega, script 05: datos de demostración y usuarios de prueba por rol.
-- Contraseña de TODOS los usuarios de prueba: Prueba2026!
--   admin_demo    -> ADMIN (acceso total)
--   ciudadano1    -> CIUDADANO (declara, canjea, compra; tiene una entrega pendiente)
--   ciudadano2    -> CIUDADANO y vendedor verificado (tiene productos publicados)
--   operador      -> OPERADOR_PUNTO_VERDE
--   gerente       -> GERENCIA
--   area_comercial-> AREA_COMERCIAL
-- Es idempotente: no duplica datos si se ejecuta de nuevo.
-- Ejecutar después de 01, 02, 03 y 04.
-- =====================================================================

DECLARE @HASH VARCHAR(255) = '+Fx3RtpBd/DyU5THZJ654Ng1IfI6sQs635DBYqsE2leR0nVAkCqPgpcTRxhlppOq';

-- ---------- usuarios ----------

DECLARE @usuarios TABLE (USERNAME VARCHAR(50), ROL NVARCHAR(50));
INSERT INTO @usuarios VALUES
    ('admin_demo', N'ADMIN'),
    ('ciudadano1', N'CIUDADANO'),
    ('ciudadano2', N'CIUDADANO'),
    ('operador', N'OPERADOR_PUNTO_VERDE'),
    ('gerente', N'GERENCIA'),
    ('area_comercial', N'AREA_COMERCIAL');

DECLARE @user VARCHAR(50), @rol NVARCHAR(50);
DECLARE cur CURSOR LOCAL FAST_FORWARD FOR SELECT USERNAME, ROL FROM @usuarios;
OPEN cur;
FETCH NEXT FROM cur INTO @user, @rol;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.USUARIO WHERE USERNAME = @user)
        INSERT INTO dbo.USUARIO (ID_USUARIO, USERNAME, PASS, BLOQUEADO, DVH_HORIZONTAL)
        VALUES ((SELECT ISNULL(MAX(ID_USUARIO), 0) + 1 FROM dbo.USUARIO), @user, @HASH, 0, 0);

    INSERT INTO dbo.USUARIO_ROL (ID_USUARIO, ID_ROL)
    SELECT U.ID_USUARIO, R.ID_ROL
    FROM dbo.USUARIO U, dbo.ROL R
    WHERE U.USERNAME = @user AND R.NOMBRE = @rol
      AND NOT EXISTS (SELECT 1 FROM dbo.USUARIO_ROL X WHERE X.ID_USUARIO = U.ID_USUARIO AND X.ID_ROL = R.ID_ROL);

    FETCH NEXT FROM cur INTO @user, @rol;
END
CLOSE cur;
DEALLOCATE cur;
GO

-- El usuario admin_demo también necesita el rol ADMIN en la tabla de roles del sistema (ya está arriba).

-- ---------- datos personales (perfil) ----------

IF NOT EXISTS (SELECT 1 FROM dbo.DATOS_USUARIO D JOIN dbo.USUARIO U ON U.ID_USUARIO = D.ID_USUARIO WHERE U.USERNAME = 'ciudadano1')
    INSERT INTO dbo.DATOS_USUARIO (ID_USUARIO, FECHA_NACIMIENTO, LATITUD, LONGITUD, LOCALIDAD)
    SELECT ID_USUARIO, '1990-05-10', -34.603700, -58.381600, N'CABA' FROM dbo.USUARIO WHERE USERNAME = 'ciudadano1';

IF NOT EXISTS (SELECT 1 FROM dbo.DATOS_USUARIO D JOIN dbo.USUARIO U ON U.ID_USUARIO = D.ID_USUARIO WHERE U.USERNAME = 'ciudadano2')
    INSERT INTO dbo.DATOS_USUARIO (ID_USUARIO, FECHA_NACIMIENTO, LATITUD, LONGITUD, LOCALIDAD)
    SELECT ID_USUARIO, '1985-03-22', -31.420100, -64.188800, N'Córdoba' FROM dbo.USUARIO WHERE USERNAME = 'ciudadano2';
GO

-- ---------- materiales adicionales ----------

IF NOT EXISTS (SELECT 1 FROM dbo.MATERIAL WHERE NOMBRE = N'Aluminio')
    INSERT INTO dbo.MATERIAL (NOMBRE, CATEGORIA, UNIDAD_MEDIDA) VALUES (N'Aluminio', N'Metal', N'kg');
IF NOT EXISTS (SELECT 1 FROM dbo.TABLA_CONVERSION C JOIN dbo.MATERIAL M ON M.ID_MATERIAL = C.ID_MATERIAL WHERE M.NOMBRE = N'Aluminio')
    INSERT INTO dbo.TABLA_CONVERSION (ID_MATERIAL, PUNTOS_POR_KG)
    SELECT ID_MATERIAL, 15 FROM dbo.MATERIAL WHERE NOMBRE = N'Aluminio';
GO

-- ---------- guías de reutilización (una por material) ----------

IF NOT EXISTS (SELECT 1 FROM dbo.GUIA_REUTILIZACION)
BEGIN
    INSERT INTO dbo.GUIA_REUTILIZACION (ID_MATERIAL, TITULO, PASOS, DIFICULTAD)
    SELECT ID_MATERIAL, N'Macetero con cartón', N'1. Recortá el cartón en tiras. 2. Enrollalas formando un vaso. 3. Pegá el borde con cola vinílica. 4. Dejá secar 24 horas.', N'Baja'
    FROM dbo.MATERIAL WHERE NOMBRE = N'Cartón';

    INSERT INTO dbo.GUIA_REUTILIZACION (ID_MATERIAL, TITULO, PASOS, DIFICULTAD)
    SELECT ID_MATERIAL, N'Organizador con papel', N'1. Doblá hojas de papel en forma de cilindro. 2. Pegá el extremo. 3. Juntá varios para formar un organizador de escritorio.', N'Baja'
    FROM dbo.MATERIAL WHERE NOMBRE = N'Papel';

    INSERT INTO dbo.GUIA_REUTILIZACION (ID_MATERIAL, TITULO, PASOS, DIFICULTAD)
    SELECT ID_MATERIAL, N'Maceta con botella plástica', N'1. Cortá la botella a la altura deseada. 2. Hacé agujeros de drenaje en la base. 3. Pintá o decorá.', N'Baja'
    FROM dbo.MATERIAL WHERE NOMBRE = N'Plástico';

    INSERT INTO dbo.GUIA_REUTILIZACION (ID_MATERIAL, TITULO, PASOS, DIFICULTAD)
    SELECT ID_MATERIAL, N'Vaso con frasco de vidrio', N'1. Lavá el frasco y quitá las etiquetas. 2. Pintá el exterior con pintura para vidrio. 3. Dejá secar antes de usar.', N'Media'
    FROM dbo.MATERIAL WHERE NOMBRE = N'Vidrio';

    INSERT INTO dbo.GUIA_REUTILIZACION (ID_MATERIAL, TITULO, PASOS, DIFICULTAD)
    SELECT ID_MATERIAL, N'Portavelas con lata', N'1. Lavá la lata y quitá los bordes filosos. 2. Pintala. 3. Colocá una vela pequeña en el interior.', N'Media'
    FROM dbo.MATERIAL WHERE NOMBRE = N'Metal';

    INSERT INTO dbo.GUIA_REUTILIZACION (ID_MATERIAL, TITULO, PASOS, DIFICULTAD)
    SELECT ID_MATERIAL, N'Bolsa de tela con remera', N'1. Cortá las mangas y el cuello. 2. Cosé la base. 3. Usá los bordes como asas.', N'Media'
    FROM dbo.MATERIAL WHERE NOMBRE = N'Textil';
END
GO

-- ---------- puntos verdes de ejemplo (con materiales aceptados) ----------

IF NOT EXISTS (SELECT 1 FROM dbo.PUNTO_VERDE WHERE NOMBRE = N'Punto Verde Palermo')
    INSERT INTO dbo.PUNTO_VERDE (NOMBRE, DIRECCION, LATITUD, LONGITUD, HORARIO, ACTIVO)
    VALUES (N'Punto Verde Palermo', N'Av. Coronel Díaz 1900, CABA', -34.587500, -58.430600, N'Lun a Vie 8:00 a 16:00', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.PUNTO_VERDE WHERE NOMBRE = N'Cooperativa Reciclar Caballito')
    INSERT INTO dbo.PUNTO_VERDE (NOMBRE, DIRECCION, LATITUD, LONGITUD, HORARIO, ACTIVO)
    VALUES (N'Cooperativa Reciclar Caballito', N'Av. Rivadavia 4500, CABA', -34.618600, -58.441800, N'Lun a Sáb 9:00 a 18:00', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.PUNTO_VERDE WHERE NOMBRE = N'Punto Verde Córdoba Centro')
    INSERT INTO dbo.PUNTO_VERDE (NOMBRE, DIRECCION, LATITUD, LONGITUD, HORARIO, ACTIVO)
    VALUES (N'Punto Verde Córdoba Centro', N'Av. Colón 800, Córdoba', -31.416700, -64.183300, N'Lun a Vie 8:00 a 14:00', 1);
GO

-- Materiales aceptados: cada punto acepta papel, cartón, plástico, vidrio, metal y aluminio según corresponda.
INSERT INTO dbo.PUNTO_VERDE_MATERIAL (ID_PUNTO_VERDE, ID_MATERIAL, ACEPTADO)
SELECT P.ID_PUNTO_VERDE, M.ID_MATERIAL, 1
FROM dbo.PUNTO_VERDE P
JOIN dbo.MATERIAL M ON M.NOMBRE IN (N'Papel', N'Cartón', N'Plástico', N'Vidrio', N'Metal', N'Aluminio')
WHERE P.NOMBRE IN (N'Punto Verde Palermo', N'Cooperativa Reciclar Caballito', N'Punto Verde Córdoba Centro')
  AND NOT EXISTS (SELECT 1 FROM dbo.PUNTO_VERDE_MATERIAL X WHERE X.ID_PUNTO_VERDE = P.ID_PUNTO_VERDE AND X.ID_MATERIAL = M.ID_MATERIAL);
GO

-- ---------- verificación y productos de ciudadano2 (vendedor verificado) ----------

IF NOT EXISTS (SELECT 1 FROM dbo.VERIFICACION_VENDEDOR V JOIN dbo.USUARIO U ON U.ID_USUARIO = V.ID_USUARIO WHERE U.USERNAME = 'ciudadano2')
    INSERT INTO dbo.VERIFICACION_VENDEDOR (ID_USUARIO, ESTADO, DOCUMENTO_REF)
    SELECT ID_USUARIO, N'APROBADA', N'DNI verificado (datos de prueba)' FROM dbo.USUARIO WHERE USERNAME = 'ciudadano2';

IF NOT EXISTS (SELECT 1 FROM dbo.PRODUCTO WHERE NOMBRE = N'Cuaderno de papel reciclado')
    INSERT INTO dbo.PRODUCTO (ID_VENDEDOR, ID_TARIFA, NOMBRE, DESCRIPCION, CATEGORIA, PRECIO, STOCK, ESTADO)
    SELECT U.ID_USUARIO,
           (SELECT TOP 1 ID_TARIFA FROM dbo.TARIFA_COMISION WHERE CATEGORIA = N'General' ORDER BY VIGENTE_DESDE DESC, ID_TARIFA DESC),
           N'Cuaderno de papel reciclado', N'Tapa de cartón y hojas de papel reciclado', N'General', 1500.00, 20, N'ACTIVO'
    FROM dbo.USUARIO U WHERE U.USERNAME = 'ciudadano2';

IF NOT EXISTS (SELECT 1 FROM dbo.PRODUCTO WHERE NOMBRE = N'Bolso de tela reciclada')
    INSERT INTO dbo.PRODUCTO (ID_VENDEDOR, ID_TARIFA, NOMBRE, DESCRIPCION, CATEGORIA, PRECIO, STOCK, ESTADO)
    SELECT U.ID_USUARIO,
           (SELECT TOP 1 ID_TARIFA FROM dbo.TARIFA_COMISION WHERE CATEGORIA = N'General' ORDER BY VIGENTE_DESDE DESC, ID_TARIFA DESC),
           N'Bolso de tela reciclada', N'Hecho con remeras en desuso', N'General', 4500.00, 10, N'ACTIVO'
    FROM dbo.USUARIO U WHERE U.USERNAME = 'ciudadano2';

IF NOT EXISTS (SELECT 1 FROM dbo.PRODUCTO WHERE NOMBRE = N'Vaso de vidrio reusado')
    INSERT INTO dbo.PRODUCTO (ID_VENDEDOR, ID_TARIFA, NOMBRE, DESCRIPCION, CATEGORIA, PRECIO, STOCK, ESTADO)
    SELECT U.ID_USUARIO,
           (SELECT TOP 1 ID_TARIFA FROM dbo.TARIFA_COMISION WHERE CATEGORIA = N'General' ORDER BY VIGENTE_DESDE DESC, ID_TARIFA DESC),
           N'Vaso de vidrio reusado', N'Frasco reciclado y pintado a mano', N'General', 2200.00, 15, N'ACTIVO'
    FROM dbo.USUARIO U WHERE U.USERNAME = 'ciudadano2';
GO

-- ---------- una declaración pendiente de entrega para probar la recepción en punto verde ----------

IF NOT EXISTS (SELECT 1 FROM dbo.DECLARACION_MATERIAL D JOIN dbo.USUARIO U ON U.ID_USUARIO = D.ID_USUARIO
               WHERE U.USERNAME = 'ciudadano1' AND D.ESTADO = N'PENDIENTE_ENTREGA')
    INSERT INTO dbo.DECLARACION_MATERIAL (ID_USUARIO, ID_MATERIAL, CANTIDAD, ESTADO)
    SELECT U.ID_USUARIO, M.ID_MATERIAL, 4.00, N'PENDIENTE_ENTREGA'
    FROM dbo.USUARIO U, dbo.MATERIAL M
    WHERE U.USERNAME = 'ciudadano1' AND M.NOMBRE = N'Papel';
GO
