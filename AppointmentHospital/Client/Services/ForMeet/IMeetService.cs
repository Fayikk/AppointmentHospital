using AppointmentHospital.Shared;

namespace AppointmentHospital.Client.Services.ForMeet
{
    public interface IMeetService
    {
        List<Meet> Meets { get; set; }
        Task<List<Meet>> GetMeet();
        Task<ServiceResponse<Meet>> CreateMeet(Meet meet);
        Task<ServiceResponse<Meet>> CancelMeet(int id);
    }
}
