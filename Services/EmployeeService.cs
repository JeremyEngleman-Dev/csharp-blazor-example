using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

public class EmployeeService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;

    public EmployeeService(HttpClient http, ILocalStorageService localStorage)
    {
        _http = http;
        _localStorage = localStorage;
    }

    private async Task AddAuthHeaderAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    /*
    public async Task<Employee[]> GetEmployeesAsync()
    {
        await AddAuthHeaderAsync();
        return await _http.GetFromJsonAsync<Employee[]>("https://your-api.com/api/employees") 
               ?? Array.Empty<Employee>();
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        await AddAuthHeaderAsync();
        return await _http.GetFromJsonAsync<Employee?>($"https://your-api.com/api/employees/{id}");
    }
    */

    // Add more secure API calls here
}
