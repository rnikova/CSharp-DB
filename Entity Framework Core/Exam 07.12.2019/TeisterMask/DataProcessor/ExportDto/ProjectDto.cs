using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ProjectDto
    {
        public string ProjectName { get; set; }

        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public TasksXmlDto[] Tasks { get; set; }
    }
}
