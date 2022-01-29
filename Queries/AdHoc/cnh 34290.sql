SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @LETTERS TABLE (CATEGORY VARCHAR(500), LETTERID VARCHAR(10))
INSERT INTO @LETTERS
VALUES('Delinquency Notifications','LTESGEMFED'),
('Billing/Account Statements','DELEML04'),
('Delinquency Notifications','PLCSLEML02'),
('Delinquency Notifications','TS09B60CP'),
('Delinquency Notifications','DELEML03'),
('Delinquency Notifications','PLCSLEML01'),
('Delinquency Notifications','ACHSOLBR2'),
('Delinquency Notifications','ACHSOLBR3'),
('Delinquency Notifications','ACHSOLBR1'),
('Military Notifications','SCRANEFEDE'),
('Military Notifications','SCRANAFED'),
('Payment-Related Notifications','APBNKFED'),
('Payment-Related Notifications','APDNYFED'),
('Payment-Related Notifications','APAPPR1FED'),
('Payment-Related Notifications','EAUTORECVD'),
('Military Notifications','BKCONFED'),
('Billing/Account Statements','BILSTFED'),
('Other Loan Maintenance','FM06BNGRD'),
('Payment-Related Notifications','TS06BR003'),
('Payment-Related Notifications','CBPCNFFED'),
('Repayment Plan Related Notices','TS06CBRPAY'),
('Delinquency Notifications','US06BMFPE'),
('Repayment Plan Related Notices','CODWLCMEML'),
('Delinquency Notifications','FM06BCPLS'),
('Payment-Related Notifications','CRDCORFED'),
('Delinquency Notifications','FM06DACMP'),
('Loan Postponement Notifications','DFACDFED'),
('Loan Postponement Notifications','DEFEMLFED'),
('Loan Postponement Notifications','DEFDNFED'),
('Loan Postponement Notifications','TS06BDDEFP'),
('Loan Postponement Notifications','DEFAPPREC'),
('Delinquency Notifications','DLQEM05FD'),
('Delinquency Notifications','DLQEM10FD'),
('Delinquency Notifications','DLQEM15FD'),
('Delinquency Notifications','DLQEM20FD'),
('Delinquency Notifications','DLQEM25FD'),
('Delinquency Notifications','DLQEM30FD'),
('Delinquency Notifications','DLQEM35FD'),
('Delinquency Notifications','DLQEM40FD'),
('Delinquency Notifications','DLQEM45FD'),
('Delinquency Notifications','DLQEM50FD'),
('Delinquency Notifications','DLQEM55FD'),
('Delinquency Notifications','DLQEM60FD'),
('Delinquency Notifications','DLQEM65FD'),
('Delinquency Notifications','DLQEM70FD'),
('Delinquency Notifications','DLQEM75FD'),
('Delinquency Notifications','DLQEM80FD'),
('Delinquency Notifications','BRSKIPEML'),
('Forgiveness/Discharge Notifications','LDAFC3FED'),
('Other Loan Maintenance','TS06BLS150'),
('Repayment Plan Related Notices','TS06BRE150'),
('Delinquency Notifications','TS09B60P'),
('Payment-Related Notifications','TS06BDDIDR'),
('Other Loan Maintenance','TS04BDLDSQ'),
('Forgiveness/Discharge Notifications','TS06BDNBNR'),
('Forgiveness/Discharge Notifications','TS06BDNBWR'),
('Forgiveness/Discharge Notifications','TS06BDWBNR'),
('Loan Postponement Notifications','TS06BBAPP'),
('Loan Postponement Notifications','TS06BBCANC'),
('Loan Postponement Notifications','TS06BD6016'),
('Repayment Plan Related Notices','CLA&PNCL'),
('Repayment Plan Related Notices','TS05BCRLTR'),
('Repayment Plan Related Notices','TS06BCNSWL'),
('Payment-Related Notifications','TS06BTCRDF'),
('Loan Postponement Notifications','TS06BD101'),
('Loan Postponement Notifications','TS06BD201'),
('Loan Postponement Notifications','TS06BD501'),
('Delinquency Notifications','TS06BDMP'),
('Other Loan Maintenance','TRANLETFED'),
('Repayment Plan Related Notices','SPCNREQFED'),
('Loan Postponement Notifications','SADFFED'),
('Tax Notifications','TAXINTFED'),
('Tax Notifications','TAXWOFED'),
('Tax Notifications','TAXNOTFFED'),
('Forgiveness/Discharge Notifications','TCHLFFED'),
('Forgiveness/Discharge Notifications','TLFAPFED'),
('Forgiveness/Discharge Notifications','TLFDENFED'),
('Forgiveness/Discharge Notifications','TLFFBFED'),
('Forgiveness/Discharge Notifications','TS06BLNFRG'),
('Account Authorization Notifications','3PACONFFED'),
('Account Authorization Notifications','3PADENFED'),
('Military Notifications','TPDAPVTFED'),
('Forgiveness/Discharge Notifications','TPDAPSPFED'),
('Military Notifications','TPDDNVTFED'),
('Forgiveness/Discharge Notifications','TPDDNSPFED'),
('Other Loan Maintenance','TS06BTRNCL'),
('Other Loan Maintenance','EMLNOADD'),
('Repayment Plan Related Notices','CSWELCOME5'),
('Repayment Plan Related Notices','CSWELCOME2'),
('Repayment Plan Related Notices','CSWELCOMEP'),
('Other Loan Maintenance','TS06BTSS'),
('Billing/Account Statements','TS06BTSA'),
('Payment-Related Notifications','TS06BTM2LM'),
('Payment-Related Notifications','TS06BTM2C'),
('Loan Postponement Notifications','TS06BDDIS'),
('Account Authorization Notifications','TS06BTPERM'),
('Forgiveness/Discharge Notifications','TS08BDISCN'),
('Forgiveness/Discharge Notifications','TS06BTPDAP'),
('Forgiveness/Discharge Notifications','TS08BDSBA7'),
('Forgiveness/Discharge Notifications','TS06BTPDSS'),
('Loan Postponement Notifications','TS06BF101M'),
('Loan Postponement Notifications','TS06BDUNEM'),
('Repayment Plan Related Notices','IDRDN'),
('Repayment Plan Related Notices','IDRPNDD'),
('Repayment Plan Related Notices','IDRPNDO'),
('Repayment Plan Related Notices','IDRPNDS'),
('Delinquency Notifications','IDRSIGN'),
('Repayment Plan Related Notices','ICRDENFED'),
('Repayment Plan Related Notices','IDREMRCVD'),
('Repayment Plan Related Notices','IDRAPRCVD'),
('Repayment Plan Related Notices','ISRREQFED'),
('Payment-Related Notifications','INTBILFED'),
('Billing/Account Statements','INNOSCHFED'),
('Delinquency Notifications','LTEMAILFED'),
('Forgiveness/Discharge Notifications','TS08BSCHAP'),
('Loan Postponement Notifications','TS06BRSMRY'),
('Repayment Plan Related Notices','PIFCLFED'),
('Forgiveness/Discharge Notifications','TS06BPIFG'),
('Other Loan Maintenance','EPHNATTFED'),
('Other Loan Maintenance','PHNATTLFED'),
('Loan Postponement Notifications','NDSTRF'),
('Loan Postponement Notifications','NDSTDF'),
('Other Loan Maintenance','NNTREMLFED'),
('Other Loan Maintenance','OSTREMLFED'),
('Forgiveness/Discharge Notifications','IDTHAPBFED'),
('Forgiveness/Discharge Notifications','IDTHNTBFED'),
('Forgiveness/Discharge Notifications','IDTHAPEFED'),
('Forgiveness/Discharge Notifications','IDTHNTEFED'),
('Forgiveness/Discharge Notifications','IDTHFDBFED'),
('Forgiveness/Discharge Notifications','IDTHFDEFED'),
('Forgiveness/Discharge Notifications','TS06BIDRFP'),
('Forgiveness/Discharge Notifications','TS06BIDRFG'),
('Forgiveness/Discharge Notifications','IDRFRGSTUS'),
('Repayment Plan Related Notices','TS06BIDRPS'),
('Loan Postponement Notifications','TS06BDSCH'),
('Billing/Account Statements','TS06BQRTLY'),
('Payment-Related Notifications','TS06BCAP'),
('Payment-Related Notifications','TS06BRATE'),
('Loan Postponement Notifications','TS06BINTFB'),
('Loan Postponement Notifications','TS06BECNFB'),
('Loan Postponement Notifications','TS06OTLINT'),
('Military Notifications','TS06BDMIL'),
('Payment-Related Notifications','TS06BDDSMT'),
('Other Loan Maintenance','TS06BTNAC'),
('Other Loan Maintenance','TS06BTNACE'),
('Repayment Plan Related Notices','TS06BTRT4'),
('Billing/Account Statements','TS06BNEGAM'),
('Loan Postponement Notifications','TS06BDPLUS'),
('Repayment Plan Related Notices','TS06BCP'),
('Repayment Plan Related Notices','PAYEDENFED'),
('Repayment Plan Related Notices','PMTMONFED'),
('Payment-Related Notifications','PAYOFFFED'),
('Repayment Plan Related Notices','PLRPYMTFED'),
('Payment-Related Notifications','ZBLREFFED'),
('Account Authorization Notifications','POACNFFED'),
('Account Authorization Notifications','POADENFED'),
('Payment-Related Notifications','EMPSNLFPL'),
('Payment-Related Notifications','LPSNLFP'),
('Forgiveness/Discharge Notifications','TS06BPSTFR'),
('Payment-Related Notifications','FINBLNOPFD'),
('Repayment Plan Related Notices','RPAYDENFED'),
('Repayment Plan Related Notices','RPYEPSTD'),
('Repayment Plan Related Notices','RPDISCFED'),
('Forgiveness/Discharge Notifications','DSCSCHFED'),
('Loan Postponement Notifications','SDWVRCNFED'),
('Loan Postponement Notifications','SDEFWVRFED'),
('Other Loan Maintenance','SCHDKPFED'),
('Other Loan Maintenance','SKPEMLFED'),
('Other Loan Maintenance','SKPBORFED'),
('Loan Postponement Notifications','TS06BDFAM'),
('Repayment Plan Related Notices','TS06OAPPYE'),
('Repayment Plan Related Notices','TS06OPYEDN'),
('Repayment Plan Related Notices','TS06BPYED'),
('Repayment Plan Related Notices','TS06BPYE45'),
('Repayment Plan Related Notices','TS06BPYEXP'),
('Payment-Related Notifications','TS11BISF'),
('Payment-Related Notifications','TS06BTPO'),
('Other Loan Maintenance','TS06BPRTCH'),
('Account Authorization Notifications','TS50BAGLB1'),
('Loan Postponement Notifications','TS06BDPUB'),
('Other Loan Maintenance','TS06BREAF'),
('Loan Postponement Notifications','TS06BRHBTR'),
('Repayment Plan Related Notices','TS06APRPYE'),
('Repayment Plan Related Notices','IDRSUBS'),
('Repayment Plan Related Notices','TS06BRPYXP'),
('Repayment Plan Related Notices','TS06BRPY45'),
('Payment-Related Notifications','TS06BR001'),
('Military Notifications','TS06BSCRAE'),
('Military Notifications','TS06BSCRAA'),
('Payment-Related Notifications','TS06BSAPPM'),
('Other Loan Maintenance','TS13B9RVH0'),
('Other Loan Maintenance','TS13B9RVH1'),
('Other Loan Maintenance','TS13B9CSGN'),
('Other Loan Maintenance','TS13B9CSG2'),
('Other Loan Maintenance','TS13B9CSG3'),
('Other Loan Maintenance','TS13B9COS2'),
('Other Loan Maintenance','TS13B9COS3'),
('Other Loan Maintenance','TS13B9PLR3'),
('Forgiveness/Discharge Notifications','LDAFC1FED'),
('Forgiveness/Discharge Notifications','LDAFC2FED'),
('Forgiveness/Discharge Notifications','LDAURFED'),
('Forgiveness/Discharge Notifications','DISEMLFED'),
('Forgiveness/Discharge Notifications','LDASCFED'),
('Forgiveness/Discharge Notifications','DSCRCV4RVW'),
('Forgiveness/Discharge Notifications','LDDEN1FED'),
('Delinquency Notifications','EMDTENFED'),
('Delinquency Notifications','LDMCSTNFED'),
('Delinquency Notifications','DUEDTEML'),
('Delinquency Notifications','US06PYCHNG'),
('Delinquency Notifications','DDECBNAFED'),
('Delinquency Notifications','DDECBAFED'),
('Delinquency Notifications','TS06BDD10'),
('Payment-Related Notifications','EASYPAY'),
('Other Loan Maintenance','ECRWRGPHN'),
('Billing/Account Statements','EBILLFED'),
('Other Loan Maintenance','ECNE2FED'),
('Other Loan Maintenance','ECNE1FED'),
('Other Loan Maintenance','ECNE3FED'),
('Other Loan Maintenance','ELCOSOFED'),
('Repayment Plan Related Notices','EMATRPTFED'),
('Loan Postponement Notifications','FRDEEMFED'),
('Other Loan Maintenance','FAXCPFED'),
('Other Loan Maintenance','FLTREMLFED'),
('Payment-Related Notifications','EFNBLNOPFD'),
('Payment-Related Notifications','TS09BFDCP'),
('Delinquency Notifications','US06BMFP4'),
('Loan Postponement Notifications','FORAPPREC'),
('Loan Postponement Notifications','FOREMLFED'),
('Loan Postponement Notifications','TS06BF201'),
('Loan Postponement Notifications','FORDNFED'),
('Loan Postponement Notifications','TS06BF601'),
('Forgiveness/Discharge Notifications','DISAPPREC'),
('Delinquency Notifications','240DELQFED'),
('Repayment Plan Related Notices','EWLCOMEFED'),
('Loan Postponement Notifications','TS06BGRDFL'),
('Other Loan Maintenance','GLTRNEMFED'),
('Repayment Plan Related Notices','IBRDENFED'),
('Repayment Plan Related Notices','TS06BIBR'),
('Forgiveness/Discharge Notifications','IDTHFT1FED'),
('Loan Postponement Notifications','TS06BMILFB'),
('Delinquency Notifications','TS06BD10DC'),
('Delinquency Notifications','TS06BDD140'),
('Delinquency Notifications','TS06BDD20'),
('Delinquency Notifications','TS06BDD40'),
('Delinquency Notifications','TS06BDD80'),
('Delinquency Notifications','TS06BD80DC'),
('Loan Postponement Notifications','TS06BDHRD'),
('Loan Postponement Notifications','TS06BDEDU'),
('Repayment Plan Related Notices','TS06BIDREC'),
('Payment-Related Notifications','TS06BFNBIL'),
('Payment-Related Notifications','TS09BFDLP'),
('Loan Postponement Notifications','TS06BF101C'),
('Loan Postponement Notifications','TS06BF101'),
('Loan Postponement Notifications','TS06BF101J'),
('Loan Postponement Notifications','TS06BDFRBP'),
('Loan Postponement Notifications','TS06BFORB'),
('Loan Postponement Notifications','TS06BD6015'),
('Loan Postponement Notifications','TS06BD6018'),
('Repayment Plan Related Notices','TS06BIBRXP'),
('Repayment Plan Related Notices','TS06BIBRD'),
('Repayment Plan Related Notices','TS06BIBR45'),
('Repayment Plan Related Notices','TS06BIL'),
('Repayment Plan Related Notices','TS06BICR'),
('Repayment Plan Related Notices','TS06BICRE1'),
('Repayment Plan Related Notices','TS06BICR45'),
('Repayment Plan Related Notices','TS06BAPIDR'),
('Other Loan Maintenance','TS06BSPLIT'),
('Repayment Plan Related Notices','TS50BGLB1'),
('Payment-Related Notifications','RFNDAPVFED'),
('Forgiveness/Discharge Notifications','UPRFDDCHAP'),
('Forgiveness/Discharge Notifications','FCDCHAPFED'),
('Forgiveness/Discharge Notifications',''),
('Forgiveness/Discharge Notifications','DSCSCHFED'),
('Forgiveness/Discharge Notifications',''),
('Forgiveness/Discharge Notifications',''),
('Forgiveness/Discharge Notifications',''),
('Repayment Plan Related Notices','IDRENOINC'),
('Forgiveness/Discharge Notifications',''),
('Other Loan Maintenance',''),
('Forgiveness/Discharge Notifications','CSREQFED'),
('Loan Postponement Notifications','DEFDNFED'),
('Other Loan Maintenance',''),
('Other Loan Maintenance','LDDEN1FED'),
('Other Loan Maintenance','LDDEN1FED'),
('Other Loan Maintenance','DFFBDNFED'),
('Military Notifications','DODDNYFED'),
('Forgiveness/Discharge Notifications','DSCRCV4RVW'),
('Repayment Plan Related Notices','EXPPMTNTC'),
('Repayment Plan Related Notices','FDAPIBRFED'),
('Loan Postponement Notifications','FORDNFED'),
('Forgiveness/Discharge Notifications','LDDEN1FED'),
('Repayment Plan Related Notices','IBRDENFED'),
('Repayment Plan Related Notices','ICRDENFED'),
('Repayment Plan Related Notices','IDRPNDD'),
('Repayment Plan Related Notices','IDRPNDO'),
('Repayment Plan Related Notices','IDRRCDFG'),
('Repayment Plan Related Notices','IDRRNRCV'),
('Repayment Plan Related Notices','ISRDENFED'),
('Forgiveness/Discharge Notifications','LDDEN1FED'),
('Forgiveness/Discharge Notifications','LDSCDENFED'),
('Repayment Plan Related Notices',''),
('Repayment Plan Related Notices','PAYEDENFED'),
('Forgiveness/Discharge Notifications',''),
('Other Loan Maintenance',''),
('Repayment Plan Related Notices','RPAYDENFED'),
('Forgiveness/Discharge Notifications','TLFDENFED'),
('Forgiveness/Discharge Notifications',''),
('Forgiveness/Discharge Notifications','UPRFDDCHAP')

SELECT
	MONTH(CreateDate) AS [MONTH],
	YEAR(CreateDate) AS [YEAR],
	COUNT(DD.DocumentDetailsId) as CATEGORY_COUNT
FROM
	ECorrFed..DocumentDetails DD
	INNER JOIN ECorrFed..Letters L
		ON L.LetterId = DD.LetterId
	INNER JOIN @LETTERS LL
		ON LL.LETTERID = L.Letter
WHERE 
	CorrMethod != 'Printed'
	AND
	CreateDate BETWEEN '01/01/2017' AND '03/31/2018'
	AND 
	DD.Active = 1
GROUP BY
	MONTH(CreateDate),
	YEAR(CreateDate)
ORDER BY
	YEAR(CreateDate),
	MONTH(CreateDate)

SELECT
	MONTH(CreateDate) AS [MONTH],
	YEAR(CreateDate) AS [YEAR],
	COUNT(DD.DocumentDetailsId) + MAX(ISNULL(AC,0))  as CATEGORY_COUNT
FROM
	ECorrFed..DocumentDetails DD
	INNER JOIN ECorrFed..Letters L
		ON L.LetterId = DD.LetterId
	INNER JOIN @LETTERS LL
		ON LL.LETTERID = L.Letter
	LEFT JOIN
	(
		SELECT	
			MONTH(LD_ATY_REQ_RCV) AS [MONTH],
			YEAR(LD_ATY_REQ_RCV) AS [YEAR],
			COUNT(*) AS AC
		FROM
			CDW..AY10_BR_LON_ATY
		WHERE
			PF_REQ_ACT IN ('R3RFX','RADEN','ADUPR')
			AND
			LD_ATY_REQ_RCV BETWEEN '01/01/2017' AND '03/31/2018'
		GROUP BY
			MONTH(LD_ATY_REQ_RCV),
			YEAR(LD_ATY_REQ_RCV)
	) ARC
		ON ARC.[MONTH] = MONTH(CreateDate)
		AND ARC.[YEAR] = YEAR(CreateDate)
WHERE 
	CorrMethod = 'Printed'
	AND
	CreateDate BETWEEN '01/01/2017' AND '03/31/2018'
	AND 
	DD.Active = 1
GROUP BY
	MONTH(CreateDate),
	YEAR(CreateDate)
ORDER BY
	YEAR(CreateDate),
	MONTH(CreateDate)



SELECT
	MONTH(CreateDate) AS [MONTH],
	YEAR(CreateDate) AS [YEAR],
	LL.CATEGORY,
	COUNT(DD.DocumentDetailsId) + MAX(ISNULL(AC,0)) as CATEGORY_COUNT
FROM
	ECorrFed..DocumentDetails DD
	INNER JOIN ECorrFed..Letters L
		ON L.LetterId = DD.LetterId
	INNER JOIN @LETTERS LL
		ON LL.LETTERID = L.Letter
	LEFT JOIN
	(
		SELECT	
			MONTH(LD_ATY_REQ_RCV) AS [MONTH],
			YEAR(LD_ATY_REQ_RCV) AS [YEAR],
			CASE 
				WHEN PF_REQ_ACT = 'ADUPR' THEN 'Forgiveness/Discharge Notifications'
				WHEN PF_REQ_ACT = 'R3RFX' THEN 'Payment-Related Notifications'
				WHEN PF_REQ_ACT = 'RADEN' THEN 'Other Loan Maintenance'
			END AS CATEGORY,
			COUNT(*) AS AC
		FROM
			CDW..AY10_BR_LON_ATY
		WHERE
			PF_REQ_ACT IN ('R3RFX','RADEN','ADUPR')
			AND
			LD_ATY_REQ_RCV BETWEEN '01/01/2017' AND '03/31/2018'
		GROUP BY
			MONTH(LD_ATY_REQ_RCV),
			YEAR(LD_ATY_REQ_RCV),
			CASE 
				WHEN PF_REQ_ACT = 'ADUPR' THEN 'Forgiveness/Discharge Notifications'
				WHEN PF_REQ_ACT = 'R3RFX' THEN 'Payment-Related Notifications'
				WHEN PF_REQ_ACT = 'RADEN' THEN 'Other Loan Maintenance'
			END 
	) ARC
		ON ARC.[MONTH] = MONTH(CreateDate)
		AND ARC.[YEAR] = YEAR(CreateDate)
		AND ARC.CATEGORY = LL.CATEGORY
WHERE 
	CorrMethod = 'Printed'
	AND
	CreateDate BETWEEN '01/01/2017' AND '03/31/2018'
	AND 
	DD.Active = 1
GROUP BY
	MONTH(CreateDate),
	YEAR(CreateDate),
	LL.CATEGORY
ORDER BY
	YEAR(CreateDate),
	MONTH(CreateDate),
	CATEGORY

