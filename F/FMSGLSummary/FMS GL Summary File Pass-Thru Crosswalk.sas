/*No tunnel is needed to run this code - change the names of the files under input to the names specified by the business unit*/

/*INPUT*/
FILENAME GL_FIL "N:\FED SAIG FILES\TEST\TEST_NFP_GL_SUMMARY_DLY_OUT_20111219212941.txt";  *GL Summary File;
FILENAME CRSWALK "N:\FED SAIG FILES\TEST\TEST_NFP_FMS_ACCT_MAPPING_IN_20111129153226.txt";  *CrossWalk File;
FILENAME REF_FIL "X:\PADD\FTP\Test\Fed Testing Files\TEST_NFP_REFUND_TSXN6_OUT_20111208133708.txt";  *Refund File;
FILENAME CON_FIL "X:\PADD\FTP\Test\Fed Testing Files\TEST_NFP_REFUND_CONF_OUT_20111208133720.txt";  *Confirmation File;

/*OUTPUT*/
FILENAME REPORT2 "X:\PADD\FTP\TEST\GL Summary Output File.txt"; *GL and CrossWalk File;
FILENAME REPORT3 "X:\PADD\FTP\TEST\Refund File.txt"; *Refund File;
FILENAME REPORT4 "X:\PADD\FTP\TEST\Refund Confirmation File.txt"; *Confirmation File;



DATA CRS ;
	INFILE CRSWALK DSD DLM = '10'x FIRSTOBS=1 MISSOVER END = EOF LRECL=32767;
	INPUT DEFAULT_STATUS $ 1-2	
		PROGRAM_TYPE $ 31-33 
		FILE_TC $ 61-66 
		AMOUNT_TYPE $ 91-108 
		Item_Number $ 121-134 
		Aseg_Fund $ 171-177 
		Aseg_Category $ 186
		Aseg_Budget_Year $ 201-202 
		Aseg_Organization $ 216-223 
		Aseg_Limitation $ 231-233 
		Aseg_Object_Class $ 246-253 
		Aseg_Activity $ 261-268 
		ASEG_CFDA $ 271-278 
		Aseg_Cohort_Year $ 291-294 
		Aseg_Sector $ 306 
		ASEG_SOURCE_CODE $ 321-322 
		Aseg_Cost_Code $ 323-325 
		Aseg_Loan_Grant_Type $ 338-345 
		Description $ 353-410 
		Dr_Account_Segment_Value $ 593-598 
		Cr_Account_Segment_Value $ 599-604 
		Special_Logic $ 652 
		School_Summary_Flag $ 701 
		START_DATE_ACTIVE $ 855-862 
		END_DATE_ACTIVE $ 863-870 
		ASEG_CREATION_DATE $ 871-878 
		ASEG_LAST_UPDATE_DATE $ 879-886;
RUN;

DATA GL(DROP=HEAD) ;
	INFILE GL_FIL DSD DLM = ',' MISSOVER end = eof LRECL=32767;
	INPUT HEAD $ 1 @;
	RETAIN BATCH_NUMBER;
	IF HEAD= 'H' THEN INPUT BATCH_NUMBER $ 2-6; 
	IF HEAD= 'D' THEN DO;
		INPUT HEAD $ 1 LOAN_PROGRAM $ 2-4 FILE_TC $ 11-16 P_DATE $ 17-24 AMOUNT_TYPE $ 25-54 AMOUNT 55-69 
			SCHEDULE_NUMBER $ 70-79 S_DATE $ 80-87 DOCUMENT_TYPE $ 88-93 CREDIT_REFORM_CODE $ 106-111
			DEFAULT_STATUS $ 116-117 TRANSACTIONAL_IDENTIFIER $ 118-133 OTHER_SERVICER_ID $ 134-139 INSTITUTION_CODE $ 140-150;
		INFORMAT AMOUNT 15.2;
		FORMAT AMOUNT COMMA15.2;
		FORMAT PROCESS_DATE SCHEDULE_DATE TRANSACTION_DATE MMDDYY10.;
			PROCESS_DATE = MDY(SUBSTR(P_DATE,5,2), SUBSTR(P_DATE,7,2),SUBSTR(P_DATE,1,4)); 
			SCHEDULE_DATE = MDY(SUBSTR(S_DATE,5,2), SUBSTR(S_DATE,7,2),SUBSTR(S_DATE,1,4));
			P_DATE_NUM = INPUT(P_DATE,8.);
			TRANSACTION_DATE = PROCESS_DATE;
		OUTPUT;
	END;
RUN;

DATA REFUND(DROP=HEAD) ;
	INFILE REF_FIL DSD DLM = ',' MISSOVER end = eof LRECL=32767;
	INPUT HEAD $ 1 @;
	RETAIN BATCH_NUMBER;
	IF HEAD= 'H' THEN INPUT BATCH_NUMBER $ 2-6; 
	IF HEAD= 'D' THEN DO;
		INPUT TRANSACTIONAL_IDENTIFIER $ 2-17 P_DATE $ 4-11 TRAN_TYP $ 18-23 AMOUNT 311-325 SCHEDULE_NUMBER $ 326-335 ;
		INFORMAT AMOUNT 15.2;
		FORMAT AMOUNT COMMA15.2;
		FORMAT PROCESS_DATE TRANSACTION_DATE MMDDYY10.;
		PROCESS_DATE = MDY(SUBSTR(P_DATE,5,2), SUBSTR(P_DATE,7,2),SUBSTR(P_DATE,1,4)); 
		TRANSACTION_DATE = PROCESS_DATE;
	AMOUNT = AMOUNT * -1;
		IF TRAN_TYP = 'CANCEL' THEN OUTPUT;
	END;
RUN;

DATA CONF(DROP=HEAD) ;
	INFILE CON_FIL DSD DLM = ',' MISSOVER end = eof LRECL=32767;
	INPUT HEAD $ 1 @;
	RETAIN BATCH_NUMBER;
	IF HEAD= 'H' THEN INPUT BATCH_NUMBER $ 2-6; 
	IF HEAD= 'D' THEN DO;
		INPUT TRANSACTIONAL_IDENTIFIER $ 2-17 P_DATE $ 133-140 AMOUNT 141-155 SCHEDULE_NUMBER $ 83-132 ;
		INFORMAT AMOUNT 15.2;
		FORMAT AMOUNT COMMA15.2;
		FORMAT PROCESS_DATE TRANSACTION_DATE MMDDYY10.;
		PROCESS_DATE = MDY(SUBSTR(P_DATE,5,2), SUBSTR(P_DATE,7,2),SUBSTR(P_DATE,1,4)); 
		TRANSACTION_DATE = PROCESS_DATE;
		OUTPUT;
	END;
RUN;

PROC SQL;
CREATE TABLE R2 AS
SELECT 'FMSGL' || PUT(TODAY(),YYMMDDN8.) AS BATCH_NUMBER
	,GL.TRANSACTION_DATE
	,'FMS GL Summary File' as REFERENCE
	,GL.DEFAULT_STATUS 
	,GL.LOAN_PROGRAM
	,GL.FILE_TC
	,GL.AMOUNT_TYPE
	,CRS.Item_Number
	,CRS.Aseg_Fund
	,CRS.Aseg_Category
	,CRS.Aseg_Budget_Year
	,CRS.Dr_Account_Segment_Value
	,CRS.Cr_Account_Segment_Value
	,CRS.Aseg_Organization
	,CRS.Aseg_Limitation
	,CRS.Aseg_Object_Class
	,CRS.Aseg_Activity
	,CRS.Aseg_Cfda
	,SUBSTR(GL.CREDIT_REFORM_CODE,3,2) AS Aseg_Cohort_Year
	,CRS.Aseg_Sector
	,CRS.ASEG_SOURCE_CODE
	,CRS.Aseg_Cost_Code
	,GL.INSTITUTION_CODE 
	,SUBSTR(GL.CREDIT_REFORM_CODE,5,2) AS Aseg_Loan_Grant_Type
	,CRS.Description
	,CRS.Special_Logic
	,CRS.School_Summary_Flag
	,GL.AMOUNT 
	,GL.PROCESS_DATE 
	,GL.SCHEDULE_NUMBER
	,GL.SCHEDULE_DATE
	,GL.DOCUMENT_TYPE
	,GL.CREDIT_REFORM_CODE
	,GL.TRANSACTIONAL_IDENTIFIER
	,GL.OTHER_SERVICER_ID
FROM GL, CRS
WHERE GL.DEFAULT_STATUS = CRS.DEFAULT_STATUS
	AND GL.LOAN_PROGRAM = CRS.PROGRAM_TYPE
	AND GL.FILE_TC = CRS.FILE_TC
	AND GL.AMOUNT_TYPE = CRS.AMOUNT_TYPE
	AND GL.LOAN_PROGRAM IN ('DLO','FAF','FAL','FBR','FCD','FCO','FIS','GAF','GAL','LNC','LP1','LP2','PI1','PI2','TPL','UNP')
	AND GL.P_DATE_NUM BETWEEN INPUT(START_DATE_ACTIVE,8.) AND INPUT(END_DATE_ACTIVE,8.);
QUIT;

PROC SQL;
CREATE TABLE R3 AS
SELECT 'REF' || PUT(TODAY(),YYMMDDN8.) AS BATCH_NUMBER
	,REFUND.TRANSACTION_DATE
	,'Refund File' as REFERENCE
	,'SG' AS DEFAULT_STATUS 
	,'UNP' AS LOAN_PROGRAM
	,'REFUND' AS FILE_TC
	,'SUM_LEV1_AMT' AS AMOUNT_TYPE
	,'UNPSG_REFUND1' AS Item_Number
	,'3875FNY' AS Aseg_Fund
	,'C' AS Aseg_Category
	,'BD' AS Aseg_Budget_Year
	,'241000' AS Dr_Account_Segment_Value
	,'1010c3' AS Cr_Account_Segment_Value
	,'0' AS Aseg_Organization
	,'Y22' AS Aseg_Limitation
	,'69018' AS Aseg_Object_Class
	,'0' AS Aseg_Activity
	,'0' AS Aseg_Cfda
	,'CY' AS Aseg_Cohort_Year
	,'N' AS Aseg_Sector
	,'NC' AS ASEG_SOURCE_CODE
	,'UNP' AS Aseg_Cost_Code
	,'' AS INSTITUTION_CODE 
	,0 AS Aseg_Loan_Grant_Type
	,'Unapplied Refund' as Description
	,'' as Special_Logic
	,'' as School_Summary_Flag
	,REFUND.AMOUNT 
	,REFUND.PROCESS_DATE 
	,REFUND.SCHEDULE_NUMBER
	,'' as SCHEDULE_DATE
	,'' as DOCUMENT_TYPE
	,'' as CREDIT_REFORM_CODE
	,TRANSACTIONAL_IDENTIFIER
	,'' as OTHER_SERVICER_ID
FROM REFUND;
QUIT;

PROC SQL;
CREATE TABLE R4 AS
SELECT 'REFCON' || PUT(TODAY(),YYMMDDN8.) AS BATCH_NUMBER
	,TRANSACTION_DATE
	,'Refund Confirmation File' as REFERENCE
	,'SG' AS DEFAULT_STATUS 
	,'UNP' AS LOAN_PROGRAM
	,'REFUND' AS FILE_TC
	,'SUM_LEV1_AMT' AS AMOUNT_TYPE
	,'UNPSG_REFUND1' AS Item_Number
	,'3875FNY' AS Aseg_Fund
	,'C' AS Aseg_Category
	,'BD' AS Aseg_Budget_Year
	,'241000' AS Dr_Account_Segment_Value
	,'1010c3' AS Cr_Account_Segment_Value
	,'0' AS Aseg_Organization
	,'Y22' AS Aseg_Limitation
	,'69018' AS Aseg_Object_Class
	,'0' AS Aseg_Activity
	,'0' AS Aseg_Cfda
	,'CY' AS Aseg_Cohort_Year
	,'N' AS Aseg_Sector
	,'NC' AS ASEG_SOURCE_CODE
	,'UNP' AS Aseg_Cost_Code
	,'' AS INSTITUTION_CODE 
	,0 AS Aseg_Loan_Grant_Type
	,'Unapplied Refund' as Description
	,'' as Special_Logic
	,'' as School_Summary_Flag
	,AMOUNT 
	,PROCESS_DATE 
	,SCHEDULE_NUMBER
	,'' as SCHEDULE_DATE
	,'' as DOCUMENT_TYPE
	,'' as CREDIT_REFORM_CODE
	,TRANSACTIONAL_IDENTIFIER
	,'' as OTHER_SERVICER_ID
FROM CONF;
QUIT;

PROC EXPORT DATA= WORK.R2 
            OUTFILE= REPORT2
            DBMS=TAB REPLACE;
     PUTNAMES=YES;
RUN;
PROC EXPORT DATA= WORK.R3 
            OUTFILE= REPORT3
            DBMS=TAB REPLACE;
     PUTNAMES=YES;
RUN;
PROC EXPORT DATA= WORK.R4 
            OUTFILE= REPORT4
            DBMS=TAB REPLACE;
     PUTNAMES=YES;
RUN;