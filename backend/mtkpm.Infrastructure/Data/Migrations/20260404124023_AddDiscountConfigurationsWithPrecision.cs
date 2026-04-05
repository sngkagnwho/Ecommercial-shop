using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mtkpm.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountConfigurationsWithPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountUsageHistories_Orders_OrderId",
                table: "DiscountUsageHistories");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "DiscountUsageHistories",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "DiscountUsageHistories",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Discounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DiscountType",
                table: "Discounts",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Discounts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Discounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicableProducts",
                table: "Discounts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicableCategories",
                table: "Discounts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminNotes",
                table: "Discounts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4854dae3-8481-46a1-82c1-d7d01055b634");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1909d624-dccd-4c2c-aa42-3a9a3674e05e");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountUsageHistories_UsedAt",
                table: "DiscountUsageHistories",
                column: "UsedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountUsageHistories_UserId",
                table: "DiscountUsageHistories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_Code",
                table: "Discounts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_EndDate",
                table: "Discounts",
                column: "EndDate");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_IsActive",
                table: "Discounts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_StartDate",
                table: "Discounts",
                column: "StartDate");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountUsageHistories_Orders_OrderId",
                table: "DiscountUsageHistories",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountUsageHistories_Orders_OrderId",
                table: "DiscountUsageHistories");

            migrationBuilder.DropIndex(
                name: "IX_DiscountUsageHistories_UsedAt",
                table: "DiscountUsageHistories");

            migrationBuilder.DropIndex(
                name: "IX_DiscountUsageHistories_UserId",
                table: "DiscountUsageHistories");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_Code",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_EndDate",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_IsActive",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_StartDate",
                table: "Discounts");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "DiscountUsageHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "DiscountUsageHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DiscountType",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicableProducts",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicableCategories",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminNotes",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5df4d501-2bf4-4623-810a-d834bb63f125");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "57d6319c-2c7f-4823-8cdd-3f9ea43400aa");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountUsageHistories_Orders_OrderId",
                table: "DiscountUsageHistories",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
