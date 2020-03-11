using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Infrastructure.Migrations
{
    public partial class CreateSharedList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoSharedLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TimeOfSharing = table.Column<DateTime>(nullable: false),
                    ToDoListId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoSharedLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoSharedLists_ToDoLists_ToDoListId",
                        column: x => x.ToDoListId,
                        principalTable: "ToDoLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoSharedLists_ToDoListId",
                table: "ToDoSharedLists",
                column: "ToDoListId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoSharedLists");
        }
    }
}
