using System.Globalization;
using System.Reflection;
using Application.DTOs.Edge;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace Application.Services.SharedService;

public static class CsvReaderService
{
    public static List<EdgeCsvModel> ReadFromCsv(string filePath, string sourceColumn, string destinationColumn, string typeIdColumn, string idColumn)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        });

        var records = new List<EdgeCsvModel>();
        var headers = csv.Context.Reader.HeaderRecord;

        while (csv.Read())
        {
            var record = new EdgeCsvModel();

            // Use the dynamic column names passed as parameters
            record.SourceValue = csv.GetField<long>(sourceColumn);
            record.DestinationValue = csv.GetField<long>(destinationColumn);
            record.Id = csv.GetField<long>(idColumn);
            record.TypeId = csv.GetField<long>(typeIdColumn);

            // Handle dynamic attributes
            var attributes = new Dictionary<string, string>();
            foreach (var header in headers)
            {
                if (!IsKnownProperty(header, new List<string> { sourceColumn, destinationColumn, typeIdColumn, idColumn }))
                {
                    var value = csv.GetField<string>(header);
                    attributes[header] = value;
                }
            }
            record.AttributesJson = JsonConvert.SerializeObject(attributes);

            records.Add(record);
        }

        return records;
    }

    private static bool IsKnownProperty(string header, List<string> knownProperties)
    {
        return knownProperties.Contains(header);
    }


}