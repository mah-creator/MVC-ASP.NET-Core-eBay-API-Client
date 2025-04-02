using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static MVC_API_Client.Service.eBay.EbayClient;

namespace MVC_API_Client.Models;

public class IndexViewModel
{
    [BindProperty]
    public List<ProductBasicInfo> Products { get; set; } = new List<ProductBasicInfo>();
    [BindProperty]
    public string TEST_RESULT_VISIBILITY { get; } = "block";
    [BindProperty]
    public string PRODUCT_IMAGE_LINK { get; set;  }
    [BindProperty]
    public string TEST_PRODUCT_NAME { get; set;  }
    [BindProperty]
    public string TEST_PRODUCT_PRICE { get; set;  }
    [BindProperty]
    public string PRODUCT_LINK { get; set; }
    [BindProperty]
    public int SelectedValue { get; set; }
    [BindProperty]
    public List<SelectListItem>? Options { get; set; } = new List<SelectListItem>
        {
            new SelectListItem("Option1", "1"),
            new SelectListItem("Option2", "2"),
            new SelectListItem("Option3", "3")
        };

    public IndexViewModel()
    {
    }
}
