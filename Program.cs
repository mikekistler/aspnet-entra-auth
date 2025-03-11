using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = builder.Configuration;

// Retrieve certificate from Azure Key Vault
var keyVaultUrl = configuration["AzureAd:KeyVaultUrl"];
var certName = configuration["AzureAd:CertificateName"];

// Get the Authentication and Authorization settings from configuration
IEnumerable<string>? initialScopes = configuration.GetSection("DownstreamApi:Scopes").Get<IEnumerable<string>>();

// Add a keyvault client to the DI container for use by Microsoft.Identity.Web.
// Only use DefaultAzureCredential in development, and ManagedIdentityCredential in production
builder.Configuration.AddAzureKeyVault(
    new Uri(configuration["AzureAd:KeyVaultUrl"]!),
    builder.Environment.IsDevelopment()
        ? new DefaultAzureCredential()
        : new ManagedIdentityCredential()
);

// Configure authentication using Microsoft.Identity.Web (retrieves cert automatically)
builder.Services.AddMicrosoftIdentityWebAppAuthentication(configuration, "AzureAd")
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
    .AddInMemoryTokenCaches();

builder.Services.AddRazorPages().AddMvcOptions(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
                      .RequireAuthenticatedUser()
                      .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    }).AddMicrosoftIdentityUI();

WebApplication app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

// app.MapGet("/", (HttpContext context) => $"Hello {context.User.GetDisplayName()}!")
//     .RequireAuthorization();

app.Run();
