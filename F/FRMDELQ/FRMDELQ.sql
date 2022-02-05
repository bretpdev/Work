USE CentralData
GO

CREATE SCHEMA [frmdelq] AUTHORIZATION [dbo]
GO

GRANT EXECUTE ON SCHEMA ::[frmdelq] TO [db_executor]
GO 

CREATE PROCEDURE [frmdelq].FormsReceivedDays1Through359
	@Reason VARCHAR(100)
AS

DECLARE @Today DATE = GETDATE()

SELECT
	[Account Number],
	[Last Name],
	[First Name],
	[Queue],
	[Days Delinquent],
	[FINAL PFH DATE],
	[Reason]
FROM
	(
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID AS [Account Number],
			RTRIM(LTRIM(PD10.DM_PRS_LST)) AS [Last Name],
			RTRIM(LTRIM(PD10.DM_PRS_1)) AS [First Name],
			CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE) AS [Queue],
			MAX(ISNULL(LN16.LN_DLQ_MAX, 0)) OVER (PARTITION BY PD10.DF_SPE_ACC_ID, CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE)) AS [Days Delinquent],
			DATEADD(MONTH, 12, RS.LD_RPS_1_PAY_DU) AS [FINAL PFH DATE],
			CASE
				WHEN CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE) IN ('S401','1P01') THEN 'DEFERMENTS'
				WHEN CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE) IN ('VRFB','VBFB','SF01') THEN 'FORBEARANCE'
				WHEN CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE) IN ('AW01','ATPD') THEN 'AUTO PAY'
				WHEN CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE) IN ('2A01','2P01','I001') THEN 'IBR'
				WHEN CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE) IN ('8701','DUDT','2501','1501','PRDF','V801','WR01','2301') THEN 'OTHER'
			END AS [Reason]
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN 
			(
				SELECT
					LN16.BF_SSN,
					LN16.LN_SEQ,
					LN16.LD_DLQ_OCC,
					LN16.LN_DLQ_MAX + 1 AS LN_DLQ_MAX
				FROM
					UDW..LN16_LON_DLQ_HST LN16
					INNER JOIN
					(
						SELECT
							MinOcc.BF_SSN,
							MinOcc.LN_SEQ,
							MIN(MinOcc.LD_DLQ_OCC) AS MinOccurance,
							MaxDelq.MaxDelq
						FROM
							UDW..LN16_LON_DLQ_HST MinOcc
							INNER JOIN
							(
								SELECT
									BF_SSN,
									LN_SEQ,
									MAX(LD_DLQ_MAX) AS MaxDelq
								FROM
									UDW..LN16_LON_DLQ_HST
								WHERE
									LC_STA_LON16 = '1'
									AND LD_DLQ_MAX >= DATEADD(DAY, -5, @Today)
								GROUP BY
									BF_SSN,
									LN_SEQ			
							) MaxDelq
								ON MinOcc.BF_SSN = MaxDelq.BF_SSN
								AND MinOcc.LN_SEQ = MaxDelq.LN_SEQ
								AND MinOcc.LD_DLQ_MAX = MaxDelq.MaxDelq
						GROUP BY
							MinOcc.BF_SSN,
							MinOcc.LN_SEQ,
							MaxDelq.MaxDelq
					) EarliestOcc
						ON EarliestOcc.BF_SSN = LN16.BF_SSN
						AND EarliestOcc.LN_SEQ = LN16.LN_SEQ
						AND EarliestOcc.MaxDelq = LN16.LD_DLQ_MAX
						AND EarliestOcc.MinOccurance = LN16.LD_DLQ_OCC
			) LN16
				ON LN10.BF_SSN = LN16.BF_SSN
				AND LN10.LN_SEQ = LN16.LN_SEQ
				AND LN16.LN_DLQ_MAX BETWEEN 2 AND 360
			INNER JOIN UDW..WQ20_TSK_QUE WQ20
				ON LN10.BF_SSN = WQ20.BF_SSN
				AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')	
			LEFT JOIN UDW.calc.RepaymentSchedules RS
				ON RS.BF_SSN = LN10.BF_SSN
				AND RS.CurrentGradation = 1
				AND RS.LC_TYP_SCH_DIS = 'IB'
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
			--Queue action requested no later than 360 days delinquent
			AND WQ20.WD_ACT_REQ <= DATEADD(DAY,360,LN16.LD_DLQ_OCC)
			--Queue action requested after delinquency occured
			AND WQ20.WD_ACT_REQ > LN16.LD_DLQ_OCC
			AND CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE) IN ('S401','1P01','VRFB','VBFB','SF01','AW01','ATPD','2A01','8701','DUDT','2501','1501','PRDF','V801','WR01','2301', '2P01', 'I001')

		UNION 

		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID AS [Account Number], 
			LTRIM(RTRIM(PD10.DM_PRS_LST)) AS [Last Name],
			LTRIM(RTRIM(PD10.DM_PRS_1)) AS [First Name],
			CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE) AS [Queue],
			MAX(ISNULL(LN16.LN_DLQ_MAX, 0)) OVER (PARTITION BY PD10.DF_SPE_ACC_ID, CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE)) AS [Days Delinquent],
			DATEADD(MONTH, 12, RS.LD_RPS_1_PAY_DU) AS [FINAL PFH DATE],
			'IBR FINAL PFH' AS [Reason]
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..LN10_LON LN10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN UDW..WQ20_TSK_QUE WQ20
				ON LN10.BF_SSN = WQ20.BF_SSN
				AND WQ20.WC_STA_WQUE20 NOT IN ('C','X')
			INNER JOIN UDW.calc.RepaymentSchedules RS
				ON RS.BF_SSN = LN10.BF_SSN
				AND RS.CurrentGradation = 1
				AND RS.LC_TYP_SCH_DIS = 'IB'
			LEFT JOIN 
			(
				SELECT
					LN16.BF_SSN,
					LN16.LN_SEQ,
					LN16.LD_DLQ_OCC,
					LN16.LN_DLQ_MAX + 1 AS LN_DLQ_MAX
				FROM
					UDW..LN16_LON_DLQ_HST LN16
					INNER JOIN
					(
						SELECT
							MinOcc.BF_SSN,
							MinOcc.LN_SEQ,
							MIN(MinOcc.LD_DLQ_OCC) AS MinOccurance,
							MaxDelq.MaxDelq
						FROM
							UDW..LN16_LON_DLQ_HST MinOcc
							INNER JOIN
							(
								SELECT
									BF_SSN,
									LN_SEQ,
									MAX(LD_DLQ_MAX) AS MaxDelq
								FROM
									UDW..LN16_LON_DLQ_HST
								WHERE
									LC_STA_LON16 = '1'
									AND LD_DLQ_MAX >= DATEADD(DAY, -5, @Today)
								GROUP BY
									BF_SSN,
									LN_SEQ			
							) MaxDelq
								ON MinOcc.BF_SSN = MaxDelq.BF_SSN
								AND MinOcc.LN_SEQ = MaxDelq.LN_SEQ
								AND MinOcc.LD_DLQ_MAX = MaxDelq.MaxDelq
						GROUP BY
							MinOcc.BF_SSN,
							MinOcc.LN_SEQ,
							MaxDelq.MaxDelq
					) EarliestOcc
						ON EarliestOcc.BF_SSN = LN16.BF_SSN
						AND EarliestOcc.LN_SEQ = LN16.LN_SEQ
						AND EarliestOcc.MaxDelq = LN16.LD_DLQ_MAX
						AND EarliestOcc.MinOccurance = LN16.LD_DLQ_OCC
			) LN16
				ON LN10.BF_SSN = LN16.BF_SSN
				AND LN10.LN_SEQ = LN16.LN_SEQ	
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
			AND CONCAT(WQ20.WF_QUE,WQ20.WF_SUB_QUE) IN ('2A01', '2P01', 'I001')
			AND DATEDIFF(DAY, GETDATE(),  DATEADD(MONTH, 12, RS.LD_RPS_1_PAY_DU)) <= 31 --FINAL_PFH_DATE
			AND DATEADD(MONTH, 12, RS.LD_RPS_1_PAY_DU) IS NOT NULL
	) POP
	WHERE
		[Reason] = @Reason
ORDER BY
	ISNULL([Days Delinquent], 0)