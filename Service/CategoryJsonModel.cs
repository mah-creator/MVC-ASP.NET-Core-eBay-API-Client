

namespace MVC_API_Client.Service;

using System;
using System.Collections.Generic;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

public partial class CategoryJsonModel
{
    [JsonPropertyName("categoryTreeId")]
    public string CategoryTreeId { get; set; }

    [JsonPropertyName("categoryTreeVersion")]
    public string CategoryTreeVersion { get; set; }

    [JsonPropertyName("categorySubtreeNode")]
    public TreeNode CategorySubtreeNode { get; set; }
}

public partial class TreeNode
{
    [JsonPropertyName("category")]
    public Category Category { get; set; }

    [JsonPropertyName("parentCategoryTreeNodeHref")]
    public Uri ParentCategoryTreeNodeHref { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("childCategoryTreeNodes")]
    public List<TreeNode> ChildCategoryTreeNodes { get; set; }

    [JsonPropertyName("categoryTreeNodeLevel")]
    public int CategoryTreeNodeLevel { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("leafCategoryTreeNode")]
    public bool? LeafCategoryTreeNode { get; set; }
}

public partial class Category
{
    [JsonPropertyName("categoryId")]
    public string CategoryId { get; set; }

    [JsonPropertyName("categoryName")]
    public string CategoryName { get; set; }
}