CREATE PROCEDURE spPRNT_EOJ_DocumentsPrintedToday
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT BusinessUnit
	FROM PRNT_DAT_Print
	WHERE DATEDIFF(d, PrintDate, GETDATE()) = 0
END