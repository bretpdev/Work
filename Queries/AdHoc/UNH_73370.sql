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
('01318900','10/17/2021'),
('01072790','10/23/2021'),
('04154901','5/28/2021'),
('04134000','9/10/2021'),
('10147955','10/2/2020'),
('001318466','6/23/2019'),
('10147933','11/30/2020'),
('01072757','8/28/2021'),
('10383763','12/24/2017'),
('20383744','12/24/2017'),
('00384211','10/31/2021'),
('10147900','9/10/2020'),
('02572013','10/8/2021'),
('02278101','3/22/2020'),
('02572003','10/8/2021'),
('02306805','10/5/2021'),
('10147935','4/30/2021'),
('02572002','10/8/2021'),
('02572007','10/8/2021'),
('02572008','10/8/2021'),
('02572009','10/8/2021'),
('02572011','10/8/2021'),
('02572012','10/8/2021'),
('00193607','5/17/2019'),
('00147974','1/31/2021'),
('00535605','4/26/2019'),
('00537904','4/23/2021'),
('02572801','10/8/2021'),
('00147998','6/30/2020'),
('04208905','9/16/2021'),
('00301033','3/18/2020'),
('00267118','12/1/2019'),
('10147949','4/1/2021'),
('04088300','5/28/2019')




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

 