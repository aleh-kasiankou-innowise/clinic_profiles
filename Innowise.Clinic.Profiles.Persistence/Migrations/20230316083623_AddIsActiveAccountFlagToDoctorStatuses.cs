using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Innowise.Clinic.Profiles.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveAccountFlagToDoctorStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActiveAccount",
                table: "Statuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableForAppointments",
                table: "Statuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("2270714a-25a4-467b-b07a-3ce15b4fa035"),
                columns: new[] { "IsActiveAccount", "IsAvailableForAppointments" },
                values: new object[] { true, false });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("27fddac5-d7cc-41d4-b22d-ff41b98a09d7"),
                columns: new[] { "IsActiveAccount", "IsAvailableForAppointments" },
                values: new object[] { true, false });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("53bc1111-9b82-4e0a-9c79-4132e7d3e472"),
                columns: new[] { "IsActiveAccount", "IsAvailableForAppointments" },
                values: new object[] { true, false });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("5a2466e3-ad9d-4454-86f6-ae6e63183116"),
                columns: new[] { "IsActiveAccount", "IsAvailableForAppointments" },
                values: new object[] { true, false });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("724eac90-dee0-454e-bdc5-10742042bbdd"),
                columns: new[] { "IsActiveAccount", "IsAvailableForAppointments" },
                values: new object[] { true, false });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("7be6ae9f-1534-416c-b420-c2c191f7b3fc"),
                columns: new[] { "IsActiveAccount", "IsAvailableForAppointments" },
                values: new object[] { true, false });

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "StatusId",
                keyValue: new Guid("e8dfe97a-1c89-45ff-b08c-a97f6edc5e41"),
                columns: new[] { "IsActiveAccount", "IsAvailableForAppointments" },
                values: new object[] { true, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActiveAccount",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "IsAvailableForAppointments",
                table: "Statuses");
        }
    }
}
