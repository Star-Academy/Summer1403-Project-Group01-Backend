using Application.Services.SharedService;
using CsvHelper.Configuration.Attributes;

namespace Application.DTOs.Transaction;

public class TransactionCsvModel
{
    [Name("TransactionId", "TransactionID")]
    public long TransactionId { get; set; }
    [Name("SourceAccount", "SourceAcount")]
    public long SourceAccount { get; set; }
    [Name("DestinationAccount","DestiantionAccount")]
    public long DestinationAccount { get; set; }
    [Name("Amount")]
    public decimal Amount { get; set; }
    [Name("Date")]
    public string Date { get; set; } = string.Empty;
    [Name("Time")]
    public string Time { get; set; } = string.Empty;
    [Name("Type")]
    public string Type { get; set; } = string.Empty;
    [Name("TrackingId", "TrackingID")]
    public long TrackingId { get; set; }
}