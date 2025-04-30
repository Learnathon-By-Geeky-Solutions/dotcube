using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeltaShareApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FileController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public FileController(IWebHostEnvironment env)
    {
        _env = env;
    }

    private string GetUserFolderPath(string userId)
    {
        var folder = Path.Combine(_env.ContentRootPath, "UploadedFiles", userId);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return folder;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var userFolder = GetUserFolderPath(userId);

        var filePath = Path.Combine(userFolder, file.FileName);

        using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { file.FileName, file.Length });
    }

    [HttpGet("download/{fileName}")]
    public IActionResult DownloadFile(string fileName)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var userFolder = GetUserFolderPath(userId);

        var filePath = Path.Combine(userFolder, fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var mimeType = "application/octet-stream";
        return PhysicalFile(filePath, mimeType, fileName);
    }

    [HttpGet("info/{fileName}")]
    public IActionResult GetFileInfo(string fileName)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var userFolder = GetUserFolderPath(userId);

        var filePath = Path.Combine(userFolder, fileName);

        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var info = new FileInfo(filePath);

        return Ok(new
        {
            info.Name,
            info.Length,
            info.CreationTimeUtc,
            info.LastWriteTimeUtc
        });
    }

    [HttpGet("list")]
    public IActionResult ListFiles()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var userFolder = GetUserFolderPath(userId);

        var files = Directory.GetFiles(userFolder)
            .Select(f => Path.GetFileName(f))
            .ToList();

        return Ok(files);
    }
}
