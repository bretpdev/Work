CREATE TABLE [billing].[LoanDetailData]
(
	[LoanDetailDataId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SASFIeldName] VARCHAR(100) NOT NULL,
	[VertialAlign] INT NULL, 
    [HorizontalAlign] INT NULL, 
    [Order] INT NOT NULL, 
)
