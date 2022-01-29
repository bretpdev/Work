/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWABX.LWABXRZ";
FILENAME REPORTX "&RPTLIB/ULWABX.LWABXRX";

%LET BEGIN 	= 'XX/XX/XXXX';
%LET END 	= 'XX/XX/XXXX';
%SYSLPUT BEGIN = &begin;
%SYSLPUT END = &end;	

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND  SLIBREF=WORK;
RSUBMIT;
/*LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;*/

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

PROC SQL;
CONNECT TO DBX (DATABASE=DNFPUTDL);
CREATE TABLE BNKR AS
SELECT *
FROM CONNECTION TO DBX (
SELECT DISTINCT A.DF_SPE_ACC_ID
				,B.BF_SSN
FROM	PKUB.PDXX_PRS_NME A
INNER JOIN PKUB.LNXX_LON B
	ON A.DF_PRS_ID = B.BF_SSN
INNER JOIN PKUB.DWXX_DW_CLC_CLU C
	ON B.BF_SSN = C.BF_SSN
	AND B.LN_SEQ = C.LN_SEQ
inner join pkub.PDXX_PRS_BKR pdXX
	on a.df_prs_id = pdXX.df_prs_id
WHERE	C.WC_DW_LON_STA = 'XX'
	AND B.LA_CUR_PRI > X
	and pdXX.DD_BKR_FIL between &begin and &end
FOR READ ONLY WITH UR
);
DISCONNECT FROM DBX;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
/*QUIT;*/

ENDRSUBMIT;

DATA BNKR; SET LEGEND.BNKR; RUN;

PROC EXPORT DATA= WORK.BNKR
            OUTFILE= "T:\SAS\Fed Bankruptcies.&SYSDATE..xlsx" 
            DBMS=EXCEL REPLACE;
     SHEET="A"; 
RUN;
