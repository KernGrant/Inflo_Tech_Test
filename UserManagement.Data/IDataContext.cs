using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagement.Data;

public interface IDataContext
{
    /// <summary>
    /// Get a list of items
    /// </summary>
    Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class;


    /// <summary>
    /// Create a new item
    /// </summary>
    Task CreateAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Update an existing item
    /// </summary>
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Delete an item
    /// </summary>
    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
}
