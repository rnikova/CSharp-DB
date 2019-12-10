using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static PetStore.Data.Models.DataValidation;

namespace PetStore.Services.Models.Category
{
    public class EditCategoryServiceModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLegth)]
        public string Name { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Decription { get; set; }
    }
}
