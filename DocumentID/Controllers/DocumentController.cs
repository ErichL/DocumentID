using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TikaOnDotNet.TextExtraction;
using DocumentID.Lib;
using DocumentID.Models;

namespace DocumentID.Controllers
{
    public class DocumentController : ApiController
    {
        private static readonly MinHash _minHashes = new MinHash(20);
        private static List<Document> documents = new List<Document>();

        public DocumentController() { }

        public DocumentController(List<Document> documents)
        {
            DocumentController.documents = documents;
        }

        // GET: api/Document
        public IEnumerable<Document> GetAllDocuments()
        {
            return documents;
        }

        // GET: api/Document/id
        public IHttpActionResult GetDocument(int id)
        {
            var doc = documents.Find((d) => d.ID == id);
            if (doc == null)
            {
                return NotFound();
            }
            return Ok(doc);
        }

        // GET: api/Document/id/idTwo
        public IHttpActionResult GetDocument(int id, int idTwo)
        {
            var docOne = documents.Find((d) => d.ID == id);
            var docTwo = documents.Find((d) => d.ID == idTwo);
            if (docOne == null || docTwo == null)
            {
                return NotFound();
            }
            return Ok(_minHashes.Similarity(docOne.MinHashes, docTwo.MinHashes));
        }

        // POST: api/Document
        public async Task<HttpResponseMessage> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Document doc = new Document();
                    string textExtractionResult = new TextExtractor().Extract(file.LocalFileName).Text;
                    List<string> shingles = _minHashes.GetShingles(textExtractionResult);
                    doc.ID = documents.Count + 1;
                    doc.Name = file.Headers.ContentDisposition.FileName.Replace("\"", "");
                    doc.MinHashes = _minHashes.GetMinHash(shingles);
                    documents.Add(doc);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

    }
}
