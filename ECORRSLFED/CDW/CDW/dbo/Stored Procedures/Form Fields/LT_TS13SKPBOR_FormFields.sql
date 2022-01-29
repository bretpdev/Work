﻿
CREATE PROCEDURE [dbo].[LT_TS13SKPBOR_FormFields]
	@AccountNumber CHAR(10),
	@IsCoborrower BIT = 0
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE	@LetterId VARCHAR(10) = 'TS13SKPBOR'
DECLARE @PF_REQ_ACT VARCHAR(5) = (SELECT AC11.PF_REQ_ACT FROM AC11_ACT_REQ_LTR AC11 WHERE PF_LTR = @LetterId)

IF @IsCoborrower = 0
	BEGIN
		SELECT DISTINCT
			'' AS FullName,
			'' AS MaidenName,
			'' AS [Address],
			'' AS City,
			'' AS [State],
			'' AS ZIP,
			'' AS HomePhone,
			'' AS WorkPhone,
			'' AS AlternatePhone,
			'' AS EmailAddress,
			'' AS ReferenceName,
			'' AS ReferencePhone,
			'' AS ReferenceAddress,
			'' AS ReferenceCity,
			'' AS ReferenceState,
			'' AS ReferenceZip
		FROM 
			PD10_PRS_NME PD10
			INNER JOIN LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0
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
				ON LN85.BF_SSN = LN10.BF_SSN
				AND LN85.LN_SEQ = LN10.LN_SEQ
		WHERE
			PD10.DF_SPE_ACC_ID = @AccountNumber
	END
ELSE
	BEGIN
		SELECT DISTINCT
			'' AS FullName,
			'' AS MaidenName,
			'' AS [Address],
			'' AS City,
			'' AS [State],
			'' AS ZIP,
			'' AS HomePhone,
			'' AS WorkPhone,
			'' AS AlternatePhone,
			'' AS EmailAddress,
			'' AS ReferenceName,
			'' AS ReferencePhone,
			'' AS ReferenceAddress,
			'' AS ReferenceCity,
			'' AS ReferenceState,
			'' AS ReferenceZip
		FROM 
			PD10_PRS_NME PD10
			INNER JOIN LN20_EDS LN20
				ON LN20.LF_EDS = PD10.DF_PRS_ID
				AND LN20.LC_STA_LON20 = 'A'
				AND LN20.LC_EDS_TYP = 'M'
			INNER JOIN LN10_LON LN10
				ON LN10.BF_SSN = LN20.BF_SSN
				AND LN10.LN_SEQ = LN20.LN_SEQ
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LA_CUR_PRI > 0
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
								AND LN20.LC_STA_LON20 = 'A'
								AND LN20.LC_EDS_TYP = 'M'
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
				ON LN85.BF_SSN = LN10.BF_SSN
				AND LN85.LN_SEQ = LN10.LN_SEQ
		WHERE
			PD10.DF_SPE_ACC_ID = @AccountNumber
	END
END