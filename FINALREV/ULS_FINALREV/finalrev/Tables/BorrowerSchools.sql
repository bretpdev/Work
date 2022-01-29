CREATE TABLE [finalrev].[BorrowerSchools]
(
	[BorrowerSchoolId] INT NOT NULL PRIMARY KEY IDENTITY,
	[BorrowerRecordId] INT NOT NULL,
	[SchoolsId] INT NOT NULL, 
    CONSTRAINT [FK_BorrowerSchools_Schools] FOREIGN KEY ([SchoolsId]) REFERENCES [finalrev].[Schools]([SchoolsId]), 
    CONSTRAINT [FK_BorrowerSchools_BorrowerRecord] FOREIGN KEY ([BorrowerRecordId]) REFERENCES [finalrev].[BorrowerRecord]([BorrowerRecordId])
)
