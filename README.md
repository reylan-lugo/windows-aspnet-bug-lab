# Windows ASP.NET Bug Lab

A deliberately small Windows-only ASP.NET monorepo for exercising a Windows execution path.

## Projects

- `src/Api`: ASP.NET Core minimal API targeting `net8.0-windows`. It uses the Windows Service Control Manager and the Application Event Log.
- `src/Frontend`: ASP.NET Core Razor Pages targeting `net8.0-windows`. It queries WMI through `System.Management` and calls the API.

The solution can be cross-compiled from a non-Windows machine, but both applications must run on Windows because their Windows APIs are exercised at runtime.

For a Windows x64 artifact, publish with `dotnet publish -r win-x64`.

## Tests

`tests/Api.Tests` exercises only the healthy list/create flows. `tests/Frontend.Tests` covers product deserialization and includes a Windows-only WMI smoke test. The intentional defects are deliberately not covered, so the baseline suite remains green. Run the suite on Windows:

```powershell
dotnet test WindowsAspnetBugLab.sln
```

## Intentional bugs

1. **API-01 — unknown product becomes HTTP 500.** `GET /api/products/{id}` uses `Single`; an ID that does not exist throws instead of returning HTTP 404.
2. **API-02 — invalid product data is accepted.** `POST /api/products` does not validate blank names or non-positive prices.
3. **FRONTEND-01 — API errors leave the UI permanently loading.** The Razor Page catches an API failure but does not clear `IsLoading`, so the error is not shown to the user.
4. **FRONTEND-02 — prices ignore locale and currency.** The Razor Page renders the raw decimal value rather than a culture-aware currency value.

## Local Windows run

Run the API first:

```powershell
dotnet run --project src/Api
```

Then run the frontend in another PowerShell window:

```powershell
dotnet run --project src/Frontend
```

The frontend expects the API at `http://localhost:5050`; override `Api:BaseUrl` if your API port differs.
