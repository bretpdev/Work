CREATE PROCEDURE [dbo].[spNDHP_RptGeneral] 

@FilterBU				varchar(50) =  'Systems Support',
@GroupIt				varchar(50) =  'Analyst',
@ReportTitle				varchar(300) ='blah'

AS


IF @FilterBU = 'Systems Support'
BEGIN
	IF  @GroupIt = 'Analyst'
	BEGIN
		SELECT @ReportTitle AS ReportTitle, 
			CAST('SS Analyst:  ' + C.AssignTo AS VARCHAR(100)) AS GroupIt, 
			CAST(A.Priority AS CHAR) AS SortIt, 
			A.Ticket, 
			A.TicketCode, 
			A.Subject, 
			A.Issue, 
			A.Status, 
			A.Requested, 
			A.Required, 
			A.Priority, 
			A.StatusDate, 
			COALESCE(B.OSA,'Not Assigned') AS OSA, 
			C.AssignTo, 
			D.FirstName+' '+D.LastName AS Requester, 
			E.FirstName+' '+E.LastName AS Court 
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
		INNER JOIN (
					SELECT DISTINCT Z.FirstName + ' ' + Z.LastName AS AssignTo, 
						Y.WindowsUserID 
					FROM  dbo.GENR_REF_BU_Agent_Xref Y
					INNER JOIN SYSA_LST_Users Z 
						ON Y.WindowsUserID = Z.WindowsUserName 
					WHERE Y.BusinessUnit = 'Systems Support' AND Y.Role = 'Member Of'
				) C ON BB.UserID = C.WindowsUserID 
		INNER JOIN SYSA_LST_Users D 
			ON CC.UserID = D.WindowsUserName 
		INNER JOIN SYSA_LST_Users E 
			ON DD.UserID = E.WindowsUserName 
		WHERE A.Status NOT IN ('Resolved','Withdrawn')
		ORDER BY GroupIt, SortIt
	END
	ELSE /*priority */
	BEGIN
		SELECT @ReportTitle AS ReportTitle, 
			CAST(A.Priority AS CHAR) AS GroupIt, 
			CAST(C.AssignTo AS VARCHAR(100)) AS SortIt, 
			A.Ticket, 
			A.TicketCode, 
			A.Subject, 
			A.Issue, 
			A.Status, 
			A.Requested, 
			A.Required, 
			A.Priority, 
			A.StatusDate, 
			COALESCE(B.OSA,'Not Assigned') AS OSA, 
			C.AssignTo, 
			D.FirstName+' '+D.LastName AS Requester, 
			E.FirstName+' '+E.LastName AS Court 
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
		INNER JOIN (
					SELECT DISTINCT Z.FirstName + ' ' + Z.LastName AS AssignTo, 
						Y.WindowsUserID 
					FROM  dbo.GENR_REF_BU_Agent_Xref Y
					INNER JOIN SYSA_LST_Users Z 
						ON Y.WindowsUserID = Z.WindowsUserName 
					WHERE Y.BusinessUnit = 'Systems Support' AND Y.Role = 'Member Of'
				) C ON BB.UserID = C.WindowsUserID 
		INNER JOIN SYSA_LST_Users D 
			ON CC.UserID = D.WindowsUserName 
		INNER JOIN SYSA_LST_Users E 
			ON DD.UserID = E.WindowsUserName 
		WHERE A.Status NOT IN ('Resolved','Withdrawn')
		ORDER BY GroupIt, SortIt
	END
END
ELSE IF  @FilterBU = 'Operations Support'
BEGIN
	IF @GroupIt = 'Analyst'
	BEGIN
		SELECT @ReportTitle AS ReportTitle, 
			CAST('OS Analyst:  ' + COALESCE(B.OSA,'Not Assigned') AS VARCHAR(100)) AS GroupIt, 
			CAST(A.Priority AS CHAR) AS SortIt, 
			A.Ticket, 
			A.TicketCode, 
			A.Subject, 
			A.Issue, 
			A.Status, 
			A.Requested, 
			A.Required, 
			A.Priority, 
			A.StatusDate, 
			COALESCE(B.OSA,'Not Assigned') AS OSA, 
			COALESCE(C.AssignTo,'Not Assigned') AS AssignTo, 
			D.FirstName+' '+D.LastName AS Requester, 
			E.FirstName+' '+E.LastName AS Court 
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
		INNER JOIN SYSA_LST_Users D 
			ON CC.UserID = D.WindowsUserName 
		INNER JOIN SYSA_LST_Users E 
			ON DD.UserID = E.WindowsUserName 
		WHERE A.Status NOT IN ('Resolved','Withdrawn')
		ORDER BY GroupIt, SortIt
	END
	ELSE
	BEGIN
		SELECT @ReportTitle AS ReportTitle, 
			CAST(A.Priority AS CHAR) AS GroupIt, 
			CAST(COALESCE(B.OSA,'Not Assigned') AS VARCHAR(100)) AS SortIt, 
			A.Ticket, 
			A.TicketCode, 
			A.Subject, 
			A.Issue, 
			A.Status, 
			A.Requested, 
			A.Required, 
			A.Priority, 
			A.StatusDate, 
			COALESCE(B.OSA,'Not Assigned') AS OSA, 
			COALESCE(C.AssignTo,'Not Assigned') AS AssignTo, 
			D.FirstName+' '+D.LastName AS Requester, 
			E.FirstName+' '+E.LastName AS Court 
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
		INNER JOIN SYSA_LST_Users D 
			ON CC.UserID = D.WindowsUserName 
		INNER JOIN SYSA_LST_Users E 
			ON DD.UserID = E.WindowsUserName 
		WHERE A.Status NOT IN ('Resolved','Withdrawn')
		ORDER BY GroupIt, SortIt
	END
END	
ELSE IF @FilterBU = 'Quality Control'
BEGIN
	IF  @GroupIt = 'Analyst'
	BEGIN
		SELECT @ReportTitle AS ReportTitle, 
			CAST('QC Analyst:  ' + C.AssignTo AS VARCHAR(100)) AS GroupIt, 
			CAST(A.Priority AS CHAR) AS SortIt, 
			A.Ticket, 
			A.TicketCode, 
			A.Subject, 
			A.Issue, 
			A.Status, 
			A.Requested, 
			A.Required, 
			A.Priority, 
			A.StatusDate, 
			COALESCE(B.OSA,'Not Assigned') AS OSA, 
			C.AssignTo, 
			D.FirstName+' '+D.LastName AS Requester, 
			E.FirstName+' '+E.LastName AS Court 
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
		INNER JOIN (
					SELECT DISTINCT Z.FirstName + ' ' + Z.LastName AS AssignTo, 
						Y.WindowsUserID 
					FROM  dbo.GENR_REF_BU_Agent_Xref Y
					INNER JOIN SYSA_LST_Users Z 
						ON Y.WindowsUserID = Z.WindowsUserName 
					WHERE Y.BusinessUnit = 'Quality Control' AND Y.Role = 'Member Of'
				) C ON BB.UserID = C.WindowsUserID 
		INNER JOIN SYSA_LST_Users D 
			ON CC.UserID = D.WindowsUserName 
		INNER JOIN SYSA_LST_Users E 
			ON DD.UserID = E.WindowsUserName 
		WHERE A.Status NOT IN ('Resolved','Withdrawn')
		ORDER BY GroupIt, SortIt
	END 
	ELSE
	BEGIN
		SELECT @ReportTitle AS ReportTitle, 
			CAST(A.Priority AS CHAR) AS GroupIt, 
			CAST(C.AssignTo AS VARCHAR(100)) AS SortIt, 
			A.Ticket, 
			A.TicketCode, 
			A.Subject, 
			A.Issue, 
			A.Status, 
			A.Requested, 
			A.Required, 
			A.Priority, 
			A.StatusDate, 
			COALESCE(B.OSA,'Not Assigned') AS OSA, 
			C.AssignTo, 
			D.FirstName+' '+D.LastName AS Requester, 
			E.FirstName+' '+E.LastName AS Court 
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
		INNER JOIN (
					SELECT DISTINCT Z.FirstName + ' ' + Z.LastName AS AssignTo, 
						Y.WindowsUserID 
					FROM  dbo.GENR_REF_BU_Agent_Xref Y
					INNER JOIN SYSA_LST_Users Z 
						ON Y.WindowsUserID = Z.WindowsUserName 
					WHERE Y.BusinessUnit ='Quality Control' AND Y.Role = 'Member Of'
				) C ON BB.UserID = C.WindowsUserID 
		INNER JOIN SYSA_LST_Users D 
			ON CC.UserID = D.WindowsUserName 
		INNER JOIN SYSA_LST_Users E 
			ON DD.UserID = E.WindowsUserName 
		WHERE A.Status NOT IN ('Resolved','Withdrawn')
		ORDER BY GroupIt, SortIt
	END
END
ELSE IF @FilterBU = 'BU'
BEGIN
	SELECT @ReportTitle AS ReportTitle, 
		CAST(A.Unit AS VARCHAR(100)) AS GroupIt, 
		CAST(A.Priority AS CHAR) AS SortIt, 
		A.Ticket, 
		A.TicketCode, 
		A.Subject, 
		A.Issue, 
		A.Status, 
		A.Requested, 
		A.Required, 
		A.Priority, 
		A.StatusDate, 
		COALESCE(B.OSA,'Not Assigned') AS OSA, 
		COALESCE(C.AssignTo,'Not Assigned') AS AssignTo, 
		D.FirstName+' '+D.LastName AS Requester, 
		E.FirstName+' '+E.LastName AS Court 
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
	INNER JOIN SYSA_LST_Users D 
		ON CC.UserID = D.WindowsUserName 
	INNER JOIN SYSA_LST_Users E 
		ON DD.UserID = E.WindowsUserName 
	WHERE A.Status NOT IN ('Resolved','Withdrawn')
	ORDER BY GroupIt, SortIt
END
/**/