USE UDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @SchoolCodes TABLE
(
	SchoolCode CHAR(8),
	SchoolCloseDate DATE
);

INSERT INTO
	@SchoolCodes
VALUES
--OPE ID	Closure Date
('00139714','8/31/2012'),
('00139720','8/31/2012'),
('00139722','8/31/2012'),
('00212301','8/31/2012'),
('00221115','3/13/2020'),
('00155609','7/31/2021'),
('00155603','12/31/2021'),
('00169241','6/30/2017'),
('02528300','10/1/2021'),
('00384212','9/30/2021'),
('00365202','8/15/2021'),
('02572800','10/8/2021'),
('02572000','10/8/2021'),
('00460815','5/31/2020'),
('00460843','5/31/2020'),
('00460857','5/31/2020'),
('00460858','5/31/2020'),
('00460859','5/31/2020'),
('00460860','5/31/2020'),
('00460863','5/31/2020'),
('00460864','5/31/2020'),
('00299217','6/4/2021'),
('00348718','8/1/2019'),
('03915300','10/29/2021'),
('03915301','10/29/2021'),
('03915303','10/29/2021'),
('00380205','8/20/2021'),
('00380216','8/20/2021'),
('04218802','7/31/2020')



--select * from @SchoolCodes

/*****************  MODIFIED QUERY  ***********************/
SELECT
	*
FROM
	(
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			PD10.DM_PRS_1 AS FirstName,
			PD10.DM_PRS_MID AS MiddleName,
			PD10.DM_PRS_LST AS LastName,
			PD30.DX_STR_ADR_1 AS Address1,
			PD30.DX_STR_ADR_2 AS Address2,
			PD30.DX_STR_ADR_3 AS Address3,
			PD30.DM_CT AS City,
			PD30.DC_DOM_ST AS [State],
			PD30.DF_ZIP_CDE AS Zip,
			PD30.DI_VLD_ADR AS AddressValid,
			LN10.LF_DOE_SCL_ORG AS OriginalSchool,
			SD10.LF_DOE_SCL_ENR_CUR AS CurrentSchool,
			SD10.LD_NTF_SCL_SPR AS SeparationDate,
			SD10.LC_REA_SCL_SPR AS SeparationReason,
			SD10.LD_SCL_SPR AS [LastDayOfEnrollment],
			School.SchoolCloseDate
		FROM
			PD10_PRS_NME PD10
			INNER JOIN PD30_PRS_ADR PD30 
				ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
			INNER JOIN 
			(
				SELECT
					LN10.BF_SSN,
					SC.SchoolCode,
					SC.SchoolCloseDate
				FROM
					@SchoolCodes SC
					INNER JOIN LN10_LON LN10 
						ON LN10.LF_DOE_SCL_ORG = SC.SchoolCode

				UNION

				SELECT DISTINCT
					SD10.LF_STU_SSN [BF_SSN],
					SC.SchoolCode,
					SC.SchoolCloseDate
				FROM
					@SchoolCodes SC
					INNER JOIN SD10_STU_SPR SD10 
						ON SD10.LF_DOE_SCL_ENR_CUR = SC.SchoolCode
				WHERE
					SD10.LC_STA_STU10 = 'A'
			) School 
				ON School.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN LN10_LON LN10 
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN SD10_STU_SPR SD10 
				ON SD10.LF_STU_SSN = PD10.DF_PRS_ID 
				AND SD10.LF_DOE_SCL_ENR_CUR = School.SchoolCode 
				AND SD10.LC_STA_STU10 = 'A'
		WHERE
			DATEADD(DAY, 121, SD10.LD_SCL_SPR) > School.SchoolCloseDate
			AND LN10.LA_CUR_PRI > 0.00
			AND LN10.LA_LON_AMT_GTR > 0.00
	) X
WHERE
	DATEADD(DAY, 121, ISNULL(X.LastDayOfEnrollment, GETDATE())) > X.SchoolCloseDate
	AND	(
			X.CurrentSchool IN (SELECT SchoolCode FROM @SchoolCodes)
			OR	X.OriginalSchool IN (SELECT SchoolCode FROM @SchoolCodes)
		)
ORDER BY
	X.AccountNUmber

 