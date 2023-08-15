using MediatR;
using NursesScheduler.Domain.Constants;
using OpenHtmlToPdf;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Documents.Commands.GeneratePdfDocument
{
    internal sealed class GeneratePdfDocumentCommandHandler : IRequestHandler<GeneratePdfDocumentRequest, GeneratePdfDocumentResponse>
    {
        public async Task<GeneratePdfDocumentResponse> Handle(GeneratePdfDocumentRequest request, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(DocumentsConstatns.DocumentsFolderPath);

            var pdf = Pdf
                .From(request.DocumentHtmlContent)
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
