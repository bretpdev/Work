using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace SftpCoordinator
{
    public class Project
    {
        [PrimaryKey, Hidden]
        public int ProjectId { get; set; }
        [Label("Project Name"), Required, Ordinal(0)]
        public string Name { get; set; }
        [TextBoxLines(8), Label("Project Notes"), Ordinal(1)]
        public string Notes { get; set; }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "GetAllProjects")]
        public static IEnumerable<Project> GetAllProjects()
        {
            return Program.PLR.LDA.ExecuteList<Project>("GetAllProjects", DataAccessHelper.Database.SftpCoordinator).Result;
        }
        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "UpdateProject")]
        public static void UpdateProject(Project p)
        {
            Program.PLR.LDA.Execute("UpdateProject", DataAccessHelper.Database.SftpCoordinator, SqlParams.Update(p));
        }
        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "InsertProject")]
        public static void InsertProject(Project p)
        {
            p.ProjectId = DataAccessHelper.ExecuteId("InsertProject", DataAccessHelper.Database.SftpCoordinator, SqlParams.Insert(p));
        }
        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "DeleteProject")]
        public static void DeleteProject(Project p)
        {
            Program.PLR.LDA.Execute("DeleteProject", DataAccessHelper.Database.SftpCoordinator, SqlParams.Delete(p));
        }
    }
}
