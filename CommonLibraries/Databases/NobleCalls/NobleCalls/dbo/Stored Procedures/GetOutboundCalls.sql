
CREATE PROCEDURE [dbo].[GetOutboundCalls]

@Campaigns VARCHAR(MAX)

AS

DECLARE @QUERY AS VARCHAR(MAX) = 
'
	SELECT 
		* 
	FROM OPENQUERY(UHEAAAPP1, 
		''SELECT 
			CH.rowid, 
			CH.call_type, 
			CH.listid, 
			CH.appl, 
			CH.lm_filler2, 
			CH.areacode, 
			CH.phone, 
			CH.addi_status, 
			CH.status, 
			CH.tsr, 
			CH.act_date, 
			CH.act_time, 
			RP.vox_file_name, 
			CH.time_connect 
		FROM 
			call_history CH 
			LEFT JOIN rec_playint RP 
				ON CH.d_record_id = RP.d_record_id 
				AND CH.act_date = RP.call_date 
				AND CH.appl = RP.appl
				AND CH.tsr = RP.tsr 
		WHERE 
			CH.call_type != 5 
			AND CH.appl NOT IN(' + @Campaigns + ')
			AND CH.lm_filler2 IS NOT NULL
			AND CH.status IS NOT NULL
			AND CH.act_date::date = NOW()::date
	'')
'

--PRINT @QUERY
EXEC(@QUERY)