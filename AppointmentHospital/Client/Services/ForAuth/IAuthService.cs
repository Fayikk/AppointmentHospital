using AppointmentHospital.Shared;
using AppointmentHospital.Shared.Model;

namespace AppointmentHospital.Client.Services.ForAuth
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin user);
        Task<bool> IsUserAuthenticated();
    }
}
