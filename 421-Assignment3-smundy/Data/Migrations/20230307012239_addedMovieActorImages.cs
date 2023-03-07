using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _421_Assignment3_smundy.Data.Migrations
{
    public partial class addedMovieActorImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "ActorPhoto",
                table: "MovieActor",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "MoviePoster",
                table: "MovieActor",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActorPhoto",
                table: "MovieActor");

            migrationBuilder.DropColumn(
                name: "MoviePoster",
                table: "MovieActor");
        }
    }
}
