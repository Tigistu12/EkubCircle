using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EkubCircle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoundAndContributionConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Members_MemberId",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Members_ReceiverMemberId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Members_CircleId",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "PayoutOrder",
                table: "Members",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "HasReceived",
                table: "Members",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Members",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Contributions",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.CreateIndex(
                name: "IX_Members_CircleId_UserId",
                table: "Members",
                columns: new[] { "CircleId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Members_MemberId",
                table: "Contributions",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Members_ReceiverMemberId",
                table: "Rounds",
                column: "ReceiverMemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_Members_MemberId",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_Rounds_Members_ReceiverMemberId",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Members_CircleId_UserId",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "PayoutOrder",
                table: "Members",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "HasReceived",
                table: "Members",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Members",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Contributions",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.CreateIndex(
                name: "IX_Members_CircleId",
                table: "Members",
                column: "CircleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_Members_MemberId",
                table: "Contributions",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rounds_Members_ReceiverMemberId",
                table: "Rounds",
                column: "ReceiverMemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }
    }
}
