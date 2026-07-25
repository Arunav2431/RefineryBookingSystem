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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Enforce Index for fast conflict-check queries on Bookings
            builder.Entity<Booking>()
                .HasIndex(b => new { b.ConferenceRoomId, b.StartTime, b.EndTime, b.Status });

            // 1-to-1 relationship between Booking and IT Facility Requirement
            builder.Entity<Booking>()
                .HasOne(b => b.ITRequirement)
                .WithOne(i => i.Booking)
                .HasForeignKey<ITFacilityRequirement>(i => i.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

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
                UserName = "admin@refinery.com",
                NormalizedUserName = "ADMIN@REFINERY.COM",
                Email = "admin@refinery.com",
                NormalizedEmail = "ADMIN@REFINERY.COM",
                EmailConfirmed = true,
                FullName = "System Administrator",
                Department = "IT Operations",
                EmployeeBadgeId = "REF-001"
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Refinery2026!");

            var allocatorUser = new ApplicationUser
            {
                Id = "bbbb-bbbb-bbbb-bbbb",
                UserName = "allocator@refinery.com",
                NormalizedUserName = "ALLOCATOR@REFINERY.COM",
                Email = "allocator@refinery.com",
                NormalizedEmail = "ALLOCATOR@REFINERY.COM",
                EmailConfirmed = true,
                FullName = "Sarah Jenkins (Logistics)",
                Department = "Facility Scheduling",
                EmployeeBadgeId = "REF-042"
            };
            allocatorUser.PasswordHash = hasher.HashPassword(allocatorUser, "Refinery2026!");

            var itfmUser = new ApplicationUser
            {
                Id = "cccc-cccc-cccc-cccc",
                UserName = "itfm@refinery.com",
                NormalizedUserName = "ITFM@REFINERY.COM",
                Email = "itfm@refinery.com",
                NormalizedEmail = "ITFM@REFINERY.COM",
                EmailConfirmed = true,
                FullName = "Marcus Vance (AV Lead)",
                Department = "IT & Facilities",
                EmployeeBadgeId = "REF-108"
            };
            itfmUser.PasswordHash = hasher.HashPassword(itfmUser, "Refinery2026!");

            var generalUser = new ApplicationUser
            {
                Id = "dddd-dddd-dddd-dddd",
                UserName = "user@refinery.com",
                NormalizedUserName = "USER@REFINERY.COM",
                Email = "user@refinery.com",
                NormalizedEmail = "USER@REFINERY.COM",
                EmailConfirmed = true,
                FullName = "Dave Miller (Engineer)",
                Department = "Catalytic Cracking Unit",
                EmployeeBadgeId = "REF-504"
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
            builder.Entity<ConferenceRoom>().HasData(
                new ConferenceRoom { Id = 1, Name = "North Gate Boardroom", BuildingLocation = "Admin Block A, Floor 3", Capacity = 24, HasVideoConferencing = true, HasProjector = true, HasWhiteboard = true, IsActive = true },
                new ConferenceRoom { Id = 2, Name = "Catalytic Cracker Briefing Room", BuildingLocation = "Plant 2 Operations Center", Capacity = 12, HasVideoConferencing = false, HasProjector = true, HasWhiteboard = true, IsActive = true },
                new ConferenceRoom { Id = 3, Name = "Safety & HAZMAT Training Hall", BuildingLocation = "Visitor Center, Ground Floor", Capacity = 60, HasVideoConferencing = true, HasProjector = true, HasWhiteboard = true, IsActive = true },
                new ConferenceRoom { Id = 4, Name = "Pipeline Engineering Hub", BuildingLocation = "Technical Services Bldg", Capacity = 8, HasVideoConferencing = true, HasProjector = false, HasWhiteboard = true, IsActive = true }
            );
        }
    }
}