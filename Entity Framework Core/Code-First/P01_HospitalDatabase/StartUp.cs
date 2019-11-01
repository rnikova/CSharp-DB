namespace P01_HospitalDatabase
{
    using P01_HospitalDatabase.Data;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new HospitalContext();
            db.Database.EnsureCreated();
        }
    }
}
