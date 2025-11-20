using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Store.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var http = new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!)
};

builder.Services.AddScoped(sp => http);

await builder.Build().RunAsync();
