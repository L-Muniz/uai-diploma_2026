-- =====================================================================
-- 3ra entrega, script 04: amplía la descripción de la bitácora.
-- La columna original es VARCHAR(50) y los mensajes de los procesos de negocio
-- superan ese largo. Ampliarla no pierde datos. Es idempotente.
-- Ejecutar después de 01, 02 y 03.
-- =====================================================================

IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
           WHERE TABLE_NAME = 'BITACORA' AND COLUMN_NAME = 'DESCRIPCION'
             AND CHARACTER_MAXIMUM_LENGTH < 500)
    ALTER TABLE dbo.BITACORA ALTER COLUMN DESCRIPCION VARCHAR(500) NOT NULL;
GO
