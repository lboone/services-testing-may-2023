using Microsoft.AspNetCore.Mvc;
using SlugGenerators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<SlugGenerator>();
builder.Services.AddScoped<ICheckForUniqueValues, DummyUniqueChecker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost("/suppliers", async ([FromBody] CreateSupplierModelRequest request, [FromServices] SlugGenerator slugGenerator) =>
{
    var response = new CreateSupplierModelResponse
    {
        Slug = await slugGenerator.GenerateSlugForAsync(request.Name)
    };
    return Results.Ok(response);

});
app.MapGet("/suppliers/{supplierId}/products/{sku}", (string supplierId, string sku) =>
{

    if (supplierId == "bobs-store")
    {
        return Results.Ok(new SupplierPricingInformationResponse
        {
            AllowWholesale = true
        });
    }
    else
    {
        return Results.Ok(new SupplierPricingInformationResponse
        {
            AllowWholesale = false,
            RequiredMsrp = 823.88M
        });

    }
});

app.Run();

public record SupplierPricingInformationResponse
{
    public bool AllowWholesale { get; set; }
    public decimal? RequiredMsrp { get; set; }
}


public record CreateSupplierModelRequest
{
    public string Name { get; set; } = string.Empty;
}

public record CreateSupplierModelResponse
{
    public string Slug { get; set; } = string.Empty;
}


public class DummyUniqueChecker : ICheckForUniqueValues
{
    public async Task<bool> IsUniqueAsync(string attempt)
    {
        return true;
    }
}