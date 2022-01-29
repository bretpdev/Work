USE CDW
GO
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	Records.AccountNumber,
	Records.FirstName,
	Records.LastName,
	Records.DaysUntilGraceEnd,
	Records.Age,
	MAX(Records.PhoneNumberX) AS PhoneX,
	MAX(Records.PhoneNumberX) AS PhoneX
FROM
(
	SELECT DISTINCT
		PDXX.DF_SPE_ACC_ID AS AccountNumber,
		LTRIM(RTRIM(SUBSTRING(PDXX.DM_PRS_X,X,X) + LOWER(SUBSTRING(PDXX.DM_PRS_X,X,XX)))) AS FirstName,
		LTRIM(RTRIM(SUBSTRING(PDXX.DM_PRS_LST,X,X) + LOWER(SUBSTRING(PDXX.DM_PRS_LST,X,XX)))) AS LastName,
		CASE WHEN PhoneRank.PHN_RNK = X THEN PhoneRank.ARA + PhoneRank.PHN ELSE NULL END AS PhoneNumberX,
		CASE WHEN PhoneRank.PHN_RNK = X THEN PhoneRank.ARA + PhoneRank.PHN ELSE NULL END AS PhoneNumberX,
		DATEDIFF(DAY,GETDATE(),DWXX.WD_LON_RPD_SR) AS DaysUntilGraceEnd,
		DATEDIFF(YEAR,PDXX.DD_BRT,GETDATE()) AS Age
	FROM
		CDW..PDXX_PRS_NME PDXX
		INNER JOIN CDW..LNXX_LON LNXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			AND LNXX.LC_STA_LONXX = 'R'
			AND LNXX.LA_CUR_PRI > X.XX
			AND CAST(LNXX.LD_LON_X_DSB AS DATE) > 'XXXX-XX-XX'
		INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
			ON DWXX.BF_SSN = PDXX.DF_PRS_ID
			AND DWXX.WC_DW_LON_STA = 'XX'
		INNER JOIN
		(
			SELECT DISTINCT
				DF_PRS_ID,
				ARA,
				PHN,
				DC_PHN,
				DENSE_RANK() OVER(PARTITION BY ARA, PHN, DC_PHN ORDER BY DF_PRS_ID) AS PHN_RNK
			FROM
			(
				SELECT DISTINCT
					PDXX.DF_PRS_ID,
					COALESCE(PDXX.DN_DOM_PHN_ARA,'') AS ARA,
					COALESCE(PDXX.DN_DOM_PHN_XCH,'') + COALESCE(PDXX.DN_DOM_PHN_LCL,'') AS PHN,
					PDXX.DC_PHN,
					DENSE_RANK() OVER (PARTITION BY PDXX.DF_PRS_ID ORDER BY Ranks.TypeRank) AS OverallRank /*Rank phones by type*/
				FROM
					CDW..PDXX_PRS_PHN PDXX
					INNER JOIN
					(
						SELECT
							*,
							CASE WHEN DC_PHN = 'H' THEN X
								 WHEN DC_PHN = 'A' THEN X
								 WHEN DC_PHN = 'W' THEN X
							END AS TypeRank
						FROM
							CDW..PDXX_PRS_PHN PDXX
						WHERE
							PDXX.DI_PHN_VLD = 'Y'
							AND PDXX.DC_NO_HME_PHN != 'J'
							AND PDXX.DC_ALW_ADL_PHN = 'P'
							AND PDXX.DC_PHN IN ('H','A','W')
							AND COALESCE(PDXX.DN_DOM_PHN_ARA,'') + COALESCE(PDXX.DN_DOM_PHN_XCH,'') + COALESCE(PDXX.DN_DOM_PHN_LCL,'') != ''
					) Ranks
						ON Ranks.DF_PRS_ID = PDXX.DF_PRS_ID
						AND Ranks.DC_PHN = PDXX.DC_PHN
				WHERE
					PDXX.DI_PHN_VLD = 'Y'
					AND PDXX.DC_NO_HME_PHN != 'J'
					AND PDXX.DC_ALW_ADL_PHN = 'P'
					AND PDXX.DC_PHN IN ('H','A','W')
					AND COALESCE(PDXX.DN_DOM_PHN_ARA,'') + COALESCE(PDXX.DN_DOM_PHN_XCH,'') + COALESCE(PDXX.DN_DOM_PHN_LCL,'') != ''
			) Ranked
			WHERE
				Ranked.OverallRank IN(X,X)
		) PhoneRank
			ON PhoneRank.DF_PRS_ID = PDXX.DF_PRS_ID
			AND PhoneRank.PHN_RNK IN(X,X)
) Records
GROUP BY
	Records.AccountNumber,
	Records.FirstName,
	Records.LastName,
	Records.DaysUntilGraceEnd,
	Records.Age
ORDER BY
	Records.DaysUntilGraceEnd

