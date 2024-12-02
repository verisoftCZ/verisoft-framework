using System.Drawing;

namespace Verisoft.Core.Excel.Extensions;

internal static class ColorExtensions
{
    public static string ToHexRGB(this Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }
}
