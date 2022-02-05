-- =============================================
-- Author:		Jarom Ryan
-- Create date: 09/30/2013
-- Description:	Will get all of the queues to be processed by AccurintFed
-- =============================================
CREATE PROCEDURE [dbo].[spGetAccurintFedQueues]

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		[Queue]
	FROM
		[dbo].[AccurintQueues]
END