CREATE TABLE [monitor].[ExemptLoanStatuses]
(
	[ExemptLoanStatusId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LoanStatusCode] CHAR(2) NOT NULL, 
    [LoanStatusDescription] VARCHAR(50) NOT NULL
)
