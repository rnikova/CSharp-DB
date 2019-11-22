namespace SoftJail.DataProcessor
{

    using Data;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.Text;
    using System.Xml;
    using System.IO;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var officersDto = context.OfficerPrisoners
                .Where(x => ids.Contains(x.Prisoner.Id))
                .Select(x => new ExportPrisonerDto
                {
                    Id = x.Prisoner.Id,
                    Name = x.Prisoner.FullName,
                    CellNumber = x.Prisoner.Cell.CellNumber,
                    Officers = x.Officer.PrisonerOfficers
                        .Select(y => new ExportOfficerDto
                        {
                            OfficerName = y.Officer.FullName,
                            Department = y.Officer.Department.Name
                        })
                        .OrderBy(o => o.OfficerName)
                        .ToArray(),
                    TotalOfficerSalary = x.Officer.Salary
                })
                .OrderBy(p => p.Name)
                .ThenBy(i => i.Id)
                .ToArray();

            var json = JsonConvert.SerializeObject(officersDto, Newtonsoft.Json.Formatting.Indented);

            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var names = prisonersNames.Split(",").ToArray();

            var prisonersDto = context.Prisoners
                .Where(x => names.Contains(x.FullName))
                .Select(x => new ExportPrisonerXmlDto
                {
                    Id = x.Id,
                    Name = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = x.Mails.Select(y => new ExportMailDto
                    {
                        Description = Reverse(y.Description)
                    })
                    .ToArray()
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();

            var serializer = new XmlSerializer(typeof(ExportPrisonerXmlDto[]), new XmlRootAttribute("Prisoners"));

            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), prisonersDto, namespaces);

            return sb.ToString().TrimEnd();

        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);

            return new string(charArray);
        }
    }
}