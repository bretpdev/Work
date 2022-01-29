LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE EMAIL AS
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID,
			PDXX.DM_PRS_X, 
			PDXX.DM_PRS_LST,
			LOWCASE(PDXX.DX_ADR_EML) AS DX_ADR_EML
		FROM
			PKUB.PDXX_PRS_NME PDXX
			JOIN PKUB.PDXX_PRS_ADR_EML PDXX
				ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		WHERE
			PDXX.DC_STA_PDXX = 'A'
			AND PDXX.DI_VLD_ADR_EML = 'Y'
			AND PDXX.DF_SPE_ACC_ID IS NOT NULL
		ORDER BY
			PDXX.DF_SPE_ACC_ID
	;
QUIT;
ENDRSUBMIT;


DATA EMAIL; SET LEGEND.EMAIL; RUN;

DATA _NULL_;
	SET		EMAIL;
	FILE	'T:\SAS\Quick Query for Special Email Campaign' delimiter=',' DSD DROPOVER lrecl=XXXXX;
	WHERE 	DF_SPE_ACC_ID <> '';
	/* write column names, remove this to create a file without a header row */
	IF _N_ = X THEN
		DO;
			PUT	
				'DF_SPE_ACC_ID'
				','
				'DM_PRS_X'
				','
				'DM_PRS_LST'
				','
				'DX_ADR_EML'
			;
		END;

	/* write data*/	
	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT DM_PRS_X $ @;
		PUT DM_PRS_LST $ @;
		PUT DX_ADR_EML $;
		;
	END;
RUN;
