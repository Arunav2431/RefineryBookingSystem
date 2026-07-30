using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RefineryBooking.Migrations
{
    /// <inheritdoc />
    public partial class SeedCostCentres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                column: "ConcurrencyStamp",
                value: "19f27f62-3499-462d-b7c3-aa1fdc9228ee");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                column: "ConcurrencyStamp",
                value: "99d35478-b66d-4ae6-aed6-d4cace274c16");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                column: "ConcurrencyStamp",
                value: "873cc7a4-aa44-42cf-a428-56b5c037600d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                column: "ConcurrencyStamp",
                value: "77c62798-290e-4360-96e7-77177a84c552");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaa-aaaa-aaaa-aaaa",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6345aeae-9110-4f02-a493-c791c77fcf54", "AQAAAAIAAYagAAAAEOnwOfthAv4+HPIcC01kJpxrbyuwOWDA9lYE7TUb4w6o8h++1GH6giYLrrZdsXVeLA==", "58a9b36d-60b3-4d4c-be46-9508de4c986c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbbb-bbbb-bbbb-bbbb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "596d33c9-9018-4087-bea2-29eea9f70d82", "AQAAAAIAAYagAAAAEHMt275qJ8X0gK7tnbEbQ8XMgbP1bLkhoFW2DD2YhXZTwMxyCQVO5jQIfPiUqT3GxA==", "e8581644-d12c-4fda-b6c4-a61ec4904039" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cccc-cccc-cccc-cccc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "da3b856c-a73d-4365-b3fe-9527be1ba6e2", "AQAAAAIAAYagAAAAEJOiw01Jkux54PKoLN3BlQdrJsyNreppmyR0kHlKpt70JTKrs8FlG8uE5H4QWb1z3A==", "6f2994fc-d926-4fe0-8535-7fe98e8b8c40" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dddd-dddd-dddd-dddd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9e285ac8-32b7-4f3c-8453-fd3cb6a71392", "AQAAAAIAAYagAAAAEM5j2uG7XmjTDaO/XqktRsk7wY/Pt4NfrLzho2Ivm71pngiqonfTn4MyIwjIwwJPUA==", "f5229b01-d634-42a0-bcee-38f77f782fcb" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 30, 18, 26, 17, 747, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 30, 18, 26, 17, 747, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 30, 18, 26, 17, 747, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 30, 18, 26, 17, 747, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 30, 18, 26, 17, 747, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 30, 18, 26, 17, 747, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 30, 18, 26, 17, 747, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 30, 18, 26, 17, 747, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 30, 18, 26, 17, 747, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 30, 18, 26, 17, 747, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.InsertData(
                table: "CostCentres",
                columns: new[] { "Id", "Code", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "1001", "Corporate, executive and administrative operations", true, "Administration & Corporate Affairs" },
                    { 2, "1002", "Plant operations and production management", true, "Operations" },
                    { 3, "1003", "HSE training, compliance and safety audits", true, "Health, Safety & Environment" },
                    { 4, "1004", "Pipeline, civil and mechanical engineering", true, "Engineering" },
                    { 5, "1005", "Equipment maintenance and turnaround planning", true, "Maintenance" },
                    { 6, "1006", "Information technology and instrumentation", true, "IT Services" },
                    { 7, "1007", "Supply chain, dispatch and warehouse management", true, "Logistics" },
                    { 8, "1008", "Financial planning, budgeting and accounting", true, "Finance" },
                    { 9, "1009", "Recruitment, training and employee relations", true, "Human Resources" },
                    { 10, "1010", "Product quality assurance and laboratory services", true, "Quality Control" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CostCentres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CostCentres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CostCentres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CostCentres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CostCentres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CostCentres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CostCentres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CostCentres",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CostCentres",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CostCentres",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                column: "ConcurrencyStamp",
                value: "91191689-6b16-455f-88a7-6768732f0941");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                column: "ConcurrencyStamp",
                value: "e83d89e8-31e1-4e70-bee6-c21e3d193087");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                column: "ConcurrencyStamp",
                value: "69e3b745-b13d-4c02-904f-5e1beb308d93");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                column: "ConcurrencyStamp",
                value: "fa9e3ca2-b87d-4ae0-9b7c-b96cc49d6045");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaa-aaaa-aaaa-aaaa",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3a934e1c-df0c-495a-9230-31a56d0509d3", "AQAAAAIAAYagAAAAEMcpuSowfxSbb+RExakkdgRQqp0SDx602RNFeC7VZ62LBMi5nzrH79bL8z2DJhlgIA==", "191653f5-af1d-454d-9847-6a272150b220" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbbb-bbbb-bbbb-bbbb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c202534-3cdf-4111-8900-7a4dbd0ea4ce", "AQAAAAIAAYagAAAAEF2CsnXCd9vSFoQ33FvTiBpmzo6Q5rLjm+39x4Y2GetqYe8xuH54aKPTQrvNLlSgcw==", "416273a1-8ac4-4725-a8c6-2b8bc0cd1e01" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cccc-cccc-cccc-cccc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "30ca6c1f-fe09-46df-bf9a-1c2bd7818e40", "AQAAAAIAAYagAAAAEMXAxEKIh07K5BT7E4+pZOYKkp/hINLZ8Lbj+si3jBM42Sd6zzf0ixqYIqecci2osQ==", "96e3416c-c7d4-42ab-9bba-35f00d7466a7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dddd-dddd-dddd-dddd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "77cd183b-cc6f-4959-a4b6-9908917ee7f6", "AQAAAAIAAYagAAAAECmOWVUwL+THhRAUMIvZwC064xZgKNM1ea67jZLFvvG3EDWhargV5ZhFjZJ4DWIZ8Q==", "29bbfc9d-e28b-4d3b-bc0c-3867b26b0149" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 16, 18, 52, 14, DateTimeKind.Utc).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 16, 18, 52, 14, DateTimeKind.Utc).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 16, 18, 52, 14, DateTimeKind.Utc).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 16, 18, 52, 14, DateTimeKind.Utc).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 16, 18, 52, 14, DateTimeKind.Utc).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 16, 18, 52, 14, DateTimeKind.Utc).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 16, 18, 52, 14, DateTimeKind.Utc).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 16, 18, 52, 14, DateTimeKind.Utc).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 16, 18, 52, 14, DateTimeKind.Utc).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 16, 18, 52, 14, DateTimeKind.Utc).AddTicks(2929));
        }
    }
}
