using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/import")]
public class ImportController : ControllerBase
{
    private readonly IImportService _importService;


    public ImportController(IImportService importService)
    {
        _importService = importService;
    }


    [HttpPost]
    public async Task<IActionResult> Import()
    {
        var report = await _importService.ImportAsync();

        return Ok(report);
    }
}