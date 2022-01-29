
    OPTIONS
	   ERRORS  = 0
           MISSING = '0'
		LS = 96
		PS = 54
                ;
      		%LET RPTLIB = %SYSGET(reportdir);
			FILENAME REPORT2 "&RPTLIB/ULWF05.LWF05R2";
			
			DATA NULLRPT;
				STRING = "END OF REPORT";
				RUN;  
 dATA _NULL_;
    IF WEEKDAY(DATE()) = 1 THEN DO;
     CALL SYMPUT('END',"'"||PUT(INTNX('DAY',DATE(),-2),mmddyy10.)||"'");
      END;
      ELSE DO;
     CALL SYMPUT('END',"'"||PUT(INTNX('DAY',DATE(),-1),mmddyy10.)||"'");
      END;
     CALL SYMPUT('RUNDT',PUT(DATE(),MMDDYY10.));   
     
 PROC SQL;
 connect to db2(database=dlgsutwh);
   CREATE TABLE TABLE1 AS
   select *
   from connection to db2(
    select  b.af_apl_id
           ,b.af_apl_id_sfx
           ,b.lf_crt_dts_dc10
           ,b.bf_ssn
           ,B.BD_TRX_PST_HST
           ,B.LA_TRX
           ,B.LD_TRX_EFF
           ,b.lf_crt_dts_dc11
           ,c.lf_clm_id
 from olwhrm1.dc11_lon_fat a inner join
      olwhrm1.dc11_lon_fat b on
      a.af_apl_id = b.af_apl_id and
      a.af_apl_id_sfx = b.af_apl_id_sfx and
      a.lf_crt_dts_dc10 = b.lf_crt_dts_dc10 inner join
      olwhrm1.dc01_lon_clm_inf c on
       a.af_apl_id = c.af_apl_id and
       a.af_apl_id_sfx = c.af_apl_id_sfx and
       a.lf_crt_dts_dc10 = c.lf_crt_dts_dc10 and
       b.af_apl_id = c.af_apl_id and
       b.af_apl_id_sfx = c.af_apl_id_sfx and
       b.lf_crt_dts_dc10 = c.lf_crt_dts_dc10
WHERE a.BD_TRX_PST_HST = &end AND
	   a.LC_RCI_TYP = 'CR' and 
       b.lc_rci_typ = 'CR');
                 disconnect from db2;
quit;
 
proc sort;
 BY AF_APL_ID AF_APL_ID_SFX LF_CRT_DTS_DC10 lf_crt_dts_dc11;
  DATA TEST;
    SET table1;
     IF LAG(AF_APL_ID) = AF_APL_ID AND
        LAG(AF_APL_ID_SFX) = AF_APL_ID_SFX AND
        LAG(LF_CRT_DTS_DC10) = LF_CRT_DTS_DC10 AND
        LAG(LD_TRX_EFF) > LD_TRX_EFF;
proc sort;
by bf_ssn lf_clm_id;

		PROC PRINTTO PRINT = REPORT2;
			RUN;
    
PROC PRINT NOOBS SPLIT='/';
VAR BF_SSN lf_clm_id BD_TRX_PST_HST LD_TRX_EFF LA_TRX;
       LABEL BF_SSN = 'SS NUMBER'
             lf_clm_id = 'Claim ID'
             BD_TRX_PST_HST  = 'POSTING/DATE'
             LD_TRX_EFF = 'EFFECTIVE/DATE'
             LA_TRX = 'AMOUNT POSTED';
      FORMAT LD_TRX_EFF BD_TRX_PST_HST MMDDYY10.
             LA_TRX DOLLAR14.2;
      TITLE1 'PAYMENT POSTED OUT TO SEQUENCE';
      TITLE2 "&RUNDT";
           RUN;

	PROC PRINT DATA = NULLRPT;
		RUN;
