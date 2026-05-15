using System.Net.Http.Headers;
using System.Net.Http.Json;
using UserDetails.Clients.Interface;
using UserDetails.DTOs;

namespace UserDetails.Clients.Implementation
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _http;

        public AuthClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<AuthUserDto?> ValidateTokenAsync(string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/validate");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AuthUserDto>();
        }

        public async Task<AuthUserDto?> GetUserByIdAsync(Guid userId, string token)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"/api/internal/users/{userId}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AuthUserDto>();
        }
    }
}