namespace CarDealer.Dtos.Import
{
    using System.Xml.Serialization;

    [XmlType("Supplier")]
    public class SupplierDto
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "isImporter")]
        public bool IsImporter { get; set; }
    }
}
