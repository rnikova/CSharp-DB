namespace VillainNames
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        private static string ConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(ConnectionString);

        public static void Main(string[] args)
        {
            connection.Open();

            using (connection)
            {
                var queryText = @"  SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                FROM Villains AS v 
                                JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                GROUP BY v.Id, v.Name 
                                HAVING COUNT(mv.VillainId) > 3 
                                ORDER BY COUNT(mv.VillainId)";

                var query = new SqlCommand(queryText, connection);
                var reader = query.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        string name = (string)reader[0];
                        int count = (int)reader[1];

                        Console.Write($"{name} - {count}");
                    }
                }
            }
        }
    }
}
