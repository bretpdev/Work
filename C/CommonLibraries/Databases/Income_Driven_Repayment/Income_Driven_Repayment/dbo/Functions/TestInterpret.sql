-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION TestInterpret
(
	@Data TEST readonly
)
RETURNS nvarchar(max)
AS
BEGIN
	declare @Result nvarchar(max)
	
	select @Result = test from @DATA

	return @Result
END