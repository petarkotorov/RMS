using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Central.API.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Tables_SyncWorker_Product_Change_Store_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StoreTo",
                table: "SyncWorkers",
                newName: "DestinationStore");

            migrationBuilder.RenameColumn(
                name: "StoreTo",
                table: "Products",
                newName: "SourceStore");

            migrationBuilder.RenameColumn(
                name: "StoreFrom",
                table: "Products",
                newName: "DestinationStore");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DestinationStore",
                table: "SyncWorkers",
                newName: "StoreTo");

            migrationBuilder.RenameColumn(
                name: "SourceStore",
                table: "Products",
                newName: "StoreTo");

            migrationBuilder.RenameColumn(
                name: "DestinationStore",
                table: "Products",
                newName: "StoreFrom");
        }
    }
}
