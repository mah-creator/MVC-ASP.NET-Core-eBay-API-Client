### Provide eBay API credentials through `dotnet user-secrets`

For more details about .NET secret storage, [go here](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=windows#enable-secret-storage).

**The eBay API keyset are read through the project's secret storage
In order to run the project properly, you need to store your eBay API keyset in the secret storage.**
Follow these steps to do so:
1) Enable secret storage
    ```.NET CLI
    dotnet user-secrets init
    ```
2) Set the `ClientId` and `ClientSecret` secrets

    ``` .NET CLI
    dotnet user-secrets set "eBayCredentials:ClientId" "your-ebay-clientId"
    ```
    ``` .NET CLI
    dotnet user-secrets set "eBayCredentials:ClientSecret" "your-ebay-clientSecret"
    ```
**That's it, you're ready to go!**