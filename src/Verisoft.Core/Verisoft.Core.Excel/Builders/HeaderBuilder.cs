using System.Drawing;
using Verisoft.Core.Excel.Extensions;
using Verisoft.Core.Excel.Settings;

namespace Verisoft.Core.Excel.Builders;

public class HeaderBuilder
{
    private readonly HeaderSettings headerSettings = new();

    public HeaderBuilder Background(string color)
    {
        headerSettings.Color = color;
        return this;
    }

    public HeaderBuilder Background(Color color)
    {
        headerSettings.Color = color.ToHexRGB();
        return this;
    }

    internal HeaderSettings Build()
    {
        return headerSettings;
    }
}
