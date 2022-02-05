CREATE PROCEDURE spCentralizedPrintingEojFaxesErroredToday
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT FAX.BusinessUnitId
	FROM CentralizedPrintingFax FAX
		INNER JOIN CentralizedPrintingFaxError ERR
			ON ERR.FaxSeqNum = FAX.SeqNum
	WHERE DATEDIFF(d, ERR.Detected, GETDATE()) = 0
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingEojFaxesErroredToday] TO [db_executor]
    AS [dbo];



