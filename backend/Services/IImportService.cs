using backend.DTOs;

namespace backend.Services;

public interface IImportService
{
    Task<ImportReportDto> ImportAsync();
}