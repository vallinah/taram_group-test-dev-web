namespace backend.DTOs;

public class ImportReportDto
{
    public int Created { get; set; }

    public int Updated { get; set; }

    public int Rejected { get; set; }


    public List<ImportErrorDto> Errors { get; set; } = new();
}


public class ImportErrorDto
{
    public string ExternalId { get; set; } = null!;

    public string Reason { get; set; } = null!;
}