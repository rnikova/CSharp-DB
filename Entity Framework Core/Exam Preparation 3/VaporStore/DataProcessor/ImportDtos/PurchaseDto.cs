using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ImportDtos
{
    [XmlType("Purchase")]
    public class PurchaseDto
    {
        [XmlAttribute("title")]
        public string Title { get; set; }

        public string Type { get; set; }

        [Required]
        [RegularExpression("^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$")]
        public string Key { get; set; }

        public string Card { get; set; }

        public string Date { get; set; }
    }
}
