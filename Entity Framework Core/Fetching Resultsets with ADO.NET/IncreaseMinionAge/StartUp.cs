namespace IncreaseMinionAge
{
    using System;
    using System.Linq;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    public class StartUp
    {
        private static string ConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(ConnectionString);

        public static void Main(string[] args)
        {
            List<int> ids = Console.ReadLine()
                .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();

            connection.Open();

            using (connection)
            {
                string update = "UPDATE Minions SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1 WHERE Id = @Id";
                SqlCommand command = new SqlCommand(update, connection);

                using (command)
                {
                    foreach (int id in ids)
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                }

                string select = "SELECT Name, Age FROM Minions";
                command = new SqlCommand(select, connection);

                using (command)
                {
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string name = (string)reader[0];
                        int age = (int)reader[1];

                        Console.WriteLine($"{name} {age}");
                    }
                }
            }
        }
    }
}
