using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Web.ViewModels.Category
{
    public class CategoryEditInputModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Decription { get; set; }
    }
}
