namespace FacadePatern
{
    using System;

    public class StartUp
    {
        static void Main(string[] args)
        {
            var car = new CarBuilderFacade()
                .Info
                    .WithType("BMW")
                    .WithColor("Blue")
                    .WithNumberOfDoors(4)
                .Built
                    .InCity("Leipzig")
                    .AtAddress("address")
                .Build();

            Console.WriteLine(car);
        }
    }
}
