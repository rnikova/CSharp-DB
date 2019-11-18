namespace VaporStore.DataProcessor.ExportDtos
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("User")]
    public class ExportUserDto
    {
        //username, purchases for that store type and total money spent for that store type
        [XmlAttribute("username")]
        public string Username { get; set; }

        public ExportPurchaseDto[] Purchases { get; set; }

        public decimal TotalSpent { get; set; }
    }
}
