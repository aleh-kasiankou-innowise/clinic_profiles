using Innowise.Clinic.Profiles.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Innowise.Clinic.Profiles.Persistence;

public class ProfilesDbContext : DbContext
{
    public ProfilesDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Receptionist> Receptionists { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<DoctorStatus> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoctorStatus>().HasKey(x => x.StatusId);
        modelBuilder.Entity<DoctorStatus>().Property(x => x.Name).HasColumnType("nvarchar(128)");
        modelBuilder.Entity<DoctorStatus>().Property(x => x.Description).HasColumnType("nvarchar(128)");

        
        modelBuilder.Entity<Doctor>().HasKey(x => x.DoctorId);
        modelBuilder.Entity<Doctor>().HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(d => d.PersonId);
        modelBuilder.Entity<Doctor>().HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(d => d.StatusId);
        modelBuilder.Entity<Doctor>().Property(x => x.Email).HasColumnType("nvarchar(128)");

        modelBuilder.Entity<Patient>().HasKey(x => x.PatientId);
        modelBuilder.Entity<Patient>().HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(d => d.PersonId);

        modelBuilder.Entity<Receptionist>().HasKey(x => x.ReceptionistId);
        modelBuilder.Entity<Receptionist>().Property(x => x.Email).HasColumnType("nvarchar(128)");
        modelBuilder.Entity<Receptionist>().HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(d => d.PersonId);

        modelBuilder.Entity<Person>().HasKey(x => x.PersonId);
        modelBuilder.Entity<Person>().HasIndex(x => x.UserId).IsUnique();
        modelBuilder.Entity<Person>().Property(x => x.FirstName).HasColumnType("nvarchar(128)");
        modelBuilder.Entity<Person>().Property(x => x.LastName).HasColumnType("nvarchar(128)");
        modelBuilder.Entity<Person>().Property(x => x.MiddleName).HasColumnType("nvarchar(128)");
    }
}