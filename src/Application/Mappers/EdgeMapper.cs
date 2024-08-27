using Application.DTOs.Edge;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Mappers;

public static class EdgeMapper
{
    public static Edge ToEdge(this EdgeCsvModel csvModel, List<EdgeAttribute> availableAttributes)
    {
        var attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(csvModel.AttributesJson);

        var attributeValues = attributes.Select(attr =>
        {
            var attribute = availableAttributes.FirstOrDefault(a => a.Label == attr.Key);
            if (attribute != null)
            {
                return new EdgeAttributeValue
                {
                    EdgeAttributeId = attribute.Id,
                    Value = attr.Value
                };
            }
            return null;
        }).Where(av => av != null).ToList();

        return new Edge
        {
            Id = csvModel.Id,
            SourceValue = csvModel.SourceValue,
            DestinationValue = csvModel.DestinationValue,
            TypeId = csvModel.TypeId,
            AttributeValues = attributeValues
        };
    }


}