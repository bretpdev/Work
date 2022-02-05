CREATE TABLE [scra].[_BorrowerCountComparison] (
    [BorrowerCountComparisonId] INT      IDENTITY (1, 1) NOT NULL,
    [Sent]                      INT      NULL,
    [Received]                  INT      NULL,
    [NoReturnDataReceived]      INT      NULL,
    [ExtraBorrowersReceived]    INT      NULL,
    [ComparisonDate]            DATETIME NULL,
    PRIMARY KEY CLUSTERED ([BorrowerCountComparisonId] ASC) WITH (FILLFACTOR = 95)
);

