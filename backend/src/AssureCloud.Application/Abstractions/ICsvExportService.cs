using System.Collections.Generic;
using System.Threading.Tasks;

namespace AssureCloud.Application.Abstractions;

public interface ICsvExportService
{
    Task<string> ExportToCsvAsync<T>(IEnumerable<T> data, CancellationToken ct = default);
    Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, CancellationToken ct = default);
}
