namespace Verisoft.Core.Pdf.Services;
/// <summary>
/// Service for pdf handling
/// </summary>
public interface IPdfService
{
    /// <summary>
    /// Generates a PDF document from the provided HTML string.
    /// </summary>
    /// <param name="html">A <see cref="string"/> representing the HTML used to generate the PDF.</param>
    /// <returns>A <see cref="Stream"/> representing the PDF document.</returns>
    public Task<Stream> GenerateFromHtml(string html);
}