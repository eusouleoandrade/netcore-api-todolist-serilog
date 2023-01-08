using System.Diagnostics.CodeAnalysis;
using Core.Application.Exceptions;
using Core.Application.Interfaces.Repositories;
using Core.Application.Resources;
using Dapper.Contrib.Extensions;
using Infra.Persistence.Configs;
using Microsoft.Extensions.Configuration;

namespace Infra.Persistence.Repositories
{
    [ExcludeFromCodeCoverage]
    public class GenericRepositoryAsync<TEntity, TId> : ConnectionConfig, IGenericRepositoryAsync<TEntity, TId>
        where TEntity : class
        where TId : struct
    {
        public GenericRepositoryAsync(IConfiguration configuration)
            : base(configuration) { }

        public virtual async Task<TId?> InsertAsync(TEntity entity)
        {
            try
            {
                var id = await _connection.InsertAsync<TEntity>(entity);

                return (TId)Convert.ChangeType(id, typeof(TId));
            }
            catch (Exception ex)
            {
                throw new AppException(Msg.DATA_BASE_SERVER_ERROR_TXT, ex);
            }
        }

        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            try
            {
                return await _connection.DeleteAsync<TEntity>(entity);
            }
            catch (Exception ex)
            {
                throw new AppException(Msg.DATA_BASE_SERVER_ERROR_TXT, ex);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _connection.GetAllAsync<TEntity>();
            }
            catch (Exception ex)
            {
                throw new AppException(Msg.DATA_BASE_SERVER_ERROR_TXT, ex);
            }
        }

        public virtual async Task<TEntity> GetAsync(TId id)
        {
            try
            {
                return await _connection.GetAsync<TEntity>(id);
            }
            catch (Exception ex)
            {
                throw new AppException(Msg.DATA_BASE_SERVER_ERROR_TXT, ex);
            }
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                return await _connection.UpdateAsync<TEntity>(entity);
            }
            catch (Exception ex)
            {
                throw new AppException(Msg.DATA_BASE_SERVER_ERROR_TXT, ex);
            }
        }
    }
}