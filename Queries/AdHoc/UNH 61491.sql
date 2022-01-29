BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

INSERT INTO [ULS]..ArcAddProcessing
           ([ArcTypeId]
           ,[ArcResponseCodeId]
           ,[AccountNumber]
           ,[RecipientId]
           ,[ARC]
           ,[ActivityType]
           ,[ActivityContact]
           ,[ScriptId]
           ,[ProcessOn]
           ,[Comment]
           ,[IsReference]
           ,[IsEndorser]
           ,[ProcessFrom]
           ,[ProcessTo]
           ,[NeededBy]
           ,[RegardsTo]
           ,[RegardsCode]
           ,[LN_ATY_SEQ]
           ,[ProcessingAttempts]
           ,[CreatedAt]
           ,[CreatedBy]
           ,[ProcessedAt])
SELECT
	1, --All loans
	NULL,
	PD10.DF_SPE_ACC_ID,
	NULL,
	'M1411', --ARC
	NULL,
	NULL,
	'UNH 61491', -- Script Id
	GETDATE(), -- Process On
	'IBR Clean up in progress', -- Comment
	0,
	0,
	NULL, --Process From
	NULL,
	NULL,
	NULL,
	NULL,
	0, --LN_ATY_SEQ
	0, --Processing Attempts
	GETDATE(), --Created At
	SUSER_NAME(), --Created By
	NULL
FROM
	UDW..PD10_PRS_NME PD10
WHERE
	PD10.DF_PRS_ID IN
(
'004825225',
'005629860',
'011569777',
'012584770',
'015683291',
'016566573',
'020622951',
'024741749',
'025685960',
'025685960',
'033741383',
'034628752',
'043744626',
'043804552',
'043804552',
'044862801',
'046860457',
'049720855',
'049720855',
'050804829',
'055744939',
'055927718',
'055927718',
'056747616',
'056987230',
'057723344',
'059804614',
'061684834',
'061684834',
'061684834',
'061743153',
'061743153',
'063504885',
'063504885',
'063781485',
'065748559',
'065809504',
'065809504',
'065809504',
'065809504',
'066740037',
'067506170',
'067786104',
'071702170',
'071707735',
'072783771',
'074545183',
'074707666',
'075503254',
'075663163',
'075782560',
'075782560',
'075783266',
'077769038',
'078680778',
'078680778',
'078709508',
'078765505',
'084541000',
'084703599',
'084703599',
'086781284',
'086781284',
'087667072',
'087667072',
'088767526',
'088884519',
'089760564',
'089762637',
'091688236',
'095541477',
'095541477',
'095541477',
'095541477',
'102668014',
'102668014',
'103700652',
'105880801',
'107706499',
'107707418',
'109682274',
'109682274',
'110746735',
'112823107',
'112847909',
'113763767',
'115764828',
'117649454',
'117649454',
'117744001',
'118681468',
'118740767',
'118740767',
'121746448',
'122643321',
'122726391',
'122726391',
'126668510',
'126721875',
'127649635',
'127649635',
'127740328',
'129662709',
'133620035',
'133620035',
'134640026',
'137848985',
'138901797',
'138901797',
'142888016',
'142888016',
'142888016',
'145507687',
'155785673',
'155785673',
'158864593',
'163680090',
'164680821',
'166726922',
'170561015',
'171842532',
'171842532',
'174567849',
'179627046',
'182644165',
'184706878',
'188608974',
'196688717',
'197602144',
'198423187',
'198708238',
'198708238',
'198709995',
'198709995',
'200706534',
'200722059',
'210501509',
'212027635',
'213350416',
'214152184',
'214152184',
'214488827',
'215358409',
'215731748',
'215731748',
'217065906',
'217065906',
'217196558',
'217621822',
'217621822',
'217947386',
'217947386',
'217947386',
'218820532',
'220087580',
'220087580',
'222569015',
'223157165',
'223411521',
'224456974',
'224518154',
'224773097',
'224773097',
'224773097',
'224773097',
'224773097',
'224773097',
'224773097',
'224773097',
'224773097',
'224773097',
'224773097',
'224773097',
'226139545',
'227438892',
'227438892',
'227471861',
'227471861',
'227474360',
'229650012',
'230454130',
'231232122',
'231232122',
'231492600',
'234210898',
'237633990',
'237674253',
'237699217',
'239139020',
'239319664',
'239353606',
'239631639',
'241179549',
'242210701',
'243215616',
'243677656',
'244048482',
'246577798',
'246577798',
'247415073',
'247493997',
'247538166',
'247551688',
'248193532',
'250298536',
'250513084',
'250731140',
'250731140',
'251697875',
'252573539',
'253672222',
'255431576',
'256672128',
'257048680',
'257313626',
'257451343',
'257613678',
'258333082',
'258655967',
'258798086',
'258798086',
'258798086',
'259653272',
'260551453',
'262614425',
'266139891',
'266139891',
'266370996',
'279824788',
'284862847',
'285762467',
'289743864',
'291941346',
'293745445',
'296703282',
'298827110',
'301826893',
'310049962',
'313682017',
'313682017',
'317884311',
'324706002',
'328760432',
'341805782',
'343846489',
'345705625',
'350862339',
'350862339',
'351825159',
'352823612',
'353841848',
'358720673',
'359609955',
'359624949',
'359682440',
'360787814',
'367172715',
'369622323',
'375081594',
'376118723',
'376900300',
'380083239',
'380083239',
'384809093',
'384809093',
'384946744',
'388725324',
'397902028',
'398903882',
'398903882',
'399866306',
'401250769',
'408657758',
'411432942',
'412632118',
'413959344',
'418532179',
'420252887',
'421310751',
'425654372',
'426134181',
'428197291',
'428618426',
'429417905',
'429739212',
'430796952',
'431061173',
'431257982',
'431739746',
'431739746',
'431750497',
'432695572',
'434690852',
'434730925',
'438770083',
'439719927',
'442663072',
'448908013',
'448922468',
'449914307',
'449914307',
'449914307',
'449914307',
'450474847',
'450474847',
'450615044',
'450638614',
'450638614',
'450755976',
'450755976',
'450755976',
'450917938',
'450958590',
'450958590',
'450958590',
'451639963',
'451639963',
'451678242',
'451678242',
'451678242',
'451832413',
'451832413',
'451832413',
'451838381',
'452152043',
'452516813',
'452573369',
'452659836',
'452731753',
'452731753',
'452874143',
'452874143',
'452897443',
'453559747',
'453954547',
'454679073',
'454679073',
'454733249',
'454733249',
'454778056',
'454819391',
'455411854',
'455616261',
'455834556',
'455834556',
'455839451',
'455839451',
'455916539',
'455918340',
'455918340',
'455973217',
'455973217',
'456290260',
'456655599',
'456890811',
'456890811',
'456896094',
'456933946',
'456933946',
'456976406',
'456976406',
'457915316',
'457915316',
'457973182',
'457973182',
'457973182',
'457973182',
'457976751',
'458554059',
'458559468',
'458576476',
'458650644',
'458791383',
'458835390',
'458854840',
'458854840',
'458910127',
'458954467',
'458954467',
'459392764',
'459638874',
'459638874',
'459638874',
'459772789',
'459793164',
'459793164',
'459793164',
'460218721',
'460753459',
'460819560',
'460819560',
'460819560',
'460833105',
'460952865',
'461830590',
'461830590',
'461935847',
'461954625',
'461977719',
'462574061',
'462574061',
'462574061',
'462574061',
'462758163',
'462758163',
'462854868',
'463494054',
'463494054',
'463494054',
'463496855',
'463573276',
'463573276',
'463573276',
'463573276',
'463638618',
'463717692',
'463732281',
'463732281',
'463814831',
'463857368',
'463857368',
'463893412',
'463893412',
'463915319',
'463915319',
'463915319',
'464318664',
'464338358',
'464536506',
'464599396',
'465454741',
'465772280',
'465825796',
'465825796',
'466155254',
'466155254',
'466754614',
'466956487',
'466997443',
'466997443',
'467256190',
'467256190',
'467256190',
'467256190',
'467553148',
'467599056',
'467637840',
'467670169',
'467670169',
'467715362',
'467715362',
'467737178',
'467916213',
'468294162',
'468294162',
'469881665',
'470883264',
'473881466',
'480385007',
'480385007',
'480961875',
'483729808',
'488964735',
'493709471',
'495582081',
'496946463',
'498867835',
'499949284',
'502983794',
'503172305',
'503238890',
'504081426',
'504192603',
'504199056',
'505087126',
'506069268',
'506213045',
'507213655',
'508086152',
'509902026',
'509902026',
'509902026',
'511047175',
'511132648',
'511132648',
'515827322',
'516111205',
'516211745',
'517044969',
'517178667',
'518159589',
'518178884',
'518196931',
'518212041',
'518315307',
'518318689',
'518338271',
'518394865',
'518476885',
'518985852',
'518987662',
'518987662',
'519040253',
'519040253',
'519178322',
'519214093',
'519215259',
'519312589',
'519316548',
'519317383',
'519542587',
'519542587',
'519800014',
'520048709',
'520048709',
'520084255',
'520084255',
'520110073',
'520152265',
'520172760',
'520173378',
'520174065',
'520174065',
'520174065',
'520174065',
'520190722',
'520190722',
'520190722',
'520190722',
'520211641',
'520213905',
'520232036',
'520232036',
'520252478',
'520252478',
'520252478',
'520258733',
'520296269',
'520609351',
'520609351',
'520661052',
'520764094',
'520764094',
'520764094',
'520764094',
'520766973',
'520800746',
'520800746',
'520864194',
'520920360',
'521087101',
'521111859',
'521111859',
'521111859',
'522173202',
'522173202',
'522233813',
'523319176',
'523515977',
'523616815',
'524114766',
'524373481',
'524571165',
'524573295',
'524573295',
'524981080',
'524981080',
'525411384',
'525591206',
'525830654',
'526024868',
'526024868',
'526590169',
'526912489',
'527792634',
'527938022',
'528060671',
'528060671',
'528087116',
'528175236',
'528175236',
'528177233',
'528257363',
'528258385',
'528290945',
'528318549',
'528334113',
'528334113',
'528358397',
'528373933',
'528416168',
'528419222',
'528451468',
'528455423',
'528497039',
'528497214',
'528518204',
'528519060',
'528519337',
'528555295',
'528579220',
'528579445',
'528592797',
'528594335',
'528611514',
'528611691',
'528615437',
'528634743',
'528675897',
'528676281',
'528676769',
'528676769',
'528690376',
'528692727',
'528703446',
'528715843',
'528732739',
'528734762',
'528734762',
'528735064',
'528735064',
'528759114',
'528759791',
'528760393',
'528790131',
'528791200',
'528799143',
'528809207',
'528810879',
'528830961',
'528831007',
'528832298',
'528832613',
'528855058',
'528855326',
'528857694',
'528874508',
'528892374',
'528892763',
'528897552',
'528897570',
'528949696',
'528961739',
'528961739',
'528989589',
'528991467',
'528991467',
'528991467',
'529029152',
'529081599',
'529112525',
'529139982',
'529192815',
'529210319',
'529253537',
'529272435',
'529319561',
'529331957',
'529333416',
'529338635',
'529373269',
'529377893',
'529395691',
'529395691',
'529398251',
'529430432',
'529434070',
'529439845',
'529456782',
'529476718',
'529494123',
'529513883',
'529535497',
'529539522',
'529539522',
'529550229',
'529550229',
'529596679',
'529599056',
'529599056',
'529616157',
'529618128',
'529645523',
'529651735',
'529652449',
'529670107',
'529672610',
'529674752',
'529679788',
'529690145',
'529692334',
'529692334',
'529693135',
'529693386',
'529693386',
'529699269',
'529722023',
'529736445',
'529736930',
'529750210',
'529750222',
'529753445',
'529757240',
'529758949',
'529770405',
'529777780',
'529779620',
'529779950',
'529793017',
'529797078',
'529799252',
'529799252',
'529803411',
'529812927',
'529825293',
'529830684',
'529835103',
'529838931',
'529872936',
'529873382',
'529883778',
'529890087',
'529890509',
'529891015',
'529901499',
'529959741',
'529970550',
'529972019',
'529976833',
'529976855',
'529978289',
'530023758',
'530397403',
'530624520',
'530624520',
'530943970',
'531150457',
'531191498',
'533399029',
'533399029',
'533623120',
'534170530',
'534173700',
'534219945',
'534219945',
'534945957',
'534945957',
'535788122',
'535788122',
'536086514',
'536394766',
'536909714',
'537804516',
'537804516',
'538178789',
'538178789',
'538293045',
'539065532',
'539137310',
'539157001',
'540232357',
'540290420',
'540293800',
'540948534',
'541298897',
'541332771',
'542860456',
'542860456',
'543043823',
'543172672',
'543230368',
'543230368',
'543230368',
'543230368',
'543317403',
'543356671',
'544254968',
'544259076',
'544335115',
'544335115',
'544847806',
'544847806',
'545994275',
'546293576',
'546935490',
'547174687',
'547352561',
'549436178',
'549795552',
'549871087',
'550158214',
'550854728',
'551499785',
'551639317',
'551735772',
'551735772',
'551811797',
'551811797',
'551824446',
'552694323',
'552929123',
'552929123',
'553471231',
'553471231',
'553651812',
'553651812',
'553790074',
'555814915',
'555814915',
'555819922',
'556394049',
'556730878',
'556939988',
'557491593',
'557938309',
'558774737',
'559157684',
'559157684',
'559411972',
'559818690',
'560637634',
'563258420',
'564615882',
'564915426',
'564933146',
'564933146',
'564933146',
'565418992',
'565418992',
'565636707',
'566047524',
'566451431',
'566451431',
'568514622',
'568632826',
'568915932',
'569534346',
'570711752',
'571611589',
'571711037',
'571711037',
'573670378',
'573758780',
'573758780',
'574029550',
'574808168',
'574882962',
'574884981',
'575114939',
'575357583',
'575517426',
'576218383',
'576372750',
'576434832',
'576478027',
'576626547',
'576982457',
'577941849',
'579137247',
'581655677',
'581735816',
'583952222',
'583952222',
'585632397',
'585758532',
'586669955',
'586760324',
'586760324',
'587452924',
'589122145',
'589904613',
'590403501',
'590765938',
'590765938',
'591082372',
'592014987',
'592014987',
'594393401',
'594443512',
'595913826',
'595913826',
'600109723',
'600109723',
'600173734',
'600328128',
'600445073',
'600540364',
'600904333',
'601078486',
'601092164',
'601092164',
'601143140',
'601362224',
'601527400',
'601768786',
'601842707',
'601842707',
'603264200',
'603264200',
'603441666',
'603500891',
'604168137',
'604229720',
'604435815',
'606207167',
'606366929',
'607095548',
'607165619',
'607165619',
'607388335',
'608124077',
'608124077',
'608124077',
'608124318',
'609627484',
'610108883',
'610173699',
'610227703',
'611203362',
'611357642',
'612037864',
'612037864',
'612038212',
'612050841',
'612422683',
'612528933',
'613981623',
'615343419',
'615408729',
'615440450',
'616039356',
'617018744',
'617018744',
'617075089',
'617220249',
'617266623',
'617424036',
'617728528',
'619073383',
'619073383',
'620496657',
'621444352',
'622344016',
'623051358',
'623548754',
'624106847',
'624438345',
'624438345',
'625015225',
'625163819',
'625521120',
'625521120',
'625634887',
'626246195',
'627056925',
'627208902',
'627208902',
'627208902',
'628186398',
'628400021',
'629100914',
'629100914',
'629208801',
'629208801',
'630039450',
'630121015',
'631015884',
'631030051',
'631092824',
'631181369',
'631181369',
'631240335',
'632107180',
'632107180',
'633036372',
'633036372',
'633079381',
'634053033',
'634146374',
'634208606',
'634807104',
'635125078',
'635125078',
'637039779',
'637052548',
'637075411',
'638037288',
'638109166',
'639146561',
'639146561',
'640014707',
'640094915',
'640123757',
'640123757',
'640123757',
'640125353',
'640125353',
'640126934',
'640126934',
'640126934',
'641109407',
'641167027',
'642070097',
'642070097',
'642071374',
'642071374',
'643091660',
'643123266',
'644167290',
'645012464',
'645105051',
'645105051',
'646052128',
'646053386',
'646059234',
'646103405',
'646285079',
'646500992',
'646520615',
'646562597',
'647016732',
'647183869',
'647207704',
'647208720',
'647220859',
'647325665',
'647525274',
'671300731',
'751092463',
'769501488',
'769501488',
'770669306',
'027687000',
'063802070',
'068425826',
'068425826',
'102562931',
'129662709',
'129662709',
'129700906',
'181704782',
'213131304',
'227497320',
'303900314',
'329822111',
'378152472',
'391787538',
'407068101',
'439354468',
'450839939',
'450839939',
'452914651',
'456558512',
'456558512',
'463617686',
'463617686',
'498081887',
'504151140',
'518213839',
'520132472',
'528171903',
'528516832',
'528696593',
'528696593',
'528755057',
'529632060',
'529876183',
'530211388',
'536984999',
'549372944',
'551979635',
'563779397',
'564615882',
'564615882',
'571696766',
'571696766',
'575254654',
'601092164',
'601092164',
'601092164',
'601865304',
'608800364',
'611784645',
'612442052',
'625340181',
'629746186',
'629746186',
'637093969'
)

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 842 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END
