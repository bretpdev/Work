/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
declare @retention int = 7
declare @autoResolve bit = 1

exec SpecifySetting 0, 'Federal Pre-DECRYPTION Archive Location', 'Z:\Archive\Encrypted Files', 1
exec SpecifySetting 13, 'Federal Pre-DECRYPTION Retention (in days)', @retention, 2
exec SpecifySetting 1, 'Federal Pre-ENCRYPTION Archive Location', 'Z:\Archive\Pre-Encrypted Files', 3
exec SpecifySetting 14, 'Federal Pre-ENCRYPTION Retention (in days)', @retention, 4

exec SpecifySetting 10, 'UHEAA Pre-DECRYPTION Archive Location', '\\uheaa-fs\seas\Archive\Encrypted Files', 5
exec SpecifySetting 15, 'UHEAA Pre-DECRYPTION Retention (in days)', @retention, 6
exec SpecifySetting 11, 'UHEAA Pre-ENCRYPTION Archive Location', '\\uheaa-fs\seas\Archive\Pre-Encrypted Files', 7
exec SpecifySetting 16, 'UHEAA Pre-ENCRYPTION Retention (in days)', @retention, 8

exec SpecifySetting 6, 'UHEAA Public Keyring Location', '\\uheaa-fs\restricted\GPG Keys\UHEAA\pubring.gpg', 9
exec SpecifySetting 7, 'UHEAA Private Keyring Location', '\\uheaa-fs\restricted\GPG Keys\UHEAA\secring.gpg', 10
exec SpecifySetting 8, 'AES Public Key Location', '\\uheaa-fs\restricted\GPG Keys\AES\AES PGP Public.asc', 11
exec SpecifySetting 9, 'UHEAA Passphrase Location', '\\uheaa-fs\restricted\GPG Keys\UHEAA\passphrase.txt', 12

exec SpecifySetting 2, 'PGP Location', '%PROGRAMFILES(x86)%\GNU\GnuPG\gpg2.exe', 13
exec SpecifySetting 3, 'Error Report Location', 'Q:\Systems Support\Batch Scripts\Error Logs', 14
exec SpecifySetting 4, 'Error Report Format String', 'SftpCoordinatorErrors_{0:MM_dd_yyyy_hh_mm_ss}.csv', 15
exec SpecifySetting 5, 'Temp Folder Location', '\\UHEAA-FS\DomainUsersData\{0}\Temp\SftpCoordinator', 16
exec SpecifySetting 12, 'Automatically Resolve Naming Conflicts', @autoResolve, 17


update PathTypes 
   set [Description] = 'Normal',
       RootPath = 'C:\' --prod: '/'
 where PathTypeId = 0

update PathTypes 
   set [Description] = 'Noble SFTP',
       RootPath = 'ftp://172.16.3.11/SftpTest/' -- prod: ftp://172.16.3.11/
 where PathTypeId = 1
