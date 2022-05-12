using DavidsGrocery.Repository;
using DavidsGrocery.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Shopkeep", policy =>
    {
        policy.RequireRole("Shopkeep");
    });
});

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
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Value.Split(","))
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSingleton(new CosmosClient(builder.Configuration.GetConnectionString("CosmosDb"), new CosmosClientOptions()
{
    SerializerOptions = new CosmosSerializationOptions()
    {
        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
    }
}));

builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IInventoryRepository, InventoryRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("Allow SPA");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
