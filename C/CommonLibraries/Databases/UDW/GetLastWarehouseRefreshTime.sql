CREATE PROCEDURE [dbo].[GetLastWarehouseRefreshTime]
AS
	SELECT 

	last_user_update as LastRefreshTime

	FROM

	sys.dm_db_index_usage_stats

	WHERE

	database_id = DB_ID('UDW')

	AND

	OBJECT_NAME(object_id) = 'LN90_FinancialTran' 

RETURN 0
