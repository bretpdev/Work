



CREATE PROCEDURE [dbo].[spDSPT_UserAccess] 
@UserReport   VARCHAR(50)
AS
BEGIN


-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

-- Insert statements for procedure here
SELECT A.FirstName + ' ' + A.LastName as FullName,
		A.WindowsUserName as WinID,
		B.AccessDesc,
		B.Access,
		B.Misc,
		CAST(CASE WHEN B.AccessRemoved = 'Jan  1 1900 12:00AM' THEN NULL else B.AccessRemoved END as varchar(50)) as AccessRemoved
FROM dbo.SYSA_LST_Users A
LEFT JOIN (
				(
					SELECT WinUName as WinID, 'Misc Script Access' as AccessDesc, TypeKey as Access, '' as Misc, '' as AccessRemoved  
					FROM dbo.GENR_REF_AuthAccess 
					WHERE WinUName = @UserReport
				) 
				UNION
				(
					SELECT WindowsUserName as WinID, 'Application Access' as AccessDesc, AppOrModName as Access, '' as Misc, '' as AccessRemoved  
					FROM dbo.SYSA_REF_User_AppAndMod 
					WHERE WindowsUserName = @UserReport
				)
				UNION
				(
					SELECT WindowsUserName as WinID, 'Non AES Systems' as AccessDesc, System as Access, CAST(Notes as VARCHAR(8000)) as Misc, DtAccessRemoved as AccessRemoved  
					FROM dbo.SYSA_REF_User_Systems 
					WHERE WindowsUserName = @UserReport
				)
			) B ON A.WindowsUserName = B.WinID
WHERE A.WindowsUserName = @UserReport

END