CREATE PROCEDURE dbo.spNDHP_SystemConversion 


AS

DELETE FROM dbo.NDHP_DAT_Tickets WHERE Ticket > 0 /* delete all former entries */

/*insert in ticket values from former live table */
INSERT INTO dbo.NDHP_DAT_Tickets
SELECT Ticket, 
	TicketCode, 
	Subject,
	Requester,
	Requested, 
	Unit,
	Area,
	Required,
	Issue,
	'' AS ResolutionCause,
	Resolution AS ResolutionFix, 
	'' AS ResolutionPrevention,
	CASE WHEN Status = 'BO Approval' THEN 'BS Approval' ELSE Status END AS Status,
	StatusDate,
	Court,
	CourtDate,
	IssueUpdate,
	History, 
	PreviousStatus,
	PreviousCourt,
	'' AS UrgencyOption,
	'' AS CatOption,
	Priority,
	GETDATE() AS LastUpdated,
	'' AS CCCIssue,
	'' AS RequestProjectNum,
	'' AS AssignedTo,
	'' AS Comments
FROM dbo.NDHP_DAT_Tickets_ConversionTemp


/* Delete data in status table */
DELETE FROM dbo.NDHP_REF_Statuses WHERE [Sequence] > 0 

/* insert Status values from former live table */
INSERT INTO dbo.NDHP_REF_Statuses 
SELECT Ticket, Status, BeginDate, EndDate, Court FROM dbo.NDHP_REF_StatusesConversionTemp


/* hold tickets that need to be converted from BO Approval to BS Approval in temp table */
SELECT Ticket
INTO #BOTickets
FROM dbo.NDHP_REF_StatusesConversionTemp
WHERE Status = 'BO Approval'


/* update current BO Approval tickets to BS Approval */
UPDATE dbo.NDHP_REF_Statuses SET EndDate = GETDATE() WHERE Status = 'BO Approval'

/* Insert rows for formerly BO Approval tickets to BS Approval tickets */
INSERT INTO dbo.NDHP_REF_Statuses (Ticket, Status, BeginDate, EndDate, Court)
SELECT Ticket, 'BS Approval' as Status, GETDATE() as BeginDate, '' as EndDate, 'bcox' as Court
FROM #BOTickets


/* delete former data in email table */
DELETE FROM dbo.NDHP_REF_EMailRecipients WHERE TICKET > 0

/* insert email records */
INSERT INTO dbo.NDHP_REF_EMailRecipients 
SELECT Ticket, eMail1
FROM dbo.NDHP_DAT_Tickets_ConversionTemp
WHERE eMail1 <> '' and eMail1 is not null

INSERT INTO dbo.NDHP_REF_EMailRecipients 
SELECT Ticket, eMail2
FROM dbo.NDHP_DAT_Tickets_ConversionTemp
WHERE eMail2 <> '' and eMail2 is not null

INSERT INTO dbo.NDHP_REF_EMailRecipients 
SELECT Ticket, eMail3
FROM dbo.NDHP_DAT_Tickets_ConversionTemp
WHERE eMail3 <> '' and eMail3 is not null

INSERT INTO dbo.NDHP_REF_EMailRecipients 
SELECT Ticket, eMail4
FROM dbo.NDHP_DAT_Tickets_ConversionTemp
WHERE eMail4 <> '' and eMail4 is not null

INSERT INTO dbo.NDHP_REF_EMailRecipients 
SELECT Ticket, eMail5
FROM dbo.NDHP_DAT_Tickets_ConversionTemp
WHERE eMail5 <> '' and eMail5 is not null

DROP TABLE #BOTickets