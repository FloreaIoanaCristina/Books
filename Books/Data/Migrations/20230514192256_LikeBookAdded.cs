using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Books.Data.Migrations
{
    /// <inheritdoc />
    public partial class LikeBookAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteBook_AspNetUsers_UserId",
                table: "FavoriteBook");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteBook_BooksTable_BookId",
                table: "FavoriteBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteBook",
                table: "FavoriteBook");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteBook_BookId",
                table: "FavoriteBook");

            migrationBuilder.DropColumn(
                name: "FavoriteBookId",
                table: "FavoriteBook");

            migrationBuilder.RenameTable(
                name: "FavoriteBook",
                newName: "FavoriteBooks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "FavoriteBooks",
                newName: "TargetBookId");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "FavoriteBooks",
                newName: "SourceUserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteBook_UserId",
                table: "FavoriteBooks",
                newName: "IX_FavoriteBooks_TargetBookId");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "FavoriteBooks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteBooks",
                table: "FavoriteBooks",
                columns: new[] { "SourceUserId", "TargetBookId" });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBooks_AppUserId",
                table: "FavoriteBooks",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteBooks_AspNetUsers_AppUserId",
                table: "FavoriteBooks",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteBooks_AspNetUsers_SourceUserId",
                table: "FavoriteBooks",
                column: "SourceUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteBooks_BooksTable_TargetBookId",
                table: "FavoriteBooks",
                column: "TargetBookId",
                principalTable: "BooksTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteBooks_AspNetUsers_AppUserId",
                table: "FavoriteBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteBooks_AspNetUsers_SourceUserId",
                table: "FavoriteBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteBooks_BooksTable_TargetBookId",
                table: "FavoriteBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteBooks",
                table: "FavoriteBooks");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteBooks_AppUserId",
                table: "FavoriteBooks");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "FavoriteBooks");

            migrationBuilder.RenameTable(
                name: "FavoriteBooks",
                newName: "FavoriteBook");

            migrationBuilder.RenameColumn(
                name: "TargetBookId",
                table: "FavoriteBook",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "SourceUserId",
                table: "FavoriteBook",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteBooks_TargetBookId",
                table: "FavoriteBook",
                newName: "IX_FavoriteBook_UserId");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteBookId",
                table: "FavoriteBook",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteBook",
                table: "FavoriteBook",
                column: "FavoriteBookId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteBook_BookId",
                table: "FavoriteBook",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteBook_AspNetUsers_UserId",
                table: "FavoriteBook",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteBook_BooksTable_BookId",
                table: "FavoriteBook",
                column: "BookId",
                principalTable: "BooksTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
