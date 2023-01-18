using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebBlazorWasm;
using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Bootstrap5;

using Template.Gateway;
using GW.Common;
using GW.Membership.Models;
using Template.ServerCode;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddSingleton<IAppSettings, TemplateAppSettings>();
builder.Services.AddSingleton<IAuthGatewayManager, AuthGateway>();
builder.Services.AddSingleton<IMembershipGatewayManager, MembershipGateway>();
builder.Services.AddSingleton<ITemplateGatewayManager, TemplateGateway>();
builder.Services.AddSingleton<IDataCacheGatewayManager, DataCacheGateway>();

builder.Services.AddScoped<IAppControllerAsync<UserAuthenticated>, TemplateAppController>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

await builder.Build().RunAsync();
