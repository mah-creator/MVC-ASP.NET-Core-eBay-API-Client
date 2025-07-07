using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_API_Client.JsonModel.eBay;
using MVC_API_Client.Service.eBay;

namespace MVC_API_Client.Models;

public class IndexViewModel
{
    [BindProperty]
    public List<ProductBasicInfo> Products { get; set; } = new List<ProductBasicInfo>();
   
    [BindProperty]
    public List<SelectListItem>? MarketPlaces { get; set; } = new List<SelectListItem>
    {
        new SelectListItem("Australia", "AU"),
        new SelectListItem("United States", "US")
    };

    [BindProperty]
    public List<Category> Categories { get; set; } = new List<Category> 
    {
        new Category{CategoryId = "0", CategoryName = "-- select a category --"},
        new Category{CategoryId = "165", CategoryName = "Drives, Storage & Blank Media"},
        new Category{CategoryId = "31530", CategoryName = "Laptop & Desktop Accessories"},
        new Category{CategoryId = "11176", CategoryName = "Home Networking & Connectivity"}        
    };

    public IndexViewModel()
    {
    }
}
