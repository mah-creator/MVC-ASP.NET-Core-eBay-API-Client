using System.Diagnostics;
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
    public static int tokenLeastTime = 0;
    // Issuance time (in milliseconds) of the most recent token 
    public static long tokenIssuance = 0;
    // This value is set every time the CurrentSeconds() method is called
    public static long currentSeconds;
    // subtracted from lease time when determining a token expiry as an additional safety measure
    private static int GUARD_PERIOD = 720; 

    private HttpClient _ebayOAuthClient;
    private ILogger<EbayOAuth> _logger;

    public EbayOAuth(HttpClient eBayOAuthClient, ILogger<EbayOAuth> logger)
    {
        _ebayOAuthClient = eBayOAuthClient;
        _logger = logger;
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

            // Send POST request to get the access token
            var response = await _ebayOAuthClient.PostAsync(tokenUrl, postData);
            // Read the response content (access token)
            var jsonResponse = await response.Content.ReadAsStringAsync();

        if(response.IsSuccessStatusCode)
        {

            // Deserialize the JSON response
            dynamic responseObject = JsonConvert.DeserializeObject(jsonResponse);

            // Extract the access token from the response
            token = responseObject.access_token;
            tokenLeastTime = responseObject.expires_in;
        }
        else
        {
            _logger.LogError($"""
            {Environment.NewLine}
            -- Server response --
            {jsonResponse} {Environment.NewLine}
            ---- {Environment.NewLine}
            """);
        }
    }

    private static bool TokenIsExpired()
    {
        return (CurrentSeconds() - tokenIssuance >= (tokenLeastTime - GUARD_PERIOD))? true : false; 
    }
    
    // return the seconds elapsed from unix timestamp
    private static long CurrentSeconds()
    {
        currentSeconds = (DateTime.UtcNow - DateTime.UnixEpoch).Seconds;
        return currentSeconds;
    }
}
