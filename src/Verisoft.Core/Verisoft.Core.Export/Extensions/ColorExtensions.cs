using System.Drawing;

namespace Verisoft.Core.Export.Extensions;

internal static class ColorExtensions
{
    public static string ToHexRgb(this Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
}
