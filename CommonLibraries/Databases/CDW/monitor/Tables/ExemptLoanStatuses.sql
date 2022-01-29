CREATE TABLE [monitor].[ExemptLoanStatuses] (
    [ExemptLoanStatusId]    INT          IDENTITY (1, 1) NOT NULL,
    [LoanStatusCode]        CHAR (2)     NOT NULL,
    [LoanStatusDescription] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([ExemptLoanStatusId] ASC)
);

