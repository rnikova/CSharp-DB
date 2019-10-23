namespace MinionNames
{
    using System;
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
                var villainName = @"SELECT Name FROM Villains WHERE Id = @Id";

                var command = new SqlCommand(villainName, connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@Id", id);

                    string villain = (string)command.ExecuteScalar();

                    if (villainName == null)
                    {
                        Console.WriteLine($"No villain with ID {id} exists in the database.");
                        return;
                    }

                    Console.WriteLine($"Villain: {villain}");
                }

                var minionNames = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum, m.Name, m.Age 
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                    WHERE mv.VillainId = @Id
                                    ORDER BY m.Name";

                command = new SqlCommand(minionNames, connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@Id", id);

                    var reader = command.ExecuteReader();

                    using (reader)
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("(no minions)");
                            return;
                        }

                        while (reader.Read())
                        {
                            long row = (long)reader[0];
                            string name = (string)reader[1];
                            int age = (int)reader[2];

                            Console.WriteLine($"{row}. {name} {age}");
                        }
                    }
                }
            }
        }
    }
}
