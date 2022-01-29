




CREATE PROCEDURE [dbo].[spNDHP_SaveTicket] 

@Ticket 		bigint,
@TicketCode		char(3),
@Subject		varchar(50),
@Requester		nvarchar(50),
@Requested		datetime,
@BU			varchar(50),
@FunctionalArea	varchar(100),
@Issue			text,
@Status		varchar(50),
@Court			nvarchar(50),
@Update		text,
@History		text,
@ResCause		varchar(50),
@Fix			text,
@Prevention		text,
@CourtDt		datetime,
@StatusDt		datetime,
@AssignedTo		nvarchar(50),
@Systems		varchar(8000),
@Req_Proj		varchar(50),
@CCC			varchar(20),
@Priority		smallint,
@Category		varchar(200),
@Urgency		varchar(200),
@RequiredDt		datetime,
@Email			varchar(8000),
@Comments		text,
@SelectedOption	varchar(50)

AS

DECLARE @SingleEmail 	varchar(50)
DECLARE @SingleSystem 	varchar(50)
DECLARE @LastPosition	int
DECLARE @PreviousStatus	varchar(50)
DECLARE @PreviousCourt	varchar(50)

/*  don't add '' to required date field, it doesn't display well on the interface */
IF @RequiredDt	= ''
BEGIN
	SET @RequiredDt = NULL
END

/* do court, status (and assigned to for submit status) population */
/* decide which ticket type is being saved */
IF @TicketCode = 'FAR'
BEGIN
	/* financial adjustments */
	IF @SelectedOption = 'Submit' 
	BEGIN
		/* if ticket is being submitted */
		IF @AssignedTo = ''
		BEGIN
			/* manager of operational accounting */
			SET @AssignedTo = (SELECT TOP 1 WindowsUserID
						FROM GENR_REF_BU_Agent_Xref
						WHERE Role = 'Manager' AND BusinessUnit = 'Operational Accounting')
		END
		/* move to BS manager's court */
		SET @Court = (SELECT TOP 1 WindowsUserID
					FROM GENR_REF_BU_Agent_Xref
					WHERE Role = 'Manager' AND BusinessUnit = 'BS')
		/* set status to discussion */
		SET @Status = 'BS Approval'
	END
	ELSE IF @SelectedOption = 'Return' 
	BEGIN
		/* return ticket to status based off current status */
		SET @Court = @Requester
		SET  @Status = 'Submitting' 
	END
	ELSE IF @SelectedOption = 'Previous Status' 
	BEGIN
		/* return to previous status */
		SET @Court = (SELECT     UserID FROM NDHP_DAT_UpdateTicketUserIDs WHERE (Ticket = @Ticket) AND (Role = 'PreviousCourt'))
		SET @Status = (SELECT PreviousStatus FROM dbo.NDHP_DAT_Tickets WHERE Ticket = @Ticket)
	END
	ELSE IF @SelectedOption = 'Hold_Release' 
	BEGIN
		IF @Status = 'Hold'
		BEGIN
			/* return to previous status for release */
			SET @Court = (SELECT     UserID FROM NDHP_DAT_UpdateTicketUserIDs WHERE (Ticket = @Ticket) AND (Role = 'PreviousCourt'))
			SET @Status = (SELECT PreviousStatus FROM dbo.NDHP_DAT_Tickets WHERE Ticket = @Ticket)
		END
		ELSE
		BEGIN
			/* ticket placed on hold */
			SET @Status = 'Hold'
		END
	END
	ELSE IF @SelectedOption = 'Completed'
	BEGIN
		SET @Court = (SELECT TOP 1 WindowsUserID
					FROM GENR_REF_BU_Agent_Xref
					WHERE Role = 'QC Assigned' AND BusinessUnit = @BU)
		SET @Status = 'QC Approval'
	END
	ELSE IF @SelectedOption = 'QCApproval'
	BEGIN
		SET @Court = ''
		SET @Status = 'Resolved'
	END
	ELSE IF @SelectedOption = 'BSApproval'
	BEGIN
		SET @Status = 'OPA - In Progress'
		SET @Court = (SELECT TOP 1 WindowsUserID
				FROM GENR_REF_BU_Agent_Xref
				WHERE Role = 'Manager' AND BusinessUnit = 'Operational Accounting')
	END
	ELSE IF @SelectedOption = 'Withdraw'
	BEGIN
		SET @Status = 'Withdrawn'
		SET @Court = ''
	END
END
ELSE IF @TicketCode = 'FAC'
BEGIN
	/* Facilities Requests */
	IF @SelectedOption = 'Submit' 
	BEGIN
		/* if ticket is being submitted */
		/* move manager of Physical Facilities */
		SET @Court = (SELECT TOP 1 WindowsUserID
						FROM GENR_REF_BU_Agent_Xref
						WHERE Role = 'Manager' AND BusinessUnit = 'Physical Facilities')
		/* set status to discussion */
		SET @Status = 'Requested'

	END
	ELSE IF @SelectedOption = 'Close'
	BEGIN
		SET @Court = ''
		SET @Status = 'Resolved'
	END
	/* manager of Physical Facilities */
	SET @AssignedTo = (SELECT TOP 1 WindowsUserID
				FROM GENR_REF_BU_Agent_Xref
				WHERE Role = 'Manager' AND BusinessUnit = 'Physical Facilities')
END
ELSE
BEGIN
	/* most tickets */
	IF @SelectedOption = 'Submit' 
	BEGIN
		/* if ticket is being submitted */
		IF @AssignedTo = ''
		BEGIN
			/* if ticket hasn't been assigned to anyone yet then do assignment */
			IF @TicketCode = 'OTH' OR @TicketCode = 'PRB' OR @TicketCode = 'FNC'   /* system problems */
			BEGIN 
				/* cycle through members of Systems Support */
				SET @AssignedTo =  (SELECT     TOP 1 A.WindowsUserID
									FROM         NDHP_LST_PreDefAssignToStaff AS A LEFT OUTER JOIN
										(SELECT     NDHP_DAT_UpdateTicketUserIDs.UserID, MAX(NDHP_DAT_Tickets.Ticket) AS TheMax
										FROM          NDHP_DAT_Tickets INNER JOIN
                                                   NDHP_DAT_UpdateTicketUserIDs ON NDHP_DAT_Tickets.Ticket = NDHP_DAT_UpdateTicketUserIDs.Ticket
										WHERE      (NDHP_DAT_UpdateTicketUserIDs.Role = 'AssignedTo')
										GROUP BY NDHP_DAT_UpdateTicketUserIDs.UserID) AS B ON A.WindowsUserID = B.UserID
									WHERE     (A.TicketCode = @TicketCode)
									ORDER BY B.TheMax)
				/* check if assigned to is populated, if not then assign to manager */
				IF @AssignedTo = '' OR @AssignedTo IS NULL
				BEGIN
					SET @AssignedTo = (SELECT TOP 1 WindowsUserID
								FROM GENR_REF_BU_Agent_Xref
								WHERE Role = 'Manager' AND BusinessUnit = @BU)
				END
			END
			ELSE IF @TicketCode = 'POL' /* policy */
			BEGIN
				/* get QC analyst assigned to BU */
				SET @AssignedTo = (SELECT TOP 1 WindowsUserID
							FROM GENR_REF_BU_Agent_Xref
							/*WHERE Role = 'QC Analysis' AND BusinessUnit = @BU)*/
							WHERE Role = 'Member Of' AND BusinessUnit = 'Compliance')
				/* check if assigned to is populated, if not then assign to manager */
				IF @AssignedTo = '' OR @AssignedTo IS NULL
				BEGIN
					SET @AssignedTo = (SELECT TOP 1 WindowsUserID
								FROM GENR_REF_BU_Agent_Xref
								/*WHERE Role = 'Manager' AND BusinessUnit = @BU)*/
								WHERE Role = 'Manager' AND BusinessUnit = 'Compliance')
				END
			END
			ELSE IF @TicketCode = 'FAR' /* policy */
			BEGIN
				IF @AssignedTo = '' OR @AssignedTo IS NULL
				BEGIN /*Tram wants these always assigned to her as per spec*/
					SET @AssignedTo = (SELECT TOP 1 WindowsUserID
								FROM GENR_REF_BU_Agent_Xref
								/*WHERE Role = 'Manager' AND BusinessUnit = @BU)*/
								WHERE Role = 'Manager' AND BusinessUnit = 'Operational Accounting')
				END
			END
			ELSE
			BEGIN
				/* BU manager */
				SET @AssignedTo = (SELECT TOP 1 WindowsUserID
							FROM GENR_REF_BU_Agent_Xref
							WHERE Role = 'Manager' AND BusinessUnit = @BU)
			END
		END
		/* move to assigned staff's court */
		SET @Court = @AssignedTo
		/* set status to discussion */
		SET @Status = 'Discussion'
	END
	ELSE IF @SelectedOption = 'Return' 
	BEGIN
		/* return ticket to status based off current status */
		IF  @Status = 'Discussion'
		BEGIN
			SET @Court = @Requester
			SET  @Status = 'Submitting' 
		END
		ELSE
		BEGIN
			SET @Court =  (SELECT TOP 1 WindowsUserID
					FROM GENR_REF_BU_Agent_Xref
					WHERE Role = 'Manager' AND BusinessUnit = @BU)
			SET  @Status = 'Discussion'
		END
	END
	ELSE IF @SelectedOption = 'Previous Status' 
	BEGIN
		/* return to previous status */
		SET @Court = (SELECT UserID FROM NDHP_DAT_UpdateTicketUserIDs WHERE (Ticket = @Ticket) AND (Role = 'PreviousCourt'))
		SET @Status = (SELECT PreviousStatus FROM dbo.NDHP_DAT_Tickets WHERE Ticket = @Ticket)
	END
	ELSE IF @SelectedOption = 'Hold_Release' 
	BEGIN
		IF @Status = 'Hold'
		BEGIN
			/* return to previous status for release */
			SET @Court = (SELECT UserID FROM NDHP_DAT_UpdateTicketUserIDs WHERE (Ticket = @Ticket) AND (Role = 'PreviousCourt'))
			SET @Status = (SELECT PreviousStatus FROM dbo.NDHP_DAT_Tickets WHERE Ticket = @Ticket)
		END
		ELSE
		BEGIN
			/* ticket placed on hold */
			SET @Status = 'Hold'
		END
	END
	ELSE IF @SelectedOption = 'Resolution'
	BEGIN
		SET @Court = (SELECT TOP 1 WindowsUserID
					FROM GENR_REF_BU_Agent_Xref
					WHERE Role = 'Manager' AND BusinessUnit = 'BS')
		SET @Status = 'BS Approval'
	END
	ELSE IF @SelectedOption = 'BSApproval'
	BEGIN
		SET @Status = 'Resolved'
		SET @Court = ''
	END
	ELSE IF @SelectedOption = 'Withdraw'
	BEGIN
		SET @Status = 'Withdrawn'
		SET @Court = ''
	END
END

/* action resulted in court or status change */
IF ((SELECT UserID FROM NDHP_DAT_UpdateTicketUserIDs WHERE (Ticket = @Ticket) AND (Role = 'Court')) <> @Court) OR ((SELECT Status FROM dbo.NDHP_DAT_Tickets WHERE Ticket = @Ticket) <> @Status) 
BEGIN
	/* update previous status and court fields if the status changes */
	IF (SELECT Status FROM dbo.NDHP_DAT_Tickets WHERE Ticket = @Ticket) <> @Status
	BEGIN 
		SET @PreviousStatus = (SELECT Status FROM dbo.NDHP_DAT_Tickets WHERE Ticket = @Ticket)
		SET @PreviousCourt = (SELECT UserID FROM NDHP_DAT_UpdateTicketUserIDs WHERE (Ticket = @Ticket) AND (Role = 'PreviousCourt'))
		SET @StatusDt = GETDATE() 
	END
	ELSE
	BEGIN
		/* get prvious status and court (equals the already set previous status and court) */
		SET @PreviousStatus = (SELECT PreviousStatus FROM dbo.NDHP_DAT_Tickets WHERE Ticket = @Ticket)
		SET @PreviousCourt = (SELECT UserID FROM NDHP_DAT_UpdateTicketUserIDs WHERE (Ticket = @Ticket) AND (Role = 'PreviousCourt'))
	END
	/* update status history table for both status and court changes because status table is sued for tracking court times*/
	EXEC spNDHP_StatusUpdate @Ticket, @Status, @Court
END
ELSE /* court and status didn't change */
BEGIN
	/* get prvious status and court (equals the already set previous status and court) */
	SET @PreviousStatus = (SELECT PreviousStatus FROM dbo.NDHP_DAT_Tickets WHERE Ticket = @Ticket)
	SET @PreviousCourt = (SELECT UserID FROM NDHP_DAT_UpdateTicketUserIDs WHERE (Ticket = @Ticket) AND (Role = 'PreviousCourt'))
END

/* update main ticket record */
UPDATE dbo.NDHP_DAT_Tickets 
SET 	TicketCode = @TicketCode,
	Subject = @Subject,
	--Requester = @Requester,
	Requested = @Requested,
	Unit = @BU,
	Area = @FunctionalArea,
	Issue = @Issue,
	Status = @Status,
	--Court = @Court,
	IssueUpdate = @Update,
	History = @History,
	ResolutionCause = @ResCause,
	ResolutionFix = @Fix,
	ResolutionPrevention = @Prevention,
	CourtDate = @CourtDt, 
	StatusDate = @StatusDt,
 	--AssignedTo = @AssignedTo,
	RequestProjectNum = @Req_Proj,
	CCCIssue = @CCC,
	Priority = @Priority,
	CatOption = @Category,
	UrgencyOption = @Urgency,
	Required = @RequiredDt,
	LastUpdated = GETDATE(),
	PreviousStatus = @PreviousStatus,
	--PreviousCourt = @PreviousCourt,
	Comments = @Comments
WHERE 	Ticket = @Ticket



-- Saves values to different table to allow for cascading updates to WinUserID
Exec spNDHP_SaveTicketUpdate @Ticket, 'Requester', @Requester
Exec spNDHP_SaveTicketUpdate @Ticket, 'Court', @Court
Exec spNDHP_SaveTicketUpdate @Ticket, 'AssignedTo', @AssignedTo
Exec spNDHP_SaveTicketUpdate @Ticket, 'PreviousCourt', @PreviousCourt

/* add individual entries into appropriate tables */

SET @SingleEmail = ''

/* delete current entries for the ticket out of the tables */
DELETE FROM dbo.NDHP_REF_EMailRecipients WHERE Ticket = @Ticket
DELETE FROM dbo.NDHP_REF_Systems WHERE Ticket = @Ticket

/* populate email recipients */
WHILE @Email <> ''
BEGIN
	SET @LastPosition = CHARINDEX(';',@Email)
	IF @LastPosition = 0
	BEGIN
		/* either last entry or the string only had one entry */
		SET @SingleEmail = @Email
		SET @Email = ''
	END
	ELSE
	BEGIN
		/* pull out first entry */
		SET @SingleEmail = LEFT(@Email, CHARINDEX(';',@Email) - 1)
		/* check if there is another delimiter to handle */ 
		IF CHARINDEX(';',@Email, @LastPosition) > 0
		BEGIN
			/* trim off the part of the string that has already been processed */
			SET @Email = RIGHT(@Email, (LEN(@Email) - (@LastPosition)))
		END
	END
	INSERT INTO dbo.NDHP_REF_EMailRecipients VALUES (@Ticket, @SingleEmail)
END

SET @SingleSystem = ''

/* populate systems */
WHILE @Systems <> ''
BEGIN
	SET @LastPosition = CHARINDEX(';',@Systems)
	IF @LastPosition = 0
	BEGIN
		/* either last entry or the string only had one entry */
		SET @SingleSystem = @Systems
		SET @Systems = ''
	END
	ELSE
	BEGIN
		/* pull out first entry */
		SET @SingleSystem = LEFT(@Systems, CHARINDEX(';',@Systems) - 1)
		/* check if there is another delimiter to handle */ 
		IF CHARINDEX(';',@Systems, @LastPosition) > 0
		BEGIN
			/* trim off the part of the string that has already been processed */
			SET @Systems = RIGHT(@Systems, (LEN(@Systems) - (@LastPosition)))
		END
	END
	INSERT INTO dbo.NDHP_REF_Systems VALUES (@Ticket, @SingleSystem)
END