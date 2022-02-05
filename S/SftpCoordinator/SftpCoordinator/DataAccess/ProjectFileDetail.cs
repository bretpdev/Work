using System.Collections.Generic;
using Uheaa.Common.DataAccess;

namespace SftpCoordinator
{
    public class ProjectFileDetail
    {
        public string ProjectType { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNotes { get; set; }
        public string SourceRoot { get; set; }
        public string SourceType { get; set; }
        public string DestinationRoot { get; set; }
        public string DestinationType { get; set; }
        public string SearchPattern { get; set; }
        public string AntiSearchPattern { get; set; }
        public string Decrypt { get; set; }
        public string Compress { get; set; }
        public string AggregationFormatString { get; set; }
        public string RenameFormatString { get; set; }
        public string Encrypt { get; set; }
        public string FixLineEndings { get; set; }
        public string IsArchiveJob { get; set; }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "GetProjectFilesDetailed")]
        public static List<ProjectFileDetail> GetAll()
        {
            return Program.PLR.LDA.ExecuteList<ProjectFileDetail>("GetProjectFilesDetailed", DataAccessHelper.Database.SftpCoordinator).Result;
        }

    }
}
