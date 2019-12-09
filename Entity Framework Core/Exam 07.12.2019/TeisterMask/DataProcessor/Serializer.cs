namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projects = context.Projects
                .Where(x => x.Tasks.Count > 0)
                 .OrderByDescending(x => x.Tasks.Count)
                 .ThenBy(x => x.Name)
                 .Select(p => new ProjectDto
                 {
                     ProjectName = p.Name,
                     TasksCount = p.Tasks.Count,
                     HasEndDate = p.DueDate == null ? "No" : "Yes",
                     Tasks = p.Tasks.Select(t => new TasksXmlDto
                     {
                         Name = t.Name,
                         Label = t.LabelType.ToString()
                     })
                     .OrderBy(x => x.Name)
                     .ToArray()
                 })
                 .ToArray();

            var serializer = new XmlSerializer(typeof(ProjectDto[]), new XmlRootAttribute("Projects"));
            var sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(new StringWriter(sb), projects, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employees = context.Employees
                .Where(x => x.EmployeesTasks.Any(d => d.Task.OpenDate >= date))
                .Select(e => new EmployeeDto
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                        .Where(x => x.Task.OpenDate >= date)
                        .OrderByDescending(x => x.Task.DueDate)
                        .ThenBy(x => x.Task.Name)
                        .Select(t => new TasksDto
                        {
                            TaskName = t.Task.Name,
                            OpenDate = t.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                            DueDate = t.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            LabelType = t.Task.LabelType.ToString(),
                            ExecutionType = t.Task.ExecutionType.ToString()
                        })
                        .ToList()
                })
                .ToList()
                .OrderByDescending(x => x.Tasks.Count)
                .ThenBy(x => x.Username)
                .Take(10)
                .ToList();

            var json = JsonConvert.SerializeObject(employees, Formatting.Indented);

            return json;
        }
    }
}