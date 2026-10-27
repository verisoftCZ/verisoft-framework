using System.Drawing;
using Verisoft.Core.Export.Extensions;
using Verisoft.Core.Export.Settings;

namespace Verisoft.Core.Export.Builders;

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
        headerSettings.Color = color.ToHexRgb();
        return this;
    }

    public HeaderSettings Build() => headerSettings;
}
