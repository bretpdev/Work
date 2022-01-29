USE CDW
GO

DECLARE @DATA TABLE ( NUMBER INT, SERVICER VARCHAR(200), SSN VARCHAR(9), CEMS_CASE_NUMBER VARCHAR(20), [TYPE] VARCHAR(50), CASE_OPENED_DATE DATE, BORROWER_NOTIFICATION_DATE DATE, DAYS_OF_INTEREST_CREDIT INT)
INSERT INTO @DATA VALUES
(1,'CS','004844090','01482076','Ineligible','2/26/2017','7/8/2020',1111),
(2,'CS','059742741','02005885','Ineligible','10/10/2019','7/7/2020',155),
(3,'CS','074803324','01546161','Ineligible','2/17/2019','7/9/2020',390),
(4,'CS','095025578','02051449','Ineligible','12/17/2019','7/8/2020',87),
(5,'CS','158703187','01518910','Ineligible','12/17/2018','7/6/2020',452),
(6,'CS','171620547','02118027','Ineligible','2/27/2020','7/8/2020',15),
(7,'CS','177762098','01551668','Ineligible','2/26/2019','7/6/2020',381),
(8,'CS','233319854','02108279','Ineligible','2/9/2020','7/7/2020',33),
(9,'CS','246815046','01573785','Ineligible','4/4/2019','7/6/2020',344),
(10,'CS','249450831','02085899','Ineligible','12/28/2019','7/14/2020',76),
(11,'CS','259651118','02103794','Ineligible','1/31/2020','7/9/2020',42),
(12,'CS','260983828','01476025','Ineligible','10/10/2017','7/6/2020',885),
(13,'CS','272804731','01439627','Ineligible','6/4/2018','7/13/2020',648),
(14,'CS','319780593','01709907','Ineligible','7/9/2019','7/13/2020',248),
(15,'CS','323707169','02041297','Ineligible','12/9/2019','7/14/2020',95),
(16,'CS','329546233','01387486','Ineligible','6/28/2017','7/2/2020',989),
(17,'CS','339943650','02338016','Ineligible','6/30/2020','7/13/2020',0),
(18,'CS','340883369','02040400','Ineligible','12/5/2019','7/1/2020',99),
(19,'CS','419451473','01403385','Ineligible','8/23/2017','7/7/2020',933),
(20,'CS','422415212','02323669','Ineligible','6/23/2020','7/13/2020',0),
(21,'CS','422472675','02107726','Ineligible','2/7/2020','7/13/2020',35),
(22,'CS','426536142','02143886','Ineligible','5/4/2020','7/6/2020',0),
(23,'CS','435190495','02258946','Ineligible','5/20/2020','7/8/2020',0),
(24,'CS','454779299','02042400','Ineligible','12/11/2019','7/6/2020',93),
(25,'CS','455672220','01365757','Ineligible','1/21/2017','7/9/2020',1147),
(26,'CS','459614132','02246701','Ineligible','5/14/2020','7/7/2020',0),
(27,'CS','462919292','01476102','Ineligible','10/10/2017','7/1/2020',885),
(28,'CS','467759812','01601927','Ineligible','6/3/2019','7/1/2020',284),
(29,'CS','486765538','01530064','Ineligible','1/15/2019','7/6/2020',423),
(30,'CS','502137401','01277930','Ineligible','10/16/2018','7/13/2020',514),
(31,'CS','521757993','01534152','Ineligible','1/23/2019','7/6/2020',415),
(32,'CS','524556688','01539354','Ineligible','2/2/2019','7/9/2020',405),
(33,'CS','534270934','01282492','Ineligible','10/28/2018','7/14/2020',502),
(34,'CS','540177097','01248224','Ineligible','7/18/2018','7/13/2020',604),
(35,'CS','548592748','01874396','Ineligible','8/16/2019','7/10/2020',210),
(36,'CS','549975706','02053737','Ineligible','12/17/2019','7/8/2020',87),
(37,'CS','550632594','01365750','Ineligible','2/28/2017','7/7/2020',1109),
(38,'CS','555231131','02128706','Ineligible','3/18/2020','7/6/2020',0),
(39,'CS','555618569','02048727','Ineligible','12/12/2019','7/2/2020',92),
(40,'CS','568156711','01997587','Ineligible','10/1/2019','7/15/2020',164),
(41,'CS','591765579','01289255','Ineligible','11/16/2018','7/6/2020',483),
(42,'CS','592632494','01567955','Ineligible','3/26/2019','7/9/2020',353),
(43,'CS','594062959','02087194','Ineligible','12/31/2019','7/7/2020',73),
(44,'CS','594704418','01359922','Ineligible','1/5/2017','7/2/2020',1163),
(45,'CS','601187718','02103879','Ineligible','1/31/2020','7/10/2020',42),
(46,'CS','609268796','02083252','Ineligible','12/21/2019','7/13/2020',83),
(47,'CS','615276656','02325682','Ineligible','6/23/2020','7/7/2020',0),
(48,'CS','617540063','02106580','Ineligible','2/5/2020','7/14/2020',37),
(49,'CS','622286190','02346103','Ineligible','7/4/2020','7/13/2020',0),
(50,'CS','626172701','01461796','Ineligible','1/30/2017','7/15/2020',1138),
(51,'CS','638522639','01253803','Ineligible','8/6/2018','7/13/2020',585)






SELECT DISTINCT
	D.*,
	'',
	LN72.LN_SEQ,
	LN72.LR_INT_RDC_PGM_ORG as LR_ITR,
	LN10.LA_CUR_PRI,
	--'' AS ITS2R,
	CAST(ROUND(((LN72.LR_INT_RDC_PGM_ORG / 365) * LN10.LA_CUR_PRI / 100) * D.DAYS_OF_INTEREST_CREDIT, 2,1) AS DECIMAL(12,2))  INT_CALCULATOR,
	FS10.LF_FED_AWD + RIGHT('000' + CAST(LN_FED_AWD_SEQ AS VARCHAR(3)), 3) AS AWARD_ID
	--'' AS TOTAL_INT_TO_ADJ_ITS2R
	--CASE WHEN LN90.BF_SSN IS NOT NULL THEN 'Y' ELSE 'N' END AS MADE_PAYMENT
FROM
	CDW..LN72_INT_RTE_HST LN72
	INNER JOIN @DATA D
		ON D.SSN = LN72.BF_SSN
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = LN72.BF_SSN
		AND LN10.LN_SEQ = LN72.LN_SEQ
	INNER JOIN CDW..FS10_DL_LON FS10
		ON FS10.BF_SSN = LN10.BF_SSN
		AND FS10.LN_SEQ = LN10.LN_SEQ
	--LEFT JOIN CDW..LN90_FIN_ATY LN90
	--	ON LN90.BF_SSN = LN10.BF_SSN
	--	AND LN90.LN_SEQ = LN10.LN_SEQ
	--	AND LN90.LD_FAT_EFF >= D.CASE_OPENED_DATE
	--	AND LN90.LC_STA_LON90 = 'A'
	--	AND ISNULL(LN90.LC_FAT_REV_REA,'') = ''
	--	AND LN90.PC_FAT_TYP = '10'
WHERE
	LN72.LC_STA_LON72 = 'A'
	AND CAST(GETDATE() AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LD_ITR_EFF_END
ORDER BY
	d.NUMBER,
	LN72.LN_SEQ