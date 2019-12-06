using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Message")]
    public class EncryptedMessageDto
    {
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
    }
}
