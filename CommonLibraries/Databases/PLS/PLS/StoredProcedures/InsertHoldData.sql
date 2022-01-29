CREATE PROCEDURE [ls008].[InsertHoldData]
	@TaskControlNumber VARCHAR(25),
    @DocumentControlNumber VARCHAR(18), 
    @AccountNumber CHAR(10),  
    @ActivitySeq VARCHAR(5), 
    @FollowUpDate DATETIME, 
	@PheaaUserId VARCHAR(8),
	@HoldReason varchar(300)
AS
	INSERT INTO [ls008].HoldTasks(TaskControlNumber, DocumentControlNumber, AccountNumber, ActivitySeq, FollowUpDate, PheaaUserId,HoldReason)
	VALUES(@TaskControlNumber, @DocumentControlNumber, @AccountNumber, @ActivitySeq, @FollowUpDate, @PheaaUserId, @HoldReason)
RETURN 0
