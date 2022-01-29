-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[spGetAccountNumberFromSsn] 
	@SSN varchar(10)
AS
BEGIN
	
SELECT 
	DF_SPE_ACC_ID 
FROM 
	PD10_PRS_NME
WHERE
	DF_PRS_ID = @SSN
	
IF(@@ROWCOUNT = 0)
	BEGIN
		RAISERROR('spGetAcctNumberFromSSN RETURNED 0 RECORDS FOR SSN:%s;', 16, 1, @SSN) 
	END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetAccountNumberFromSsn] TO [db_executor]
    AS [dbo];

