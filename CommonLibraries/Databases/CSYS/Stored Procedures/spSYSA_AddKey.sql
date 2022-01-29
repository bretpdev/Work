CREATE PROCEDURE [dbo].[spSYSA_AddKey]
	@Application	VARCHAR(30),
	@Key			VARCHAR(100),
	@Type			VARCHAR(20),
	@Description	VARCHAR(8000),
	@SqlUserID		varchar(5)
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @CurrentDate DateTime = GetDate()

	INSERT INTO SYSA_LST_UserKeys (
		UserKey,
		[Application],
		[Type],
		[Description],
		[AddedBy],
		[StartDate]
	)
	VALUES (
		@Key,
		@Application,
		@Type,
		@Description,
		CONVERT(INT, @SqlUserID),
		@CurrentDate
	)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_AddKey] TO [db_executor]
    AS [dbo];

