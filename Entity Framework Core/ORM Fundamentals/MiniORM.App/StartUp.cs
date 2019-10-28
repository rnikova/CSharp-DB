namespace MiniORM.App
{
    using MiniORM.App.Data;
    using MiniORM.App.Data.Entities;
    using System;
    using System.Linq;

    public class StartUp
    {
        static void Main(string[] args)
        {
            var connectionString = @"Server =.\SQLEXPRESS; Database=MiniORM; Integrated Security=true; ";

            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "Gosho",
                LastName = "Goshov",
                DepartmentId = context.Departments.First().Id,
                IsEmployeed = true
            });

            var employee = context.Employees.Last();

            employee.FirstName = "Pesho";

            context.SaveChanges();
        }
    }
}
