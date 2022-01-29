CREATE PROCEDURE [dbo].[spMD_HasAP03Record]
	@Ssn char(9)
AS
	declare @HasAp03Record as bit = 0
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AP03_Master_Application'))
	BEGIN
		set @HasAp03Record = cast(case when exists(
			select * from dbo.AP03_Master_Application ap03 where BF_SSN = @Ssn) then 1 else 0 end as bit)
	END
	select @HasAp03Record as HasAP03Record
RETURN 0
