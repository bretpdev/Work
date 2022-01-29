/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT	
						DISTINCT
						A.WF_QUE
						,A.WF_SUB_QUE
						,WN_CTL_TSK
						,PF_REQ_ACT
					FROM
						PKUB.WQXX_TSK_QUE A
					WHERE
						A.WF_QUE = 'RX'
						AND
						A.WF_SUB_QUE = 'XX'
/*						AND*/
/*						A.WD_ACT_REQ >= 'XX/XX/XXXX'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;


/*write to comma delimited file*/
DATA _NULL_;
	SET		WORK.DEMO;
	FILE
		'T:\SAS\NH XXXX.txt'
		DELIMITER = ','
		DSD
		DROPOVER
		LRECL = XXXXX
	;
	/* write data*/	
	DO;
		PUT WF_QUE $ @;
		PUT WF_SUB_QUE $ @;
		PUT WN_CTL_TSK $ @;
		PUT PF_REQ_ACT;
		;
	END;
RUN;
