using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_API_Client.Models;
using MVC_API_Client.Service.eBay;

namespace MVC_API_Client.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly EbayClient _ebayClient;

    public HomeController(ILogger<HomeController> logger, EbayClient ebayClient)
    {
        _ebayClient = ebayClient;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(new IndexViewModel());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult ContactInfo()
    {
        ContactInfoViewModel model = new ContactInfoViewModel();
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult SearchItems()
    {
        byte[] buff = new byte[500];
        HttpContext.Request.Body.ReadAsync(buff, 0, buff.Length);
        Console.WriteLine(Encoding.Default.GetString(buff));
        return RedirectToAction("Index");
    }

    [HttpGet]
    public string GetSubcategories(int selectedValue)
    {
        return JsonSerializer.Serialize(_ebayClient.GetSubcategories(selectedValue+""));
    }

    [HttpGet]
    public async Task<string> GetItems(int selectedValue)
    {
        return JsonSerializer.Serialize(_ebayClient.SearchProductsByCategory(selectedValue+""));
    }
}
