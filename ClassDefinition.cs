using System.Text;
using System.Xml;

namespace Xml2CSharp;

public class ClassDefinition(string className, bool hasValue)
{
    private string ClassName { get; } = className;
    private readonly Dictionary<string, PropertyDefinition> _classProperties = new();
    private bool HasValue { get; } = hasValue;

    public void AddProperty(
        string propertyName,
        XmlNodeType xmlNodeType,
        bool hasAttributes = false,
        bool hasElements = false,
        int count = 0)
    {
        if (!_classProperties.TryGetValue(propertyName, out var propertyDefinition))
            _classProperties.Add(
                propertyName, new PropertyDefinition(propertyName, xmlNodeType, hasAttributes, hasElements, count));
        else if (count > propertyDefinition.Count)
            _classProperties[propertyName] =
                new PropertyDefinition(propertyName, xmlNodeType, hasAttributes, hasElements, count);
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"[XmlRoot(\"{ClassName}\")]");
        stringBuilder.AppendLine($"public class {ClassName.ToPascalCase()}Dto");
        stringBuilder.AppendLine("{");
        foreach (var propertyDefinitionKvp in _classProperties.OrderBy(x => x.Key))
            stringBuilder.AppendLine(
                $"{propertyDefinitionKvp.Value.PropertyXmlAnnotation}\"{propertyDefinitionKvp.Key}\")] public {propertyDefinitionKvp.Value.PropertyType} {propertyDefinitionKvp.Key.ToPascalCase()} {{ get; init; }}");
        if (HasValue)
            stringBuilder.AppendLine("    [XmlText] public string? Value { get; init; }");
        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }
}