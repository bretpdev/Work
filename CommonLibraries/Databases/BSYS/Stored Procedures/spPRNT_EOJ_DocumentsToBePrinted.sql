﻿CREATE PROCEDURE spPRNT_EOJ_DocumentsToBePrinted
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT BusinessUnit
	FROM PRNT_DAT_Print
	WHERE PrintDate IS NULL
END