/*TEST*/
/*%LET TESTMSG = - THIS IS A TEST;
%LET RPTLIB = T:\SAS;
%LET LOGLIB = Y:\Batch\Logs;
%LET CODELIB = Y:\Codebase\SAS\CDW;
%LET ODBCFILE = BSYS_TEST;
%LET DB = CDW_test;
%LET ODBCFILE_CSYS = CSYSTest;*/

/*LIVE*/
%LET TESTMSG = - THIS IS LIVE;
%LET RPTLIB = Z:\Batch\FTP;
%LET LOGLIB = Z:\Batch\Logs;
%LET CODELIB = Z:\Codebase\SAS\CDW;
%LET ODBCFILE = BSYS;
%LET DB = CDW;
%LET ODBCFILE_CSYS = CSYS;

%INCLUDE "&CODELIB\UTNWDW1 CDW DAILY UPDATE.FOLDER AND DATABASE.SAS"  ;
%FILECHECK(46);


DATA LN83(drop=trunc);
	INFILE REPORT46 DSD DLM = ',' FIRSTOBS=1 MISSOVER end = eof LRECL = 32767;
	INPUT 
		BF_SSN :$9.
		LN_SEQ :2.
		BN_EFT_SEQ :2.
		LF_EFT_OCC_DTS :DATETIME25.
		LF_LST_DTS_LN83 :DATETIME25.
		LD_EFT_EFF_BEG :DATE9.
		LD_EFT_EFF_END :DATE9.
		LC_EFT_SUS_REA :$1.
		LC_STA_LN83 :$1.
		LR_EFT_RDC :5.3
		LI_EFT_RIR_MNL_OVR :$1.
		LI_EFT_RST :$1.
		LF_LST_USR_LN83 :$12.
		LF_LST_SRC_LN83 :$16.
	;
	%TRUNCAT1(46, BF_SSN);
RUN;

/*convert SAS dates to SQL Server datetime*/
DATA LN83 (drop=hour minute sec mil LF_EFT_OCC_DTS LF_LST_DTS_LN83_d LF_LST_DTS_LN83_t);
	SET LN83;
		length VARCHAR_LF_EFT_OCC_DTS $40.;
		LD_EFT_EFF_BEG = LD_EFT_EFF_BEG * 86400;
		LD_EFT_EFF_END = LD_EFT_EFF_END * 86400;
		LF_LST_DTS_LN83_d = datepart(LF_EFT_OCC_DTS);
		LF_LST_DTS_LN83_t = timepart(LF_EFT_OCC_DTS);
		hour = hour(LF_LST_DTS_LN83_t);
		minute = minute(LF_LST_DTS_LN83_t);
		sec = second(LF_LST_DTS_LN83_t);
		mil = substr(LF_LST_DTS_LN83_t, (index(LF_LST_DTS_LN83_t, '.')+ 1));
		format LF_LST_DTS_LN83_d mmddyy10.;
		format hour Z2.;
		format minute Z2.;
		format sec Z2.;
		VARCHAR_LF_EFT_OCC_DTS = cat(put(LF_LST_DTS_LN83_d, mmddyy10.),' ', put(hour, $Z2.), ':', put(minute, $Z2.),':',put(sec, $Z2.),'.',mil);
RUN;

PROC SQL;
	CREATE TABLE LN83 AS 
		SELECT DISTINCT
			*
		FROM
			LN83
;
QUIT;


%COPYERROR;

%ENCHILADA
	(
		LN83,
		LN83_EFT_TO_LON,
		BF_SSN  
		LN_SEQ  
		BN_EFT_SEQ  
		VARCHAR_LF_EFT_OCC_DTS,  
		LF_LST_DTS_LN83  
		LD_EFT_EFF_BEG  
		LD_EFT_EFF_END  
		LC_EFT_SUS_REA  
		LC_STA_LN83  
		LR_EFT_RDC  
		LI_EFT_RIR_MNL_OVR  
		LI_EFT_RST  
		LF_LST_USR_LN83  
		LF_LST_SRC_LN83  
/*		no filter*/
	);

PROC SQL;
    CONNECT TO ODBC AS SQL (REQUIRED="FILEDSN=X:\PADR\ODBC\&DB..dsn; update_lock_typ=nolock; bl_keepnulls=no");
    
    EXECUTE              
	    (
	        EXEC dbo.cast_SAS_timestamp_to_datetime2                                                 
	    ) BY SQL
    ;

    DISCONNECT FROM SQL;
QUIT;

%FINISH(SOURCE=LN83_EFT_TO_LON);
