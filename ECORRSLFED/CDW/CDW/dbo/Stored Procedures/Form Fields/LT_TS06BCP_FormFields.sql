﻿CREATE PROCEDURE [dbo].[LT_TS06BCP_FormFields]
	@AccountNumber CHAR(10),
	@IsCoborrower BIT = 0
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE	@LetterId VARCHAR(10) = 'TS06BCP'
DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = @LetterId)

IF @IsCoborrower = 0
	BEGIN
		SELECT DISTINCT
			CONVERT(CHAR(10),DATEADD(DAY, -35, RS05.BD_ANV_QLF_IBR),101) AS SoftDeadline,
			CONVERT(CHAR(10),DATEADD(DAY, -25, RS05.BD_ANV_QLF_IBR),101) AS HardDeadline,
			CASE WHEN DAY(RS05.BD_ANV_QLF_IBR) <= LN66.DueDay
					THEN CONVERT(CHAR(10),DATEADD(DAY, LN66.DueDay - DAY(RS05.BD_ANV_QLF_IBR), RS05.BD_ANV_QLF_IBR),101)--set the day to DUE_DAY
				 WHEN DAY(RS05.BD_ANV_QLF_IBR) > LN66.DueDay
					THEN CONVERT(CHAR(10),DATEADD(MONTH, 1, DATEADD(DAY, LN66.DueDay - DAY(RS05.BD_ANV_QLF_IBR), RS05.BD_ANV_QLF_IBR)),101)--add a month and set the day to DUE_DAY
			END AS AltPayDate,
			'$' + CAST(LN66.LA_RPS_ISL AS VARCHAR) AS EstAltPayAmt
		FROM 
			PD10_PRS_NME PD10
			INNER JOIN RS05_IBR_RPS RS05
				ON PD10.DF_PRS_ID = RS05.BF_SSN
			INNER JOIN 
			(
				SELECT
					LN65.BF_SSN,
					MAX(DAY(LN65.LD_RPD_MAX_TRM_SR)) AS DueDay,
					SUM(COALESCE(LN66.LA_RPS_ISL,0)) AS LA_RPS_ISL
				FROM
					LN65_LON_RPS LN65
					INNER JOIN LN66_LON_RPS_SPF LN66
						ON LN65.BF_SSN = LN66.BF_SSN
						AND LN65.LN_SEQ = LN66.LN_SEQ
						AND LN65.LN_RPS_SEQ = LN66.LN_RPS_SEQ
					INNER JOIN
					(
						SELECT
							LN85.BF_SSN,
							LN85.LN_SEQ
						FROM
							LN85_LON_ATY LN85
							INNER JOIN --GETS THE MOST RECENT ARC LEFT ON THE BORROWERS ACCOUNT TO GET THE LOAN SEQ'S THE LETTER APPLIES TO
							(
								SELECT
									AY10.BF_SSN,
									MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ,
									MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
								FROM 
									AY10_BR_LON_ATY AY10
									INNER JOIN PD10_PRS_NME PD10
										ON PD10.DF_PRS_ID = AY10.BF_SSN
								WHERE
									AY10.PF_REQ_ACT = @PF_REQ_ACT
									AND PD10.DF_SPE_ACC_ID = @AccountNumber
								GROUP BY
									AY10.BF_SSN
							)AY10
								ON AY10.BF_SSN = LN85.BF_SSN
								AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
					)LN85
						ON LN85.BF_SSN = LN65.BF_SSN
						AND LN85.LN_SEQ = LN65.LN_SEQ
				WHERE
					LN65.LC_STA_LON65 = 'A'
					AND
					(
						(
							LN66.LN_GRD_RPS_SEQ = 2
							AND LN65.LC_TYP_SCH_DIS = 'CA'
						) --Active CA with a CP pending if no requal
						OR
						(
							LN66.LN_GRD_RPS_SEQ = 1
							AND LN65.LC_TYP_SCH_DIS = 'CP'
						) --Active CP as the borrower already failed to requal
					)
				GROUP BY
					LN65.BF_SSN
			) LN66 
				ON PD10.DF_PRS_ID = LN66.BF_SSN
		WHERE
			RS05.BC_STA_RS05 = 'A'
			AND PD10.DF_SPE_ACC_ID = @AccountNumber
	END
ELSE
	BEGIN
		SELECT DISTINCT
			CONVERT(CHAR(10),DATEADD(DAY, -35, RS05.BD_ANV_QLF_IBR),101) AS SoftDeadline,
			CONVERT(CHAR(10),DATEADD(DAY, -25, RS05.BD_ANV_QLF_IBR),101) AS HardDeadline,
			CASE WHEN DAY(RS05.BD_ANV_QLF_IBR) <= LN66.DueDay
					THEN CONVERT(CHAR(10),DATEADD(DAY, LN66.DueDay - DAY(RS05.BD_ANV_QLF_IBR), RS05.BD_ANV_QLF_IBR),101)--set the day to DUE_DAY
				 WHEN DAY(RS05.BD_ANV_QLF_IBR) > LN66.DueDay
					THEN CONVERT(CHAR(10),DATEADD(MONTH, 1, DATEADD(DAY, LN66.DueDay - DAY(RS05.BD_ANV_QLF_IBR), RS05.BD_ANV_QLF_IBR)),101)--add a month and set the day to DUE_DAY
			END AS AltPayDate,
			'$' + CAST(LN66.LA_RPS_ISL AS VARCHAR) AS EstAltPayAmt
		FROM 
			PD10_PRS_NME PD10
			INNER JOIN LN20_EDS LN20
				ON LN20.LF_EDS = PD10.DF_PRS_ID
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.LC_EDS_TYP = 'M' --Coborrower
			INNER JOIN RS05_IBR_RPS RS05
				ON LN20.BF_SSN = RS05.BF_SSN
			INNER JOIN 
			(
				SELECT
					LN65.BF_SSN,
					MAX(DAY(LN65.LD_RPD_MAX_TRM_SR)) AS DueDay,
					SUM(COALESCE(LN66.LA_RPS_ISL,0)) AS LA_RPS_ISL
				FROM
					LN65_LON_RPS LN65
					INNER JOIN LN66_LON_RPS_SPF LN66
						ON LN65.BF_SSN = LN66.BF_SSN
						AND LN65.LN_SEQ = LN66.LN_SEQ
						AND LN65.LN_RPS_SEQ = LN66.LN_RPS_SEQ
					INNER JOIN LN20_EDS LN20
						ON LN20.BF_SSN = LN65.BF_SSN
						AND LN20.LN_SEQ = LN65.LN_SEQ
						AND LN20.LC_EDS_TYP = 'M'
						AND LN20.LC_STA_LON20 = 'A'
					INNER JOIN PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN20.LF_EDS
					INNER JOIN
					(
						SELECT
							LN85.BF_SSN,
							LN85.LN_SEQ
						FROM
							LN85_LON_ATY LN85
							INNER JOIN --GETS THE MOST RECENT ARC LEFT ON THE BORROWERS ACCOUNT TO GET THE LOAN SEQ'S THE LETTER APPLIES TO
							(
								SELECT
									AY10.BF_SSN,
									MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ,
									MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
								FROM 
									AY10_BR_LON_ATY AY10
									INNER JOIN LN20_EDS LN20
										ON LN20.BF_SSN = AY10.BF_SSN
										AND LN20.LC_EDS_TYP = 'M'
										AND LN20.LC_STA_LON20 = 'A'
									INNER JOIN PD10_PRS_NME PD10
										ON PD10.DF_PRS_ID = LN20.LF_EDS
								WHERE
									AY10.PF_REQ_ACT = @PF_REQ_ACT
									AND PD10.DF_SPE_ACC_ID = @AccountNumber
								GROUP BY
									AY10.BF_SSN
							)AY10
								ON AY10.BF_SSN = LN85.BF_SSN
								AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
					)LN85
						ON LN85.BF_SSN = LN65.BF_SSN
						AND LN85.LN_SEQ = LN65.LN_SEQ
				WHERE
					LN65.LC_STA_LON65 = 'A'
					AND
					(
						(
							LN66.LN_GRD_RPS_SEQ = 2
							AND LN65.LC_TYP_SCH_DIS = 'CA'
						) --Active CA with a CP pending if no requal
						OR
						(
							LN66.LN_GRD_RPS_SEQ = 1
							AND LN65.LC_TYP_SCH_DIS = 'CP'
						) --Active CP as the borrower already failed to requal
					)
					AND PD10.DF_SPE_ACC_ID = @AccountNumber
				GROUP BY
					LN65.BF_SSN
			) LN66 
				ON LN20.BF_SSN = LN66.BF_SSN
		WHERE
			RS05.BC_STA_RS05 = 'A'
			AND PD10.DF_SPE_ACC_ID = @AccountNumber
	END
END