using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uheaa.Common.DataAccess;

namespace ProjectRequest
{
    public class DataAccess
    {
        public static List<string> GetNotificationRecipients()
        {
            return DataAccessHelper.ExecuteList<string>("[projectrequest].[GetNotificationRecipients]", DataAccessHelper.Database.CentralData);
        }

        public static void InsertProjectRequest(Models.ProjectRequest projectRequest)
        {
            DataAccessHelper.Execute("[projectrequest].[InsertProjectRequest]", DataAccessHelper.Database.CentralData,
                    SqlParams.Single("ProjectName", projectRequest.ProjectName),
                    SqlParams.Single("SubmittedBy", projectRequest.SubmittedBy),
                    SqlParams.Single("Date", projectRequest.Date),
                    SqlParams.Single("Department", projectRequest.Department),
                    SqlParams.Single("ProjectSummary", projectRequest.ProjectSummary),
                    SqlParams.Single("BusinessNeed", projectRequest.BusinessNeed),
                    SqlParams.Single("Benefits", projectRequest.Benefits),
                    SqlParams.Single("ImplementationApproach", projectRequest.ImplementationApproach == null ? (object)DBNull.Value : (object)projectRequest.ImplementationApproach),
                    SqlParams.Single("ProjectStatus", projectRequest.Status.Replace(" ", "")),
                    SqlParams.Single("RequestorScore", projectRequest.RequestorScore.HasValue ? (object)projectRequest.RequestorScore.Value : DBNull.Value),
                    SqlParams.Single("UrgencyScore", projectRequest.UrgencyScore.HasValue ? (object)projectRequest.UrgencyScore.Value : DBNull.Value),
                    SqlParams.Single("RiskScore", projectRequest.RiskScore.HasValue ? (object)projectRequest.RiskScore.Value : DBNull.Value));
        }

        public static void UpdateProjectRequest(Models.ProjectRequest projectRequest)
        {
            DataAccessHelper.Execute("[projectrequest].[UpdateProjectRequest]", DataAccessHelper.Database.CentralData,
                    SqlParams.Single("ProjectId", projectRequest.ProjectRequestId.Value),
                    SqlParams.Single("ProjectName", projectRequest.ProjectName),
                    SqlParams.Single("SubmittedBy", projectRequest.SubmittedBy),
                    SqlParams.Single("Date", projectRequest.Date),
                    SqlParams.Single("Department", projectRequest.Department),
                    SqlParams.Single("ProjectSummary", projectRequest.ProjectSummary),
                    SqlParams.Single("BusinessNeed", projectRequest.BusinessNeed),
                    SqlParams.Single("Benefits", projectRequest.Benefits),
                    SqlParams.Single("ImplementationApproach", projectRequest.ImplementationApproach == null ? (object)DBNull.Value : (object)projectRequest.ImplementationApproach),
                    SqlParams.Single("ProjectStatus", projectRequest.Status.Replace(" ", "")),
                    SqlParams.Single("RequestorScore", projectRequest.RequestorScore.HasValue ? (object)projectRequest.RequestorScore.Value : DBNull.Value),
                    SqlParams.Single("UrgencyScore", projectRequest.UrgencyScore.HasValue ? (object)projectRequest.UrgencyScore.Value : DBNull.Value),
                    SqlParams.Single("RiskScore", projectRequest.RiskScore.HasValue ? (object)projectRequest.RiskScore.Value : DBNull.Value));
        }

        public static List<Models.ScoreType> GetScoreOverview(int? projectId)
        {
            return DataAccessHelper.ExecuteList<Models.ScoreType>("[projectrequest].[GetScoreOverview]", DataAccessHelper.Database.CentralData, SqlParams.Single("ProjectId", projectId)) ?? new List<Models.ScoreType>();
        }

        public static List<Models.ProductPrioritization> GetProductPrioritization(bool checkArchive)
        {
            var result =  DataAccessHelper.ExecuteList<Models.ProductPrioritization>("[projectrequest].[GetProductPrioritization]", DataAccessHelper.Database.CentralData, SqlParams.Single("Archived", checkArchive)).ToList<Models.ProductPrioritization>() ?? new List<Models.ProductPrioritization>();
            foreach(var rec in result)
            {
                if(rec != null)
                {
                    rec.Status = Models.ProjectRequest.GetStateWithSpace(rec.Status);
                } 
            }
            return result;
        }

        public static Models.ProjectRequest GetProject(int projectId)
        {
            var result = DataAccessHelper.ExecuteSingle<Models.ProjectRequest>("[projectrequest].[GetProject]", DataAccessHelper.Database.CentralData, SqlParams.Single("ProjectId", projectId));
            if(result != null)
            {
                result.Status = Models.ProjectRequest.GetStateWithSpace(result.Status);
            }
            return result;
        }

        public static string GetScoreType(int scoreTypeId)
        {
            return DataAccessHelper.ExecuteSingle<string>("[projectrequest].[GetScoreType]", DataAccessHelper.Database.CentralData, SqlParams.Single("ScoreTypeId", scoreTypeId));
        }

        public static Models.ProjectScoring GetScore(int scoreId)
        {
            return DataAccessHelper.ExecuteSingle<Models.ProjectScoring>("[projectrequest].[GetScore]", DataAccessHelper.Database.CentralData, SqlParams.Single("ScoreId", scoreId));
        }

        public static void ArchiveProject(int projectId)
        {
            DataAccessHelper.Execute("[projectrequest].[ArchiveProject]", DataAccessHelper.Database.CentralData, SqlParams.Single("ProjectId", projectId));
        }

        public static void InsertScore(Models.ProjectScoring projectScoring)
        {
            DataAccessHelper.Execute("[projectrequest].[InsertScore]", DataAccessHelper.Database.CentralData,
                SqlParams.Single("ProjectId", projectScoring.ProjectId),
                SqlParams.Single("ScoreTypeId", projectScoring.ScoreTypeId),
                SqlParams.Single("ScoreId", projectScoring.ScoreId.HasValue ? (object)projectScoring.ScoreId : (object)DBNull.Value),
                SqlParams.Single("Score", projectScoring.Score));
        }

        public static List<string> GetDepartments()
        {
            return DataAccessHelper.ExecuteList<string>("[dbo].[ProjectRequestGetDepartments]", DataAccessHelper.Database.Bsys);
        }

        public static List<Models.RolePermissions> GetRolePermissions()
        {
            return DataAccessHelper.ExecuteList<Models.RolePermissions>("[projectrequest].[GetRolePermissions]", DataAccessHelper.Database.CentralData);
        }
    }
}