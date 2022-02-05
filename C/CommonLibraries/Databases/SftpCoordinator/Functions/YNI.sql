CREATE FUNCTION [dbo].[YNI]
(
	@Value int = null
)
RETURNS nvarchar(3)
AS
BEGIN
	RETURN case when @Value is null then 'N/A' when @Value = 1 then 'Yes' else 'No' end 
END
