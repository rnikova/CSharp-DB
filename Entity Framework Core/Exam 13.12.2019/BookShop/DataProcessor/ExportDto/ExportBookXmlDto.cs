using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class ExportBookXmlDto
    {
        [XmlAttribute("Pages")]
        public int Pages { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }
    }
}
