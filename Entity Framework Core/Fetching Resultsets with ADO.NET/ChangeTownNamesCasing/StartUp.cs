namespace ChangeTownNamesCasing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class StartUp
    {
        private static string ConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(ConnectionString);

        public static void Main(string[] args)
        {
            string country = Console.ReadLine();

            connection.Open();

            using (connection)
            {
                string editTowns = @"UPDATE Towns
                                         SET Name = UPPER(Name)
                                        WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = @countryName)";

                SqlCommand command = new SqlCommand(editTowns, connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@countryName", country);
                    int count = command.ExecuteNonQuery();

                    Console.WriteLine($"{count} town names were affected. ");
                }

                string findTowns = @" SELECT t.Name
                                      FROM Towns as t
                                      JOIN Countries AS c ON c.Id = t.CountryCode
                                      WHERE c.Name = @countryName";

                command = new SqlCommand(findTowns, connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@countryName", country);

                    List<string> cities = new List<string>();

                    SqlDataReader reader = command.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            cities.Add((string)reader[0]);
                        }
                    }

                    if (cities.Count == 0)
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                    else
                    {
                        Console.WriteLine($"[{string.Join(", ", cities)}]");
                    }
                }
            }
        }
    }
}
