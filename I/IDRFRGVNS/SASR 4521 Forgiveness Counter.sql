USE CDW

GO

--BEGIN Base forgiveness population
DECLARE @ICRPlan TABLE(BF_SSN CHAR(9), LN_SEQ INT, CurrentActiveSchedule VARCHAR(20), ScheduleCode VARCHAR(2), PmtQlfy_Level INT, PmtQlfy_LevelPrev INT, PmtQlfy_PreConvIBR INT, PmtQlfy_PreConvICR INT, PmtQlfy_IDR INT, PmtQlfy_IDRPrev INT, PmtQlfy_PermStnd INT, PmtQlfy_PermStndPrev INT, PmtCoveredByEHD INT, Total INT)
DECLARE @IBRPlan TABLE(BF_SSN CHAR(9), LN_SEQ INT, CurrentActiveSchedule VARCHAR(20), ScheduleCode VARCHAR(2), PmtQlfy_Level INT, PmtQlfy_LevelPrev INT, PmtQlfy_PreConvIBR INT, PmtQlfy_PreConvICR INT, PmtQlfy_IDR INT, PmtQlfy_IDRPrev INT, PmtQlfy_PermStnd INT, PmtQlfy_PermStndPrev INT, PmtCoveredByEHD INT, Total INT)
DECLARE @IBR2014Plan TABLE(BF_SSN CHAR(9), LN_SEQ INT, CurrentActiveSchedule VARCHAR(20), ScheduleCode VARCHAR(2), PmtQlfy_Level INT, PmtQlfy_LevelPrev INT, PmtQlfy_PreConvIBR INT, PmtQlfy_PreConvICR INT, PmtQlfy_IDR INT, PmtQlfy_IDRPrev INT, PmtQlfy_PermStnd INT, PmtQlfy_PermStndPrev INT, PmtCoveredByEHD INT, Total INT)
DECLARE @PAYEPlan TABLE(BF_SSN CHAR(9), LN_SEQ INT, CurrentActiveSchedule VARCHAR(20), ScheduleCode VARCHAR(2), PmtQlfy_Level INT, PmtQlfy_LevelPrev INT, PmtQlfy_PreConvIBR INT, PmtQlfy_PreConvICR INT, PmtQlfy_IDR INT, PmtQlfy_IDRPrev INT, PmtQlfy_PermStnd INT, PmtQlfy_PermStndPrev INT, PmtCoveredByEHD INT, Total INT)
DECLARE @REPAYEPlan TABLE(BF_SSN CHAR(9), LN_SEQ INT, CurrentActiveSchedule VARCHAR(20), ScheduleCode VARCHAR(2), PmtQlfy_Level INT, PmtQlfy_LevelPrev INT, PmtQlfy_PreConvIBR INT, PmtQlfy_PreConvICR INT, PmtQlfy_IDR INT, PmtQlfy_IDRPrev INT, PmtQlfy_PermStnd INT, PmtQlfy_PermStndPrev INT, PmtCoveredByEHD INT, Total INT)
DECLARE @OverallPlan TABLE(Projection VARCHAR(10), BF_SSN CHAR(9), LN_SEQ INT, CurrentActiveSchedule VARCHAR(20), ScheduleCode VARCHAR(2), PmtQlfy_Level INT, PmtQlfy_LevelPrev INT, PmtQlfy_PreConvIBR INT, PmtQlfy_PreConvICR INT, PmtQlfy_IDR INT, PmtQlfy_IDRPrev INT, PmtQlfy_PermStnd INT, PmtQlfy_PermStndPrev INT, PmtCoveredByEHD INT, Total INT)

INSERT INTO @ICRPlan(BF_SSN, LN_SEQ, CurrentActiveSchedule, ScheduleCode, PmtQlfy_Level, PmtQlfy_LevelPrev, PmtQlfy_PreConvIBR, PmtQlfy_PreConvICR, PmtQlfy_IDR, PmtQlfy_IDRPrev,PmtQlfy_PermStnd, PmtQlfy_PermStndPrev, PmtCoveredByEHD, Total)
EXEC dbo.GetQualifyingIDRForgivenessPayments 'ICR'
INSERT INTO @IBRPlan(BF_SSN, LN_SEQ, CurrentActiveSchedule, ScheduleCode, PmtQlfy_Level, PmtQlfy_LevelPrev, PmtQlfy_PreConvIBR, PmtQlfy_PreConvICR, PmtQlfy_IDR, PmtQlfy_IDRPrev,PmtQlfy_PermStnd, PmtQlfy_PermStndPrev, PmtCoveredByEHD, Total)
EXEC dbo.GetQualifyingIDRForgivenessPayments 'IBR'
INSERT INTO @IBR2014Plan(BF_SSN, LN_SEQ, CurrentActiveSchedule, ScheduleCode, PmtQlfy_Level, PmtQlfy_LevelPrev, PmtQlfy_PreConvIBR, PmtQlfy_PreConvICR, PmtQlfy_IDR, PmtQlfy_IDRPrev,PmtQlfy_PermStnd, PmtQlfy_PermStndPrev, PmtCoveredByEHD, Total)
EXEC dbo.GetQualifyingIDRForgivenessPayments 'IBR 2014'
INSERT INTO @PAYEPlan(BF_SSN, LN_SEQ, CurrentActiveSchedule, ScheduleCode, PmtQlfy_Level, PmtQlfy_LevelPrev, PmtQlfy_PreConvIBR, PmtQlfy_PreConvICR, PmtQlfy_IDR, PmtQlfy_IDRPrev,PmtQlfy_PermStnd, PmtQlfy_PermStndPrev, PmtCoveredByEHD, Total)
EXEC dbo.GetQualifyingIDRForgivenessPayments 'PAYE'
INSERT INTO @REPAYEPlan(BF_SSN, LN_SEQ, CurrentActiveSchedule, ScheduleCode, PmtQlfy_Level, PmtQlfy_LevelPrev, PmtQlfy_PreConvIBR, PmtQlfy_PreConvICR, PmtQlfy_IDR, PmtQlfy_IDRPrev,PmtQlfy_PermStnd, PmtQlfy_PermStndPrev, PmtCoveredByEHD, Total)
EXEC dbo.GetQualifyingIDRForgivenessPayments 'REPAYE'


INSERT INTO @OverallPlan(Projection, BF_SSN, LN_SEQ, CurrentActiveSchedule, ScheduleCode, PmtQlfy_Level, PmtQlfy_LevelPrev, PmtQlfy_PreConvIBR, PmtQlfy_PreConvICR, PmtQlfy_IDR, PmtQlfy_IDRPrev,PmtQlfy_PermStnd, PmtQlfy_PermStndPrev, PmtCoveredByEHD, Total)
SELECT 'ICR' AS Projection, * FROM @ICRPlan
UNION ALL
SELECT 'IBR' AS Projection, * FROM @IBRPlan
UNION ALL
SELECT 'IBR 2014' AS Projection, * FROM @IBR2014Plan
UNION ALL
SELECT 'PAYE' AS Projection, * FROM @PAYEPlan
UNION ALL
SELECT 'REPAYE' AS Projection, * FROM @REPAYEPlan

--SELECT * FROM @OverallPlan order by BF_SSN, LN_SEQ, Projection --For testing
--END Base Population

--BEGIN Forgiveness
IF OBJECT_ID('tempdb..#ForgivenessArcPopulation') IS NOT NULL
	BEGIN
		DROP TABLE #ForgivenessArcPopulation
	END

-- create temp table with AAP data to be added to CLS.ArcAddProcessing table
SELECT
	LN10.LN_SEQ,
	0 [ArcTypeId], -- by loan
	LN10.BF_SSN,
	PD10.DF_SPE_ACC_ID [AccountNumber],
	CASE 
		 WHEN OP.Total >= 300 AND Arcs.HASFSAFM = 1 AND Arcs.HASIDRFG = 0 AND OP.ScheduleCode IN('C1','C2','C3','IB','CQ','IL') THEN 'IDRFG' --IBR ICR
		 WHEN OP.Total >= 300 AND Arcs.HASFSAFM = 1 AND Arcs.HASIDRFG = 0 AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('A','B','C','D','5','6','7','8','9','06','07','08','09','10','11','12','13','14','15') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'IDRFG' --REPAYE UNDERGRAD
 		 WHEN OP.Total BETWEEN 294 AND 299 AND Arcs.HASFSA6M = 1 AND Arcs.HASIDRFP = 0 AND OP.ScheduleCode IN('C1','C2','C3','IB','CQ','IL') THEN 'IDRFP' --IBR ICR
		 WHEN OP.Total BETWEEN 294 AND 299 AND Arcs.HASFSA6M = 1 AND Arcs.HASIDRFP = 0 AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('A','B','C','D','5','6','7','8','9','06','07','08','09','10','11','12','13','14','15') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'IDRFP' --REPAYE UNDERGRAD
		 WHEN OP.Total >= 240 AND Arcs.HASFSAFM = 1 AND Arcs.HASIDRFG = 0 AND OP.ScheduleCode IN('I3','IP','CA','CP') THEN 'IDRFG' --IBR 2014 / PAYE
		 WHEN OP.Total >= 240 AND Arcs.HASFSAFM = 1 AND Arcs.HASIDRFG = 0 AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('01','02','03','04','05','2','3','4','5') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'IDRFG' --REPAYE GRAD
		 WHEN OP.Total BETWEEN 234 AND 239 AND Arcs.HASFSA6M = 1 AND Arcs.HASIDRFP = 0 AND OP.ScheduleCode IN('I3','IP','CA','CP') THEN 'IDRFP' --IBR 2014 / PAYE
		 WHEN OP.Total BETWEEN 234 AND 239 AND Arcs.HASFSA6M = 1 AND Arcs.HASIDRFP = 0 AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('01','02','03','04','05','2','3','4','5') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'IDRFP' --REPAYE GRAD
		 WHEN OP.Total >= 300 AND(ISNULL(Arcs.HASIDFRF,0) = 0 OR (Arcs.HASIDFRF = 1 AND Arcs.HASIDFRT = 1 AND (Arcs.MaxIDFRFDate < Arcs.MaxIDFRTDate AND GETDATE() - 30 > Arcs.MaxIDFRTDate)/*re-do full month review if IDFRT is newer, and more than 30 days old*/)) AND OP.ScheduleCode IN('C1','C2','C3','IB','CQ','IL') THEN 'IDFRF' --IBR ICR
		 WHEN OP.Total >= 300 AND(ISNULL(Arcs.HASIDFRF,0) = 0 OR (Arcs.HASIDFRF = 1 AND Arcs.HASIDFRT = 1 AND (Arcs.MaxIDFRFDate < Arcs.MaxIDFRTDate AND GETDATE() - 30 > Arcs.MaxIDFRTDate)/*re-do full month review if IDFRT is newer, and more than 30 days old*/)) AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('A','B','C','D','5','6','7','8','9','06','07','08','09','10','11','12','13','14','15') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'IDFRF' --REPAYE UNDERGRAD
		 WHEN OP.Total >= 240 AND ISNULL(Arcs.HASFAT25,0) = 0 AND(ISNULL(Arcs.HASIDFRF,0) = 0 OR (Arcs.HASIDFRF = 1 AND Arcs.HASIDFRT = 1 AND (Arcs.MaxIDFRFDate < Arcs.MaxIDFRTDate AND GETDATE() - 30 > Arcs.MaxIDFRTDate)/*re-do full month review if IDFRT is newer, and more than 30 days old*/)) AND OP.ScheduleCode IN('I3','IP','CA','CP') THEN 'IDFRF' --IBR 2014 / PAYE
		 WHEN OP.Total >= 240 AND ISNULL(Arcs.HASFAT25,0) = 0 AND(ISNULL(Arcs.HASIDFRF,0) = 0 OR (Arcs.HASIDFRF = 1 AND Arcs.HASIDFRT = 1 AND (Arcs.MaxIDFRFDate < Arcs.MaxIDFRTDate AND GETDATE() - 30 > Arcs.MaxIDFRTDate)/*re-do full month review if IDFRT is newer, and more than 30 days old*/)) AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('01','02','03','04','05','2','3','4','5') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'IDFRF' --REPAYE GRAD
		 ELSE ''
	END [ARC],
	'IDRFRGVNS' [ScriptId],
	GETDATE() [ProcessOn],
	CASE WHEN OP.Total >= 300 AND Arcs.HASFSAFM = 1 AND Arcs.HASIDRFG = 0 AND OP.ScheduleCode IN('C1','C2','C3','IB','CQ','IL') THEN 'Borrower approved in full forgiveness review. Trigger TS06BIDRFG letter. 25 year plan'
		 WHEN OP.Total >= 300 AND Arcs.HASFSAFM = 1 AND Arcs.HASIDRFG = 0 AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('A','B','C','D','5','6','7','8','9','06','07','08','09','10','11','12','13','14','15') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'Borrower approved in full forgiveness review. Trigger TS06BIDRFG letter. 25 year plan'
		 WHEN OP.Total BETWEEN 294 AND 299 AND Arcs.HASFSA6M = 1 AND Arcs.HASIDRFP = 0 AND OP.ScheduleCode IN('C1','C2','C3','IB','CQ','IL') THEN 'Borrower approved in 6 month forgiveness review. Trigger TS06BIDRFP letter. 25 year plan'
		 WHEN OP.Total BETWEEN 294 AND 299 AND Arcs.HASFSA6M = 1 AND Arcs.HASIDRFP = 0 AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('A','B','C','D','5','6','7','8','9','06','07','08','09','10','11','12','13','14','15') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'Borrower approved in 6 month forgiveness review. Trigger TS06BIDRFP letter. 25 year plan'
	     WHEN OP.Total >= 240 AND Arcs.HASFSAFM = 1 AND Arcs.HASIDRFG = 0 AND OP.ScheduleCode IN('I3','IP','CA','CP') THEN 'Borrower approved in full forgiveness review. Trigger TS06BIDRFG letter. 20 year plan'
		 WHEN OP.Total >= 240 AND Arcs.HASFSAFM = 1 AND Arcs.HASIDRFG = 0 AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('01','02','03','04','05','2','3','4','5') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'Borrower approved in full forgiveness review. Trigger TS06BIDRFG letter. 20 year plan'
		 WHEN OP.Total BETWEEN 234 AND 239 AND Arcs.HASFSA6M = 1 AND Arcs.HASIDRFP = 0 AND OP.ScheduleCode IN('I3','IP','CA','CP') THEN 'Borrower approved in 6 month forgiveness review. Trigger TS06BIDRFP letter. 20 year plan'
		 WHEN OP.Total BETWEEN 234 AND 239 AND Arcs.HASFSA6M = 1 AND Arcs.HASIDRFP = 0 AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('01','02','03','04','05','2','3','4','5') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'Borrower approved in 6 month forgiveness review. Trigger TS06BIDRFP letter. 20 year plan'
		 WHEN OP.Total >= 300 AND(ISNULL(HASIDFRF,0) = 0 OR (Arcs.HASIDFRF = 1 AND Arcs.HASIDFRT = 1 AND (Arcs.MaxIDFRFDate < Arcs.MaxIDFRTDate AND GETDATE() - 30 > Arcs.MaxIDFRTDate)/*re-do full month review if IDFRT is newer, and more than 30 days old*/)) AND OP.ScheduleCode IN('C1','C2','C3','IB','CQ','IL') THEN 'Borrower requires full eligibility forgiveness review. 25 year plan'	
		 WHEN OP.Total >= 300 AND(ISNULL(HASIDFRF,0) = 0 OR (Arcs.HASIDFRF = 1 AND Arcs.HASIDFRT = 1 AND (Arcs.MaxIDFRFDate < Arcs.MaxIDFRTDate AND GETDATE() - 30 > Arcs.MaxIDFRTDate)/*re-do full month review if IDFRT is newer, and more than 30 days old*/)) AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('A','B','C','D','5','6','7','8','9','06','07','08','09','10','11','12','13','14','15') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'Borrower requires full eligibility forgiveness review. 25 year plan'	
		 WHEN OP.Total >= 240 AND ISNULL(HASFAT25,0) = 0 AND(ISNULL(Arcs.HASIDFRF,0) = 0 OR (Arcs.HASIDFRF = 1 AND Arcs.HASIDFRT = 1 AND (Arcs.MaxIDFRFDate < Arcs.MaxIDFRTDate AND GETDATE() - 30 > Arcs.MaxIDFRTDate)/*re-do full month review if IDFRT is newer, and more than 30 days old*/)) AND OP.ScheduleCode IN('I3','IP','CA','CP') THEN 'Borrower requires full eligibility forgiveness review. 20 year plan'
		 WHEN OP.Total >= 240 AND ISNULL(HASFAT25,0) = 0 AND(ISNULL(Arcs.HASIDFRF,0) = 0 OR (Arcs.HASIDFRF = 1 AND Arcs.HASIDFRT = 1 AND (Arcs.MaxIDFRFDate < Arcs.MaxIDFRTDate AND GETDATE() - 30 > Arcs.MaxIDFRTDate)/*re-do full month review if IDFRT is newer, and more than 30 days old*/)) AND OP.ScheduleCode IN('I5','IA') AND (LN10.LC_ACA_GDE_LEV IN('01','02','03','04','05','2','3','4','5') OR LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS')) THEN 'Borrower requires full eligibility forgiveness review. 20 year plan'
	     ELSE ''
	END [Comment],
	0 [IsReference],
	0 [IsEndorser]
INTO
	#ForgivenessArcPopulation
FROM
	@OverallPlan OP
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = OP.BF_SSN
	INNER JOIN CDW..LN10_LON LN10 
		ON LN10.BF_SSN = OP.BF_SSN
		AND LN10.LN_SEQ = OP.LN_SEQ
	INNER JOIN CDW..LN65_LON_RPS LN65 
		ON LN65.BF_SSN = LN10.BF_SSN 
		AND LN65.LN_SEQ = LN10.LN_SEQ
		AND	LN65.LC_STA_LON65 = 'A' -- active
		AND	LN65.LC_TYP_SCH_DIS IN ('CA','CP', 'C1', 'C2', 'C3', 'CQ', 'IB', 'IL', 'I3', 'IP', 'I5')--removed IA because they cant get forgiveness until moving back to I5
	LEFT JOIN CDW..WQ20_TSK_QUE WQ20
		ON WQ20.BF_SSN = LN10.BF_SSN
		AND WQ20.WF_QUE = 'BO'
		AND WQ20.WC_STA_WQUE20 NOT IN('X','C') --Has an open queue
	LEFT JOIN
	( 
		SELECT DISTINCT
			AY10.BF_SSN,
			LN85.LN_SEQ,
			SUM(CASE WHEN ArcExists.BF_SSN IS NOT NULL AND AY10.PF_REQ_ACT = 'FAT25' THEN 1 ELSE 0 END) OVER (PARTITION BY AY10.BF_SSN, LN85.LN_SEQ) AS HASFAT25,
			SUM(CASE WHEN ArcExists.BF_SSN IS NOT NULL AND AY10.PF_REQ_ACT = 'FSA6M' THEN 1 ELSE 0 END) OVER (PARTITION BY AY10.BF_SSN, LN85.LN_SEQ) AS HASFSA6M,
			SUM(CASE WHEN ArcExists.BF_SSN IS NOT NULL AND AY10.PF_REQ_ACT = 'FSAFM' THEN 1 ELSE 0 END) OVER (PARTITION BY AY10.BF_SSN, LN85.LN_SEQ) AS HASFSAFM,
			SUM(CASE WHEN ArcExists.BF_SSN IS NOT NULL AND AY10.PF_REQ_ACT = 'IDFRT' THEN 1 ELSE 0 END) OVER (PARTITION BY AY10.BF_SSN, LN85.LN_SEQ) AS HASIDFRT,
			SUM(CASE WHEN ArcExists.BF_SSN IS NOT NULL AND AY10.PF_REQ_ACT = 'IDFRF' THEN 1 ELSE 0 END) OVER (PARTITION BY AY10.BF_SSN, LN85.LN_SEQ) AS HASIDFRF,
			SUM(CASE WHEN ArcExists.BF_SSN IS NOT NULL AND AY10.PF_REQ_ACT = 'IDRFG' THEN 1 ELSE 0 END) OVER (PARTITION BY AY10.BF_SSN, LN85.LN_SEQ) AS HASIDRFG,
			SUM(CASE WHEN ArcExists.BF_SSN IS NOT NULL AND AY10.PF_REQ_ACT = 'IDRFP' THEN 1 ELSE 0 END) OVER (PARTITION BY AY10.BF_SSN, LN85.LN_SEQ) AS HASIDRFP,
			MAX(CASE WHEN ArcExists.BF_SSN IS NOT NULL AND AY10.PF_REQ_ACT = 'IDFRF' THEN ArcExists.LD_ATY_REQ_RCV ELSE NULL END) OVER (PARTITION BY AY10.BF_SSN, LN85.LN_SEQ) AS MaxIDFRFDate,
			MAX(CASE WHEN ArcExists.BF_SSN IS NOT NULL AND AY10.PF_REQ_ACT = 'IDFRT' THEN ArcExists.LD_ATY_REQ_RCV ELSE NULL END) OVER (PARTITION BY AY10.BF_SSN, LN85.LN_SEQ) AS MaxIDFRTDate,
			MAX(CASE WHEN ArcExists.BF_SSN IS NOT NULL AND AY10.PF_REQ_ACT = 'FAT25' THEN ArcExists.LD_ATY_REQ_RCV ELSE NULL END) OVER (PARTITION BY AY10.BF_SSN, LN85.LN_SEQ) AS MaxFAT25Date
		FROM
			CDW..AY10_BR_LON_ATY AY10
			INNER JOIN CDW..LN85_LON_ATY LN85
				ON LN85.BF_SSN = AY10.BF_SSN
				AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
			LEFT JOIN
			(
				SELECT
					AY10.BF_SSN,
					LN85.LN_SEQ,
					AY10.LN_ATY_SEQ,
					AY10.PF_REQ_ACT,
					MAX(CAST(AY10.LD_ATY_REQ_RCV AS DATE)) OVER(PARTITION BY AY10.BF_SSN, LN85.LN_SEQ, AY10.PF_REQ_ACT) AS LD_ATY_REQ_RCV
				FROM
					CDW..AY10_BR_LON_ATY AY10
					INNER JOIN CDW..LN85_LON_ATY LN85 
						ON LN85.BF_SSN = AY10.BF_SSN 
						AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				WHERE
					AY10.PF_REQ_ACT IN('IDFRT','FAT25','FSA6M','FSAFM','IDFRF','IDRFG','IDRFP')
					AND AY10.LC_STA_ACTY10 = 'A'
			) ArcExists
				ON ArcExists.BF_SSN = AY10.BF_SSN
				AND ArcExists.LN_SEQ = LN85.LN_SEQ
				AND ArcExists.LN_ATY_SEQ = AY10.LN_ATY_SEQ
				AND ArcExists.PF_REQ_ACT = AY10.PF_REQ_ACT
		WHERE
			AY10.PF_REQ_ACT IN ('IDFRT','FAT25','FSA6M','FSAFM','IDFRF','IDRFG','IDRFP')
	) Arcs 
		ON Arcs.BF_SSN = LN10.BF_SSN 
		AND Arcs.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN 
	( -- prevent duplicate inserts
		SELECT
			AccountNumber,
			ALSS.LoanSequence,
			AAP.ARC
		FROM
			CLS..ArcAddProcessing AAP 
			INNER JOIN CLS..ArcLoanSequenceSelection ALSS
				ON ALSS.ArcAddProcessingId = AAP.ArcAddProcessingId 
		WHERE
			AAP.ScriptId = 'IDRFRGVNS'
			AND	AAP.ArcTypeId = 0 --by loan type
			AND	CAST(AAP.CreatedAt AS DATE) <= CAST(GETDATE() AS DATE)
			AND AAP.ProcessedAt IS NULL
			AND
			(
				(
					AAP.Comment IN('Borrower approved in 6 month forgiveness review. Trigger TS06BIDRFP letter. 20 year plan','Borrower approved in 6 month forgiveness review. Trigger TS06BIDRFP letter. 25 year plan')
					AND AAP.ARC = 'IDRFP'
				)
				OR
				(
					AAP.Comment IN('Borrower requires full eligibility forgiveness review. 20 year plan','Borrower requires full eligibility forgiveness review. 25 year plan')
					AND AAP.ARC = 'IDFRF'
				)
				OR
				(
					AAP.Comment IN('Borrower approved in full forgiveness review. Trigger TS06BIDRFG letter. 20 year plan','Borrower approved in full forgiveness review. Trigger TS06BIDRFG letter. 25 year plan')
					AND AAP.ARC = 'IDRFG'
				)
			)			
	) AAP
		ON AAP.AccountNumber = PD10.DF_SPE_ACC_ID
		AND AAP.LoanSequence = LN10.LN_SEQ
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND OP.CurrentActiveSchedule IS NOT NULL -- dont drop arcs on non active people
	AND	AAP.LoanSequence IS NULL -- prevent duplicate inserts in the event the script is run multiple times before a warehouse refresh completes
	AND WQ20.BF_SSN IS NULL -- No open BO queue
	AND OP.Projection + ' - ' + LN65.LC_TYP_SCH_DIS = OP.CurrentActiveSchedule

BEGIN TRANSACTION
DECLARE @ERROR INT

-- insert data into AAP

INSERT INTO
	CLS..ArcAddProcessing
(
	ArcTypeId,
	AccountNumber,
	ARC,
	ScriptId,
	ProcessOn,
	Comment,
	IsReference,
	IsEndorser
)
SELECT DISTINCT
	AD.ArcTypeId,
	AD.AccountNumber,
	AD.ARC,
	AD.ScriptId,
	AD.ProcessOn,
	AD.Comment,
	AD.IsReference,
	AD.IsEndorser
FROM
	#ForgivenessArcPopulation AD
WHERE
	AD.ARC != ''
	AND AD.Comment != ''

SET @ERROR = @@ERROR


--Insert Loan Sequences 

INSERT INTO CLS..ArcLoanSequenceSelection
(
	ArcAddProcessingId,
	LoanSequence
)
SELECT DISTINCT
	AAP.ArcAddProcessingId,
	AD.LN_SEQ
FROM
	#ForgivenessArcPopulation AD
	INNER JOIN CLS..ArcAddProcessing AAP 
		ON AAP.AccountNumber = AD.AccountNumber
WHERE
	AD.ARC != ''
	AND AD.Comment != ''
	AND AAP.ScriptId = 'IDRFRGVNS'
	AND	AAP.ArcTypeId = 0
	AND	CAST(AAP.CreatedAt AS DATE) BETWEEN CAST(GETDATE()-1 AS DATE) AND CAST(GETDATE() AS DATE) --Give 24 hour buffer for arcs to process before re-adding
	AND	AAP.ProcessedAt IS NULL
	AND
	(
		(
			AAP.Comment IN('Borrower approved in 6 month forgiveness review. Trigger TS06BIDRFP letter. 20 year plan','Borrower approved in 6 month forgiveness review. Trigger TS06BIDRFP letter. 25 year plan')
			AND AAP.ARC = 'IDRFP'
		)
		OR
		(
			AAP.Comment IN('Borrower requires full eligibility forgiveness review. 20 year plan','Borrower requires full eligibility forgiveness review. 25 year plan')
			AND AAP.ARC = 'IDFRF'
		)
		OR
		(
			AAP.Comment IN('Borrower approved in full forgiveness review. Trigger TS06BIDRFG letter. 20 year plan','Borrower approved in full forgiveness review. Trigger TS06BIDRFG letter. 25 year plan')
			AND AAP.ARC = 'IDRFG'
		)
	)	

SET @ERROR = @ERROR + @@ERROR

IF OBJECT_ID('tempdb..#ForgivenessArcPopulation') IS NOT NULL
	BEGIN
		DROP TABLE #ForgivenessArcPopulation
	END

SET @ERROR = @ERROR + @@ERROR

IF @ERROR != 0
	BEGIN
		PRINT '!!! ERROR - The transaction was rolled back.'
		RAISERROR('!!! ERROR - The transaction was rolled back.', 16, 1)
		ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'The transaction was committed.'
		COMMIT TRANSACTION
	END
