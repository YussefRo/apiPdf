using apiPDF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iText.Layout.Element;
using Document = iText.Layout.Document;
using iText.Layout.Properties;
using iText.Layout.Borders;


namespace apiPDF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PdfController(AppDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTb_detalle_demoras()
        {
            // Extrae la informacion de la tabla de la base de datos
            var datos = await _context.Tb_Detalle_Demoras.ToListAsync();
            return Ok(datos);
        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetTb_detalle_demorasPdf()
        {
            // Extrae la informacion de la tabla de la base de datos
            var datos = await _context.Tb_Detalle_Demoras.ToListAsync();

            // Fecha del dia
            DateTime fecha = DateTime.Today;

            using (var stream = new MemoryStream())
            {
                // Lectura del PDF con el formato necesario
                iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader("resumen_demora1.pdf");
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(stream);
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(pdfReader,writer);
             
                Document document = new Document(pdf);
                iText.Forms.PdfAcroForm form = iText.Forms.PdfAcroForm.GetAcroForm(pdf, true);


                // Lectura del cuadro de texto de formulario para ingresar informacion
                iText.Forms.Fields.PdfFormField nomFecha = form.GetField("Text1");
                // Asigancion del valor de fecha al campo
                nomFecha.SetValue(fecha.ToString());

                // Cracion de tabla para ingresar la informacion
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1 })).UseAllAvailableWidth();
                
                // creacion del encabezado de cada columna, se le da formato al texto y a la tabla
                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Tren").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10)) // tamaño y texto centrado
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER)); // Sin borde para el encabezado

                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Región").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER)); 

                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Distrito").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER)); 

                // Iteracion para imprimir los datos obtenidos de la tabla de la base de datos
                foreach ( var x in datos )
                {
                    // filtrado de los datos, se le indica que no compare entre mayusculas y minusculas
                    if (x.Distrito.Equals("ZACATECAS", StringComparison.OrdinalIgnoreCase))
                    {
                        // Asignacion de las filas de acorde a su columna 
                        table.AddCell(new Cell()
                        .Add(new Paragraph(x.Tren)
                        .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                        .SetBorder(Border.NO_BORDER)); // sin bordes

                        table.AddCell(new Cell()
                            .Add(new Paragraph(x.Region)
                            .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                            .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE)
                            .SetBorder(Border.NO_BORDER));

                        table.AddCell(new Cell()
                            .Add(new Paragraph(x.Distrito)
                            .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                            .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE)
                            .SetBorder(Border.NO_BORDER));
                    }
                    
                }

                // Salto de linea para pocisionar adecuadamente la tabla creada
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("\n"));
                // Agregar la tabla al documento
                document.Add(table);
                // Instruccion para que la fecha agregada al campo de formulario no pueda ser modificada
                nomFecha.SetReadOnly(true);
                form.FlattenFields();
                // Cerramos el archivo
                pdf.Close();
                // Regresamos el archivo Pdf con el nombre de Datos.pdf
                return File(stream.ToArray(), "application/pdf", "Datos.pdf");

            }
        }


        [HttpGet("pdf2")]
        public async Task<IActionResult> GetTb_detalle_demorasPdf2()
        {
            // Extrae la informacion de la tabla de la base de datos
            var datos = await _context.Tb_Detalle_Demoras.ToListAsync();

            // Fecha del dia
            DateTime fecha = DateTime.Today;

            using (var stream = new MemoryStream())
            {
                // Lectura del PDF con el formato necesario
                iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader("resumen_demora1.pdf");
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(stream);
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(pdfReader, writer);

                Document document = new Document(pdf);
                iText.Forms.PdfAcroForm form = iText.Forms.PdfAcroForm.GetAcroForm(pdf, true);


                // Lectura del cuadro de texto de formulario para ingresar informacion
                iText.Forms.Fields.PdfFormField nomFecha = form.GetField("Text1");
                // Asigancion del valor de fecha al campo
                nomFecha.SetValue(fecha.ToString());
                // Instruccion para que la fecha agregada al campo de formulario no pueda ser modificada
                nomFecha.SetReadOnly(true);
                form.FlattenFields();

                // Cracion de tabla para ingresar la informacion
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1 })).UseAllAvailableWidth();

                // Encabezado arriba de las tres columnas
                Cell headerCell = new Cell(1, 3) // 1 fila y 3 columnas
                    .Add(new Paragraph("REGIÓN CENTRO"))
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetFontSize(10)
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER);
                table.AddHeaderCell(headerCell);

                // creacion del encabezado de cada columna, se le da formato al texto y a la tabla
                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph("VELOCIDAD PROMEDIO").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10)) // tamaño y texto centrado
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER)); // Sin borde para el encabezado

                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph("TRENES").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER));

                table.AddHeaderCell(new Cell()
                    .Add(new Paragraph("TRENES < PROM.").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER));

                // valores en la filas
                table.AddCell(new Cell().Add(new Paragraph("27")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table.AddCell(new Cell().Add(new Paragraph("22")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table.AddCell(new Cell().Add(new Paragraph("13")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes



                Table table2 = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1, 1 })).UseAllAvailableWidth();

                // creacion del encabezado de cada columna, se le da formato al texto y a la tabla
                table2.AddHeaderCell(new Cell()
                    .Add(new Paragraph("DISTRITO").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10)) // tamaño y texto centrado
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER)); // Sin borde para el encabezado

                table2.AddHeaderCell(new Cell()
                    .Add(new Paragraph("DIST. KM").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER));

                table2.AddHeaderCell(new Cell()
                    .Add(new Paragraph("VEL. NTE.").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER));

                table2.AddHeaderCell(new Cell()
                    .Add(new Paragraph("VEL. SUR").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER));

                // valores en la filas
                table2.AddCell(new Cell().Add(new Paragraph("VIBORILLAS ")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("328")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("25")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("29")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("lEON")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("251")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("11")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("14")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("CAMACHO-ZACATECAS")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("531")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("33")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                table2.AddCell(new Cell().Add(new Paragraph("26")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                    .SetBorder(Border.NO_BORDER)); // sin bordes

                Table table3 = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1, 1, 1 })).UseAllAvailableWidth();

                // creacion del encabezado de cada columna, se le da formato al texto y a la tabla
                table3.AddHeaderCell(new Cell()
                    .Add(new Paragraph("DISTRITO").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10)) // tamaño y texto centrado
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER)); // Sin borde para el encabezado

                table3.AddHeaderCell(new Cell()
                    .Add(new Paragraph("DIRECCION").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER));

                table3.AddHeaderCell(new Cell()
                    .Add(new Paragraph("TREN").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER));

                table3.AddHeaderCell(new Cell()
                    .Add(new Paragraph("VELOCIDAD MEDIA").SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                    .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                    .SetBold()
                    .SetBorder(Border.NO_BORDER));


                // Iteracion para imprimir los datos obtenidos de la tabla de la base de datos
                foreach (var x in datos)
                {
                    // filtrado de los datos, se le indica que no compare entre mayusculas y minusculas
                    if (x.Distrito.Equals("ZACATECAS", StringComparison.OrdinalIgnoreCase))
                    {
                        // Asignacion de las filas de acorde a su columna 
                        table3.AddCell(new Cell()
                        .Add(new Paragraph(x.Distrito)
                        .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))  // Alineacion del texto y tamaño de letra
                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE) // fondo blanco
                        .SetBorder(Border.NO_BORDER)); // sin bordes

                        table3.AddCell(new Cell()
                            .Add(new Paragraph(x.Pk)
                            .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                            .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE)
                            .SetBorder(Border.NO_BORDER));

                        table3.AddCell(new Cell()
                            .Add(new Paragraph(x.Tren)
                            .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                            .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE)
                            .SetBorder(Border.NO_BORDER));

                        table3.AddCell(new Cell()
                            .Add(new Paragraph(x.Tiempo_demora.ToString())
                            .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10))
                            .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE)
                            .SetBorder(Border.NO_BORDER));

                    }

                }

                // Salto de linea para pocisionar adecuadamente la tabla creada
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("\n"));
                // Agregar la tabla al documento
                document.Add(table);
                document.Add(new Paragraph("\n"));
                document.Add(table2);
                document.Add(new Paragraph("\n"));
                document.Add(table3);

                // Cerramos el archivo
                pdf.Close();
                // Regresamos el archivo Pdf con el nombre de Datos.pdf
                return File(stream.ToArray(), "application/pdf", "Datos.pdf");

            }

        }
    }
}
