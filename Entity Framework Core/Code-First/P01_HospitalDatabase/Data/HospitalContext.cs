namespace P01_HospitalDatabase.Data
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using P01_HospitalDatabase.Data.Models;

    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelBuilderPatient(modelBuilder);
            ModelBuilderVisitation(modelBuilder);
            ModelBuilderDiagnose(modelBuilder);
            ModelBuilderMedicament(modelBuilder);
            ModelBuilderPatientMedicament(modelBuilder);
        }

        private void ModelBuilderPatientMedicament(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PatientMedicament>()
                .HasKey(k => new { k.MedicamentId, k.PatientId });

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(p => p.Patient)
                .WithMany(pm => pm.Prescriptions)
                .HasForeignKey(k => k.PatientId);

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(p => p.Medicament)
                .WithMany(pm => pm.Prescriptions)
                .HasForeignKey(k => k.MedicamentId);
        }

        private void ModelBuilderMedicament(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Medicament>()
                .HasKey(k => k.MedicamentId);

            modelBuilder
                .Entity<Medicament>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Medicament>()
                .HasMany(p => p.Prescriptions)
                .WithOne(m => m.Medicament)
                .HasForeignKey(k => k.MedicamentId);
        }

        private void ModelBuilderDiagnose(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Diagnose>()
                .HasKey(k => k.DiagnoseId);

            modelBuilder
                .Entity<Diagnose>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
               .Entity<Diagnose>()
               .Property(p => p.Comments)
               .HasMaxLength(250)
               .IsUnicode();

            modelBuilder
                .Entity<Diagnose>()
                .HasOne(p => p.Patient)
                .WithMany(d => d.Diagnoses)
                .HasForeignKey(k => k.PatientId);
        }

        private void ModelBuilderVisitation(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<Visitation>()
               .HasKey(v => v.VisitationId);

            modelBuilder
                .Entity<Visitation>()
                .HasOne(p => p.Patient)
                .WithMany(v => v.Visitations)
                .HasForeignKey(k => k.PatientId);

            modelBuilder
                .Entity<Visitation>()
                .Property(v => v.Comments)
                .HasMaxLength(250)
                .IsUnicode();
        }

        private void ModelBuilderPatient(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Patient>()
                .HasKey(p => p.PatientId);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Visitations)
                .WithOne(v => v.Patient);
            
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Prescriptions)
                .WithOne(p => p.Patient);
            
            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Diagnoses)
                .WithOne(p => p.Patient);

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.LastName)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.Address)
                .HasMaxLength(250)
                .IsUnicode();

            modelBuilder
                .Entity<Patient>()
                .Property(p => p.Email)
                .HasMaxLength(80);
        }
    }
}
