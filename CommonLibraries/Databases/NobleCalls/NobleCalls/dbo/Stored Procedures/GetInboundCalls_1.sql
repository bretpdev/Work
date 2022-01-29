
CREATE PROCEDURE [dbo].[GetInboundCalls]

AS

DECLARE @QUERY AS VARCHAR(MAX) = 
'
	SELECT 
		* 
	FROM OPENQUERY(UHEAAAPP1, 
		''SELECT 
			IB.rowid, 
			IB.call_type, 
			IB.listid, 
			IB.appl,
			IB.filler2, 
			IB.ani_acode, 
			IB.ani_phone, 
			IB.addi_status, 
			IB.status, 
			IB.tsr, 
			IB.call_date, 
			IB.call_time, 
			RP.vox_file_name, 
			IB.time_connect 
		FROM 
			inboundlog IB 
			LEFT JOIN rec_playint RP 
				ON IB.call_date = RP.call_date 
				AND IB.appl = RP.appl 
				AND IB.d_record_id = RP.d_record_id
				AND IB.tsr = RP.tsr
		WHERE
			IB.call_date::date = NOW()::date
			AND IB.status IS NOT NULL
	'')
'

--PRINT @QUERY
EXEC(@QUERY)