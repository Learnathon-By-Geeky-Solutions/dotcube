using Microsoft.AspNetCore.Mvc;

namespace DeltaShareApi.Controller;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;

    public FileController(ILogger<FileController> logger)
    {
        _logger = logger;
    }

    [HttpGet("upload")]
    public string Upload()
    {
        return "not implemented";
    }

    [HttpGet("download")]
    public string Download()
    {
        return "not implemented";
    }

    [HttpGet("all")]
    public string All()
    {
        return "not implemented";
    }
}
