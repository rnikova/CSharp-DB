﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static PetStore.Data.Models.DataValidation;
using static PetStore.Data.Models.DataValidation.User;

namespace PetStore.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLegth)]
        public string Name { get; set; }

        [MaxLength(EmailMaxLength)]
        public string Email { get; set; }

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
