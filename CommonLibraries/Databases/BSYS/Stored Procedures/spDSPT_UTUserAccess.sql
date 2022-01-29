

CREATE PROCEDURE [dbo].[spDSPT_UTUserAccess] 
	@UserReport		varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT A.*,
		B.UTID,
		B.AccessTo,
		B.AccessDescription
FROM dbo.SYSA_LST_UserIDInfo A
LEFT JOIN (
				(
					SELECT UserID as UTID, 'Compass Supervisor' as AccessDescription, Department as AccessTo
					FROM dbo.SYSA_REF_UserID_COMPASSSpecInfo
					WHERE Supervisor = 1
				)
				UNION
				(
					SELECT UserID as UTID, 'Groups' as AccessDescription, [Group] as AccessTo
					FROM dbo.SYSA_REF_UserID_Groups
				)
				UNION
				(
					SELECT UserID as UTID, 'OneLINK Queue Groups' as AccessDescription, [Queue Group] + '/' + SeqNum as AccessTo
					FROM dbo.SYSA_REF_UserID_OneLINKQueueGroups
				)
				UNION
				(
					SELECT UserID as UTID, 'OneLINK Tier Levels' as AccessDescription, ScreenID + '/' + SpecifiedTier as AccessTo
					FROM dbo.SYSA_REF_UserID_OneLINKTierLevels
				)
				UNION
				(
					SELECT UserID as UTID, 'Pagecenter Mail Boxes' as AccessDescription, Mailbox as AccessTo
					FROM dbo.SYSA_REF_UserID_PagecenterMailBoxes
				)
				UNION
				(
					SELECT UserID as UTID, 'AES Systems' as AccessDescription, System as AccessTo
					FROM dbo.SYSA_REF_UserID_Systems
				)
				UNION
				(
					SELECT UserID as UTID, 'Compass ARCs' as AccessDescription, ARC as AccessTo
					FROM dbo.SYSA_REF_UserID_COMPASSARCs
				)
				UNION
				(
					SELECT UserID as UTID, 'Compass Queues' as AccessDescription, Queue as AccessTo
					FROM dbo.SYSA_REF_UserID_COMPASSQueue
				)
			) B ON A.UserID = B.UTID
WHERE A.WindowsUserName = @UserReport
END