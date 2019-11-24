namespace Cinema.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Projection")]
    public class ProjectionDto
    {
        public int MovieId { get; set; }

        public int HallId { get; set; }

        public string DateTime { get; set; }
    }
}
