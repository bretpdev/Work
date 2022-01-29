USE UDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @LETTER VARCHAR(7) = 'LR 7553'
	DECLARE @EmailCampaignId TINYINT = (SELECT EmailCampaignId FROM ULS.emailbatch.EmailCampaigns WHERE AddedBy = @LETTER)
	--select @LETTER,@EmailCampaignId --test

INSERT INTO [ULS].[emailbatch].[EmailProcessing]
(
	[EmailCampaignId]
	,[AccountNumber]
	,[EmailData]
	,[ArcNeeded]
	,[ProcessingAttempts]
	,[AddedBy]
	,[AddedAt]
)
SELECT DISTINCT
	@EmailCampaignId AS EmailCampaignId
	,PD10.DF_SPE_ACC_ID AS AccountNumber
	,PD10.DF_SPE_ACC_ID + ',' 
		+ LTRIM(RTRIM(PD10.DM_PRS_1)) + ',' 
		+ COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS EmailData
	,1 AS ArcNeeded
	,1 AS ProcessingAttempts
	,'UNH 59618' AS AddedBy
	,GETDATE() AS AddedAt

	--TEST FIELDS:
	--,LN10.LC_STA_LON10
	--,DW01.WC_DW_LON_STA
	--,CASE
	--	WHEN LN10.LA_CUR_PRI > 0.00
	--	THEN '>0'
	--	ELSE NULL
	--END AS LA_CUR_PRI
FROM
	LN10_LON LN10
	INNER JOIN DW01_DW_CLC_CLU DW01
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
	INNER JOIN PD10_PRS_NME PD10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN PH05_CNC_EML PH05
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
		AND	PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
		AND PH05.DI_CNC_ELT_OPI = 'Y' --on ecorr
	LEFT JOIN
	( -- email address 
		SELECT 
			DF_PRS_ID,
			EMail.EM [ALT_EM],
 			ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber 
 		FROM 
 		( 
 			SELECT 
 				PD32.DF_PRS_ID, 
				PD32.DI_VLD_ADR_EML,
 				PD32.DX_ADR_EML [EM], 
 				CASE	  
 					WHEN DC_ADR_EML = 'H' THEN 1 -- home 
 					WHEN DC_ADR_EML = 'A' THEN 2 -- alternate 
 					WHEN DC_ADR_EML = 'W' THEN 3 -- work 
 				END AS PriorityNumber
 			FROM 
 				PD32_PRS_ADR_EML PD32 
 			WHERE 
 				PD32.DI_VLD_ADR_EML = 'Y' -- valid email address 
 				AND PD32.DC_STA_PD32 = 'A' -- active email address record 
 		) EMail 
	) PD32 ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32.EmailPriority = 1
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00
	AND DW01.WC_DW_LON_STA NOT IN ('01','02','17','19','21','22')
	AND (
			PH05.DX_CNC_EML_ADR IS NOT NULL
			OR PD32.ALT_EM IS NOT NULL
		)
;

IF @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		--PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
;

--select * from [ULS].[emailbatch].[EmailProcessing] where AddedBy = 'UNH 59618';--test


/*STOP! DO NOT RUN ANYTHING BELOW THIS LINE!*/

/*JACOB'S LR 7553*/

--USE [ULS]
--GO

--BEGIN TRANSACTION
--	DECLARE @ERROR INT = 0
--	DECLARE @ROWCOUNT INT = 0

--DECLARE @HTMLfileId INT = (SELECT HTMLFileId FROM [emailbatch].HTMLFiles WHERE HTMLFile = 'ACHROEMLUH.html')
--IF @HTMLfileId IS NULL
--	BEGIN
--		INSERT INTO [emailbatch].[HTMLFiles]
--			   ([HTMLFile])
--		VALUES
--			   ('ACHROEMLUH.html')
--		SET @HTMLfileId = SCOPE_IDENTITY()
--	END
--DECLARE @FromAddressId INT = (SELECT FromAddressId FROM [emailbatch].FromAddresses WHERE FromAddress = 'noreply@utahsbr.edu')
--IF @FromAddressId IS NULL
--	BEGIN
--		INSERT INTO [emailbatch].[FromAddresses]
--			   ([FromAddress])
--		VALUES
--			   ('noreply@utahsbr.edu')
--		SET @FromAddressId = SCOPE_IDENTITY()
--	END
--DECLARE @SubjectLineId INT = (SELECT SubjectLineId FROM [emailbatch].SubjectLines WHERE SubjectLine = 'dummy subject please update when known')
--IF @SubjectLineId IS NULL
--	BEGIN
--		INSERT INTO [emailbatch].[SubjectLines]
--			   ([SubjectLine])
--		VALUES
--			   ('dummy subject please update when known')
--		SET @SubjectLineId = SCOPE_IDENTITY()
--	END

--INSERT INTO [emailbatch].[EmailCampaigns]
--           ([SourceFile]
--           ,[HTMLFileId]
--           ,[FromAddressId]
--           ,[SubjectLineId]
--           ,[ProcessAllFiles]
--           ,[OKIfEmpty]
--           ,[OKIfMissing]
--           ,[ArcId]
--           ,[CommentId]
--		   ,[ActivityTypeId]
--		   ,[ActivityContactId]
--		   ,[AddedBy]
--		   ,[AddedAt]
--		   ,[DeletedBy]
--		   ,[DeletedAt]
--		   )
--     VALUES
--           ('' --SourceFiles
--           ,@HTMLfileId
--           ,@FromAddressId
--           ,@SubjectLineId
--           ,1 --ProcessAllFiles
--           ,1 --OKIfEmpty
--           ,1 --OKIfMissing
--           ,NULL --ArcId
--           ,NULL --CommentId
--		   ,NULL --ActivityTypeId
--		   ,NULL --ActivityContactId
--		   ,SUSER_NAME()
--		   ,GETDATE()
--		   ,NULL
--		   ,NULL
--		   )
--	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

--IF @ROWCOUNT = 1 AND @ERROR = 0
--	BEGIN
--		PRINT 'Transaction committed'
--		COMMIT TRANSACTION
--		--ROLLBACK TRANSACTION
--	END
--ELSE
--	BEGIN
--		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
--		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
--		PRINT 'Transaction NOT committed'
--		ROLLBACK TRANSACTION
--	END