using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RefineryBooking.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceBookingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedsLaptop",
                table: "ITFacilityRequirements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsLaserPointer",
                table: "ITFacilityRequirements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsPASystem",
                table: "ITFacilityRequirements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NeedsWhiteboard",
                table: "ITFacilityRequirements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "Bookings",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostCentre",
                table: "Bookings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrganizerName",
                table: "Bookings",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Bookings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

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

            migrationBuilder.InsertData(
                table: "ConferenceRooms",
                columns: new[] { "Id", "BuildingLocation", "Capacity", "HasProjector", "HasVideoConferencing", "HasWhiteboard", "IsActive", "Name" },
                values: new object[,]
                {
                    { 5, "HQ Tower, Floor 5", 16, true, true, true, true, "Executive Strategy Suite" },
                    { 6, "Central Control Building", 30, true, true, true, true, "Refinery Operations War Room" },
                    { 7, "Safety Block, Ground Floor", 120, true, true, false, true, "HSE Training Auditorium" },
                    { 8, "Maintenance Bldg, Floor 1", 20, true, false, true, true, "Turnaround Planning Room" },
                    { 9, "IT Services Block", 10, false, true, true, true, "IT & Instrumentation Lab" },
                    { 10, "Warehouse Block B, Floor 2", 14, true, false, true, true, "Logistics & Dispatch Conference Room" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DropColumn(
                name: "NeedsLaptop",
                table: "ITFacilityRequirements");

            migrationBuilder.DropColumn(
                name: "NeedsLaserPointer",
                table: "ITFacilityRequirements");

            migrationBuilder.DropColumn(
                name: "NeedsPASystem",
                table: "ITFacilityRequirements");

            migrationBuilder.DropColumn(
                name: "NeedsWhiteboard",
                table: "ITFacilityRequirements");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CostCentre",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "OrganizerName",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "11111111-1111-1111-1111-111111111111",
                column: "ConcurrencyStamp",
                value: "91de0d28-0198-49ca-80c8-03a8a39c092f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22222222-2222-2222-2222-222222222222",
                column: "ConcurrencyStamp",
                value: "f1f9499e-f8f5-48c0-8ad2-0e0e4719faef");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33333333-3333-3333-3333-333333333333",
                column: "ConcurrencyStamp",
                value: "cedbb007-b2e1-4c0a-9a54-47e0b8bdea49");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44444444-4444-4444-4444-444444444444",
                column: "ConcurrencyStamp",
                value: "760bf9b8-dd03-4380-b717-de2d18398edc");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "aaaa-aaaa-aaaa-aaaa",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d6e51cf8-ce7c-4eda-8cda-277d512602d8", "AQAAAAIAAYagAAAAEPNRQyz8ulN8fJVgAL0PqEmuh4cTdL6iOQZZXlSdPfkFHhXZl+GvDdXf1uvoC/dobg==", "ba4dc9b5-e709-44a2-bd3c-afe63d37f40d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bbbb-bbbb-bbbb-bbbb",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "51532d81-0c98-4331-9a0e-5a534877ecff", "AQAAAAIAAYagAAAAEIsbjrwnWroPoiRd613nbs/D+zDW4GRSW05qVsDvFGdFSvbF1CMRYyCcrgMrZd1Ynw==", "2c122402-ec2e-42f0-a3dd-bcf83929ffb2" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cccc-cccc-cccc-cccc",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "323e6a44-bc92-4be6-bed9-8175c55b817a", "AQAAAAIAAYagAAAAEMij9VVUAjLEUanH0DcPi33vO//my0pyIVbsfBfVRE0vOLCaxLrxWIdZW05dpxXtaw==", "25f918d0-6589-41f4-afea-5a8c81789feb" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dddd-dddd-dddd-dddd",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7091b728-8be9-47cc-af2b-16b771d113bf", "AQAAAAIAAYagAAAAEHTEg3nXaxhMtIDh3ulWXZkR70zvf0R0KJLyIl/XnUcKMTCkG0yqlqAXyzU0XKjULQ==", "3508a8b3-fdf7-42f5-a7df-3f293419786e" });
        }
    }
}
