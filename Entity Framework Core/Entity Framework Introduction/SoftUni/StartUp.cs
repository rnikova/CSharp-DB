using System;
using System.Text;
using System.Linq;
using SoftUni.Data;
using SoftUni.Models;
using System.Globalization;

namespace SoftUni
{
	public class StartUp
	{
		private static void Main(string[] args)
		{
			using (SoftUniContext context = new SoftUniContext())
			{
				Console.WriteLine(DeleteProjectById(context));
			}
		}

        public static string RemoveTown(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Address.Town.Name == "Seattle")
                .ToList();

            foreach (var employee in employees)
            {
                employee.AddressId = null;
                context.SaveChanges();
            }

            var towns = context.Towns
                .Where(t => t.Name == "Seattle")
                .ToList();

            var addresses = context.Addresses
                .Where(a => a.Town.Name == "Seattle")
                .ToList();

            int count = addresses.Count();

            foreach (var address in addresses)
            {
                context.Addresses.Remove(address);
                context.SaveChanges();
            }

            foreach (var town in towns)
            {
                context.Towns.Remove(town);
                context.SaveChanges();
            }

            return $"{count} addresses in Seattle were deleted";
        }


        public static string DeleteProjectById(SoftUniContext context)
		{
			var sb = new StringBuilder();

            var project = context.Projects
                .First(p => p.ProjectId == 2);

            context.EmployeesProjects
                .ToList()
                .ForEach(ep => context.EmployeesProjects.Remove(ep));

            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects
                .Take(10)
                .Select(p => p.Name)
                .ToList();

            foreach (var p in projects)
            {
                sb.AppendLine($"{p}");
            }


			return sb.ToString().TrimEnd();
		}


		public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
		{
			var sb = new StringBuilder();

			var employees = context.Employees
				.Where(e => e.FirstName.StartsWith("Sa"))
				.Select(e => new
				{
					FirstName = e.FirstName,
					LastName = e.LastName,
					JobTitle = e.JobTitle,
					Salary = e.Salary                    
				})
				.OrderBy(e => e.FirstName)
				.ThenBy(e => e.LastName)
				.ToList();

			foreach (var employee in employees)
			{
				sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
			}

			return sb.ToString().TrimEnd();
		}

		public static string IncreaseSalaries(SoftUniContext context)
		{
			var sb = new StringBuilder();

			var employees = context.Employees
				.Where(e => e.Department.Name == "Engineering" ||
						e.Department.Name == "Tool Design" ||
						e.Department.Name == "Marketing" ||
						e.Department.Name == "Information Services")
				.ToList();

			foreach (var employee in employees)
			{
				employee.Salary *= 1.12m;
				context.SaveChanges();
			}

			foreach (var employee in employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
			{
				sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
			}

			return sb.ToString().TrimEnd();
		}

		public static string GetLatestProjects(SoftUniContext context)
		{
			var sb = new StringBuilder();

			var projects = context.Projects
				.Select( p => new 
				{
					ProjectName = p.Name,
					Description = p.Description,
					StartDate = p.StartDate
				})
				.OrderByDescending(p => p.StartDate)
				.Take(10)
				.OrderBy(p => p.ProjectName)
				.ToList();

			foreach (var project in projects)
			{
				sb.AppendLine($"{project.ProjectName}");
				sb.AppendLine($"{project.Description}");
				sb.AppendLine($"{project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}");
			}

			return sb.ToString().TrimEnd();
		}

		public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
		{
			var sb = new StringBuilder();

			var departments = context.Departments
				.Where(d => d.Employees.Count() > 5)
				.OrderBy(d => d.Employees.Count())
				.ThenBy(d => d.Name)
				.Select(d => new
				{
					DepartmentName = d.Name,
					ManagerFirstName = d.Manager.FirstName,
					ManagerLastName = d.Manager.LastName,
					Employees = d.Employees
								.Select(e => new
								{
									EmployeeFirstName = e.FirstName,
									EmployeeLastName = e.LastName,
									JobTitile = e.JobTitle
								})
					.OrderBy(e => e.EmployeeFirstName)
					.ThenBy(e => e.EmployeeLastName)
					.ToList()
				})
				.ToList();

			foreach (var department in departments)
			{
				sb.AppendLine($"{department.DepartmentName} - {department.ManagerFirstName} {department.ManagerLastName}");

				foreach (var employee in department.Employees)
				{
					sb.AppendLine($"{employee.EmployeeFirstName} {employee.EmployeeLastName} - {employee.JobTitile}");
				}
			}

			return sb.ToString().TrimEnd();
		}

		public static string GetEmployee147(SoftUniContext context)
		{
			var sb = new StringBuilder();

			var employees = context.Employees
				.Where(e => e.EmployeeId == 147)
				.Select(e => new
				{
					FirstName = e.FirstName,
					LastName = e.LastName,
					JobTitle = e.JobTitle,
					Projects = e.EmployeesProjects.Select(p => new
					{
						ProjectName = p.Project.Name
					})
					.OrderBy(p => p.ProjectName)
					.ToList()
				})
				.ToList();

			foreach (var employee in employees)
			{
				sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

				foreach (var project in employee.Projects)
				{
					sb.AppendLine($"{project.ProjectName}");
				}
			}

			return sb.ToString().TrimEnd();
		}

		public static string GetAddressesByTown(SoftUniContext context)
		{
			var sb = new StringBuilder();

			var addresses = context.Addresses
				.Select(a => new
				{
					TownName = a.Town.Name,
					Address = a.AddressText,
					Employee = a.Employees.Count,
				})
				.OrderByDescending(a => a.Employee)
				.ThenBy(a => a.TownName)
				.ThenBy(a => a.Address)
				.ToList();

			foreach (var address in addresses)
			{
				sb.AppendLine($"{address.Address}, {address.TownName} - {address.Employee} employees");
			}

			return sb.ToString().TrimEnd();
		}

		public static string GetEmployeesInPeriod(SoftUniContext context)
		{
			var sb = new StringBuilder();

			var employees = context.Employees
				.Where(p => p.EmployeesProjects.Any(s => s.Project.StartDate.Year >= 2001 &&
													s.Project.StartDate.Year <= 2003))
				.Select(e => new
				{
					FirstName = e.FirstName,
					LastName = e.LastName,
					ManagerFirstName = e.Manager.FirstName,
					ManagerLastName = e.Manager.LastName,
					Projects = e.EmployeesProjects
										.Select(p => new
										{
											ProjectName = p.Project.Name,
											StartDate = p.Project.StartDate,
											EndDate = p.Project.EndDate
										})
				})
				.Take(10)
				.ToList();

			foreach (var employee in employees)
			{
				sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

				foreach (var project in employee.Projects)
				{
					sb.AppendLine($"--{project.ProjectName} - {project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {(project.EndDate == null ? "not finished" : project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture))}");
				}
			}

			return sb.ToString().TrimEnd();
		}

		public static string AddNewAddressToEmployee(SoftUniContext context)
		{
			var sb = new StringBuilder();

			Address address = new Address
			{
				AddressText = "Vitoshka 15",
				TownId = 4
			};

			var nakov = context.Employees
				.FirstOrDefault(e => e.LastName == "Nakov");

			nakov.Address = address;

			context.SaveChanges();

			var employees = context.Employees
				.OrderByDescending(e => e.AddressId)
				.Select(a => a.Address.AddressText)
				.Take(10)
				.ToList();

			foreach (var employee in employees)
			{
				sb.AppendLine(employee);
			}

			return sb.ToString().TrimEnd();
		}

		public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
		{
			var sb = new StringBuilder();

			var employees = context.Employees
				.Select(e => new
				{
					FirstName = e.FirstName,
					LastName = e.LastName,
					DepartmentName = e.Department.Name,
					Salary = e.Salary
				})
				.Where(e => e.DepartmentName == "Research and Development")
				.OrderBy(e => e.Salary)
				.ThenByDescending(e => e.FirstName)
				.ToList();

			foreach (var e in employees)
			{
				sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:F2}");
			}

			return sb.ToString().TrimEnd();
		}

		public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
		{
			var sb = new StringBuilder();

			var employees = context.Employees
				.Select(e => new
				{
					FirstName = e.FirstName,
					Salary = e.Salary
				})
				.Where(e => e.Salary > 50000)
				.OrderBy(e => e.FirstName)
				.ToList();

			foreach (var employee in employees)
			{
				sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
			}

			return sb.ToString().TrimEnd();
		}

		public static string GetEmployeesFullInformation(SoftUniContext context)
		{
			var sb = new StringBuilder();

			var employees = context.Employees
				.Select(e => new
				{
					EmployeeId = e.EmployeeId,
					FirstName = e.FirstName,
					MiddleName = e.MiddleName,
					LastName = e.LastName,
					JobTitle = e.JobTitle,
					Salary = e.Salary
				})
				.OrderBy(e => e.EmployeeId)
				.ToList();

			foreach (var e in employees)
			{
				sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
			}

			return sb.ToString().TrimEnd();
		}
	}
}
