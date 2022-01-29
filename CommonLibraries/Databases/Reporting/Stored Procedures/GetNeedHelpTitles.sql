CREATE PROCEDURE [dbo].[GetNeedHelpTitles]
	@IsFed bit
AS
BEGIN
	IF (@IsFed = 1)
		BEGIN
			SELECT
				[Subject]
			FROM
				NeedHelpCornerStone.dbo.DAT_Ticket
		END
	ELSE
		BEGIN
			SELECT
				[Subject]
			FROM
				NeedHelpUheaa.dbo.DAT_Ticket
		END
END

RETURN 0

GRANT EXECUTE
    ON OBJECT::[dbo].[GetNeedHelpTitles] TO [db_executor]
    AS [dbo];