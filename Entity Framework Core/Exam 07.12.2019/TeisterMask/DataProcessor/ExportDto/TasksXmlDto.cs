using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Task")]
    public class TasksXmlDto
    {
        public string Name { get; set; }

        public string Label { get; set; }
    }
}
