CREATE PROCEDURE spPRNT_EOJ_FaxesSentToday
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT BusinessUnit
	FROM PRNT_DAT_Fax
	WHERE DATEDIFF(d, FaxDate, GETDATE()) = 0
END