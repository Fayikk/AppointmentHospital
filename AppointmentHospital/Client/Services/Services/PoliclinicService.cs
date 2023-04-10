using AppointmentHospital.Shared;
using System.Net.Http.Json;

namespace AppointmentHospital.Client.Services.Services
{
    public class PoliclinicService : IPoliclinicService
    {
        private readonly HttpClient _client;
        public PoliclinicService(HttpClient client)
        {
            _client = client;
        }

        public List<Policlinic> Policlinics { get ; set ; } = new List<Policlinic>();

        public event Action OnChange;
    

        public async Task CreatePoliclinics(Policlinic policlinic)
        {
            var result = await _client.PostAsJsonAsync("api/policlinic/add", policlinic);
            OnChange?.Invoke();
        }

        public async Task DeletePoliclinics(int id)
        {
            var result = await _client.DeleteAsync($"api/policlinic/delete/{id}");
             OnChange?.Invoke();
           
        }

        public async Task<List<Policlinic>> GetPoliclinics()
        {
            var result = await _client.GetFromJsonAsync<ServiceResponse<List<Policlinic>>>("api/policlinic/getall");
            if(result != null)
            {
                Policlinics = result.Data;
            }
            OnChange?.Invoke();
            return Policlinics;
        }
    }
}
