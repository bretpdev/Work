

CREATE PROCEDURE [dbo].[spQCTR_CriteriaResults] 

@Mode				VARCHAR(8000),
@UserID				VARCHAR(50),
@ShortVersion		VARCHAR(1) = 'N'

AS

DECLARE @CourtPosition	INT

IF @Mode = 'ALL'
BEGIN
	 SELECT DISTINCT A.[ID], 
		CASE 
			WHEN @ShortVersion = 'N' OR LEN(A.SUBJECT) <= 20 THEN A.SUBJECT
			ELSE LEFT(A.Subject, 20) + '...'
		END AS SUBJECT,
		A.SUBJECT AS  SUBJECT2,
		COALESCE(B.Status,'') as Status 
	FROM QCTR_DAT_Issue A
	LEFT JOIN  (
			SELECT BB.ISSUEID, BB.Status
			FROM (
				SELECT AAA.ISSUEID, MAX(AAA.Updated) as Updated
				FROM dbo.QCTR_DAT_Status AAA
				GROUP BY AAA.ISSUEID
				) AA
			INNER JOIN dbo.QCTR_DAT_Status BB
				ON AA.ISSUEID = BB.ISSUEID AND AA.Updated = BB.Updated 
				) B ON A.[ID] = B.ISSUEID
	LEFT JOIN dbo.QCTR_DAT_BU C
		ON A.[ID] = C.ISSUEID
	LEFT JOIN dbo.SYSA_DAT_AccessAndNotificationUserAccess D
		ON C.BU = D.BusinessUnit AND D.AccessNotificationKey = 'Authorized QC' AND D.System = 'Quality Control' AND D.EndDate IS NULL
	LEFT JOIN (
				SELECT EEE.WindowsUserName, EE.IssueID
				FROM dbo.QCTR_DAT_Responsible EE
				INNER JOIN dbo.SYSA_LST_UserIDInfo EEE
					ON EE.UserID = EEE.UserID
				) E
		ON A.[ID] = E.IssueID
	WHERE ((D.WindowsUserID = @UserID) OR (C.BU IS NULL and A.Requester = @UserID) OR (E.WindowsUserName = @UserID))
	ORDER BY A.[ID]
END
ELSE IF @Mode = 'OPEN' 
BEGIN
	 SELECT DISTINCT A.[ID], 
		CASE 
			WHEN @ShortVersion = 'N' OR LEN(A.SUBJECT) <= 20 THEN A.SUBJECT
			ELSE LEFT(A.Subject, 20) + '...'
		END AS SUBJECT,
		A.SUBJECT AS SUBJECT2,
		COALESCE(B.Status,'') as Status
	FROM QCTR_DAT_Issue A
	LEFT JOIN  (
			SELECT BB.ISSUEID, BB.Status
			FROM (
				SELECT AAA.ISSUEID, MAX(AAA.Updated) as Updated
				FROM dbo.QCTR_DAT_Status AAA
				GROUP BY AAA.ISSUEID
				) AA
			INNER JOIN dbo.QCTR_DAT_Status BB
				ON AA.ISSUEID = BB.ISSUEID AND AA.Updated = BB.Updated 
				) B ON A.[ID] = B.ISSUEID 
	LEFT JOIN dbo.QCTR_DAT_BU C
		ON A.[ID] = C.ISSUEID
	LEFT JOIN dbo.SYSA_DAT_AccessAndNotificationUserAccess D
		ON C.BU = D.BusinessUnit AND D.AccessNotificationKey = 'Authorized QC' AND D.System = 'Quality Control' AND D.EndDate IS NULL
	LEFT JOIN (
				SELECT EEE.WindowsUserName, EE.IssueID
				FROM dbo.QCTR_DAT_Responsible EE
				INNER JOIN dbo.SYSA_LST_UserIDInfo EEE
					ON EE.UserID = EEE.UserID
				) E
		ON A.[ID] = E.IssueID
	WHERE ((D.WindowsUserID = @UserID) OR (C.BU IS NULL and A.Requester = @UserID) OR (E.WindowsUserName = @UserID)) and A.Priority > 0 and B.Status NOT IN ('Withdrawn', 'Deactivate', 'Complete')
	ORDER BY A.[ID]
END
ELSE
BEGIN
	IF @ShortVersion = 'N'
	BEGIN
		--figure out if court was specified in the criteria 
		SET @CourtPosition = Patindex('%C.UserID%',@Mode)
		--run query
		EXEC('SELECT DISTINCT A.[ID], 
				A.SUBJECT,
				A.SUBJECT AS SUBJECT2,
				COALESCE(B.Status,'''') as Status
			FROM QCTR_DAT_Issue A
			LEFT JOIN  (
					SELECT BB.ISSUEID, BB.Status
					FROM (
						SELECT AAA.ISSUEID, MAX(AAA.Updated) as Updated
						FROM dbo.QCTR_DAT_Status AAA
						GROUP BY AAA.ISSUEID
						) AA
					INNER JOIN dbo.QCTR_DAT_Status BB
						ON AA.ISSUEID = BB.ISSUEID AND AA.Updated = BB.Updated 
						) B ON A.[ID] = B.ISSUEID
			LEFT JOIN dbo.QCTR_DAT_Court C
				ON A.[ID] = C.ISSUEID
				AND C.Ended IS NULL
			LEFT JOIN dbo.QCTR_DAT_BU D
				ON A.[ID] = D.ISSUEID
			LEFT JOIN dbo.SYSA_DAT_AccessAndNotificationUserAccess E
				ON D.BU = E.BusinessUnit  AND  E.AccessNotificationKey = ''Authorized QC'' AND  E.System = ''Quality Control'' AND E.EndDate IS NULL
			LEFT JOIN dbo.QCTR_DAT_Responsible F
				ON A.[ID] = F.ISSUEID
			LEFT JOIN (
				SELECT EEE.WindowsUserName, EE.IssueID
				FROM dbo.QCTR_DAT_Responsible EE
				INNER JOIN dbo.SYSA_LST_UserIDInfo EEE
					ON EE.UserID = EEE.UserID
				) G
				ON A.[ID] = G.IssueID
			WHERE ' + @Mode +  '  
			AND ((E.WindowsUserID = ''' + @UserID + ''') 
					OR (D.BU IS NULL and A.Requester = ''' + @UserID + ''') 
					OR (G.WindowsUserName = ''' + @UserID + '''))
			AND ((' + @CourtPosition + ' > 0 AND B.Status NOT IN (''Deactivated'',''Complete'',''Withdrawn''))
					OR (' + @CourtPosition + ' = 0)) 
			ORDER BY A.[ID]')
	END
	ELSE
	BEGIN
		--figure out if court was specified in the criteria 
		SET @CourtPosition = Patindex('%C.UserID%',@Mode)
		--run query
		EXEC('SELECT DISTINCT A.[ID], 
			CASE 
				WHEN LEN(A.SUBJECT) <= 20 THEN A.SUBJECT
				ELSE LEFT(A.SUBJECT, 20) + ''...''
			END AS SUBJECT,
			A.SUBJECT AS SUBJECT2,
			COALESCE(B.Status,'''') as Status
			FROM QCTR_DAT_Issue A
			LEFT JOIN  (
					SELECT BB.ISSUEID, BB.Status
					FROM (
						SELECT AAA.ISSUEID, MAX(AAA.Updated) as Updated
						FROM dbo.QCTR_DAT_Status AAA
						GROUP BY AAA.ISSUEID
						) AA
					INNER JOIN dbo.QCTR_DAT_Status BB
						ON AA.ISSUEID = BB.ISSUEID AND AA.Updated = BB.Updated 
						) B ON A.[ID] = B.ISSUEID
			LEFT JOIN dbo.QCTR_DAT_Court C
				ON A.[ID] = C.ISSUEID
				AND C.Ended IS NULL
			LEFT JOIN dbo.QCTR_DAT_BU D
				ON A.[ID] = D.ISSUEID
			LEFT JOIN dbo.SYSA_DAT_AccessAndNotificationUserAccess E
				ON D.BU = E.BusinessUnit  AND  E.AccessNotificationKey = ''Authorized QC'' AND E.WindowsUserID = ''' + @UserID + ''' AND  E.System = ''Quality Control'' AND E.EndDate IS NULL
			LEFT JOIN dbo.QCTR_DAT_Responsible F
				ON A.[ID] = F.ISSUEID
			LEFT JOIN (
				SELECT EEE.WindowsUserName, EE.IssueID
				FROM dbo.QCTR_DAT_Responsible EE
				INNER JOIN dbo.SYSA_LST_UserIDInfo EEE
					ON EE.UserID = EEE.UserID
				) G
				ON A.[ID] = G.IssueID
			WHERE ' + @Mode +  '	AND ((E.WindowsUserID = ''' + @UserID + ''') OR (D.BU IS NULL and A.Requester = ''' + @UserID + ''') OR (G.WindowsUserName = ''' + @UserID + '''))
			AND ((' + @CourtPosition + ' > 0 AND B.Status NOT IN (''Deactivated'',''Complete'',''Withdrawn''))
					OR (' + @CourtPosition + ' = 0)) 
			ORDER BY A.[ID]'
			)
		END
END