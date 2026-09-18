using Microsoft.AspNetCore.Mvc.RazorPages;
using Frontend.Models;
using Frontend.Services;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ProductCatalogClient _catalog;
    private readonly WindowsMachineProbe _machineProbe;

    public IndexModel(ProductCatalogClient catalog, WindowsMachineProbe machineProbe)
    {
        _catalog = catalog;
        _machineProbe = machineProbe;
    }

    public IReadOnlyList<Product> Products { get; private set; } = [];
    public string? OperatingSystemCaption { get; private set; }
    public bool IsLoading { get; private set; }
    public string? ErrorMessage { get; private set; }

    public async Task OnGetAsync()
    {
        IsLoading = true;
        OperatingSystemCaption = _machineProbe.GetOperatingSystemCaption();

        try
        {
            Products = await _catalog.GetProductsAsync();
            IsLoading = false;
        }
        catch (HttpRequestException)
        {
            // Intentional FRONTEND-01: keeps the page in a loading state and hides the failure.
            ErrorMessage = "The product API could not be reached.";
        }
    }
}
