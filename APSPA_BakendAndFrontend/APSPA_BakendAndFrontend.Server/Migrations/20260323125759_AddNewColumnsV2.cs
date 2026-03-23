using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace APSPA_BakendAndFrontend.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumnsV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recommendations");

            migrationBuilder.DropColumn(
                name: "effort_score",
                table: "predictions");

            migrationBuilder.RenameColumn(
                name: "fatigue_risk_score",
                table: "predictions",
                newName: "difference");

            migrationBuilder.AddColumn<string>(
                name: "effort_level",
                table: "predictions",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "fatigue_risk",
                table: "predictions",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_anomaly",
                table: "predictions",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "recommendation",
                table: "predictions",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_type",
                table: "activity_records",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<float>(
                name: "measured_heart_rate",
                table: "activity_records",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "weather_conditions",
                table: "activity_records",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "effort_level",
                table: "predictions");

            migrationBuilder.DropColumn(
                name: "fatigue_risk",
                table: "predictions");

            migrationBuilder.DropColumn(
                name: "is_anomaly",
                table: "predictions");

            migrationBuilder.DropColumn(
                name: "recommendation",
                table: "predictions");

            migrationBuilder.DropColumn(
                name: "measured_heart_rate",
                table: "activity_records");

            migrationBuilder.DropColumn(
                name: "weather_conditions",
                table: "activity_records");

            migrationBuilder.RenameColumn(
                name: "difference",
                table: "predictions",
                newName: "fatigue_risk_score");

            migrationBuilder.AddColumn<decimal>(
                name: "effort_score",
                table: "predictions",
                type: "numeric(8,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "activity_type",
                table: "activity_records",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prediction_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    level = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recommendations_predictions_prediction_id",
                        column: x => x.prediction_id,
                        principalTable: "predictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recommendations_prediction_id",
                table: "recommendations",
                column: "prediction_id");
        }
    }
}
