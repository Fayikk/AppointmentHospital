using AppointmentHospital.Server.Context;
using AppointmentHospital.Shared;
using AppointmentHospital.Shared.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AppointmentHospital.Server.Services.ForDoctor
{
    public class DoctorService : IDoctorService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public DoctorService(IMapper mapper,DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<DoctorDTO>> AddDoctor(DoctorDTO doctor)
        {
            var result = _mapper.Map<DoctorDTO, Doctor>(doctor);
            var addedObj = _context.Doctors.Add(result);
            var reverseMap = _mapper.Map<Doctor,DoctorDTO>(addedObj.Entity);
            await _context.SaveChangesAsync();
            return new ServiceResponse<DoctorDTO>
            {
                Data = reverseMap,
                Success = true,
                Message = "Added Process Is Successfully",

            };

        }

        public async Task<ServiceResponse<List<Doctor>>> GetAll()
        {
            var result = await _context.Doctors.ToListAsync();
            if (result.Count == 0)
            {
                return new ServiceResponse<List<Doctor>>
                {
                    Message = "Doctors is not found",
                    Success = false,
                };

            }
            return new ServiceResponse<List<Doctor>>
            {
                Data = result,
                Success = true,
            };
        }

        public async Task<ServiceResponse<List<Doctor>>> GetByDoctor(int policlinicId)
        {
            var result = await _context.Doctors.Where(x=>x.PoliclinicId==policlinicId).ToListAsync();
            if (result == null)
            {
                return new ServiceResponse<List<Doctor>>
                {
                    Message = "Doctor is not found"
                ,
                    Success = false
                };
            }
            return new ServiceResponse<List<Doctor>>
            {
                Data=result,
                Success = true,
            };
        }
    }
}
