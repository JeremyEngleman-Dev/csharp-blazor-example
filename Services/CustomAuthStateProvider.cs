using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Blazored.LocalStorage;

namespace BlazorIntro.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public CustomAuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            ClaimsIdentity identity;

            if (!string.IsNullOrEmpty(savedToken))
            {
                var claims = ParseClaimsFromJwt(savedToken);
                identity = new ClaimsIdentity(claims, "jwt");
            }
            else
            {
                identity = new ClaimsIdentity(); // anonymous
            }

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task MarkUserAsAuthenticated(string token)
        {
            await _localStorage.SetItemAsync("authToken", token);
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity))));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorage.RemoveItemAsync("authToken");
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = Convert.FromBase64String(PadBase64(payload));
            var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var claims = keyValuePairs!.Select(kvp =>
                kvp.Key == "role"
                    ? new Claim(ClaimTypes.Role, kvp.Value.ToString()!)
                    : new Claim(kvp.Key, kvp.Value.ToString()!)
            );

            return claims;
        }

        private string PadBase64(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return base64;
        }
    }
}
