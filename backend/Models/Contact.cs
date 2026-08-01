namespace backend.Models;

public class Contact
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string ExternalId { get; set; } = null!;

    public string Societe { get; set; } = null!;

    public string NomContact { get; set; } = null!;

    public string? Email { get; set; } 

    public string? Telephone { get; set; }

    public DateTime DateCreation { get; set; }

    public decimal? CaAnnuel { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}