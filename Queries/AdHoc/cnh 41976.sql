USE CDW
GO

DECLARE @DATA TABLE ( NUMBER INT, SERVICER VARCHAR(200), SSN VARCHAR(9), CEMS_CASE_NUMBER VARCHAR(20), [TYPE] VARCHAR(50), CASE_OPENED_DATE DATE, BORROWER_NOTIFICATION_DATE DATE, DAYS_OF_INTEREST_CREDIT INT)
INSERT INTO @DATA VALUES
(1,'CS','044403210','01478842','Ineligible','02/10/17','05/11/2020',1127),
(2,'CS','085868570','01548362','Ineligible','02/21/19','05/13/2020',386),
(3,'CS','173640309','01252143','Ineligible','07/31/18','05/05/2020',591),
(4,'CS','246492763','01274314','Ineligible','10/05/18','05/06/2020',525),
(5,'CS','254994093','01415699','Ineligible','12/28/17','05/05/2020',806),
(6,'CS','279025450','01250267','Ineligible','07/25/18','05/04/2020',597),
(7,'CS','317170771','01266233','Ineligible','09/12/18','05/13/2020',548),
(8,'CS','355920971','02140610','Ineligible','04/22/20','05/12/2020',0),
(9,'CS','438978786','01386134','Ineligible','12/17/16','05/08/2020',1182),
(10,'CS','505477968','01606566','Ineligible','06/17/19','05/13/2020',270),
(11,'CS','522390715','02094530','Ineligible','01/14/20','05/13/2020',59),
(12,'CS','524972681','01251069','Ineligible','07/26/18','05/06/2020',596),
(13,'CS','528139554','01243553','Ineligible','07/05/18','05/04/2020',617),
(14,'CS','571652152','02053412','Ineligible','12/17/19','05/14/2020',87),
(15,'CS','572836991','01559592','Ineligible','03/11/19','05/14/2020',368),
(16,'CS','576634696','01592237','Ineligible','05/08/19','05/13/2020',310),
(17,'CS','586789954','02004882','Ineligible','10/09/19','05/14/2020',156),
(18,'CS','600852495','02126034','Ineligible','03/12/20','05/07/2020',0),
(19,'CS','601316303','01600128','Ineligible','05/29/19','05/06/2020',289),
(20,'CS','606689279','01497526','Ineligible','06/22/18','05/13/2020',630),
(21,'CS','609962072','01531703','Ineligible','01/17/19','05/06/2020',421),
(22,'CS','613153810','02143816','Ineligible','05/02/20','05/12/2020',0),
(23,'CS','616104507','01298222','Ineligible','07/06/15','05/13/2020',1712)


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