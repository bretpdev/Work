/***************************************************************/
/*     Set the following variables before running the job.     */
/***************************************************************/

/*delete the asterisk to uncomment the line of the desired region*/
*  %LET REGION = COM;				*commercial COMPASS/ONELINK production region;
*  %LET REGION = FED;				*federal COMPASS production region;
*  %LET REGION = VUK1;				*federal COMPASS VUK1 test region;
*  %LET REGION = VUK3;				*federal COMPASS VUK3 test region;

*enter the name of the table to dump;
%LET TABLE = DW01_DW_CLC_CLU;

*enter the name of the SSN field in the table to dump (e.g. BF_SSN, DF_PRS_ID);	
%LET ID_FIELD = BF_SSN;				

*enter account numbers or SSNs surrounded by single quotes and separated by commas;
%LET ID_LIST = '85529939930','52152542710';	

*delete the asterisk to uncomment the line below to include the entire table;
*%LET ID_LIST = ALL;

*change the value below if you want to limit the number of observations (i.e. you only want the first 1000 records);
%LET NO_OBS = 0;

/***************************************************************/
/*     Set the variables above before running the job.         */
/***************************************************************/
;


%MACRO DUMPIT();

/*set variables*/
	/*DUSTER*/
	%IF &REGION = COM %THEN
		%DO;
			%LET OWNER = OLWHRM1;
			%LET DB = DLGSUTWH;
			%LET SERVER = DUSTER;
		%END;

	/*LEGEND PROD*/
	%IF &REGION = FED %THEN
		%DO;
			%LET DB = DNFPUTDL;
			%LET OWNER = PKUB;
			%LET SERVER = LEGEND;
		%END;

	/*LEGEND TEST*/
	%IF &REGION = VUK1 OR &REGION = VUK3 %THEN
		%DO;
			%IF &REGION = VUK1 %THEN %LET DB = DNFPRQUT;
			%IF &REGION = VUK3 %THEN %LET DB = DNFPRUUT;
			%LET OWNER = PKUB;
			%LET SERVER = LEGEND;
		%END;

	/*where clause*/
	%LET WHERE_CLAUSE = WHERE A.&ID_FIELD IN (&ID_LIST) OR B.DF_SPE_ACC_ID IN (&ID_LIST);
	%IF &ID_LIST = ALL %THEN %LET WHERE_CLAUSE = ;

	/*inobs argument*/
	%LET INOBS_ARG = ;
	%IF &NO_OBS > 0 %THEN %LET INOBS_ARG = INOBS = &NO_OBS;


	LIBNAME  AES  REMOTE  SERVER = &SERVER SLIBREF = WORK;

/*pass variables and submit code to server*/
	%SYSLPUT TABLE = &TABLE;
	%SYSLPUT DB = &DB;
	%SYSLPUT OWNER = &OWNER;
	%SYSLPUT ID_FIELD = &ID_FIELD;
	%SYSLPUT ID_LIST = &ID_LIST;
	%SYSLPUT WHERE_CLAUSE = &WHERE_CLAUSE;
	%SYSLPUT INOBS_ARG = &INOBS_ARG;

	RSUBMIT &SERVER;

	PROC SQL &INOBS_ARG;
		CONNECT TO DB2 (DATABASE = &DB);

		CREATE TABLE &TABLE AS
			SELECT	
				*
			FROM	
				CONNECTION TO DB2 
					(
						SELECT	
							B.DF_SPE_ACC_ID,
							A.*
						FROM
							&OWNER..&TABLE A
							LEFT JOIN &OWNER..PD10_PRS_NME B
								ON A.&ID_FIELD = B.DF_PRS_ID
						&WHERE_CLAUSE
							
						FOR READ ONLY WITH UR
					)
		;

		DISCONNECT FROM DB2;
	QUIT;

	ENDRSUBMIT;

	PROC EXPORT
			DATA = AES.&TABLE
	    	OUTFILE= "T:\SAS\&TABLE Data Dump.xlsx" 
	    	DBMS = EXCEL2007
			REPLACE;
	RUN;

%MEND;
%DUMPIT;
