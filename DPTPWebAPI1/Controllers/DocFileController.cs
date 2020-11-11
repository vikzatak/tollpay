using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;


namespace DPTPWebAPI.Controllers
{
    public class eBookResult : IHttpActionResult
    {
        MemoryStream bookStuff;
        string PdfFileName;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;
        public eBookResult(MemoryStream data, HttpRequestMessage request, string filename)
        {
            bookStuff = data;
            httpRequestMessage = request;
            PdfFileName = filename;
        }
        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(bookStuff);
            //httpResponseMessage.Content = new ByteArrayContent(bookStuff.ToArray());  
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = PdfFileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
        }
    }
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DocFileController : ApiController
    {
        //string fullPath = @"~/downloadfile/";
        string bookPath_Pdf = @"E:\backup\tp\TollPayAdmin\DPTPWebAPI\downloadfile\a.pdf";
        string bookPath_xls = @"E:\backup\tp\TollPayAdmin\DPTPWebAPI\downloadfile\a.xls";
        string bookPath_doc = @"E:\backup\tp\TollPayAdmin\DPTPWebAPI\downloadfile\a.rtf";
        string bookPath_zip = @"E:\backup\tp\TollPayAdmin\DPTPWebAPI\downloadfile\a.zip";
        public HttpResponseMessage Post()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/docfile/" + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("api/statement/{filename}")]
        public HttpResponseMessage GetstatementFor(string filename)
        {
            Paragraph paragraph = new Paragraph("Digipment Digital Solutions Pvt Ltd, Pune 411033");
            var dateString = DateTime.Now.ToString("yyyyMMdd");
            string bookName = filename + dateString +".pdf";
            string filepath = "~/downloadfile/";
            string imageURL = System.Web.Hosting.HostingEnvironment.MapPath(filepath  + "/tollpayoldlogo.jpg");

            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);

            //Resize image depend upon your need

            jpg.ScaleToFit(140f, 120f);

            //Give space before image

            jpg.SpacingBefore = 10f;

            //Give some space after the image

            jpg.SpacingAfter = 1f;

            jpg.Alignment = Element.ALIGN_CENTER;



            //Create document  
            Document doc = new Document(PageSize.A4);

          
            //Create PDF Table  
            PdfPTable tableLayout = new PdfPTable(4);
            //Create a PDF file in specific path  
            PdfWriter.GetInstance(doc, new FileStream(System.Web.Hosting.HostingEnvironment.MapPath(filepath+bookName), FileMode.Create));
            //Open the PDF document  
            doc.Open();
            doc.AddHeader("H1", "Digipment Digital Solutions Pvt Ltd, Pune 411033");
            doc.AddCreator("DPTP");
            doc.Add(paragraph);
              doc.Add(jpg);
            //Add Content to PDF  
            doc.Add(Add_Content_To_PDF(tableLayout));
            // Closing the document  
            doc.Close();
            using (Stream input = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath(filepath + bookName), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (Stream output = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath(filepath +"Encry_" +bookName), FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfReader reader = new PdfReader(input);
                    PdfEncryptor.Encrypt(reader, output, true, "secret", "secret", PdfWriter.ALLOW_SCREENREADERS);
                }
            }
            var dataBytes = File.ReadAllBytes(System.Web.Hosting.HostingEnvironment.MapPath(filepath + "Encry_" + bookName));
            //adding bytes to memory stream   
            var dataStream = new MemoryStream(dataBytes);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = "Encry_" + bookName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;

        }
        private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {
            float[] headers = {     20,      20,      30,      30    }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
                                              //Add Title to the PDF file at the top  
            tableLayout.AddCell(new PdfPCell(new Phrase("Creating PDF file using iTextsharp", new Font(Font.FontFamily.COURIER, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0))))
            {
                Colspan = 4,
                Border = 0,
                PaddingBottom = 20,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //Add header  
            AddCellToHeader(tableLayout, "Cricketer Name");
            AddCellToHeader(tableLayout, "Height");
            AddCellToHeader(tableLayout, "Born On");
            AddCellToHeader(tableLayout, "Parents");
            //Add body  
            AddCellToBody(tableLayout, "Sachin Tendulkar");
            AddCellToBody(tableLayout, "1.65 m");
            AddCellToBody(tableLayout, "April 24, 1973");
            AddCellToBody(tableLayout, "Ramesh Tendulkar, Rajni Tendulkar");
            AddCellToBody(tableLayout, "Mahendra Singh Dhoni");
            AddCellToBody(tableLayout, "1.75 m");
            AddCellToBody(tableLayout, "July 7, 1981");
            AddCellToBody(tableLayout, "Devki Devi, Pan Singh");
            AddCellToBody(tableLayout, "Virender Sehwag");
            AddCellToBody(tableLayout, "1.70 m");
            AddCellToBody(tableLayout, "October 20, 1978");
            AddCellToBody(tableLayout, "Aryavir Sehwag, Vedant Sehwag");
            AddCellToBody(tableLayout, "Virat Kohli");
            AddCellToBody(tableLayout, "1.75 m");
            AddCellToBody(tableLayout, "November 5, 1988");
            AddCellToBody(tableLayout, "Saroj Kohli, Prem Kohli");
         
            return tableLayout;
        }
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.COURIER, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(0, 51, 102)
            });
        }
        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.COURIER, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = iTextSharp.text.BaseColor.WHITE
            });
        }
            [AllowAnonymous]
        [HttpGet]
        [Route("api/statementforhrm/{format}")]
        public HttpResponseMessage GetstatementForHRM(string format)
        {


            string reqBook = format.ToLower() == "pdf" ? bookPath_Pdf : (format.ToLower() == "xls" ? bookPath_xls : (format.ToLower() == "doc" ? bookPath_doc : bookPath_zip));
            string bookName = "a." + format.ToLower();
            //converting Pdf file into bytes array  
            var dataBytes = File.ReadAllBytes(reqBook);
            //adding bytes to memory stream   
            var dataStream = new MemoryStream(dataBytes);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = bookName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }
    }
}
