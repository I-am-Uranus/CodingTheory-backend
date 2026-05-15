using CodingTheory.Models;
using CodingTheory.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodingTheory.Controllers;

[ApiController]
[Route("api/huffman")]
public class HuffmanController : ControllerBase
{
    private readonly HuffmanService _huffmanService;

    public HuffmanController(HuffmanService huffmanService)
    {
        _huffmanService = huffmanService;
    }

    [HttpPost("encode")]
    public IActionResult Encode([FromBody] HuffmanRequest request)
    {
        try
        {
            var result = _huffmanService.Encode(request.Text);
            return Ok(result);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
    }
}