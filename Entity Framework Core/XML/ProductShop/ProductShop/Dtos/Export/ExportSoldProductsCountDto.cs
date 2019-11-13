namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("SoldProducts")]
    public class ExportSoldProductsCountDto
    {
        [XmlElement(ElementName = "count")]
        public int Count { get; set; }

        [XmlArray(ElementName = "products")]
        public ExportSoldProductDto[] Products { get; set; }
    }
}
