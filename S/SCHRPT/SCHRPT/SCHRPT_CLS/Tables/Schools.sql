CREATE TABLE [schrpt].[Schools]
(
	[SchoolId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SchoolCode] CHAR(6) NOT NULL, 
    [BranchCode] CHAR(2) NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [AddedAt] DATETIME NOT NULL DEFAULT GETDATE(), 
    [AddedBy] VARCHAR(50) NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(50) NULL, 
    CONSTRAINT [CK_Schools_Deleted] CHECK ((DeletedAt IS NULL AND DeletedBy IS NULL) OR (DeletedAt IS NOT NULL AND DeletedBy IS NOT NULL)), 
    CONSTRAINT [CK_Schools_IntegerCodes] CHECK (ISNUMERIC(SchoolCode) = 1 AND ISNUMERIC(BranchCode) = 1)
)

GO

CREATE TRIGGER [schrpt].[Trigger_Schools_UpperName]
    ON [schrpt].[Schools]
    AFTER INSERT, UPDATE
    AS
    BEGIN
		SET NOCOUNT ON;

        UPDATE s
		SET
			s.Name = UPPER(s.Name)
		FROM
			schrpt.Schools s
			INNER JOIN inserted i on s.SchoolId = i.SchoolId
    END