using System.Text;
using Newtonsoft.Json;

namespace MVC_API_Client.Service.eBay;

public class EbayOAuth
{    
    // Token endpoint
    private static readonly string tokenUrl = "https://api.ebay.com/identity/v1/oauth2/token";

    private HttpClient _ebayOAuthClient;

    public EbayOAuth(HttpClient eBayOAuthClient)
    {
        _ebayOAuthClient = eBayOAuthClient;
    }
    public async Task<string> GetAccessTokenAsync()
    {
        // Prepare the POST data (application/x-www-form-urlencoded)
        var postData = new StringContent(
            "grant_type=client_credentials&scope=https://api.ebay.com/oauth/api_scope", 
            Encoding.UTF8, 
            "application/x-www-form-urlencoded");

        try
        {
            // Send POST request to get the access token
            var response = await _ebayOAuthClient.PostAsync(tokenUrl, postData);

            // Ensure we got a successful response
            response.EnsureSuccessStatusCode();

            // Read the response content (access token)
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response
            dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);

            // Extract the access token from the response
            string accessToken = responseObject.access_token;

            return accessToken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
}
