namespace ProductShop.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("SoldProduct")]
    public class ExportSoldProductDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }
    }
}
