CREATE PROCEDURE [dbo].[InsertDocumentDetails]

    @LetterId int,
    @Path varchar(max),
    @Ssn char(9),
    @DocDate date,
    @ADDR_ACCT_NUM varchar(10),
    @RequestUser varchar(8),
    @CorrMethod char(20),
    @LoadTime datetime,
    @AddresseeEmail varchar(254),
    @CreateDate date,
    @DueDate varchar(50) = null,
    @TotalDue varchar(15) = null,
    @BillSeq char(4) = null,
    @Printed datetime = null

AS
    
    INSERT INTO [dbo].[DocumentDetails]([LetterId],[Path],[Ssn],[DocDate],[ADDR_ACCT_NUM],[RequestUser], [CorrMethod],[LoadTime],[AddresseeEmail],[CreateDate],[DueDate],[TotalDue],[BillSeq] ,[Printed])
    VALUES(@LetterId, @Path, @Ssn, @DocDate, @ADDR_ACCT_NUM, @RequestUser, @CorrMethod, @LoadTime, @AddresseeEmail, @CreateDate, @DueDate, @TotalDue, @BillSeq, @Printed);

RETURN 0
