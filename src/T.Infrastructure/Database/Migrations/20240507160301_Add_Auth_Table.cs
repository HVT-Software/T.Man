using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DR.Infrastructure.Database.Migrations {
    /// <inheritdoc />
    public partial class Add_Auth_Table : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "public",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 32, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", maxLength: 32, nullable: true),
                    ClaimName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsClaim = table.Column<bool>(type: "boolean", nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "public",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 32, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SearchName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "public",
                columns: table => new {
                    RoleId = table.Column<Guid>(type: "uuid", maxLength: 32, nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", maxLength: 32, nullable: false),
                    IsEnable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_RolePermission", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "public",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "public",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "public",
                columns: table => new {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "public",
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Permission",
                columns: new[] { "Id", "ClaimName", "DisplayName", "IsActive", "IsClaim", "IsDefault", "OrderIndex", "ParentId" },
                values: new object[,]
                {
                    { new Guid("01f543fd-521f-41c7-83b3-00253996dd69"), "DR.Kaban", "Quản lý dự án", true, true, true, 0, null },
                    { new Guid("31f07c51-7067-4e96-9f44-de6a02818513"), "DR.User.Password", "Quản lý mật khẩu", true, true, false, 3, new Guid("de5ffa57-021d-4768-b361-894828259350") },
                    { new Guid("4ff1aa66-fc29-4e06-becb-6e307e6aa09a"), "DR.DevTools", "Công cụ", true, true, true, 1, null },
                    { new Guid("8ad5baf8-b7f6-433c-94e7-87ca45728945"), "DR.User.Edit", "Cập nhật người dùng", true, true, false, 4, new Guid("cc91c9c4-5845-407d-867b-0c1453f2b852") },
                    { new Guid("cc91c9c4-5845-407d-867b-0c1453f2b852"), "DR.User", "Quản lý người dùng", true, true, false, 2, null }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "User",
                columns: new[] { "Id", "Address", "IsActive", "IsAdmin", "IsDeleted", "IsSystem", "Name", "Password", "Phone", "RoleId", "Username" },
                values: new object[] { new Guid("dec5aee5-12e1-4b61-8d3f-ad5d5235e6cd"), "Thanh An, Hớn Quản, Bình Phước", true, true, false, true, "Admin", "", "", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "public",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                schema: "public",
                table: "User",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "public");

            migrationBuilder.DropTable(
                name: "User",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "public");
        }
    }
}
