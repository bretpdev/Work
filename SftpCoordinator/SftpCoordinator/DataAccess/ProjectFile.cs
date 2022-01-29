using System.Collections.Generic;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace SftpCoordinator
{
    public class ProjectFile
    {
        [PrimaryKey, Hidden]
        public int ProjectFileId { get; set; }
        [InsertOnly, Hidden]
        public int ProjectId { get; set; }
        [Manual, Required, Ordinal(0)]
        public string SourceRoot { get; set; }
        [Manual, Ordinal(1)]
        public int SourcePathTypeId { get; set; }
        [Manual, Required, Ordinal(2)]
        public string DestinationRoot { get; set; }
        [Manual, Ordinal(3)]
        public int DestinationPathTypeId { get; set; }
        [Manual, Ordinal(4)]
        public string SearchPattern { get; set; }
        [Manual, Ordinal(5)]
        public string AntiSearchPattern { get; set; }
        [Ordinal(6)]
        public bool DecryptFile { get; set; }
        [Manual, Ordinal(7)]
        public bool CompressFile { get; set; }
        [Ordinal(8)]
        public bool EncryptFile { get; set; }
        [Manual, Ordinal(9)]
        public string AggregationFormatString { get; set; }
        [Manual, Ordinal(10)]
        public string RenameFormatString { get; set; }
        [Ordinal(11)]
        public bool FixLineEndings { get; set; }
        [Ordinal(12)]
        public bool IsArchiveJob { get; set; }
        public string CalculatedSourceRoot
        {
            get
            {
                string root = PathType.CachedPathTypes[SourcePathTypeId].RootPath;
                return GenericPath.Combine(root, SourceRoot).UpdatePath();
            }
        }
        public string CalculatedDestinationRoot
        {
            get
            {
                string root = PathType.CachedPathTypes[DestinationPathTypeId].RootPath;
                return GenericPath.Combine(root, DestinationRoot).UpdatePath();
            }
        }

        public string CalculateSourcePath(string fileName)
        {
            return GenericPath.Combine(CalculatedSourceRoot, fileName);
        }

        public string CalculateDestinationPath(string fileName)
        {
            return GenericPath.Combine(CalculatedDestinationRoot, fileName);
        }

        public static IEnumerable<ProjectFile> GetProjectFilesByProject(Project p)
        {
            return GetProjectFilesByProjectId(p.ProjectId);
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "GetProjectFilesByProjectId")]
        public static IEnumerable<ProjectFile> GetProjectFilesByProjectId(int projectId)
        {
            return Program.PLR.LDA.ExecuteList<ProjectFile>("GetProjectFilesByProjectId", DataAccessHelper.Database.SftpCoordinator, SqlParams.Generate(new { ProjectId = projectId })).Result;
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "UpdateProjectFile")]
        public static void Update(ProjectFile pf)
        {
            StringHelper.Sanitize(pf);
            Program.PLR.LDA.Execute("UpdateProjectFile", DataAccessHelper.Database.SftpCoordinator, SqlParams.Update(pf));
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "InsertProjectFile")]
        public static void Insert(ProjectFile pf)
        {
            StringHelper.Sanitize(pf);
            Program.PLR.LDA.Execute("InsertProjectFile", DataAccessHelper.Database.SftpCoordinator, SqlParams.Insert(pf));
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "DeleteProjectFile")]
        public static void Delete(ProjectFile pf)
        {
            Program.PLR.LDA.Execute("DeleteProjectFile", DataAccessHelper.Database.SftpCoordinator, SqlParams.Delete(pf));
        }
    }
}
