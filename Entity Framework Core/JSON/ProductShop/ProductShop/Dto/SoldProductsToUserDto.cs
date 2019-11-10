namespace ProductShop.Dto
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class SoldProductsToUserDto
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("products")]
        public ICollection<SoldProductsDto> Products { get; set; }
    }
}
