using MediatR;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Documents.Commands.GeneratePdfDocument
{
    public sealed class GeneratePdfDocumentRequest : IRequest<GeneratePdfDocumentResponse>
    {
        public string DocumentHtmlContent { get; set; }
    }
}
