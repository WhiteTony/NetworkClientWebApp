using Microsoft.AspNetCore.Mvc.RazorPages;
using FruitWebApp.Models;
using System.Text.Json;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace FruitWebApp.Pages
{
  public class IndexModel : PageModel
  {
    // IHttpClientFactory set using dependency injection 
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Add the data model and bind the form data to the page model properties
    // Enumerable since an array is expected as a response
    [BindProperty]
    public IEnumerable<FruitModel> FruitModels { get; set; }
    [BindProperty]
    public IEnumerable<NetworkClientModel> NetworkClients { get; set; }

    // Begin GET operation code
    public async Task OnGet()
    {
      // Create the HTTP client using the FruitAPI named factory
      var httpClient2 = _httpClientFactory.CreateClient("NetworkClientAPI");

      // Perform the GET request and store the response. The empty parameter
      // in GetAsync doesn't modify the base address set in the client factory 
      using HttpResponseMessage response2 = await httpClient2.GetAsync("");

      // If the request is successful deserialize the results into the data model
      if (response2.IsSuccessStatusCode)
      {
        using var contentStream2 = await response2.Content.ReadAsStreamAsync();

        try
        { 
            // Log the raw JSON response for NetworkClients
            var jsonResponse = await new StreamReader(contentStream2).ReadToEndAsync();
            Console.WriteLine($"NetworkClients JSON response: {jsonResponse}");

            NetworkClients = JsonSerializer.Deserialize<IEnumerable<NetworkClientModel>>(jsonResponse);

        }
        catch (JsonException ex)
        {
            // Log the exception (use your preferred logging framework)
            Console.WriteLine($"Deserialization error: {ex.Message}");
        }
      }
      else
      {
          // Log the status code for debugging
          Console.WriteLine($"NetworkClientAPI response status: {response2.StatusCode}");
      }
    }
    // End GET operation code
  }
}

