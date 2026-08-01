namespace backend.DTOs;

public class ContactDto
{
    public string Id { get; set; } = null!;

    public string ExternalId { get; set; } = null!;

    public string Societe { get; set; } = null!;

    public string NomContact { get; set; } = null!;

    public string? Email { get; set; }

    public string? Telephone { get; set; }

    public DateTime DateCreation { get; set; }

    public decimal? CaAnnuel { get; set; }
}