namespace WebAPI_Projeto02.Pagination
{
    public class ProductsFilterPrice : QueryStringParameters
    {
        public decimal? Price { get; set; }
        public string? PriceCriterion { get; set; }
    }
}
