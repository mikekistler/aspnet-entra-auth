using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Get the Authentication and Authorization settings from configuration
IEnumerable<string>? initialScopes = builder.Configuration.GetSection("DownstreamApi:Scopes").Get<IEnumerable<string>>();

// The following line configures Microsoft Identity Web to authenticate using Azure AD settings,
// enabling token acquisition to securely call downstream APIs.
// builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd")
//     .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
//         .AddDownstreamApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
//         .AddInMemoryTokenCaches();

builder.Services.AddRazorPages();//.AddMvcOptions(options =>
    // {
    //     var policy = new AuthorizationPolicyBuilder()
    //                   .RequireAuthenticatedUser()
    //                   .Build();
    //     options.Filters.Add(new AuthorizeFilter(policy));
    // }).AddMicrosoftIdentityUI();

WebApplication app = builder.Build();

// app.UseAuthentication();
// app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.MapGet("/", () => "Hello World!");

app.Run();
