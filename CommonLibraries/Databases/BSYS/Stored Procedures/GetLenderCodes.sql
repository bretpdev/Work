-- =============================================
-- Author:		Bret Pehrson
-- Create date: 1/30/2014
-- Description:	
-- =============================================
CREATE PROCEDURE GetLenderCodes
	@Affiliation varchar(50) = 'UHEAA'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		LenderID
	FROM
		GENR_REF_LenderAffiliation
	WHERE
		Affiliation = @Affiliation
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLenderCodes] TO [db_executor]
    AS [dbo];

