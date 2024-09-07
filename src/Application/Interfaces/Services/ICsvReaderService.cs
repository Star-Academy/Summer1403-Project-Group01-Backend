namespace Application.Interfaces.Services;

public interface ICsvReaderService
{
    List<T> ReadFromCsv<T>(string filePath);
}