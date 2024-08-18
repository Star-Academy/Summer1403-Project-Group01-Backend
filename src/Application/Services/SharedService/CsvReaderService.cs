using System.Globalization;
using Application.DTOs.TransactionCsv;
using CsvHelper;
using CsvHelper.Configuration;

namespace Application.Services.SharedService;

public static class CsvReaderService
{
    public static List<T> ReadFromCsv<T>(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        });

        var records = csv.GetRecords<T>().ToList();
        return records;
    }
}