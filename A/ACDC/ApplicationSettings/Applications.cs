using System.Collections.Generic;

namespace ApplicationSettings
{
    public class Applications
    {
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string AccessKey { get; set; }
        public string StartingClass { get; set; }
        public string StartingDll { get; set; }
        public string SourcePath { get; set; }
        public List<Arguments> Arguments { get; set; }
    }
}