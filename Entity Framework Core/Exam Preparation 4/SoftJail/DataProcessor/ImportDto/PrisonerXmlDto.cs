using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class PrisonerXmlDto
    {
        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }
    }
}
