using Application.Services.SharedService;
using CsvHelper.Configuration.Attributes;

namespace Application.DTOs.TransactionCsv;

public class TransactionCsvModel
{
    public int TransactionId { get; set; }
    public int SourceAccount { get; set; }
    public int DestinationAccount { get; set; }
    public decimal Amount { get; set; }
    [TypeConverter(typeof(PersianDateConverter))]
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
}