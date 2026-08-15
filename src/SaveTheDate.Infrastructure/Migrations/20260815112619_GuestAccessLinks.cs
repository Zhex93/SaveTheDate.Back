using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaveTheDate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GuestAccessLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invitations_guests_guest_id",
                table: "invitations");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_guests_id",
                table: "guests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_guests",
                table: "guests");

            migrationBuilder.DropIndex(
                name: "IX_guests_id",
                table: "guests");

            migrationBuilder.AddColumn<DateTime>(
                name: "access_token_created_at",
                table: "invitations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "access_token_hash",
                table: "invitations",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "guests",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_guests",
                table: "guests",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_invitations_access_token_hash",
                table: "invitations",
                column: "access_token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_guests_phone",
                table: "guests",
                column: "phone",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_invitations_guests_guest_id",
                table: "invitations",
                column: "guest_id",
                principalTable: "guests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invitations_guests_guest_id",
                table: "invitations");

            migrationBuilder.DropIndex(
                name: "IX_invitations_access_token_hash",
                table: "invitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_guests",
                table: "guests");

            migrationBuilder.DropIndex(
                name: "IX_guests_phone",
                table: "guests");

            migrationBuilder.DropColumn(
                name: "access_token_created_at",
                table: "invitations");

            migrationBuilder.DropColumn(
                name: "access_token_hash",
                table: "invitations");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "guests",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_guests_id",
                table: "guests",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_guests",
                table: "guests",
                column: "phone");

            migrationBuilder.CreateIndex(
                name: "IX_guests_id",
                table: "guests",
                column: "id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_invitations_guests_guest_id",
                table: "invitations",
                column: "guest_id",
                principalTable: "guests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
