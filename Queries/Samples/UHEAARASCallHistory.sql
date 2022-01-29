SELECT
	*
FROM
	OPENQUERY
	(
		UHEAARAS,
		'
			SELECT 
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
				rp.vox_file_name, 
				CH.time_connect 
			FROM 
				call_history111816 CH 
				LEFT JOIN rec_playinth111816 RP 
					ON CH.d_record_id = RP.d_record_id 
					AND CH.act_date = RP.call_date 
					AND CH.appl = RP.appl 
			WHERE 
				CH.call_type != 5 
				AND CH.appl NOT IN (''CLDA'',''USKP'',''CLCY'',''UKST'',''CLOU'',''CLDU'',''CLDL'',''CLOI'',''CLDI'',''DSC6'',''ISU6'',''SCRC'',''SLCC'',''USU6'',''UUBW'',''UVU6'',''WGU6'',''CLIN'',''CLDO'',''USPN'',''USTN'',''NIC6'')
			ORDER BY
				act_date DESC
		'
	)