using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DocumentID.Controllers;
using DocumentID.Models;
using System.Net.Http;

namespace DocumentID.Tests
{
    [TestClass]
    public class TestDocumenttController
    {
        [TestMethod]
        public void GetAllDocuments_ShouldReturnAllDocuments()
        {
            var testDocuments = GetTestDocuments();
            var controller = new DocumentController(testDocuments);

            var result = controller.GetAllDocuments() as List<Document>;
            Assert.AreEqual(testDocuments.Count, result.Count);
        }

        [TestMethod]
        public void GetDocument_ShouldReturnCorrectDocument()
        {
            var testDocuments = GetTestDocuments();
            var controller = new DocumentController(testDocuments);

            var result = controller.GetDocument(4) as OkNegotiatedContentResult<Document>;
            Assert.IsNotNull(result);
            Assert.AreEqual(testDocuments[3].Name, result.Content.Name);
        }

        [TestMethod]
        public void GetDocument_ShouldNotFindDocument()
        {
            var testDocuments = GetTestDocuments();
            var controller = new DocumentController(testDocuments);

            var result = controller.GetDocument(999);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetDocument_ShouldCompareDocuments()
        {
            var testDocuments = GetTestDocuments();
            var controller = new DocumentController(testDocuments);

            var result = controller.GetDocument(1, 2) as OkNegotiatedContentResult<double>;
            Assert.AreEqual(1.0, result.Content);
        }

        private List<Document> GetTestDocuments()
        {
            var testDocuments = new List<Document>
            {
                new Document { ID = 1, Name = "Demo1", MinHashes = new List<uint>() { 1, 2, 3 } },
                new Document { ID = 2, Name = "Demo2", MinHashes = new List<uint>() { 1, 2, 3 } },
                new Document { ID = 3, Name = "Demo3", MinHashes = new List<uint>() { 1, 2, 3 } },
                new Document { ID = 4, Name = "Demo4", MinHashes = new List<uint>() { 1, 2, 3 } }
            };

            return testDocuments;
        }
    }
}