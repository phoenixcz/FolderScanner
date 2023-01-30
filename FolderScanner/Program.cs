using System.IO.Abstractions;
using FolderScanner.Interfaces;
using FolderScanner.Orchestrators;
using FolderScanner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFileSystem, FileSystem>();
builder.Services.AddScoped<IFileSystemService, FileSystemService>();
builder.Services.AddScoped<IFileCompareService, FileCompareService>();
builder.Services.AddSingleton<IFileMetadataRepository, FileMetadataRepository>();

builder.Services.AddScoped<IScanFolderOrchestrator, ScanFolderOrchestrator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();