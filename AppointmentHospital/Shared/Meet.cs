using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentHospital.Shared
{
    public class Meet
    {
        [Key]
        public int Id { get; set; } 
        public int DoctorId { get; set; }   
        public int? UserId { get; set; }
        public DateTime CreatedMeet { get; set; } = DateTime.UtcNow;
        public string? DoctorName { get; set; }
        public string? PoliclinicName { get; set; }
        public DateTime MeetDate { get; set; }
        [Required(ErrorMessage ="Please select your appointment time")]
        public TimeSpan MeetTime { get; set; }
        public bool Status { get; set; } = true;
    }
}
