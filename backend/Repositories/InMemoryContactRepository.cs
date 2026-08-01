using backend.Models;

namespace backend.Repositories;

public class InMemoryContactRepository : IContactRepository
{
    private readonly List<Contact> contacts = new();


    public Task<IEnumerable<Contact>> GetAllAsync()
    {
        return Task.FromResult(contacts.AsEnumerable());
    }


    public Task<Contact?> GetByIdAsync(string id)
    {
        var contact = contacts.FirstOrDefault(c => c.Id == id);

        return Task.FromResult(contact);
    }


    public Task<Contact?> GetByExternalIdAsync(string externalId)
    {
        var contact = contacts
            .FirstOrDefault(c => c.ExternalId == externalId);

        return Task.FromResult(contact);
    }


    public Task AddAsync(Contact contact)
    {
        contacts.Add(contact);

        return Task.CompletedTask;
    }


    public Task UpdateAsync(Contact contact)
    {
        var existing = contacts
            .FirstOrDefault(c => c.Id == contact.Id);


        if(existing != null)
        {
            existing.ExternalId = contact.ExternalId;
            existing.Societe = contact.Societe;
            existing.NomContact = contact.NomContact;
            existing.Email = contact.Email;
            existing.Telephone = contact.Telephone;
            existing.DateCreation = contact.DateCreation;
            existing.CaAnnuel = contact.CaAnnuel;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        return Task.CompletedTask;
    }


    public Task DeleteAsync(string id)
    {
        var contact = contacts
            .FirstOrDefault(c => c.Id == id);


        if(contact != null)
        {
            contacts.Remove(contact);
        }

        return Task.CompletedTask;
    }
}