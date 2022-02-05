*-------------------------------------------*
|UTLWK23 - SKIP ATTEMPT TO CONTACT ATTORNEY |
*-------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*%LET PROGREVW = /sas/whse/progrevw;*/
%LET RPTLIB = T:\SAS;
%LET PROGREVW = Q:\Support Services\Test Files\SAS\PROGREVW;


FILENAME REPORT2 "&RPTLIB/ULWK23.LWK23R2";
FILENAME REPORTZ "&RPTLIB/ULWK23.LWK23RZ";

LIBNAME PROGREVW "&PROGREVW";

PROC SQL NOPRINT;
	SELECT 
		"'"||STRIP(LENDER_ID)||"'" INTO :EXCL_LIST SEPARATED BY ","
	FROM 
		PROGREVW.LENDER_GROUP_LENDERS
	WHERE 
		LENDER_GROUP_ID = 1
	;
QUIT;
%PUT &EXCL_LIST;

DATA _NULL_;
     CALL SYMPUT('DTE',"'"||PUT(INTNX('DAY',TODAY(),-30,'BEGINNING'), MMDDYYD10.)||"'");
RUN;

%SYSLPUT EXCL_LIST = &EXCL_LIST;
%SYSLPUT DTE = &DTE;
LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LENDER_ID)||"'"
		INTO :UHEAA_LIST SEPARATED BY ","
	FROM SAS_TAB.LDR_AFF
	WHERE AFFILIATION = 'UHEAA';
QUIT;
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DATCA AS
		SELECT 
			*
		FROM CONNECTION TO DB2 
			(
				SELECT DISTINCT 
					B.BF_SSN
					,B.BF_RFR
					,'BWNHDGF' AS ACS_CODE
					,CASE 
						WHEN G.DM_PRS_MID = '' THEN RTRIM(G.DM_PRS_1)||' '||G.DM_PRS_LST
						ELSE RTRIM(G.DM_PRS_1)||' '||RTRIM(G.DM_PRS_MID)||' '||G.DM_PRS_LST
					 END AS RNAME
					,G.DX_STR_ADR_1
					,G.DX_STR_ADR_2
					,G.DM_CT
					,CASE
						WHEN G.DC_DOM_ST = 'FC' THEN ''
						ELSE G.DC_DOM_ST
					 END AS DC_DOM_ST
					,G.DF_ZIP_CDE
					,G.DM_FGN_CNY
					,D.LF_STU_SSN AS STU_SSN
					,CASE 
						WHEN F.DM_PRS_MID = '' THEN RTRIM(F.DM_PRS_1)||' '||F.DM_PRS_LST
						ELSE RTRIM(F.DM_PRS_1)||' '||RTRIM(F.DM_PRS_MID)||' '||F.DM_PRS_LST
					 END AS BNAME
					,F.DF_SPE_ACC_ID
					,E.BKY_CASE
					,F.B_DX_STR_ADR_1
					,F.B_DX_STR_ADR_2
					,F.B_DM_CT
					,F.B_DC_DOM_ST
					,F.B_DF_ZIP_CDE
					,F.B_DM_FGN_CNY
					,F.DN_PHN
					,F.B_DC_DOM_ST AS STATE_IND
					,CASE 
						WHEN D.LF_LON_CUR_OWN = '1' THEN 'MA2324'
						ELSE 'MA2327'
					END AS COST_CENTER_CODE
					,F.DC_ADR
					,CASE 
						WHEN G.DM_FGN_CNY <> '' THEN 1
						ELSE 2
					 END AS SVAR
				FROM 
					OLWHRM1.RF10_RFR B
					INNER JOIN 
						(
							SELECT DISTINCT 
								DF_PRS_ID AS BF_SSN
							FROM 
								OLWHRM1.PD30_PRS_ADR 
							WHERE 
								DC_ADR = 'L'
								AND DI_VLD_ADR = 'N'

							UNION

							SELECT DISTINCT 
								DF_PRS_ID AS BF_SSN
							FROM 
								OLWHRM1.PD42_PRS_PHN 
							WHERE 
								DI_PHN_VLD = 'N'
								AND DC_PHN = 'H'
						) A
						ON B.BF_SSN = A.BF_SSN
					INNER JOIN 
						(
							SELECT DISTINCT 
								BF_SSN
								,LF_STU_SSN
								,CASE 
									WHEN LF_LON_CUR_OWN IN (&UHEAA_LIST) THEN '1'
									ELSE ''
								END AS LF_LON_CUR_OWN
							FROM 
								OLWHRM1.LN10_LON
							WHERE 
								LA_CUR_PRI > 0
								AND LC_STA_LON10 = 'R'
								AND LF_LON_CUR_OWN NOT IN (&EXCL_LIST)
						) D
						ON B.BF_SSN = D.BF_SSN
					INNER JOIN 
						(
							SELECT DISTINCT 
								DF_PRS_ID AS BF_SSN
								,DD_BKR_STA
								,DF_COU_DKT AS BKY_CASE
								,DF_ATT 
							FROM 
								OLWHRM1.PD24_PRS_BKR 
							WHERE 
								DF_COU_DKT <> ''
						) E
						ON B.BF_SSN = E.BF_SSN
						AND B.BF_RFR = E.DF_ATT
					LEFT OUTER JOIN 
						(
							SELECT DISTINCT 
								Z.DF_PRS_ID AS BF_SSN 
								,Z.DF_SPE_ACC_ID 
								,Z.DM_PRS_MID
								,Z.DM_PRS_1
								,Z.DM_PRS_LST
								,Y.DX_STR_ADR_1	AS B_DX_STR_ADR_1
								,Y.DX_STR_ADR_2	AS B_DX_STR_ADR_2
								,Y.DM_CT AS B_DM_CT
								,Y.DC_DOM_ST AS B_DC_DOM_ST
								,Y.DF_ZIP_CDE AS B_DF_ZIP_CDE
								,Y.DM_FGN_CNY AS B_DM_FGN_CNY
								,Y.DC_ADR
								,W.DN_PHN
							FROM 
								OLWHRM1.PD10_PRS_NME Z
							INNER JOIN 
								(
									SELECT DISTINCT 
										DF_PRS_ID AS BF_SSN
										,DC_ADR
										,DX_STR_ADR_1
										,DX_STR_ADR_2
										,DM_CT
										,DC_DOM_ST
										,DF_ZIP_CDE
										,DM_FGN_CNY
									FROM 
										OLWHRM1.PD30_PRS_ADR 
									WHERE 
										DC_ADR = 'L'
								) Y
								ON Z.DF_PRS_ID = Y.BF_SSN
							INNER JOIN 
								(
									SELECT DISTINCT 
										DF_PRS_ID AS BF_SSN
										,DN_DOM_PHN_ARA || DN_DOM_PHN_XCH || DN_DOM_PHN_LCL AS DN_PHN
									FROM 
										OLWHRM1.PD42_PRS_PHN
									WHERE DC_PHN = 'H'
								) W
								ON Z.DF_PRS_ID = W.BF_SSN
						) F
						ON B.BF_SSN = F.BF_SSN
					LEFT OUTER JOIN
						(
							SELECT DISTINCT 
								V.DF_PRS_ID AS BF_SSN 
								,V.DM_PRS_MID
								,V.DM_PRS_1
								,V.DM_PRS_LST
								,U.DX_STR_ADR_1
								,U.DX_STR_ADR_2
								,U.DM_CT
								,U.DC_DOM_ST
								,U.DF_ZIP_CDE
								,U.DM_FGN_CNY
							FROM 
								OLWHRM1.PD10_PRS_NME V
								INNER JOIN 
									(
										SELECT DISTINCT 
											DF_PRS_ID AS BF_SSN
											,DX_STR_ADR_1
											,DX_STR_ADR_2
											,DM_CT
											,DC_DOM_ST
											,DF_ZIP_CDE
											,DM_FGN_CNY
										FROM OLWHRM1.PD30_PRS_ADR 
									) U
								ON V.DF_PRS_ID = U.BF_SSN
						) G
						ON B.BF_RFR = G.BF_SSN 	
					WHERE 
						B.BC_STA_REFR10 = 'A'
						AND B.BC_RFR_REL_BR = '15'
						AND E.DD_BKR_STA = 
							(
								SELECT 
									MAX(Z.DD_BKR_STA)
								FROM 
									OLWHRM1.PD24_PRS_BKR Z
								WHERE 
									Z.DF_PRS_ID = E.BF_SSN
							) 
						AND NOT EXISTS 
							(
								SELECT 
									*
								FROM 
									OLWHRM1.WQ20_TSK_QUE C
								WHERE  
									B.BF_SSN = C.BF_SSN
									AND C.WF_QUE = 'KA'
									AND WF_SUB_QUE = '01'
							)
						AND NOT EXISTS 
							(
						    	SELECT 
									1
						      	FROM 
									OLWHRM1.AY10_BR_LON_ATY X
						      	WHERE 
									X.BF_SSN = A.BF_SSN
						            AND X.PF_REQ_ACT = 'KATNY'
						            AND X.LD_ATY_REQ_RCV > &DTE
							)
				ORDER BY 
					DF_SPE_ACC_ID
					,COST_CENTER_CODE

			FOR READ ONLY WITH UR
		)
	;

	DISCONNECT FROM DB2;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK (SQLRPT=ULWK23.LWK23RZ);*/
	/*QUIT;*/
ENDRSUBMIT;

DATA DATCA;
	SET WORKLOCL.DATCA;
	BY DF_SPE_ACC_ID;
	IF FIRST.DF_SPE_ACC_ID;
RUN;
PROC SORT DATA=DATCA;
	BY SVAR DC_DOM_ST;
RUN;

DATA DATCA (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET DATCA;
KEYSSN = TRANSLATE(BF_SSN,'MYLAUGHTER','0987654321');
MODAY = PUT(DATE(),MMDDYYN4.);
KEYLINE = "P"||KEYSSN||MODAY||DC_ADR;
CHKDIG = 0;
LENGTH DIG $2.;
DO I = 1 TO LENGTH(KEYLINE);
	IF I/2 NE ROUND(I/2,1) 
		THEN DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4) * 2, 2.);
	ELSE DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4), 2.);
	IF SUBSTR(DIG,1,1) = " " 
		THEN CHKDIG = CHKDIG + INPUT(SUBSTR(DIG,2,1),1.);
		ELSE DO;
			CHK1 = INPUT(SUBSTR(DIG,1,1),1.);
			CHK2 = INPUT(SUBSTR(DIG,2,1),1.);
			IF CHK1 + CHK2 >= 10
				THEN DO;
					CHK3 = PUT(CHK1 + CHK2,2.);
					CHK1 = INPUT(SUBSTR(CHK3,1,1),1.);
					CHK2 = INPUT(SUBSTR(CHK3,2,1),1.);
				END;
			CHKDIG = CHKDIG + CHK1 + CHK2;
		END;
END;
CHKDIGIT = 10 - INPUT(SUBSTR((RIGHT(PUT(CHKDIG,3.))),3,1),3.);
IF CHKDIGIT = 10 THEN CHKDIGIT = 0;
CHECK = PUT(CHKDIGIT,1.);
ACSKEY = "#"||KEYLINE||CHECK||"#";
RUN;

PROC SORT DATA=DATCA;
BY COST_CENTER_CODE	SVAR DC_DOM_ST;
RUN;

DATA _NULL_;
SET  WORK.DATCA;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT BF_SSN $9. ;
   FORMAT BF_RFR $9. ;
   FORMAT STU_SSN $9. ;
   FORMAT BKY_CASE $15. ;
   FORMAT B_DX_STR_ADR_1 $35. ;
   FORMAT B_DX_STR_ADR_2 $35. ;
   FORMAT B_DC_DOM_ST $2. ;
   FORMAT B_DF_ZIP_CDE $14. ;
   FORMAT B_DM_FGN_CNY $25. ;
   FORMAT DN_PHN $17. ;
   FORMAT DX_STR_ADR_1 $30. ;
   FORMAT DX_STR_ADR_2 $30. ;
   FORMAT DC_DOM_ST $2. ;
   FORMAT DF_ZIP_CDE $17. ;
   FORMAT DM_FGN_CNY $25. ;
   FORMAT DC_ADR $1. ;
   FORMAT ACS_CODE $7. ;
   FORMAT COST_CENTER_CODE $6. ;
   FORMAT SVAR 11. ;
   FORMAT ACSKEY $18. ;
   FORMAT DM_CT $20.;
   FORMAT B_DM_CT $30.;
IF _N_ = 1 THEN DO;
PUT  'BF_SSN'
	','
	'BF_RFR'
	','
	'ACS_CODE' 
	','
	'ACSKEY' 
	','
	'RNAME' 
	','
	'DX_STR_ADR_1'
	','
	'DX_STR_ADR_2'
	','
	'DM_CT'
	','
	'DC_DOM_ST'
	','
	'DF_ZIP_CDE'
	','
	'DM_FGN_CNY'
	','
	'STU_SSN' 
	','
	'BNAME'
	','
	'DF_SPE_ACC_ID'
	','
	'BKY_CASE'
	','
	'B_DX_STR_ADR_1'
	','
	'B_DX_STR_ADR_2'
	','
	'B_DM_CT' 
	','
	'B_DC_DOM_ST' 
	','
	'B_DF_ZIP_CDE' 
	','
	'B_DM_FGN_CNY' 
	','
	'DN_PHN' 
	','
	'STATE_IND'
	','
	'COST_CENTER_CODE'
	;
END;
DO;
PUT BF_SSN $ @;
PUT BF_RFR $ @;
PUT ACS_CODE $ @;
PUT ACSKEY $ @;
PUT RNAME $ @;
PUT DX_STR_ADR_1 $ @;
PUT DX_STR_ADR_2 $ @;
PUT DM_CT $ @;
PUT DC_DOM_ST $ @;
PUT DF_ZIP_CDE $ @;
PUT DM_FGN_CNY $ @;
PUT STU_SSN $ @;
PUT BNAME $ @;
PUT DF_SPE_ACC_ID $ @;
PUT BKY_CASE $ @;
PUT B_DX_STR_ADR_1 $ @;
PUT B_DX_STR_ADR_2 $ @;
PUT B_DM_CT $ @;
PUT B_DC_DOM_ST $ @;
PUT B_DF_ZIP_CDE $ @;
PUT B_DM_FGN_CNY $ @;
PUT DN_PHN $ @;
PUT DC_DOM_ST $ @;
PUT COST_CENTER_CODE $ ;
END;
RUN;
