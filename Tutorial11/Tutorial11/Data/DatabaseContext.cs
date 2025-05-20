using Microsoft.EntityFrameworkCore;
using Tutorial5.Models;

namespace Tutorial5.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>().HasData(new List<Doctor>()
        {
            new Doctor(){IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jkowalski@gmail.com"},
            new Doctor(){IdDoctor = 2, FirstName = "Jerzy", LastName = "Weselski", Email = "jerzy@gmail.com"},
        });
        
        modelBuilder.Entity<Patient>().HasData(new List<Patient>()
        {
            new Patient(){IdPatient = 1, FirstName = "Maciej", LastName = "Maciak", Birthdate = new DateTime(1985, 12,25)},
            new Patient(){IdPatient = 2, FirstName = "Jarosław", LastName = "Sykson", Birthdate = new DateTime(1955, 2,12)}

        });
        
        modelBuilder.Entity<Medicament>().HasData(new List<Medicament>()
        {
            new Medicament(){IdMedicament = 1, Name = "Rutinoscorbin", Description = "Doesn't work, but many people like it", Type = "Suplement"},
            new Medicament(){IdMedicament = 2, Name = "Gagarin", Description = "Good for sore throat", Type = "Suplement"},
        });
        
        modelBuilder.Entity<Prescription>().HasData(new List<Prescription>()
        {
            new Prescription(){IdPrescription = 1, Date = DateTime.Today, DueDate = new DateTime(2025, 5,20).Add(TimeSpan.FromDays(14)), IdPatient = 1, IdDoctor = 1},
            new Prescription(){IdPrescription = 2, Date = DateTime.Today, DueDate = new DateTime(2025, 5,20).Add(TimeSpan.FromDays(14)), IdPatient = 2, IdDoctor = 1},
        });
        
        modelBuilder.Entity<PrescriptionMedicament>().HasData(new List<PrescriptionMedicament>()
        {
            new PrescriptionMedicament(){IdMedicament = 1, IdPrescription = 1, Dose = 1, Details = "Be careful with that"},
            new PrescriptionMedicament(){IdMedicament = 2, IdPrescription = 1, Dose = 3, Details = "Take 3 per day"},
            new PrescriptionMedicament(){IdMedicament = 1, IdPrescription = 2, Dose = 1, Details = "Be careful with that"}
        });
    }
}