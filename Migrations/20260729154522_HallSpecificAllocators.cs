using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefineryBooking.Migrations
{
    /// <inheritdoc />
    public partial class HallSpecificAllocators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AllocatorHallAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllocatorUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConferenceRoomId = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedByUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllocatorHallAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllocatorHallAssignments_AspNetUsers_AllocatorUserId",
                        column: x => x.AllocatorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllocatorHallAssignments_ConferenceRooms_ConferenceRoomId",
                        column: x => x.ConferenceRoomId,
                        principalTable: "ConferenceRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AllocatorHallAssignments_AllocatorUserId_ConferenceRoomId",
                table: "AllocatorHallAssignments",
                columns: new[] { "AllocatorUserId", "ConferenceRoomId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AllocatorHallAssignments_ConferenceRoomId",
                table: "AllocatorHallAssignments",
                column: "ConferenceRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllocatorHallAssignments");

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "324d44e9-d378-4f8b-8912-6e342b2c2c35", "AQAAAAIAAYagAAAAEKXf143+0bWUjo3wVUorKGMRj0MWZWubZs1AEw2Sk5MvhPZxOy9mPFHSuXSektIg3Q==", "77244867-1bc2-4535-a999-9000c4964a23" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbbb-bbbb-bbbb-bbbb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "45228da3-d73d-4de5-b9fb-48f41ec2d056", "AQAAAAIAAYagAAAAEE+8cZuz8J2LqmjBbNYp3jpdqPvR9YfJROO2XWWWPGGzXhnQ+2y/O2gHaUuUJ8vm8Q==", "31b0ed54-116d-4e27-a85a-8e53ecc0d807" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cccc-cccc-cccc-cccc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "14c46eb9-37dc-4fbc-b21c-a560cc317764", "AQAAAAIAAYagAAAAEGq12tyK2v0Ri8/0J2R0V6j+eA5C2le8SU9e+9iHRQCcEl0jJL16VyrxoKuUPvstMw==", "6477d2bd-191c-4fa4-926d-d3507fe4845a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dddd-dddd-dddd-dddd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "130b75a4-4088-493a-b764-1aa80ab3fdc8", "AQAAAAIAAYagAAAAEMOR9UvbC/DEOzWDYs19EqzEfGUmQ7LTEgYwQQcGVIMz4HK8SZNcX1Y7DvDiUYo11Q==", "3e2c93ff-c1f0-4885-8b22-e4e5b31bea8e" });
        }
    }
}
