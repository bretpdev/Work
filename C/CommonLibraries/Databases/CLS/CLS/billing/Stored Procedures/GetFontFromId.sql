
CREATE PROCEDURE billing.[GetFontFromId]
	@FontTypeId int
AS
	SELECT
		[FontTypeId]
      ,[FontType]
      ,[EnumValue]
      ,[FontSize]
      ,[IsBold]
	FROM
		FontType
	WHERE
		FontTypeId = @FontTypeId

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[billing].[GetFontFromId] TO [db_executor]
    AS [dbo];

