CREATE TABLE [schrpt].[Recipients]
(
	[RecipientId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Email] VARCHAR(256) NOT NULL, 
    [CompanyName] VARCHAR(50) NULL,
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(50) NULL, 
    CONSTRAINT [CK_Recipients_Deleted] CHECK ((DeletedAt IS NULL AND DeletedBy IS NULL) OR (DeletedAt IS NOT NULL AND DeletedBy IS NOT NULL))
)

GO

CREATE TRIGGER [schrpt].[Trigger_Recipients_UpperName]
    ON [schrpt].[Recipients]
    AFTER INSERT, UPDATE
    AS
    BEGIN
		SET NOCOUNT ON;

        UPDATE r
		SET
			r.Name = UPPER(r.Name),
			r.CompanyName = UPPER(r.CompanyName)
		FROM
			schrpt.Recipients r
			INNER JOIN inserted i on r.RecipientId = i.RecipientId
    END