using DavidsGrocery;
using DavidsGrocery.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(
           jwtOptions =>
           {
               jwtOptions.Audience = "api://b4373faf-503c-4930-9661-cb93b74e3625";
               builder.Configuration.Bind("AzureAd", jwtOptions);
           },
           msOptions =>
           {
               builder.Configuration.Bind("AzureAd", msOptions);
           });

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow SPA", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<CartRepository>(new CartRepository());

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("Allow SPA");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
