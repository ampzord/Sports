namespace Sports.Tests.Shared;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

public static class DatabaseHelper
{
    public const string SqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest";

    public static Task ResetAsync(this DatabaseFacade database) =>
        database.ExecuteSqlRawAsync(
            """
            DELETE FROM Matches;
            DELETE FROM Players;
            DELETE FROM Teams;
            DELETE FROM Leagues;
            DBCC CHECKIDENT ('Matches', RESEED, 0);
            DBCC CHECKIDENT ('Players', RESEED, 0);
            DBCC CHECKIDENT ('Teams', RESEED, 0);
            DBCC CHECKIDENT ('Leagues', RESEED, 0);
            """);
}
