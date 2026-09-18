using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Tests;

public sealed class ProductEndpointsTests(WebApplicationFactory<global::Program> factory) : IClassFixture<WebApplicationFactory<global::Program>>
{
    [Fact]
    public async Task Lists_seed_products()
    {
        var products = await factory.CreateClient().GetFromJsonAsync<List<ProductResponse>>("/api/products");

        Assert.NotNull(products);
        Assert.Contains(products, product => product.Name == "Keyboard");
    }

    [Fact]
    public async Task Creates_a_valid_product()
    {
        var response = await factory.CreateClient().PostAsJsonAsync("/api/products", new { name = "Headset", price = 79.99m });

        Assert.True(response.IsSuccessStatusCode);
    }

    private sealed record ProductResponse(int Id, string Name, decimal Price);
}
