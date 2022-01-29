-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/01/2012
-- Description:	Will return the Document file path from Letter tracking Db
-- =============================================
CREATE PROCEDURE [dbo].[spLTDBGetPathAndName]

	@LetterId As Varchar(10)

AS
BEGIN

	SET NOCOUNT ON;

	Select Top 1 
	[Path] as OriginalDBEntry
	From LTDB_DAT_CentralPrintingDocData
	Where ID = @LetterId

END