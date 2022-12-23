using Microsoft.AspNetCore.Components.Web;
using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Bootstrap5;

using Template.Gateway;
using GW.Common;
using GW.Membership.Models;
using Template.ServerCode;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddSingleton<IAppSettings, TemplateAppSettings>();
builder.Services.AddSingleton<IAuthGatewayManager, AuthGateway>();
builder.Services.AddSingleton<IMembershipGatewayManager, MembershipGateway>();
builder.Services.AddSingleton<IDataCacheGatewayManager, DataCacheGateway>();
//builder.Services.AddSingleton<IAdminGatewayManager, AdminGateway>();
//builder.Services.AddSingleton<IDentistaGatewayManager, ModuloDentistaGateway>();

builder.Services.AddScoped<IAppControllerAsync<UserAuthenticated>, TemplateAppController>();
//builder.Services.AddScoped<IMenuItemActive, MenuItemActive>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Environment.WebRootPath) });

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
