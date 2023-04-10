using AppointmentHospital.Shared;
using AppointmentHospital.Shared.Model;
using AutoMapper;

namespace AppointmentHospital.Server.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorDTO>().ReverseMap();
        }
    }
}
