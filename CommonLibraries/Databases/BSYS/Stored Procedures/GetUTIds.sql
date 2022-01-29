CREATE PROCEDURE [dbo].[GetUTIds]
	@LabelPrefix varchar(20),
	@LabelLen int
AS
	select 
		MAX(cast(right(UserID, LEN(UserID) - @LabelLen) as int)) 
	from 
		dbo.SYSA_LST_UserIDInfo 
	where 
		[Date Access Removed] is NULL 
		and UserID like @LabelPrefix+'%'
RETURN 0
