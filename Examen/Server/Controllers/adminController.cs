using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly Service _service;

    public AdminController(Service service)
    {
        _service = service;
    }

    [HttpPost("execute-query")]
    public async Task<IActionResult> ExecuteQuery([FromBody] QueryRequest request)
    {
        try
        {
            var result = await _service.ExecuteSqlQueryAsync(request.SqlQuery);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

public class QueryRequest
{
    public string SqlQuery { get; set; } = string.Empty;
}