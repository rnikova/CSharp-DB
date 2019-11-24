using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Cinema.DataProcessor.ExportDto
{
    [XmlType("Customer")]
    public class CustomerXmlDto
    {
        [XmlAttribute("FirstName")]
        [StringLength(20, MinimumLength = 3)]
        public string FirstName { get; set; }

        [XmlAttribute("LastName")]
        [StringLength(20, MinimumLength = 3)]
        public string LastName { get; set; }

        [XmlElement("SpentMoney")]
        public string SpentMoney { get; set; }

        [XmlElement("SpentTime")]
        public string SpentTime { get; set; }
    }
}
