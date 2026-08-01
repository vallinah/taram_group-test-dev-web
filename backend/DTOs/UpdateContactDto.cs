using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

public class UpdateContactDto
{
    [Required]
    public string Societe { get; set; } = null!;


    [Required]
    public string NomContact { get; set; } = null!;


    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;


    public string? Telephone { get; set; }


    public DateTime DateCreation { get; set; }


    [Range(0, double.MaxValue)]
    public decimal? CaAnnuel { get; set; }
}