CREATE PROCEDURE [achsetup].[BorrowerHasEndorser]
	@AccountNumber CHAR(10)
AS
	DECLARE @Server NVARCHAR(MAX) = CASE WHEN @@ServerName = 'UHEAASQLDB' THEN 'DUSTER' ELSE 'QADBD004' END
	DECLARE @HasEndorser BIT = 0

	SELECT
		@HasEndorser = 1
	FROM
		LN20_Endorser ln20
	WHERE
		ln20.LC_STA_LON20 = 'A'
	AND
		ln20.DF_SPE_ACC_ID = @AccountNumber
	
	IF (@HasEndorser = 1)
		SELECT @HasEndorser HasEndorser
	ELSE
	BEGIN
		DECLARE @Query NVARCHAR(MAX) = 'SELECT CAST(HASENDORSER AS BIT) HasEndorser FROM OPENQUERY(' + @Server + ',''
		SELECT 
			CASE WHEN COUNT(ln20.LF_EDS) > 0 THEN 1 ELSE 0 END HASENDORSER
		FROM 
			OLWHRM1.LN20_EDS ln20
		JOIN
			OLWHRM1.PD10_PRS_NME pd10 on pd10.DF_PRS_ID = ln20.BF_SSN
		WHERE
			ln20.LC_STA_LON20 = ''''A''''
		AND
			pd10.DF_SPE_ACC_ID = ''''' + @AccountNumber + '''''
	                     
				  '')'
		EXEC (@Query)
	END
RETURN 0