CREATE PROCEDURE [dbo].[UpdateDocumentDetails]

@DocumentDetailsId int,
@LetterId int,
@Path varchar(max),
@Ssn char(9),
@DocDate date,
@ADDR_ACCT_NUM varchar(10),
@RequestUser varchar(8),
@CorrMethod varchar(20),
@LoadTime datetime,
@AddresseeEmail varchar(254),
@CreateDate date,
@DueDate varchar(50) = null,
@TotalDue varchar(15) = null,
@BillSeq char(4) = null,
@Printed datetime = null

AS

    UPDATE
        [dbo].[DocumentDetails]
    SET
        [LetterId] = @LetterId,
        [Path] = @Path,
        [Ssn] = @Ssn,
        [DocDate] = @DocDate,
        ADDR_ACCT_NUM = @ADDR_ACCT_NUM,
        [RequestUser] = @RequestUser,
        [CorrMethod] = @CorrMethod,
        [LoadTime] = @LoadTime,
        [AddresseeEmail] = @AddresseeEmail,
        [CreateDate] = @CreateDate,
        [DueDate] = @DueDate,
        [TotalDue] = @TotalDue,
        [BillSeq] = @BillSeq,
        [Printed] = @Printed
    WHERE
        [DocumentDetailsId] = @DocumentDetailsId

RETURN 0
