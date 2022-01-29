/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWGG9.LWGG9RZ";
FILENAME REPORT2 "&RPTLIB/ULWGG9.LWGG9R2";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

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
	CREATE TABLE REHAB AS
		SELECT DISTINCT
			DC01.BF_SSN AS DF_PRS_ID
		FROM 
			OLWHRM1.DC01_LON_CLM_INF DC01
			JOIN OLWHRM1.BR01_BR_CRF BR01
				ON DC01.BF_SSN = BR01.BF_SSN
			LEFT JOIN OLWHRM1.AY01_BR_ATY DWGRL
				ON DC01.BF_SSN = DWGRL.DF_PRS_ID
				AND DWGRL.PF_ACT = 'DWGRL'
			LEFT JOIN OLWHRM1.AY01_BR_ATY DWGRB
				ON DC01.BF_SSN = DWGRB.DF_PRS_ID
				AND DWGRB.PF_ACT = 'DWGRB'
				AND DWGRB.BD_ATY_PRF >= TODAY()-7
		WHERE
			DC01.LC_GRN IN ('02', '04', '05', '06', '07')
			AND BR01.BN_RHB_PAY_CTR = 4
			AND DWGRL.DF_PRS_ID IS NULL
			AND DWGRB.DF_PRS_ID IS NULL
		;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;
DATA REHAB; SET DUSTER.REHAB; RUN;


/*write to file for queue builder*/
DATA _NULL_;
	SET  REHAB; 
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;

	PUT DF_PRS_ID 'AWG5PMTS,,,,,Review account to see if borrower has made 5 voluntary payments for rehab and if eligible for AWG release.';
RUN;
