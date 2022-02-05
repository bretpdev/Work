


CREATE PROCEDURE [dbo].[spEMCP_CreateEmailID]
	@AccountNumber		VARCHAR(10),
	@EmailCampID		INT,
	@EmailAddress		VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


    INSERT INTO dbo.EMCP_DAT_SPEMLIDs (AccountNumber, EmailCampID, EmailAddress) VALUES (@AccountNumber, @EmailCampID, @EmailAddress)

	SELECT @@Identity as ID --return created auto number
END