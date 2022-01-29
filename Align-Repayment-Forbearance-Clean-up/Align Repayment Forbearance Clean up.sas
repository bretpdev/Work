*--------------------------------------*
| ALIGN REPAYMENT FORBEARANCE CLEAN UP |
*--------------------------------------*;
/*GET DATES FOR PROCESING*/
DATA _NULL_;
     CALL SYMPUT('_07_01_2006',INTNX('YEAR.7',TODAY(),0,'BEGINNING'));
	 CALL SYMPUT('TODAY',INTNX('DAY',TODAY(),0,'BEGINNING'));
     CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'),MMDDYYN8.));
RUN;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE ARFCU AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,A.LN_SEQ
	,B.LD_FOR_BEG
	,B.LD_FOR_END
	,B.LF_FOR_CTL_NUM
	,B.LN_FOR_OCC_SEQ
	,D.DM_PRS_1
	,D.DM_PRS_MID     
	,D.DM_PRS_LST 
	,E.DX_STR_ADR_1 
	,E.DX_STR_ADR_2
	,E.DC_DOM_ST
	,E.DM_CT
	,E.DF_ZIP_CDE     
	,E.DM_FGN_ST          
	,'MA2324' AS COST_CENTER_CODE
	,E.DC_ADR 
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.LN60_BR_FOR_APV B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.FB10_BR_FOR_REQ C
	ON B.BF_SSN = C.BF_SSN
	AND B.LF_FOR_CTL_NUM = C.LF_FOR_CTL_NUM
INNER JOIN OLWHRM1.PD10_PRS_NME D
	ON A.BF_SSN = D.DF_PRS_ID
LEFT OUTER JOIN OLWHRM1.PD30_PRS_ADR E
	ON A.BF_SSN = E.DF_PRS_ID
	AND E.DC_ADR = 'L'
	AND E.DI_VLD_ADR = 'Y'
WHERE A.IC_LON_PGM IN (
	'SUBCNS','UNCNS','SUBSPC',
	'UNSPC','PLUS','PLUSGB'
	)
AND A.LA_CUR_PRI > 0 
AND C.LC_FOR_TYP ='15'
AND C.LC_FOR_STA ='A'
AND B.LC_STA_LON60 ='A'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
DATA ARFCU;
SET WORKLOCL.ARFCU;
RUN;
DATA ARFCU;
SET ARFCU;
IF DC_DOM_ST IN ('','FC') THEN SVAR = 1;
ELSE SVAR = 2;
RUN;

%MACRO GTTXT(CRIT,RNO,FTYPE);
DATA TXTFL;
SET ARFCU;
WHERE &CRIT;
RUN;
%DO I=0 %TO 1;
	%IF &I = 0 %THEN 
		%LET RPTLIB = C:\WINDOWS\TEMP;
	%ELSE %IF &I = 1 %THEN 
		%LET RPTLIB = X:\ARCHIVE\SAS;
	FILENAME REPORT&RNO "&RPTLIB/AlignRepayForbCU.R&RNO..&RUNDATE..txt";
	%IF &FTYPE = S %THEN %DO;
	/***************************************************************
	* CREATE SHORT FILE
	****************************************************************/
		PROC SORT DATA=TXTFL;BY BF_SSN LN_SEQ LD_FOR_BEG LD_FOR_END;RUN;
		DATA _NULL_;
		SET  WORK.TXTFL;
		FILE REPORT&RNO DELIMITER=',' DSD DROPOVER LRECL=32767;
		   FORMAT BF_SSN $9. ;
		   FORMAT LN_SEQ 6. ;
		   FORMAT LD_FOR_BEG MMDDYY10. ;
		   FORMAT LD_FOR_END MMDDYY10. ;
		IF _N_ = 1 THEN       
		DO;
		   PUT
		   'BF_SSN'
		   ','
		   'LN_SEQ'
		   ','
		   'LD_FOR_BEG'
		   ','
		   'LD_FOR_END'
		   ;
		 END;
		 DO;
		   PUT BF_SSN $ @;
		   PUT LN_SEQ @;
		   PUT LD_FOR_BEG @;
		   PUT LD_FOR_END ;
		END;
		RUN;
	%END ;
	%ELSE %DO;
	/***************************************************************
	* CREATE LONG FILE
	****************************************************************/
		PROC SORT DATA=TXTFL;BY SVAR DC_DOM_ST BF_SSN LN_SEQ LD_FOR_BEG LD_FOR_END;RUN;
		DATA _NULL_;
		SET  WORK.TXTFL;
		FILE REPORT&RNO DELIMITER=',' DSD DROPOVER LRECL=32767;
		   FORMAT BF_SSN $9. ;
		   FORMAT LN_SEQ 6. ;
		   FORMAT LD_FOR_BEG MMDDYY10. ;
		   FORMAT LD_FOR_END MMDDYY10. ;
		   FORMAT DM_PRS_1 $13. ;
		   FORMAT DM_PRS_MID $13. ;
		   FORMAT DM_PRS_LST $23. ;
		   FORMAT DX_STR_ADR_1 $30. ;
		   FORMAT DX_STR_ADR_2 $30. ;
		   FORMAT DC_DOM_ST $2. ;
		   FORMAT DM_CT $20. ;
		   FORMAT DF_ZIP_CDE $17. ;
		   FORMAT DM_FGN_ST $15. ;
		   FORMAT COST_CENTER_CODE $6. ;
		   FORMAT LF_FOR_CTL_NUM $3. ;
		   FORMAT LN_FOR_OCC_SEQ 6.;
		IF _N_ = 1 THEN      
		 DO;
		   PUT
		   'BF_SSN'
		   ','
		   'LN_SEQ'
		   ','
		   'LD_FOR_BEG'
		   ','
		   'LD_FOR_END'
		   ','
		   'LF_FOR_CTL_NUM'
		   ','
		   'LN_FOR_OCC_SEQ'
		   ','
		   'DM_PRS_1'
		   ','
		   'DM_PRS_MID'
		   ','
		   'DM_PRS_LST'
		   ','
		   'DX_STR_ADR_1'
		   ','
		   'DX_STR_ADR_2'
		   ','
		   'DC_DOM_ST'
		   ','
		   'DM_CT'
		   ','
		   'DF_ZIP_CDE'
		   ','
		   'DM_FGN_ST'
		   ','
		   'STATE_IND'
		   ','
		   'COST_CENTER_CODE'
		   ;
		END;
		DO;
		   PUT BF_SSN $ @;
		   PUT LN_SEQ @;
		   PUT LD_FOR_BEG @;
		   PUT LD_FOR_END @;
		   PUT LF_FOR_CTL_NUM @;
		   PUT LN_FOR_OCC_SEQ @;
		   PUT DM_PRS_1 $ @;
		   PUT DM_PRS_MID $ @;
		   PUT DM_PRS_LST $ @;
		   PUT DX_STR_ADR_1 $ @;
		   PUT DX_STR_ADR_2 $ @;
		   PUT DC_DOM_ST $ @;
		   PUT DM_CT $ @;
		   PUT DF_ZIP_CDE $ @;
		   PUT DM_FGN_ST $ @;
		   PUT DC_DOM_ST $ @;
		   PUT COST_CENTER_CODE $ ;
		   ;
		END;
		RUN;
	%END;
%END;
%MEND GTTXT;
%GTTXT(LD_FOR_BEG LT &_07_01_2006 AND LD_FOR_END LT &_07_01_2006,2,S);
%GTTXT(LD_FOR_BEG LE &TODAY AND LD_FOR_END GT &TODAY AND DC_ADR EQ 'L',3,L);
%GTTXT(LD_FOR_END BETWEEN &_07_01_2006 AND &TODAY AND DC_ADR EQ 'L',4,L);
%GTTXT(LD_FOR_BEG GT &TODAY,5,S);
%GTTXT(LD_FOR_BEG LE &TODAY AND LD_FOR_END GT &TODAY AND DC_ADR NE 'L',6,S);
%GTTXT(LD_FOR_END BETWEEN &_07_01_2006 AND &TODAY AND DC_ADR NE 'L',7,S);
