namespace NursesScheduler.Domain.Constants
{
    public static class DocumentsConstatns
    {
        private const string _wwwRoot = "wwwroot/";
        private const string _css = "css/";
        private const string _documentsFolder = "documents/";
        private const string _fileName = "schedule.pdf";
        private const string _bulma = "bulma.min.css";
        private const string _site = "site.css";

        public const string FullFilePath = _wwwRoot + _documentsFolder + _fileName;
        public const string DocumentsFolderPath = _wwwRoot + _documentsFolder;
        public const string UiFilePath = _documentsFolder + _fileName;

        public const string SiteCSSPath = _wwwRoot + _css + _site;
        public const string BulmaCSSPath = _wwwRoot + _css + _bulma;
    }
}
