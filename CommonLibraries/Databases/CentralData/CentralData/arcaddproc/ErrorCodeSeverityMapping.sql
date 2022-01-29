CREATE TABLE [arcaddproc].[ErrorCodeSeverityMapping]
(
	[ErrorCodeSeverityMappingId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ErrorCode] VARCHAR(5) NOT NULL, 
	[ProcessingAttempts] INT NOT NULL,
    [NotificationSeverityTypeId] INT NOT NULL, 
    [RequeueHours] INT NULL, 
    [Test] NCHAR(10) NULL, 
)
