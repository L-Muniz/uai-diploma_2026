using System;
using System.Collections.Generic;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace SERVICE
{
    // Genera informes en PDF con una tabla (PdfSharp, librería de terceros).
    public static class SV_GeneradorPdf
    {
        private const double Margen = 40;

        public static void GenerarTabla(string ruta, string titulo, string subtitulo,
                                        string[] columnas, IList<string[]> filas, string pie = null)
        {
            if (string.IsNullOrWhiteSpace(ruta)) throw new ArgumentException("Indique la ruta del archivo.");
            if (columnas == null || columnas.Length == 0) throw new ArgumentException("El informe no tiene columnas.");

            var fTitulo = new XFont("Arial", 16, XFontStyle.Bold);
            var fTexto = new XFont("Arial", 10, XFontStyle.Regular);
            var fCabecera = new XFont("Arial", 10, XFontStyle.Bold);

            using (PdfDocument doc = new PdfDocument())
            {
                doc.Info.Title = titulo;
                PdfPage pagina = doc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(pagina);
                double ancho = pagina.Width.Point - 2 * Margen;
                double colAncho = ancho / columnas.Length;
                double y = Margen;

                gfx.DrawString(titulo, fTitulo, XBrushes.Black, new XRect(Margen, y, ancho, 24), XStringFormats.TopLeft);
                y += 28;
                if (!string.IsNullOrWhiteSpace(subtitulo))
                {
                    gfx.DrawString(subtitulo, fTexto, XBrushes.DimGray, new XRect(Margen, y, ancho, 16), XStringFormats.TopLeft);
                    y += 22;
                }

                y = DibujarFila(gfx, columnas, fCabecera, XBrushes.LightGray, y, colAncho);

                foreach (string[] fila in filas)
                {
                    if (y > pagina.Height.Point - 60)
                    {
                        pagina = doc.AddPage();
                        gfx = XGraphics.FromPdfPage(pagina);
                        y = Margen;
                        y = DibujarFila(gfx, columnas, fCabecera, XBrushes.LightGray, y, colAncho);
                    }
                    y = DibujarFila(gfx, fila, fTexto, XBrushes.White, y, colAncho);
                }

                if (!string.IsNullOrWhiteSpace(pie))
                {
                    gfx.DrawString(pie, fTexto, XBrushes.DimGray, new XRect(Margen, y + 12, ancho, 30), XStringFormats.TopLeft);
                }

                doc.Save(ruta);
            }
        }

        private static double DibujarFila(XGraphics gfx, string[] celdas, XFont fuente, XBrush fondo, double y, double colAncho)
        {
            const double alto = 18;
            for (int i = 0; i < celdas.Length; i++)
            {
                XRect celda = new XRect(Margen + i * colAncho, y, colAncho, alto);
                gfx.DrawRectangle(XPens.Gray, fondo, celda);
                gfx.DrawString(celdas[i] ?? "", fuente, XBrushes.Black,
                               new XRect(celda.X + 3, celda.Y + 3, colAncho - 6, alto - 4), XStringFormats.TopLeft);
            }
            return y + alto;
        }
    }
}
