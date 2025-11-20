using Central.API.Data;
using Central.API.HostedServices;
using Central.API.Services;
using Central.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CentralApplicationDbContext>(options =>
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

var storesSection = builder.Configuration.GetSection("Stores").GetChildren();

foreach (var store in storesSection)
{
    builder.Services.AddHttpClient(store.Key, client =>
    {
        client.BaseAddress = new Uri(store.Value);
    });
}


builder.Services.AddScoped<ISyncWorkerProcessor, SyncWorkerProcessor>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISyncService, SyncService>();

builder.Services.AddHostedService<SyncWorkerHostedService>();

var app = builder.Build();

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CentralApplicationDbContext>();

    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
