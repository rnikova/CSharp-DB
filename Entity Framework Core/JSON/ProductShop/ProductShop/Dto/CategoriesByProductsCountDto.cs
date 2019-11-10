namespace ProductShop.Dto
{
    using Newtonsoft.Json;

    public class CategoriesByProductsCountDto
    {
        [JsonProperty(PropertyName = "category")]
        public string CategoryName { get; set; }

        [JsonProperty(PropertyName = "productsCount")]
        public int ProductsCount { get; set; }

        [JsonProperty(PropertyName = "averagePrice")]
        public string AveragePrice { get; set; }

        [JsonProperty(PropertyName = "totalRevenue")]
        public string TotalRevenue { get; set; }
    }
}
