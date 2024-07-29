
using OnlineStore.Implementations;
using OnlineStore.Services;
using OnlineStore.Implementations;
using OnlineStore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
