CREATE PROCEDURE [dbo].[GetAllDocumentDetailData]

AS
begin
    SELECT 
        [DocumentDetailsId],
        [LetterId],
        [Path],
        [Ssn],
        [DocDate],
        ADDR_ACCT_NUM,
        RequestUser,
        [CorrMethod],
        LoadTime,
        AddresseeEmail,
        [CreateDate],
        [DueDate],
        [TotalDue],
        [BillSeq],
        [Printed]
    FROM
        [dbo].[DocumentDetails]
    WHERE Active = 1
end
