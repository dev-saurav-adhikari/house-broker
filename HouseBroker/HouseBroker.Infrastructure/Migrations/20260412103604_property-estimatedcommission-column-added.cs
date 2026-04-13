using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseBroker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class propertyestimatedcommissioncolumnadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedCommission",
                table: "Properties",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC64pCHxX6y6MHjezqB+AMZUd28sBMlyaprQsSUcduek38AnitPhKefIXGRx8fvX8g==");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2L,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPQ+YPGPsFQMktbylSUwwQnQUznamPWoTssA2d5b6YTY19oomWIYUEsX/DP+5/n+Eg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedCommission",
                table: "Properties");

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
    }
}
