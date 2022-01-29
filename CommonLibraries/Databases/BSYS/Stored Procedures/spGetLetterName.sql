-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/05/2012
-- Description:	Will return the Letter Name from the letter id
-- =============================================
CREATE PROCEDURE [dbo].[spGetLetterName] 

	@letterId As Varchar(10)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DocName FROM LTDB_DAT_DocDetail WHERE ID = @letterId

END