CREATE PROCEDURE [accurint].[OL_AddSpecialRequestDemoRecord]
	@AccountNumber CHAR(10),
	@WorkGroup VARCHAR(8),
	@Department VARCHAR(3),
	@RunId INT
AS

	INSERT INTO accurint.DemosProcessingQueue_OL (AccountNumber, WorkGroup, Department, TaskCreatedAt, SendToAccurint, RunId) 
	OUTPUT 
		inserted.DemosId,
		inserted.AccountNumber,
		inserted.WorkGroup,
		inserted.Department,
		inserted.TaskCreatedAt,
		inserted.SendToAccurint,
		inserted.AddedToRequestFileAt,
		inserted.TaskCompletedAt,
		inserted.RequestCommentAdded, --Comment placed on request task
		inserted.AddressTaskQueueId, --Task created when processing response file
		inserted.PhoneTaskQueueId
	VALUES (@AccountNumber, @WorkGroup, @Department, NULL, 1, @RunId)

RETURN 0
