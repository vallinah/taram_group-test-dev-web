using backend.DTOs;
using backend.Repositories;
using backend.Validators;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactsController : ControllerBase
{
    private readonly IContactRepository _repository;


    public ContactsController(
        IContactRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetContacts(int page = 1, int pageSize = 10, string? search = null, string? sort = null)
    {
        var contacts = await _repository.GetAllAsync();


        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();

            contacts = contacts
                .Where(c =>
                    c.Societe.ToLower().Contains(search)
                    ||
                    c.NomContact.ToLower().Contains(search)
                    ||
                    (c.Email != null &&
                    c.Email.ToLower().Contains(search))
                )
                .ToList();
        }


        if (sort == "societe")
        {
            contacts = contacts
                .OrderBy(c => c.Societe)
                .ToList();
        }
        else if (sort == "date")
        {
            contacts = contacts
                .OrderByDescending(c => c.DateCreation)
                .ToList();
        }


        var totalCount = contacts.Count;


        contacts = contacts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();


        var result = new PagedResultDto<ContactDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = contacts.Select(c => new ContactDto
            {
                Id = c.Id,
                ExternalId = c.ExternalId,
                Societe = c.Societe,
                NomContact = c.NomContact,
                Email = c.Email,
                Telephone = c.Telephone,
                DateCreation = c.DateCreation,
                CaAnnuel = c.CaAnnuel
            }).ToList()
        };


        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetContact(string id)
    {
        var contact = await _repository.GetByIdAsync(id);


        if (contact == null)
        {
            return NotFound(new
            {
                message = "Contact introuvable"
            });
        }


        return Ok(
            contact
        );
    }
    

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(string id, UpdateContactDto dto)
    {
        var contact = await _repository.GetByIdAsync(id);


        if(contact == null)
        {
            return NotFound(new
            {
                message = "Contact introuvable"
            });
        }


        var error = ContactValidator.Validate(dto);


        if(error != null)
        {
            return BadRequest(new
            {
                message = error
            });
        }


        contact.Societe = dto.Societe;
        contact.NomContact = dto.NomContact;
        contact.Email = dto.Email?
            .Trim()
            .ToLower();

        contact.Telephone = dto.Telephone?
            .Trim()
            .Replace(" ", "")
            .Replace(".", "")
            .Replace("-", "");

        contact.CaAnnuel = dto.CaAnnuel;

        contact.UpdatedAt = DateTime.UtcNow;


        await _repository.UpdateAsync(contact);


        return Ok(
            contact
        );
    }
}