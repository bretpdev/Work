-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/01/2012
-- Description:	This sp will return the number of pages a letter has and if it is duplex
-- =============================================
CREATE PROCEDURE [dbo].[spLTDBGetPageCount] 

	@LetterId as Varchar (10)
AS
BEGIN

	SET NOCOUNT ON;

	Select Top 1
	[Pages],
	[Duplex]
	From LTDB_DAT_CentralPrintingDocData
	Where ID = @LetterId
	
END