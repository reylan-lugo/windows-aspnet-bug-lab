using System.Net.Http.Json;
using Frontend.Models;

namespace Frontend.Services;

public sealed class ProductCatalogClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<List<Product>>("/api/products", cancellationToken)
            ?? [];
    }
}
