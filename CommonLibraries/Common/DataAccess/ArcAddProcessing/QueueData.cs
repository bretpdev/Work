using System;
using System.Data.SqlClient;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace Uheaa.Common.DataAccess
{
    public class QueueData
    {
        public string AccountIdentifier { get; set; }
        public string QueueName { get; set; }
        public string InstitutionId { get; set; } = "";
        public string InstitutionType { get; set; } = "";
        public DateTime DateDue { get; set; } = DateTime.Now;
        public DateTime TimeDue { get; set; } = DateTime.Now;
        public string Comment { get; set; } = "";
        public string SourceFileName { get; set; } = "";

        private SqlConnection Conn { get; set; }
        private QueueResults Result { get; set; }

        private const string AccountIdentifierError = "Account Identifier is a required field";
        private const string QueueNameError = "Queue Name is a required field";

        public QueueData()
        {
            Conn = DataAccessHelper.GetManagedConnection(Ols, DataAccessHelper.CurrentMode);
        }

        public QueueResults AddQueue()
        {
            Result = ValidateRecord();
            if (Result.Errors.Count > 0)
                return Result;

            AddLP9OAQueue(AccountIdentifier, QueueName, InstitutionId, InstitutionType, DateDue, TimeDue, Comment, SourceFileName);
            return Result;
        }

        private QueueResults ValidateRecord()
        {
            QueueResults Result = new QueueResults();
            if (AccountIdentifier.IsNullOrEmpty())
                Result.Errors.Add(AccountIdentifierError);
            if (QueueName.IsNullOrEmpty())
                Result.Errors.Add(QueueNameError);
            return Result;
        }

        private void AddLP9OAQueue(string accountIdentifier, string queueName, string institutionId, string institutionType, DateTime dateDue, DateTime timeDue, string comment, string sourceFileName)
        {
            Result.QueueId = DataAccessHelper.ExecuteId("olqtskbldr.InsertSingleRecordToQueues", Conn,
                Sp("TargetId", accountIdentifier),
                Sp("QueueName", queueName),
                Sp("InstitutionId", institutionId),
                Sp("InstitutionType", institutionType),
                Sp("DateDue", dateDue.ToShortDateString()),
                Sp("TimeDue", timeDue.TimeOfDay),
                Sp("Comment", comment),
                Sp("SourceFileName", sourceFileName));
            Result.QueueAdded = Result.QueueId > 0;
        }

        private SqlParameter Sp(string name, object value) => SqlParams.Single(name, value);
    }
}