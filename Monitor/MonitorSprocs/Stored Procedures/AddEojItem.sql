CREATE PROCEDURE [monitor].[AddEojItem]
	@RunHistoryId INT,
	@EojReportId INT,
	@Ssn CHAR(9), 
    @TaskControl VARCHAR(30), 
    @ActionRequest VARCHAR(10), 
    @R0CreateDate DATETIME = NULL, 
    @MonitorReason VARCHAR(50), 
    @OldMonthlyPayment MONEY = NULL, 
    @NewMonthlyPayment MONEY = NULL, 
    @ForcedDisclosure BIT = NULL, 
    @MaxIncrease MONEY = NULL, 
    @10CreateDate DATETIME = NULL, 
    @CancelReason VARCHAR(1000) = NULL
AS
	
	INSERT INTO
		monitor.EojItems 
		(RunHistoryId, EojReportId, Ssn, TaskControl, ActionRequest, R0CreateDate, MonitorReason, OldMonthlyPayment, NewMonthlyPayment, ForcedDisclosure, MaxIncrease, [10CreateDate], CancelReason)
	VALUES           
		(@RunHistoryId, @EojReportId, @Ssn, @TaskControl, @ActionRequest, @R0CreateDate, @MonitorReason, @OldMonthlyPayment, @NewMonthlyPayment, @ForcedDisclosure, @MaxIncrease, @10CreateDate, @CancelReason)

RETURN 0
