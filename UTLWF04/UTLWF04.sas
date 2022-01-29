OPTION ERRORS = 0
       MISSING = '0'
        LS      =96
        PS      =54
                ;
/*    %LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
     FILENAME REPORT2 "&RPTLIB/ULWF04.LWF04R2" ;

     DATA NULLRPT;
         STRING = "END OF REPORT";
         RUN;
            
DATA _NULL_;
	IF WEEKDAY(DATE()) = 1 THEN DO;
	CALL SYMPUT('END',"'"||put(INTNX('DAY',DATE(),-2),mmddyy10.)||"'");
	END;
	ELSE DO;
    CALL SYMPUT('END',"'"||put(INTNX('DAY',DATE(),-1),mmddyy10.)||"'");
	END;
	CALL SYMPUT('RUNDT',PUT(DATE(),MMDDYY10.));

RSUBMIT;
PROC SQL;
connect to db2(database=dlgsutwh);
CREATE TABLE TABLE2 AS
select *
from connection to db2(
SELECT     BF_SSN
          ,AF_APL_ID
          ,AF_APL_ID_SFX
          ,LF_CRT_DTS_DC11
          ,BD_TRX_PST_HST
          ,LC_TRX_TYP
          ,LA_TRX
          ,LD_TRX_EFF
          ,LF_USR_PST_TXN
FROM olwhrm1.DC11_LON_FAT
WHERE   LC_TRX_TYP IN ('RH','CN','RP','OV') AND
                    LD_TRX_ADJ = &end);
    disconnect from db2;
    QUIT;
ENDRSUBMIT;
DATA TABLE2;
SET WORKLOCL.TABLE2;
RUN;

PROC SORT;
 BY LC_TRX_TYP BF_SSN;

	proc printto print=report2 NEW;
		run; 
 	
PROC PRINT NOOBS SPLIT='/' data=table2;
   BY LC_TRX_TYP;
   VAR BF_SSN LD_TRX_EFF BD_TRX_PST_HST LA_TRX LF_USR_PST_TXN;
 LABEL BF_SSN = 'SS NUMBER'
       LC_TRX_TYP = 'TRANSACTION TYPE'
       LD_TRX_EFF = 'EFFECTIVE DATE'
       BD_TRX_PST_HST = 'POSTING DATE'
       LA_TRX = 'AMOUNT POSTED'
       LF_USR_PST_TXN = 'USER ID';
FORMAT LD_TRX_EFF BD_TRX_PST_HST MMDDYY10.
		LA_TRX DOLLAR14.2;
TITLE1 'SELECTED REVERSED PAYMENTS';
TITLE2 "&RUNDT";		
        RUN;
	PROC PRINT DATA=NULLRPT;
		RUN;

PROC PRINTTO;
RUN;
