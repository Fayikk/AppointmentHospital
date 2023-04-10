using AppointmentHospital.Shared;
using AppointmentHospital.Shared.Model;

namespace AppointmentHospital.Server.Services.ForDoctor
{
    public interface IDoctorService
    {
        Task<ServiceResponse<List<Doctor>>> GetAll();
        Task<ServiceResponse<List<Doctor>>> GetByDoctor(int policlinicId);
        Task<ServiceResponse<DoctorDTO>> AddDoctor(DoctorDTO doctor); 
    }
}
