using AppointmentHospital.Shared;
using Microsoft.EntityFrameworkCore;

namespace AppointmentHospital.Server.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }   
        public DbSet<Meet> Meets { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Policlinic> Policlinics { get; set; }
    }
}
