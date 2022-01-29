%LET MD = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\MD.dsn;");
/*%LET RPTLIB = X:\PADD\FTP;*/
LIBNAME DUDE ODBC USER="sqlserver;" PASSWORD="procauto;" &MD ;

/*In the set statement, put the source file */
/*you want a random sample taken from.  */
DATA SOURCESET(KEEP=DF_SPE_ACC_ID);
SET DUDE.BORROWER(KEEP=DF_SPE_ACC_ID la_cur_pri);
if la_cur_pri > 0;
RUN;

/*The row selection is random: each row has*/
/*an equal likelihood of being selected.*/
/*The "sampsize= " statement determines how*/
/*many rows will be selected.*/
DATA WORK.RSUBSET(KEEP=DF_SPE_ACC_ID);
SAMPSIZE=1000;
sampsize = min(sampsize,TOTOBS); *if the sample size is larger than number of observations, it will cause an infinite loop. This line prevents that from happening.;
OBSLEFT=TOTOBS;
DO WHILE(SAMPSIZE>0);
	PICKIT+1;
	IF RANUNI(0)<SAMPSIZE/OBSLEFT THEN DO;
		SET SOURCESET POINT=PICKIT NOBS=TOTOBS;
		OUTPUT;
		SAMPSIZE=SAMPSIZE-1;
	END;
	OBSLEFT=OBSLEFT-1;
END;
STOP;
RUN;

DATA _NULL_;
SET RSUBSET ;
FILE 'T:\SAS\RANDOM SAMPLE.TXT' DELIMITER=',' DSD DROPOVER LRECL=32767;
IF _N_ = 1 THEN DO;
	PUT "ACCOUNT NUMBER";
END;
DO;
   PUT DF_SPE_ACC_ID $ ;
END;
RUN;

