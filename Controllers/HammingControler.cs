using CodingTheory.Models;
using CodingTheory.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodingTheory.Controllers;

[ApiController]
[Route("api/hamming")]
public class HammingController : ControllerBase
{
    private readonly HammingService _hammingService;

    public HammingController(HammingService hammingService)
    {
        _hammingService = hammingService;
    }

    [HttpPost("run")]
    public IActionResult Run([FromBody] HammingRequest request)
    {
        try
        {
            var result = _hammingService.Run(request.Data, request.ErrorPosition);
            return Ok(result);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}