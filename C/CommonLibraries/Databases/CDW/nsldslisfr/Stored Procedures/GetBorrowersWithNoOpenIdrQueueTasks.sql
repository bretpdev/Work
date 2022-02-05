
CREATE PROCEDURE [nsldslisfr].[GetBorrowersWithNoOpenIdrQueueTasks]
AS

declare @Server nvarchar(max) = case when @@ServerName = 'UHEAASQLDB' then 'LEGEND' else 'LEGEND_TEST_VUK1' end

declare @Query nvarchar(max) = 'SELECT * FROM OPENQUERY(' + @Server + ',''
SELECT 
	distinct p.DF_SPE_ACC_ID as AccountNumber, w.BF_SSN as Ssn
FROM 
	PKUB.WQ20_TSK_QUE w
JOIN
	PKUB.PD10_PRS_NME p on p.DF_PRS_ID = w.BF_SSN
WHERE
	w.WF_QUE IN (''''2A'''', ''''2P'''') AND w.WC_STA_WQUE20 = ''''U''''
                     
          '')'
exec (@Query)

RETURN 0