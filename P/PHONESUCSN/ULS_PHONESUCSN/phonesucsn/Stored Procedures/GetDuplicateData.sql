CREATE PROCEDURE [phonesucsn].[GetDuplicateData]
	@Count INT = 1000
AS

-- Delete any record that was added before today and not processed 
UPDATE
	ULS.phonesucsn.PhoneSuccession
SET
	DeletedAt = GETDATE(),
	DeletedBy = 'PreviousDayUnprocessed'
WHERE
	DeletedAt IS NULL
	AND	ProcessedAt IS NULL
	AND	CAST(AddedAt AS DATE) < CAST(GETDATE() AS DATE)

-- Get the number of records added today and subtract that from 1000. We only want to load 1000 a day to keep the first week of processing from getting too big
DECLARE @DailyCount INT = @Count - (SELECT COUNT(*) FROM ULS.phonesucsn.PhoneSuccession WHERE CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE) AND DeletedAt IS NULL)

IF (@DailyCount) > 0
	BEGIN
		INSERT INTO ULS.phonesucsn.PhoneSuccession(Ssn, HomePhone, HomeExt, HomeSrc, HomeInd, HomeConsent, HomeIsValid, HomeVerifiedDate, AltPhone, AltExt, AltSrc, AltInd, AltConsent, AltIsValid, AltVerifiedDate, WorkPhone, WorkExt, WorkSrc, WorkInd, WorkConsent, WorkIsValid, WorkVerifiedDate, MorningRun)
		SELECT TOP (@DailyCount)
			PhonesByBorrower.Ssn,
			PhonesByBorrower.HomePhone,
			PhonesByBorrower.HomeExt,
			PhonesByBorrower.HomeSrc,
			PhonesByBorrower.HomeInd,
			PhonesByBorrower.HomeConsent,
			PhonesByBorrower.HomeIsValid,
			PhonesByBorrower.HomeVerifiedDate,
			PhonesByBorrower.AltPhone,
			PhonesByBorrower.AltExt,
			PhonesByBorrower.AltSrc,
			PhonesByBorrower.AltInd,
			PhonesByBorrower.AltConsent,
			PhonesByBorrower.AltIsValid,
			PhonesByBorrower.AltVerifiedDate,
			PhonesByBorrower.WorkPhone,
			PhonesByBorrower.WorkExt,
			PhonesByBorrower.WorkSrc,
			PhonesByBorrower.WorkInd,
			PhonesByBorrower.WorkConsent,
			PhonesByBorrower.WorkIsValid,
			PhonesByBorrower.WorkVerifiedDate,
			PhonesByBorrower.MorningRun
		FROM
			(
				SELECT DISTINCT
					RTRIM(Phones.Ssn) AS Ssn,
					MAX(CASE WHEN Phones.PhoneType = 'H' THEN RTRIM(LTRIM(Phones.Phone)) ELSE NULL END) AS HomePhone,
					MAX(CASE WHEN Phones.PhoneType = 'H' THEN RTRIM(Phones.Extension) ELSE NULL END) AS HomeExt,
					MAX(CASE WHEN Phones.PhoneType = 'H' THEN RTRIM(Phones.[Source]) ELSE NULL END) AS HomeSrc,
					MAX(CASE WHEN Phones.PhoneType = 'H' THEN RTRIM(Phones.Indicator) ELSE NULL END) AS HomeInd,
					MAX(CASE WHEN Phones.PhoneType = 'H' THEN RTRIM(Phones.Consent) ELSE NULL END) AS HomeConsent,
					MAX(CASE WHEN Phones.PhoneType = 'H' THEN RTRIM(Phones.IsValid) ELSE NULL END) AS HomeIsValid,
					MAX(CASE WHEN Phones.PhoneType = 'H' THEN RTRIM(Phones.DD_PHN_VER) ELSE NULL END) AS HomeVerifiedDate,
					MAX(CASE WHEN Phones.PhoneType = 'A' THEN RTRIM(Phones.Phone) ELSE NULL END) AS AltPhone,
					MAX(CASE WHEN Phones.PhoneType = 'A' THEN RTRIM(Phones.Extension) ELSE NULL END) AS AltExt,
					MAX(CASE WHEN Phones.PhoneType = 'A' THEN RTRIM(Phones.[Source]) ELSE NULL END) AS AltSrc,
					MAX(CASE WHEN Phones.PhoneType = 'A' THEN RTRIM(Phones.Indicator) ELSE NULL END) AS AltInd,
					MAX(CASE WHEN Phones.PhoneType = 'A' THEN RTRIM(Phones.Consent) ELSE NULL END) AS AltConsent,
					MAX(CASE WHEN Phones.PhoneType = 'A' THEN RTRIM(PHONES.IsValid) ELSE NULL END) AS AltIsValid,
					MAX(CASE WHEN Phones.PhoneType = 'A' THEN RTRIM(Phones.DD_PHN_VER) ELSE NULL END) AS AltVerifiedDate,
					MAX(CASE WHEN Phones.PhoneType = 'W' THEN RTRIM(LTRIM(Phones.Phone)) ELSE NULL END) AS WorkPhone,
					MAX(CASE WHEN Phones.PhoneType = 'W' THEN RTRIM(Phones.Extension) ELSE NULL END) AS WorkExt,
					MAX(CASE WHEN Phones.PhoneType = 'W' THEN RTRIM(Phones.[Source]) ELSE NULL END) AS WorkSrc,
					MAX(CASE WHEN Phones.PhoneType = 'W' THEN RTRIM(Phones.Indicator) ELSE NULL END) AS WorkInd,
					MAX(CASE WHEN Phones.PhoneType = 'W' THEN RTRIM(Phones.Consent) ELSE NULL END) AS WorkConsent,
					MAX(CASE WHEN Phones.PhoneType = 'W' THEN RTRIM(PHONES.IsValid) ELSE NULL END) AS WorkIsValid,
					MAX(CASE WHEN Phones.PhoneType = 'W' THEN RTRIM(Phones.DD_PHN_VER) ELSE NULL END) AS WorkVerifiedDate,
					0 AS MorningRun
				FROM
					(
						SELECT
							PD42.DF_PRS_ID AS Ssn,
							PD42.DC_PHN AS PhoneType,
							CASE WHEN RTRIM(PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL) != ''
								THEN RTRIM(PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL)
								ELSE RTRIM(PD42.DN_FGN_PHN_CNY + PD42.DN_FGN_PHN_CT + PD42.DN_FGN_PHN_LCL)
							END AS Phone,
							CASE WHEN PD42.DI_PHN_VLD = 'Y' THEN 1 ELSE 0 END AS IsValid,
							PD42.DN_PHN_XTN AS Extension,
							PD42.DC_PHN_SRC AS [Source],
							CASE WHEN PD42.DC_ALW_ADL_PHN = 'L' THEN 'L'
									WHEN PD42.DC_ALW_ADL_PHN = 'N' THEN 'M'
									WHEN PD42.DC_ALW_ADL_PHN = 'P' THEN 'M'
									WHEN PD42.DC_ALW_ADL_PHN = 'Q' THEN 'L'
									WHEN PD42.DC_ALW_ADL_PHN = 'U' THEN 'U'
									WHEN PD42.DC_ALW_ADL_PHN = 'X' THEN 'U'
							END AS Indicator,
							CASE WHEN PD42.DC_ALW_ADL_PHN = 'L' THEN 'Y'
									WHEN PD42.DC_ALW_ADL_PHN = 'N' THEN 'N'
									WHEN PD42.DC_ALW_ADL_PHN = 'P' THEN 'Y'
									WHEN PD42.DC_ALW_ADL_PHN = 'Q' THEN 'N'
									WHEN PD42.DC_ALW_ADL_PHN = 'U' THEN 'N'
									WHEN PD42.DC_ALW_ADL_PHN = 'X' THEN 'Y'
							END AS Consent,
							PD42.DD_PHN_VER
						FROM
							UDW..PD42_PRS_PHN PD42
							INNER JOIN UDW..LN10_LON LN10
								ON LN10.BF_SSN = PD42.DF_PRS_ID
						WHERE
							LN10.LA_CUR_PRI > 0.00
							AND LN10.LC_STA_LON10 = 'R'
							AND PD42.DI_PHN_VLD = 'Y'
							AND
							(
								RTRIM(PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL) != ''
								OR RTRIM(PD42.DN_FGN_PHN_CNY + PD42.DN_FGN_PHN_CT + PD42.DN_FGN_PHN_LCL) != ''
							)
					) Phones
				GROUP BY
					Phones.Ssn
			) PhonesByBorrower
			LEFT JOIN ULS.phonesucsn.PhoneSuccession PS
				ON PS.Ssn = PhonesByBorrower.Ssn
				AND CAST(PS.AddedAt AS DATE) = CAST(GETDATE() AS DATE)
				AND PS.DeletedAt IS NULL
				AND PS.MorningRun = 0
		WHERE
			(
				(PhonesByBorrower.HomePhone = PhonesByBorrower.AltPhone AND PhonesByBorrower.HomeExt = PhonesByBorrower.AltExt AND PhonesByBorrower.HomeIsValid = 1 AND PhonesByBorrower.AltIsValid = 1)
				OR (PhonesByBorrower.HomePhone = PhonesByBorrower.WorkPhone AND PhonesByBorrower.HomeExt = PhonesByBorrower.WorkExt AND PhonesByBorrower.HomeIsValid = 1 AND	PhonesByBorrower.WorkIsValid = 1)
				OR (PhonesByBorrower.AltPhone = PhonesByBorrower.WorkPhone AND PhonesByBorrower.AltExt = PhonesByBorrower.WorkExt AND PhonesByBorrower.AltIsValid = 1 AND PhonesByBorrower.WorkIsValid = 1)
			)
			AND PS.Ssn IS NULL
		ORDER BY
			PhonesByBorrower.Ssn
	END

--Pull back the data for processing
SELECT
	PhoneSuccessionId,
	Ssn,
	HomePhone,
	HomeExt,
	HomeSrc,
	HomeInd,
	HomeConsent,
	HomeIsValid,
	HomeVerifiedDate,
	AltPhone,
	AltExt,
	AltSrc,
	AltInd,
	AltConsent,
	AltIsValid,
	AltVerifiedDate,
	WorkPhone,
	WorkExt,
	WorkSrc,
	WorkInd,
	WorkConsent,
	WorkIsValid,
	WorkVerifiedDate,
	IsEndorser
FROM
	phonesucsn.PhoneSuccession
WHERE
	InvalidatedAt IS NULL
	AND DeletedAt IS NULL
	AND HadError IS NULL
	AND MorningRun = 0