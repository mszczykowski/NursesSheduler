namespace NursesScheduler.BusinessLogic.CommandsAndQueries.Documents.Commands.DeleteDocument
{
    public sealed class DeleteDocumentResponse
    {
        public bool Success { get; set; }

        public DeleteDocumentResponse(bool success)
        {
            Success = success;
        }
    }
}
