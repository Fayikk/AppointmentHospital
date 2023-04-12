using AppointmentHospital.Server.Context;
using AppointmentHospital.Server.Services.ForAuth;
using AppointmentHospital.Shared;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace AppointmentHospital.Server.Services.ForMeet
{
    public class MeetService : IMeetService
    {
        private readonly DataContext _dataContext;
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;
        public MeetService(DataContext context
                          ,IAuthService authService
                          ,IEmailSender emailSender)
        {
            _dataContext = context;
            _authService = authService;
            _emailSender = emailSender;
        }

        public async Task<ServiceResponse<Meet>> CancelMeet(int id)
        {
            var result = await _dataContext.Meets.FirstOrDefaultAsync(x => x.Id == id);
            var response = result.MeetDate - DateTime.UtcNow;
            var hour = response.Hours;
            var day = result.MeetDate.Day - DateTime.UtcNow.Day;
            if (result.Status != false)
            {
                if (hour<6 && day<1)
                {
                    return new ServiceResponse<Meet>
                    {
                        Success = false,
                        Message = "Randevu en az 6 saat kala iptal edilebilir",
                    };
                }
                else
                {
                    result.Status = false;
                    await _dataContext.SaveChangesAsync();
                    return new ServiceResponse<Meet>
                    {
                        Success = true,
                    };
                }
            }
            return null;

        }

        public async Task<ServiceResponse<Meet>> CreateMeet(Meet meet)
        {
            var result = await _dataContext.Meets.Where(x => x.DoctorId == meet.DoctorId).ToListAsync();
            var doctor = await _dataContext.Doctors.FirstOrDefaultAsync(x => x.DoctorId == meet.DoctorId);
            var poly = await _dataContext.Policlinics.FirstOrDefaultAsync(x => x.Id == doctor.PoliclinicId);
            var mDate = await _dataContext.Meets.Where(x=>x.MeetDate.Month==meet.MeetDate.Month).ToListAsync();
            var user = _authService.GetUserId();
            var User = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == user);
            var checkUser = await _dataContext.Meets.Where(x => x.UserId == user).ToListAsync();

            if (meet.MeetTime.Hours==00)
            {
                return new ServiceResponse<Meet>
                {
                    Success = false,
                    Message = "Randevu saati girmeniz zorunludur",
                };
            }
            foreach (var item in checkUser)
            {
                if (item.Status==true)
                {
                    return new ServiceResponse<Meet>
                    {
                        Message = "Zaten aktif bir randevuya sahipsiniz",
                        Success = false,
                    };
                }
            }
            foreach (var item in result)
            {
                if (item.MeetDate==meet.MeetDate && item.MeetTime==meet.MeetTime)
                {
                    return new ServiceResponse<Meet>
                    {
                        Success = false,
                        Message = "Seçili alanda ilgili randevu doludur"
                    };
                }
            }
            if (doctor != null && poly != null)
            {
                meet.DoctorId = doctor.DoctorId;
                meet.UserId = user;
                meet.PoliclinicName = poly.PoliclinicName;
                meet.DoctorName = doctor.Name;
                var addedObj = _dataContext.Meets.Add(meet);
                await _dataContext.SaveChangesAsync();
                await _emailSender.SendEmailAsync(User.Email, "Randevunuz alınmıştır.Randevu saatinde orada olmanız önemlidir.", meet.MeetTime.ToString() + "saatine randevunuz ayarlanmıştır");
                return new ServiceResponse<Meet>
                {
                    Success = true,
                    Data = addedObj.Entity,
                    Message = "Randevunuz başarıyla oluşturulmuştur",
                };
            }

            return new ServiceResponse<Meet>
            {
                Success = false,
            };


        }

        public async Task<ServiceResponse<List<Meet>>> GetMeetById()
        {
            var user = _authService.GetUserId();
            var result = await _dataContext.Meets.Where(x => x.UserId == user).ToListAsync();

            if (result.Count == 0)
            {
                return new ServiceResponse<List<Meet>>
                {
                    Message = "Herhangi bir randevunuz bulunmamaktadır",
                    Success = false,
                };
            }
            var response = new ServiceResponse<List<Meet>>
            {
                Data = result,
                Success = true,
            };
            return response;
        }
    }
}
