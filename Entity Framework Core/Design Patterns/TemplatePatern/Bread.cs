﻿namespace TemplatePatern
{
    using System;

    public abstract class Bread
    {
        public abstract void MixIngridients();

        public abstract void Bake();

        public virtual void Slice()
        {
            Console.WriteLine($"Slicing the " + GetType().Name + " bread!");
        }

        public void Make()
        {
            this.MixIngridients();
            this.Bake();
            this.Slice();
        }
    }
}
