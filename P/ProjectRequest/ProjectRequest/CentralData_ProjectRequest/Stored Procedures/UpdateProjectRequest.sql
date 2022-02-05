CREATE PROCEDURE [projectrequest].[UpdateProjectRequest]
(
	@ProjectId INT,
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

UPDATE 
	[projectrequest].[Projects]
SET
	BusinessUnitId = @BusinessUnitId,
	ProjectName = @ProjectName,
	SubmittedBy = @SubmittedBy,
	SubmittedAt = @Date,
	ProjectSummary = @ProjectSummary,
	BusinessNeed = @BusinessNeed,
	Benefits = @Benefits,
	ImplementationApproach = ISNULL(@ImplementationApproach, ''),
	ProjectStatus = @ProjectStatus,
	UpdatedAt = GETDATE(),
	UpdatedBy = SUSER_NAME()
WHERE
	ProjectId = @ProjectId

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

DECLARE @RequestorScoreTable TABLE
(
	ScoreId INT,
	Score INT
)

INSERT INTO @RequestorScoreTable(ScoreId, Score)
		SELECT
			ScoreId,
			Score
		FROM
			[projectrequest].[Scores]
		WHERE
			ScoreTypeId = @RequestorScoreTypeId
			AND ProjectId = @ProjectId
			AND Active = 1

DECLARE @UrgencyScoreTable TABLE
(
	ScoreId INT,
	Score INT
)

INSERT INTO @UrgencyScoreTable (ScoreId, Score)
	(
		SELECT
			ScoreId,
			Score
		FROM
			[projectrequest].[Scores]
		WHERE
			ScoreTypeId = @UrgencyScoreTypeId
			AND ProjectId = @ProjectId
			AND Active = 1
	)

DECLARE @RiskScoreTable TABLE
(
	ScoreId INT,
	Score INT
)

INSERT INTO @RiskScoreTable (ScoreId, Score)
(
	SELECT
		ScoreId,
		Score
	FROM
		[projectrequest].[Scores]
	WHERE
		ScoreTypeId = @RiskScoreTypeId
		AND ProjectId = @ProjectId
		AND Active = 1
)

DECLARE @ExistingRequestor BIT =
(
	SELECT	
		1
	FROM
		@RequestorScoreTable
	WHERE
		Score = @RequestorScore
)

DECLARE @ExistingUrgency BIT =
(
	SELECT	
		1
	FROM
		@UrgencyScoreTable
	WHERE
		Score = @UrgencyScore
)

DECLARE @ExistingRisk BIT =
(
	SELECT	
		1
	FROM
		@RiskScoreTable
	WHERE
		Score = @RiskScore
)

DECLARE @RequestorScoreId INT =
(
	SELECT
		ScoreId
	FROM
		@RequestorScoreTable
)

DECLARE @UrgencyScoreId INT =
(
	SELECT
		ScoreId
	FROM
		@UrgencyScoreTable
)

DECLARE @RiskScoreId INT =
(
	SELECT
		ScoreId
	FROM
		@RiskScoreTable
)

IF @RequestorScore IS NOT NULL AND (@ExistingRequestor IS NULL OR @ExistingRequestor = 0)
BEGIN
	EXEC [projectrequest].[InsertScore] @ProjectId, @RequestorScoreTypeId, @RequestorScore, @RequestorScoreId
END

IF @UrgencyScore IS NOT NULL AND (@ExistingUrgency IS NULL OR @ExistingUrgency = 0)
BEGIN
	EXEC [projectrequest].[InsertScore] @ProjectId, @UrgencyScoreTypeId, @UrgencyScore, @UrgencyScoreId
END

IF @RiskScore IS NOT NULL AND (@ExistingRisk IS NULL OR @ExistingRisk = 0)
BEGIN
	EXEC [projectrequest].[InsertScore] @ProjectId, @RiskScoreTypeId, @RiskScore, @RiskScoreId
END
