CREATE PROCEDURE [soaletteru].[InactivateDuplicateRequests]

AS
	
	BEGIN
		
		DECLARE @DuplicateReason INT = (SELECT SystemLetterExclusionReasonId FROM ULS..SystemLetterExclusionReasons WHERE SystemLetterExclusionReason = 'Duplicate Removal')

		UPDATE
			LT20
		SET
			LT20.InactivatedAt = GETDATE(),
			LT20.SystemLetterExclusionReasonId = @DuplicateReason
		FROM
			(
				SELECT DISTINCT
					DF_SPE_ACC_ID,
					RN_SEQ_LTR_CRT_PRC
				FROM
					(
						SELECT
							DF_SPE_ACC_ID,
							RN_SEQ_LTR_CRT_PRC,
							DENSE_RANK() OVER (PARTITION BY DF_SPE_ACC_ID ORDER BY RN_SEQ_LTR_CRT_PRC) RNK --Rank requests. 1 = Original request
						FROM
							UDW..LT20_LTR_REQ_PRC
						WHERE
							InactivatedAt IS NULL
							AND RM_DSC_LTR_PRC = 'US06BTSA'
							AND 
								(
									(
										PrintedAt IS NULL 
										AND OnEcorr = 0
									) 
									OR EcorrDocumentCreatedAt IS NULL
								) --Ltr requests that haven't printed or ecorred
					) UnprocessedPop 
				WHERE
					UnprocessedPop.RNK != 1 --Select duplicates that are not part of the original ltr request 
			) DupePop
			INNER JOIN UDW..LT20_LTR_REQ_PRC LT20
				ON LT20.DF_SPE_ACC_ID = DupePop.DF_SPE_ACC_ID
				AND LT20.RN_SEQ_LTR_CRT_PRC = DupePop.RN_SEQ_LTR_CRT_PRC
				AND RM_DSC_LTR_PRC = 'US06BTSA'
	END

	BEGIN
		
		UPDATE
			LT20
		SET
			LT20.InactivatedAt = GETDATE(),
			LT20.SystemLetterExclusionReasonId = @DuplicateReason
		FROM
			(
				SELECT DISTINCT
					DF_SPE_ACC_ID,
					RN_SEQ_LTR_CRT_PRC
				FROM
					(
						SELECT
							DF_SPE_ACC_ID,
							RN_SEQ_LTR_CRT_PRC,
							DENSE_RANK() OVER (PARTITION BY DF_SPE_ACC_ID ORDER BY RN_SEQ_LTR_CRT_PRC) RNK --Rank requests. 1 = Original request
						FROM
							UDW..LT20_LTR_REQ_PRC_Coborrower
						WHERE
							InactivatedAt IS NULL
							AND RM_DSC_LTR_PRC = 'US06BTSA'
							AND 
								(
									(
										PrintedAt IS NULL 
										AND OnEcorr = 0
									) 
									OR EcorrDocumentCreatedAt IS NULL
								) --Ltr requests that haven't printed or ecorred
					) UnprocessedPop 
				WHERE
					UnprocessedPop.RNK != 1 --Select duplicates that are not part of the original ltr request 
			) DupePop
			INNER JOIN UDW..LT20_LTR_REQ_PRC_Coborrower LT20
				ON LT20.DF_SPE_ACC_ID = DupePop.DF_SPE_ACC_ID
				AND LT20.RN_SEQ_LTR_CRT_PRC = DupePop.RN_SEQ_LTR_CRT_PRC
				AND RM_DSC_LTR_PRC = 'US06BTSA'
	END
	


RETURN 0
