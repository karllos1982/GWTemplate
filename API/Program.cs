using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Template.API;
using GW.Core.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IAppSettingsManager<TemplateSettings>, TemplateManagerSettings>();

//configure auth

var key = Encoding.ASCII.GetBytes(TokenService.PRIVATEKEY);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddMvc()
    .AddJsonOptions(op => op.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
           );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
