using Central.Client;
using Central.Client.Config;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

using var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

var config = await http.GetFromJsonAsync<ClientConfig>("appsettings.json");

builder.Services.AddSingleton(config);

var httpClient = new HttpClient
{
    BaseAddress = new Uri(config.ApiBaseUrl)
};

builder.Services.AddScoped(sp => httpClient);
await builder.Build().RunAsync();
