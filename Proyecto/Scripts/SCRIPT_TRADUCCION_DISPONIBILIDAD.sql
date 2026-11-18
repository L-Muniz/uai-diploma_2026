-- ============================================================
-- Traducciones de los mensajes de disponibilidad y bloqueo de usuario
-- Claves: integridad.no_disponible y usuario.bloqueado (ES y EN).
-- Ejecutar a mano sobre la base de datos. Es idempotente.
-- ============================================================

INSERT INTO TRADUCCION (CLAVE, ID_IDIOMA, VALOR)
SELECT v.CLAVE, i.ID_IDIOMA, v.VALOR
FROM (VALUES
    ('integridad.no_disponible', 'ES', N'El servicio no está disponible en este momento. Contacte al administrador.'),
    ('integridad.no_disponible', 'EN', N'The service is not available at this time. Please contact the administrator.'),
    ('usuario.bloqueado',        'ES', N'El usuario está bloqueado. Contacte al administrador.'),
    ('usuario.bloqueado',        'EN', N'This user is locked. Please contact the administrator.')
) AS v (CLAVE, CODIGO, VALOR)
JOIN IDIOMA i ON i.CODIGO = v.CODIGO
WHERE NOT EXISTS (
    SELECT 1 FROM TRADUCCION t
    WHERE t.CLAVE = v.CLAVE AND t.ID_IDIOMA = i.ID_IDIOMA
);
GO

-- Verificación (solo lectura)
SELECT t.CLAVE, i.CODIGO, t.VALOR
FROM TRADUCCION t
JOIN IDIOMA i ON i.ID_IDIOMA = t.ID_IDIOMA
WHERE t.CLAVE IN ('integridad.no_disponible', 'usuario.bloqueado');
GO
