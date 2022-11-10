using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcProject.Dal.Migrations
{
    public partial class NEW_ROLE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT [dbo].AspNetRoles (Name, NormalizedName) VALUES ('test','TEST')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
