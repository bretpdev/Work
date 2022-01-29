USE CDW
GO

DECLARE @DATA TABLE ( NUMBER INT, SERVICER VARCHAR(200), SSN VARCHAR(9), CEMS_CASE_NUMBER VARCHAR(20), [TYPE] VARCHAR(50), CASE_OPENED_DATE DATE, BORROWER_NOTIFICATION_DATE DATE, DAYS_OF_INTEREST_CREDIT INT)
INSERT INTO @DATA VALUES
(1,'CS','035588948','02295533','Ineligible','6/8/2020','7/28/2020',0),
(2,'CS','101681864','01437735','Ineligible','5/25/2018','7/21/2020',658),
(3,'CS','107745525','02103609','Ineligible','1/31/2020','7/30/2020',42),
(4,'CS','131846447','01952669','Ineligible','9/16/2019','7/23/2020',179),
(5,'CS','135868406','02028299','Ineligible','11/13/2019','7/30/2020',121),
(6,'CS','138748007','01275087','Ineligible','10/9/2018','7/20/2020',521),
(7,'CS','156024725','02029809','Ineligible','11/17/2019','7/20/2020',117),
(8,'CS','208609213','01546688','Ineligible','2/19/2019','7/24/2020',388),
(9,'CS','223590158','01525643','Ineligible','1/4/2019','7/17/2020',434),
(10,'CS','223856935','02352845','Ineligible','7/8/2020','7/31/2020',0),
(11,'CS','224773504','01542315','Ineligible','2/8/2019','7/29/2020',399),
(12,'CS','243671493','02147480','Ineligible','5/8/2020','7/27/2020',0),
(13,'CS','247634332','01517499','Ineligible','12/14/2018','7/27/2020',455),
(14,'CS','247656363','01273882','Ineligible','10/4/2018','7/27/2020',526),
(15,'CS','248956060','02346318','Ineligible','7/6/2020','7/31/2020',0),
(16,'CS','250894666','02013177','Ineligible','10/22/2019','7/16/2020',143),
(17,'CS','289646390','02127492','Ineligible','3/16/2020','7/28/2020',0),
(18,'CS','290789153','02050800','Ineligible','12/16/2019','7/30/2020',88),
(19,'CS','290823702','01559116','Ineligible','3/11/2019','7/23/2020',368),
(20,'CS','313868097','02015795','Ineligible','10/29/2019','7/16/2020',136),
(21,'CS','316866515','01555647','Ineligible','3/5/2019','7/20/2020',374),
(22,'CS','332768635','02126422','Ineligible','3/13/2020','7/28/2020',0),
(23,'CS','341943045','01953671','Ineligible','9/16/2019','7/27/2020',179),
(24,'CS','368257773','02361563','Ineligible','7/10/2020','7/30/2020',0),
(25,'CS','407377267','01544497','Ineligible','2/13/2019','7/20/2020',394),
(26,'CS','410858039','01516240','Ineligible','12/12/2018','7/28/2020',457),
(27,'CS','429536337','02138185','Ineligible','4/14/2020','7/27/2020',0),
(28,'CS','438557848','02345986','Ineligible','7/4/2020','7/28/2020',0),
(29,'CS','445769831','02313752','Ineligible','6/17/2020','7/28/2020',0),
(30,'CS','446948197','01452628','Ineligible','1/22/2016','7/21/2020',1512),
(31,'CS','465599164','01467098','Ineligible','1/17/2017','7/31/2020',1151),
(32,'CS','523554395','01581136','Ineligible','4/16/2019','7/29/2020',332),
(33,'CS','529292587','01414970','Ineligible','3/14/2018','7/20/2020',730),
(34,'CS','532256588','01583407','Ineligible','4/19/2019','7/20/2020',329),
(35,'CS','536900731','01502029','Ineligible','6/19/2018','7/27/2020',633),
(36,'CS','567622452','01261886','Ineligible','8/29/2018','7/23/2020',562),
(37,'CS','571734381','02035491','Ineligible','11/25/2019','7/20/2020',109),
(38,'CS','571793066','02308060','Ineligible','6/15/2020','7/27/2020',0),
(39,'CS','581738693','01387438','Ineligible','7/7/2017','7/31/2020',980),
(40,'CS','591033458','01882436','Ineligible','8/19/2019','7/29/2020',207),
(41,'CS','592822805','02103342','Ineligible','1/30/2020','7/16/2020',43),
(42,'CS','593632616','02326130','Ineligible','6/24/2020','7/21/2020',0),
(43,'CS','594519876','01425444','Ineligible','3/14/2018','7/21/2020',730),
(44,'CS','595453149','01296407','Ineligible','12/6/2018','7/27/2020',463),
(45,'CS','603239704','01430484','Ineligible','8/28/2017','7/23/2020',928),
(46,'CS','612642661','01850711','Ineligible','8/12/2019','7/23/2020',214),
(47,'CS','616513301','02103341','Ineligible','1/30/2020','7/23/2020',43),
(48,'CS','641093916','01436625','Ineligible','2/11/2018','7/20/2020',761),
(49,'CS','641529701','02049076','Ineligible','12/12/2019','7/29/2020',92),
(50,'CS','667053318','01526916','Ineligible','1/7/2019','7/24/2020',431),
(51,'CS','675015943','02311246','Ineligible','6/16/2020','7/23/2020',0),
(52,'CS','772528313','02116118','Ineligible','2/25/2020','7/16/2020',17)


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