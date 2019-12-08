namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using System.Xml.Serialization;
    using TeisterMask.DataProcessor.ImportDto;
    using System.IO;
    using System.Text;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using Newtonsoft.Json;
    using System.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProjectDto[]), new XmlRootAttribute("Projects"));
            var projectsDto = (ProjectDto[])serializer.Deserialize(new StringReader(xmlString));

            var sb = new StringBuilder();
            var projects = new List<Project>();

            foreach (var p in projectsDto)
            {
                if (!IsValid(p))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var projDueDate = DateTime.TryParseExact(p.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime prDueDate);

                var project = new Project
                {
                    Name = p.Name,
                    OpenDate = DateTime.ParseExact(p.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    DueDate = projDueDate == false ? null : (DateTime?)prDueDate
                };

                foreach (var t in p.Tasks)
                {
                    if (!IsValid(t))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var taskOpenDate = DateTime.ParseExact(t.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var taskDueDate = DateTime.ParseExact(t.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (taskOpenDate < project.OpenDate || taskDueDate > project.DueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var task = new Task
                    {
                        Name = t.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = Enum.Parse<ExecutionType>(t.ExecutionType),
                        LabelType = Enum.Parse<LabelType>(t.LabelType)
                    };

                    project.Tasks.Add(task);
                }

                projects.Add(project);
                sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var employeesDto = JsonConvert.DeserializeObject<EmployeeDto[]>(jsonString);

            var sb = new StringBuilder();
            var employees = new List<Employee>();

            // new solution
            foreach (var e in employeesDto)
            {
                if (!IsValid(e))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var uniqueTasks = e.Tasks.Distinct();

                var employee = new Employee
                {
                    Username = e.Username,
                    Email = e.Email,
                    Phone = e.Phone,
                    EmployeesTasks = new List<EmployeeTask>()
                };

                foreach (var t in uniqueTasks)
                {
                    var isExist = context.Tasks.FirstOrDefault(x => x.Id == t);

                    if (isExist == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var taks = new EmployeeTask
                    {
                        TaskId = t,
                        Employee = employee
                    };

                    employee.EmployeesTasks.Add(taks);
                }

                employees.Add(employee);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            }

            // first solution
            //foreach (var emp in employeesDto)
            //{
            //    if (!IsValid(emp))
            //    {
            //        sb.AppendLine(ErrorMessage);
            //        continue;
            //    }

            //    var employee = new Employee
            //    {
            //        Username = emp.Username,
            //        Email = emp.Email,
            //        Phone = emp.Phone,
            //        EmployeesTasks = new List<EmployeeTask>()
            //    };

            //    var uniqueTasks = new HashSet<int>(emp.Tasks);

            //    for (int i = 0; i < uniqueTasks.Count; i++)
            //    {
            //        var isExist = context.EmployeesTasks.FirstOrDefault(x => x.TaskId == i);

            //        if (isExist == null)
            //        {
            //            sb.AppendLine(ErrorMessage);
            //            continue;
            //        }

            //        employee.EmployeesTasks.Add(isExist);
            //    }

            //    employees.Add(employee);
            //    sb.AppendLine(string.Format(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count));
            //}

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        //ImportProjects


        //foreach (var pr in projectsDto)
        //{
        //    if (!IsValid(pr))
        //    {
        //        sb.AppendLine(ErrorMessage);
        //        continue;
        //    }

        //    var isValidDueDate = DateTime.TryParse(pr.DueDate, out DateTime dueDate);

        //    var project = new Project
        //    {
        //        Name = pr.Name,
        //        OpenDate = DateTime.ParseExact(pr.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        //    };

        //    if (isValidDueDate)
        //    {
        //        project.DueDate = DateTime.ParseExact(pr.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    }
        //    //if (pr.DueDate == null)
        //    //{
        //    //    sb.AppendLine(ErrorMessage);
        //    //    continue;
        //    //}

        //    foreach (var taskDto in pr.Tasks)
        //    {
        //        if (!IsValid(taskDto))
        //        {
        //            sb.AppendLine(ErrorMessage);
        //            continue;
        //        }

        //        var task = new Task
        //        {
        //            Name = taskDto.Name,
        //            OpenDate = DateTime.ParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
        //            DueDate = DateTime.ParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
        //            ExecutionType = Enum.Parse<ExecutionType>(taskDto.ExecutionType),
        //            LabelType = Enum.Parse<LabelType>(taskDto.LabelType)
        //        };


        //        //var isValidOpenDate = DateTime.Compare(task.OpenDate, project.OpenDate);
        //        //var isValidTaskDueDate = DateTime.Compare(task.DueDate, (DateTime)project.DueDate);

        //        if ((task.OpenDate < project.OpenDate 
        //            || (DateTime?)task.DueDate > project.DueDate))
        //        {
        //            sb.AppendLine(ErrorMessage);
        //            continue;
        //        }

        //        project.Tasks.Add(task);
        //    }

        //    projects.Add(project);
        //    sb.AppendLine(string.Format(SuccessfullyImportedProject, project.Name, project.Tasks.Count));
        //}       
    }
}