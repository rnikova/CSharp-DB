﻿namespace CarDealer.Dtos.Import
{
    using System.Xml.Serialization;

    [XmlType("Sale")]
    public class SaleDto
    {
        [XmlElement("carId")]
        public int CarId { get; set; }

        [XmlElement("customerId")]
        public int CustomerId { get; set; }

        [XmlElement("discount")]
        public int Discount { get; set; }
    }
}
