CREATE PROCEDURE [dbo].[GetQKillerQueues]
AS

	SELECT [Queue], Department from QKIL_LST_QueueKiller

RETURN 0
