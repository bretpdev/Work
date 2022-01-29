--SASR_4616 PROMOTION FILE BASED ON "2 EXEC SPROC RefreshTableWithValidation.sql"

EXEC ODW..RefreshTableWithValidation 'DC11_LON_FAT','LF_LST_DTS_DC11','DC11_LON_FAT','BF_SSN'

--@TableName, @OverrideDateColumn, @OverrideLocalTableName, @SSNField


---- COUNT OF RECORDS LOADED
--USE ODW;
--GO
--EXEC sp_spaceused N'dbo.DC11_LON_FAT'; 
--GO