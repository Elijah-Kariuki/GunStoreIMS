using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GunStoreIMS.Domain.Models;

namespace GunStoreIMS.Abstractions.Interfaces // Or GunStoreIMS.Persistence.Interfaces
{
    /// <summary>
    /// Defines the repository interface for accessing DealerRecord data.
    /// </summary>
    public interface IDealerRecordRepository
    {
        /// <summary>
        /// Gets a DealerRecord by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the dealer record.</param>
        /// <returns>The DealerRecord if found; otherwise, null.</returns>
        Task<DealerRecord?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all DealerRecords.
        /// </summary>
        /// <returns>An enumerable collection of all DealerRecords.</returns>
        Task<IEnumerable<DealerRecord>> GetAllAsync();

        /// <summary>
        /// Finds DealerRecords matching the provided trade name (supports partial matches).
        /// </summary>
        /// <param name="tradeName">The trade name to search for.</param>
        /// <returns>An enumerable collection of matching DealerRecords.</returns>
        Task<IEnumerable<DealerRecord>> FindByTradeNameAsync(string tradeName);

        /// <summary>
        /// Finds DealerRecords matching the provided FFL number.
        /// </summary>
        /// <param name="fflNumber">The FFL number to search for.</param>
        /// <returns>An enumerable collection of matching DealerRecords.</returns>
        Task<IEnumerable<DealerRecord>> FindByFflNumberAsync(string fflNumber);

        /// <summary>
        /// Adds a new DealerRecord to the repository.
        /// </summary>
        /// <param name="dealerRecord">The DealerRecord to add.</param>
        Task AddAsync(DealerRecord dealerRecord);

        /// <summary>
        /// Updates an existing DealerRecord.
        /// </summary>
        /// <param name="dealerRecord">The DealerRecord with updated values.</param>
        /// <returns>True if the update was successful; otherwise, false (e.g., not found).</returns>
        Task<bool> UpdateAsync(DealerRecord dealerRecord);

        /// <summary>
        /// Deletes a DealerRecord by its unique identifier.
        /// </summary>
        /// <param name="id">The unique ID of the dealer record to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false (e.g., not found).</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Checks if a DealerRecord exists with the given ID.
        /// </summary>
        /// <param name="id">The unique ID to check.</param>
        /// <returns>True if a record exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// Saves changes made to the repository.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync();
    }
}