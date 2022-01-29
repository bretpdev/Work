DECLARE @Populations TABLE
(
	SSN VARCHAR(9)
)

INSERT INTO
	@Populations
(
	SSN
)
VALUES
('021722098')
,('026700565')
,('030702184')
,('037580439')
,('040881661')
,('042905054')
,('044803872')
,('046820433')
,('048069083')
,('049862598')
,('055781365')
,('056584941')
,('057745854')
,('064786737')
,('074767674')
,('078564805')
,('079681075')
,('083781584')
,('084680744')
,('091662816')
,('093762268')
,('094787380')
,('096843908')
,('098781239')
,('099760742')
,('106625990')
,('111703486')
,('115700296')
,('117727499')
,('117783506')
,('118804651')
,('120740236')
,('121662175')
,('126781559')
,('143900361')
,('151880394')
,('153869746')
,('154861235')
,('156880952')
,('157805045')
,('191581264')
,('219139119')
,('220237559')
,('224412792')
,('224439391')
,('229531080')
,('238419415')
,('243638790')
,('303117432')
,('382082033')
,('383862682')
,('393042944')
,('414438588')
,('446841897')
,('450933882')
,('451760837')
,('455871406')
,('464899458')
,('469088065')
,('487041195')
,('493927399')
,('525596432')
,('532159880')
,('532251475')
,('532688467')
,('533986736')
,('534171164')
,('534964278')
,('535111530')
,('537049686')
,('537139075')
,('537801565')
,('537941580')
,('538884494')
,('539110837')
,('540170895')
,('540332007')
,('542315445')
,('543297390')
,('544297716')
,('544786492')
,('545538146')
,('545818330')
,('546895967')
,('546938682')
,('548870492')
,('549577513')
,('552633829')
,('552815276')
,('553841831')
,('554991203')
,('555671099')
,('556758445')
,('556774073')
,('557731636')
,('558771877')
,('558777264')
,('560047813')
,('560778904')
,('561772743')
,('561938879')
,('562958396')
,('563978968')
,('565851285')
,('565930864')
,('566275088')
,('566596721')
,('566758354')
,('566931363')
,('567612629')
,('567699338')
,('567938172')
,('567977314')
,('568993882')
,('570395377')
,('570971544')
,('571633358')
,('572086302')
,('573694352')
,('573997440')
,('574802223')
,('574820997')
,('577333095')
,('591909097')
,('594422142')
,('600155348')
,('601261782')
,('602020511')
,('602383361')
,('603132395')
,('603202247')
,('604320165')
,('605461794')
,('606426612')
,('607782408')
,('608800322')
,('609262216')
,('609422182')
,('610685724')
,('611201600')
,('611261603')
,('611725066')
,('612300676')
,('612524159')
,('613187428')
,('613326813')
,('613425852')
,('613589076')
,('613662189')
,('613741861')
,('614266190')
,('615058843')
,('615540740')
,('617054019')
,('618525791')
,('619385648')
,('621327224')
,('621327439')
,('623080623')
,('623504930')
,('623561456')
,('624119347')
,('624242595')
,('627087498')
,('635145610')
,('636019188')
,('638031677')
,('638149064')
,('641207023')

--finds on borrower & student
SELECT DISTINCT
	'' AS EA27
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.BorrowerSSN
	AND P.SSN = BR.StudentSSN

SELECT DISTINCT
	'' AS EA27_BANA
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.BorrowerSSN
	AND P.SSN = BR.StudentSSN

SELECT DISTINCT
	'' AS EA27_BANA_1
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA_1].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.BorrowerSSN
	AND	P.SSN = BR.StudentSSN

SELECT DISTINCT
	'' AS EA27_BANA_2
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA_2].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.BorrowerSSN
	AND	P.SSN = BR.StudentSSN

SELECT DISTINCT
	'' AS EA27_BANA_TEST
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA_TEST].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.BorrowerSSN
	AND	P.SSN = BR.StudentSSN

--finds on borrower
SELECT DISTINCT
	'' AS EA27
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.BorrowerSSN

SELECT DISTINCT
	'' AS EA27_BANA
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.BorrowerSSN

SELECT DISTINCT
	'' AS EA27_BANA_1
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA_1].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.BorrowerSSN

SELECT DISTINCT
	'' AS EA27_BANA_2
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA_2].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.BorrowerSSN

SELECT DISTINCT
	'' AS EA27_BANA_TEST
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA_TEST].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.BorrowerSSN

--finds on student
SELECT DISTINCT
	'' AS EA27
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.StudentSSN

SELECT DISTINCT
	'' AS EA27_BANA
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.StudentSSN

SELECT DISTINCT
	'' AS EA27_BANA_1
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA_1].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.StudentSSN

SELECT DISTINCT
	'' AS EA27_BANA_2
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA_2].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.StudentSSN

SELECT DISTINCT
	'' AS EA27_BANA_TEST
	,SSN
	,[BorrowerSSN]
	,[StudentSSN]
FROM 
	[EA27_BANA_TEST].[dbo].[_01BorrowerRecord] BR
	INNER JOIN @Populations P 
	ON P.SSN = BR.StudentSSN