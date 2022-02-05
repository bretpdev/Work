CREATE TABLE [dbo].[LoanPrograms]
(
	[LoanProgramId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [LoanProgram] VARCHAR(6) NOT NULL, 
    [NsldsCode] VARCHAR(3) NULL, 
    [IsDirect] BIT NOT NULL
)
