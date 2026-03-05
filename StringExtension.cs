namespace Xml2CSharp;

public static class StringExtension
{
    public static string ToPascalCase(this string value) =>
        string.Concat(value.Split('_', '-', '.').Select(x => char.ToUpperInvariant(x[0]) + x[1..]));
}