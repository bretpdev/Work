/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWS36.NWS36RZ";
FILENAME REPORT2 "&RPTLIB/UNWS36.NWS36R2";
FILENAME REPORT3 "&RPTLIB/UNWS36.NWS36R3";
FILENAME REPORT4 "&RPTLIB/UNWS36.NWS36R4";
FILENAME REPORT5 "&RPTLIB/UNWS36.NWS36R5";
FILENAME REPORT6 "&RPTLIB/UNWS36.NWS36R6";
FILENAME REPORT7 "&RPTLIB/UNWS36.NWS36R7";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE S36 AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT DISTINCT
						CARC.BF_SSN,
						PARC.LD_ATY_REQ_RCV,
						CARC.PF_REQ_ACT
					FROM
						PKUB.AY10_BR_LON_ATY CARC
						JOIN PKUB.AY10_BR_LON_ATY PARC
							ON CARC.BF_SSN = PARC.BF_SSN
					WHERE
						CARC.PF_REQ_ACT IN ('ED180', 'ED270', 'ED360', 'LRGED', 'FINED', 'GRCED')
						AND PARC.PF_REQ_ACT = 'P200C'
						AND 
							(
								CARC.LD_ATY_RSP < PARC.LD_ATY_RSP
								OR
								(CARC.LD_ATY_RSP = PARC.LD_ATY_RSP AND CARC.LT_ATY_RSP < PARC.LT_ATY_RSP)
							)
						AND CARC.LC_STA_ACTY10 = 'A'
						AND PARC.LC_STA_ACTY10 = 'A'
					ORDER BY 
						BF_SSN

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA S36; SET LEGEND.S36; RUN;

/*export to comman delimited text file*/
%MACRO PRINTREPORT(RNO,ARC);
	DATA _NULL_;
		SET S36 ;
		FILE REPORT&RNO DELIMITER=',' DSD DROPOVER LRECL=32767;
		WHERE PF_REQ_ACT = "&ARC";
		FORMAT LD_ATY_REQ_RCV MMDDYY10.;

		IF _N_ = 1 THEN
			DO;
				PUT "Borrower SSN,Contact Date";
			END;

		DO;
		   PUT BF_SSN @;
		   PUT LD_ATY_REQ_RCV $ ;
		END;
	RUN;
%MEND;

%PRINTREPORT(2,ED180);
%PRINTREPORT(3,ED270);
%PRINTREPORT(4,ED360);
%PRINTREPORT(5,LRGED);
%PRINTREPORT(6,FINED);
%PRINTREPORT(7,GRCED);
