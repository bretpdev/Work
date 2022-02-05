CREATE TABLE [pendingnocalls].[BorrowerCallSuspensions] (
    [BorrowerCallSuspensionId] INT            IDENTITY (1, 1) NOT NULL,
    [AccountNumber]            CHAR (10)      NOT NULL,
    [SubmittedBy]              NVARCHAR (100) DEFAULT (suser_sname()) NOT NULL,
    [SubmittedAt]              DATETIME       DEFAULT (getdate()) NOT NULL,
    [StartDate]                DATETIME       NOT NULL,
    [EndDate]                  DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([BorrowerCallSuspensionId] ASC)
);

