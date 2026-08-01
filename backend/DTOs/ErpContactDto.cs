using System.Text.Json.Serialization;

namespace backend.DTOs;

public class ErpContactDto
{
    [JsonPropertyName("id_erp")]
    public string? IdErp { get; set; }


    [JsonPropertyName("societe")]
    public string? Societe { get; set; }


    [JsonPropertyName("contact")]
    public string? Contact { get; set; }


    [JsonPropertyName("email")]
    public string? Email { get; set; }


    [JsonPropertyName("telephone")]
    public string? Telephone { get; set; }


    [JsonPropertyName("date_creation")]
    public string? DateCreation { get; set; }


    [JsonPropertyName("ca_annuel")]
    public object? CaAnnuel { get; set; }
}