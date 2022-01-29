﻿CREATE PROCEDURE [dbo].[LT_TS06BICRE1_FormFields]
	@AccountNumber CHAR(10),
	@IsCoborrower BIT = 0
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF @IsCoborrower = 0
	BEGIN
		SELECT DISTINCT
			CONVERT(VARCHAR(10),DATEADD(DAY, -35, RS05.BD_ANV_QLF_IBR),101) AS SoftDeadline, -- Anniversary date for requalification
			CONVERT(VARCHAR(10),DATEADD(DAY, -25, RS05.BD_ANV_QLF_IBR),101) AS HardDeadline,
			'$' + CAST(RS05.BA_PMN_STD_TOT_PAY AS VARCHAR) AS PermStandardAmount, -- Permanant Standard Total Payment
			CASE WHEN DAY(RS05.BD_ANV_QLF_IBR) <= DAY(LN65.LD_RPD_MAX_TRM_SR)
				THEN CONVERT(VARCHAR(10), DATEADD(DAY, DAY(LN65.LD_RPD_MAX_TRM_SR) - DAY(RS05.BD_ANV_QLF_IBR), RS05.BD_ANV_QLF_IBR),101)--set the day to DUE_DAY
				ELSE CONVERT(VARCHAR(10),DATEADD(MONTH, 1, DATEADD(DAY, DAY(LN65.LD_RPD_MAX_TRM_SR) - DAY(RS05.BD_ANV_QLF_IBR), RS05.BD_ANV_QLF_IBR)),101)--add a month and set the day to DUE_DAY
			END AS PermStandardBeginDate
		FROM
			RS05_IBR_RPS RS05
			INNER JOIN PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = RS05.BF_SSN
			INNER JOIN LN65_LON_RPS LN65
				ON LN65.BF_SSN = RS05.BF_SSN
		WHERE
			RS05.BC_STA_RS05 = 'A'
			AND LN65.LC_TYP_SCH_DIS IN('IB','C1','C2','C3','CQ','I3','IP','I5','CA','CP','IA','RE')
			AND LN65.LC_STA_LON65 = 'A'
			AND PD10.DF_SPE_ACC_ID = @AccountNumber
	END
ELSE
	BEGIN
		SELECT DISTINCT
			CONVERT(VARCHAR(10),DATEADD(DAY, -35, RS05.BD_ANV_QLF_IBR),101) AS SoftDeadline, -- Anniversary date for requalification
			CONVERT(VARCHAR(10),DATEADD(DAY, -25, RS05.BD_ANV_QLF_IBR),101) AS HardDeadline,
			'$' + CAST(RS05.BA_PMN_STD_TOT_PAY AS VARCHAR) AS PermStandardAmount, -- Permanant Standard Total Payment
			CASE WHEN DAY(RS05.BD_ANV_QLF_IBR) <= DAY(LN65.LD_RPD_MAX_TRM_SR)
				THEN CONVERT(VARCHAR(10), DATEADD(DAY, DAY(LN65.LD_RPD_MAX_TRM_SR) - DAY(RS05.BD_ANV_QLF_IBR), RS05.BD_ANV_QLF_IBR),101)--set the day to DUE_DAY
				ELSE CONVERT(VARCHAR(10),DATEADD(MONTH, 1, DATEADD(DAY, DAY(LN65.LD_RPD_MAX_TRM_SR) - DAY(RS05.BD_ANV_QLF_IBR), RS05.BD_ANV_QLF_IBR)),101)--add a month and set the day to DUE_DAY
			END AS PermStandardBeginDate
		FROM
			PD10_PRS_NME PD10
			INNER JOIN LN20_EDS LN20
				ON LN20.LF_EDS = PD10.DF_PRS_ID
				AND LN20.LC_EDS_TYP = 'M'
				AND LN20.LC_STA_LON20 = 'A'
			INNER JOIN RS05_IBR_RPS RS05
				ON RS05.BF_SSN = LN20.BF_SSN
			INNER JOIN LN65_LON_RPS LN65
				ON LN65.BF_SSN = RS05.BF_SSN
		WHERE
			RS05.BC_STA_RS05 = 'A'
			AND LN65.LC_TYP_SCH_DIS IN('IB','C1','C2','C3','CQ','I3','IP','I5','CA','CP','IA','RE')
			AND LN65.LC_STA_LON65 = 'A'
			AND PD10.DF_SPE_ACC_ID = @AccountNumber
	END
END