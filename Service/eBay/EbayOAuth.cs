using System.Text;
using Newtonsoft.Json;

namespace MVC_API_Client.Service.eBay;

public class EbayOAuth
{    
    // Token endpoint
    private static readonly string tokenUrl = "https://api.ebay.com/identity/v1/oauth2/token";

    // Most recently granted token
    private static string token;
    // The lease time for the most recent token
    private static int tokenLeastTime = 0;
    // Issuance time (in milliseconds) of the most recent token 
    private static long tokenIssuance = 0;
    // subtracted from lease time when determining a token expiry as an additional safety measure
    private static int GUARD_PERIOD = 720; 

    private HttpClient _ebayOAuthClient;

    public EbayOAuth(HttpClient eBayOAuthClient)
    {
        _ebayOAuthClient = eBayOAuthClient;
    }
    public async Task<string> GetAccessTokenAsync()
    {
        if(TokenIsExpired())
            await MintToken();
        return token;
    }


    private async Task MintToken()
    {
        // Prepare the POST data (application/x-www-form-urlencoded)
        // <p>The <a href="https://developer.ebay.com/api-docs/static/oauth-client-credentials-grant.html#:~:text=Configuring%20the%20request%20payload">request body</a> is specified in the Identity API documentation
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
            token = responseObject.access_token;
            tokenLeastTime = responseObject.expires_in;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static bool TokenIsExpired()
    {
        return (CurrentSeconds() - tokenIssuance >= (tokenLeastTime - GUARD_PERIOD))? true : false; 
    }
    
    // return the seconds elapsed from unix timestamp
    private static long CurrentSeconds()
    {
        return (DateTime.UtcNow - DateTime.UnixEpoch).Seconds;
    }
}
