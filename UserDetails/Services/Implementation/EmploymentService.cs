using System.Net.Http.Headers;
using UserDetails.DTOs;
using UserDetails.Models;
using UserDetails.Repositories.Interface;
using UserDetails.Services.Interface;

namespace UserDetails.Services.Implementation
{
    public class EmploymentService : IEmploymentService
    {
        private readonly IEmploymentRepository _repo;
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public EmploymentService(IEmploymentRepository repo, HttpClient http, IConfiguration config)
        {
            _repo = repo;
            _http = http;
            _config = config;
        }

        public async Task CreateEmployment(CreateEmploymentDto dto, Guid userId, string token)
        {
            // Send some fields to Service1
            var baseUrl = _config["ServiceUrls:AuthService"];

            //Create Request With Authroization Header
            var request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/api/internal/users/{userId}/employment");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(new { dto.EmploymentID, dto.PanCard });

            var response = await _http.SendAsync(request); // ✅ Token is now sent

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update Service 1: {response.StatusCode}");
            }

            var entity = new EmploymentDetails
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DOJ = dto.DOJ,
                EmploymentID = dto.EmploymentID,
                PanCard = dto.PanCard,
                AadharCard = dto.AadharCard
            };

            await _repo.CreateAsync(entity);
        }

        public async Task UpdateEmployment(Guid userId, UpdateEmploymentDto dto, string token) // ✅ ADD token
        {
            var entity = await _repo.GetByUserIdAsync(userId);

            if (entity == null)
                throw new Exception("Employment record not found");

            var baseUrl = _config["ServiceUrls:AuthService"];

            // ✅ Create request with Authorization header
            var request = new HttpRequestMessage(HttpMethod.Put,
                $"{baseUrl}/api/internal/users/{userId}/employment");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(new { dto.EmploymentID, dto.PanCard });

            await _http.SendAsync(request); // ✅ Token is now sent

            entity.DOJ = dto.DOJ;
            entity.EmploymentID = dto.EmploymentID;
            entity.PanCard = dto.PanCard;
            entity.AadharCard = dto.AadharCard;

            await _repo.UpdateAsync(entity);
        }
    }
}
