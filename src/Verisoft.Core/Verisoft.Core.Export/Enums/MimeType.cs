using System.Net.Mime;

namespace Verisoft.Core.Export.Enums;

public static class MimeType
{
    public const string Csv = MediaTypeNames.Text.Csv;
    public const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const string Json = MediaTypeNames.Application.Json;
    public const string Xml = MediaTypeNames.Application.Xml;
}