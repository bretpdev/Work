select 
	* 
from 
	openquery
			(legend,
			'
			SELECT
				*
			FROM 
				PKUB.RMXX_RMT_SPS_RFD RMXX
			WHERE
				RMXX.LM_RCP_SPS_RFD_CHK = ''ESTATE OF RICHARD L TRACY''

			')