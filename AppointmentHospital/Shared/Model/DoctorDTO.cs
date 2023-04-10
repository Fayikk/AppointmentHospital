using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentHospital.Shared.Model
{
    public class DoctorDTO
    {
        public int PoliclinicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DoctorTitle { get; set; }
    }
}
