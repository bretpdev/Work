CREATE TABLE [cpp].[PaymentTypes]
(
	[PaymentTypeId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TivaFileLoanType] CHAR NOT NULL, 
    [CompassLoanType] VARCHAR(6) NOT NULL, 
    [Active] BIT NOT NULL DEFAULT 1, 
    [CreatedAt] DATE NOT NULL DEFAULT GETDATE(), 
    [CreatedBy] VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME()
)