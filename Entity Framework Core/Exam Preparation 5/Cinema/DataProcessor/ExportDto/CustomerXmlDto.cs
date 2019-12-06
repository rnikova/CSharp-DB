﻿using System.Xml.Serialization;

namespace Cinema.DataProcessor.ExportDto
{
    [XmlType("Customer")]
    public class CustomerXmlDto
    {
        [XmlAttribute("FirstName")]
        public string FirstName { get; set; }

        [XmlAttribute("LastName")]
        public string LastName { get; set; }

        public string SpentMoney { get; set; }

        public string SpentTime { get; set; }
    }
}
