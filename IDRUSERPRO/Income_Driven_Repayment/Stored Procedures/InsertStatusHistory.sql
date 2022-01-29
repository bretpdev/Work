CREATE PROCEDURE [dbo].[InsertStatusHistory]
	@ApplicationId int,
	@UpdatedBy varchar(50),
	@Active bit
AS
	IF((SELECT COUNT(*) FROM [dbo].[Application_Status_History] WHERE ApplicationId = @ApplicationId) = 0)
		BEGIN
			INSERT INTO Application_Status_History(ApplicationId, UpdatedBy, UpdatedAt, Active)
			VALUES(@ApplicationId, @UpdatedBy, GETDATE(), @Active)
		END
	--Check to get the most recent status if they are different then insert the new status.
	ELSE IF((
		SELECT 
			HIS.Active
		FROM 
			[dbo].[Application_Status_History] HIS
			INNER JOIN
			(
				SELECT 
					MAX(HIS.ApplicationStatusHistoryId) AS ApplicationStatusHistoryId
				FROM 
					[dbo].[Application_Status_History] HIS
				WHERE
					ApplicationId = @ApplicationId

			) MAX_APP
			ON MAX_APP.ApplicationStatusHistoryId = HIS.ApplicationStatusHistoryId) != @Active)
		BEGIN
			INSERT INTO Application_Status_History(ApplicationId, UpdatedBy, UpdatedAt, Active)
			VALUES(@ApplicationId, @UpdatedBy, GETDATE(), @Active)
		END
RETURN 0