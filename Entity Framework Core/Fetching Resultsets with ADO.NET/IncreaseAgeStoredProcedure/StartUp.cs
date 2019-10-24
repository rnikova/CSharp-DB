namespace IncreaseAgeStoredProcedure
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class StartUp
    {
        private static string ConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(ConnectionString);

        public static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            connection.Open();

            using (connection)
            {
                string commandText = @"CREATE PROC usp_GetOlder @id INT 
                                        AS
                                        UPDATE Minions
                                        SET Age += 1
                                        WHERE Id = @id";

                SqlCommand command = new SqlCommand(commandText, connection);

                using (command)
                {
                    command.ExecuteNonQuery();
                }

                command = new SqlCommand("usp_GetOlder", connection);

                using (command)
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }

                commandText = "SELECT Name, Age FROM Minions WHERE Id = @Id";
                command = new SqlCommand(commandText, connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@Id", id);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string name = (string)reader[0];
                        int age = (int)reader[1];

                        Console.WriteLine($"{name} – {age} years old");
                    }
                }
            }
        }
    }
}
