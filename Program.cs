using System.Net.Http.Headers;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Net.Http.Headers;
using MVC_API_Client.Service.eBay;
using Serilog;
using Serilog.Core;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Set the minimum log level
    .WriteTo.Console()
    .WriteTo.File("logs/eBay.txt", rollingInterval: RollingInterval.Day, levelSwitch: new LoggingLevelSwitch {MinimumLevel = LogEventLevel.Error}) // Log to file with daily rolling
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

// Add rate limiter service
/*
 * The numbers below are picked randomly (based on trial and error).
 * I still haven't spent much time researching this topic. Thus,
 * 
 * TODO: implement a more realistic rate limiting policy
 */
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    RateLimitPartition.GetFixedWindowLimiter(
	   string.Format("{0}", httpContext.Request.Cookies.FirstOrDefault().Value),
	   factory: partition => new FixedWindowRateLimiterOptions
	   {
		  AutoReplenishment = true,
		  PermitLimit = 5,
		  QueueLimit = 0,
		  Window = TimeSpan.FromSeconds(1.25)
	   }));
});

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

/*
 * Add a guid coockie to a request from a new client (i.e., if the request doesn't already have that coockie)
 * this cookie is used as an id in the rate limiter partitioning
 */
app.Use(async (context, next) =>
{
    if (context.Request.Cookies.Count == 0)
	   context.Response.Cookies.Append("guid", Guid.NewGuid().ToString(), new()
	   {
		  Path = "/",
		  HttpOnly = true,
		  SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
	   });

    await next(context);
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseRateLimiter();

app.UseAuthorization();

app.MapStaticAssets();

// Use static web assets in environments other than Development
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
