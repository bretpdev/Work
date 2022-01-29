using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPALETTERS
{
    public class CancellationResponse
    {
        public enum CancellationReason
        {
            PIF,
            Rehabilitation,
            Consolidation,
            BorrowerRequest,
            NSF,
            BankClosed,
            StoppedPayment,
            OtherReason,
            ManualDPACANP
        }

        public string BorrowerAccountIdentifier { get; set; }
        public string CosignerAccountIdentifier { get; set; } = null;
        public CancellationReason Reason { get; set; }
        public string OtherComment { get; set; }

        public CancellationDocumentInfo GetDocumentInfoFromResponse()
        {
            if (Reason == CancellationResponse.CancellationReason.BankClosed)
                return new CancellationDocumentInfo() 
                {
                    ActionCode = "DLUDC", 
                    CommentText = "BANK ACCOUNT IS CLOSED REMOVED DPA AND SENT LTR TO BORR", 
                    VariableText = "your bank account is closed", 
                    Document = "DPACANO", 
                    ResultIndicator = "" 
                };
            if (Reason == CancellationResponse.CancellationReason.BorrowerRequest)
                return new CancellationDocumentInfo()
                {
                    ActionCode = "DLBDC",
                    CommentText = "PER BORRS REQUEST REMOVED DPA AND SENT LTR TO BORR",
                    VariableText = "of your request to discontinue automatic payments",
                    Document = "DPACANO",
                    ResultIndicator = ""
                };
            if (Reason == CancellationResponse.CancellationReason.Consolidation)
                return new CancellationDocumentInfo()
                {
                    ActionCode = "DLUDC",
                    CommentText = "ACCOUNT HAS BEEN CONSOLIDATED REMOVED DPA AND SENT LTR TO BORR",
                    VariableText = "your loans have been paid through consolidation",
                    Document = "DPACANP",
                    ResultIndicator = "C"
                };
            if (Reason == CancellationResponse.CancellationReason.NSF)
                return new CancellationDocumentInfo()
                {
                    ActionCode = "DLUDC",
                    CommentText = "DUE TO REPEAT NSF REMOVED DPA AND SENT LTR TO BORR",
                    VariableText = "the excessive number of withdrawals UHEAA has attempted to make on your account which have been denied due to lack of sufficient funds",
                    Document = "DPACANO",
                    ResultIndicator = ""
                };
            if (Reason == CancellationResponse.CancellationReason.OtherReason)
                return new CancellationDocumentInfo()
                {
                    ActionCode = "DLUDC",
                    CommentText = $"{OtherComment}, removed DPA and sent ltr to borr",
                    VariableText = OtherComment,
                    Document = "DPACANO",
                    ResultIndicator = ""
                };
            if (Reason == CancellationResponse.CancellationReason.PIF)
                return new CancellationDocumentInfo()
                {
                    ActionCode = "DLUDC",
                    CommentText = "ACCOUNT IS PIF REMOVED DPA AND SENT LTR TO BORR",
                    VariableText = "your loans are paid-in-full",
                    Document = "DPACANP",
                    ResultIndicator = "P"
                };
            if (Reason == CancellationResponse.CancellationReason.Rehabilitation)
                return new CancellationDocumentInfo()
                {
                    ActionCode = "DLUDC",
                    CommentText = "ACCOUNT HAS BEEN REHABILITATED REMOVED DPA AND SENT LTR TO BORR",
                    VariableText = "your loans have been rehabilitated",
                    Document = "DPACANP",
                    ResultIndicator = "R"
                };
            if (Reason == CancellationResponse.CancellationReason.StoppedPayment)
                return new CancellationDocumentInfo()
                {
                    ActionCode = "DLUDC",
                    CommentText = "BORR STOPPED PAYMENTS REMOVED DPA AND SENT LTR TO BORR",
                    VariableText = "of stopped payments",
                    Document = "DPACANO",
                    ResultIndicator = ""
                };
            if(Reason == CancellationResponse.CancellationReason.ManualDPACANP)
                return new CancellationDocumentInfo()
                {
                    ActionCode = "DLUDC",
                    CommentText = "MANUAL DPACANP LETTER",
                    VariableText = "manual dpacanp letter",
                    Document = "DPACANP",
                    ResultIndicator = "C"
                };
            return null;
        }
    }
}
