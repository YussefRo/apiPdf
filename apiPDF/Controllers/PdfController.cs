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
                iText.Kernel.Pdf.PdfReader pdfReader = new iText.Kernel.Pdf.PdfReader("C:/Users/yrodriguezc/Desktop/resumen_demora1.pdf");
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

    }
}
