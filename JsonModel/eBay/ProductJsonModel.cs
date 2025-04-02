namespace MVC_API_Client.JsonModel.eBay;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


public partial class ProductJsonModel
{
    [JsonPropertyName("href")]
    public Uri Href { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("next")]
    public Uri Next { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("itemSummaries")]
    public List<ItemSummary> ItemSummaries { get; set; }
}

public partial class ItemSummary
{
    [JsonPropertyName("itemId")]
    public string ItemId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("leafCategoryIds")]
    public List<string> LeafCategoryIds { get; set; }

    [JsonPropertyName("categories")]
    public List<Category> Categories { get; set; }

    [JsonPropertyName("image")]
    public Image Image { get; set; }

    [JsonPropertyName("price")]
    public Price Price { get; set; }

    [JsonPropertyName("itemHref")]
    public Uri ItemHref { get; set; }

    [JsonPropertyName("seller")]
    public Seller Seller { get; set; }

    [JsonPropertyName("condition")]
    public string Condition { get; set; }

    [JsonPropertyName("conditionId")]
    public string ConditionId { get; set; }

    [JsonPropertyName("thumbnailImages")]
    public List<Image> ThumbnailImages { get; set; }

    [JsonPropertyName("shippingOptions")]
    public List<ShippingOption> ShippingOptions { get; set; }

    [JsonPropertyName("buyingOptions")]
    public List<string> BuyingOptions { get; set; }

    [JsonPropertyName("itemAffiliateWebUrl")]
    public Uri ItemAffiliateWebUrl { get; set; }

    [JsonPropertyName("itemWebUrl")]
    public Uri ItemWebUrl { get; set; }

    [JsonPropertyName("itemLocation")]
    public ItemLocation ItemLocation { get; set; }

    [JsonPropertyName("additionalImages")]
    public List<Image> AdditionalImages { get; set; }

    [JsonPropertyName("adultOnly")]
    public bool AdultOnly { get; set; }

    [JsonPropertyName("legacyItemId")]
    public string LegacyItemId { get; set; }

    [JsonPropertyName("availableCoupons")]
    public bool AvailableCoupons { get; set; }

    [JsonPropertyName("itemCreationDate")]
    public DateTimeOffset ItemCreationDate { get; set; }

    [JsonPropertyName("topRatedBuyingExperience")]
    public bool TopRatedBuyingExperience { get; set; }

    [JsonPropertyName("priorityListing")]
    public bool PriorityListing { get; set; }

    [JsonPropertyName("listingMarketplaceId")]
    public string ListingMarketplaceId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("marketingPrice")]
    public MarketingPrice MarketingPrice { get; set; }
}

public partial class Image
{
    [JsonPropertyName("imageUrl")]
    public Uri ImageUrl { get; set; }
}

public partial class ItemLocation
{
    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }
}

public partial class MarketingPrice
{
    [JsonPropertyName("originalPrice")]
    public Price OriginalPrice { get; set; }

    [JsonPropertyName("discountPercentage")]
    public string DiscountPercentage { get; set; }

    [JsonPropertyName("discountAmount")]
    public Price DiscountAmount { get; set; }

    [JsonPropertyName("priceTreatment")]
    public string PriceTreatment { get; set; }
}

public partial class Price
{
    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }
}

public partial class Seller
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("feedbackPercentage")]
    public string FeedbackPercentage { get; set; }

    [JsonPropertyName("feedbackScore")]
    public int FeedbackScore { get; set; }
}

public partial class ShippingOption
{
    [JsonPropertyName("shippingCostType")]
    public string ShippingCostType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("shippingCost")]
    public Price ShippingCost { get; set; }
}