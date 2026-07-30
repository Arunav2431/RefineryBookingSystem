// File: Data/ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RefineryBooking.Models;

namespace RefineryBooking.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ignores the dynamic password hash warning during database updates
            optionsBuilder.ConfigureWarnings(warnings => 
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
                
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<ConferenceRoom> ConferenceRooms { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<ITFacilityRequirement> ITFacilityRequirements { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<HallBlock> HallBlocks { get; set; } = null!;
        public DbSet<AllocatorHallAssignment> AllocatorHallAssignments { get; set; } = null!;
        public DbSet<CostCentre> CostCentres { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Enforce Index for fast conflict-check queries on Bookings
            builder.Entity<Booking>()
                .HasIndex(b => new { b.ConferenceRoomId, b.StartTime, b.EndTime, b.Status });

            // Ensure HallCode is unique
            builder.Entity<ConferenceRoom>()
                .HasIndex(r => r.HallCode)
                .IsUnique();

            builder.Entity<CostCentre>()
                .HasIndex(c => c.Code)
                .IsUnique();

            // 1-to-1 relationship between Booking and IT Facility Requirement
            builder.Entity<Booking>()
                .HasOne(b => b.ITRequirement)
                .WithOne(i => i.Booking)
                .HasForeignKey<ITFacilityRequirement>(i => i.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // HallBlock â†’ ConferenceRoom
            builder.Entity<HallBlock>()
                .HasOne(h => h.ConferenceRoom)
                .WithMany()
                .HasForeignKey(h => h.ConferenceRoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // HallBlock â†’ CreatedBy (no cascade to avoid multiple cascade paths)
            builder.Entity<HallBlock>()
                .HasOne(h => h.CreatedBy)
                .WithMany()
                .HasForeignKey(h => h.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // AllocatorHallAssignment â†’ Allocator (no cascade)
            builder.Entity<AllocatorHallAssignment>()
                .HasOne(a => a.Allocator)
                .WithMany()
                .HasForeignKey(a => a.AllocatorUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // AllocatorHallAssignment â†’ ConferenceRoom
            builder.Entity<AllocatorHallAssignment>()
                .HasOne(a => a.ConferenceRoom)
                .WithMany()
                .HasForeignKey(a => a.ConferenceRoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique: one allocator cannot be assigned to the same hall twice
            builder.Entity<AllocatorHallAssignment>()
                .HasIndex(a => new { a.AllocatorUserId, a.ConferenceRoomId })
                .IsUnique();

            // --- SEED ROLES ---
            var adminRoleId = "11111111-1111-1111-1111-111111111111";
            var allocatorRoleId = "22222222-2222-2222-2222-222222222222";
            var itfmRoleId = "33333333-3333-3333-3333-333333333333";
            var userRoleId = "44444444-4444-4444-4444-444444444444";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = allocatorRoleId, Name = "Allocator", NormalizedName = "ALLOCATOR" },
                new IdentityRole { Id = itfmRoleId, Name = "ITFM", NormalizedName = "ITFM" },
                new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" }
            );

            // --- SEED USERS (Password for all: "Refinery2026!") ---
            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = "aaaa-aaaa-aaaa-aaaa",
                UserName = "sys.admin",
                NormalizedUserName = "SYS.ADMIN",
                Email = "admin@nrl.co.in",
                NormalizedEmail = "ADMIN@NRL.CO.IN",
                EmailConfirmed = true,
                FullName = "System Administrator",
                Department = "IT Operations",
                EmployeeBadgeId = "NRL-ADM-001"
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Refinery2026!");

            var allocatorUser = new ApplicationUser
            {
                Id = "bbbb-bbbb-bbbb-bbbb",
                UserName = "sarah.jenkins",
                NormalizedUserName = "SARAH.JENKINS",
                Email = "sarah.jenkins@nrl.co.in",
                NormalizedEmail = "SARAH.JENKINS@NRL.CO.IN",
                EmailConfirmed = true,
                FullName = "Sarah Jenkins",
                Department = "Facility Scheduling",
                EmployeeBadgeId = "NRL-042"
            };
            allocatorUser.PasswordHash = hasher.HashPassword(allocatorUser, "Refinery2026!");

            var itfmUser = new ApplicationUser
            {
                Id = "cccc-cccc-cccc-cccc",
                UserName = "marcus.vance",
                NormalizedUserName = "MARCUS.VANCE",
                Email = "marcus.vance@nrl.co.in",
                NormalizedEmail = "MARCUS.VANCE@NRL.CO.IN",
                EmailConfirmed = true,
                FullName = "Marcus Vance",
                Department = "IT & Facilities",
                EmployeeBadgeId = "NRL-108"
            };
            itfmUser.PasswordHash = hasher.HashPassword(itfmUser, "Refinery2026!");

            // Default general user â€” regular employees are auto-provisioned on first
            // login via CompanyAuthService (their name & dept come from the company server).
            var generalUser = new ApplicationUser
            {
                Id = "dddd-dddd-dddd-dddd",
                UserName = "dave.miller",
                NormalizedUserName = "DAVE.MILLER",
                Email = "dave.miller@nrl.co.in",
                NormalizedEmail = "DAVE.MILLER@NRL.CO.IN",
                EmailConfirmed = true,
                FullName = "Dave Miller",
                Department = "Catalytic Cracking Unit",
                EmployeeBadgeId = "NRL-504"
            };
            generalUser.PasswordHash = hasher.HashPassword(generalUser, "Refinery2026!");

            builder.Entity<ApplicationUser>().HasData(adminUser, allocatorUser, itfmUser, generalUser);

            // --- ASSIGN USERS TO ROLES ---
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = adminRoleId, UserId = adminUser.Id },
                new IdentityUserRole<string> { RoleId = allocatorRoleId, UserId = allocatorUser.Id },
                new IdentityUserRole<string> { RoleId = itfmRoleId, UserId = itfmUser.Id },
                new IdentityUserRole<string> { RoleId = userRoleId, UserId = generalUser.Id }
            );

            // --- SEED CONFERENCE ROOMS ---
            var sysTime = DateTime.UtcNow;
            var adminId = "aaaa-aaaa-aaaa-aaaa";

            builder.Entity<ConferenceRoom>().HasData(
                new ConferenceRoom { Id = 1, HallCode = "CC-1001-ADM-01", OwnerDepartment = "Administration", CostCentreCode = "1001", Name = "North Gate Boardroom",                BuildingLocation = "Admin Block A",          FloorNumber = "3", Capacity = 24, HasVideoConferencing = true,  HasProjector = true,  HasWhiteboard = true,  IsActive = true, CreatedAt = sysTime, CreatedByUserId = adminId },
                new ConferenceRoom { Id = 2, HallCode = "CC-1002-OPS-01", OwnerDepartment = "Operations",     CostCentreCode = "1002", Name = "Catalytic Cracker Briefing Room",    BuildingLocation = "Plant 2 Operations Center",FloorNumber = "G", Capacity = 12, HasVideoConferencing = false, HasProjector = true,  HasWhiteboard = true,  IsActive = true, CreatedAt = sysTime, CreatedByUserId = adminId },
                new ConferenceRoom { Id = 3, HallCode = "CC-1003-HSE-01", OwnerDepartment = "HSE",            CostCentreCode = "1003", Name = "Safety & HAZMAT Training Hall",       BuildingLocation = "Visitor Center",         FloorNumber = "G", Capacity = 60, HasVideoConferencing = true,  HasProjector = true,  HasWhiteboard = true,  IsActive = true, CreatedAt = sysTime, CreatedByUserId = adminId },
                new ConferenceRoom { Id = 4, HallCode = "CC-1004-ENG-01", OwnerDepartment = "Engineering",    CostCentreCode = "1004", Name = "Pipeline Engineering Hub",            BuildingLocation = "Technical Services Bldg",FloorNumber = "1", Capacity = 8,  HasVideoConferencing = true,  HasProjector = false, HasWhiteboard = true,  IsActive = true, CreatedAt = sysTime, CreatedByUserId = adminId },
                new ConferenceRoom { Id = 5, HallCode = "CC-1001-EXC-01", OwnerDepartment = "Executive",      CostCentreCode = "1001", Name = "Executive Strategy Suite",            BuildingLocation = "HQ Tower",               FloorNumber = "5", Capacity = 16, HasVideoConferencing = true,  HasProjector = true,  HasWhiteboard = true,  IsActive = true, CreatedAt = sysTime, CreatedByUserId = adminId },
                new ConferenceRoom { Id = 6, HallCode = "CC-1002-OPS-02", OwnerDepartment = "Operations",     CostCentreCode = "1002", Name = "Refinery Operations War Room",        BuildingLocation = "Central Control Building",FloorNumber = "2", Capacity = 30, HasVideoConferencing = true,  HasProjector = true,  HasWhiteboard = true,  IsActive = true, CreatedAt = sysTime, CreatedByUserId = adminId },
                new ConferenceRoom { Id = 7, HallCode = "CC-1003-HSE-02", OwnerDepartment = "HSE",            CostCentreCode = "1003", Name = "HSE Training Auditorium",             BuildingLocation = "Safety Block",           FloorNumber = "G", Capacity = 120,HasVideoConferencing = true,  HasProjector = true,  HasWhiteboard = false, IsActive = true, CreatedAt = sysTime, CreatedByUserId = adminId },
                new ConferenceRoom { Id = 8, HallCode = "CC-1005-MNT-01", OwnerDepartment = "Maintenance",    CostCentreCode = "1005", Name = "Turnaround Planning Room",            BuildingLocation = "Maintenance Bldg",       FloorNumber = "1", Capacity = 20, HasVideoConferencing = false, HasProjector = true,  HasWhiteboard = true,  IsActive = true, CreatedAt = sysTime, CreatedByUserId = adminId },
                new ConferenceRoom { Id = 9, HallCode = "CC-1006-ITS-01", OwnerDepartment = "IT Services",    CostCentreCode = "1006", Name = "IT & Instrumentation Lab",            BuildingLocation = "IT Services Block",      FloorNumber = "G", Capacity = 10, HasVideoConferencing = true,  HasProjector = false, HasWhiteboard = true,  IsActive = true, CreatedAt = sysTime, CreatedByUserId = adminId },
                new ConferenceRoom { Id = 10,HallCode = "CC-1007-LOG-01", OwnerDepartment = "Logistics",      CostCentreCode = "1007", Name = "Logistics & Dispatch Conference Room",BuildingLocation = "Warehouse Block B",      FloorNumber = "2", Capacity = 14, HasVideoConferencing = false, HasProjector = true,  HasWhiteboard = true,  IsActive = true, CreatedAt = sysTime, CreatedByUserId = adminId }
            );

            // --- SEED COST CENTRES ---
            builder.Entity<CostCentre>().HasData(
                new CostCentre { Id = 1, Code = "1001", Name = "Administration & Corporate Affairs", Description = "Corporate, executive and administrative operations",           IsActive = true },
                new CostCentre { Id = 2, Code = "1002", Name = "Operations",                         Description = "Plant operations and production management",                  IsActive = true },
                new CostCentre { Id = 3, Code = "1003", Name = "Health, Safety & Environment",       Description = "HSE training, compliance and safety audits",                IsActive = true },
                new CostCentre { Id = 4, Code = "1004", Name = "Engineering",                        Description = "Pipeline, civil and mechanical engineering",                 IsActive = true },
                new CostCentre { Id = 5, Code = "1005", Name = "Maintenance",                        Description = "Equipment maintenance and turnaround planning",              IsActive = true },
                new CostCentre { Id = 6, Code = "1006", Name = "IT Services",                        Description = "Information technology and instrumentation",                 IsActive = true },
                new CostCentre { Id = 7, Code = "1007", Name = "Logistics",                          Description = "Supply chain, dispatch and warehouse management",            IsActive = true },
                new CostCentre { Id = 8, Code = "1008", Name = "Finance",                            Description = "Financial planning, budgeting and accounting",               IsActive = true },
                new CostCentre { Id = 9, Code = "1009", Name = "Human Resources",                   Description = "Recruitment, training and employee relations",               IsActive = true },
                new CostCentre { Id = 10, Code = "1010", Name = "Quality Control",                  Description = "Product quality assurance and laboratory services",           IsActive = true }
            );
        }
    }
}
