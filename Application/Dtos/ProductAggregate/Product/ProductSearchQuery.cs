namespace Application.Dtos.ProductAggregate.Product;
public class ProductSearchQuery
{
    public ProductSearchQuery()
    {

    }
    public ProductSearchQuery(string? searchKey,
                              double? minPrice,
                              double? maxPrice,
                              bool? exist,
                              SortFilterParam? sort,
                              long[]? categories)
    {
        SearchKey = searchKey;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        Exist = exist;
        Sort = sort;
        Categories = categories;
    }
    public string? SearchKey { get; set; }
    public double? MinPrice { get; set; }
    public double? MaxPrice { get; set; }
    public bool? Exist { get; set; }
    public SortFilterParam? Sort { get; set; }
    public long[]? Categories { get; set; }
}
