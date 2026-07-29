using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefineryBooking.Migrations
{
    /// <inheritdoc />
    public partial class UseWindowsUsernameLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                column: "ConcurrencyStamp",
                value: "1c1a818f-b5b2-4448-87fc-77d3b1c05d8d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                column: "ConcurrencyStamp",
                value: "1ca015da-0b67-4182-b247-57e042e6307f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                column: "ConcurrencyStamp",
                value: "9c717780-97c8-40ef-b10d-a3fb86e32150");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                column: "ConcurrencyStamp",
                value: "0b618b5b-97c8-4b65-82b4-32052adc01dd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaa-aaaa-aaaa-aaaa",
                columns: new[] { "ConcurrencyStamp", "Email", "EmployeeBadgeId", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "324d44e9-d378-4f8b-8912-6e342b2c2c35", "admin@nrl.co.in", "NRL-ADM-001", "ADMIN@NRL.CO.IN", "SYS.ADMIN", "AQAAAAIAAYagAAAAEKXf143+0bWUjo3wVUorKGMRj0MWZWubZs1AEw2Sk5MvhPZxOy9mPFHSuXSektIg3Q==", "77244867-1bc2-4535-a999-9000c4964a23", "sys.admin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbbb-bbbb-bbbb-bbbb",
                columns: new[] { "ConcurrencyStamp", "Email", "EmployeeBadgeId", "FullName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "45228da3-d73d-4de5-b9fb-48f41ec2d056", "sarah.jenkins@nrl.co.in", "NRL-042", "Sarah Jenkins", "SARAH.JENKINS@NRL.CO.IN", "SARAH.JENKINS", "AQAAAAIAAYagAAAAEE+8cZuz8J2LqmjBbNYp3jpdqPvR9YfJROO2XWWWPGGzXhnQ+2y/O2gHaUuUJ8vm8Q==", "31b0ed54-116d-4e27-a85a-8e53ecc0d807", "sarah.jenkins" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cccc-cccc-cccc-cccc",
                columns: new[] { "ConcurrencyStamp", "Email", "EmployeeBadgeId", "FullName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "14c46eb9-37dc-4fbc-b21c-a560cc317764", "marcus.vance@nrl.co.in", "NRL-108", "Marcus Vance", "MARCUS.VANCE@NRL.CO.IN", "MARCUS.VANCE", "AQAAAAIAAYagAAAAEGq12tyK2v0Ri8/0J2R0V6j+eA5C2le8SU9e+9iHRQCcEl0jJL16VyrxoKuUPvstMw==", "6477d2bd-191c-4fa4-926d-d3507fe4845a", "marcus.vance" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dddd-dddd-dddd-dddd",
                columns: new[] { "ConcurrencyStamp", "Email", "EmployeeBadgeId", "FullName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "130b75a4-4088-493a-b764-1aa80ab3fdc8", "dave.miller@nrl.co.in", "NRL-504", "Dave Miller", "DAVE.MILLER@NRL.CO.IN", "DAVE.MILLER", "AQAAAAIAAYagAAAAEMOR9UvbC/DEOzWDYs19EqzEfGUmQ7LTEgYwQQcGVIMz4HK8SZNcX1Y7DvDiUYo11Q==", "3e2c93ff-c1f0-4885-8b22-e4e5b31bea8e", "dave.miller" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                column: "ConcurrencyStamp",
                value: "507ad72e-38ab-4e96-8c6d-bd8f177ecd9e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                column: "ConcurrencyStamp",
                value: "7e98e2db-5017-4a81-8661-34e0f286ceb3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                column: "ConcurrencyStamp",
                value: "42a7499e-8930-4109-972d-7f756a4d12f6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                column: "ConcurrencyStamp",
                value: "d2ba4f7c-da70-4ef0-8d87-58ee2c941b0b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaa-aaaa-aaaa-aaaa",
                columns: new[] { "ConcurrencyStamp", "Email", "EmployeeBadgeId", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "ada25aa6-7586-4e3f-9a98-e73b9b8c4eef", "admin@refinery.com", "REF-001", "ADMIN@REFINERY.COM", "ADMIN@REFINERY.COM", "AQAAAAIAAYagAAAAECHRN9GLjRfYEDJq2iMiTkjXfByLRwWj1+4rvW6vCCEpYd0hvwSNIv2/4aPQp1yRGQ==", "9f82dbd4-1f83-438b-a711-27b073327da4", "admin@refinery.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbbb-bbbb-bbbb-bbbb",
                columns: new[] { "ConcurrencyStamp", "Email", "EmployeeBadgeId", "FullName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "8b2a8b05-cfcc-4bdf-bc4a-d779e989322f", "allocator@refinery.com", "REF-042", "Sarah Jenkins (Logistics)", "ALLOCATOR@REFINERY.COM", "ALLOCATOR@REFINERY.COM", "AQAAAAIAAYagAAAAEDI79FJWG/9W7L8uDh7Lzy5E0pcGB20biRmSm+OuB9zPn0HG31f1tev9ZYlcDfkUwQ==", "93ffe227-7197-4327-b03f-f97d6c3d2bb7", "allocator@refinery.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cccc-cccc-cccc-cccc",
                columns: new[] { "ConcurrencyStamp", "Email", "EmployeeBadgeId", "FullName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "899200b2-8309-44ca-84ff-d28361aceea9", "itfm@refinery.com", "REF-108", "Marcus Vance (AV Lead)", "ITFM@REFINERY.COM", "ITFM@REFINERY.COM", "AQAAAAIAAYagAAAAEE823OzFcmRYXdp3f2EoQq0MbSo1Yh45uPkBOAptxT0uubbUUrenBKMLNQWVVORKlQ==", "ebdd7df6-a6eb-4f2b-8b25-e09dd4b87079", "itfm@refinery.com" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dddd-dddd-dddd-dddd",
                columns: new[] { "ConcurrencyStamp", "Email", "EmployeeBadgeId", "FullName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "47dc5a0c-a8d2-426c-8369-78203e99a7c4", "user@refinery.com", "REF-504", "Dave Miller (Engineer)", "USER@REFINERY.COM", "USER@REFINERY.COM", "AQAAAAIAAYagAAAAEFMcBg8VILmoe2xr6MxDWx7bpwlhKa9yUSChFl1G7GwO3FL2wGQHlUwYwM7NflJ1Zg==", "ea17e766-7bf5-4224-bf66-d0221bb8e319", "user@refinery.com" });
        }
    }
}
