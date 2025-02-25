using LabelPad.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.Data
{
    public class LabelPadDbContext : DbContext
    {
        public LabelPadDbContext(DbContextOptions<LabelPadDbContext> options)
          : base(options)
        {
        }

        public DbSet<RegisterUser> RegisterUsers { get; set; }
        public DbSet<DomainName> DomainNames { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DbSet<Module> Modules { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<RoleScreen> RoleScreens { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<VehicleRegistration> VehicleRegistrations { get; set; }

        public DbSet<VehicleRegistrationTimeSlot> VehicleRegistrationTimeSlots { get; set; }
        public DbSet<Support> Supports { get; set; }

        public DbSet<ParkingBay> ParkingBays { get; set; }
        public DbSet<ParkingBayNo> ParkingBayNos { get; set; }
        public DbSet<BayConfig> BayConfigs { get; set; }
        public DbSet<VisitorBay> VisitorBays { get; set; }
        public DbSet<VisitorBayNo> VisitorBayNos { get; set; }
        public DbSet<VisitorParking> VisitorParkings { get; set; }
        public DbSet<VisitorParkingVehicleDetails> VisitorParkingVehicleDetails { get; set; }
        public DbSet<VisitorBaySession> VisitorBaySessions { get; set; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<DataType> DataTypes { get; set; }
        public DbSet<AnnotationType> AnnotationTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectAnnotationType> ProjectAnnotationTypes { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamUser> TeamUsers { get; set; }
        public DbSet<LabelClass> LabelClasses { get; set; }

        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<DatasetFiles> DatasetFiles { get; set; }
        public DbSet<VisitorParkingTemp> VisitorParkingTemps { get; set; }
        public DbSet<AuditLogs> AuditLogs { get; set; }
        public DbSet<Ping> Pings { get; set; }
        public DbSet<SoftwareVersion> SoftwareVersions { get; set; }
        public DbSet<OperatorDetails> OperatorDetail { get; set; }
        public DbSet<Industry> Industries { get; set; }

    }
}
