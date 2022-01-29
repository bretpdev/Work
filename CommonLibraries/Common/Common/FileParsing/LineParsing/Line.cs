
namespace Uheaa.Common
{
    public struct Line<T>
    {
        public T Content { get; set; }
        public string OriginalLine { get; set; }
        public int LineNumber { get; set; }
        public Line(string originalLine, T content, int lineNumber)
            : this()
        {
            OriginalLine = originalLine;
            Content = content;
            LineNumber = lineNumber;
        }
    }
}
