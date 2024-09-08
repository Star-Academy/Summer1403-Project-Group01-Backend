using Application.DTOs.Transaction;
using Domain.Entities;

namespace Application.Mappers;
public static class TransactionMapper
{
    public static Transaction ToTransaction(this TransactionCsvModel csvModel, long fileId)
    {
        var date = DateOnly.Parse(csvModel.Date).ToDateTime(TimeOnly.Parse(csvModel.Time));
        var utcDate = DateTime.SpecifyKind(date, DateTimeKind.Utc);
        return new Transaction
        {
            TransactionId = csvModel.TransactionId,
            SourceAccountId = csvModel.SourceAccount,
            DestinationAccountId = csvModel.DestinationAccount,
            Amount = csvModel.Amount,
            Date = utcDate,
            Type = csvModel.Type,
            TrackingId = csvModel.TrackingId,
            FileId = fileId
        };
    }
}