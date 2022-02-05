using System.Collections.Generic;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace SftpCoordinator
{
    public class PathType
    {
        [PrimaryKey]
        public int PathTypeId { get; set; }
        public string Description { get; set; }
        public string RootPath { get; set; }
        [DbReadOnly]
        public int AffectedFiles { get; set; }

        public string Name { get { return Description; } }


        static PathType()
        {
            CachedPathTypes = new Dictionary<int, PathType>();
            GetAll();
        }
        public static Dictionary<int, PathType> CachedPathTypes;
        public static IEnumerable<CycleOption<int>> CachedCycleOptions
        {
            get
            {
                return CachedPathTypes.Select(o => new CycleOption<int>() { Label = o.Value.Description, Value = o.Value.PathTypeId });
            }
        }
        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "GetAllPathTypes")]
        public static List<PathType> GetAll()
        {
            var results = Program.PLR.LDA.ExecuteList<PathType>("GetAllPathTypes", DataAccessHelper.Database.SftpCoordinator).Result;
            foreach (var path in results)
                CachedPathTypes[path.PathTypeId] = path;
            return results;
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "DeletePathType")]

        public static void Delete(PathType pt)
        {
            Program.PLR.LDA.Execute("DeletePathType", DataAccessHelper.Database.SftpCoordinator, SqlParams.Delete(pt));
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "InsertPathType")]
        public static void Insert(PathType pt)
        {
            pt.PathTypeId = Program.PLR.LDA.ExecuteSingle<int>("InsertPathType", DataAccessHelper.Database.SftpCoordinator, SqlParams.Insert(pt)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "UpdatePathType")]
        public static void Save(PathType pt)
        {
            Program.PLR.LDA.Execute("UpdatePathType", DataAccessHelper.Database.SftpCoordinator, SqlParams.Update(pt));
        }
    }
}
