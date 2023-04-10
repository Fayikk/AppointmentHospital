using AppointmentHospital.Shared;
using AppointmentHospital.Shared.Model;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace AppointmentHospital.Client.Services.ForAuth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _stateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider stateProvider)
        {
            _httpClient = httpClient;
            _stateProvider = stateProvider;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            return (await _stateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task<ServiceResponse<string>> Login(UserLogin user)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account/login", user);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account/register", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

        }
    }
}
