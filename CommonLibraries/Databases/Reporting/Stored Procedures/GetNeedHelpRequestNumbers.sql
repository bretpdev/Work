CREATE PROCEDURE [dbo].[GetNeedHelpRequestNumbers]
	@IsFed bit
AS
BEGIN
	IF (@IsFed = 1)
		BEGIN
			SELECT
				Ticket
			FROM
				NeedHelpCornerStone.dbo.DAT_Ticket
		END
	ELSE
		BEGIN
			SELECT
				Ticket
			FROM
				NeedHElpUheaa.dbo.DAT_Ticket
		END
	END
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNeedHelpRequestNumbers] TO [db_executor]
    AS [dbo];