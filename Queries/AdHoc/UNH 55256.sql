use SftpCoordinator_1
go

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 636 --538 for first piece and 98 on opsdev (98 will change for live)

--538 total
--select sourceroot, REPLACE(SourceRoot, '\uheaa-fs\seas\','\fsuheaaxyz\seas\') from ProjectFiles where SourceRoot like '%uheaa-fs\seas\%' --12
--select sourceroot, REPLACE(SourceRoot, '\uheaa-fs\seascs\','\fsuheaaxyz\seascs\')  from ProjectFiles where SourceRoot like '%uheaa-fs\seascs\%' --54
--select sourceroot, REPLACE(SourceRoot, '\uheaa-fs\restricted\','\fsuheaaq\restricted\')  from ProjectFiles where SourceRoot like '%uheaa-fs\restricted\%' --12

--select DestinationRoot, REPLACE(DestinationRoot, '\uheaa-fs\seas\','\fsuheaaxyz\seas\') from ProjectFiles where DestinationRoot like '%uheaa-fs\seas\%' --127
--select DestinationRoot, REPLACE(DestinationRoot, '\uheaa-fs\seascs\','\fsuheaaxyz\seascs\') from ProjectFiles where DestinationRoot like '%uheaa-fs\seascs\%' --170
--select DestinationRoot, REPLACE(DestinationRoot, '\uheaa-fs\restricted\','\fsuheaaq\restricted\') from ProjectFiles where DestinationRoot like '%uheaa-fs\restricted\%' --163

update
	SftpCoordinator_1..ProjectFiles
set
	SourceRoot = REPLACE(SourceRoot, '\uheaa-fs\seas\','\fsuheaaxyz\seas\')
WHERE
	SourceRoot like '%uheaa-fs\seas\%'

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

update
	SftpCoordinator_1..ProjectFiles
set
	SourceRoot = REPLACE(SourceRoot, '\uheaa-fs\seascs\','\fsuheaaxyz\seascs\')
WHERE
	SourceRoot like '%uheaa-fs\seascs\%'

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

update
	SftpCoordinator_1..ProjectFiles
set
	SourceRoot = REPLACE(SourceRoot, '\uheaa-fs\restricted\','\fsuheaaq\restricted\')
WHERE
	SourceRoot like '%uheaa-fs\restricted\%'

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

update
	SftpCoordinator_1..ProjectFiles
set
	DestinationRoot = REPLACE(DestinationRoot, '\uheaa-fs\seas\','\fsuheaaxyz\seas\')
where
	DestinationRoot like '%uheaa-fs\seas\%'

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

update
	SftpCoordinator_1..ProjectFiles
set
	DestinationRoot = REPLACE(DestinationRoot, '\uheaa-fs\seascs\','\fsuheaaxyz\seascs\')
where
	DestinationRoot like '%uheaa-fs\seascs\%'

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

update
	SftpCoordinator_1..ProjectFiles
set
	DestinationRoot = REPLACE(DestinationRoot, '\uheaa-fs\restricted\','\fsuheaaxyz\restricted\')
where
	DestinationRoot like '%uheaa-fs\restricted\%'

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

update --98
	PF
set
	PF.sourceroot = PF_1.Sourceroot,
	PF.destinationRoot = PF_1.destinationRoot
FROM
	SftpCoordinator..ProjectFiles PF
	INNER JOIN SftpCoordinator_1..ProjectFiles PF_1
		ON PF.ProjectFileId = PF_1.ProjectFileId

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		--COMMIT TRANSACTION
		ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
