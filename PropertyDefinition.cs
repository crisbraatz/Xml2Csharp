using System.Xml;

namespace Xml2CSharp;

public class PropertyDefinition(
    string propertyName,
    XmlNodeType xmlNodeType,
    bool hasAttributes = false,
    bool hasElements = false,
    int count = 0)
{
    public int Count { get; } = count;

    public string PropertyType { get; } = count switch
    {
        0 or 1 when !hasAttributes && !hasElements => "string?",
        1 when hasAttributes || hasElements => $"{propertyName.ToPascalCase()}Dto?",
        _ when propertyName == "string" => "Collection<string>?",
        _ when propertyName != "string" => $"Collection<{propertyName.ToPascalCase()}Dto>?"
    };

    public string PropertyXmlAnnotation { get; } = xmlNodeType switch
    {
        XmlNodeType.Attribute => "    [XmlAttribute(",
        XmlNodeType.Element => "    [XmlElement("
    };
}