using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace T.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Add_Avatar_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                schema: "public",
                table: "User",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                schema: "public",
                table: "User",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "public",
                table: "User",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                schema: "public",
                table: "User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Merchant_MerchantId",
                schema: "public",
                table: "User",
                column: "MerchantId",
                principalSchema: "public",
                principalTable: "Merchant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Merchant_MerchantId",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Avatar",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "public",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Provider",
                schema: "public",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                schema: "public",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11,
                oldNullable: true);
        }
    }
}
