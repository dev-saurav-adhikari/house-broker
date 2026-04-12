using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HouseBroker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class tableandseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommissionSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinimumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommissionSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    ProvinceId1 = table.Column<long>(type: "bigint", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceId1",
                        column: x => x.ProvinceId1,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ProvinceId = table.Column<long>(type: "bigint", nullable: false),
                    DistrictId = table.Column<long>(type: "bigint", nullable: false),
                    Municipality = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    WardNumber = table.Column<int>(type: "int", nullable: false),
                    LandMark = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PropertyType = table.Column<int>(type: "int", nullable: false),
                    BrokerId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Properties_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEGZrtfOqyTU6I2VCSl+L/Axp9fQq8QjcZGaAu/XwBZFm+cPAEkG42xUeh6/oUDl1CA==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEP5URHh48ZNtyPIHafeeahm91ABzpFCkEB1scBK1WoBs0aGgGdQwSN9RpwSByIDerg==");

            migrationBuilder.InsertData(
                table: "CommissionSettings",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "MaximumAmount", "MinimumAmount", "ModifiedBy", "ModifiedOn", "Rate" },
                values: new object[,]
                {
                    { 1L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 5000000m, 0m, null, null, 2m },
                    { 2L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 10000000m, 5000000m, null, null, 1.75m },
                    { 3L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 0m, 10000000m, null, null, 1.5m }
                });

            migrationBuilder.InsertData(
                table: "Provinces",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Koshi Province" },
                    { 2L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Madhesh Province" },
                    { 3L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Bagmati Province" },
                    { 4L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Gandaki Province" },
                    { 5L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Lumbini Province" },
                    { 6L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Karnali Province" },
                    { 7L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Sudurpashchim Province" }
                });

            migrationBuilder.InsertData(
                table: "Districts",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "ModifiedBy", "ModifiedOn", "Name", "ProvinceId", "ProvinceId1" },
                values: new object[,]
                {
                    { 1L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Taplejung", 1L, null },
                    { 2L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Panchthar", 1L, null },
                    { 3L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Ilam", 1L, null },
                    { 4L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Jhapa", 1L, null },
                    { 5L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Morang", 1L, null },
                    { 6L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Sunsari", 1L, null },
                    { 7L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Dhankuta", 1L, null },
                    { 8L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Terhathum", 1L, null },
                    { 9L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Sankhuwasabha", 1L, null },
                    { 10L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Bhojpur", 1L, null },
                    { 11L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Solukhumbu", 1L, null },
                    { 12L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Okhaldhunga", 1L, null },
                    { 13L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Khotang", 1L, null },
                    { 14L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Udayapur", 1L, null },
                    { 15L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Saptari", 2L, null },
                    { 16L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Siraha", 2L, null },
                    { 17L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Dhanusha", 2L, null },
                    { 18L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Mahottari", 2L, null },
                    { 19L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Sarlahi", 2L, null },
                    { 20L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Rautahat", 2L, null },
                    { 21L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Bara", 2L, null },
                    { 22L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Parsa", 2L, null },
                    { 23L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Kathmandu", 3L, null },
                    { 24L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Lalitpur", 3L, null },
                    { 25L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Bhaktapur", 3L, null },
                    { 26L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Chitwan", 3L, null },
                    { 27L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Makwanpur", 3L, null },
                    { 28L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Kavrepalanchowk", 3L, null },
                    { 29L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Nuwakot", 3L, null },
                    { 30L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Dhading", 3L, null },
                    { 31L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Sindhupalchok", 3L, null },
                    { 32L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Dolakha", 3L, null },
                    { 33L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Ramechhap", 3L, null },
                    { 34L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Sindhuli", 3L, null },
                    { 35L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Rasuwa", 3L, null },
                    { 36L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Kaski", 4L, null },
                    { 37L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Tanahun", 4L, null },
                    { 38L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Syangja", 4L, null },
                    { 39L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Nawalpur", 4L, null },
                    { 40L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Gorkha", 4L, null },
                    { 41L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Baglung", 4L, null },
                    { 42L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Lamjung", 4L, null },
                    { 43L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Parbat", 4L, null },
                    { 44L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Myagdi", 4L, null },
                    { 45L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Mustang", 4L, null },
                    { 46L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Manang", 4L, null },
                    { 47L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Rupandehi", 5L, null },
                    { 48L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Dang", 5L, null },
                    { 49L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Banke", 5L, null },
                    { 50L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Kapilvastu", 5L, null },
                    { 51L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Bardiya", 5L, null },
                    { 52L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Arghakhanchi", 5L, null },
                    { 53L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Gulmi", 5L, null },
                    { 54L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Palpa", 5L, null },
                    { 55L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Pyuthan", 5L, null },
                    { 56L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Rolpa", 5L, null },
                    { 57L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Parasi", 5L, null },
                    { 58L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Rukum East", 5L, null },
                    { 59L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Surkhet", 6L, null },
                    { 60L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Dailekh", 6L, null },
                    { 61L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Salyan", 6L, null },
                    { 62L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Jajarkot", 6L, null },
                    { 63L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Rukum West", 6L, null },
                    { 64L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Kalikot", 6L, null },
                    { 65L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Jumla", 6L, null },
                    { 66L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Mugu", 6L, null },
                    { 67L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Dolpa", 6L, null },
                    { 68L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Humla", 6L, null },
                    { 69L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Kailali", 7L, null },
                    { 70L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Kanchanpur", 7L, null },
                    { 71L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Doti", 7L, null },
                    { 72L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Baitadi", 7L, null },
                    { 73L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Achham", 7L, null },
                    { 74L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Bajhang", 7L, null },
                    { 75L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Bajura", 7L, null },
                    { 76L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Dadeldhura", 7L, null },
                    { 77L, null, new DateTimeOffset(new DateTime(2026, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Darchula", 7L, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceId1",
                table: "Districts",
                column: "ProvinceId1");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_DistrictId",
                table: "Properties",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ProvinceId",
                table: "Properties",
                column: "ProvinceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommissionSettings");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAED0+57jc0hWeJGUaoKhOAuTWHk4QxQ6VUfxmgJ98LM13c5MmMHY+PKdt/CCxbVMwQw==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEAFOtlgcttwSYrX2pTtl+6v1SaKPe98FyuAyXKdCQEVe6K3afxq+HSYjUKQH9jVFbA==");
        }
    }
}
