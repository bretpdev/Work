----- LIVE SPROC PARAMETERS
EXEC ODW..RefreshTableWithValidation
'DC11_LON_FAT',		--@TableName
'LF_LST_DTS_DC11',	--@OverrideDateColumn
'DC11_LON_FAT',		--@OverrideLocalTableName
'BF_SSN'			--@SSNField
;

---- COUNT OF RECORDS LOADED
--USE ODW;
--GO
--EXEC sp_spaceused N'dbo.DC11_LON_FAT'; 
--GO