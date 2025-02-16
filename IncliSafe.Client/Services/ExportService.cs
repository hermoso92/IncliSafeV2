using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Threading.Tasks;

public interface IExportService
{
    Task ExportToExcel(string fileName, object data);
    Task ExportToPdf(string fileName, object data);
    Task ExportToCsv(string fileName, object data);
    Task ExportToJson(string fileName, object data);
}

public class ExportService : IExportService
{
    private readonly IJSRuntime _js;
    private readonly ILogger<ExportService> _logger;

    public ExportService(IJSRuntime js, ILogger<ExportService> logger)
    {
        _js = js;
        _logger = logger;
    }

    public async Task ExportToExcel(string fileName, object data)
    {
        try
        {
            await _js.InvokeVoidAsync("exportToExcel", fileName, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to Excel");
            throw;
        }
    }

    public async Task ExportToPdf(string fileName, object data)
    {
        try
        {
            await _js.InvokeVoidAsync("exportToPdf", fileName, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to PDF");
            throw;
        }
    }

    public async Task ExportToCsv(string fileName, object data)
    {
        try
        {
            await _js.InvokeVoidAsync("exportToCsv", fileName, data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to CSV");
            throw;
        }
    }

    public async Task ExportToJson(string fileName, object data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            await _js.InvokeVoidAsync("saveAsFile", fileName, json, "application/json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting to JSON");
            throw;
        }
    }
} 