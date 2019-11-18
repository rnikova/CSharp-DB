namespace VaporStore.DataProcessor.ExportDtos
{
    using System.Xml.Serialization;

    public class ExportPurchaseGameDto
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }
    }
}
