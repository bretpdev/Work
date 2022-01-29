SELECT * FROM CLS.scra.Data_CR3666
	
SELECT DISTINCT 
	Bor.BorrowerAccountNumber,
	AD.BeginDate,
	AD.EndDate,
	AD.NotificationDate
FROM
	CLS.scra.Borrowers_CR3666 Bor
	INNER JOIN CLS.scra.ActiveDuty_CR3666 AD 
		ON AD.BorrowerId = Bor.BorrowerId
ORDER BY
	Bor.BorrowerAccountNumber,
	AD.NotificationDate,
	AD.BeginDate
	
--Category 3
SELECT DISTINCT
	Bor.BorrowerAccountNumber,
	AD.BeginDate,
	AD.EndDate,
	MAX(CASE WHEN AD.NotificationDate = '2013-07-01' THEN 1 ELSE 0 END) AS Match2013,
	MAX(CASE WHEN AD.NotificationDate = '2012-07-01' THEN 1 ELSE 0 END) AS Match2012,
	MAX(CASE WHEN AD.NotificationDate = '2011-07-01' THEN 1 ELSE 0 END) AS Match2011,
	MAX(CASE WHEN AD.NotificationDate = '2010-07-01' THEN 1 ELSE 0 END) AS Match2010,
	MAX(CASE WHEN AD.NotificationDate = '2009-07-01' THEN 1 ELSE 0 END) AS Match2009
FROM 
	CLS.scra.Borrowers_CR3666 Bor 
	INNER JOIN CLS.scra.ActiveDuty_CR3666 AD 
		ON AD.BorrowerId = Bor.BorrowerId
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = Bor.BorrowerAccountNumber
	INNER JOIN CDW..LN72_INT_RTE_HST LN72
		ON LN72.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN CDW..LN72_INT_RTE_HST LN72MP
		ON LN72MP.BF_SSN = PD10.DF_PRS_ID
		AND LN72MP.LC_INT_RDC_PGM IN ('M','P')
		AND LN72MP.LC_STA_LON72 = 'A'
WHERE
	LN72.LD_ITR_EFF_BEG >= '2008-08-14'
	AND LN72.LR_ITR > 6 
	AND LN72.LC_STA_LON72 = 'A'
	AND LN72MP.BF_SSN IS NULL
GROUP BY
	Bor.BorrowerAccountNumber,
	AD.BeginDate,
	AD.EndDate
ORDER BY
	Bor.BorrowerAccountNumber,
	AD.BeginDate

--Category 4
SELECT DISTINCT
	Bor.BorrowerAccountNumber,
	AD.BeginDate,
	AD.EndDate,
	MAX(CASE WHEN AD.NotificationDate = '2013-07-01' THEN 1 ELSE 0 END) AS Match2013,
	MAX(CASE WHEN AD.NotificationDate = '2012-07-01' THEN 1 ELSE 0 END) AS Match2012,
	MAX(CASE WHEN AD.NotificationDate = '2011-07-01' THEN 1 ELSE 0 END) AS Match2011,
	MAX(CASE WHEN AD.NotificationDate = '2010-07-01' THEN 1 ELSE 0 END) AS Match2010,
	MAX(CASE WHEN AD.NotificationDate = '2009-07-01' THEN 1 ELSE 0 END) AS Match2009
FROM 
	CLS.scra.Borrowers_CR3666 Bor 
	INNER JOIN CLS.scra.ActiveDuty_CR3666 AD 
		ON AD.BorrowerId = Bor.BorrowerId
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = Bor.BorrowerAccountNumber
	INNER JOIN CDW..LN72_INT_RTE_HST LN72
		ON LN72.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN 
	(
		SELECT DISTINCT
			Bor.BorrowerAccountNumber
		FROM 
			CLS.scra.Borrowers_CR3666 Bor 
			INNER JOIN CLS.scra.ActiveDuty_CR3666 AD 
				ON AD.BorrowerId = Bor.BorrowerId
			INNER JOIN CDW..PD10_PRS_NME PD10
				ON PD10.DF_SPE_ACC_ID = Bor.BorrowerAccountNumber
			INNER JOIN CDW..LN72_INT_RTE_HST LN72
				ON LN72.BF_SSN = PD10.DF_PRS_ID
		WHERE
			CASE WHEN AD.BeginDate < '2008-08-14' THEN '2008-08-14' ELSE AD.BeginDate END = LN72.LD_ITR_EFF_BEG 
			AND
			(
				AD.EndDate = LN72.LD_ITR_EFF_END
				OR AD.EndDate = '2099-12-31'
			)
			AND	LN72.LD_ITR_EFF_BEG >= '2008-08-14'
			AND LN72.LR_ITR <= 6 
			AND LN72.LC_INT_RDC_PGM IN ('M','P')
			AND LN72.LC_STA_LON72 = 'A'
	) CATEGORY5 --Exclude category 5 borrowers from this group
		ON CATEGORY5.BorrowerAccountNumber = Bor.BorrowerAccountNumber
WHERE
	(
		CASE WHEN AD.BeginDate < '2008-08-14' THEN '2008-08-14' ELSE AD.BeginDate END != LN72.LD_ITR_EFF_BEG
		OR 
		(
			AD.EndDate != LN72.LD_ITR_EFF_END
			AND AD.EndDate != '2099-12-31'
		)
	)
	AND	LN72.LD_ITR_EFF_BEG >= '2008-08-14'
	AND LN72.LR_ITR <= 6 
	AND LN72.LC_INT_RDC_PGM IN ('M','P')
	AND LN72.LC_STA_LON72 = 'A'
	AND CATEGORY5.BorrowerAccountNumber IS NULL
GROUP BY
	Bor.BorrowerAccountNumber,
	AD.BeginDate,
	AD.EndDate
ORDER BY
	Bor.BorrowerAccountNumber,
	AD.BeginDate

--Category 5
SELECT DISTINCT
	Bor.BorrowerAccountNumber,
	AD.BeginDate,
	AD.EndDate,
	MAX(CASE WHEN AD.NotificationDate = '2013-07-01' THEN 1 ELSE 0 END) AS Match2013,
	MAX(CASE WHEN AD.NotificationDate = '2012-07-01' THEN 1 ELSE 0 END) AS Match2012,
	MAX(CASE WHEN AD.NotificationDate = '2011-07-01' THEN 1 ELSE 0 END) AS Match2011,
	MAX(CASE WHEN AD.NotificationDate = '2010-07-01' THEN 1 ELSE 0 END) AS Match2010,
	MAX(CASE WHEN AD.NotificationDate = '2009-07-01' THEN 1 ELSE 0 END) AS Match2009
FROM 
	CLS.scra.Borrowers_CR3666 Bor 
	INNER JOIN CLS.scra.ActiveDuty_CR3666 AD 
		ON AD.BorrowerId = Bor.BorrowerId
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = Bor.BorrowerAccountNumber
	INNER JOIN CDW..LN72_INT_RTE_HST LN72
		ON LN72.BF_SSN = PD10.DF_PRS_ID
WHERE
	CASE WHEN AD.BeginDate < '2008-08-14' THEN '2008-08-14' ELSE AD.BeginDate END = LN72.LD_ITR_EFF_BEG 
	AND
	(
		AD.EndDate = LN72.LD_ITR_EFF_END
		OR AD.EndDate = '2099-12-31'
	)
	AND	LN72.LD_ITR_EFF_BEG >= '2008-08-14'
	AND LN72.LR_ITR <= 6 
	AND LN72.LC_INT_RDC_PGM IN ('M','P')
	AND LN72.LC_STA_LON72 = 'A'
GROUP BY
	Bor.BorrowerAccountNumber,
	AD.BeginDate,
	AD.EndDate
ORDER BY
	Bor.BorrowerAccountNumber,
	AD.BeginDate

--Category 6
SELECT DISTINCT 
	Bor.BorrowerAccountNumber,
	A.BeginDate,
	A.EndDate,
	MAX(CASE WHEN AD.NotificationDate = '2013-07-01' THEN 1 ELSE 0 END) AS Match2013,
	MAX(CASE WHEN AD.NotificationDate = '2012-07-01' THEN 1 ELSE 0 END) AS Match2012,
	MAX(CASE WHEN AD.NotificationDate = '2011-07-01' THEN 1 ELSE 0 END) AS Match2011,
	MAX(CASE WHEN AD.NotificationDate = '2010-07-01' THEN 1 ELSE 0 END) AS Match2010,
	MAX(CASE WHEN AD.NotificationDate = '2009-07-01' THEN 1 ELSE 0 END) AS Match2009
FROM 
	CLS.scra.Borrowers_CR3666 Bor 
	INNER JOIN CLS.scra.ActiveDuty_CR3666 AD 
		ON AD.BorrowerId = Bor.BorrowerId
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = Bor.BorrowerAccountNumber
	LEFT OUTER JOIN --exclude accounts that have any active interest rate history record where the interest rate is greater than 6%
	(
		SELECT
			BF_SSN,
			LR_ITR,
			LD_ITR_EFF_BEG,
			LD_ITR_EFF_END
		FROM
			CDW..LN72_INT_RTE_HST
		WHERE
			LD_ITR_EFF_BEG >= '2008-08-14'
			AND LR_ITR > 6
			AND LC_STA_LON72 = 'A'
	) LN72
		ON LN72.BF_SSN = PD10.DF_PRS_ID
WHERE
	LN72.BF_SSN IS NULL
GROUP BY
	Bor.BorrowerAccountNumber,
	AD.BeginDate,
	AD.EndDate
ORDER BY
	Bor.BorrowerAccountNumber,
	AD.BeginDate



/*Start of Arc Portion.  Based on 3666 phase 2 arc accounts.txt */
--INSERT INTO CLS..ArcAddProcessing(ArcTypeId, ArcResponseCodeId, AccountNumber, RecipientId, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessFrom, ProcessTo, NeededBy, RegardsTo, RegardsCode, CreatedAt, CreatedBy, ProcessedAt)
SELECT
	2,
	PD10.DF_SPE_ACC_ID,
	NULL,
	'HSCRA',
	'SCRAC3666',
	GETDATE(),
	'Historical SCRA benefit.',
	0,
	0,
	NULL,
	NULL,
	NULL,
	NULL,
	NULL,
	GETDATE(),
	'SCRAC3666',
	NULL
FROM
	CDW.dbo.PD10_PRS_NME PD10
WHERE 
	PD10.DF_SPE_ACC_ID IN('0025866867','0062087079','0069137920','0071085704','0094374868','0100620552','0111018390','0132746778','0189381750','0201161170','0241257064','0243990188','0294783930','0314707369','0358819664','0386312186','0433041193','0500633610','0509408142','0557983064','0596770684','0615259227','0619792571','0691471596','0700238312','0707226552','0732743475','0754929126','0771123874','0807720613','0839172708','0862902404','0881652451','0884904512','0885121099','0897955219','0941353339','0963412154','1009374473','1024435246','1048384745','1065763116','1100233916','1150281592','1156897247','1179260257','1183197541','1196014205','1224930929','1243974169','1307253351','1328097829','1370665182','1407243897','1485459945','1504948185','1615310009','1649021419','1653810372','1712246325','1737220386','1764583684','1782939767','1837694782','2037841753','2075572151','2082580273','2083705431','2126473508','2210500142','2236831520','2284981163','2300170854','2374630372','2426606584','2547254225','2560013239','2582847100','2623001925','2660377308','2732695471','2809304078','2890001350','2927890862','2946816890','3042419718','3072394215','3115560283','3164248824','3184747276','3189076089','3233103778','3271531921','3284942197','3365419223','3387318693','3414166233','3428660270','3482748530','3484867023','3512444087','3546935465','3582273807','3591382469','3628740340','3647394483','3655750126','3674477209','3702522962','3704067175','3731381417','3738965737','3748080200','3767203952','3888520541','3906215195','3939174319','3958673495','3996670087','4010502963','4012470789','4048618492','4077383519','4079924450','4080667734','4107722435','4121331901','4146891401','4180028647','4237550464','4283353555','4294506184','4318307949','4322840482','4362104531','4369734758','4471005914','4477796415','4479111348','4481764558','4516159545','4543084255','4546780193','4577321844','4624436230','4637313686','4723361391','4811071901','4834672504','4836787217','4848239573','4907602451','4931988053','4940231647','4949143707','4952257339','4963081481','4975603411','5036188833','5061869533','5069230397','5074908673','5129660646','5157066727','5183898610','5301780897','5320540727','5352962836','5444885915','5454059905','5553711030','5557498799','5560357882','5638210204','5699137224','5706720140','5716704782','5774475617','5779352204','5857108492','5889616668','5969230628','5986719971','5992240227','6029865917','6037670005','6041651723','6058104720','6064081431','6076431165','6088373712','6105339888','6116606186','6164453563','6177046532','6300024607','6338772638','6353175836','6407902395','6416994493','6420666857','6460496699','6465483087','6477128724','6478357047','6548500999','6570149277','6599651846','6640776535','6653911400','6679604005','6698312455','6708807276','6721236136','6724623970','6742338763','6748833742','6771076955','6807870848','6857276861','6858400920','6890755327','6899637947','6911321406','6930612672','7057283608','7150936381','7238039406','7249848191','7368668506','7433734465','7446523435','7471637267','7505607589','7564399481','7661226199','7681988499','7707853494','7768966221','7791299533','7796787648','7822405680','7844649653','7858721789','7859940167','7910383750','7942100461','8023896877','8066430986','8089331247','8092353930','8135284478','8269079704','8280507534','8290003435','8295245557','8325857150','8364733836','8412504750','8429607224','8475246652','8507548425','8514412255','8539787071','8556132843','8578553945','8583050364','8626503667','8654041089','8671465525','8680003865','8734141220','8741202070','8741223208','8787301411','8800234129','8821440955','8822400700','8863323361','8871005225','8939763507','8953915024','8957105383','8965311858','8978954853','8982244365','8986342599','9030670132','9084267726','9094123630','9117717702','9160016130','9161089648','9175409017','9183472077','9197861860','9254924873','9278905884','9352822832','9422814361','9432029129','9441930093','9472929475','9527139891','9531960833','9595898871','9608849989','9633067599','9637791753','9663658984','9682761112','9684658551','9750974904','9771690903','9800894352','9808996986','9842606549','9907747540','9925255305','9926756940','9927437375','9949417471','9952574526','9979034792','9989406906')

/*Start of Detail Portion.  Based on 3666 phase 2 arc accounts.txt */
SELECT DISTINCT
	OQ.*--,
	--SCRA.*,
	--LN72.*,
	--DATEDIFF(DAY, SCRA.BeginDate, SCRA.EndDate) AS DaysInterest,
	--ROUND((LN72.LR_ITR - 6) / 365.25, 8) AS DailyRate,
	--DATEDIFF(DAY, SCRA.BeginDate, SCRA.EndDate) * ROUND((LN72.LR_ITR - 6) / 365.25, 8) * OQ.AssignedPrincipal AS TotalInterestAdjustment
FROM OPENQUERY
	(LEGEND,
	'
		SELECT
			PD10.DF_SPE_ACC_ID,
			LN10.BF_SSN,
			LN10.LN_SEQ,
			PD10.DM_PRS_LST,
			PD10.DM_PRS_1,
			LN20.LC_EDS_TYP,
			CASE WHEN LN20.LF_EDS != LN10.BF_SSN OR LN20.LC_EDS_TYP IS NULL THEN ''B''
				 WHEN LN20.LF_EDS = LN10.BF_SSN AND LN20.LC_EDS_TYP = ''M'' THEN ''C''
				 WHEN LN20.LF_EDS = LN10.BF_SSN AND LN20.LC_EDS_TYP = ''S'' THEN ''E''
			ELSE ''U'' END AS BorrowerType,
			FS10.LF_FED_AWD||FS10.LN_FED_AWD_SEQ AS LA,
			CASE WHEN LN10.IC_LON_PGM IN(''DLSTFD'',''DLSCT'') THEN ''D1'' 
				 WHEN LN10.IC_LON_PGM IN(''DLUNST'',''DLSCUN'') THEN ''D2'' 
				 WHEN LN10.IC_LON_PGM =''DLSCPG'' THEN ''D3'' 
				 WHEN LN10.IC_LON_PGM IN(''DLPLUS'',''DLSCPL'') THEN ''D4'' 
				 WHEN LN10.IC_LON_PGM IN(''DLUCNS'',''DLSCUC'',''DLSCCN'',''CNSLDN'') THEN ''D5'' 
				 WHEN LN10.IC_LON_PGM IN(''DLSCNS'',''DLSCSC'') THEN ''D6'' 
				 WHEN LN10.IC_LON_PGM =''DLPCNS'' THEN ''D7'' 
				 WHEN LN10.IC_LON_PGM =''TEACH'' THEN ''D8'' 
				 WHEN LN10.IC_LON_PGM =''FISL'' THEN ''FI'' 
				 WHEN LN10.IC_LON_PGM =''SLS'' THEN ''SL''
			ELSE '''' END AS LoanType,
			LN10.LA_CUR_PRI,
			CASE WHEN DW01.WC_DW_LON_STA =''01'' THEN ''IG''
				 WHEN DW01.WC_DW_LON_STA =''02'' THEN ''ID''
				 WHEN DW01.WC_DW_LON_STA =''03'' THEN ''RP''
				 WHEN DW01.WC_DW_LON_STA =''04'' THEN ''DA''
				 WHEN DW01.WC_DW_LON_STA =''05'' THEN ''FB''
				 WHEN DW01.WC_DW_LON_STA IN(''07'',''08'',''13'',''14'') THEN ''DF''
				 WHEN DW01.WC_DW_LON_STA IN(''16'',''17'') THEN ''DE''
				 WHEN DW01.WC_DW_LON_STA IN(''18'',''19'') THEN ''DI''
				 WHEN DW01.WC_DW_LON_STA IN(''20'',''21'') THEN ''BK''
				 WHEN DW01.WC_DW_LON_STA =''22'' THEN ''PF''
			ELSE ''RP'' END AS LoanStatus,
			LN90.LA_FAT_CUR_PRI AS AssignedPrincipal,
			CASE WHEN PD10.DF_SPE_ACC_ID IN (''0314707369'',''0386312186'',''0500633610'',''0509408142'',''0557983064'',''0619792571'',''0732743475'',''0862902404'',''0941353339'',''0980613850'',''1009374473'',''1048384745'',''2210500142'',''2890001350'',''3164248824'',''3271531921'',''3484867023'',''3546935465'',''3704067175'',''3731381417'',''3748080200'',''4079924450'',''4146891401'',''4180028647'',''4836787217'',''4963081481'',''5301780897'',''5444885915'',''5638210204'',''5889616668'',''6177046532'',''6407902395'',''6640776535'',''6742338763'',''6748833742'',''6899637947'',''7057283608'',''7471637267'',''7661226199'',''7892916159'',''8023896877'',''8429607224'',''8787301411'',''8863323361'',''8939763507'',''9160016130'',''9183472077'',''9254924873'',''9352822832'',''9472929475'',''9608849989'',''9952574526'',''9979034792'',''8978954853'',''3996670087'',''8412504750'',''8135284478'',''4952257339'',''4121331901'',''4107722435'',''3482748530'',''2374630372'',''5706720140'',''4931988053'',''0839172708'',''5183898610'',''0361416031'',''1764583684'',''2732695471'',''5129660646'',''5699137224'',''6465483087'',''6698312455'',''8269079704'') THEN ''YES'' 
			ELSE ''NO'' END AS AdditionalAdjustment,
			LN10.LD_PIF_RPT
		FROM
			PKUB.PD10_PRS_NME PD10
			INNER JOIN PKUB.LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN PKUB.DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
			INNER JOIN PKUB.FS10_DL_LON FS10
				ON FS10.BF_SSN = LN10.BF_SSN
				AND FS10.LN_SEQ = LN10.LN_SEQ
			INNER JOIN PKUB.LN90_FIN_ATY LN90
				ON LN90.BF_SSN = LN10.BF_SSN
				AND LN90.LN_SEQ = LN10.LN_SEQ
				AND LN90.LC_STA_LON90 = ''A''
				AND LN90.PC_FAT_TYP = ''02''
				AND LN90.PC_FAT_SUB_TYP = ''91''
			LEFT OUTER JOIN PKUB.LN20_EDS LN20
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
			LEFT OUTER JOIN
			(
				SELECT DISTINCT
					BF_SSN,
					LN_SEQ
				FROM
					PKUB.LN72_INT_RTE_HST
				WHERE
					LR_ITR > 6
					AND LD_ITR_EFF_END >= ''2008-08-14''
			)HasGreater6
				ON HasGreater6.BF_SSN = LN10.BF_SSN
				AND HasGreater6.LN_SEQ = LN10.LN_SEQ
		WHERE
			PD10.DF_SPE_ACC_ID IN(''0025866867'',''0062087079'',''0069137920'',''0071085704'',''0094374868'',''0100620552'',''0111018390'',''0132746778'',''0189381750'',''0201161170'',''0241257064'',''0243990188'',''0294783930'',''0314707369'',''0358819664'',''0361416031'',''0386312186'',''0433041193'',''0500633610'',''0509408142'',''0557983064'',''0583368966'',''0596770684'',''0615259227'',''0619792571'',''0691471596'',''0700238312'',''0707226552'',''0732743475'',''0754929126'',''0771123874'',''0807720613'',''0839172708'',''0862902404'',''0881652451'',''0884904512'',''0885121099'',''0897955219'',''0941353339'',''0963412154'',''0980613850'',''1009374473'',''1024435246'',''1048384745'',''1065763116'',''1100233916'',''1150281592'',''1156897247'',''1179260257'',''1183197541'',''1196014205'',''1224930929'',''1243974169'',''1307253351'',''1328097829'',''1370665182'',''1407243897'',''1485459945'',''1504948185'',''1615310009'',''1649021419'',''1653810372'',''1712246325'',''1737220386'',''1764583684'',''1782939767'',''1837694782'',''2037841753'',''2075572151'',''2082580273'',''2083705431'',''2126473508'',''2210500142'',''2236831520'',''2284981163'',''2300170854'',''2374630372'',''2426606584'',''2547254225'',''2560013239'',''2582847100'',''2623001925'',''2660377308'',''2732695471'',''2809304078'',''2890001350'',''2927890862'',''2946816890'',''3042419718'',''3072394215'',''3115560283'',''3164248824'',''3184747276'',''3189076089'',''3233103778'',''3271531921'',''3284942197'',''3365419223'',''3387318693'',''3414166233'',''3428660270'',''3482748530'',''3484867023'',''3512444087'',''3546935465'',''3582273807'',''3591382469'',''3628740340'',''3647394483'',''3655750126'',''3674477209'',''3702522962'',''3704067175'',''3731381417'',''3738965737'',''3748080200'',''3767203952'',''3888520541'',''3906215195'',''3939174319'',''3958673495'',''3996670087'',''4010502963'',''4012470789'',''4048618492'',''4077383519'',''4079924450'',''4080667734'',''4107722435'',''4121331901'',''4146891401'',''4180028647'',''4237550464'',''4283353555'',''4294506184'',''4318307949'',''4322840482'',''4362104531'',''4369734758'',''4471005914'',''4477796415'',''4479111348'',''4481764558'',''4516159545'',''4543084255'',''4546780193'',''4577321844'',''4624436230'',''4637313686'',''4723361391'',''4811071901'',''4834672504'',''4836787217'',''4848239573'',''4907602451'',''4931988053'',''4940231647'',''4949143707'',''4952257339'',''4963081481'',''4975603411'',''5036188833'',''5061869533'',''5069230397'',''5074908673'',''5129660646'',''5157066727'',''5183898610'',''5301780897'',''5320540727'',''5352962836'',''5444885915'',''5454059905'',''5553711030'',''5557498799'',''5560357882'',''5638210204'',''5699137224'',''5706720140'',''5716704782'',''5774475617'',''5779352204'',''5857108492'',''5889616668'',''5969230628'',''5986719971'',''5992240227'',''6029865917'',''6037670005'',''6041651723'',''6054045703'',''6058104720'',''6064081431'',''6076431165'',''6088373712'',''6105339888'',''6116606186'',''6164453563'',''6177046532'',''6300024607'',''6338772638'',''6353175836'',''6407902395'',''6416994493'',''6420666857'',''6460496699'',''6465483087'',''6477128724'',''6478357047'',''6548500999'',''6570149277'',''6599651846'',''6640776535'',''6653911400'',''6679604005'',''6698312455'',''6708807276'',''6721236136'',''6724623970'',''6742338763'',''6748833742'',''6771076955'',''6807870848'',''6857276861'',''6858400920'',''6890755327'',''6899637947'',''6911321406'',''6930612672'',''7057283608'',''7150936381'',''7238039406'',''7249848191'',''7277830156'',''7368668506'',''7433734465'',''7446523435'',''7471637267'',''7505607589'',''7564399481'',''7661226199'',''7681988499'',''7707853494'',''7768966221'',''7791299533'',''7796787648'',''7822405680'',''7844649653'',''7858721789'',''7859940167'',''7892916159'',''7910383750'',''7942100461'',''8023896877'',''8066430986'',''8089331247'',''8092353930'',''8135284478'',''8269079704'',''8280507534'',''8290003435'',''8295245557'',''8325857150'',''8364733836'',''8412504750'',''8429607224'',''8475246652'',''8507548425'',''8514412255'',''8539787071'',''8556132843'',''8578553945'',''8583050364'',''8626503667'',''8654041089'',''8671465525'',''8680003865'',''8734141220'',''8741202070'',''8741223208'',''8787301411'',''8800234129'',''8821440955'',''8822400700'',''8863323361'',''8871005225'',''8939763507'',''8953915024'',''8957105383'',''8965311858'',''8978954853'',''8982244365'',''8986342599'',''9030670132'',''9084267726'',''9094123630'',''9117717702'',''9160016130'',''9161089648'',''9175409017'',''9183472077'',''9197861860'',''9254924873'',''9278905884'',''9352822832'',''9422814361'',''9432029129'',''9441930093'',''9472929475'',''9527139891'',''9531960833'',''9595898871'',''9608849989'',''9633067599'',''9637791753'',''9663658984'',''9682761112'',''9684658551'',''9750974904'',''9771690903'',''9800894352'',''9808996986'',''9842606549'',''9907747540'',''9925255305'',''9926756940'',''9927437375'',''9949417471'',''9952574526'',''9979034792'',''9989406906'')
			AND HasGreater6.BF_SSN IS NOT NULL
	'
	) OQ 
ORDER BY
	OQ.BF_SSN,
	OQ.LN_SEQ