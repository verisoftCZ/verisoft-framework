using Mustache;

namespace Verisoft.Core.Template.Services;

public class TemplateService : ITemplateService
{
    public string FillTemplate(string template, object templateData)
    {
        FormatCompiler compiler = new()
        {
            RemoveNewLines = false
        };
        Generator generator = compiler.Compile(template);
        return generator.Render(templateData);
    }
}