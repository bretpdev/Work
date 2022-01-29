/***************************************************************/
/*     Set the following variables before running the job.     */
/***************************************************************/

/*delete the asterisk to uncomment the line of the desired region*/
*  %LET REGION = COM;				*commercial COMPASS/ONELINK production region;
  %LET REGION = FED;				*federal COMPASS production region;
*  %LET REGION = VUKX;				*federal COMPASS VUKX test region;
*  %LET REGION = VUKX;				*federal COMPASS VUKX test region;

*enter the name of the table to dump;
%LET TABLE = RSXX_IBR_RPS;

*enter the name of the SSN field in the table to dump (e.g. BF_SSN, DF_PRS_ID);	
%LET ID_FIELD = BF_SSN;				

*enter account numbers or SSNs surrounded by single quotes and separated by commas;
%LET ID_LIST = 'XXXXXXXXXX',
'XXXXXXXXXX',
'XXXXXXXXXX';	

*delete the asterisk to uncomment the line below to include the entire table;
*%LET ID_LIST = ALL;

*change the value below if you want to limit the number of observations (i.e. you only want the first XXXX records);
%LET NO_OBS = X;

/***************************************************************/
/*     Set the variables above before running the job.         */
/***************************************************************/
;


%MACRO DUMPIT();

/*set variables*/
	/*DUSTER*/
	%IF &REGION = COM %THEN
		%DO;
			%LET OWNER = OLWHRMX;
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
	%IF &REGION = VUKX OR &REGION = VUKX %THEN
		%DO;
			%IF &REGION = VUKX %THEN %LET DB = DNFPRQUT;
			%IF &REGION = VUKX %THEN %LET DB = DNFPRUUT;
			%LET OWNER = PKUB;
			%LET SERVER = LEGEND;
		%END;

	/*where clause*/
	%LET WHERE_CLAUSE = WHERE A.&ID_FIELD IN (&ID_LIST) OR B.DF_SPE_ACC_ID IN (&ID_LIST);
	%IF &ID_LIST = ALL %THEN %LET WHERE_CLAUSE = ;

	/*inobs argument*/
	%LET INOBS_ARG = ;
	%IF &NO_OBS > X %THEN %LET INOBS_ARG = INOBS = &NO_OBS;


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
		CONNECT TO DBX (DATABASE = &DB);

		CREATE TABLE &TABLE AS
			SELECT	
				*
			FROM	
				CONNECTION TO DBX 
					(
						SELECT	
							B.DF_SPE_ACC_ID,
							A.*
						FROM
							&OWNER..&TABLE A
							LEFT JOIN &OWNER..PDXX_PRS_NME B
								ON A.&ID_FIELD = B.DF_PRS_ID
						&WHERE_CLAUSE
							
						FOR READ ONLY WITH UR
					)
		;

		DISCONNECT FROM DBX;
	QUIT;

	ENDRSUBMIT;

	PROC EXPORT
			DATA = AES.&TABLE
	    	OUTFILE= "T:\SAS\&TABLE Data Dump.xlsx" 
	    	DBMS = EXCELXXXX
			REPLACE;
	RUN;

%MEND;
%DUMPIT;
