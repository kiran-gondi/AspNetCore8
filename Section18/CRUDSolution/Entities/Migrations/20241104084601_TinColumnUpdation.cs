using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class TinColumnUpdation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'GetAllPersons') AND type IN (N'P', N'PC'))
                    BEGIN
                        DROP PROCEDURE [dbo].[GetAllPersons];
                    END
                        GO
                    CREATE PROCEDURE [dbo].[GetAllPersons]
                    AS BEGIN
                        SELECT PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters, TIN 
                        FROM [dbo].[Persons]
                    END";

            migrationBuilder.Sql(sp_GetAllPersons);

            string sp_InsertPerson = @"
                    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'InsertPerson') AND type IN (N'P', N'PC'))
                    BEGIN
                        DROP PROCEDURE [dbo].[InsertPerson];
                    END
                        GO
                    CREATE PROCEDURE [dbo].[InsertPerson]
                         (@PersonID uniqueidentifier, @PersonName nvarchar(40), @Email nvarchar(40), 
                            @DateOfBirth datetime2(7), @Gender nvarchar(10), @CountryID uniqueidentifier, 
                            @Address nvarchar(1000), @ReceiveNewsLetters bit, @TIN nvarchar(40))
                    AS BEGIN
                        INSERT INTO [dbo].[Persons](PersonID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters, TIN) 
                        VALUES (@PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters, @TIN)
                    END";

            migrationBuilder.Sql(sp_InsertPerson);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"DROP PROCEDURE [dbo].[GetAllPersons]";

            migrationBuilder.Sql(sp_GetAllPersons);

            string sp_InsertPerson = @"DROP PROCEDURE [dbo].[InsertPerson]";

            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
