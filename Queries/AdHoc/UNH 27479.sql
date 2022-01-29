USE EA27
GO

--DROP TABLE _CSV_MATCHES
--DROP TABLE _EA27_DATA

---- matches on IDENT AND CLUID
--SELECT DISTINCT
--	DCER.wave, 
--	DCER.loan_number,
--	CSV.CSVID,
--	CSV.Borrower_SSN,
--	CSV.Commonline_Unique_ID,
--	DCER.CommonLineUniqueID,
--	CSV.LON_IDENT,
--	DCER.LoanIdentification
--INTO
--	_CSV_MATCHES 
--FROM 
--	EA27.dbo._CSVSOURCE_NH_27479 CSV
--	INNER JOIN
--	(
--		SELECT
--			1 [wave],
--			*
--		FROM
--			EA27_BANA_1.[dbo].[_07_08DisbClaimEnrollRecord] DCER

--		UNION ALL

--		SELECT
--			2 [wave],
--			*
--		FROM
--			EA27_BANA_2.[dbo].[_07_08DisbClaimEnrollRecord] DCER

--		UNION ALL

--		SELECT
--			3 [wave],
--			*
--		FROM
--			EA27_BANA_3.[dbo].[_07_08DisbClaimEnrollRecord] DCER
--	) DCER ON DCER.BorrowerSSN = CSV.Borrower_SSN AND ISNULL(LTRIM(RTRIM(DCER.CommonLineUniqueID)), '0') = ISNULL(LTRIM(RTRIM(CSV.Commonline_Unique_ID)), '0') AND LTRIM(RTRIM(DCER.LoanIdentification)) = LTRIM(RTRIM(CSV.LON_IDENT))


---- match on unique ident only
--INSERT INTO
--	_CSV_MATCHES
--(
--	wave,
--	loan_number,
--	CSVID,
--	Borrower_SSN,
--	Commonline_Unique_ID,
--	CommonLineUniqueID,
--	LON_IDENT,
--	LoanIdentification
--)
--SELECT
--	DCERA.wave, 
--	DCERA.loan_number,
--	UM.CSVID,
--	UM.Borrower_SSN,
--	UM.Commonline_Unique_ID,
--	DCERa.CommonLineUniqueID,
--	UM.LON_IDENT,
--	DCERA.LoanIdentification
--FROM
--	( -- unmatched
--		SELECT
--			CSV.*
--		FROM
--			EA27.._CSVSOURCE_NH_27479 CSV
--			LEFT JOIN _CSV_MATCHES CSVM ON CSVM.CSVID = CSV.CSVID
--		WHERE
--			CSVM.CSVID is NULL
--	) UM -- unmatched
--	INNER JOIN 
--	(
--		SELECT --unique LON_IDENT
--			CSV.LON_IDENT,
--			COUNT(CSV.LON_IDENT) IDENT_COUNT
--		FROM
--			EA27.._CSVSOURCE_NH_27479 CSV
--		GROUP BY
--			CSV.LON_IDENT
--		HAVING
--			COUNT(CSV.LON_IDENT) = 1
--	) UI ON UI.LON_IDENT = UM.LON_IDENT
--	LEFT JOIN
--	(
--		SELECT
--			1 [wave],
--			*
--		FROM
--			EA27_BANA_1.[dbo].[_07_08DisbClaimEnrollRecord] DCER

--		UNION ALL

--		SELECT
--			2 [wave],
--			*
--		FROM
--			EA27_BANA_2.[dbo].[_07_08DisbClaimEnrollRecord] DCER

--		UNION ALL

--		SELECT
--			3 [wave],
--			*
--		FROM
--			EA27_BANA_3.[dbo].[_07_08DisbClaimEnrollRecord] DCER
--	) DCERA ON DCERA.BorrowerSSN = UM.Borrower_SSN AND DCERA.LoanIdentification = UM.LON_IDENT
--WHERE
--	DCERA.BorrowerSSN IS NOT NULL


---- match on unique CLUID only
--INSERT INTO
--	_CSV_MATCHES
--(
--	wave,
--	loan_number,
--	CSVID,
--	Borrower_SSN,
--	Commonline_Unique_ID,
--	CommonLineUniqueID,
--	LON_IDENT,
--	LoanIdentification
--)
--SELECT
--	DCERA.wave, 
--	DCERA.loan_number,
--	UM.CSVID,
--	UM.Borrower_SSN,
--	UM.Commonline_Unique_ID,
--	DCERa.CommonLineUniqueID,
--	UM.LON_IDENT,
--	DCERA.LoanIdentification
--FROM
--	( -- unmatched
--		SELECT
--			CSV.*
--		FROM
--			EA27.._CSVSOURCE_NH_27479 CSV
--			LEFT JOIN _CSV_MATCHES CSVM ON CSVM.CSVID = CSV.CSVID
--		WHERE
--			CSVM.CSVID is NULL
--	) UM -- unmatched
--	INNER JOIN 
--	(
--		SELECT --unique LON_IDENT
--			CSV.Commonline_Unique_ID,
--			COUNT(CSV.Commonline_Unique_ID) CLUID_COUNT
--		FROM
--			EA27.._CSVSOURCE_NH_27479 CSV
--		GROUP BY
--			CSV.Commonline_Unique_ID
--		HAVING
--			COUNT(CSV.Commonline_Unique_ID) = 1
--	) UC ON UC.Commonline_Unique_ID = UM.Commonline_Unique_ID
--	LEFT JOIN
--	(
--		SELECT
--			1 [wave],
--			*
--		FROM
--			EA27_BANA_1.[dbo].[_07_08DisbClaimEnrollRecord] DCER

--		UNION ALL

--		SELECT
--			2 [wave],
--			*
--		FROM
--			EA27_BANA_2.[dbo].[_07_08DisbClaimEnrollRecord] DCER

--		UNION ALL

--		SELECT
--			3 [wave],
--			*
--		FROM
--			EA27_BANA_3.[dbo].[_07_08DisbClaimEnrollRecord] DCER
--	) DCERA ON DCERA.BorrowerSSN = UM.Borrower_SSN AND DCERA.CommonLineUniqueID = UM.Commonline_Unique_ID
--WHERE
--	DCERA.BorrowerSSN IS NOT NULL


---- manual mapping
--DECLARE @ManualMapping TABLE
--(
--	BF_SSN CHAR(9),
--	CSVID INT,
--	SubsidyCode CHAR(1),
--	LN_SEQ INT
--)

--INSERT INTO
--	@ManualMapping
--VALUES
--	('049889783',104884,4,1),
--	('049889783',104885,8,2),
--	('052787346',90595,4,8),
--	('079600851',68654,4,1),
--	('230394337',80533,8,5),
--	('332703106',81229,8,1),
--	('332703106',81232,4,4),
--	('409412004',70750,4,1),
--	('537684725',86031,4,1),
--	('537684725',86032,8,2),
--	('541980966',74556,4,3),
--	('541980966',74557,8,4),
--	('541980966',74558,8,5),
--	('593079641',105115,8,2),
--	('593079641',105114,4,1)

---- second manual mapping
--DECLARE @MappingData TABLE
--(
--	wave tinyint,
--	loan_number int,
--	LN_SEQ tinyint
--)

--INSERT INTO 
--	@MappingData
--VALUES
--(1,27534,2),
--(1,12141,1),
--(1,12141,2),
--(1,12935,3),
--(1,12935,3),
--(1,12936,5),
--(1,12936,5),
--(1,1278,3),
--(1,14472,2),
--(1,14472,2),
--(1,14473,3),
--(1,14473,3),
--(1,3048,1),
--(1,3048,1),
--(1,3049,3),
--(1,3049,3),
--(1,24378,7),
--(1,24378,7),
--(1,3487,1),
--(1,3487,1),
--(1,3488,3),
--(1,3488,3),
--(1,15352,4),
--(1,15352,4),
--(1,15352,5),
--(1,15352,5),
--(1,15576,1),
--(1,15576,1),
--(1,15577,3),
--(1,15577,3),
--(1,31547,1),
--(1,31547,1),
--(1,31547,1),
--(1,31547,1),
--(1,18710,1),
--(1,18710,1),
--(1,18710,2),
--(1,18710,2),
--(1,18711,3),
--(1,18711,3),
--(1,18711,4),
--(1,18711,4),
--(1,18711,5),
--(1,18711,5),
--(1,19139,1),
--(1,19139,1),
--(1,20771,2),
--(1,20771,2),
--(1,20771,3),
--(1,20771,3),
--(1,23034,2),
--(1,23034,2),
--(1,11305,1),
--(1,11305,1),
--(1,11305,2),
--(1,11305,2),
--(1,11306,3),
--(1,11306,3),
--(1,11306,4),
--(1,11306,4),
--(1,37888,1),
--(1,37888,1),
--(1,37889,3),
--(1,37889,3),
--(2,91034,2),
--(2,79895,1),
--(2,79895,2),
--(2,24816,1),
--(2,24816,1),
--(2,24816,2),
--(2,53078,1),
--(2,53078,2),
--(2,66760,2),
--(2,66793,2),
--(2,66809,2),
--(2,544,5),
--(2,66842,1),
--(2,66842,2),
--(2,91085,1),
--(2,91086,2),
--(2,66886,2),
--(2,91109,2),
--(2,66934,2),
--(2,91117,3),
--(2,67005,2),
--(2,41527,1),
--(2,41527,1),
--(2,41527,2),
--(2,67050,2),
--(2,68753,1),
--(2,68753,2),
--(2,1427,1),
--(2,1427,2),
--(2,67116,2),
--(2,91197,2),
--(2,67183,2),
--(2,67298,2),
--(2,26835,3),
--(2,26836,4),
--(2,26837,5),
--(2,26838,6),
--(2,67378,2),
--(2,27045,3),
--(2,27045,3),
--(2,27046,4),
--(2,27046,4),
--(2,91306,1),
--(2,91306,1),
--(2,91306,1),
--(2,91306,1),
--(2,91306,2),
--(2,91307,3),
--(2,91307,3),
--(2,91307,4),
--(2,67432,2),
--(2,67434,1),
--(2,31975,2),
--(2,31977,2),
--(2,70054,1),
--(2,70054,1),
--(2,70054,1),
--(2,70055,2),
--(2,70055,2),
--(2,70055,2),
--(2,92388,3),
--(2,92388,3),
--(2,92388,3),
--(2,65117,1),
--(2,65117,1),
--(2,65118,2),
--(2,65118,2),
--(2,32063,2),
--(2,32147,2),
--(2,3139,1),
--(2,3140,2),
--(2,3140,2),
--(2,3140,2),
--(2,32435,2),
--(2,32612,1),
--(2,32612,2),
--(2,32613,3),
--(2,32613,4),
--(2,32630,2),
--(2,32644,1),
--(2,32644,1),
--(2,32644,2),
--(2,32645,3),
--(2,32645,3),
--(2,32645,4),
--(2,32671,2),
--(2,32915,2),
--(2,91668,2),
--(2,33087,5),
--(2,33088,6),
--(2,33187,2),
--(2,4797,1),
--(2,4797,2),
--(2,4798,3),
--(2,4798,4),
--(2,91712,7),
--(2,91713,8),
--(2,33247,3),
--(2,33299,2),
--(2,33324,1),
--(2,33325,2),
--(2,58129,2),
--(2,58129,2),
--(2,58129,3),
--(2,91750,4),
--(2,33365,3),
--(2,33366,4),
--(2,91773,7),
--(2,91774,8),
--(2,33400,5),
--(2,33401,6),
--(2,33407,1),
--(2,33408,2),
--(2,33519,2),
--(2,91845,1),
--(2,91846,2),
--(2,91867,2),
--(2,91867,2),
--(2,91867,2),
--(2,91867,2),
--(2,91867,2),
--(2,91867,2),
--(2,91867,2),
--(2,91867,2),
--(2,91867,2),
--(2,91867,2),
--(2,91867,3),
--(2,91867,3),
--(2,33707,2),
--(2,91895,2),
--(2,33801,1),
--(2,33802,2),
--(2,33856,2),
--(2,7655,5),
--(2,7655,5),
--(2,91987,2),
--(2,34117,3),
--(2,34179,2),
--(2,21154,2),
--(2,21154,2),
--(2,8647,1),
--(2,8647,2),
--(2,8648,3),
--(2,8648,4),
--(2,34344,2),
--(2,34447,2),
--(2,34447,2),
--(2,34448,4),
--(2,34448,4),
--(2,34448,4),
--(2,92132,2),
--(2,92194,1),
--(2,92194,1),
--(2,92194,2),
--(2,92194,2),
--(2,92195,3),
--(2,92195,4),
--(2,92195,4),
--(2,22355,2),
--(2,22355,3),
--(2,22355,3),
--(2,34721,2),
--(2,92255,4),
--(2,34881,2),
--(2,34988,2),
--(2,35018,6),
--(2,35040,1),
--(2,35041,2),
--(2,35042,3),
--(2,35043,4),
--(3,93671,2),
--(3,93671,2),
--(3,93671,2),
--(3,93671,2),
--(3,115692,3),
--(3,115693,4),
--(3,160591,1),
--(3,160592,2),
--(3,115809,1),
--(3,115809,1),
--(3,115810,2),
--(3,115838,1),
--(3,115838,1),
--(3,115838,1),
--(3,115839,2),
--(3,115959,1),
--(3,115960,2),
--(3,144015,1),
--(3,116017,1),
--(3,116018,2),
--(3,26222,6),
--(3,26222,6),
--(3,116132,1),
--(3,116132,1),
--(3,116132,1),
--(3,38461,1),
--(3,38461,1),
--(3,38462,2),
--(3,38462,2),
--(3,116386,4),
--(3,116387,5),
--(3,155844,3),
--(3,155844,3),
--(3,155844,3),
--(3,155844,3),
--(3,116599,1),
--(3,116599,1),
--(3,116599,1),
--(3,116599,1),
--(3,116600,2),
--(3,116960,1),
--(3,116961,2),
--(3,125136,3),
--(3,125326,2),
--(3,125326,2),
--(3,125326,2),
--(3,125326,2),
--(3,125327,4),
--(3,125327,4),
--(3,125327,4),
--(3,125327,4),
--(3,117063,3),
--(3,117064,4),
--(3,160362,1),
--(3,160362,1),
--(3,160362,1),
--(3,160362,1),
--(3,161133,1),
--(3,161133,1),
--(3,161133,1),
--(3,161134,2),
--(3,117442,2),
--(3,117442,2),
--(3,117442,2),
--(3,101831,3),
--(3,101831,3),
--(3,44049,3),
--(3,44049,3),
--(3,44050,5),
--(3,44050,5),
--(3,101967,2),
--(3,101967,2),
--(3,117522,1),
--(3,117523,2),
--(3,117558,6),
--(3,117559,7),
--(3,117652,1),
--(3,117652,1),
--(3,117652,1),
--(3,117652,1),
--(3,117653,2),
--(3,161299,1),
--(3,161300,2)

--UPDATE
--	CLM
--SET
--	CLM.LN_SEQ = MD.LN_SEQ
--FROM
--	EA27_BANA_1..CompassLoanMapping CLM
--	INNER JOIN @MappingData MD ON MD.loan_number = CLM.loan_number
--WHERE
--	MD.loan_number = 1 -- wave 1 only

--UPDATE
--	CLM
--SET
--	CLM.LN_SEQ = MD.LN_SEQ
--FROM
--	EA27_BANA_2..CompassLoanMapping CLM
--	INNER JOIN @MappingData MD ON MD.loan_number = CLM.loan_number
--WHERE
--	MD.loan_number = 2 -- wave 2 only

--UPDATE
--	CLM
--SET
--	CLM.LN_SEQ = MD.LN_SEQ
--FROM
--	EA27_BANA_3..CompassLoanMapping CLM
--	INNER JOIN @MappingData MD ON MD.loan_number = CLM.loan_number
--WHERE
--	MD.loan_number = 3 -- wave 3 only


--INSERT INTO
--	_CSV_MATCHES
--(
--	wave,
--	loan_number,
--	CSVID,
--	Borrower_SSN,
--	Commonline_Unique_ID,
--	CommonLineUniqueID,
--	LON_IDENT,
--	LoanIdentification
--)
--SELECT
--	0, 
--	MM.LN_SEQ,
--	UM.CSVID,
--	UM.Borrower_SSN,
--	UM.Commonline_Unique_ID,
--	NULL,
--	UM.LON_IDENT,
--	NULL
--FROM
--	(
--		SELECT
--			CSV.*
--		FROM
--			EA27.._CSVSOURCE_NH_27479 CSV
--			LEFT JOIN _CSV_MATCHES CSVM ON CSVM.CSVID = CSV.CSVID
--		WHERE
--			CSVM.CSVID is NULL
--	) UM
--	INNER JOIN @ManualMapping MM ON MM.CSVID = UM.CSVID


---- unmapped
--SELECT DISTINCT
--	CSV.*
--FROM
--	EA27.._CSVSOURCE_NH_27479 CSV
--	LEFT JOIN _CSV_MATCHES CSVM ON CSVM.CSVID = CSV.CSVID
--WHERE
--	CSVM.CSVID is NULL


 --load _EA27_DATA table with EA27 data mapped to CSV file
-- DROP TABLE _EA27_DATA
-- GO

--SELECT
--	EA27.*
--INTO
--	_EA27_DATA
--FROM
--	(
--		SELECT
--			EA27.*,
--			CM.CSVID
--		FROM
--			_CSV_MATCHES CM
--			LEFT JOIN
--			(		
--				SELECT
--					1 [wave],
--					CLM.*
--				FROM
--					EA27_BANA_1.[dbo].[_07_08DisbClaimEnrollRecord] DCER
--					LEFT JOIN EA27_BANA_1.[dbo].CompassLoanMapping CLM ON CLM.loan_number = DCER.loan_number
--			) EA27 ON EA27.wave = CM.wave AND EA27.loan_number = CM.loan_number
--		WHERE
--			CM.wave = 1

--		UNION ALL

--		SELECT
--			EA27.*,
--			CM.CSVID
--		FROM
--			_CSV_MATCHES CM
--			LEFT JOIN
--			(		
--				SELECT
--					2 [wave],
--					CLM.*
--				FROM
--					EA27_BANA_2.[dbo].[_07_08DisbClaimEnrollRecord] DCER
--					INNER JOIN EA27_BANA_2.[dbo].CompassLoanMapping CLM ON CLM.loan_number = DCER.loan_number
--			) EA27 ON EA27.wave = CM.wave AND EA27.loan_number = CM.loan_number
--		WHERE
--			CM.wave = 2

--		UNION ALL

--		SELECT
--			EA27.*,
--			CM.CSVID
--		FROM
--			_CSV_MATCHES CM
--			LEFT JOIN
--			(		
--				SELECT
--					3 [wave],
--					CLM.*
--				FROM
--					EA27_BANA_3.[dbo].[_07_08DisbClaimEnrollRecord] DCER
--					INNER JOIN EA27_BANA_3.[dbo].CompassLoanMapping CLM ON CLM.loan_number = DCER.loan_number
--			) EA27 ON EA27.wave = CM.wave AND EA27.loan_number = CM.loan_number
--		WHERE
--			CM.wave = 3
--	) EA27

--DECLARE @Mapping TABLE
--(
--	SSN CHAR(9),
--	loan_number VARCHAR(25),
--	LN_SEQ VARCHAR (2)	
--)


--INSERT INTO
--	@Mapping
--VALUES
--	('011721015',129956,5),
--	('020725680',24816,1),
--	('027689405',23474,2),
--	('029602722',542,2),
--	('070707586',41420,3),
--	('092927452',41929,1),
--	('093742662',1627,1),
--	('102668394',67212,1),
--	('102668394',67213,3),
--	('107769627',54749,3),
--	('112749483',54854,4),
--	('133740623',91306,1),
--	('133740623',91307,3),
--	('137845025',2529,2),
--	('140764903',37154,7),
--	('172722570',2765,2),
--	('228532775',3424,3),
--	('230317220',32612,1),
--	('230317220',32613,3),
--	('231253006',32644,1),
--	('231253006',32645,3),
--	('244593442',14979,4),
--	('382720503',72238,3),
--	('512961813',34924,2),
--	('537067408',20233,2),
--	('557087860',20771,3),
--	('557986242',27463,1),
--	('558977062',20840,5),
--	('559790294',76224,6)

--UPDATE
--	CLM
--SET
--	CLM.LN_SEQ = M.LN_SEQ
--FROM
--	EA27_BANA_1..CompassLoanMapping CLM
--	INNER JOIN @Mapping M ON M.loan_number = CLM.loan_number AND M.SSN = CLM.BorrowerSsn 

--UPDATE
--	CLM
--SET
--	CLM.LN_SEQ = M.LN_SEQ
--FROM
--	EA27_BANA_2..CompassLoanMapping CLM
--	INNER JOIN @Mapping M ON M.loan_number = CLM.loan_number AND M.SSN = CLM.BorrowerSsn 

--UPDATE
--	CLM
--SET
--	CLM.LN_SEQ = M.LN_SEQ
--FROM
--	EA27_BANA_3..CompassLoanMapping CLM
--	INNER JOIN @Mapping M ON M.loan_number = CLM.loan_number AND M.SSN = CLM.BorrowerSsn


-- R2 – Loans ineligible/disqualified in the file yet have an active, eligible RIR on compass
SELECT DISTINCT
	'R2',
	LN54.BF_SSN,
	LN54.LN_SEQ,
	CSV.Loan_Ident,
	CSV.Borrower_Benefit_Code,
	LN54.PM_BBS_PGM
FROM
	EA27.._CSVSOURCE_NH_27479 CSV
	INNER JOIN _EA27_DATA EA27 ON EA27.CSVID = CSV.CSVID
	INNER JOIN OPENQUERY
	(
		DUSTER,
		'
			SELECT
				LN54.BF_SSN,
				LN54.LN_SEQ,
				LN54.PM_BBS_PGM,
				LN54.LC_STA_LN54,
				LN54.LC_BBS_ELG
			FROM
				OLWHRM1.LN54_LON_BBS LN54
				INNER JOIN OLWHRM1.LN55_LON_BBS_TIR LN55 ON LN55.BF_SSN = LN54.BF_SSN and LN55.LN_SEQ = LN54.LN_SEQ
			WHERE
				LN54.LC_STA_LN54 = ''A''
				AND
				LN54.LC_BBS_ELG = ''Y''
				AND
				LN55.LC_STA_LN55 = ''A''
				AND
				LN55.LC_LON_BBT_STA IN (''A'', ''Q'', ''R'')
		'
	) LN54 ON LN54.BF_SSN = EA27.BorrowerSsn AND LN54.LN_SEQ = EA27.LN_SEQ
WHERE
	CSV.Disqualification_Date > '000000'
	OR
	(
		CSV.Disqualification_Date = '000000'
		AND
		(
			(
				CSV.Interest_Status IN ('00000', '00004', '00005', '00006', '00008', '00010', '00011', '00013', '00015', '00016', '00019', '00099')
				AND
				CSV.Borrower_Benefit_Code IN ('0290','0835','1560','1565','1717','0007','1716','1000','1520','4400','5101','6742','6749','1570','1580','6720','1706','0830','1715','2585','2590','6740','6741','6744','6745','6746','6747','1710','0006','1020','0230','0800','1010','1530','3330','3335','3340')
			)
			OR
			(
				CSV.Rebate_Status IN ('00000', '00004', '00005', '00006', '00008', '00011', '00013', '00014', '00020', '00022', '00023', '00024', '00025', '00026', '00099')
				AND
				CSV.Borrower_Benefit_Code IN ('2740','6748','2204','2205','2206','2207','2208','2550','2552','2555','2560','2565','2214','2215','2216','2217','2530','2533','2535','2540','2570','2573','2575','2580','7722','7723','7724','7725')
			)
		)
	)
ORDER BY
	LN54.BF_SSN


-- R3 – Loans eligible/qualified in the file yet do not have an active BB on compass
SELECT DISTINCT
	'R3',
	CSV.Borrower_SSN [BF_SSN],
	EA27.LN_SEQ,
	CSV.Loan_Ident,
	CSV.Borrower_Benefit_Code
FROM
	EA27.._CSVSOURCE_NH_27479 CSV
	INNER JOIN _EA27_DATA EA27 ON EA27.CSVID = CSV.CSVID
	LEFT JOIN OPENQUERY
	(
		DUSTER,
		'
			SELECT
				LN54.BF_SSN,
				LN54.LN_SEQ,
				MIN(LN54.LC_STA_LN54) "LC_STA_LN54"
			FROM
				OLWHRM1.LN54_LON_BBS LN54
			GROUP BY
				LN54.BF_SSN,
				LN54.LN_SEQ
		'
	) LN54 ON LN54.BF_SSN = EA27.BorrowerSsn AND LN54.LN_SEQ = EA27.LN_SEQ
WHERE
	(
		LN54.BF_SSN IS NULL
		OR
		LN54.LC_STA_LN54 = 'I'
	)
	AND
	(
		CSV.Disqualification_Date = '000000'
		AND
		(
			(
				CSV.Interest_Status IN ('00001', '00002', '00003', '00007', '00009', '00012', '00014', '00017', '00018')
				AND
				CSV.Borrower_Benefit_Code IN ('0290','0835','1560','1565','1717','0007','1716','1000','1520','4400','5101','6742','6749','1570','1580','6720','1706','0830','1715','2585','2590','6740','6741','6744','6745','6746','6747','1710','0006','1020','0230','0800','1010','1530','3330','3335','3340')
			)
			OR
			(
				CSV.Rebate_Status IN ('00001', '00002', '00003', '00007', '00009', '00010', '00012', '00015', '00016', '00017', '00018', '00019', '00021', '00027')
				AND
				CSV.Borrower_Benefit_Code IN ('2740','6748','2204','2205','2206','2207','2208','2550','2552','2555','2560','2565','2214','2215','2216','2217','2530','2533','2535','2540','2570','2573','2575','2580','7722','7723','7724','7725')
			)
		)
	)
ORDER BY
	CSV.Borrower_SSN


-- R4 – Loans not listed in the file yet have an active BANA BB on compass
SELECT DISTINCT
	'R4',
	LN54.BF_SSN,
	LN54.LN_SEQ,
	CSV.Loan_Ident,
	CSV.Borrower_Benefit_Code,
	LN54.PM_BBS_PGM
-- SELECT *
FROM
	OPENQUERY
	(
		DUSTER,
		'
			SELECT DISTINCT
				LN54.BF_SSN,
				LN54.LN_SEQ,
				LN54.PM_BBS_PGM,
				LN54.LC_STA_LN54,
				LN54.LC_BBS_ELG
			FROM 
				OLWHRM1.LN54_LON_BBS LN54
			WHERE
				LN54.PM_BBS_PGM LIKE ''B%''
				AND 
				LN54.LC_STA_LN54 = ''A''
				AND 
				LN54.LC_BBS_ELG = ''Y''
				AND
				LN54.BF_SSN = ''049889783''
		'
	) LN54 
	LEFT JOIN _EA27_DATA EA27 ON EA27.BorrowerSsn = LN54.BF_SSN --AND EA27.LN_SEQ = LN54.LN_SEQ
	LEFT JOIN EA27.._CSVSOURCE_NH_27479 CSV ON EA27.CSVID = CSV.CSVID
WHERE
	EA27.BorrowerSsn IS NULL

-- helper queries for resolving mapping problems
--SELECT
--	*
--FROM
--	EA27.._CSVSOURCE_NH_27479 CSV
--	LEFT JOIN _EA27_DATA EA27 ON EA27.CSVID = CSV.CSVID
--WHERE
--	CSV.Borrower_SSN = '005885530'

--UPDATE
--	CLM
--SET
--	CLM.LN_SEQ = 3
---- SELECT *
--FROM
--	EA27_BANA_2..CompassLoanMapping CLM
--WHERE
--	CLM.BorrowerSsn = '642035557'
--	AND
--	CLM.loan_number = 35042

--SELECT TOP 10 * FROM _CSV_MATCHES CSV WHERE CSV.Borrower_SSN = '642035557'

--DELETE FROM _CSV_MATCHES WHERE Borrower_SSN = '005847779' AND loan_number = 66669 AND CSVID = 62683

-- R5 – Loans that have an active mismatched BB between the file and compass
-- build benefit code mapping table
DECLARE @BBMapping TABLE
(
	BBCode VARCHAR(10),
	UHEAACode VARCHAR(3)
)

-- insert mapping values
INSERT INTO 
	@BBMapping
VALUES
('0290','BI1'),
('0835','BI1'),
('1560','BI2'),
('1565','BI2'),
('1717','BI2'),
('0007','BI3'),
('1716','BI3'),
('1000','BI4'),
('1520','BI4'),
('4400','BI4'),
('5101','BI4'),
('6742','BI4'),
('6749','BI4'),
('1570','BI5'),
('1580','BI5'),
('6720','BI5'),
('1706','BI6'),
('0830','BI7'),
('1715','BI7'),
('2585','BI8'),
('2590','BI8'),
('6740','BI9'),
('6741','BI9'),
('6744','BIA'),
('6745','BIA'),
('6746','BIB'),
('6747','BIB'),
('1710','BIC'),
('0006','BID'),
('1020','BID'),
('0230','BIE'),
('0800','BIE'),
('1010','BIE'),
('1530','BIE'),
('2740','BR1'),
('6748','BR1'),
('2204','BT1')

-- select R5 data
SELECT DISTINCT
	'R5',
	LN54.BF_SSN,
	LN54.LN_SEQ,
	CSV.Loan_Ident,
	CSV.Borrower_Benefit_Code,
	LN54.PM_BBS_PGM,
	BBM_CSV.UHEAACode [ShouldBe]
FROM
	EA27.._CSVSOURCE_NH_27479 CSV
	INNER JOIN _EA27_DATA EA27 ON EA27.CSVID = CSV.CSVID
	INNER JOIN OPENQUERY
	(
		DUSTER,
		'
			SELECT
				LN54.BF_SSN,
				LN54.LN_SEQ,
				LN54.PM_BBS_PGM,
				LN54.LC_STA_LN54,
				LN54.LC_BBS_ELG
			FROM
				OLWHRM1.LN54_LON_BBS LN54 
			WHERE
				LN54.LC_STA_LN54 = ''A''
				AND
				LN54.LC_BBS_ELG = ''Y''
		'
	) LN54 ON LN54.BF_SSN = EA27.BorrowerSsn AND LN54.LN_SEQ = EA27.LN_SEQ
	LEFT JOIN @BBMapping BBM_LN54 ON BBM_LN54.UHEAACode = LN54.PM_BBS_PGM
	LEFT JOIN @BBMapping BBM_CSV ON BBM_CSV.BBCode = CSV.Borrower_Benefit_Code
WHERE
	BBM_LN54.UHEAACode != BBM_CSV.UHEAACode
ORDER BY
	LN54.BF_SSN
