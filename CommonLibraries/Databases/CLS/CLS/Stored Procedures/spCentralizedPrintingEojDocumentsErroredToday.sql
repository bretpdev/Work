CREATE PROCEDURE spCentralizedPrintingEojDocumentsErroredToday
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT LTR.BusinessUnitId
	FROM CentralizedPrintingLetter LTR
		INNER JOIN CentralizedPrintingLetterError ERR
			ON ERR.LetterSeqNum = LTR.SeqNum
	WHERE DATEDIFF(d, ERR.Detected, GETDATE()) = 0
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingEojDocumentsErroredToday] TO [db_executor]
    AS [dbo];



