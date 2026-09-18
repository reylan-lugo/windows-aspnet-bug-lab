using System.Net;
using System.Net.Http;
using System.Text;
using Frontend.Services;

namespace Frontend.Tests;

public sealed class ProductCatalogClientTests
{
    [Fact]
    public async Task Deserializes_products_from_the_api()
    {
        using var client = new HttpClient(new StubHandler("[{\"id\":1,\"name\":\"Keyboard\",\"price\":49.99}]"))
        {
            BaseAddress = new Uri("http://localhost")
        };
        var catalog = new ProductCatalogClient(client);

        var products = await catalog.GetProductsAsync();

        var product = Assert.Single(products);
        Assert.Equal("Keyboard", product.Name);
    }

    private sealed class StubHandler(string responseBody) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
            });
        }
    }
}
