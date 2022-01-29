CREATE TABLE [deskaudits].[Audits]
(
	[AuditId] INT NOT NULL PRIMARY KEY IDENTITY,
	[Auditor] VARCHAR(250) NOT NULL,
	[Auditee] VARCHAR(250) NOT NULL,
	[Passed] BIT NOT NULL,
	[CommonFailReasonId] INT NULL,
	[CustomFailReasonId] INT NULL,
	[AuditDate] DATETIME NOT NULL,
	[CreatedAt] DATETIME DEFAULT GETDATE() NOT NULL,
	[DeletedAt] DATETIME NULL,
	[DeletedBy] VARCHAR(250) NULL
)
