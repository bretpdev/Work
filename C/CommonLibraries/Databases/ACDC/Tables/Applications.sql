CREATE TABLE [dbo].[Applications]
(
	[ApplicationId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ApplicationName] VARCHAR(100) NOT NULL, 
    [AccessKey] VARCHAR(50) NULL, 
    [SourcePath] VARCHAR(256) NOT NULL,
	[StartingDll] VARCHAR(256) NULL,
    [StartingClass] VARCHAR(50) NULL, 
    [AddedOn] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] INT NOT NULL, 
    [RemovedOn] DATETIME NULL, 
    [RemovedBy] INT NULL, 
    CONSTRAINT [CK_Applications_StartingDll] CHECK (
		(StartingDll IS NULL AND StartingClass IS NULL) OR (StartingDll IS NOT NULL AND StartingClass IS NOT NULL)
	) 
)

GO

CREATE TRIGGER [dbo].[Trigger_Applications_ApplicationName]
    ON [dbo].[Applications]
    FOR INSERT, UPDATE
    AS
    BEGIN
        SET NoCount ON

		DECLARE @Count int =
			(SELECT
				COUNT(*)
			FROM
				Applications A
				INNER JOIN INSERTED I
					ON A.ApplicationName = I.ApplicationName
					AND A.RemovedBy IS NULL)
		IF (@Count > 1)
		BEGIN
			ROLLBACK TRANSACTION
			RAISERROR('Duplicate application names are not permitted.', 16, 1)
			RETURN
		END
    END
