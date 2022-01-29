CREATE PROCEDURE billing.[GetSpecialMessages]
	@ReportNumber int
AS
	SELECT
	  [SpecialMessageId]
      ,[ReportNumber]
      ,isnull([FirstSpecialMessageTitle],'') as [FirstSpecialMessageTitle]
      ,isnull([FirstSpecialMessage],'') as [FirstSpecialMessage]
      ,isnull([SecondSpecialMessageTitle],'') as [SecondSpecialMessageTitle]
      ,isnull([SecondSpecialMessage],'') as [SecondSpecialMessage]
      ,[Message1XCoord]
      ,[Message1YCoord]
      ,[Message1FontTypeId]
      ,[Message2XCoord]
      ,[Message2YCoord]
      ,[Message2FontTypeId]
	FROM
		SpecialMessages
	WHERE
		ReportNumber = @ReportNumber
RETURN 0