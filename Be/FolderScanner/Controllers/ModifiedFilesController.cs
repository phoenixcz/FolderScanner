using AutoMapper;
using FolderScanner.DTOs;
using Microsoft.AspNetCore.Mvc;
using FolderScanner.Interfaces;

namespace FolderScanner.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModifiedFilesController : ControllerBase
{
    private readonly IModifiedFilesOrchestrator _scanFolderOrchestrator;
    private readonly IMapper _mapper;
    private readonly ILogger<ModifiedFilesController> _logger;

    public ModifiedFilesController(
        IModifiedFilesOrchestrator scanFolderOrchestrator,
        IMapper mapper,
        ILogger<ModifiedFilesController> logger
    )
    {
        _scanFolderOrchestrator = scanFolderOrchestrator;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("{path}")]
    public IActionResult GetModifiedFiles(string path)
    {
        _logger.LogInformation("Started getting of modified files for path: {Path}", path);

        var result = _scanFolderOrchestrator.ScanFolder(path);
        var dto = _mapper.Map<IReadOnlyCollection<ModifiedFileDto>>(result);

        _logger.LogInformation("Finished getting of modified files for path: {Path}", path);
        return Ok(dto);
    }
}