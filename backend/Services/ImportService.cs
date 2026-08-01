using backend.DTOs;
using backend.Models;
using backend.Repositories;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace backend.Services;

public class ImportService : IImportService
{
    private readonly IContactRepository _repository;


    public ImportService(IContactRepository repository)
    {
        _repository = repository;
    }

    private string? NormalizePhone(string? phone)
    {
        if(string.IsNullOrWhiteSpace(phone))
            return null;


        phone = phone.Trim();


        phone = Regex.Replace(
            phone,
            @"[ .-]",
            ""
        );


        return phone;
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(
            email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
        );
    }
    
    private DateTime ParseDate(string? date)
    {
        if(string.IsNullOrWhiteSpace(date))
        {
            throw new Exception(
                "Date de création absente"
            );
        }


        string[] formats =
        {
            "yyyy-MM-dd",
            "dd/MM/yyyy"
        };


        if(DateTime.TryParseExact(
            date,
            formats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var result))
        {
            return result;
        }


        throw new Exception(
            "Date invalide"
        );
    }

    private decimal? ParseCa(object? ca)
    {
        if(ca == null)
            return null;


        var value = ca.ToString();


        if(decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            if(result < 0)
            {
                throw new Exception(
                    "Chiffre d'affaires négatif"
                );
            }

            return result;
        }


        throw new Exception(
            "Chiffre d'affaires invalide"
        );
    }

    private async Task ProcessContactAsync(ErpContactDto erp, ImportReportDto report)
    {

        if (string.IsNullOrWhiteSpace(erp.IdErp))
        {
            throw new Exception(
                "Identifiant ERP absent"
            );
        }


        if (string.IsNullOrWhiteSpace(erp.Societe))
        {
            throw new Exception(
                "Société obligatoire"
            );
        }


        if (string.IsNullOrWhiteSpace(erp.Contact))
        {
            throw new Exception(
                "Nom du contact obligatoire"
            );
        }

        if (string.IsNullOrWhiteSpace(erp.Email) && string.IsNullOrWhiteSpace(erp.Telephone))
        {
            throw new Exception(
                "Email ou téléphone obligatoire"
            );
        }

        if (!string.IsNullOrWhiteSpace(erp.Email) && !IsValidEmail(erp.Email.Trim()))
        {
            throw new Exception(
                "Email invalide"
            );
        }


        var email = erp.Email?
            .Trim()
            .ToLower();


        var telephone = NormalizePhone(
            erp.Telephone
        );

        var contact = new Contact
        {
            ExternalId = erp.IdErp,
            Societe = erp.Societe!,
            NomContact = erp.Contact!,
            Email = email,
            Telephone = telephone,
            DateCreation = ParseDate(erp.DateCreation),
            CaAnnuel = ParseCa(erp.CaAnnuel)
        };

        var existing = await _repository
            .GetByExternalIdAsync(contact.ExternalId);


        if(existing == null)
        {
            await _repository.AddAsync(contact);
            report.Created++;
        }
        else
        {
            existing.Societe = contact.Societe;
            existing.NomContact = contact.NomContact;
            existing.Email = contact.Email;
            existing.Telephone = contact.Telephone;
            existing.CaAnnuel = contact.CaAnnuel;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);

            report.Updated++;
        }

    }


    public async Task<ImportReportDto> ImportAsync()
    {
        var report = new ImportReportDto();


        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "export_erp_clients.json"
        );


        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "Fichier export ERP introuvable"
            );
        }


        var json = await File.ReadAllTextAsync(filePath);


        var erpContacts = JsonSerializer.Deserialize<List<ErpContactDto>>(json);


        if (erpContacts == null)
        {
            return report;
        }


        for(int i = 0; i < erpContacts.Count; i++)
        {
            var erpContact = erpContacts[i];

            try
            {
                await ProcessContactAsync(
                    erpContact,
                    report
                );
            }
            catch(Exception ex)
            {
                report.Rejected++;

                report.Errors.Add(
                    new ImportErrorDto
                    {
                        Line = i + 1,
                        ExternalId = erpContact.IdErp ?? "UNKNOWN",
                        Reason = ex.Message
                    }
                );
            }
        }


        return report;
    }
}