USE NobleCalls
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = XXXXX

UPDATE 
	TX
SET TX.ActivityDate = TX.ActivityDate
--SELECT TX.ActivityDate, TX.ActivityDate,*
FROM [NobleCalls].[dbo].[NobleCallHistory] TX
LEFT JOIN
(
	SELECT 
		CAST([Call Date] AS DATETIME) + CAST(Call_time AS DATETIME) AS ActivityDate
		,*
	FROM(

		SELECT 
			act_date AS [Call Date]
			,seqno AS NobleRowId
			,tsr [Noble ID]
			,X AS IsInbound
			,time_connect [Connect Time]
			,time_acw AS [ACW]
			,appl AS [Campaign]
			,status
			,addi_status
			,call_type
			,CONCAT(Areacode,Phone) AS [Phone Number]
			,CONVERT(TIME(X),CAST((act_time / XXXXX) % XXX AS varchar(X)) + ':' + CAST((act_time / XXX) % XXX AS varchar(X)) + ':' + CAST(act_time % XXX AS VARCHAR(X))) AS Call_time
		FROM OPENQUERY
			(UHEAARAS,'
				--Outbound
				SELECT * FROM  call_historyXXXXXX UNION ALL
				SELECT * FROM  call_historyXXXXXX UNION ALL
				SELECT * FROM  call_historyXXXXXX UNION ALL
				SELECT * FROM  call_historyXXXXXX UNION ALL
				SELECT * FROM  call_historyXXXXXX
			')


		UNION ALL

		SELECT 
			call_date AS [Call Date]
			,record_id AS NobleRowId
			,tsr [Noble ID]
			, X AS IsInbound
			,time_connect AS [Connect Time]
			,time_acwork AS [ACW]
			,appl AS [Campaign]
			,status
			,addi_status
			,call_type
			,CONCAT(ani_acode, ani_phone) AS [Phone Number]
			,Call_time
		FROM OPENQUERY
			(UHEAARAS,'
				--INBOUND
				SELECT * FROM  inboundloghsXXXXXX UNION ALL
				SELECT * FROM  inboundloghsXXXXXX UNION ALL
				SELECT * FROM  inboundloghsXXXXXX UNION ALL
				SELECT * FROM  inboundloghsXXXXXX UNION ALL
				SELECT * FROM  inboundloghsXXXXXX 
			')
	) UpdateTable
)TX
	ON TX.NobleRowId = TX.NobleRowId
	AND CAST(tX.ActivityDate AS DATE) = tX.[Call Date]
WHERE CAST(tX.ActivityDate AS DATE)  BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
AND TX.ActivityDate != TX.ActivityDate


SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
