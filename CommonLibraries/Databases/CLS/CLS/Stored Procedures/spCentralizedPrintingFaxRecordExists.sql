
CREATE PROCEDURE [dbo].[spCentralizedPrintingFaxRecordExists]
	@FaxSeqNum	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM CentralizedPrintingFax WHERE SeqNum = @FaxSeqNum) > 0
		SELECT CAST(1 AS BIT)
	ELSE
		SELECT CAST(0 AS BIT)
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingFaxRecordExists] TO [db_executor]
    AS [dbo];



