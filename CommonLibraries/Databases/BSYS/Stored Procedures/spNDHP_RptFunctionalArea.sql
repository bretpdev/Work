CREATE PROCEDURE [dbo].[spNDHP_RptFunctionalArea] 

@Start			VARCHAR(20),
@End			VARCHAR(20),
@Area			VARCHAR(200)

AS

SELECT 'Tickets for "' + @Area + '" Area'  AS Title, 
	A.History,
	A.Ticket, 
	A.TicketCode, 
	A.Subject,  
	A.Status, 
	A.Requested, 
	A.Required, 
	A.Priority, 
	A.StatusDate, 
	COALESCE(B.OSA,'Not Assigned') AS OSA, 
	COALESCE(C.AssignTo,'Not Assigned') AS AssignTo, 
	D.FirstName+' '+D.LastName AS Requester, 
	E.FirstName+' '+E.LastName AS Court,
	A.LastUpdated
FROM NDHP_DAT_Tickets A 
INNER JOIN (
		SELECT UserID, Ticket
		FROM dbo.NDHP_DAT_UpdateTicketUserIDs
		WHERE Role = 'AssignedTo' 
		) BB ON BB.Ticket = A.Ticket
INNER JOIN (
		SELECT UserID, Ticket
		FROM dbo.NDHP_DAT_UpdateTicketUserIDs
		WHERE Role = 'Requester' 
		) CC ON CC.Ticket = A.Ticket
INNER JOIN (
		SELECT UserID, Ticket
		FROM dbo.NDHP_DAT_UpdateTicketUserIDs
		WHERE Role = 'Court' 
		) DD ON DD.Ticket = A.Ticket
LEFT OUTER JOIN (
			SELECT DISTINCT Z.FirstName + ' ' + Z.LastName AS OSA, 
				Y.BusinessUnit 
			FROM GENR_REF_BU_Agent_Xref Y  
			INNER JOIN SYSA_LST_Users Z 
				ON Y.WindowsUserID = Z.WindowsUserName 
				AND Y.Role = 'OS Assigned'
		) B ON A.Unit = B.BusinessUnit 
LEFT OUTER JOIN (
			SELECT DISTINCT Z.FirstName + ' ' + Z.LastName AS AssignTo, 
				Z.WindowsUserName 
			FROM  SYSA_LST_Users Z 
		) C ON BB.UserID = C.WindowsUserName 
LEFT OUTER JOIN SYSA_LST_Users D 
	ON CC.UserID = D.WindowsUserName 
LEFT OUTER JOIN SYSA_LST_Users E 
	ON DD.UserID = E.WindowsUserName 
WHERE A.LastUpdated BETWEEN CAST(@Start + ' 00:00:00' AS DateTime) AND CAST(@End + ' 23:59:59' AS DateTime) AND A.Area = @Area