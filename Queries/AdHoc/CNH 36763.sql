--SELECT * FROM [CDW].[dbo].[MRXX_BR_TAX] WHERE LF_TAX_YR = XXXX --table dump for ticket

select count(*) FROM [CDW].[dbo].[MRXX_BR_TAX] --X,XXX,XXX --UHEAASQLDB
select count(*) FROM [CDW].[dbo].[MRXX_BR_TAX] WHERE LF_TAX_YR = XXXX --XXX,XXX --UHEAASQLDB

--staging table to update OPSDEV:
--select * into [CDW].[dbo].[_MRXX_BR_TAX] from [CDW].[dbo].[MRXX_BR_TAX] where X=X --create table structure for staging table
select count(*) FROM [CDW].[dbo].[_MRXX_BR_TAX] --X,XXX,XXX OPSDEV before refresh
												--X,XXX,XXX OPSDEV after refresh
select count(*) FROM [CDW].[dbo].[_MRXX_BR_TAX] WHERE LF_TAX_YR = XXXX --XXX,XXX --OPSDEV after refresh
