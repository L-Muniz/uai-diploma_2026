-- ============================================================
-- Preferencia de idioma por usuario
-- Agrega la columna CODIGO_IDIOMA a USUARIO y los procedimientos
-- que usa la capa DAL (MP_USUARIO.ObtenerIdiomaPreferido /
-- MP_USUARIO.ActualizarIdiomaPreferido).
-- Ejecutar a mano sobre la base de datos. Es idempotente.
-- ============================================================

IF COL_LENGTH('dbo.USUARIO', 'CODIGO_IDIOMA') IS NULL
BEGIN
    ALTER TABLE dbo.USUARIO ADD CODIGO_IDIOMA NVARCHAR(5) NULL;
END
GO

CREATE OR ALTER PROCEDURE dbo.USUARIO_OBTENER_IDIOMA
    @ID_USUARIO INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CODIGO_IDIOMA
    FROM dbo.USUARIO
    WHERE ID_USUARIO = @ID_USUARIO;
END;
GO

CREATE OR ALTER PROCEDURE dbo.USUARIO_ACTUALIZAR_IDIOMA
    @ID_USUARIO INT,
    @CODIGO_IDIOMA NVARCHAR(5)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.USUARIO
    SET CODIGO_IDIOMA = @CODIGO_IDIOMA
    WHERE ID_USUARIO = @ID_USUARIO;
END;
GO

-- Verificación (solo lectura): debe devolver la columna nueva y los dos procedimientos.
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'USUARIO' AND COLUMN_NAME = 'CODIGO_IDIOMA';
GO
