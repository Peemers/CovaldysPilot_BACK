using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CovaldysPilot.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteConfigurationSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SiteConfigurations",
                columns: new[] { "Id", "GlobalAlertMessage", "IsMaintenanceMode" },
                values: new object[] { 1, null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SiteConfigurations",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
