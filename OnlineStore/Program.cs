
using OnlineStore.Implementations;
using OnlineStore.Services;
using OnlineStore.Implementations;
using OnlineStore.Services;
using OnlineStore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OnlineStore.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSetting>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDBService>();

// Add services to the container.


builder.Services.AddControllers();

builder.Services.AddAuthentication(cfg => {
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8
            .GetBytes("uweqyrtweuqwerytqweuirytqwueyrtuweqyrtweuqwerytqweuirytqwueyrt")
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
builder.Services.AddMvc();

builder.Services.AddScoped<IAuthenticationService, AuthImplementation>();
builder.Services.AddScoped<IProductService, ProductImplementation>();
builder.Services.AddScoped<IPurchaseService, PurchaseImplementation>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();
