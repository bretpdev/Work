USE ULS
GO

GRANT EXECUTE ON [subscrprt].[GetAvailableData] TO db_executor
GRANT EXECUTE ON [subscrprt].[LoadNewData] TO db_executor
GRANT EXECUTE ON [subscrprt].[SetProcessed] TO db_executor