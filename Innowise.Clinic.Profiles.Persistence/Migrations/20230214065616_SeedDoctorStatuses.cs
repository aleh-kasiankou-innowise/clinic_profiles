using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Innowise.Clinic.Profiles.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedDoctorStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "StatusId", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("2270714a-25a4-467b-b07a-3ce15b4fa035"), "Self-isolation", "Self-isolation" },
                    { new Guid("27fddac5-d7cc-41d4-b22d-ff41b98a09d7"), "Inactive", "Inactive" },
                    { new Guid("53bc1111-9b82-4e0a-9c79-4132e7d3e472"), "Sick Leave", "Sick Leave" },
                    { new Guid("5a2466e3-ad9d-4454-86f6-ae6e63183116"), "Leave without pay", "Leave without pay" },
                    { new Guid("724eac90-dee0-454e-bdc5-10742042bbdd"), "On vacation", "On vacation" },
                    { new Guid("7be6ae9f-1534-416c-b420-c2c191f7b3fc"), "Sick Day", "Sick Day" },
                    { new Guid("e8dfe97a-1c89-45ff-b08c-a97f6edc5e41"), "At work", "At work" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("2270714a-25a4-467b-b07a-3ce15b4fa035"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("27fddac5-d7cc-41d4-b22d-ff41b98a09d7"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("53bc1111-9b82-4e0a-9c79-4132e7d3e472"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("5a2466e3-ad9d-4454-86f6-ae6e63183116"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("724eac90-dee0-454e-bdc5-10742042bbdd"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("7be6ae9f-1534-416c-b420-c2c191f7b3fc"));

            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("e8dfe97a-1c89-45ff-b08c-a97f6edc5e41"));
        }
    }
}
