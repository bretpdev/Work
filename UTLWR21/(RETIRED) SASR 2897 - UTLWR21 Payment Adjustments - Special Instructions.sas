/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWR21.LWR21RZ";
FILENAME REPORT2 "&RPTLIB/ULWR21.LWR21R2";
FILENAME REPORT3 "&RPTLIB/ULWR21.LWR21R3";
FILENAME REPORT4 "&RPTLIB/ULWR21.LWR21R4";
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

*Lines 11-43 determine the last business day.;
data _null_;
runday = today();
/*runday = '31jan2011'd;*/
bus_day = 0;

array all_hol{12} ;
do until (bus_day = 1);
	do until(weekday(runday) in (2,3,4,5,6));
		runday = runday - 1;
	end;
	all_hol(1) = holiday('christmas',year(runday)) ;
	all_hol(2) = holiday('thanksgiving',year(runday)) + 1 ;	*Black Friday;
	all_hol(3) = holiday('thanksgiving',year(runday)) ;
	all_hol(4) = holiday('labor',year(runday)) ;
	all_hol(5) = holiday('usindependence',year(runday)) + 20 ; *Pioneer Day;
	all_hol(6) = holiday('usindependence',year(runday)) ;
	all_hol(7) = holiday('memorial',year(runday)) ;
	all_hol(8) = holiday('mlk',year(runday)) ;
	all_hol(9) = holiday('uspresidents',year(runday)) ;
	all_hol(10) = holiday('newyear',year(runday)) ;
	all_hol(11) = holiday('columbus',year(runday)) ; *not UHEAA;
	all_hol(12) = holiday('veterans',year(runday)) ; *not UHEAA;
	do a = 1,2;
		do i = 1 to 12 ;
			if weekday(runday) = 2 and runday = all_hol(i) + 1 then runday = runday - 1;
			if runday = all_hol(i) then runday = runday - 1;
			if weekday(runday) = 6 and runday = all_hol(i) - 1 then runday = runday - 1;
		end;
	end;
	if weekday(runday) in (2,3,4,5,6) then bus_day + 1;
end;
call symput('BUS_DAYS_AGO_1',"'" || put(runday,date9.)|| "'d");
run;
/*In production we will use one week, but for test current - 30*/

%SYSLPUT BUS_DAYS_AGO_1 = &BUS_DAYS_AGO_1;

RSUBMIT;
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
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
	FROM CONNECTION TO DB2 (
		SELECT DISTINCT A.BF_SSN	
			,A.LN_SEQ 
			,ABS(COALESCE(A.LA_FAT_CUR_PRI,0) + COALESCE(A.LA_FAT_NSI,0) + COALESCE(A.LA_FAT_LTE_FEE,0)) AS AMOUNT
			,A.LD_FAT_EFF
			,A.LD_FAT_APL
			,E.PF_REQ_ACT
			,A.PC_FAT_TYP ||A.PC_FAT_SUB_TYP AS TYPE
		FROM	OLWHRM1.LN90_FIN_ATY A 	
			INNER JOIN OLWHRM1.LN10_LON B
				ON A.BF_SSN=B.BF_SSN	
				AND A.LN_SEQ = B.LN_SEQ
			INNER JOIN OLWHRM1.LN85_LON_ATY C
				ON A.BF_SSN = C.BF_SSN
				AND A.LN_SEQ = C.LN_SEQ
			INNER JOIN OLWHRM1.AY10_BR_LON_ATY E
				ON C.BF_SSN=E.BF_SSN
				AND C.LN_ATY_SEQ = E.LN_ATY_SEQ 
			LEFT OUTER JOIN OLWHRM1.AY10_BR_LON_ATY D
				ON A.BF_SSN = D.BF_SSN
				AND D.LD_ATY_REQ_RCV = A.LD_FAT_APL
				AND D.LC_STA_ACTY10 = 'A'
				AND ((E.PF_REQ_ACT ='APPMT' AND D.PF_REQ_ACT = 'PMTAD')
					OR (E.PF_REQ_ACT ='APMIL' AND D.PF_REQ_ACT = 'PMTML')
					OR (E.PF_REQ_ACT ='APAMC' AND D.PF_REQ_ACT = 'PMTAM'))
		WHERE A.LC_STA_LON90 = 'A' 
			AND	A.LC_FAT_REV_REA = ''
			AND	A.LC_CSH_ADV = 'C'
			AND DAYS(A.LD_FAT_EFF) > DAYS(CURRENT DATE)- 30
/*			AND	DATE(A.LD_FAT_EFF) >= DATE(&BUS_DAYS_AGO_1)*/
			AND B.LA_CUR_PRI > 0
			AND B.LC_STA_LON10 = 'R'
			AND E.LC_STA_ACTY10 = 'A'
			AND D.BF_SSN IS NULL
			AND A.PC_FAT_TYP = '10'
			AND ((E.PF_REQ_ACT = 'APPMT' AND A.PC_FAT_SUB_TYP IN ('10','11','12','41'))
				OR (E.PF_REQ_ACT = 'APMIL' AND A.PC_FAT_SUB_TYP = '35')
				OR (E.PF_REQ_ACT = 'APAMC' AND A.PC_FAT_SUB_TYP IN ('20','21','22','36','37')))
FOR READ ONLY WITH UR
);
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK;
QUIT;

PROC SORT DATA=DEMO; BY PF_REQ_ACT BF_SSN LD_FAT_EFF LN_SEQ; RUN;

DATA DEMO (DROP=LN_SEQ A);
SET DEMO END=LAST;
LENGTH SEQLIST $200. ;
BY PF_REQ_ACT BF_SSN LD_FAT_EFF LN_SEQ;
RETAIN SEQLIST A;
IF FIRST.LD_FAT_EFF THEN DO;
	SEQLIST = LEFT(PUT(LN_SEQ,2.));
	A = 0;
END;
ELSE IF FIRST.LN_SEQ THEN DO;
	SEQLIST = CATX(',',TRIM(SEQLIST),LEFT(PUT(LN_SEQ,2.)));
END;

A = A + AMOUNT;

IF LAST.LD_FAT_EFF THEN DO;
	AMOUNT = A;
	OUTPUT ;
END;
RUN;

ENDRSUBMIT;
DATA DEMO; SET DUSTER.DEMO; RUN;

PROC SORT DATA=DEMO; BY BF_SSN ; RUN;

%MACRO REPORTS(NUM,GRP);
DATA _NULL_;
SET DEMO ;
WHERE PF_REQ_ACT = &GRP;
FILE REPORT&NUM DELIMITER=',' DSD DROPOVER LRECL=32767;

LENGTH COMMENTS $600.;
COMMENTS = 
	'SEE INSTRUCTION TO REAPPLY PAYMENT IN ARC APPMT. PAYMENT EFFECT DATE = ' || PUT(LD_FAT_EFF, MMDDYY10.) 
	|| ', AMOUNT = $ ' || TRIM(LEFT(AMOUNT)) 
	||'; TYPE = ' || TYPE
;

IF _N_ = 1 THEN PUT "TARGET ID,ARC NAME,FROM DATE,TO DATE,NEEDED BY DATE,RECIPIENT ID,REGARDS TO CODE,REGARDS TO ID,LOAN SEQUENCE NUMBER(S),COMMENTS";
PUT BF_SSN 'PMTAJ,,,,' BF_SSN BF_SSN BF_SSN SEQLIST COMMENTS;

RUN; 
%MEND;

%REPORTS(2,'APPMT');
%REPORTS(3,'APMIL');
%REPORTS(4,'APAMC');
