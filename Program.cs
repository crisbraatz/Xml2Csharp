using System.Text;
using System.Xml.Linq;
using Xml2CSharp;

var classDefinitionDict = new Dictionary<string, ClassDefinition>();

Build(XDocument.Load("/path/file.xml").Root!, classDefinitionDict);

var stringBuilder = new StringBuilder();

foreach (var classDefinition in classDefinitionDict.Values)
    stringBuilder.Append(classDefinition);

File.WriteAllText("/path/output.txt", stringBuilder.ToString());

return;

static void Build(XElement element, Dictionary<string, ClassDefinition> classDefinitionDict)
{
    if (!classDefinitionDict.ContainsKey(element.Name.LocalName))
        classDefinitionDict[element.Name.LocalName] = new ClassDefinition(
            element.Name.LocalName, !string.IsNullOrWhiteSpace(element.Value) && !element.HasElements);

    var classDefinition = classDefinitionDict[element.Name.LocalName];

    foreach (var attribute in element.Attributes())
        classDefinition.AddProperty(attribute.Name.LocalName, attribute.NodeType);

    foreach (var childElement in element.Elements())
    {
        if (childElement.HasAttributes || childElement.HasElements)
            Build(childElement, classDefinitionDict);

        classDefinition.AddProperty(
            childElement.Name.LocalName,
            childElement.NodeType,
            childElement.HasAttributes,
            childElement.HasElements,
            element.Elements(childElement.Name).Count());
    }
}