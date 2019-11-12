namespace ProductShop.Dtos.Import
{
    using System.Xml.Serialization;

    [XmlType("Product")]
    public class ImportProductDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }

        [XmlAttribute("sellerId")]
        public int SellerId { get; set; }

        [XmlAttribute("buyerId")]
        public int BuyerId { get; set; }
    }
}
