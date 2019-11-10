namespace ProductShop.Dto
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class UserSoldProductsDto
    {
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "soldProducts")]
        public ICollection<SoldProductsDto> SoldProducts { get; set; }
    }
}
