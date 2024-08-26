using Domain.Entities;
using Web.DTOs.Edge;

namespace Web.Mappers;

public static class EdgeMapper
{
    public static List<EdgeDto> ToGotAllEdgesDto(this List<Edge> edges)
    {
        return edges.Select(edge => new EdgeDto
        {
            Id = edge.Id,
            SourceValue = edge.SourceValue,
            DestinationValue = edge.DestinationValue,
            TypeId = edge.TypeId,
            TypeLabel = edge.Type?.Label ?? String.Empty,
            Attributes = edge.AttributeValues?.ToDictionary(
                av => av.EdgeAttribute?.Label ?? string.Empty,
                av => av.Value
            ) ?? new Dictionary<string, string>()
        }).ToList();
    }
}