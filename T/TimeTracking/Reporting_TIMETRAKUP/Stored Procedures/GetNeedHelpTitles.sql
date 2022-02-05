CREATE PROCEDURE [dbo].[GetNeedHelpTitles]
AS
	SELECT
		[Subject]
	FROM
		NeedHelpUheaa.dbo.DAT_Ticket