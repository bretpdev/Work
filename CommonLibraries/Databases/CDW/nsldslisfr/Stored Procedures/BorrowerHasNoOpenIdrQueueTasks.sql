CREATE PROCEDURE [nsldslisfr].[BorrowerHasNoOpenIdrQueueTasks]
	@Ssn char(9)
AS

declare @Server nvarchar(max) = case when @@ServerName = 'UHEAASQLDB' then 'LEGEND' else 'LEGEND_TEST_VUK1' end

declare @Query nvarchar(max) = 'SELECT cast(HasNoOpenIdrQueueTasks as BIT) as HasNoOpenIdrQueueTasks FROM OPENQUERY(' + @Server + ',''
SELECT 
	case MIN(count(*), 1) when 1 then 0 else 1 end as HasNoOpenIdrQueueTasks
FROM 
	PKUB.GRRQ_NDS_PRS_REQ g
WHERE
	g.DD_LIS_REQ_BY_DPT >= (CURRENT DATE - 3 DAYS)
AND 
	g.DF_PRS_ID = ''''' + @Ssn + '''''
                     
          '')'
exec (@Query)

RETURN 0