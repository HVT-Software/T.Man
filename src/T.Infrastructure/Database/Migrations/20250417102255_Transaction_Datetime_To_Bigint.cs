using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace T.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Transaction_Datetime_To_Bigint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateT",
                schema: "public",
                table: "Transaction",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreatedAtT",
                schema: "public",
                table: "Transaction",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "public",
                table: "Transaction",
                newName: "DateT");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "Transaction",
                newName: "CreatedAtT");
        }
    }
}
