-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [STRQ]
(
	@Value NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	return '''''' + dbo.STR(@Value) + ''''''

END