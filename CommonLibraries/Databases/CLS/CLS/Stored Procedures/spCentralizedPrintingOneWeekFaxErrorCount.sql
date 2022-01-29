CREATE PROCEDURE spCentralizedPrintingOneWeekFaxErrorCount
	@LetterId	VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*) AS OneWeekErrorCount
	FROM CentralizedPrintingFax A
	INNER JOIN CentralizedPrintingFaxError B
		ON A.SeqNum = B.FaxSeqNum
	WHERE DATEDIFF(wk, A.Requested, GETDATE()) = 0
		AND A.LetterID = @LetterId
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingOneWeekFaxErrorCount] TO [db_executor]
    AS [dbo];



