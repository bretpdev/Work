-- =============================================
-- Author:		Daren Beattie
-- Create date: January 9, 2012
-- Description:	Gets the COMPASS user IDs of people who are assigned to the "CornerStone Accurint Queue Tasks" key.
-- =============================================
CREATE PROCEDURE spGetAccurintQueueUserIds
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT UID.UserID
	FROM CSYS.dbo.SYSA_DAT_UserKeyAssignment UKA
	INNER JOIN CSYS.dbo.SYSA_DAT_Users USR
		ON USR.SqlUserId = UKA.SqlUserId
	INNER JOIN (
		SELECT
			UII.UserID,
			UII.WindowsUserName,
			MIN(UII.DateEstablished) AS DateEstablished
		FROM BSYS.dbo.SYSA_LST_UserIDInfo UII
		WHERE UII.[Date Access Removed] IS NULL
		GROUP BY UserID, WindowsUserName
	) UID
		ON UID.WindowsUserName = USR.WindowsUserName
	WHERE UKA.UserKey = 'CornerStone Accurint Queue Tasks'
	AND UKA.EndDate IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetAccurintQueueUserIds] TO [UHEAA\CornerStoneUsers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetAccurintQueueUserIds] TO [db_executor]
    AS [dbo];



