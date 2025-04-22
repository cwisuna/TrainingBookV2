using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainingBookV2.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingNoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingNotes",
                columns: table => new
                {
                    TrainingNoteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTrainingStepID = table.Column<int>(type: "int", nullable: false),
                    AuthorID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingNotes", x => x.TrainingNoteID);
                    table.ForeignKey(
                        name: "FK_TrainingNotes_UserTrainingSteps_UserTrainingStepID",
                        column: x => x.UserTrainingStepID,
                        principalTable: "UserTrainingSteps",
                        principalColumn: "UserTrainingStepID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingNotes_Users_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingNotes_AuthorID",
                table: "TrainingNotes",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingNotes_UserTrainingStepID",
                table: "TrainingNotes",
                column: "UserTrainingStepID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingNotes");
        }
    }
}
