namespace AddMinion
{
    using System;
    using System.Linq;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    public class StartUp
    {
        private static string ConnectionString = @"Server=.\SQLEXPRESS;Database=MinionsDB;Integrated Security=true;";

        private static SqlConnection connection = new SqlConnection(ConnectionString);

        public static int townId;
        public static int minionId;
        public static int villainId;

        public static void Main(string[] args)
        {
            List<string> minions = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .ToList();

            string minionName = minions[0];
            int minionAge = int.Parse(minions[1]);
            string minionTown = minions[2];

            List<string> villains = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .ToList();

            string villainName = villains[0];

            SqlConnection connection = new SqlConnection(ConnectionString);

            connection.Open();

            using (connection)
            {
                ExecuteTown(minionTown, connection);
                ExecuteMinion(minionName, minionAge, connection);
                ExecuteVilain(villainName, connection);
                ExecuteAddMinionToVillain(minionName, villainName, connection);
            }
        }

        private static void ExecuteAddMinionToVillain(string minionName, string villainName, SqlConnection connection)
        {
            string insert = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

            SqlCommand insertMinionToVilain = new SqlCommand(insert, connection);

            using (insertMinionToVilain)
            {
                insertMinionToVilain.Parameters.AddWithValue("@villainId", villainId);
                insertMinionToVilain.Parameters.AddWithValue("@minionId", minionId);

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
            }
        }

        private static void ExecuteVilain(string villainName, SqlConnection connection)
        {
            string viillainId = "SELECT Id FROM Villains WHERE Name = @Name";

            SqlCommand checkVillain = new SqlCommand(viillainId, connection);

            using (checkVillain)
            {
                checkVillain.Parameters.AddWithValue("@Name", villainName);

                object targetId = checkVillain.ExecuteScalar();

                if (targetId != null)
                {
                    villainId = (int)targetId;
                }
                else
                {
                    string insert = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

                    SqlCommand insertVillain = new SqlCommand(insert, connection);

                    using (insertVillain)
                    {
                        insertVillain.Parameters.AddWithValue("@villainName", villainName);
                        insertVillain.ExecuteNonQuery();

                        Console.WriteLine($"Villain {villainName} was added to the database.");
                    }
                }
            }
        }

        private static void ExecuteMinion(string minionName, int minionAge, SqlConnection connection)
        {
            string takeMinionId = "SELECT Id FROM Minions WHERE Name = @Name";

            SqlCommand checkMinion = new SqlCommand(takeMinionId, connection);

            using (checkMinion)
            {
                checkMinion.Parameters.AddWithValue("@Name", minionName);

                object targetId = checkMinion.ExecuteScalar();

                if (targetId != null)
                {
                    minionId = (int)targetId;
                }
                else
                {
                    string insert = "INSERT INTO Minions (Name, Age, TownId) VALUES (@nam, @age, @townId)";
                    SqlCommand insertMinion = new SqlCommand(insert, connection);

                    using (insertMinion)
                    {
                        insertMinion.Parameters.AddWithValue("@nam", minionName);
                        insertMinion.Parameters.AddWithValue("@age", minionAge);
                        insertMinion.Parameters.AddWithValue("@townId", townId);
                        insertMinion.ExecuteNonQuery();

                        Console.WriteLine($"Minion {minionName} was added to the database.");
                    }
                }
            }
        }

        private static void ExecuteTown(string minionTown, SqlConnection connection)
        {
            string takeTownId = @"SELECT Id FROM Towns WHERE Name = @townName";

            SqlCommand checkTown = new SqlCommand(takeTownId, connection);

            using (checkTown)
            {
                checkTown.Parameters.AddWithValue("@townName", minionTown);

                object targetId = checkTown.ExecuteScalar();

                if (targetId != null)
                {
                    townId = (int)targetId;
                }
                else
                {
                    string insertTown = "INSERT INTO Towns (Name) VALUES (@townName)";

                    SqlCommand insert = new SqlCommand(insertTown, connection);

                    using (insert)
                    {
                        insert.Parameters.AddWithValue("@townName", minionTown);
                        insert.ExecuteNonQuery();

                        Console.WriteLine($"Town {minionTown} was added to the database.");
                    }
                }
            }
        }
    }
}