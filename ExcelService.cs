using OfficeOpenXml;
using System.Globalization;

public class ExcelService
{
    private readonly string _filePath;

    private readonly string[] _sheets = { "Reading", "Writing", "Speaking", "Listening", "Grammar" };

    public ExcelService(string filePath)
    {
        _filePath = filePath;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public List<Dictionary<string, object>> QueryProximity(List<string> fromLanguages, string toLanguage)
    {
        var results = new List<Dictionary<string, object>>();

        using var package = new ExcelPackage(new FileInfo(_filePath));

        foreach (var fromLang in fromLanguages)
        {
            var entry = new Dictionary<string, object>
            {
                ["From"] = fromLang,
                ["To"] = toLanguage
            };

            foreach (var sheetName in _sheets)
            {
                var worksheet = package.Workbook.Worksheets[sheetName];
                if (worksheet == null)
                    continue;

                int colCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                // Find headers
                var headers = new List<string>();
                for (int c = 1; c <= colCount; c++)
                    headers.Add(worksheet.Cells[1, c].Text);

                // Find row index for "toLanguage"
                int rowIndex = -1;
                for (int r = 2; r <= rowCount; r++)
                {
                    if (worksheet.Cells[r, 1].Text.Equals(toLanguage, StringComparison.OrdinalIgnoreCase))
                    {
                        rowIndex = r;
                        break;
                    }
                }

                if (rowIndex == -1) continue;

                // Find column index for "fromLang"
                int colIndex = headers.FindIndex(h => 
                    h.Equals(fromLang, StringComparison.OrdinalIgnoreCase)) + 1;

                if (colIndex <= 0) continue;

                var rawValue = worksheet.Cells[rowIndex, colIndex].Text;
                if (string.IsNullOrWhiteSpace(rawValue))
                {
                    entry[sheetName] = null;
                }
                else
                {
                    // Normalize decimal separator
                    rawValue = rawValue.Replace(",", ".");
                    if (decimal.TryParse(rawValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var num))
                        entry[sheetName] = num;
                    else
                        entry[sheetName] = null;
                }
            }

            results.Add(entry);
        }

        return results;
    }
}