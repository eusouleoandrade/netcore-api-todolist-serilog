using System.Diagnostics.CodeAnalysis;
using Core.Application.Exceptions;
using Core.Application.Resources;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Infra.Persistence.Bootstraps
{
    public static class TodoBootstrap
    {
        [ExcludeFromCodeCoverage]
        public static void Setup(SqliteConnection connection)
        {
            try
            {
                string selectTableNameSql = @"SELECT name
                                            FROM sqlite_master
                                            WHERE type='table' AND name = 'Todo';";

                var table = connection.Query<string>(selectTableNameSql);

                var tableName = table.FirstOrDefault();

                if (!string.IsNullOrEmpty(tableName) && tableName == "Todo")
                    return;

                string createTableSql = @"CREATE TABLE Todo (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Title VARCHAR(60) NOT NULL,
                                        Done BOOLEAN NOT NULL);";

                connection.Execute(createTableSql);
            }
            catch (Exception ex)
            {
                throw new AppException(Msg.DATA_BASE_SERVER_ERROR_TXT, ex);
            }
        }
    }
}