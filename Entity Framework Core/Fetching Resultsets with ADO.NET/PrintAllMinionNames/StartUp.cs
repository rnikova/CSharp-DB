namespace PrintAllMinionNames
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class StartUp
    {
        private static string ConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(ConnectionString);

        public static void Main(string[] args)
        {
            List<string> minionsNames = new List<string>();

            connection.Open();

            using (connection)
            {
                string commandText = "SELECT Name FROM Minions";

                using (connection)
                {
                    SqlCommand command = new SqlCommand(commandText, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        minionsNames.Add((string)reader[0]);
                    }
                }
            }

            while (minionsNames.Count != 0)
            {
                Console.WriteLine(minionsNames[0]);
                minionsNames.RemoveAt(0);

                if (minionsNames.Count == 0)
                {
                    break;
                }

                Console.WriteLine(minionsNames.Last());
                minionsNames.RemoveAt(minionsNames.Count - 1);
            }
        }
    }
}
