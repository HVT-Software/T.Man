using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace T.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Transaction_Change_Names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Date",
                schema: "public",
                table: "Transaction");

            migrationBuilder.AddColumn<long>(
                name: "CreatedAtT",
                schema: "public",
                table: "Transaction",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DateT",
                schema: "public",
                table: "Transaction",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtT",
                schema: "public",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "DateT",
                schema: "public",
                table: "Transaction");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "public",
                table: "Transaction",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Date",
                schema: "public",
                table: "Transaction",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
