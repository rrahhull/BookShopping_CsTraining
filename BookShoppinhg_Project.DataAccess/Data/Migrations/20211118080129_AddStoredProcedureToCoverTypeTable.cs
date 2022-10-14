using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShoppinhg_Project.DataAccess.Migrations
{
    public partial class AddStoredProcedureToCoverTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CoverType_Create
                @name varchar(50)
                As
                insert coverTypes values(@name)
        ");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CoverType_Update
                @id int,
                @name varchar(50)
                As
                Update coverTypes set name=@name where id=@id
        ");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CoverType_Delete
                @id int
                As
                delete coverTypes where id=@id;
        ");
            migrationBuilder.Sql(@"CREATE PROCEDURE SP_CoverType_GetCoverTypes                
                As
                select * from coverTypes
        ");
             migrationBuilder.Sql(@"CREATE PROCEDURE SP_CoverType_GetCoverType
                @id int
                As
                select * from coverTypes where id=@id
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE SP_CoverType_GetCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE SP_CoverType_Create");
            migrationBuilder.Sql(@"DROP PROCEDURE SP_CoverType_Delete");
            migrationBuilder.Sql(@"DROP PROCEDURE SP_CoverType_GetCoverTypes");
            migrationBuilder.Sql(@"DROP PROCEDURE SP_CoverType_Update");

        }
    }
}
