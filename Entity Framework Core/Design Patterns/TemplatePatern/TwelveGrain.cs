namespace TemplatePatern
{
    using System;

    public class TwelveGrain : Bread
    {
        public override void MixIngridients()
        {
            Console.WriteLine("Gathering ingridients for 12-grains bread.");
        }

        public override void Bake()
        {
            Console.WriteLine("Baking the 12-grains bread. (25 minutes)");
        }
    }
}
