/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/NH25523.RZ";
FILENAME REPORT2 "&RPTLIB/NH25523.R2";
FILENAME REPORT3 "&RPTLIB/NH25523.R3";
FILENAME REPORT4 "&RPTLIB/NH25523.R4";


DATA TestFile;
	INFILE 'T:/J00BX_829769_INCENTIVE_SC370R5_2016020301130493_UO.CSV' DLM="," DSD MISSOVER FIRSTOBS=2;
	FORMAT 	DEALID $10. SSN $9. LAST $30. FIRST $30. COMMONLINE $20. GUARDATE $6. 
			BBCODE $4. APPDATE $6. ONTIMEPAY 8. INTSTAT $5. INTAPP $6. 
			REBATESTAT $5. REBATEAPP $6. REBATEAMT DOLLAR10.2 DISQUALDATE $10. 
			ACHSTAT $5. SPCSTAT $5. LOANIDENT $50. SUBSIDYCODE $1. 
			REDUCEDINTRATE DOLLAR10.5 PLANSEQ $2. PROGRAMEND $6. INCENTPLANOPT $1.;
	INPUT 	DEALID $ SSN $ LAST $ FIRST $ COMMONLINE $ GUARDATE $
			BBCODE $ APPDATE $ ONTIMEPAY INTSTAT $ INTAPP $ 
			REBATESTAT $ REBATEAPP $ REBATEAMT  DISQUALDATE $ 
			ACHSTAT $ SPCSTAT $ LOANIDENT $ SUBSIDYCODE $ 
			REDUCEDINTRATE PLANSEQ $ PROGRAMEND $ INCENTPLANOPT $;
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ;*/ /*test*/
DATA DUSTER.TestFile; *Send data to Duster;
SET TestFile;
	COMMONLINE = COMPRESS(COMMONLINE,'''');
	LOANIDENT = COMPRESS(LOANIDENT,'''');
RUN;
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
CONNECT TO DB2 (DATABASE=DLGSUTWH); /*DLGSWQUT TEST*//*DLGSUTWH LIVE*/
CREATE TABLE Matches AS
SELECT DISTINCT
	TestFile.*,
	RemoteData.LN_SEQ,
	CASE WHEN TestFile.BBCODE IN ('0290','0835') THEN 'BI1'
         WHEN TestFile.BBCODE IN ('1560','1565','1717') THEN 'BI2'
         WHEN TestFile.BBCODE IN ('0007','1716') THEN 'BI3'
         WHEN TestFile.BBCODE IN ('1000','1520','4400','5101','6742','6749') THEN 'BI4'
         WHEN TestFile.BBCODE IN ('1570','1580','6720') THEN 'BI5'
         WHEN TestFile.BBCODE IN ('1706') THEN 'BI6'
         WHEN TestFile.BBCODE IN ('0830','1715') THEN 'BI7'
         WHEN TestFile.BBCODE IN ('2585','2590') THEN 'BI8'
         WHEN TestFile.BBCODE IN ('6740','6741') THEN 'BI9'
         WHEN TestFile.BBCODE IN ('6744','6745') THEN 'BIA'
         WHEN TestFile.BBCODE IN ('6746','6747') THEN 'BIB'
         WHEN TestFile.BBCODE IN ('1710') THEN 'BIC'
         WHEN TestFile.BBCODE IN ('0006','1020') THEN 'BID'
         WHEN TestFile.BBCODE IN ('0230','0800','1010','1530') THEN 'BIE'
         WHEN TestFile.BBCODE IN ('2740','2741','6748') THEN 'BR1'
         WHEN TestFile.BBCODE IN ('2204','2205','2206','2207','2208','2550','2552','2555','2560','2565') THEN 'BT1'
         WHEN TestFile.BBCODE IN ('2214','2215','2216','2217','2530','2533','2535','2540','2570','2573','2575','2580','7722','7723','7724','7725') THEN 'BT2'
         WHEN TestFile.BBCODE IN ('3330','3335','3340') THEN 'BT3'
         ELSE '' END AS BorrowerBenefitType,
	CASE WHEN TestFile.DISQUALDATE = '000000' THEN 'Qualified' ELSE 'Disqualified' END AS CorrectedStatus,
	CASE WHEN TestFile.BBCODE IN ('0290','0835') THEN 1 
         WHEN TestFile.BBCODE IN ('1560','1565','1717') THEN 1
         WHEN TestFile.BBCODE IN ('0007','1716') THEN 1
         WHEN TestFile.BBCODE IN ('1000','1520','4400','5101','6742','6749') THEN
			CASE WHEN TestFile.ONTIMEPAY > 12 THEN 12 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('1570','1580','6720') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 36 THEN 36 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('1706') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 48 THEN 48 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('0830','1715') THEN 1
         WHEN TestFile.BBCODE IN ('2585','2590') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 12 THEN 12 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('6740','6741') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 36 THEN 36 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('6744','6745') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 36 THEN 36 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('6746','6747') THEN
		 	CASE WHEN TestFile.ONTIMEPAY > 36 THEN 36 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('1710') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 48 THEN 48 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('0006','1020') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 48 THEN 48 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('0230','0800','1010','1530') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 48 THEN 48 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('2740','2741','6748') THEN 1
         WHEN TestFile.BBCODE IN ('2204','2205','2206','2207','2208','2550','2552','2555','2560','2565') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 37 THEN 37 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('2214','2215','2216','2217','2530','2533','2535','2540','2570','2573','2575','2580','7722','7723','7724','7725') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 36 THEN 36 ELSE TestFile.ONTIMEPAY END
         WHEN TestFile.BBCODE IN ('3330','3335','3340') THEN 
		 	CASE WHEN TestFile.ONTIMEPAY > 36 THEN 36 ELSE TestFile.ONTIMEPAY END
         ELSE 0 END AS PCVPayments,
	TestFile.DISQUALDATE AS DisqualificationDate,
	'07' AS DisqualReason,
	'Assigning BBP for BANA-UHEAA Conversion.' AS Comment,
	CASE WHEN RemoteData.LN_SEQ IS NULL THEN 'R3'
		 ELSE '' END AS R3Output,
	CASE WHEN TestFile.BBCODE = '0000' OR TestFile.BBCODE = '' OR TestFile.BBCODE IS NULL THEN 'EXCLUDE'
         WHEN TestFile.BBCODE NOT IN 
			('0290','0835','1560','1565','1717','0007','1716','1000','1520','4400','5101','6742','6749',
			'1570','1580','6720','1706','0830','1715','2585','2590','6740','6741','6744','6745','6746',
			'6747','1710','0006','1020','0230','0800','1010','1530','2740','6748','2204','2205','2206',
			'2207','2208','2550','2552','2555','2560','2565','2214','2215','2216','2217','2530','2533',
			'2535','2540','2570','2573','2575','2580','7722','7723','7724','7725','3330','3335','3340')
			AND TestFile.BBCODE NOT IN
			('0012','0210','0230','0800','0810','0845','1010','1020','1030','1500','1505','1510','1520',
			'1530','1570','1580','1706','1710','1719','2805','4400','5101','6741','6749','6750','6751',
			'7604','7713','7714','7716','7720','7722','2585','0830','2590','245','1548','0290','1560',
			'1565','1550','1553','1555','1558','2510','7600','7601','1545','1585','1590','2740','7602',
			'7603','2224','2225','2226','2227','0820','1540','1545','1590','2510','2805','7728','0245',
			'7708') THEN 'R4'
		ELSE '' END AS R4Output
FROM TestFile LEFT OUTER JOIN CONNECTION TO DB2 
						(
							SELECT
								LN10.BF_SSN,
								LN10.LN_SEQ,
								CONCAT(LN10.LF_LON_ALT,(CASE WHEN LN10.LN_LON_ALT_SEQ < 10 THEN CONCAT('0',LN10.LN_LON_ALT_SEQ) ELSE LN10.LN_LON_ALT_SEQ END)) AS COMMONLINEID,
								LN10.LF_GTR_RFR_XTN 
							FROM
								OLWHRM1.LN10_LON LN10						
						) RemoteData
							ON RemoteData.LF_GTR_RFR_XTN = TestFile.LOANIDENT
							AND RemoteData.BF_SSN = TestFile.SSN
;
DISCONNECT FROM DB2;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  
%SQLCHECK;
QUIT;

ENDRSUBMIT;

DATA _NULL_;
	SET DUSTER.Matches(where=(R3Output='' AND R4Output='' AND BorrowerBenefitType <> ''));
		FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767; 
		
		FORMAT SSN $9. ;
		FORMAT LN_SEQ $3. ;
		FORMAT BorrowerBenefitType $3. ;
		FORMAT CorrectedStatus $15. ;
		FORMAT PCVPayments 5. ;
		FORMAT DisqualificationDate $10. ;
		FORMAT DisqualReason $2. ;
		FORMAT Comment $40. ;
		
		IF _N_ = 1 THEN DO;
			PUT
			'SSN,'
			'LN_SEQ,'
			'BorrowerBenefitType,'
			'CorrectedStatus,'
			'PCVPayments,'
			'DisqualDate,'
			'DisqualReason,'
			'Comment';
		END;
		DO;
			PUT SSN @;
			PUT LN_SEQ @;
			PUT BorrowerBenefitType @;
			PUT CorrectedStatus @;
			PUT PCVPayments @;
			PUT DisqualificationDate @;
			PUT DisqualReason @;
			PUT Comment $;
		END;
	RUN;

DATA _NULL_;
	SET DUSTER.Matches(drop=LN_SEQ where=(R3Output='R3' AND R4Output <> 'EXCLUDE' AND BorrowerBenefitType <> ''));
		RETAIN _SSN;
		IF (SSN eq _SSN) THEN delete;
		ELSE output;
		_SSN = SSN;
		FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767; 
		
		FORMAT DEALID $10. ;
		FORMAT SSN $9. ;
		FORMAT LAST $30. ;
		FORMAT FIRST $30. ;
		FORMAT COMMONLINE $20. ;
		FORMAT GUARDATE $6. ;
		FORMAT BBCODE $4. ;
		FORMAT APPDATE $6. ;
		FORMAT ONTIMEPAY $5. ;
		FORMAT INTSTAT $5. ;
		FORMAT INTAPP $6. ;
		FORMAT REBATESTAT $5. ;
		FORMAT REBATEAPP $6. ;
		FORMAT REBATEAMT DOLLAR10.2 ;
		FORMAT DisqualificationDate $10. ;
		FORMAT ACHSTAT $5. ;
		FORMAT SPCSTAT $5. ;
		FORMAT LOANIDENT $50. ;
		FORMAT SUBSIDYCODE $1. ;
		FORMAT REDUCEDINTRATE DOLLAR10.5 ;
		FORMAT PLANSEQ $2. ;
		FORMAT PROGRAMEND $6. ;
		FORMAT INCENTPLANOPT $1.;
		
		IF _N_ = 1 THEN DO;
			PUT
			'DEALID' ','
			'SSN' ','
			'LAST' ','
			'FIRST' ','
			'COMMONLINE' ','
			'GUARDATE' ','
			'BBCODE' ','
			'APPDATE' ','
			'ONTIMEPAY' ','
			'INTSTAT' ','
			'INTAPP' ','
			'REBATESTAT' ','
			'REBATEAPP' ','
			'REBATEAMT' ','
			'DISQUALDATE' ','
			'ACHSTAT' ','
			'SPCSTAT' ','
			'LOANIDENT' ','
			'SUBSIDYCODE' ','
			'REDUCEDINTRATE' ','
			'PLANSEQ' ','
			'PROGRAMEND' ','
			'INCENTPLANOPT';
		END;
		DO;
		PUT DEALID $ @ ;
		PUT SSN $ @ ;
		PUT LAST $ @ ;
		PUT FIRST $ @ ;
		PUT COMMONLINE $ @ ;
		PUT GUARDATE $ @ ;
		PUT BBCODE $ @ ;
		PUT APPDATE $ @ ;
		PUT ONTIMEPAY $ @ ;
		PUT INTSTAT $ @ ;
		PUT INTAPP $ @ ;
		PUT REBATESTAT $ @ ;
		PUT REBATEAPP $ @ ;
		PUT REBATEAMT @ ;
		PUT DisqualificationDate $ @ ;
		PUT ACHSTAT $ @ ;
		PUT SPCSTAT $ @ ;
		PUT LOANIDENT $ @ ;
		PUT SUBSIDYCODE $ @ ;
		PUT REDUCEDINTRATE @ ;
		PUT PLANSEQ $ @ ;
		PUT PROGRAMEND $ @ ;
		PUT INCENTPLANOPT $ ;
		END;
	RUN;

DATA _NULL_;
	SET DUSTER.Matches(drop=LN_SEQ where=(R3Output='' AND R4Output='R4' AND BorrowerBenefitType <> ''));
		RETAIN _SSN;
		IF (SSN eq _SSN) THEN delete;
		ELSE output;
		_SSN = SSN;
		FILE REPORT4 DELIMITER=',' DSD DROPOVER LRECL=32767; 
		
		FORMAT DEALID $10. ;
		FORMAT SSN $9. ;
		FORMAT LAST $30. ;
		FORMAT FIRST $30. ;
		FORMAT COMMONLINE $20. ;
		FORMAT GUARDATE $6. ;
		FORMAT BBCODE $4. ;
		FORMAT APPDATE $6. ;
		FORMAT ONTIMEPAY $5. ;
		FORMAT INTSTAT $5. ;
		FORMAT INTAPP $6. ;
		FORMAT REBATESTAT $5. ;
		FORMAT REBATEAPP $6. ;
		FORMAT REBATEAMT DOLLAR10.2 ;
		FORMAT DisqualificationDate $10. ;
		FORMAT ACHSTAT $5. ;
		FORMAT SPCSTAT $5. ;
		FORMAT LOANIDENT $50. ;
		FORMAT SUBSIDYCODE $1. ;
		FORMAT REDUCEDINTRATE DOLLAR10.5 ;
		FORMAT PLANSEQ $2. ;
		FORMAT PROGRAMEND $6. ;
		FORMAT INCENTPLANOPT $1.;
		
		IF _N_ = 1 THEN DO;
			PUT
			'DEALID' ','
			'SSN' ','
			'LAST' ','
			'FIRST' ','
			'COMMONLINE' ','
			'GUARDATE' ','
			'BBCODE' ','
			'APPDATE' ','
			'ONTIMEPAY' ','
			'INTSTAT' ','
			'INTAPP' ','
			'REBATESTAT' ','
			'REBATEAPP' ','
			'REBATEAMT' ','
			'DISQUALDATE' ','
			'ACHSTAT' ','
			'SPCSTAT' ','
			'LOANIDENT' ','
			'SUBSIDYCODE' ','
			'REDUCEDINTRATE' ','
			'PLANSEQ' ','
			'PROGRAMEND' ','
			'INCENTPLANOPT';
		END;
		DO;
		PUT DEALID $ @ ;
		PUT SSN $ @ ;
		PUT LAST $ @ ;
		PUT FIRST $ @ ;
		PUT COMMONLINE $ @ ;
		PUT GUARDATE $ @ ;
		PUT BBCODE $ @ ;
		PUT APPDATE $ @ ;
		PUT ONTIMEPAY $ @ ;
		PUT INTSTAT $ @ ;
		PUT INTAPP $ @ ;
		PUT REBATESTAT $ @ ;
		PUT REBATEAPP $ @ ;
		PUT REBATEAMT @ ;
		PUT DisqualificationDate $ @ ;
		PUT ACHSTAT $ @ ;
		PUT SPCSTAT $ @ ;
		PUT LOANIDENT $ @ ;
		PUT SUBSIDYCODE $ @ ;
		PUT REDUCEDINTRATE @ ;
		PUT PLANSEQ $ @ ;
		PUT PROGRAMEND $ @ ;
		PUT INCENTPLANOPT $ ;
		END;
	RUN;
