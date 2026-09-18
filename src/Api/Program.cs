using System.Diagnostics;
using System.ServiceProcess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var products = new List<Product>
{
    new(1, "Keyboard", 49.99m),
    new(2, "Mouse", 24.50m)
};

app.MapGet("/api/products", () => Results.Ok(products));

app.MapGet("/api/products/{id:int}", (int id) =>
{
    // Intentional API-01: an unknown id throws and becomes a 500 instead of a 404.
    return Results.Ok(products.Single(product => product.Id == id));
})
.WithOpenApi();

app.MapPost("/api/products", (CreateProductRequest request) =>
{
    // Intentional API-02: accepts invalid names and prices without validation.
    var product = new Product(products.Max(product => product.Id) + 1, request.Name, request.Price);
    products.Add(product);
    return Results.Created($"/api/products/{product.Id}", product);
})
.WithOpenApi();

app.MapGet("/api/windows/diagnostics", () =>
{
    var serviceCount = ServiceController.GetServices().Length;
    var applicationLogEntries = new EventLog("Application").Entries.Count;
    return Results.Ok(new WindowsDiagnostics(serviceCount, applicationLogEntries));
})
.WithOpenApi();

app.Run();

public sealed record Product(int Id, string Name, decimal Price);
public sealed record CreateProductRequest(string Name, decimal Price);
public sealed record WindowsDiagnostics(int ServiceCount, int ApplicationLogEntries);

public partial class Program
{
}
