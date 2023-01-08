using System.Diagnostics.CodeAnalysis;
using Core.Application.Exceptions;
using Core.Application.Interfaces.Repositories;
using Core.Application.Resources;
using Core.Domain.Entities;
using Dapper;
using Microsoft.Extensions.Configuration;
using static Dapper.SqlMapper;

namespace Infra.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TodoRepositoryAsync : GenericRepositoryAsync<Todo, int>, ITodoRepositoryAsync
    {
        public TodoRepositoryAsync(IConfiguration configuration)
        : base(configuration) { }

        public async Task<Todo?> CreateAsync(Todo entity)
        {
            try
            {
                string insertSql = @"INSERT INTO todo (title, done)
                                    VALUES(@title, @done)
                                    RETURNING id;";

                var id = await _connection.ExecuteScalarAsync<int>(insertSql,
                new
                {
                    title = entity.Title,
                    done = entity.Done
                });

                if (id > decimal.Zero)
                    return await base.GetAsync(id);

                return await Task.FromResult<Todo?>(default);
            }
            catch (Exception ex)
            {
                throw new AppException(Msg.DATA_BASE_SERVER_ERROR_TXT, ex);
            }
        }
    }
}