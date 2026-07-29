// File: Services/CompanyAuthService.cs
//
// PURPOSE:
//   This service is the bridge between this application and your company's
//   employee directory / HR system.
//
//   When a regular employee logs in with their Employee ID + Password:
//     1. This service validates the credentials against the company server.
//     2. On success, it also RETURNS the employee's profile (Full Name,
//        Department, Email) directly from the company directory.
//     3. The AccountController then auto-provisions the user in the local
//        database with those details on their first login — no manual
//        user creation needed.
//
// HOW TO CONNECT TO THE COMPANY SERVER:
// ─────────────────────────────────────
//
// OPTION A — Windows Active Directory / LDAP  ← Most common in refineries
//   NuGet package: Novell.Directory.Ldap.NETStandard  OR  System.DirectoryServices.Protocols
//   Step 1: Replace the TODO block below with an LDAP bind using the employee's
//           credentials (ldapConnection.Bind(userId + "@nrl.co.in", password)).
//   Step 2: On successful bind, search for the user's CN entry and read:
//             cn           → FullName
//             department   → Department
//             mail         → Email
//             employeeID   → EmployeeId
//   appsettings.json entry needed:
//     "CompanyServer": {
//       "LdapHost": "192.168.1.10",        ← your AD server IP / hostname
//       "LdapPort": 389,
//       "BaseDn":   "DC=nrl,DC=co,DC=in"
//     }
//
// OPTION B — Company HR REST API
//   If your company exposes a REST endpoint to verify employee credentials:
//   Step 1: POST to https://hrportal.nrl.co.in/api/auth/verify
//           Body: { "employeeId": "NRL-1042", "password": "..." }
//   Step 2: On HTTP 200, parse the response for { fullName, department, email }.
//   appsettings.json entry needed:
//     "CompanyServer": {
//       "AuthApiUrl": "https://hrportal.nrl.co.in/api/auth/verify",
//       "ApiKey":     "YOUR_INTERNAL_API_KEY"
//     }
//
// OPTION C — Direct HR Database Query (SQL Server / Oracle)
//   If the company HR database is accessible on the same network:
//   Step 1: Add a second DbContext pointing to the HR database.
//   Step 2: Query the EMPLOYEE table: WHERE EMP_ID = @userId AND PASSWORD = HASH(@password).
//   Step 3: Read FULL_NAME, DEPT_NAME, EMAIL columns.
//   appsettings.json entry needed:
//     "CompanyServer": {
//       "HrDbConnection": "Server=hrdb.nrl.co.in;Database=HRMS;User Id=...;Password=...;"
//     }
//
// ─────────────────────────────────────────────────────────────────────────────

using Microsoft.Extensions.Configuration;

namespace RefineryBooking.Services
{
    /// <summary>
    /// Represents the profile data fetched from the company directory for an employee.
    /// All fields are populated by the company server on successful login.
    /// </summary>
    public class CompanyUserProfile
    {
        /// <summary>Company-issued Employee ID (used as the login username).</summary>
        public string EmployeeId { get; set; } = string.Empty;

        /// <summary>Employee's full name as maintained in the HR/AD directory.</summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>Department name from the HR/AD directory.</summary>
        public string Department { get; set; } = string.Empty;

        /// <summary>Corporate email address (optional, for notifications).</summary>
        public string Email { get; set; } = string.Empty;
    }

    public interface ICompanyAuthService
    {
        /// <summary>
        /// Validates employee credentials against the company server.
        /// On success, returns a <see cref="CompanyUserProfile"/> with the
        /// employee's Full Name, Department, and Email fetched from the directory.
        /// Returns <c>null</c> if credentials are invalid.
        /// </summary>
        Task<CompanyUserProfile?> ValidateAndGetProfileAsync(string employeeId, string password);

        /// <summary>
        /// Looks up an employee's profile from the company directory WITHOUT
        /// authenticating them. Uses a read-only service account to query AD/LDAP.
        /// Returns the employee's Full Name, Department, and Email.
        /// Returns <c>null</c> if the employee is not found or company server
        /// is not yet connected.
        /// 
        /// Used by the Admin panel when creating Admin/ITFM/Allocator accounts
        /// so that Full Name and Department are auto-fetched — the admin only
        /// needs to enter the Windows Username.
        /// </summary>
        Task<CompanyUserProfile?> GetProfileAsync(string employeeId);
    }

    public class CompanyAuthService : ICompanyAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CompanyAuthService> _logger;

        public CompanyAuthService(IConfiguration configuration, ILogger<CompanyAuthService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<CompanyUserProfile?> ValidateAndGetProfileAsync(string employeeId, string password)
        {
            // ─────────────────────────────────────────────────────────────────
            // TODO: Replace with LDAP Bind / REST API / HR DB call.
            // See header comments for Option A, B, C integration guide.
            // ─────────────────────────────────────────────────────────────────
            _logger.LogInformation(
                "CompanyAuthService: Company server not yet configured. " +
                "Employee '{EmployeeId}' cannot be authenticated via company network.",
                employeeId);

            await Task.CompletedTask;
            return null;
        }

        /// <inheritdoc />
        public async Task<CompanyUserProfile?> GetProfileAsync(string employeeId)
        {
            // ─────────────────────────────────────────────────────────────────
            // TODO: Replace with a READ-ONLY LDAP search using a service account.
            //
            // This method is called when an Admin creates an account for a new
            // Admin/ITFM/Allocator. It fetches the person's Full Name and
            // Department from the company directory WITHOUT needing their password.
            //
            // HOW TO IMPLEMENT (LDAP Option A):
            //   1. Bind using a read-only service account (not the employee):
            //        conn.Bind("svc.bookingsystem@nrl.co.in", "ServiceAccountPassword");
            //      Add these to appsettings.json:
            //        "CompanyServer": {
            //          "LdapHost":           "192.168.1.10",
            //          "LdapPort":           389,
            //          "BaseDn":             "DC=nrl,DC=co,DC=in",
            //          "ServiceAccountUser": "svc.bookingsystem@nrl.co.in",
            //          "ServiceAccountPass": "ServiceAccountPassword"
            //        }
            //   2. Search for the employee by sAMAccountName (= their Windows username):
            //        var results = conn.Search(baseDn, LdapConnection.ScopeOne,
            //            $"(sAMAccountName={employeeId})", null, false);
            //        var entry = results.Next();
            //   3. Read and return their attributes:
            //        return new CompanyUserProfile {
            //            EmployeeId = employeeId,
            //            FullName   = entry.GetAttribute("cn").StringValue,
            //            Department = entry.GetAttribute("department").StringValue,
            //            Email      = entry.GetAttribute("mail").StringValue
            //        };
            //
            // HOW TO IMPLEMENT (REST API Option B):
            //   GET https://hrportal.nrl.co.in/api/employees/{employeeId}
            //   Headers: X-Api-Key: YOUR_KEY
            //   Parse response for fullName, department, email.
            // ─────────────────────────────────────────────────────────────────
            _logger.LogInformation(
                "CompanyAuthService: Profile lookup not yet configured. " +
                "Full Name and Department for '{EmployeeId}' will use placeholder.",
                employeeId);

            await Task.CompletedTask;
            return null; // Remove this when integration is implemented
        }
    }
}
