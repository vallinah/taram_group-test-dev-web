namespace backend.DTOs;

public class UpdateContactDto
{
    public string Societe { get; set; } = null!;

    public string NomContact { get; set; } = null!;

    public string? Email { get; set; }

    public string? Telephone { get; set; }

    public decimal? CaAnnuel { get; set; }
}