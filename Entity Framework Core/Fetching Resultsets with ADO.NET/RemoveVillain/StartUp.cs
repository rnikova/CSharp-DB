namespace RemoveVillain
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        private static string ConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(ConnectionString);

        private static SqlTransaction transaction;

        public static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            connection.Open();

            using (connection)
            {
                transaction = connection.BeginTransaction();

                try
                {
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;
                    command.CommandText = "SELECT Name FROM Villains WHERE Id = @villainId";

                    command.Parameters.AddWithValue("@villainId", id);

                    object value = command.ExecuteScalar();

                    if (value == null)
                    {
                        throw new ArgumentNullException(nameof(id), "No such villain was found.");
                    }

                    string vilianName = (string)value;

                    command.CommandText = "DELETE FROM MinionsVillains WHERE VillainId = @villainId";
                    int deletedMinions = command.ExecuteNonQuery();

                    command.CommandText = "DELETE FROM Villains WHERE Id = @villainId";
                    command.ExecuteNonQuery();

                    transaction.Commit();

                    Console.WriteLine($"{vilianName} was deleted.");
                    Console.WriteLine($"{deletedMinions} minions were released.");

                }
                catch (ArgumentNullException ae)
                {

                    Console.WriteLine(ae.Message);
                    transaction.Rollback();
                }
            }
        }
    }
}
