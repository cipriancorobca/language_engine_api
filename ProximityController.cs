using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/proximity")]
public class ProximityController : ControllerBase
{
    private readonly ExcelService _excelService;

    public ProximityController(ExcelService excelService)
    {
        _excelService = excelService;
    }

    [HttpPost("query")]
    public IActionResult Query([FromBody] QueryRequest request)
    {
        if (request == null || request.FromLanguages == null || string.IsNullOrEmpty(request.ToLanguage))
        {
            return BadRequest("Invalid request: fromLanguages and toLanguage are required.");
        }

        try
        {
            var results = _excelService.QueryProximity(request.FromLanguages, request.ToLanguage);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}