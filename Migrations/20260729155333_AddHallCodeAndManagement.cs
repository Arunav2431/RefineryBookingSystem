using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefineryBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddHallCodeAndManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CostCentreCode",
                table: "ConferenceRooms",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ConferenceRooms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "ConferenceRooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ConferenceRooms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FloorNumber",
                table: "ConferenceRooms",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HallCode",
                table: "ConferenceRooms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerDepartment",
                table: "ConferenceRooms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "BuildingLocation", "CostCentreCode", "CreatedAt", "CreatedByUserId", "Description", "FloorNumber", "HallCode", "OwnerDepartment" },
                values: new object[] { "Admin Block A", "1001", new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708), "aaaa-aaaa-aaaa-aaaa", null, "3", "CC-1001-ADM-01", "Administration" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CostCentreCode", "CreatedAt", "CreatedByUserId", "Description", "FloorNumber", "HallCode", "OwnerDepartment" },
                values: new object[] { "1002", new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708), "aaaa-aaaa-aaaa-aaaa", null, "G", "CC-1002-OPS-01", "Operations" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BuildingLocation", "CostCentreCode", "CreatedAt", "CreatedByUserId", "Description", "FloorNumber", "HallCode", "OwnerDepartment" },
                values: new object[] { "Visitor Center", "1003", new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708), "aaaa-aaaa-aaaa-aaaa", null, "G", "CC-1003-HSE-01", "HSE" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CostCentreCode", "CreatedAt", "CreatedByUserId", "Description", "FloorNumber", "HallCode", "OwnerDepartment" },
                values: new object[] { "1004", new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708), "aaaa-aaaa-aaaa-aaaa", null, "1", "CC-1004-ENG-01", "Engineering" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BuildingLocation", "CostCentreCode", "CreatedAt", "CreatedByUserId", "Description", "FloorNumber", "HallCode", "OwnerDepartment" },
                values: new object[] { "HQ Tower", "1001", new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708), "aaaa-aaaa-aaaa-aaaa", null, "5", "CC-1001-EXC-01", "Executive" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CostCentreCode", "CreatedAt", "CreatedByUserId", "Description", "FloorNumber", "HallCode", "OwnerDepartment" },
                values: new object[] { "1002", new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708), "aaaa-aaaa-aaaa-aaaa", null, "2", "CC-1002-OPS-02", "Operations" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BuildingLocation", "CostCentreCode", "CreatedAt", "CreatedByUserId", "Description", "FloorNumber", "HallCode", "OwnerDepartment" },
                values: new object[] { "Safety Block", "1003", new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708), "aaaa-aaaa-aaaa-aaaa", null, "G", "CC-1003-HSE-02", "HSE" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "BuildingLocation", "CostCentreCode", "CreatedAt", "CreatedByUserId", "Description", "FloorNumber", "HallCode", "OwnerDepartment" },
                values: new object[] { "Maintenance Bldg", "1005", new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708), "aaaa-aaaa-aaaa-aaaa", null, "1", "CC-1005-MNT-01", "Maintenance" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CostCentreCode", "CreatedAt", "CreatedByUserId", "Description", "FloorNumber", "HallCode", "OwnerDepartment" },
                values: new object[] { "1006", new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708), "aaaa-aaaa-aaaa-aaaa", null, "G", "CC-1006-ITS-01", "IT Services" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "BuildingLocation", "CostCentreCode", "CreatedAt", "CreatedByUserId", "Description", "FloorNumber", "HallCode", "OwnerDepartment" },
                values: new object[] { "Warehouse Block B", "1007", new DateTime(2026, 7, 29, 15, 53, 32, 117, DateTimeKind.Utc).AddTicks(2708), "aaaa-aaaa-aaaa-aaaa", null, "2", "CC-1007-LOG-01", "Logistics" });

            migrationBuilder.CreateIndex(
                name: "IX_ConferenceRooms_HallCode",
                table: "ConferenceRooms",
                column: "HallCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConferenceRooms_HallCode",
                table: "ConferenceRooms");

            migrationBuilder.DropColumn(
                name: "CostCentreCode",
                table: "ConferenceRooms");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ConferenceRooms");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "ConferenceRooms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ConferenceRooms");

            migrationBuilder.DropColumn(
                name: "FloorNumber",
                table: "ConferenceRooms");

            migrationBuilder.DropColumn(
                name: "HallCode",
                table: "ConferenceRooms");

            migrationBuilder.DropColumn(
                name: "OwnerDepartment",
                table: "ConferenceRooms");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                column: "ConcurrencyStamp",
                value: "d16813a1-7755-4d13-95e7-6c93afe1bc06");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                column: "ConcurrencyStamp",
                value: "9c71bd2f-d8cb-4f56-a28c-922a68107c66");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                column: "ConcurrencyStamp",
                value: "83260c85-3f35-4134-91bf-f46fcffd2f06");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                column: "ConcurrencyStamp",
                value: "f34a9b08-4aad-4887-8b9a-ff8b7673c7f2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaa-aaaa-aaaa-aaaa",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "096cd762-252f-4bd2-b201-55a9da0e1e8c", "AQAAAAIAAYagAAAAEMS/UG/awBxriw03kqbmPzdeq9ry8gfNMozhCtYn8yJRfsawGaCbDkdRAdRx4Q6HtA==", "8599b31b-e4ed-433a-bb8c-7e5adea8718f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbbb-bbbb-bbbb-bbbb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "71f69e20-a188-4bef-89b6-5020c0268690", "AQAAAAIAAYagAAAAEKO1RCNoxZOhIUeKzGSiPY3UFB4oMYK8mUbw2Zv8KdYDJQ3MgzHOnPQwEJXjPwtU6A==", "c88c7e64-d2d1-42e8-ba3a-4ecb4d2858fd" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cccc-cccc-cccc-cccc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "77246e1b-18fc-48ad-ab93-18d64d392499", "AQAAAAIAAYagAAAAENYLhZWnG4+Qbgh2uKcnSrvpdqPSRI2RhFzjaZDqrQnLL7c/UOR12TnyIIG49eoOhw==", "96555fdd-444d-4f6b-b4db-1cf777f6b373" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dddd-dddd-dddd-dddd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3697af9c-c517-443c-8833-c5923c24ff43", "AQAAAAIAAYagAAAAEHHjLcsF6tse85K/BqztJH0amzaIoVO7iIzoo4fm1NF/Snj0S/m+OamVLXlM+9dFxQ==", "643ee1ae-5f03-4d9a-ada1-5d7e33c962d2" });

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "BuildingLocation",
                value: "Admin Block A, Floor 3");

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "BuildingLocation",
                value: "Visitor Center, Ground Floor");

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 5,
                column: "BuildingLocation",
                value: "HQ Tower, Floor 5");

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 7,
                column: "BuildingLocation",
                value: "Safety Block, Ground Floor");

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 8,
                column: "BuildingLocation",
                value: "Maintenance Bldg, Floor 1");

            migrationBuilder.UpdateData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 10,
                column: "BuildingLocation",
                value: "Warehouse Block B, Floor 2");
        }
    }
}
