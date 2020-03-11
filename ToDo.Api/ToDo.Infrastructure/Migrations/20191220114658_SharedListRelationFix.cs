using Microsoft.EntityFrameworkCore.Migrations;

namespace ToDo.Infrastructure.Migrations
{
    public partial class SharedListRelationFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ToDoSharedLists_ToDoListId",
                table: "ToDoSharedLists");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoSharedLists_ToDoListId",
                table: "ToDoSharedLists",
                column: "ToDoListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ToDoSharedLists_ToDoListId",
                table: "ToDoSharedLists");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoSharedLists_ToDoListId",
                table: "ToDoSharedLists",
                column: "ToDoListId",
                unique: true);
        }
    }
}
