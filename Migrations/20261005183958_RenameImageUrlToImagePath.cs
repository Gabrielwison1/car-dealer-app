using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealerApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameImageUrlToImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Cars",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Cars",
                newName: "ImageUrl");
        }
    }
}
