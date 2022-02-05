-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION SC
(
	@Value NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	if ISDATE(@Value) = 1 return CONVERT(nvarchar(10), CONVERT(datetime, @Value), 101)
	return @Value

END