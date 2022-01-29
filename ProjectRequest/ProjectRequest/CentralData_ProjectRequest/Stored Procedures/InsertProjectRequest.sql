CREATE PROCEDURE [projectrequest].[InsertProjectRequest]
(
	@ProjectName VARCHAR(50),
	@SubmittedBy VARCHAR(50),
	@Department VARCHAR(50),
	@Date DATETIME,
	@ProjectSummary VARCHAR(MAX),
	@BusinessNeed VARCHAR(MAX),
	@Benefits VARCHAR(MAX),
	@ImplementationApproach VARCHAR(MAX) NULL,
	@ProjectStatus VARCHAR(100),
	@RequestorScore INT NULL,
	@UrgencyScore INT NULL,
	@RiskScore INT NULL
)
AS

DECLARE @BusinessUnitId INT = (SELECT BusinessUnitId FROM [projectrequest].BusinessUnits WHERE BusinessUnit = @Department)
IF @BusinessUnitId IS NULL
	BEGIN
		INSERT INTO [projectrequest].BusinessUnits(BusinessUnit, Active)
		VALUES(@Department,1)
		SET @BusinessUnitId = @@IDENTITY	
	END

DECLARE @ProjectRequestId INT
INSERT INTO [projectrequest].[Projects]
           ([BusinessUnitId]
           ,[FinanceScoreId]
           ,[RequestorScoreId]
           ,[UrgencyScoreId]
           ,[ResourcesScoreId]
           ,[ProjectName]
           ,[SubmittedBy]
           ,[SubmittedAt]
           ,[ProjectSummary]
           ,[BusinessNeed]
           ,[Benefits]
           ,[ImplementationApproach]
           ,[UpdatedAt]
           ,[UpdatedBy]
           ,[ArchivedAt]
           ,[ArchivedBy]
		   ,[ProjectStatus])
     VALUES
           (@BusinessUnitId
           ,NULL
           ,NULL
           ,NULL
           ,NULL
           ,@ProjectName
           ,@SubmittedBy
           ,@Date
           ,@ProjectSummary
           ,@BusinessNeed
           ,@Benefits
           ,ISNULL(@ImplementationApproach, '')
           ,GETDATE()
           ,SUSER_NAME()
           ,NULL
           ,NULL
		   ,@ProjectStatus)
SET @ProjectRequestId = @@IDENTITY

DECLARE @RequestorScoreTypeId INT = 
	(
		SELECT
			ScoreTypeId
		FROM
			[projectrequest].[ScoreTypes]
		WHERE
			ScoreType = 'Requestor Score'
	)

DECLARE @UrgencyScoreTypeId INT = 
	(
		SELECT
			ScoreTypeId
		FROM
			[projectrequest].[ScoreTypes]
		WHERE
			ScoreType = 'Urgency Score'
	)

DECLARE @RiskScoreTypeId INT = 
	(
		SELECT
			ScoreTypeId
		FROM
			[projectrequest].[ScoreTypes]
		WHERE
			ScoreType = 'Risk Score'
	)

IF @RequestorScore IS NOT NULL
BEGIN
	EXEC [projectrequest].[InsertScore] @ProjectRequestId, @RequestorScoreTypeId, @RequestorScore, NULL
END

IF @UrgencyScore IS NOT NULL
BEGIN
	EXEC [projectrequest].[InsertScore] @ProjectRequestId, @UrgencyScoreTypeId, @UrgencyScore, NULL
END

IF @RiskScore IS NOT NULL
BEGIN
	EXEC [projectrequest].[InsertScore] @ProjectRequestId, @RiskScoreTypeId, @RiskScore, NULL
END

GO