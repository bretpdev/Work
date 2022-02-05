CREATE TABLE [schrpt].[SchoolRecipients]
(
	[SchoolRecipientId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SchoolId] INT NOT NULL, 
    [RecipientId] INT NOT NULL,
	[ReportTypeId] INT NOT NULL,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(50) NULL, 
    CONSTRAINT [CK_SchoolRecipients_Deleted] CHECK ((DeletedAt IS NULL AND DeletedBy IS NULL) OR (DeletedAt IS NOT NULL AND DeletedBy IS NOT NULL)),
    CONSTRAINT [FK_SchoolRecipients_Schools] FOREIGN KEY ([SchoolId]) REFERENCES schrpt.Schools([SchoolId]), 
    CONSTRAINT [FK_SchoolRecipients_Recipients] FOREIGN KEY ([RecipientId]) REFERENCES schrpt.Recipients([RecipientId]),
    CONSTRAINT [FK_SchoolRecipients_ReportTypes] FOREIGN KEY ([ReportTypeId]) REFERENCES schrpt.ReportTypes([ReportTypeId])
)

GO

CREATE TRIGGER [schrpt].[Trigger_SchoolRecipients_OnlyOneActive]
    ON [schrpt].[SchoolRecipients]
    FOR INSERT, UPDATE
    AS
    BEGIN
		SET NOCOUNT ON;
        IF EXISTS(
			SELECT
				COUNT(*)
			FROM
				schrpt.SchoolRecipients
			WHERE
				DeletedAt IS NULL
			GROUP BY
				RecipientId, SchoolId
			HAVING 
				COUNT(*) > 1
		)
		BEGIN
			RAISERROR('Cannot have more than 1 active School/Recipient combination.', 16, 1)
			ROLLBACK TRANSACTION
		END
    END