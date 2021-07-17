using Microsoft.EntityFrameworkCore.Migrations;

namespace HopOn.Model.Migrations
{
    public partial class CHunkData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChunkCount",
                table: "ProgressBarLists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChunkCount",
                table: "ProgressBarLists");
        }
    }
}
