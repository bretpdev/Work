--The script takes people with an open task for counting PREIDR months 
--for the purpose of counting qualifying IDR payments before they transferred
--onto our system

DECLARE @Ssns TABLE(SSN CHAR(9))
INSERT INTO @Ssns
SELECT DISTINCT
	BI.Ssn
FROM
	CLS.pridrcrp.BorrowerInformation BI
	INNER JOIN CLS.pridrcrp.Disbursements D
		ON D.BorrowerInformationId = BI.BorrowerInformationId
	INNER JOIN CDW..FS10_DL_LON FS10
		ON FS10.BF_SSN = BI.Ssn
		AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
			CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
					WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
					WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
			END + D.LoanId
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = FS10.BF_SSN
		AND LN10.LN_SEQ = FS10.LN_SEQ
	INNER JOIN CDW..LN65_LON_RPS LN65
		ON LN65.BF_SSN = LN10.BF_SSN
		AND LN65.LN_SEQ = LN10.LN_SEQ
		AND LN65.LC_STA_LON65 = 'A'
		AND LN65.LC_TYP_SCH_DIS IN ('CA','CP','CQ','C1','C2','C3','IA','IB','IL','IP','I3','I5')
	INNER JOIN CDW..WQ20_TSK_QUE WQ20
		ON WQ20.BF_SSN = LN10.BF_SSN
	LEFT JOIN CDW..WQ20_TSK_QUE WQ20_HL
		ON WQ20_HL.BF_SSN = LN10.BF_SSN
		AND WQ20_HL.WC_STA_WQUE20 NOT IN('X','C')
		AND WQ20_HL.WF_QUE = 'HL'
	LEFT JOIN CLS.pridrcrp.ReviewQueue RQ
		ON RQ.SSN = LN10.BF_SSN
		AND RQ.DeletedAt IS NULL
		AND RQ.DeletedBy IS NULL
WHERE
	((WQ20.WF_QUE = 'BO' AND WQ20.PF_REQ_ACT = 'PMTHR')
	OR WQ20.WF_QUE = 'HN')
	AND WQ20_HL.BF_SSN IS NULL
	AND RQ.SSN IS NULL
	AND WQ20.WC_STA_WQUE20 NOT IN('X','C') --Has open queue
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND BI.DeletedAt IS NULL;

DROP TABLE IF EXISTS tempdb.dbo.#PreQualifyingPayments
CREATE TABLE #PreQualifyingPayments(BF_SSN VARCHAR(9), LN_SEQ SMALLINT, ScheduleCode VARCHAR(10), PaymentsQualifyingLevelPrevious INT, PaymentsQualifyingIDRPrevious INT, PaymentsQualifyingPermanentStandardPrevious INT, PaymentsCoveredByEHDPre INT, Total INT)

DECLARE @SSN CHAR(9) = (SELECT TOP 1 Ssn FROM @Ssns);
WHILE @SSN IS NOT NULL
BEGIN
	INSERT INTO #PreQualifyingPayments
	EXEC CDW..GetQualifyingPreconversionPayments 'ICR', @SSN;

	INSERT INTO #PreQualifyingPayments
	EXEC CDW..GetQualifyingPreconversionPayments 'IBR', @SSN;

	INSERT INTO #PreQualifyingPayments
	EXEC CDW..GetQualifyingPreconversionPayments 'PAYE', @SSN;

	INSERT INTO #PreQualifyingPayments
	EXEC CDW..GetQualifyingPreconversionPayments 'REPAYE', @SSN;

	DELETE FROM @Ssns WHERE Ssn = @SSN;
	SET @SSN = (SELECT TOP 1 Ssn FROM @Ssns);
END

INSERT INTO CLS.pridrcrp.PreQualifyingPayments(BF_SSN, LN_SEQ, ScheduleCode, PaymentsQualifyingLevelPrevious, PaymentsQualifyingIDRPrevious, PaymentsQualifyingPermanentStandardPrevious, PaymentsCoveredByEHDPre, Total)
SELECT
	TPQP.BF_SSN,
	TPQP.LN_SEQ,
	TPQP.ScheduleCode,
	TPQP.PaymentsQualifyingLevelPrevious,
	TPQP.PaymentsQualifyingIDRPrevious,
	TPQP.PaymentsQualifyingPermanentStandardPrevious,
	TPQP.PaymentsCoveredByEHDPre,
	TPQP.Total
FROM
	#PreQualifyingPayments TPQP
	LEFT JOIN CLS.pridrcrp.PreQualifyingPayments PQP
		ON TPQP.BF_SSN = PQP.BF_SSN
		AND TPQP.LN_SEQ = PQP.LN_SEQ
		AND TPQP.ScheduleCode = PQP.ScheduleCode
		AND PQP.DeletedAt IS NULL
WHERE
	PQP.BF_SSN IS NULL