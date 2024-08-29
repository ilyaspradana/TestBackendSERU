using Microsoft.EntityFrameworkCore;
using TestBackendSERU.Entity;
using static TestBackendSERU.Models.User;

namespace TestBackendSERU.Service
{
    public class VehicleDbContext : DbContext
    {
        public VehicleDbContext(DbContextOptions<VehicleDbContext>
            options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<Vehicle_Brand> Vehicle_Brand { get; set; }
        public DbSet<Vehicle_Type> Vehicle_Type { get; set; }
        public DbSet<Vehicle_Model> Vehicle_Model { get; set; }
        public DbSet<Vehicle_Year> Vehicle_Year { get; set; }
        public DbSet<Pricelist> Pricelist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle_Type>()
            .HasOne(vt => vt.Vehicle_Brand)
            .WithMany(vb => vb.Vehicle_Types)
            .HasForeignKey(vt => vt.Brand_ID)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Vehicle_Model>()
            .HasOne(vt => vt.Vehicle_Type)
            .WithMany(vb => vb.Vehicle_Model)
            .HasForeignKey(vt => vt.Type_ID)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Pricelist>()
            .HasOne(p => p.Vehicle_Model)
            .WithMany(vm => vm.Pricelist)
            .HasForeignKey(p => p.Model_ID)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Pricelist>()
            .HasOne(p => p.Vehicle_Year)
            .WithMany(vy => vy.Pricelist)
            .HasForeignKey(p => p.Year_ID)
            .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}
