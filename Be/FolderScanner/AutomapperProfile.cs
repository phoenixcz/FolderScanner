using AutoMapper;
using FolderScanner.DTOs;
using FolderScanner.Models;

namespace FolderScanner;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<ModifiedFileModel, ModifiedFileDto>();
    }
}