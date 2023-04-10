using AppointmentHospital.Shared;
using System.Net.Http.Json;

namespace AppointmentHospital.Client.Services.ForDoctor
{
    //Delegate
    public class DoctorService : IDoctorService
    {
        private readonly HttpClient _client;
        public DoctorService(HttpClient client)
        {
            _client = client;
        }

        public List<Doctor> Doctors { get ; set ; } = new List<Doctor>();
        public string Message { get ; set ; }

        public event Action doctorsChanged;

        public async Task CreateDoctor(Doctor doctor)
        {
            var response = await _client.PostAsJsonAsync("api/doctor/addDoctor", doctor);
            doctorsChanged?.Invoke();
        }

        public async Task GetDoctor(int id)
        {
            var result = await _client.GetFromJsonAsync<ServiceResponse<List<Doctor>>>($"api/doctor/{id}");
            if (result != null)
            {
                Doctors = result.Data;
            }
            doctorsChanged?.Invoke();
        }

        public async Task GetDoctors()
        {
            var result = await _client.GetFromJsonAsync<ServiceResponse<List<Doctor>>>("api/doctor/getAll");
            if (result != null && result.Data != null)
            {
                Doctors = result.Data;
            }
        }
    }
}
