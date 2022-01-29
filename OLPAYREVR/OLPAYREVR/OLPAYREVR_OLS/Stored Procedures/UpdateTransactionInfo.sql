CREATE PROCEDURE [olpayrevr].[UpdateTransactionInfo]
	
AS
	UPDATE
		PQ
	SET
		PQ.PaymentAlreadyReversed = REVS.AlreadyHasReversal,
		PQ.PaymentPostDate = REVS.PaymentPostDate
	FROM
		olpayrevr.ReversalsProcessingQueue PQ
		LEFT JOIN 
		(
			SELECT
				TRANS_SUMMARY.BF_SSN AS Ssn,
				SUM(TRANS_SUMMARY.LA_TRX) AS PaymentAmount,
				TRANS_SUMMARY.LC_TRX_TYP AS PaymentType,
				TRANS_SUMMARY.LD_TRX_EFF AS PaymentEffectiveDate,
				TRANS_SUMMARY.BD_TRX_PST_HST AS PaymentPostDate,
				TRANS_SUMMARY.AlreadyHasReversal,
				TRANS_SUMMARY.BN_DLY_RCI_SEQ_HST
			FROM
			(
				SELECT
					DC11.AF_APL_ID,
					DC11.AF_APL_ID_SFX,
					DC11.BF_SSN,
					DC11.BD_TRX_PST_HST,
					DC11.BN_DLY_RCI_SEQ_HST,
					CASE
						WHEN DC11.LC_TRX_TYP = 'OV' AND TRANS_CHECK.BF_SSN IS NOT NULL THEN 'GP' --Count as GP since was on same day
						ELSE DC11.LC_TRX_TYP
					END AS LC_TRX_TYP,
					CASE
						WHEN DC11.LC_REV_IND_TYP IN ('PE', 'BC') THEN 1 ELSE 0
					END AS AlreadyHasReversal,
					DC11.LA_TRX,
					DC11.LD_TRX_EFF
				FROM
					ODW..DC11_LON_FAT DC11
					INNER JOIN 
					(
						SELECT
							MAX(LF_CRT_DTS_DC11) AS MaxUpdatedAt,
							AF_APL_ID,
							AF_APL_ID_SFX,
							BF_SSN,
							LD_TRX_EFF,
							LC_TRX_TYP,
							LA_TRX
						FROM
							ODW..DC11_LON_FAT MAX_DTS
						GROUP BY
							AF_APL_ID,
							AF_APL_ID_SFX,
							BF_SSN,
							LD_TRX_EFF,
							LC_TRX_TYP,
							LA_TRX
					) MAX_DTS
						ON MAX_DTS.AF_APL_ID = DC11.AF_APL_ID
						AND MAX_DTS.AF_APL_ID_SFX = DC11.AF_APL_ID_SFX
						AND MAX_DTS.BF_SSN = DC11.BF_SSN
						AND MAX_DTS.LD_TRX_EFF = DC11.LD_TRX_EFF
						AND MAX_DTS.LC_TRX_TYP = DC11.LC_TRX_TYP
						AND MAX_DTS.LA_TRX = DC11.LA_TRX
						AND MAX_DTS.MaxUpdatedAt = DC11.LF_CRT_DTS_DC11
					LEFT JOIN
					( 
						SELECT DISTINCT
							BF_SSN,
							LD_TRX_EFF
						FROM
							ODW..DC11_LON_FAT 
						WHERE
							LC_TRX_TYP IN ('GP', 'EP')
					) TRANS_CHECK --We only count OV payments as GP payments if there was a GP/EP payment on the same day
						ON TRANS_CHECK.BF_SSN = DC11.BF_SSN
						AND TRANS_CHECK.LD_TRX_EFF = DC11.LD_TRX_EFF
				) TRANS_SUMMARY
			GROUP BY
				TRANS_SUMMARY.BF_SSN,
				TRANS_SUMMARY.LC_TRX_TYP,
				TRANS_SUMMARY.LD_TRX_EFF,
				TRANS_SUMMARY.BD_TRX_PST_HST,
				TRANS_SUMMARY.AlreadyHasReversal,
				TRANS_SUMMARY.BN_DLY_RCI_SEQ_HST
		) REVS
			ON REVS.Ssn = PQ.Ssn
			AND REVS.PaymentAmount = PQ.PaymentAmount
			AND REVS.PaymentType = PQ.PaymentType
			AND REVS.PaymentEffectiveDate = PQ.PaymentEffectiveDate
	WHERE 
		PQ.ProcessedAt IS NULL
		AND PQ.DeletedAt IS NULL
		AND
		(
			PQ.PaymentAlreadyReversed IS NULL
			OR PQ.PaymentPostDate IS NULL
		)

	UPDATE
		olpayrevr.ReversalsProcessingQueue
	SET
		ProcessedAt = GETDATE()
	WHERE
		PaymentAlreadyReversed = 1
		AND ProcessedAt IS NULL
		AND DeletedAt IS NULL

RETURN 0
