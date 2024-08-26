using Application.DTOs.Edge;
using Domain.Entities;
using Newtonsoft.Json;

namespace Application.Mappers;

public static class EdgeMapper
{
    public static Edge ToEdge(this EdgeCsvModel csvModel, List<EdgeAttribute> availableAttributes)
    {
        // Parse the Attributes JSON string into a dictionary
        var attributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(csvModel.AttributesJson);

        // Convert the dictionary into a list of EdgeAttributeValue entities
        var attributeValues = attributes.Select(attr =>
        {
            // Find the corresponding EdgeAttribute by label
            var attribute = availableAttributes.FirstOrDefault(a => a.Label == attr.Key);
            if (attribute != null)
            {
                return new EdgeAttributeValue
                {
                    EdgeAttributeId = attribute.Id,
                    Value = attr.Value
                };
            }
            return null; // Handle case where the attribute is not found (optional)
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