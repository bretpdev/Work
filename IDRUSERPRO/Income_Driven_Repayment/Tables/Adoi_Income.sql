CREATE TABLE [dbo].[Adoi_Income]
(
	[adoi_income_id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [income_source_id] INT NULL,
	[income_changed] BIT NULL,
	[income_taxable] BIT NOT NULL,
	[supporting_docs_required] BIT NOT NULL,
	[received_date] DATETIME NULL, 
    CONSTRAINT [FK_Adoi_Income_Income_Source] FOREIGN KEY ([income_source_id]) REFERENCES [Income_Source]([income_source_id])
)
