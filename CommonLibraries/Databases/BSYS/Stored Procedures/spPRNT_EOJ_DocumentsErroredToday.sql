CREATE PROCEDURE spPRNT_EOJ_DocumentsErroredToday
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT PRT.BusinessUnit
	FROM PRNT_DAT_Print PRT
		INNER JOIN PRNT_DAT_PrintingErrors ERR
			ON ERR.PrintSeqNum = PRT.SeqNum
	WHERE DATEDIFF(d, ERR.ErrorEncountered, GETDATE()) = 0
END