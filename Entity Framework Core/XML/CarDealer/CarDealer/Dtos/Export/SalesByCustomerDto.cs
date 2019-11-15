﻿namespace CarDealer.Dtos.Export
{
    using System.Xml.Serialization;

    [XmlType("customers")]
    public class SalesByCustomerDto
    {
        [XmlAttribute("full-name")]
        public string FullName { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }
    }
}
