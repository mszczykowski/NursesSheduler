using MediatR;
using NursesScheduler.Domain.Constants;

namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Documents.Commands.DeleteDocument
{
    public sealed class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentRequest, DeleteDocumentResponse>
    {
        public Task<DeleteDocumentResponse> Handle(DeleteDocumentRequest request, CancellationToken cancellationToken)
        {
            File.Delete(DocumentsConstatns.FullFilePath);

            return Task.FromResult(new DeleteDocumentResponse(true));
        }
    }
}
