using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class EbayOAuth
{
    private static readonly string clientId = "MahmoudT-TestScra-PRD-bafe06ab6-2cb8fd24"; // Replace with your Client ID (App ID)
    private static readonly string clientSecret = "PRD-afe06ab6f1b2-f7a8-4dd7-8836-2211"; // Replace with your Client Secret (Cert ID)
    
    // Token endpoint
    private static readonly string tokenUrl = "https://api.ebay.com/identity/v1/oauth2/token";

    public static async Task Main(string[] args)
    {
        var accessToken = await GetAccessTokenAsync();
        Console.WriteLine("Access Token: " + accessToken);
    }

    public static async Task<string> GetAccessTokenAsync()
    {
        using (var client = new HttpClient())
        {
            // Create Basic Authentication header using clientId and clientSecret
            var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Prepare the POST data (application/x-www-form-urlencoded)
            var postData = new StringContent(
                "grant_type=client_credentials&scope=https://api.ebay.com/oauth/api_scope", 
                Encoding.UTF8, 
                "application/x-www-form-urlencoded");

            try
            {
                // Send POST request to get the access token
                var response = await client.PostAsync(tokenUrl, postData);

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
}
