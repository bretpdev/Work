CREATE TABLE [dbo].[SackerCache]
(
	[SackerCacheId] INT NOT NULL  IDENTITY, 
    [RequestTypeId] INT NOT NULL, 
    [Name] VARCHAR(100) NULL, 
	[Id] INT NOT NULL,
    [Status] VARCHAR(100) NOT NULL, 
    [Priority] TINYINT NULL, 
	[Court] VARCHAR(50) NULL,
	[AssignedProgrammer] VARCHAR(50) NULL,
	[AssignedTester] VARCHAR(50) NULL,
    [DevEstimate] DECIMAL(6, 2) NULL, 
    [TestEstimate] DECIMAL(6, 2) NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    [ModifiedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [ModifiedBy] VARCHAR(50) NOT NULL DEFAULT SYSTEM_USER, 
    PRIMARY KEY ([SackerCacheId]), 
    CONSTRAINT [FK_SackerCache_RequestTypes] FOREIGN KEY ([RequestTypeId]) REFERENCES [RequestTypes]([RequestTypeId]),

)

GO

CREATE TRIGGER [dbo].[Trigger_SackerCache_UpdateModified]
    ON [dbo].[SackerCache]
    FOR UPDATE
    AS
    BEGIN
        UPDATE
			[SackerCache]
		SET
			ModifiedAt = GETDATE(),
			ModifiedBy = SYSTEM_USER
		WHERE
			SackerCacheId IN (SELECT SackerCacheId FROM INSERTED)
    END