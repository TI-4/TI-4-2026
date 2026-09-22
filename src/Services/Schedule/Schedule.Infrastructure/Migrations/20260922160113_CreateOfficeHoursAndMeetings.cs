using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Schedule.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateOfficeHoursAndMeetings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfficeHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherRefId = table.Column<Guid>(type: "uuid", nullable: false),
                    DayOfWeek = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeHours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherRefId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentRefId = table.Column<Guid>(type: "uuid", nullable: false),
                    StructureRefId = table.Column<Guid>(type: "uuid", nullable: false),
                    OfficeHourId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meetings_OfficeHours_OfficeHourId",
                        column: x => x.OfficeHourId,
                        principalTable: "OfficeHours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_OfficeHourId",
                table: "Meetings",
                column: "OfficeHourId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_StructureRefId_ScheduledAt",
                table: "Meetings",
                columns: new[] { "StructureRefId", "ScheduledAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_StudentRefId_ScheduledAt",
                table: "Meetings",
                columns: new[] { "StudentRefId", "ScheduledAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_TeacherRefId_ScheduledAt",
                table: "Meetings",
                columns: new[] { "TeacherRefId", "ScheduledAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OfficeHours_TeacherRefId_DayOfWeek",
                table: "OfficeHours",
                columns: new[] { "TeacherRefId", "DayOfWeek" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "OfficeHours");
        }
    }
}
