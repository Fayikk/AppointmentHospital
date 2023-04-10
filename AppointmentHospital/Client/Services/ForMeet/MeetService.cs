using AppointmentHospital.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace AppointmentHospital.Client.Services.ForMeet
{
    public class MeetService : IMeetService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public MeetService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public List<Meet> Meets { get ; set ; }

        public async Task<ServiceResponse<Meet>> CancelMeet(int meetId)
        {
            var response = await _httpClient.PutAsJsonAsync<ServiceResponse<Meet>>($"api/meet/cancelMeet/{meetId}", null);
            if (response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<Meet>>();
                var errors = new ServiceResponse<Meet>
                {
                    Message = errorResponse.Message,
                    Success = errorResponse.Success,
                };
                return errors;
            }
            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<Meet>>();
            return responseData;
        }

        public async Task<ServiceResponse<Meet>> CreateMeet(Meet meet)
        {
            var response = await _httpClient.PostAsJsonAsync("api/meet/createMeet", meet);//istek
            if (response.IsSuccessStatusCode)//200 durum kodu gönderdiyse(201,204)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ServiceResponse<Meet>>();//endpointten geri dönen değerleri okur
                var errors = new ServiceResponse<Meet>//geri dönüş değeri için gerekli atamalar
                {
                    Message = errorResponse.Message,
                    Success = errorResponse.Success,
                };
                return errors;
            }
            var responseData = await response.Content.ReadFromJsonAsync<ServiceResponse<Meet>>();
            return responseData;
        }

        public async Task<ServiceResponse<List<Meet>>> GetMeet()
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Meet>>>("api/meet");
            return result;
        }
    }
}
