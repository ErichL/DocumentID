using System.Collections.Generic;

namespace DocumentID.Models
{
    public class Document
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<uint> MinHashes { get; set; }
    }
}