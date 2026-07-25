using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefineryBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddHallBlocksAndHelpFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllocatorHelpDetails",
                table: "Bookings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ITFMHelpDetails",
                table: "Bookings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresAllocatorHelp",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresITFMHelp",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "HallBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConferenceRoomId = table.Column<int>(type: "int", nullable: false),
                    BlockedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HallBlocks_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_HallBlocks_ConferenceRooms_ConferenceRoomId",
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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ada25aa6-7586-4e3f-9a98-e73b9b8c4eef", "AQAAAAIAAYagAAAAECHRN9GLjRfYEDJq2iMiTkjXfByLRwWj1+4rvW6vCCEpYd0hvwSNIv2/4aPQp1yRGQ==", "9f82dbd4-1f83-438b-a711-27b073327da4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbbb-bbbb-bbbb-bbbb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8b2a8b05-cfcc-4bdf-bc4a-d779e989322f", "AQAAAAIAAYagAAAAEDI79FJWG/9W7L8uDh7Lzy5E0pcGB20biRmSm+OuB9zPn0HG31f1tev9ZYlcDfkUwQ==", "93ffe227-7197-4327-b03f-f97d6c3d2bb7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cccc-cccc-cccc-cccc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "899200b2-8309-44ca-84ff-d28361aceea9", "AQAAAAIAAYagAAAAEE823OzFcmRYXdp3f2EoQq0MbSo1Yh45uPkBOAptxT0uubbUUrenBKMLNQWVVORKlQ==", "ebdd7df6-a6eb-4f2b-8b25-e09dd4b87079" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dddd-dddd-dddd-dddd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "47dc5a0c-a8d2-426c-8369-78203e99a7c4", "AQAAAAIAAYagAAAAEFMcBg8VILmoe2xr6MxDWx7bpwlhKa9yUSChFl1G7GwO3FL2wGQHlUwYwM7NflJ1Zg==", "ea17e766-7bf5-4224-bf66-d0221bb8e319" });

            migrationBuilder.CreateIndex(
                name: "IX_HallBlocks_ConferenceRoomId",
                table: "HallBlocks",
                column: "ConferenceRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_HallBlocks_CreatedByUserId",
                table: "HallBlocks",
                column: "CreatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HallBlocks");

            migrationBuilder.DropColumn(
                name: "AllocatorHelpDetails",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ITFMHelpDetails",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RequiresAllocatorHelp",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RequiresITFMHelp",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                column: "ConcurrencyStamp",
                value: "928aa44a-8b3c-46d5-b320-a82da19e3692");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                column: "ConcurrencyStamp",
                value: "83989ce9-be91-4ccf-900d-67450c3d5c3e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                column: "ConcurrencyStamp",
                value: "27b8d3a0-6089-4cd0-95a7-0d84818d0e5b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                column: "ConcurrencyStamp",
                value: "5ecb3fcc-85f9-4a12-bea9-9e36c4b21cc9");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaa-aaaa-aaaa-aaaa",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "df81668a-5c5a-478b-8182-38d10bf93841", "AQAAAAIAAYagAAAAEHeWJtqUIGnNzRFBvaRJnFgsfM5Vyk1F/mTi994wImpNc/EeAYCj41MZnN63G5Rwig==", "9b2216ce-b661-41bc-86a8-10ef711ad420" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbbb-bbbb-bbbb-bbbb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c21156f-c820-47f8-8f0c-c44d460a6c8d", "AQAAAAIAAYagAAAAEFKCxTKyHLLaUAsVtiHc4s6SoFa9AwdU27/hAhP/foRevZ1EHDF0BVputMLH2tuu1Q==", "14e79640-72ba-42e2-9562-924a4ce0ca97" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cccc-cccc-cccc-cccc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78a086d1-87b2-4d1e-963a-0d90ae6039f8", "AQAAAAIAAYagAAAAEDDy/2qNUyoJgaPeNgvJa6S+oXrcwMvmvLXzpi7YVaQa9l12yMXFxylL2LVyXVMVBg==", "7c809da6-1ef8-4c95-be0f-1dbc09395733" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dddd-dddd-dddd-dddd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8874bca6-51a2-412f-b1bf-449358849a1b", "AQAAAAIAAYagAAAAEC5eQy3h5B10C4S37onMEWPxZF6cYfC+7kL16+M+e17MpCdn1vS8Khw2kNnSy75DUQ==", "dfc8860d-3df7-4243-85f0-34b726361795" });
        }
    }
}
