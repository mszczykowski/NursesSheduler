using MediatR;
using NursesScheduler.Domain.Constants;
using OpenHtmlToPdf;
using System.Text;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Documents.Commands.GeneratePdfDocument
{
    internal sealed class GeneratePdfDocumentCommandHandler : IRequestHandler<GeneratePdfDocumentRequest, GeneratePdfDocumentResponse>
    {
        public async Task<GeneratePdfDocumentResponse> Handle(GeneratePdfDocumentRequest request, CancellationToken cancellationToken)
        {
            var documentContent = new StringBuilder();

            var siteCSSContent = File.ReadAllText(DocumentsConstatns.SiteCSSPath);
            var bulmaCSSContent = File.ReadAllText(DocumentsConstatns.BulmaCSSPath);

            documentContent.Append("<style>");
            documentContent.Append(siteCSSContent);
            documentContent.Append(bulmaCSSContent);
            documentContent.Append("</style>");
            documentContent.Append(request.DocumentHtmlContent);

            Directory.CreateDirectory(DocumentsConstatns.DocumentsFolderPath);

            var pdf = Pdf
                .From(documentContent.ToString())
                .Landscape()
                .Content();

            await File.WriteAllBytesAsync(DocumentsConstatns.FullFilePath, pdf);

            return new GeneratePdfDocumentResponse
            {
                GeneratedFilePath = DocumentsConstatns.UiFilePath,
            };
        }
    }
}
