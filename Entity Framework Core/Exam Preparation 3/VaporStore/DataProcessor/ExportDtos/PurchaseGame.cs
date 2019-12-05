using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDtos
{
    public class PurchaseGameDto
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }
    }
}
