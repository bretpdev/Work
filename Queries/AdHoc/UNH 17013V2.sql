USE [SftpCoordinator]
GO

UPDATE dbo.ProjectFiles 
SET DecryptFile = 1
WHERE SourceRoot IN (
'\UHEAA-SFTP\UHEAA\LGSQ',
'\UHEAA-SFTP\UHEAA\NELNET')