using System.ComponentModel.DataAnnotations;
using static PetStore.Data.Models.DataValidation;

namespace PetStore.Web.ViewModels
{
    public class CreateCategoryInputModel
    {
        [Required]
        [MaxLength(NameMaxLegth)]
        public string Name { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }
    }
}
