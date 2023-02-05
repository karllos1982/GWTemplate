using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Template.API;
using GW.Common;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Template.Core.Manager;
using GW.Core;
using GW.Membership.Data;
using GW.Membership.Contracts.Data;
using GW.Membership.Contracts.Domain;
using GW.Membership.Domain;
using GW.ApplicationHelpers;
using Core.Data;
using Template.Contracts.Domain;
using Template.Contracts.Data;
using Template.Domain;
using Template.Data;
using API.Code;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<ISettings, TemplateSettings>();
builder.Services.AddScoped<IContextBuilder, ContextBuilder>();
builder.Services.AddScoped<IContext, DapperContext>();
builder.Services.AddScoped<IMembershipRepositorySet, MembershipRepositorySet>();
builder.Services.AddScoped<IMembershipManager, MembershipManager>();
builder.Services.AddScoped<ITemplateRepositorySet, TemplateRepositorySet>();
builder.Services.AddScoped<ITemplateManager, TemplateManager>();
builder.Services.AddScoped<MailManager, TemplateMailCenter>(); 

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

builder.Services.AddSwaggerGen(c =>
{
   
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Template-API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization header using the Bearer scheme.
                    Enter 'Bearer'[space] and then your token in the text input below.
                    Example: Bearer 12345abcdef",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });


});

builder.Services.AddMvc()
    .AddJsonOptions(op => op.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()  );
         

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Template-API V1");
    
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
