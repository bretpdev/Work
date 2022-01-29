CREATE PROCEDURE [dbo].[spNDHP_RptComplByAnalyst] 

@AssignedTo			varchar(50),
@Start				varchar(20),
@End				varchar(20)

AS

SELECT 'Completed Tickets Assigned To ' + B.FirstName + ' ' + B.LastName AS ReportTitle, 
	A.TicketCode AS GroupIt, 
	A.Priority AS SortIt, 
	A.Ticket, 
	A.TicketCode, 
	A.Subject, 
	(CASE 
		WHEN C.EndDate <> '' THEN CAST(DATEDIFF(d,C.EndDate,D.BeginDate) AS FLOAT)
	 	ELSE CAST(DATEDIFF(d,A.Requested,D.BeginDate) AS FLOAT) END) AS TimeToComplete
FROM NDHP_DAT_Tickets A  
INNER JOIN (
		SELECT UserID, Ticket
		FROM dbo.NDHP_DAT_UpdateTicketUserIDs
		WHERE Role = 'AssignedTo' 
		) BB ON BB.Ticket = A.Ticket
INNER JOIN dbo.SYSA_LST_Users B ON BB.UserID = B.WindowsUserName
INNER JOIN (
		SELECT MAX([Sequence]) AS SubmitStart, EndDate, Ticket
		FROM dbo.NDHP_REF_Statuses
		WHERE STATUS = 'Submitting'
		GROUP BY Ticket, EndDate
		) C ON A.Ticket = C.Ticket
INNER JOIN (
		SELECT MIN([Sequence]) AS ResolvedStart, BeginDate, Ticket
		FROM dbo.NDHP_REF_Statuses
		WHERE STATUS = 'Resolved'
		GROUP BY Ticket, BeginDate
		) D ON A.Ticket = D.Ticket
WHERE A.Status = 'Resolved' AND BB.UserID = @AssignedTo AND D.BeginDate BETWEEN  CAST(@Start + ' 00:00:00' AS DateTime) AND CAST(@End + ' 23:59:59' AS DateTime)
ORDER BY GroupIt, SortIt