using System.Net.Http.Headers;
using System.Text;
using Microsoft.Net.Http.Headers;
using MVC_API_Client.Service.eBay;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// eBay brows and search API client
builder.Services.AddHttpClient<EbayClient>(client => {
    // marketplace header
    client.DefaultRequestHeaders.Add("X-EBAY-C-MARKETPLACE-ID", builder.Configuration["Marketplace"]);
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    client.DefaultRequestHeaders.Add(HeaderNames.AcceptEncoding, "utf-8");
});

builder.Services.AddHttpClient<EbayOAuth>(client => {
    // Create Basic Authentication header using clientId and clientSecret
    var byteArray = Encoding.ASCII.GetBytes($"{builder.Configuration["eBayCredentials:ClientId"]}:{builder.Configuration["eBayCredentials:ClientSecret"]}");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
