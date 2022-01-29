USE [EA27_BANA_1]
GO

SELECT DISTINCT
	BR.BorrowerSSN
	,BR.[BorrowerLastName]
	,BR.[BorrowerLastNameSuffix]
	,BR.[BorrowerFirstName]
	,BR.[BorrowerMiddleName]
	,BAR.[BorrowerStreetAddress]
	,BAR.[BorrowerCareOfAddress]
	,BAR.[BorrowerCity]
	,BAR.[BorrowerState]
	,BAR.[BorrowerZip]
	,SBR.[HomePhoneNumber]
	,SBR.[AlternatePhoneNumber]
FROM 
	[dbo].[_01BorrowerRecord] BR
	INNER JOIN [dbo].[_05SupplementalBorrowerRecord] SBR
		ON BR.BorrowerSSN = SBR.BorrowerSSN
	INNER JOIN [dbo].[_06BorrowerAddressRecord] BAR
		ON BR.BorrowerSSN = BAR.BorrowerSSN
WHERE
	BR.BorrowerSSN in 
	(
	030702184
	,042905054
	,044803872
	,049862598
	,074767674
	)


USE [EA27_BANA_2]
GO

SELECT DISTINCT
	BR.BorrowerSSN
	,BR.[BorrowerLastName]
	,BR.[BorrowerLastNameSuffix]
	,BR.[BorrowerFirstName]
	,BR.[BorrowerMiddleName]
	,BAR.[BorrowerStreetAddress]
	,BAR.[BorrowerCareOfAddress]
	,BAR.[BorrowerCity]
	,BAR.[BorrowerState]
	,BAR.[BorrowerZip]
	,SBR.[HomePhoneNumber]
	,SBR.[AlternatePhoneNumber]
FROM 
	[dbo].[_01BorrowerRecord] BR
	INNER JOIN [dbo].[_05SupplementalBorrowerRecord] SBR
		ON BR.BorrowerSSN = SBR.BorrowerSSN
	INNER JOIN [dbo].[_06BorrowerAddressRecord] BAR
		ON BR.BorrowerSSN = BAR.BorrowerSSN
WHERE
	BR.BorrowerSSN IN 
	(
	040881661
	,046820433
	,048069083
	,055781365
	,083781584
	,093762268
	,098781239
	,126781559
	,143900361
	,151880394
	,153869746
	,154861235
	,156880952
	,219139119
	,229531080
	,464899458
	,487041195
	,532159880
	,534171164
	,540170895
	,540332007
	,544297716
	,561938879
	,565930864
	,574802223
	,591909097
	,605461794
	,606426612
	,609262216
	,609422182
	,610685724
	,613326813
	,613425852
	,615540740
	,621327439
	,623561456
	,627087498
	,638149064
	,641207023
	)
