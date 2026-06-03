using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CovaldysPilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCancellationReasonToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancellationReason",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationReason",
                table: "Events");
        }
    }
}
