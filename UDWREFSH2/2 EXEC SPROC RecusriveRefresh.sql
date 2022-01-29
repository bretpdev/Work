-----TEST SPROC PARAMETERS

USE ODW

GO

EXEC RecusriveRefresh
'AY01_BR_ATY',
'OLWHRM1.AY01_BR_ATY',
'BF_LST_DTS_AY01',
'30'

--@LocalTableName VARCHAR(MAX),
--@RemoteTableName VARCHAR(MAX),
--@DateColumn VARCHAR(MAX),
--@DaysPerCycle VARCHAR(MAX)











----- LIVE SPROC PARAMETERS

EXEC RefreshTableWithValidation
'OLWHRM1.LN18_DSB_FEE',
'LF_LST_DTS_LN18',
'LN18_DSB_FEE',
'BF_SSN'
;

--@TableName varchar(50),
--@OverrideDateColumn varchar(50),
--@OverrideLocalTableName varchar(50),
--@SSNField varchar(50)