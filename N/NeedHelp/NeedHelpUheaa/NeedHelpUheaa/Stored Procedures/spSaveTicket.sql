CREATE PROCEDURE [dbo].[spSaveTicket]
	@TicketID						BIGINT,
	@TicketCode						VARCHAR(50),
	@Status							VARCHAR(50),
	@StatusDate						DATETIME,
	@CourtDate						DATETIME,
	@History						VARCHAR(MAX),					
	@Subject						VARCHAR(50) = '',
	@Unit							INT = NULL,
	@Area							VARCHAR(100) = '',
	@Required						DATETIME = NULL,
	@Issue							TEXT = '',
	@ResolutionCause				VARCHAR(50) = '',
	@ResolutionFix					TEXT = '',
	@ResolutionPrevention			TEXT = '',
	@IssueUpdate					TEXT = '',
	@PreviousStatus					VARCHAR(50) = '',
	@UrgencyOption					VARCHAR(200) = '',
	@CatOption						VARCHAR(200) = '',
	@Priority						SMALLINT = NULL,
	@CCCIssue						VARCHAR(20) = '',
	@RequestProjectNum				VARCHAR(50) = '',
	@Comments						TEXT = '',
	@RelatedQCIssues				VARCHAR(100) = '',
	@RelatedProcedures				VARCHAR(1000) = ''
AS
BEGIN 

	SET NOCOUNT ON;

	DECLARE @LastUpdated			DATETIME 
	SET @LastUpdated = GETDATE()
	
	IF @Unit = 0
	SET @Unit = NULL

	UPDATE dbo.DAT_Ticket
			SET TicketCode = @TicketCode,
			[Subject] = @Subject,
			Unit = @Unit,
			Area = @Area,
			[Required] = @Required,
			Issue = @Issue,
			ResolutionCause = @ResolutionCause,
			ResolutionFix = @ResolutionFix,
			ResolutionPrevention = @ResolutionPrevention,
			[Status] = @Status,
			StatusDate = @StatusDate,
			CourtDate = @CourtDate,
			IssueUpdate = @IssueUpdate,
			History = @History + CAST(History AS VARCHAR(MAX)),
			PreviousStatus = @PreviousStatus,
			UrgencyOption = @UrgencyOption,
			CatOption = @CatOption,
			Priority = @Priority,
			LastUpdated = @LastUpdated,
			CCCIssue = @CCCIssue,
			RequestProjectNum = @RequestProjectNum,
			Comments = @Comments,
			RelatedQCIssues = @RelatedQCIssues,
			RelatedProcedures = @RelatedProcedures
	WHERE Ticket = @TicketID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSaveTicket] TO [db_executor]
    AS [dbo];

