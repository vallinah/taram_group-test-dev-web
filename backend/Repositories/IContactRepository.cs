using backend.Models;

namespace backend.Repositories;

public interface IContactRepository
{
    Task<IEnumerable<Contact>> GetAllAsync();

    Task<Contact?> GetByIdAsync(string id);

    Task<Contact?> GetByExternalIdAsync(string externalId);

    Task AddAsync(Contact contact);

    Task UpdateAsync(Contact contact);

    Task DeleteAsync(string id);
}