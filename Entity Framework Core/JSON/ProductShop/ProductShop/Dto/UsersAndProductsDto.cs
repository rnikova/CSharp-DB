namespace ProductShop.Dto
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class UsersAndProductsDto
    {
        [JsonProperty("usersCount")]
        public int UsersCount { get; set; }

        [JsonProperty("users")]
        public ICollection<UsersWithProductsDto> Users { get; set; }
    }
}
