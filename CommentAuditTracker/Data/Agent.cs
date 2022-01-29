using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace CommentAuditTracker
{
    /// <summary>
    /// A customer service agent from the database.
    /// </summary>
    public class Agent
    {
        public int? AgentId { get; set; }
        public string FullName { get; set; }
        public bool Active { get; set; }
        public decimal AuditPercentage { get; set; }
        public List<string> UtIds { get; set; }
        public Agent()
        {
            UtIds = new List<string>();
        }

        /// <summary>
        /// Search the database for any agent that partially matches the given fullname, utid, or active status.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[comment_audit].SearchAgents")]
        public static IEnumerable<Agent> SearchAgents(string fullName, string utId, bool? active)
        {
            if (string.IsNullOrEmpty(fullName))
                fullName = null;
            else
                fullName = '%' + fullName + '%';
            if (string.IsNullOrEmpty(utId))
                utId = null;
            else
            {
                utId = new string(utId.Where(o => char.IsDigit(o)).ToArray());
                utId = '%' + utId + '%';
            }
            using (SqlCommand comm = DataAccessHelper.GetCommand("[comment_audit].SearchAgents", DataAccessHelper.Database.Cls))
            {
                comm.Parameters.AddWithValue("FullName", (object)fullName ?? DBNull.Value);
                comm.Parameters.AddWithValue("UtId", (object)utId ?? DBNull.Value);
                comm.Parameters.AddWithValue("Active", (object)active ?? DBNull.Value);

                var set = DataAccessHelper.ExecuteDataSet(comm);
                var agents = DataAccessHelper.ParseDataTable<Agent>(set.Tables[0]);
                var utIds = DataAccessHelper.ParseDataTable<UtIdPoco>(set.Tables[1]);
                foreach (var agent in agents)
                    agent.UtIds = utIds.Where(o => o.AgentId == agent.AgentId).Select(o => o.UtId).ToList();
                return agents;
            }
        }

        /// <summary>
        /// Used for retrieving search results
        /// </summary>
        private class UtIdPoco
        {
            public int AgentId { get; set; }
            public string UtId { get; set; }
        }

        /// <summary>
        /// Persist any agent changes to the database.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[comment_audit].SaveAgent")]
        public void UpdateDatabase()
        {
            using (SqlCommand comm = DataAccessHelper.GetCommand("[comment_audit].SaveAgent", DataAccessHelper.Database.Cls))
            {
                if (AgentId.HasValue)
                    comm.Parameters.AddWithValue("AgentId", AgentId.Value);
                comm.Parameters.AddWithValue("FullName", FullName);
                comm.Parameters.AddWithValue("Active", Active);
                comm.Parameters.AddWithValue("AuditPercentage", AuditPercentage);
                DataTable dt = new DataTable();
                var column = dt.Columns.Add("UtId");
                foreach (string utId in UtIds)
                    dt.Rows.Add(utId);
                comm.Parameters.AddWithValue("UtIds", dt);
                this.AgentId = (int)comm.ExecuteScalar();
            }
        }

        /// <summary>
        /// Returns null if valid, or a string describing the invalid data if invalid.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[comment_audit].GetAgentsByUtId")]
        public string ValidateChanges()
        {
            List<string> errors = new List<string>();
            //find any agents with the same full name
            Agent matchingFullNameAgent = GetByFullName(FullName);
            if (matchingFullNameAgent != null && matchingFullNameAgent.AgentId != this.AgentId)
                errors.Add(string.Format("There's already an existing agent with the name \"{0}\".  That agent's internal ID is {1}.", matchingFullNameAgent.FullName, matchingFullNameAgent.AgentId));
            foreach (string utId in this.UtIds)
            {
                //find any agents with the same ut id
                List<Agent> matchingIdAgents = DataAccessHelper.ExecuteList<Agent>("[comment_audit].GetAgentsByUtId", DataAccessHelper.Database.Cls, SqlParams.Single("UtId", utId));
                foreach (var agent in matchingIdAgents)
                    if (agent.AgentId != this.AgentId)
                        errors.Add(string.Format("Agent {0} already has UT ID {1} assigned to them.", agent.FullName, utId));
            }
            if (string.IsNullOrEmpty(FullName))
                errors.Add("Please enter a Full Name.");
            if (UtIds.Count == 0)
                errors.Add("Please enter at least one UT ID for this agent.");
            if (errors.Any())
                return string.Join(Environment.NewLine, errors.ToArray());
            return null;
        }
        /// <summary>
        /// Returns an agent with a matching full name.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[comment_audit].GetAgentsByFullName")]
        public static Agent GetByFullName(string fullName)
        {
            return DataAccessHelper.ExecuteList<Agent>("[comment_audit].GetAgentsByFullName", DataAccessHelper.Database.Cls, SqlParams.Single("FullName", fullName)).FirstOrDefault();
        }

        /// <summary>
        /// Remove this agent from the database.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Cls, "[comment_audit].DeleteAgent")]
        public void Delete()
        {
            DataAccessHelper.Execute("[comment_audit].DeleteAgent", DataAccessHelper.Database.Cls, SqlParams.Single("AgentId", AgentId));
        }
    }
}
