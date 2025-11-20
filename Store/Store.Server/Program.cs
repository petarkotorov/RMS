using Microsoft.EntityFrameworkCore;
using Store.API.Data;
using Store.API.HostedServices;
using Store.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoreApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient("CentralApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["CentralApi:BaseUrl"]);
});

builder.Services.AddScoped<ISyncWorkerProcessor, SyncWorkerProcessor>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISyncService, SyncService>();

builder.Services.AddHostedService<SyncWorkerHostedService>();

var app = builder.Build();

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoreApplicationDbContext>();

    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

