/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

PROC IMPORT 
	DATAFILE="X:\ARCHIVE\BANA\PRODUCTION FILES\SUPPLEMENTAL FILES\J00BX_829769_INCENTIVE_SC370R5_2016020301130493_UO.CSV"
	OUT=CSV1 DBMS=CSV REPLACE;
	GETNAMES=YES;
RUN;
PROC IMPORT 
	DATAFILE="X:\Archive\BANA\Production Files\Supplemental Files\J00BX_829769_INCENTIVE_SC370R5_2016020303193552_UU.CSV"
	OUT=CSV2 DBMS=CSV REPLACE;
	GETNAMES=YES;
RUN;
PROC IMPORT 
	DATAFILE="X:\Archive\BANA\Production Files Wave 2\Supplemental Files\J00BX_829769_INCENTIVE_SC370R5_2016021600123053_UO.CSV"
	OUT=CSV3 DBMS=CSV REPLACE;
	GETNAMES=YES;
RUN;
PROC IMPORT 
	DATAFILE="X:\Archive\BANA\Production Files Wave 2\Supplemental Files\J00BX_829769_INCENTIVE_SC370R5_2016021604181866_UU.CSV"
	OUT=CSV4 DBMS=CSV REPLACE;
	GETNAMES=YES;
RUN;
PROC IMPORT 
	DATAFILE="X:\Archive\BANA\Production Files Wave 3\Supplemental Files\J00BX_829769_INCENTIVE_SC370R5_2016030520224383_UO.CSV"
	OUT=CSV5 DBMS=CSV REPLACE;
	GETNAMES=YES;
RUN;
PROC IMPORT 
	DATAFILE="X:\Archive\BANA\Production Files Wave 3\Supplemental Files\J00BX_829769_INCENTIVE_SC370R5_2016030601054788_UU.CSV"
	OUT=CSV6 DBMS=CSV REPLACE;
	GETNAMES=YES;
RUN;
PROC IMPORT 
	DATAFILE="X:\Archive\BANA\Production Files Wave 3\Supplemental Files\J00BX_829769_INCENTIVE_SC370R5_2016030601371382_UO.CSV"
	OUT=CSV7 DBMS=CSV REPLACE;
	GETNAMES=YES;
RUN;
PROC IMPORT 
	DATAFILE="X:\Archive\BANA\Production Files Wave 3\Supplemental Files\J00BX_829769_INCENTIVE_SC370R5_2016030601375610_UU.CSV"
	OUT=CSV8 DBMS=CSV REPLACE;
	GETNAMES=YES;
RUN;

DATA CSVSOURCE;
	SET CSV4 CSV7 CSV1 CSV2 CSV3 CSV5 CSV6 CSV8;
/*	COMMONLINE_UNIQUE_ID = TRIM(COALESCEC(SUBSTR(COMMONLINE_UNIQUE_ID,2),'000'));*/
	COMMONLINE_UNIQUE_ID = TRIM(SUBSTR(COMMONLINE_UNIQUE_ID,2));
	LON_IDENT = TRIM(SUBSTR(LOAN_IDENT,2));
	SELECT (BORROWER_BENEFIT_CODE);
		WHEN (0290,0835) UHEAA_CODE =  "BI1";
		WHEN (1560,1565,1717) UHEAA_CODE =  "BI2";
		WHEN (0007,1716) UHEAA_CODE =  "BI3";
		WHEN (1000,1520,4400,5101,6742,6749) UHEAA_CODE =  "BI4";
		WHEN (1570,1580,6720) UHEAA_CODE =  "BI5";
		WHEN (1706) UHEAA_CODE =  "BI6";
		WHEN (0830,1715) UHEAA_CODE =  "BI7";
		WHEN (2585,2590) UHEAA_CODE =  "BI8";
		WHEN (6740,6741) UHEAA_CODE =  "BI9";
		WHEN (6744,6745) UHEAA_CODE =  "BIA";
		WHEN (6746,6747) UHEAA_CODE =  "BIB";
		WHEN (1710) UHEAA_CODE =  "BIC";
		WHEN (0006,1020) UHEAA_CODE =  "BID";
		WHEN (0230,0800,1010,1530) UHEAA_CODE =  "BIE";
		WHEN (2740,6748) UHEAA_CODE =  "BR1";
		WHEN (2204,2205,2206,2207,2208,2550,2552,2555,2560,2565) UHEAA_CODE =  "BT1";
		WHEN (2214,2215,2216,2217,2530,2533,2535,2540,2570,2573,2575,2580,7722,7723,7724,7725) UHEAA_CODE =  "BT2";
		WHEN (3330,3335,3340) UHEAA_CODE =  "BT3";
	OTHERWISE UHEAA_CODE = "";
	END;
RUN;

PROC DATASETS NOLIST;
	DELETE CSV4 CSV7 CSV1 CSV2 CSV3 CSV5 CSV6 CSV8;
QUIT;

/*send data to SQL Server*/
LIBNAME SQL ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_TEST.DSN; BL_KEEPNULLS=NO; READ_ISOLATION_LEVEL=RU" SCHEMA= DBO;
DATA SQL._CSVSOURCE_NH_27479;
	SET CSVSOURCE;
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; 

/*DATA DUSTER.CSVSOURCE; *Send data to Duster;*/
/*SET CSVSOURCE;*/
/*RUN;*/

RSUBMIT;

%LET DB = DLGSUTWH; 
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;

PROC SQL;
/*	CREATE TABLE R2 AS*/
/*		SELECT DISTINCT*/
/*			LN54.BF_SSN*/
/*			,LN54.LN_SEQ*/
/*			,CSV.LON_IDENT*/
/*			,CSV.BORROWER_BENEFIT_CODE	AS BA_BBC*/
/*			,LN54.PM_BBS_PGM			AS UH_BBP*/
/*			,CSV.DISQUALIFICATION_DATE	AS BA_DSQ*/
/*			,CSV.INTEREST_STATUS*/
/*			,CSV.REBATE_STATUS*/
/*			,LN10A.CLUID*/
/*			,CSV.COMMONLINE_UNIQUE_ID*/
/*			,LN55.LC_LON_BBT_STA*/
/*		FROM */
/*			OLWHRM1.LN10_LON LN10*/
/*			INNER JOIN*/
/*				(*/
/*					SELECT DISTINCT*/
/*						BF_SSN*/
/*						,LN_SEQ*/
/*						,CATS(COALESCEC(LF_LON_ALT,'0'), PUT(COALESCE(LN_LON_ALT_SEQ,0),Z2.)) AS CLUID*/
/*					FROM*/
/*						OLWHRM1.LN10_LON */
/*				)LN10A*/
/*				ON LN10.BF_SSN = LN10A.BF_SSN*/
/*				AND LN10.LN_SEQ = LN10A.LN_SEQ*/
/*			INNER JOIN CSVSOURCE CSV*/
/*				ON CSV.BORROWER_SSN = LN10.BF_SSN*/
/*				AND CSV.LON_IDENT = LN10.LF_GTR_RFR_XTN*/
/*				AND CSV.COMMONLINE_UNIQUE_ID = LN10A.CLUID*/
/*			INNER JOIN OLWHRM1.LN54_LON_BBS LN54*/
/*				ON LN10.BF_SSN = LN54.BF_SSN*/
/*				AND LN10.LN_SEQ = LN54.LN_SEQ	*/
/*			INNER JOIN OLWHRM1.LN55_LON_BBS_TIR LN55*/
/*				ON LN10.BF_SSN = LN55.BF_SSN*/
/*				AND LN10.LN_SEQ = LN55.LN_SEQ*/
/*		WHERE*/
/*			LN55.LC_STA_LN55 = 'A' */
/*			AND LN55.LC_LON_BBT_STA IN ('A', 'Q', 'R')*/
/*			AND LN54.LC_STA_LN54 = 'A'*/
/*			AND LN54.LC_BBS_ELG = 'Y'*/
/*			AND	(*/
/*					CSV.DISQUALIFICATION_DATE > '000000'*/
/*					OR (*/
/*							CSV.DISQUALIFICATION_DATE = '000000'*/
/*							AND (	*/
/*									(/*Interest Disqualified*/*/
/*										CSV.INTEREST_STATUS IN */
/*											(*/
/*												'00000', '00004', '00005', '00006', '00008', '00010', */
/*												'00011', '00013', '00015', '00016', '00019', '00099'*/
/*											) */
/*										AND CSV.BORROWER_BENEFIT_CODE IN */
/*											(/*Plan type = any of the BBC in Appendix B*/*/
/*												/*BI1*/'0290','0835',*/
/*												/*BI2*/'1560','1565','1717',*/
/*												/*BI3*/'0007','1716',*/
/*												/*BI4*/'1000','1520','4400','5101','6742','6749',*/
/*												/*BI5*/'1570','1580','6720',*/
/*												/*BI6*/'1706',*/
/*												/*BI7*/'0830','1715',*/
/*												/*BI8*/'2585','2590',*/
/*												/*BI9*/'6740','6741',*/
/*												/*BIA*/'6744','6745',*/
/*												/*BIB*/'6746','6747',*/
/*												/*BIC*/'1710',*/
/*												/*BID*/'0006','1020',*/
/*												/*BIE*/'0230','0800','1010','1530',*/
/*												/*BT3*/'3330','3335','3340'*/
/*											)*/
/*									)*/
/*									OR */
/*									(/*Rebate Disqualified*/*/
/*										CSV.REBATE_STATUS IN */
/*											(*/
/*												'00000', '00004', '00006', '00008', '00011', '00013','00014',*/
/*												'00020', '00022', '00023', '00024', '00025', '00026', '00099'*/
/*											)*/
/*										AND CSV.BORROWER_BENEFIT_CODE IN*/
/*											(/*Plan type = any of the BBC in Appendix C*/*/
/*												/*BR1*/'2740','6748',*/
/*												/*BT1*/'2204','2205','2206','2207','2208','2550','2552','2555','2560','2565',*/
/*												/*BT2*/'2214','2215','2216','2217','2530','2533','2535','2540','2570','2573','2575','2580','7722','7723','7724','7725'*/
/*											)*/
/*									)*/
/*								)*/
/*						)*/
/*				)*/
/*	;*/
/*	CREATE TABLE R3A AS /*where LN54 is inactive*/*/
/*		SELECT DISTINCT*/
/*			LN54.BF_SSN*/
/*			,LN54.LN_SEQ*/
/*			,CSV.LON_IDENT*/
/*			,CSV.BORROWER_BENEFIT_CODE	AS BA_BBC*/
/*			,LN54.PM_BBS_PGM			AS UH_BBP*/
/*			,CSV.DISQUALIFICATION_DATE	AS BA_DSQ*/
/*			,LN54.LC_STA_LN54*/
/*			,CSV.INTEREST_STATUS*/
/*			,CSV.REBATE_STATUS*/
/*			,LN10A.CLUID*/
/*			,CSV.COMMONLINE_UNIQUE_ID*/
/*		FROM */
/*			OLWHRM1.LN10_LON LN10*/
/*			INNER JOIN*/
/*				(*/
/*					SELECT DISTINCT*/
/*						BF_SSN*/
/*						,LN_SEQ*/
/*						,CATS(COALESCEC(LF_LON_ALT,'0'), PUT(COALESCE(LN_LON_ALT_SEQ,0),Z2.)) AS CLUID*/
/*					FROM*/
/*						OLWHRM1.LN10_LON */
/*				)LN10A*/
/*				ON LN10.BF_SSN = LN10A.BF_SSN*/
/*				AND LN10.LN_SEQ = LN10A.LN_SEQ*/
/*			INNER JOIN CSVSOURCE CSV*/
/*				ON CSV.BORROWER_SSN = LN10.BF_SSN*/
/*				AND CSV.LON_IDENT = LN10.LF_GTR_RFR_XTN*/
/*				AND CSV.COMMONLINE_UNIQUE_ID = LN10A.CLUID*/
/*			INNER JOIN OLWHRM1.LN54_LON_BBS LN54*/
/*				ON LN10.BF_SSN = LN54.BF_SSN*/
/*				AND LN10.LN_SEQ = LN54.LN_SEQ*/
/*				AND LN54.LC_STA_LN54 = 'I'*/
/*			LEFT OUTER JOIN OLWHRM1.LN54_LON_BBS LN54A*/
/*				ON LN10.BF_SSN = LN54A.BF_SSN*/
/*				AND LN10.LN_SEQ = LN54A.LN_SEQ	*/
/*				AND LN54A.LC_STA_LN54 = 'A'*/
/*		WHERE*/
/*			LN54A.BF_SSN IS NULL*/
/*			AND CSV.DISQUALIFICATION_DATE = '000000'*/
/*			AND (*/
/*					(/*Interest eligible/qualified*/*/
/*						CSV.INTEREST_STATUS IN */
/*							(*/
/*								'00001', '00002', '00003', '00007', '00009', '00012', '00014', '00017', '00018' */
/*							)*/
/*						AND CSV.BORROWER_BENEFIT_CODE IN */
/*							(/*Plan type = any of the BBC in Appendix B*/*/
/*								/*BI1*/'0290','0835',*/
/*								/*BI2*/'1560','1565','1717',*/
/*								/*BI3*/'0007','1716',*/
/*								/*BI4*/'1000','1520','4400','5101','6742','6749',*/
/*								/*BI5*/'1570','1580','6720',*/
/*								/*BI6*/'1706',*/
/*								/*BI7*/'0830','1715',*/
/*								/*BI8*/'2585','2590',*/
/*								/*BI9*/'6740','6741',*/
/*								/*BIA*/'6744','6745',*/
/*								/*BIB*/'6746','6747',*/
/*								/*BIC*/'1710',*/
/*								/*BID*/'0006','1020',*/
/*								/*BIE*/'0230','0800','1010','1530',*/
/*								/*BT3*/'3330','3335','3340'*/
/*							)*/
/*					)*/
/*					OR*/
/*					(/*Rebate eligible/qualified*/*/
/*						CSV.REBATE_STATUS IN*/
/*							(*/
/*								'00001', '00002', '00003', '00005', '00007', '00009', '00010', '00012', */
/*								'00015','00016', '00017', '00018', '00019', '00021', '00027'*/
/*							)*/
/*						AND CSV.BORROWER_BENEFIT_CODE IN*/
/*							(/*Plan type = any of the BBC in Appendix C*/*/
/*								/*BR1*/'2740','6748',*/
/*								/*BT1*/'2204','2205','2206','2207','2208','2550','2552','2555','2560','2565',*/
/*								/*BT2*/'2214','2215','2216','2217','2530','2533','2535','2540','2570','2573','2575','2580','7722','7723','7724','7725'*/
/*							)*/
/*					)*/
/*				)*/
/*	;*/
/*	CREATE TABLE R3B AS /*where LN54 does not exist*/*/
/*		SELECT DISTINCT*/
/*			LN54.BF_SSN*/
/*			,LN54.LN_SEQ*/
/*			,LN10.LN_SEQ				AS LN10_LN_SEQ*/
/*			,CSV.BORROWER_SSN*/
/*			,CSV.LON_IDENT*/
/*			,CSV.BORROWER_BENEFIT_CODE	AS BA_BBC*/
/*			,LN54.PM_BBS_PGM			AS UH_BBP*/
/*			,CSV.DISQUALIFICATION_DATE	AS BA_DSQ*/
/*			,CSV.INTEREST_STATUS*/
/*			,CSV.REBATE_STATUS*/
/*			,LN10A.CLUID*/
/*			,CSV.COMMONLINE_UNIQUE_ID*/
/*		FROM */
/*			OLWHRM1.LN10_LON LN10*/
/*			INNER JOIN*/
/*				(*/
/*					SELECT DISTINCT*/
/*						BF_SSN*/
/*						,LN_SEQ*/
/*						,CATS(COALESCEC(LF_LON_ALT,'0'), PUT(COALESCE(LN_LON_ALT_SEQ,0),Z2.)) AS CLUID*/
/*					FROM*/
/*						OLWHRM1.LN10_LON */
/*				)LN10A*/
/*				ON LN10.BF_SSN = LN10A.BF_SSN*/
/*				AND LN10.LN_SEQ = LN10A.LN_SEQ*/
/*			INNER JOIN CSVSOURCE CSV*/
/*				ON CSV.BORROWER_SSN = LN10.BF_SSN*/
/*				AND CSV.LON_IDENT = LN10.LF_GTR_RFR_XTN*/
/*				AND CSV.COMMONLINE_UNIQUE_ID = LN10A.CLUID*/
/*			LEFT OUTER JOIN OLWHRM1.LN54_LON_BBS LN54*/
/*				ON LN10.BF_SSN = LN54.BF_SSN*/
/*				AND LN10.LN_SEQ = LN54.LN_SEQ	*/
/*		WHERE */
/*			LN54.BF_SSN IS NULL*/
/*			AND CSV.DISQUALIFICATION_DATE = '000000'			 */
/*			AND (*/
/*					(/*Interest eligible/qualified*/*/
/*						CSV.INTEREST_STATUS IN */
/*							(*/
/*								'00001', '00002', '00003', '00007', '00009', '00012', '00014', '00017', '00018' */
/*							)*/
/*						AND CSV.BORROWER_BENEFIT_CODE IN */
/*							(/*Plan type = any of the BBC in Appendix B*/*/
/*								/*BI1*/'0290','0835',*/
/*								/*BI2*/'1560','1565','1717',*/
/*								/*BI3*/'0007','1716',*/
/*								/*BI4*/'1000','1520','4400','5101','6742','6749',*/
/*								/*BI5*/'1570','1580','6720',*/
/*								/*BI6*/'1706',*/
/*								/*BI7*/'0830','1715',*/
/*								/*BI8*/'2585','2590',*/
/*								/*BI9*/'6740','6741',*/
/*								/*BIA*/'6744','6745',*/
/*								/*BIB*/'6746','6747',*/
/*								/*BIC*/'1710',*/
/*								/*BID*/'0006','1020',*/
/*								/*BIE*/'0230','0800','1010','1530',*/
/*								/*BT3*/'3330','3335','3340'*/
/*							)*/
/*					)*/
/*					OR*/
/*					(/*Rebate eligible/qualified*/*/
/*						CSV.REBATE_STATUS IN*/
/*							(*/
/*								'00001', '00002', '00003', '00005', '00007', '00009', '00010', '00012', */
/*								'00015','00016', '00017', '00018', '00019', '00021', '00027'*/
/*							)*/
/*						AND CSV.BORROWER_BENEFIT_CODE IN*/
/*							(/*Plan type = any of the BBC in Appendix C*/*/
/*								/*BR1*/'2740','6748',*/
/*								/*BT1*/'2204','2205','2206','2207','2208','2550','2552','2555','2560','2565',*/
/*								/*BT2*/'2214','2215','2216','2217','2530','2533','2535','2540','2570','2573','2575','2580','7722','7723','7724','7725'*/
/*							)*/
/*					)*/
/*				)*/
	;
	CREATE TABLE R4 AS
		SELECT DISTINCT
			LN54.BF_SSN
			,LN54.LN_SEQ
/*			,CSV.LON_IDENT*/
/*			,CSV.BORROWER_BENEFIT_CODE	AS BA_BBC*/
			,LN54.PM_BBS_PGM			AS UH_BBP
/*			,CSV.DISQUALIFICATION_DATE	AS BA_DSQ*/
			,TRIM(LN10.LF_GTR_RFR_XTN) 	AS LF_GTR_RFR_XTN
			,LN54.LC_STA_LN54
			,LN54.LC_BBS_ELG
			,TRIM(LN10A.CLUID)			AS CLUID
/*			,CSV.COMMONLINE_UNIQUE_ID*/
		FROM 
			OLWHRM1.LN10_LON LN10
			INNER JOIN
				(
					SELECT DISTINCT
						BF_SSN
						,LN_SEQ
/*						,CATS(COALESCEC(LF_LON_ALT,'0'), PUT(COALESCE(LN_LON_ALT_SEQ,0),Z2.)) AS CLUID*/
						,CATS(LF_LON_ALT, PUT(LN_LON_ALT_SEQ,Z2.)) AS CLUID
					FROM
						OLWHRM1.LN10_LON 
				)LN10A
				ON LN10.BF_SSN = LN10A.BF_SSN
				AND LN10.LN_SEQ = LN10A.LN_SEQ
			INNER JOIN OLWHRM1.LN54_LON_BBS LN54
				ON LN10.BF_SSN = LN54.BF_SSN
				AND LN10.LN_SEQ = LN54.LN_SEQ
		WHERE
			LN54.PM_BBS_PGM LIKE 'B%'
			AND LN54.LC_STA_LN54 = 'A'
			AND LN54.LC_BBS_ELG = 'Y'
	;
/*	CREATE TABLE R4_1 AS*/
/*		SELECT DISTINCT*/
/*			LN54.BF_SSN*/
/*			,LN54.LN_SEQ*/
/*			,CSV.LON_IDENT*/
/*			,CSV.BORROWER_BENEFIT_CODE	AS BA_BBC*/
/*			,LN54.PM_BBS_PGM			AS UH_BBP*/
/*			,CSV.DISQUALIFICATION_DATE	AS BA_DSQ*/
/*			,TRIM(LN10.LF_GTR_RFR_XTN) 	AS LF_GTR_RFR_XTN*/
/*			,LN54.LC_STA_LN54*/
/*			,LN54.LC_BBS_ELG*/
/*			,TRIM(LN10A.CLUID)			AS CLUID*/
/*			,CSV.COMMONLINE_UNIQUE_ID*/
/*		FROM */
/*			OLWHRM1.LN10_LON LN10*/
/*			INNER JOIN*/
/*				(*/
/*					SELECT DISTINCT*/
/*						BF_SSN*/
/*						,LN_SEQ*/
/*						,CATS(COALESCEC(LF_LON_ALT,'0'), PUT(COALESCE(LN_LON_ALT_SEQ,0),Z2.)) AS CLUID*/
/*					FROM*/
/*						OLWHRM1.LN10_LON */
/*				)LN10A*/
/*				ON LN10.BF_SSN = LN10A.BF_SSN*/
/*				AND LN10.LN_SEQ = LN10A.LN_SEQ*/
/*			INNER JOIN OLWHRM1.LN54_LON_BBS LN54*/
/*				ON LN10.BF_SSN = LN54.BF_SSN*/
/*				AND LN10.LN_SEQ = LN54.LN_SEQ*/
/*			LEFT JOIN */
/*				(*/
/*					SELECT * */
/*					FROM CSVSOURCE*/
/*					WHERE DUPE ^= .*/
/*					AND COMMONLINE_UNIQUE_ID ^= '000'*/
/*				) CSV*/
/*				ON CSV.BORROWER_SSN = LN10.BF_SSN*/
/*				AND CSV.LON_IDENT = LN10.LF_GTR_RFR_XTN*/
/*		WHERE*/
/*			CSV.BORROWER_SSN IS NULL*/
/*			AND LN54.PM_BBS_PGM LIKE 'B%'*/
/*			AND LN54.LC_STA_LN54 = 'A'*/
/*			AND LN54.LC_BBS_ELG = 'Y'*/
/*	;*/
/*	CREATE TABLE R4_2 AS*/
/*		SELECT DISTINCT*/
/*			LN54.BF_SSN*/
/*			,LN54.LN_SEQ*/
/*			,CSV.LON_IDENT*/
/*			,CSV.BORROWER_BENEFIT_CODE	AS BA_BBC*/
/*			,LN54.PM_BBS_PGM			AS UH_BBP*/
/*			,CSV.DISQUALIFICATION_DATE	AS BA_DSQ*/
/*			,TRIM(LN10.LF_GTR_RFR_XTN) 	AS LF_GTR_RFR_XTN*/
/*			,LN54.LC_STA_LN54*/
/*			,LN54.LC_BBS_ELG*/
/*			,TRIM(LN10A.CLUID)			AS CLUID*/
/*			,CSV.COMMONLINE_UNIQUE_ID*/
/*		FROM */
/*			OLWHRM1.LN10_LON LN10*/
/*			INNER JOIN*/
/*				(*/
/*					SELECT DISTINCT*/
/*						BF_SSN*/
/*						,LN_SEQ*/
/*						,CATS(COALESCEC(LF_LON_ALT,'0'), PUT(COALESCE(LN_LON_ALT_SEQ,0),Z2.)) AS CLUID*/
/*					FROM*/
/*						OLWHRM1.LN10_LON */
/*				)LN10A*/
/*				ON LN10.BF_SSN = LN10A.BF_SSN*/
/*				AND LN10.LN_SEQ = LN10A.LN_SEQ*/
/*			INNER JOIN OLWHRM1.LN54_LON_BBS LN54*/
/*				ON LN10.BF_SSN = LN54.BF_SSN*/
/*				AND LN10.LN_SEQ = LN54.LN_SEQ*/
/*			LEFT JOIN */
/*				(*/
/*					SELECT * */
/*					FROM CSVSOURCE*/
/*					WHERE DUPE = .*/
/*					OR COMMONLINE_UNIQUE_ID = '000'*/
/*				) CSV*/
/*				ON CSV.BORROWER_SSN = LN10.BF_SSN*/
/*				AND CSV.COMMONLINE_UNIQUE_ID = LN10A.CLUID*/
/*		WHERE*/
/*			CSV.BORROWER_SSN IS NULL*/
/*			AND LN54.PM_BBS_PGM LIKE 'B%'*/
/*			AND LN54.LC_STA_LN54 = 'A'*/
/*			AND LN54.LC_BBS_ELG = 'Y'*/
/*	;*/
/*	CREATE TABLE R5 AS*/
/*		SELECT DISTINCT*/
/*			LN54.BF_SSN*/
/*			,LN54.LN_SEQ*/
/*			,CSV.LON_IDENT*/
/*			,CSV.BORROWER_BENEFIT_CODE	AS BA_BBC*/
/*			,LN54.PM_BBS_PGM			AS UH_BBP*/
/*			,CSV.UHEAA_CODE*/
/*			,CSV.DISQUALIFICATION_DATE	AS BA_DSQ*/
/*			,LN54.LC_STA_LN54			*/
/*			,LN10A.CLUID*/
/*			,CSV.COMMONLINE_UNIQUE_ID*/
/*		FROM*/
/*			OLWHRM1.LN10_LON LN10*/
/*			INNER JOIN*/
/*				(*/
/*					SELECT DISTINCT*/
/*						BF_SSN*/
/*						,LN_SEQ*/
/*						,CATS(COALESCEC(LF_LON_ALT,'0'), PUT(COALESCE(LN_LON_ALT_SEQ,0),Z2.)) AS CLUID*/
/*					FROM*/
/*						OLWHRM1.LN10_LON */
/*				)LN10A*/
/*				ON LN10.BF_SSN = LN10A.BF_SSN*/
/*				AND LN10.LN_SEQ = LN10A.LN_SEQ*/
/*			INNER JOIN CSVSOURCE CSV*/
/*				ON CSV.BORROWER_SSN = LN10.BF_SSN*/
/*				AND CSV.LON_IDENT = LN10.LF_GTR_RFR_XTN*/
/*				AND CSV.COMMONLINE_UNIQUE_ID = LN10A.CLUID*/
/*			INNER JOIN OLWHRM1.LN54_LON_BBS LN54*/
/*				ON LN10.BF_SSN = LN54.BF_SSN*/
/*				AND LN10.LN_SEQ = LN54.LN_SEQ*/
/*		WHERE*/
/*			LN54.LC_STA_LN54 = 'A'*/
/*			AND LN54.PM_BBS_PGM ^= CSV.UHEAA_CODE*/
/*	;*/
QUIT;

ENDRSUBMIT;

/*DATA R2;*/
/*	SET DUSTER.R2;*/
/*RUN;*/
/*DATA R3A;*/
/*	SET DUSTER.R3A;*/
/*RUN;*/
/*DATA R3B;*/
/*	SET DUSTER.R3B;*/
/*RUN;*/
DATA R4;
	SET DUSTER.R4;
RUN;
/*DATA R4_1;*/
/*	SET DUSTER.R4_1;*/
/*RUN;*/
/*DATA R4_2;*/
/*	SET DUSTER.R4_2;*/
/*RUN;*/
/*DATA R5;*/
/*	SET DUSTER.R5;*/
/*RUN;*/

/*PROC EXPORT*/
/*	DATA=R2*/
/*	OUTFILE="&RPTLIB\UNH_27479.xlsx"*/
/*	DBMS = EXCEL*/
/*	REPLACE;*/
/*RUN;*/
/*PROC EXPORT*/
/*	DATA=R3A*/
/*	OUTFILE="&RPTLIB\UNH_27479.xlsx"*/
/*	DBMS = EXCEL*/
/*	REPLACE;*/
/*RUN;*/
/*PROC EXPORT*/
/*	DATA=R3B*/
/*	OUTFILE="&RPTLIB\UNH_27479.xlsx"*/
/*	DBMS = EXCEL*/
/*	REPLACE;*/
/*RUN;*/
/*PROC EXPORT*/
/*	DATA=R4*/
/*	OUTFILE="&RPTLIB\UNH_27479.xlsx"*/
/*	DBMS = EXCEL*/
/*	REPLACE;*/
/*RUN;*/
/*PROC EXPORT*/
/*	DATA=R5*/
/*	OUTFILE="&RPTLIB\UNH_27479.xlsx"*/
/*	DBMS = EXCEL*/
/*	REPLACE;*/
/*RUN;*/
/**/
/*proc sql;*/
/*select * from csvsource */
/*where borrower_ssn in ('519253338');*/
/*quit;*/
/**/
/**/
/*proc sql noprint;*/
/*	create table csvsource_dupes as*/
/*		select */
/*			borrower_ssn*/
/*			,lon_ident*/
/*			,count(lon_ident) as dupe*/
/*		from csvsource*/
/*		group by */
/*			borrower_ssn*/
/*			,lon_ident*/
/*		having count(lon_ident) > 1*/
/*	;*/
/*	create table _csvsource as*/
/*		select */
/*			a.dupe*/
/*			,b.**/
/*		from csvsource_dupes a*/
/*		full outer join csvsource b*/
/*			on a.borrower_ssn = b.borrower_ssn*/
/*			and a.lon_ident = b.lon_ident*/
/*	;*/
/*quit;*/
/**/
/*proc sql noprint;*/
/*	create table _csvsource as*/
/*	select */
/*		case*/
/*			when lon_ident is null */
/*				then 1*/
/*			else 0*/
/*		end as flag*/
/**/
/*		,* */
/*	from csvsource;*/
/*quit;*/
/**/

proc sql noprint;
	create table r4_dupes as
		select 
			bf_ssn
			,count(LF_GTR_RFR_XTN) as dupe
			,count(distinct LF_GTR_RFR_XTN) as dupe_distinct
		from r4
		group by
			bf_ssn
	;
quit;
proc sql noprint;
	create table r4_dupes as
		select 
			LF_GTR_RFR_XTN
			,count(bf_ssn) as dupe
			,count(distinct bf_ssn) as dupe_distinct
		from r4
		group by
			LF_GTR_RFR_XTN
		having count(distinct bf_ssn) > 1
	;
quit;















/*CAN BE JOINED ON lon_ident:  matches on unique ident*/
PROC SQL NOPRINT;
	CREATE TABLE R4_LEFT_JOIN AS
		SELECT 
			* 
		FROM
			R4
			LEFT JOIN CSVSOURCE CSV
				ON R4.BF_SSN = CSV.BORROWER_SSN
				AND R4.LF_GTR_RFR_XTN = CSV.LON_IDENT
			LEFT JOIN
			(
				SELECT 
					LF_GTR_RFR_XTN
					,count(LF_GTR_RFR_XTN) as dupe
				FROM
					R4
				GROUP BY
					LF_GTR_RFR_XTN
				HAVING
					count(LF_GTR_RFR_XTN) > 1					
			)A
				ON A.LF_GTR_RFR_XTN = R4.LF_GTR_RFR_XTN
		WHERE
			CSV.BORROWER_SSN IS NULL 
			AND A.LF_GTR_RFR_XTN IS NULL
	;
QUIT;


/*PROC EXPORT*/
/*	DATA=R4_LEFT_JOIN*/
/*	OUTFILE="&RPTLIB\UNH_27479.xlsx"*/
/*	DBMS = EXCEL*/
/*	REPLACE;*/
/*	SHEET='LON_IDENT';*/
/*RUN;*/

/*match on lon_ident*/
/*PROC SQL NOPRINT;*/
/*	CREATE TABLE R4_INNER_JOIN AS*/
/*		SELECT */
/*			* */
/*		FROM*/
/*			R4*/
/*			INNER JOIN CSVSOURCE CSV*/
/*				ON R4.BF_SSN = CSV.BORROWER_SSN*/
/*				AND R4.LF_GTR_RFR_XTN = CSV.LON_IDENT*/
/*;*/
/*QUIT;*/




proc sql;
select * from r4_final
where bf_ssn = '003786527'
;
quit;

proc sql;
select * from csvsource
where borrower_ssn = '003786527'
;
quit;

proc sql;
select * from r4_left_join
where bf_ssn= '003786527'
;
quit;
proc sql;
select * from r4
where bf_ssn= '003786527'
;
quit;

proc sql;
select * from csvsource where commonline_unique_id in
('0025720000P01802401','0025720000P02076402','0025720000P02149701','0025720000P02200301');
quit;

proc sql;
select * from r4 where cluid in
('0025720000P01802401','0025720000P02076402','0025720000P02149701','0025720000P02200301');
quit;









/*no match on cluid & no match on lon_ident*/
PROC SQL NOPRINT;
CREATE TABLE R4_FINAL AS
	SELECT 
		R4.*
	FROM
		R4
		LEFT JOIN CSVSOURCE CSV 
			ON CSV.COMMONLINE_UNIQUE_ID = R4.CLUID
		LEFT JOIN R4_left_JOIN R4lJ 
/*			ON R4lJ.LF_GTR_RFR_XTN = R4.LF_GTR_RFR_XTN*/
			on r4lj.bf_ssn = r4.bf_ssn
			and r4lj.ln_seq = r4.ln_seq
	WHERE
		CSV.BORROWER_SSN IS NULL
	 	AND R4lJ.BF_SSN IS NULL
	 ORDER BY 
		R4.BF_SSN
	;
QUIT;
/*PROC EXPORT*/
/*	DATA=R4_FINAL*/
/*	OUTFILE="&RPTLIB\UNH_27479.xlsx"*/
/*	DBMS = EXCEL*/
/*	REPLACE;*/
/*	SHEET='CLUID';*/
/*RUN;*/
