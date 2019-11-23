namespace TemplatePatern
{
    using System;

    public class Sourdough : Bread
    {
        public override void MixIngridients()
        {
            Console.WriteLine("Gathering ingridients for Sourdough bread.");
        }

        public override void Bake()
        {
            Console.WriteLine("Baking the Sourdough bread. (15 minutes)");
        }
    }
}
