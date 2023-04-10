using AppointmentHospital.Server.Context;
using AppointmentHospital.Shared;
using Microsoft.EntityFrameworkCore;

namespace AppointmentHospital.Server.Services.ForPoliclinic
{
    public class PoliclinicService : IPoliclinicService
    {
        private readonly DataContext _context;
        public PoliclinicService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Policlinic>> AddPoly(Policlinic policlinic)
        {
            var result = await _context.Policlinics.
                FirstOrDefaultAsync(x=>x.PoliclinicName.ToLower().
            Equals(policlinic.PoliclinicName.ToLower()));

            if (result == null)
            {
                _context.Policlinics.Add(policlinic);
                await _context.SaveChangesAsync();
                return new ServiceResponse<Policlinic>
                {
                    Data = policlinic,
                    Message = "Success",
                    Success = true,
                };
            }
            return new ServiceResponse<Policlinic>
            {
                Success = false,
                Message = "Policlinic name already exist",
            };
        
        }

        public async Task<ServiceResponse<int>> DeletePoly(int poliId)
        {
            var result = await _context.Policlinics.FindAsync(poliId);
            if (result == null)
            {
                return new ServiceResponse<int>
                {
                    Message = "Policlinic is nor found",
                    Success = false,
                };
            }
            _context.Policlinics.Remove(result);
            _context.SaveChangesAsync();
            return new ServiceResponse<int>
            {
                Data = result.Id,
                Message = "Deleted is success",
                Success = true,
            };
        }

        public async Task<ServiceResponse<List<Policlinic>>> GetAll()
        {
            var result = await _context.Policlinics.ToListAsync();
            var response = new ServiceResponse<List<Policlinic>>();
            if (result.Count == 0)
            {
                return new ServiceResponse<List<Policlinic>>
                {
                    Success = false,
                    Message = "Policlinic is not found",
                };
            }
            return new ServiceResponse<List<Policlinic>>
            {
                Data = result,
                Success = true,
            };
        }

        public async Task<ServiceResponse<Policlinic>> GetById(int policid)
        {
            var result = await _context.Policlinics.FirstOrDefaultAsync(x => x.Id == policid);
            if (result == null)
            {
                return new ServiceResponse<Policlinic>
                {
                    Success = false,
                    Message = "Fail",
                };
            }
            return new ServiceResponse<Policlinic>
            {
                Data = result,
                Message = "Success",
                Success = true,
            };
        }

        public async Task<ServiceResponse<Policlinic>> UpdatePoly(Policlinic policlinic)
        {
            var result = await _context.Policlinics.FindAsync(policlinic.Id);
            if (result  == null)
            {
                return new ServiceResponse<Policlinic>
                {
                    Message = "policlinic is not found",
                    Success = false,
                };
            }
            result.PoliclinicName = policlinic.PoliclinicName;
            await _context.SaveChangesAsync();
            return new ServiceResponse<Policlinic>
            {
                Data = result,
                Message = "Updated",
                Success = true,
            };
        }
    }
}
