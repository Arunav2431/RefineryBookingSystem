using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefineryBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddCostCentreManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostCentres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCentres", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CostCentres_Code",
                table: "CostCentres",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostCentres");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                column: "ConcurrencyStamp",
                value: "7c520ca2-bf3b-48e2-b377-deb5513cd8b2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                column: "ConcurrencyStamp",
                value: "bed62dd4-025b-4426-a47d-9eb8500a4674");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                column: "ConcurrencyStamp",
                value: "a8d5508a-ffef-4077-8e8c-56652143305c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                column: "ConcurrencyStamp",
                value: "9b6ad91d-bcfc-4564-a1aa-5721c45d04d0");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaa-aaaa-aaaa-aaaa",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c17bb685-233d-4ce9-90af-0f3af79521cd", "AQAAAAIAAYagAAAAECXJAw0fN61XnFayTddXmh8AjsTHT3BF9EUbjDsT77PsasKB/owxtvNIr2PRX6BaHg==", "0a5c6ce9-762a-43f7-82a7-f21b6911bbce" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbbb-bbbb-bbbb-bbbb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "caae781f-8705-428a-b340-abc5f7a30c31", "AQAAAAIAAYagAAAAENNmEo4XUOdoNWz3GWcZNpH2gPXKheqZoz3SjQ11YvHCdXxhn7fdN2x3db4D6oeFDA==", "8ee4a23a-935d-48b8-b6e4-51025b2b3b89" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cccc-cccc-cccc-cccc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "47118480-547f-465c-9560-cb1862e7e4d8", "AQAAAAIAAYagAAAAEJqvsVpBD0WqpfQ3pWQUOG9TNhZ8EAJ4nuNNgj8IYitmAsKn0nCucrjzp/p6kbwQDA==", "0565ff60-9721-48d8-ab75-ea0700a8fe17" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dddd-dddd-dddd-dddd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8943a07-9ffe-418c-b77e-0b65edd05a4e", "AQAAAAIAAYagAAAAENXiBKNr4amErfvpQXCYiFwX+kLWMld2VmkbdQuH7eGFqRkf2SYSnhLOxG+HvSAE7w==", "30b1e035-f180-49ec-9d38-d57cfb22ebc0" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708));

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708));
        }
    }
}
