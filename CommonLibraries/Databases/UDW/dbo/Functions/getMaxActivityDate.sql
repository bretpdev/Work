-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[getMaxActivityDate]()

RETURNS Date AS

BEGIN
	DECLARE @MaxDate Date
	SET @MaxDate = '';
	
	SELECT @MaxDate = MAX(CAST(LD_ATY_REQ_RCV AS DATE))
	FROM AY10_History
	
	RETURN @MaxDate
END