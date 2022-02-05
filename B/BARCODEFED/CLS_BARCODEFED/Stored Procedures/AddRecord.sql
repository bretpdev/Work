CREATE PROCEDURE [barcodefed].[AddRecord]
	-- Add the parameters for the stored procedure here
	@RecipientId	VARCHAR(10),
	@LetterId		VARCHAR(10),
	@CreateDate     DATETIME,
	@Address1		VARCHAR(30) = NULL,
	@Address2		VARCHAR(30) = NULL,
	@City			VARCHAR(20) = NULL,
	@State			CHAR(2) = NULL,
	@Zip			VARCHAR(9) = NULL,
	@Country		VARCHAR(25) = NULL,
	@AddedBy VARCHAR(50),
	@BorrowerSsn VARCHAR(9) = NULL,
	@PersonType CHAR(1) = NULL,
	@ReceivedDate DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [barcodefed].ReturnMail (RecipientId,LetterId,CreateDate,Address1,Address2,City,[State],Zip,Country,AddedBy,BorrowerSsn,PersonType,ReceivedDate)
	VALUES (@RecipientId,@LetterId,@CreateDate,@Address1,@Address2,@City,@State,@Zip,@Country,@AddedBy,@BorrowerSsn,@PersonType,@ReceivedDate)
END