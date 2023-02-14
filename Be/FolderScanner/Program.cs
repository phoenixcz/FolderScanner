using System.IO.Abstractions;
using System.Text.Json.Serialization;
using FolderScanner;
using FolderScanner.Interfaces;
using FolderScanner.Orchestrators;
using FolderScanner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper((opt) => opt.AddProfile<AutomapperProfile>());

builder.Services.AddScoped<IFileSystem, FileSystem>();
builder.Services.AddScoped<IFileSystemService, FileSystemService>();
builder.Services.AddScoped<IFileCompareService, FileCompareService>();
builder.Services.AddSingleton<IFileMetadataRepository, FileMetadataRepository>();

builder.Services.AddScoped<IModifiedFilesOrchestrator, ModifiedFilesOrchestrator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseCors(b => b
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.Run();