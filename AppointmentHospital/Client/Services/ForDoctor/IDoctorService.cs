using AppointmentHospital.Shared;

namespace AppointmentHospital.Client.Services.ForDoctor
{
    public interface IDoctorService
    {
        event Action doctorsChanged;
        List<Doctor> Doctors { get; set; }
        string Message { get; set; }

        Task GetDoctor(int id);
        Task GetDoctors();
        Task CreateDoctor(Doctor doctor);
        
    }
}
