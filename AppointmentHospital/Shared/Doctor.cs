using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentHospital.Shared
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }   
        public int PoliclinicId { get; set; }   
        public string Name { get; set; }
        public string Description { get; set; }
        public string DoctorTitle { get; set; }
    }
}
