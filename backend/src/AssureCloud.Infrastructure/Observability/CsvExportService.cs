using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Infrastructure;

public class CsvExportService : Application.Abstractions.ICsvExportService
{
    public Task<string> ExportToCsvAsync<T>(IEnumerable<T> data, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var sb = new StringBuilder();
        var properties = typeof(T).GetProperties();

        var header = string.Join(",", properties.Select(p => $"\"{p.Name}\""));
        sb.AppendLine(header);

        foreach (var item in data)
        {
            var values = properties.Select(p => $"\"{EscapeCsvValue(Convert.ToString(p.GetValue(item) ?? string.Empty))}\"");
            sb.AppendLine(string.Join(",", values));
        }

        return Task.FromResult(sb.ToString());
    }

    public Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, CancellationToken ct = default)
    {
        throw new NotImplementedException("Excel export requires ClosedXML package.");
    }

    private static string EscapeCsvValue(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        {
            value = value.Replace("\"", "\"\"");
        }
        return value;
    }
}
