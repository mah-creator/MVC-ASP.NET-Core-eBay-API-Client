using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_API_Client.Models;
using MVC_API_Client.Service;
using static MVC_API_Client.Service.EbayClient;

namespace MVC_API_Client.Controllers;

public class HomeController : Controller
{
    private const int MAX_SUBCATS = 3;
    private readonly ILogger<HomeController> _logger;
    private readonly EbayClient _ebayClient;
    private IndexViewModel _indexViewModel;

    public HomeController(ILogger<HomeController> logger, EbayClient ebayClient)
    {
        _ebayClient = ebayClient;
        _logger = logger;
    }

    public IActionResult Index()
    {
        IndexViewModel model = new IndexViewModel 
        {
            Options = new List<SelectListItem>
            {
                new SelectListItem {Text = "1", Value = "1"},
                new SelectListItem {Text = "2", Value = "2"}
            },
            SelectedValue = 0
        };

        _indexViewModel = model;
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public string Test(int selectedValue)
    {
        return JsonSerializer.Serialize(_ebayClient.GetSubcategories(selectedValue+"", MAX_SUBCATS));
    }

    [HttpGet]
    public async Task<string> Test2(int selectedValue)
    {
        return JsonSerializer.Serialize(_ebayClient.SearchProductsByCategory(selectedValue+"", MAX_SUBCATS));
    }
}
