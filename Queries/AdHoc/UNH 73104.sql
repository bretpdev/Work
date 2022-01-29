USE CentralData
GO

INSERT INTO CentralData.dbo.ActiveDirRoles(ScriptId, RoleId, CreatedAt)
VALUES
('LSLETTERSU','ROLE - Division II Supervisor – Compliance',GETDATE()),
('LSLETTERSU','ROLE - Audit Coordinator',GETDATE()),
('LSLETTERSU','ROLE - Audit Coordinator plus Client Services',GETDATE()),
('LSLETTERSU','ROLE - UHEAA Processing REP',GETDATE())
