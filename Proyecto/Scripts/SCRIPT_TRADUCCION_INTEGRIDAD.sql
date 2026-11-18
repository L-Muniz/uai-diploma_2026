-- ============================================================
-- Traducción del ítem de menú "Integridad / DV" (clave INTEGRIDAD)
-- MainForm.ActualizarItemMenu usa el Tag "INTEGRIDAD" para buscar el texto.
-- Ejecutar a mano sobre la base de datos. Es idempotente.
-- ============================================================

INSERT INTO TRADUCCION (CLAVE, ID_IDIOMA, VALOR)
SELECT v.CLAVE, i.ID_IDIOMA, v.VALOR
FROM (VALUES
    ('INTEGRIDAD', 'ES', N'Integridad / DV'),
    ('INTEGRIDAD', 'EN', N'Integrity / DV')
) AS v (CLAVE, CODIGO, VALOR)
JOIN IDIOMA i ON i.CODIGO = v.CODIGO
WHERE NOT EXISTS (
    SELECT 1 FROM TRADUCCION t
    WHERE t.CLAVE = v.CLAVE AND t.ID_IDIOMA = i.ID_IDIOMA
);
GO

-- Verificación (solo lectura): debe devolver una fila por idioma.
SELECT t.CLAVE, i.CODIGO, t.VALOR
FROM TRADUCCION t
JOIN IDIOMA i ON i.ID_IDIOMA = t.ID_IDIOMA
WHERE t.CLAVE = 'INTEGRIDAD';
GO
