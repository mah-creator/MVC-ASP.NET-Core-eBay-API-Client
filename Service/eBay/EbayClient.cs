using System.Text.Json;
using System.Net.Http.Headers;
using MVC_API_Client.JsonModel.eBay;

namespace MVC_API_Client.Service.eBay;

public class EbayClient
{
    private const int SUBCATEGORY_LIMIT = 4;
    private const int ITEM_LIMIT = 3;
    private readonly HttpClient _httpClient;
    private readonly EbayOAuth _eBayOAuthClient;
    private readonly ILogger<EbayClient> _logger;
    public EbayClient(HttpClient httpClient, EbayOAuth ebayOAuthClient, ILogger<EbayClient> logger)
    {
        _httpClient = httpClient;
        _eBayOAuthClient = ebayOAuthClient;
        _logger = logger;
    }

    public async Task<List<Category>> GetSubcategories(string parentId)
    {
        string accessToken =  await _eBayOAuthClient.GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        HttpResponseMessage response = await _httpClient.GetAsync($"https://api.ebay.com/commerce/taxonomy/v1/category_tree/15/get_category_subtree?category_id={parentId}");
        string jsonString = await response.Content.ReadAsStringAsync();
        List<Category> subcategories = new List<Category>();
        if(response.IsSuccessStatusCode)
        {
            CategoryJsonModel? categoryJsonModel = JsonSerializer.Deserialize<CategoryJsonModel>(jsonString);
            
            var demoSubcategoryList = categoryJsonModel.CategorySubtreeNode.ChildCategoryTreeNodes[0..SUBCATEGORY_LIMIT];
            foreach (TreeNode childCategoryTreeNode in demoSubcategoryList)
            {
                subcategories.Add(childCategoryTreeNode.Category);
            }
        }
        else
        {
            _logger.LogError($"""
                {Environment.NewLine}
                Token issued: {EbayOAuth.tokenIssuance} {Environment.NewLine}
                Token lease : {EbayOAuth.tokenLeastTime} {Environment.NewLine}
                Current Time: {EbayOAuth.currentSeconds} {Environment.NewLine}
                -- Server response -- {Environment.NewLine}
                {jsonString}
                ---- {Environment.NewLine}
            """);
        }
        return subcategories;
    }

    public async Task<List<ProductBasicInfo>> SearchProductsByCategory(string categoryID)
    {
        string accessToken =  await _eBayOAuthClient.GetAccessTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        HttpResponseMessage response = await _httpClient.GetAsync($"https://api.ebay.com/buy/browse/v1/item_summary/search?category_ids={categoryID}&limit={ITEM_LIMIT}");
        string jsonString = await response.Content.ReadAsStringAsync();
        List<ProductBasicInfo> products = new List<ProductBasicInfo>();
        if(response.IsSuccessStatusCode)
        {
            ProductJsonModel? productJsonModel = JsonSerializer.Deserialize<ProductJsonModel>(jsonString);
                      
            foreach (ItemSummary itemSummary in productJsonModel.ItemSummaries)
            {
                products.Add(new ProductBasicInfo 
                {
                    Title= itemSummary.Title, ImageUrl = itemSummary.ThumbnailImages[0].ImageUrl.ToString(),
                    Price = itemSummary.Price.Value + " " + itemSummary.Price.Currency,
                    ProductUrl = itemSummary.ItemWebUrl.ToString()
                });
            }
        }
        else
        {
            Console.WriteLine(_httpClient.DefaultRequestHeaders.Authorization);
            Console.WriteLine(jsonString);
        }
        return products;
    }

    public class ProductBasicInfo
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Price { get; set; } 
        public string ProductUrl { get; set; }
    }
}