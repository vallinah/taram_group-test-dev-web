using backend.DTOs;
using backend.Models;
using backend.Repositories;
using System.Text.Json;

namespace backend.Services;

public class ImportService : IImportService
{
    private readonly IContactRepository _repository;


    public ImportService(
        IContactRepository repository)
    {
        _repository = repository;
    }


    public async Task<ImportReportDto> ImportAsync()
    {
        var report = new ImportReportDto();


        return report;
    }
}