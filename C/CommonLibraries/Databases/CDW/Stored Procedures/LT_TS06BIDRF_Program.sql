







CREATE PROCEDURE [dbo].[LT_TS06BIDRF_Program]
	@AccountNumber CHAR(10) 

AS


DECLARE @LETTERID VARCHAR(10) = 'TS06BIDRFG'
DECLARE @ARC_QUERY VARCHAR(MAX)
SELECT @ARC_QUERY = 'SELECT ('''' +  p.TotAmtForgiven +  '''') as TotAmtForgiven, p.IDRScheduleType FROM OPENQUERY(LEGEND_TEST_VUK3,''
			SELECT DISTINCT
				VARCHAR_FORMAT(SUM(DISTINCT (LN10.LA_CUR_PRI +  coalesce(DW01.WA_TOT_BRI_OTS,0))), ''''$999999.99'''') AS TotAmtForgiven,
				max(COALESCE(CASE 
						WHEN LN65.LC_TYP_SCH_DIS IN (''''I5'''') THEN ''''Revised Pay As You Earn Plan''''
                        WHEN LN65.LC_TYP_SCH_DIS IN (''''CA'''',''''CP'''') THEN ''''Pay As You Earn Plan''''
                        WHEN LN65.LC_TYP_SCH_DIS IN (''''CQ'''',''''C1'''',''''C2'''',''''C3'''') THEN ''''Income Contingent Plan''''
                        WHEN LN65.LC_TYP_SCH_DIS IN (''''IB'''',''''IL'''',''''IP'''',''''I3'''') THEN ''''Income Based Plan''''
                        ELSE ''''See attached loan sheet.''''
                END,''''See attached loan sheet'''')) AS IDRScheduleType
			FROM
				PKUB.LN10_LON LN10
				INNER JOIN
				(
					SELECT DISTINCT
						LN85.BF_SSN,
						LN85.LN_SEQ
					FROM
						PKUB.LN85_LON_ATY LN85
						INNER JOIN
						(
							SELECT
								AY10.BF_SSN,
								MAX(AY10.LN_ATY_SEQ) AS LN_ATY_SEQ
							FROM
								PKUB.AY10_BR_LON_ATY AY10
								INNER JOIN
								(
									SELECT 
										PF_REQ_ACT
									FROM 
										PKUB.AC11_ACT_REQ_LTR
									WHERE 
										 PF_LTR = ''''' + @LETTERID + '''''
								) ARC
									ON ARC.PF_REQ_ACT = AY10.PF_REQ_ACT
							GROUP BY
								AY10.BF_SSN
						)AY10
							ON AY10.BF_SSN = LN85.BF_SSN
							AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
				)LN85
					ON LN85.BF_SSN = LN10.BF_SSN
					AND LN85.LN_SEQ = LN10.LN_SEQ
				INNER JOIN PKUB.PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = LN10.BF_SSN
				INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
					ON DW01.BF_SSN = LN10.BF_SSN
					AND DW01.LN_SEQ = LN10.LN_SEQ
				INNER JOIN PKUB.LN65_LON_RPS LN65
					ON LN65.BF_SSN = LN10.BF_SSN
					AND LN65.LN_SEQ = LN10.LN_SEQ
			WHERE
				PD10.DF_SPE_ACC_ID = ''''' + @AccountNumber + '''''
				AND LN65.LC_STA_LON65 = ''''A''''
			GROUP BY
				COALESCE(CASE 
						WHEN LN65.LC_TYP_SCH_DIS IN (''''I5'''') THEN ''''Revised Pay As You Earn Plan''''
                        WHEN LN65.LC_TYP_SCH_DIS IN (''''CA'''',''''CP'''') THEN ''''Pay As You Earn Plan''''
                        WHEN LN65.LC_TYP_SCH_DIS IN (''''CQ'''',''''C1'''',''''C2'''',''''C3'''') THEN ''''Income Contingent Plan''''
                        WHEN LN65.LC_TYP_SCH_DIS IN (''''IB'''',''''IL'''',''''IP'''',''''I3'''') THEN ''''Income Based Plan''''
                        ELSE ''''See attached loan sheet.''''
                END,''''See attached loan sheet'''') 
	   '') p'
EXEC (@ARC_QUERY)
