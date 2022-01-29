using CentralizedPrintingProcess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

public class DataAccess
{
    private LogDataAccess LDA { get; set; }

    public DataAccess(LogDataAccess lda) =>
        LDA = lda;

    #region Letters
    [UsesSproc(Bsys, "[cntrprnt].[GetUnprocessedLetterRecords]")]
    public List<LetterRecord> GetUnprocessedRecords() =>
         LDA.ExecuteList<LetterRecord>("[cntrprnt].[GetUnprocessedLetterRecords]", Bsys).Result;

    [UsesSproc(Bsys, "[cntrprnt].[GetUnprocessedLetterRecordsSummary]")]
    public List<LetterRecord> GetUnprocessedRecordsSummary() =>
         LDA.ExecuteList<LetterRecord>("[cntrprnt].[GetUnprocessedLetterRecordsSummary]", Bsys).Result;

    [UsesSproc(Bsys, "[cntrprnt].[MarkLetterRecordAsPrinted]")]
    public void MarkLetterRecordAsPrinted(int seqNum) =>
        LDA.Execute("[cntrprnt].[MarkLetterRecordAsPrinted]", Bsys, Sp("SeqNum", seqNum));

    [UsesSproc(Bsys, "[cntrprnt].[MarkRecordEcorrStatus]")]
    public void MarkRecordEcorrStatus(int seqNum, bool isOnEcorr, DateTime? ecorrDocumentCreatedAt) =>
        LDA.Execute("[cntrprnt].[MarkRecordEcorrStatus]", Bsys, Sp("SeqNum", seqNum), Sp("IsOnEcorr", isOnEcorr), Sp("EcorrDocumentCreatedAt", ecorrDocumentCreatedAt));

    [UsesSproc(Udw, "[cntrprnt].[GetBorrowerEcorrInfo]")]
    public BorrowerEcorrInfo GetBorrowerEcorrInfo(string accountNumber) =>
        LDA.ExecuteList<BorrowerEcorrInfo>("[cntrprnt].[GetBorrowerEcorrInfo]", Udw, Sp("AccountNumber", accountNumber)).Result.SingleOrDefault();  

    [UsesSproc(EcorrUheaa, "[dbo].[InsertDocumentDetails]")]
    public void AddEcorrDocumentDetails(int letterId, string filePath, string ssn, DateTime docDate, string accountNumber, string utId, string corrMethod, DateTime loadTime, string email, DateTime createDate)
    {
        LDA.Execute("[dbo].[InsertDocumentDetails]", EcorrUheaa,
            Sp("LetterId", letterId),
            Sp("Path", filePath),
            Sp("Ssn", ssn),
            Sp("DocDate", docDate),
            Sp("ADDR_ACCT_NUM", accountNumber),
            Sp("RequestUser", utId),
            Sp("CorrMethod", corrMethod),
            Sp("LoadTime", loadTime),
            Sp("AddresseeEmail", email),
            Sp("CreateDate", createDate));
    }

    [UsesSproc(EcorrUheaa, "[cntrprnt].[GetEcorrLetterId]")]
    public int? GetLetterId(string letterId) =>
        LDA.ExecuteSingle<int?>("[cntrprnt].[GetEcorrLetterId]", EcorrUheaa, Sp("Letter", letterId)).Result ?? null;

    #endregion
    #region Fax
    [UsesSproc(Bsys, "[cntrprnt].[GetUnprocessedFaxRecords]")]
    public List<FaxRecord> GetUnprocessedFaxRecords()
    {
        return LDA.ExecuteList<FaxRecord>("[cntrprnt].[GetUnprocessedFaxRecords]", Bsys).Result;
    }

    /// <summary>
    /// Update the database to current status.
    /// </summary>
    [UsesSproc(Bsys, "[cntrprnt].[MarkFaxRecordAsFaxed]")]
    public void MarkFaxRecordFaxed(FaxRecord record)
    {
        LDA.Execute("[cntrprnt].[MarkFaxRecordAsFaxed]", Bsys, new SqlParameter("SeqNum", record.SeqNum));
    }
    #endregion

    /// <summary>
    /// Retrieve Business unit Name for a given Business unit ID
    /// </summary>
    /// <param name="businessUnitId">integer representing the business unit</param>
    /// <returns>Business Unit Name</returns>
    [UsesSproc(Csys, "spGENR_GetBusinessUnits")]
    public string GetBusinessUnitName(int businessUnitId) =>
        LDA.ExecuteList<BusinessUnit>("spGENR_GetBusinessUnits", Csys).Result.SingleOrDefault(p => p.ID == businessUnitId).Name ?? "";

    [UsesSproc(Bsys, "spGENRRecipientString")]
    [UsesSproc(Bsys, "spCentralizedPrintingGetManagerEmail")]
    [UsesSproc(Bsys, "spCentralizedPrintingGetBUEmailGroup")]
    public string GetEmailRecipients(string bsysEmailKey = "", string businessUnit = "")
    {
        //get recips
        string recipients = null;
        if ((!string.IsNullOrEmpty(bsysEmailKey)))
        {
            //get list from BSYS if TypeKey is given and add BU Manager if BU is given
            recipients = LDA.ExecuteSingle<string>("spGENRRecipientString", Bsys, Sp("EmailID", bsysEmailKey)).Result.ToString();
            if ((!string.IsNullOrEmpty(businessUnit)))
            {
                //get BU manager if BU is populated
                recipients += ";" + LDA.ExecuteList<string>("spCentralizedPrintingGetManagerEmail", Bsys, Sp("BusinessUnit", businessUnit)).Result.FirstOrDefault();
            }
        }
        else
        {
            //send email to BU group email addr
            recipients = LDA.ExecuteSingle<string>("spCentralizedPrintingGetBUEmailGroup", Bsys, Sp("BusinessUnit", businessUnit)).Result;
        }
#if DEBUG
        recipients = Environment.UserName + "@utahsbr.edu";
#endif
        return recipients;
    }

    /// <summary>
    /// Determine whether the BU wants an Arc or a queue task added
    /// </summary>
    /// <param name="businessUnit">Name of business unit</param>
    /// <param name="error">ErrorType enum value</param>
    /// <returns>Arc name or Queue name for each Error type</returns>
    [UsesSproc(Bsys, "spCentralizedPrintingArcOrQueue")]
    public string ARCsAndQueuesForBusinessUnits(string businessUnit, CentralizedPrintingErrorType error)
    {
        string whatClass = "";
        switch (error)
        {
            case CentralizedPrintingErrorType.CPrintingErrArc:
                whatClass = "COMPASS Print Err";
                break;
            case CentralizedPrintingErrorType.OLPrintingErrQueue:
                whatClass = "OneLINK Print Err";
                break;
            case CentralizedPrintingErrorType.CFaxingErrArc:
                whatClass = "COMPASS Fax Err";
                break;
            case CentralizedPrintingErrorType.OLFaxingErrQueue:
                whatClass = "OneLINK Fax Err";
                break;
        }
        return LDA.ExecuteList<string>("spCentralizedPrintingArcOrQueue", Bsys, Sp("BusinessUnit", businessUnit), Sp("WhatClass", whatClass)).Result.SingleOrDefault();
    }

    [UsesSproc(Bsys, "cntrprnt.MarkRecordDeleted")]
    public void MarkRecordDeleted(int seqNum) =>
        LDA.Execute("cntrprnt.MarkRecordDeleted", Bsys, Sp("SeqNum", seqNum));

    [UsesSproc(Bsys, "cntrprnt.MarkFaxDeleted")]
    public void MarkFaxRecordDeleted(int seqNum) =>
        LDA.Execute("cntrprnt.MarkFaxDeleted", Bsys, Sp("SeqNum", seqNum));

    private SqlParameter Sp(string parameterName, object parameterValue) =>
        SqlParams.Single(parameterName, parameterValue);
}