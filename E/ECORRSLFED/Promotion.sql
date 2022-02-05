USE [CDW]
GO

INSERT INTO CDW.[ecorrslfed].[InactivationStoredProcedures] (StoredProcedureName, AddedAt, AddedBy)
VALUES
	('dbo.InactivateLetters', GETDATE(), SUSER_SNAME())
GO
