LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE NREF AS
	SELECT	*
	FROM	CONNECTION TO DB2 (
		SELECT DISTINCT 
			PD10_2.DF_SPE_ACC_ID
		FROM
			OLWHRM1.PD10_PRS_NME PD10
		INNER JOIN OLWHRM1.PD42_PRS_PHN PD42
			ON PD10.DF_PRS_ID = PD42.DF_PRS_ID
		INNER JOIN OLWHRM1.LN20_EDS LN20
			ON PD10.DF_PRS_ID = LN20.LF_EDS
		INNER JOIN OLWHRM1.LN10_LON LN10
			ON LN20.BF_SSN = LN10.BF_SSN
			AND LN20.LN_SEQ = LN10.LN_SEQ
		INNER JOIN OLWHRM1.PD10_PRS_NME PD10_2
			ON PD10_2.DF_PRS_ID = LN20.LF_EDS
		WHERE PD42.DI_PHN_VLD = 'Y'
			AND PD42.DC_ALW_ADL_PHN IN ('P','X')
			AND PD42.DF_LST_DTS_PD42 < '04/01/2012'
			AND LN10.LA_CUR_PRI > 0
			AND LN10.LC_STA_LON10 = 'R'
			FOR READ ONLY WITH UR
		);

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
DISCONNECT FROM DB2;
QUIT;
ENDRSUBMIT;

/*write to comma delimited file*/
DATA _NULL_;
	SET  DUSTER.NREF;
	FILE 'T:\SAS\NH 16573.txt' delimiter=',' DSD DROPOVER lrecl=32767;

	FORMAT DF_SPE_ACC_ID $10. ;

	IF _N_ = 1 THEN        /* write column names */
		DO;
			PUT
			'DF_SPE_ACC_ID'
			
			;
		END;

	DO;
		PUT DF_SPE_ACC_ID $ @;
		PUT '\n'
		;
	END;
RUN;
