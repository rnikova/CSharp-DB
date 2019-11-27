using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static PetStore.Data.Models.DataValidation;

namespace PetStore.Data.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLegth)]
        public string Name { get; set; }

        public ICollection<Toy> Toys { get; set; } = new HashSet<Toy>();

        public ICollection<Food> Food { get; set; } = new HashSet<Food>();
    }
}
