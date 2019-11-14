namespace CarDealer.Dtos.Import
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "parts")]
    public class CarPartDto
    {
        [XmlElement(ElementName = "partId")]
        public PartsIdDto[] PartsId { get; set; }
    }
}
