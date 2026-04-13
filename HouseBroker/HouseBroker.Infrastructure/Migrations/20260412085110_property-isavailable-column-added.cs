using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseBroker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class propertyisavailablecolumnadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMc9cBI3iENfK32AjwD3WnkLQb6ZLKcL45ja4SD3G0cdE3DgRhCv+e/P+LTXqLs5iQ==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEFJ7mJR7nD/1Ph/f4+5KeIL5tcQpktgtN++sCGI6QvoqGJ62PJWEe+1AV123gZ/iyw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Properties");

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
        }
    }
}
