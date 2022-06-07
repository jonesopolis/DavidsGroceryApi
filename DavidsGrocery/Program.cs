using DavidsGrocery;
using DavidsGrocery.Repository;
using DavidsGrocery.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Cosmos;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

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
              .AllowAnyMethod()
              .AllowCredentials();
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

app.UseDeveloperExceptionPage();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("Allow SPA");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapHub<InventoryHub>("/hubs/inventory");


app.UseSwagger();
app.UseSwaggerUI();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.Run();
