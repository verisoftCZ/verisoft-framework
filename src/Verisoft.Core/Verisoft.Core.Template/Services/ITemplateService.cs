namespace Verisoft.Core.Template.Services;

/// <summary>
/// Service for template handling
/// </summary>
public interface ITemplateService
{
    /// <summary>
    /// Populates the specified template string with the provided data.
    /// </summary>
    /// <param name="template">A <see cref="string"/> containing placeholders to be replaced.</param>
    /// <param name="templateData">An object that supplies values for the template placeholders.</param>
    /// <returns>A <see cref="string"/> representing the template with all placeholders replaced by the corresponding values from the template data.</returns>
    /// <exception cref="KeyNotFoundException">Throws when one or more placeholders were not satisfied.</exception>
    public string FillTemplate(string template, object templateData);
}