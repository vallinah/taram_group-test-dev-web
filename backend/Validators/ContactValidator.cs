using System.Text.RegularExpressions;
using backend.DTOs;

namespace backend.Validators;

public static class ContactValidator
{
    public static string? Validate(UpdateContactDto dto)
    {
        if(string.IsNullOrWhiteSpace(dto.Societe))
        {
            return "Société obligatoire";
        }


        if(string.IsNullOrWhiteSpace(dto.NomContact))
        {
            return "Nom du contact obligatoire";
        }


        if(
            string.IsNullOrWhiteSpace(dto.Email)
            &&
            string.IsNullOrWhiteSpace(dto.Telephone)
        )
        {
            return "Email ou téléphone obligatoire";
        }


        if(
            !string.IsNullOrWhiteSpace(dto.Email)
            &&
            !IsValidEmail(dto.Email.Trim())
        )
        {
            return "Email invalide";
        }


        if(
            !string.IsNullOrWhiteSpace(dto.Telephone)
            &&
            !IsValidPhone(dto.Telephone)
        )
        {
            return "Téléphone invalide";
        }


        if(dto.CaAnnuel < 0)
        {
            return "Chiffre d'affaires négatif";
        }


        return null;
    }


    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(
            email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
        );
    }


    private static bool IsValidPhone(string phone)
    {
        var normalized = Regex.Replace(
            phone,
            @"[ .-]",
            ""
        );


        return Regex.IsMatch(
            normalized,
            @"^\+?[0-9]{10,15}$"
        );
    }
}