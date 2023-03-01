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
    public DbSet<Office> Offices { get; set; }
    public DbSet<Specialization> Specializations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DoctorStatus>().HasKey(x => x.StatusId);
        modelBuilder.Entity<DoctorStatus>().Property(x => x.Name).HasColumnType("nvarchar(128)");
        modelBuilder.Entity<DoctorStatus>().HasData(new List<DoctorStatus>
        {
            new()
            {
                StatusId = Guid.Parse("e8dfe97a-1c89-45ff-b08c-a97f6edc5e41"),
                Name = "At work"
            },
            new()
            {
                StatusId = Guid.Parse("724eac90-dee0-454e-bdc5-10742042bbdd"),
                Name = "On vacation"
            },
            new()
            {
                StatusId = Guid.Parse("7be6ae9f-1534-416c-b420-c2c191f7b3fc"),
                Name = "Sick Day"
            },
            new()
            {
                StatusId = Guid.Parse("53bc1111-9b82-4e0a-9c79-4132e7d3e472"),
                Name = "Sick Leave"
            },
            new()
            {
                StatusId = Guid.Parse("2270714a-25a4-467b-b07a-3ce15b4fa035"),
                Name = "Self-isolation"
            },
            new()
            {
                StatusId = Guid.Parse("5a2466e3-ad9d-4454-86f6-ae6e63183116"),
                Name = "Leave without pay"
            },
            new()
            {
                StatusId = Guid.Parse("27fddac5-d7cc-41d4-b22d-ff41b98a09d7"),
                Name = "Inactive"
            }
        });


        modelBuilder.Entity<Doctor>()
            .HasKey(x => x.DoctorId);
        
        modelBuilder.Entity<Doctor>()
            .HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(d => d.PersonId);
        
        modelBuilder.Entity<Doctor>()
            .HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(d => d.StatusId);
        
        modelBuilder.Entity<Doctor>()
            .HasOne(x => x.Office)
            .WithMany()
            .HasForeignKey(d => d.OfficeId);
        
        modelBuilder.Entity<Doctor>()
            .HasOne(x => x.Specialization)
            .WithMany()
            .HasForeignKey(d => d.SpecializationId);
        
        modelBuilder.Entity<Doctor>()
            .Property(x => x.Email)
            .HasColumnType("nvarchar(128)");

        
        modelBuilder.Entity<Patient>()
            .HasKey(x => x.PatientId);
        
        modelBuilder.Entity<Patient>()
            .HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(d => d.PersonId);

        
        modelBuilder.Entity<Receptionist>()
            .HasKey(x => x.ReceptionistId);
        modelBuilder.Entity<Receptionist>()
            .Property(x => x.Email)
            .HasColumnType("nvarchar(128)");
        
        modelBuilder.Entity<Receptionist>()
            .HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(d => d.PersonId);
        
        modelBuilder.Entity<Receptionist>()
            .HasOne(x => x.Office)
            .WithMany()
            .HasForeignKey(d => d.OfficeId);

        
        modelBuilder.Entity<Person>()
            .HasKey(x => x.PersonId);
        modelBuilder.Entity<Person>()
            .HasIndex(x => x.UserId)
            .IsUnique();
        
        modelBuilder.Entity<Person>()
            .Property(x => x.FirstName)
            .HasColumnType("nvarchar(128)");
        
        modelBuilder.Entity<Person>()
            .Property(x => x.LastName)
            .HasColumnType("nvarchar(128)");
        
        modelBuilder.Entity<Person>()
            .Property(x => x.MiddleName)
            .HasColumnType("nvarchar(128)");

        
        modelBuilder.Entity<Office>()
            .HasKey(x => x.OfficeId);

        modelBuilder.Entity<Office>()
            .Property(x => x.OfficeAddress)
            .HasColumnType("nvarchar(128)");
        
        
        modelBuilder.Entity<Specialization>()
            .HasKey(x => x.SpecializationId);

        modelBuilder.Entity<Specialization>()
            .Property(x => x.SpecializationName)
            .HasColumnType("nvarchar(128)");
    }
}