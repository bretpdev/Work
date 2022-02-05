CREATE PROCEDURE spPRNT_EOJ_FaxesErroredToday
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT FAX.BusinessUnit
	FROM PRNT_DAT_Fax FAX
		INNER JOIN PRNT_DAT_FaxingErrors ERR
			ON ERR.FaxSeqNum = FAX.SeqNum
	WHERE DATEDIFF(d, ERR.ErrorEncountered, GETDATE()) = 0
END