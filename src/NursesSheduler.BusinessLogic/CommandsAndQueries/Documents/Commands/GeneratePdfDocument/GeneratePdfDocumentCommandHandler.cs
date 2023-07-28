using MediatR;
using OpenHtmlToPdf;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Documents.Commands.GeneratePdfDocument
{
    internal sealed class GeneratePdfDocumentCommandHandler : IRequestHandler<GeneratePdfDocumentRequest, GeneratePdfDocumentResponse>
    {
        private const string wwwRoot = "wwwroot/";
        private const string documentsFolder = "documents/";
        private const string fileName = "schedule.pdf";

        public async Task<GeneratePdfDocumentResponse> Handle(GeneratePdfDocumentRequest request, CancellationToken cancellationToken)
        {
            var fullPath = wwwRoot + documentsFolder;

            Directory.CreateDirectory(fullPath);

            var x = "<head><link href=\"D:\\Projekty\\inżynierka\\NursesSheduler\\src\\NursesSheduler.BlazorShared\\wwwroot\\css\\bulma.min.css\" rel=\"stylesheet\"/><link href=\"_content/NursesScheduler.BlazorShared/css/all.min.css\" rel=\"stylesheet\"/><link href =\"~/wwwroot/css/site.css\" rel=\"stylesheet\"/></head>";

            var pdf = Pdf
                .From(request.DocumentHtmlContent)
                .Landscape()
                .Content();

            await File.WriteAllBytesAsync(fullPath + fileName, pdf);

            return new GeneratePdfDocumentResponse
            {
                GeneratedFilePath = documentsFolder + fileName,
            };
        }
    }
}
