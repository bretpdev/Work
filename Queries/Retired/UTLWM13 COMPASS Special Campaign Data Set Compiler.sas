/*=====================================================*/
/*UTLWM13 - COMPASS SPECIAL CAMPAIGN DATA SET COMPILER*/
/*=====================================================*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*%LET RPTLIBX = /sas/whse/progrevw;*/
/*FILENAME REPORT2 "&RPTLIB/ULWM13.LWM13R2";*/
/*FILENAME REPORT3 "&RPTLIBX/rcvfile9.raw";*/
DATA _NULL_;
     CALL SYMPUT('RUNDT',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYYS10.));
RUN;
/*COMMENT FOR PRODUCTION*/
/*===================================================*/
/*FILENAME REPORT2 "C:\WINDOWS\Temp\ULWM13.LWM13R2";*/
FILENAME REPORT3 "C:\WINDOWS\Temp\rcvfile9.raw";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
/*===================================================*/
LIBNAME PROGRVW V8 '/sas/whse/progrevw';

%MACRO AVEDSET(DATA,SET);
DATA &DATA ;
SET &SET ;
RUN;
%MEND AVEDSET;
%AVEDSET(ULWS03_LWS03R3,PROGRVW.ULWS03_LWS03R3);
%AVEDSET(ULWS03_LWS03R3_HST,PROGRVW.ULWS03_LWS03R3_HST);
%AVEDSET(ULWS12_LWS12R3,PROGRVW.ULWS12_LWS12R3);
%AVEDSET(ULWS12_LWS12R3_HST,PROGRVW.ULWS12_LWS12R3_HST);
/*COMMENT FOR PRODUCTION*/
/*===================================================*/
ENDRSUBMIT;
%MACRO NVGT(DSET1,DSET2);
DATA &DSET1 ;
SET &DSET2 ;
RUN;
%MEND;
%NVGT(ULWS03_LWS03R3,WORKLOCL.ULWS03_LWS03R3);
%NVGT(ULWS12_LWS12R3,WORKLOCL.ULWS12_LWS12R3);

%MACRO NVGT2(DSETA,DSETB);
DATA &DSETA;
SET &DSETB;
FORMAT HST_RUNDT DATETIME.;
HST_RUNDT = RUNDT;
RUN;
%MEND NVGT2;
%NVGT2(ULWS03_LWS03R3_HST,WORKLOCL.ULWS03_LWS03R3_HST);
%NVGT2(ULWS12_LWS12R3_HST,WORKLOCL.ULWS12_LWS12R3_HST);
/*===================================================*/

/*UNCOMMENT FOR PRODUCTION*/
/*===================================================*/
/*%MACRO NVGT2(DSETA);*/
/*DATA &DSETA;*/
/*SET &DSETA;*/
/*FORMAT HST_RUNDT DATETIME.;*/
/*HST_RUNDT = RUNDT;*/
/*RUN;*/
/*%MEND NVGT2;*/
/*%NVGT2(ULWS03_LWS03R3_HST);*/
/*%NVGT2(ULWS12_LWS12R3_HST);*/
/*===================================================*/

%MACRO TCHK(NEWDS,MSET1,MSET2);
DATA &NEWDS (KEEP=QUEUE RUNDT HST_RUNDT);
MERGE &MSET1 &MSET2 ;
BY QUEUE;
RUN;
PROC SORT DATA=&NEWDS NODUPKEY;BY QUEUE;RUN;
%MEND TCHK;
%TCHK(X1,ULWS03_LWS03R3_HST,ULWS03_LWS03R3);
%TCHK(X2,ULWS12_LWS12R3_HST,ULWS12_LWS12R3);

DATA TMCHK;
SET X1 X2 ;
RUN;

DATA TMCHK;
SET TMCHK;
IF QUEUE = 'X1BS' AND RUNDT GT HST_RUNDT THEN ERR = 'N';
ELSE IF QUEUE = 'X2BS' AND RUNDT GT HST_RUNDT THEN ERR = 'N';
ELSE ERR = 'Y';
RUN;

DATA G2G;
SET TMCHK;
IF ERR = 'Y' THEN OUTPUT;
ELSE DELETE;
RUN;
	
DATA USE;
SET ULWS03_LWS03R3 ULWS12_LWS12R3 ;
RUN;

/*CREATE AUTODIALER MASTER FILE*/
DATA _NULL_;
SET USE;
FILE REPORT3 DROPOVER LRECL=32767;
FORMAT LD_ATY_RSP_ATT YYMMDDN8. 
	LD_ATY_RSP_CTC YYMMDDN8. 
	DD_BRT YYMMDDN8.
	MX_EFFDT YYMMDDN8. 
	LD_REQ_RSP_ATY_PRF YYMMDDN8.
	AMTDU 10.2
	LA_CUR_PRI 10.2;
DO;
PUT @1 SSN   			
	@10 REGION
	@11 QUEUE
	@15 NAME
	@59 DTC
	@62 LD_ATY_RSP_ATT
	@70 LN_DLQ_MAX
	@74 LD_ATY_RSP_CTC
	@82 REL
	@92 REL_CODE
	@94 SSN
	@103 REL
	@113 REL_CODE
	@115 NAME
	@159 GEO_PHN_IND
	@160 PHN_H
	@175 GEO_PHN_IND
	@176 PHN_A
	@191 GEO_PHN_IND
	@192 PHN_W
	@207 SSN
	@216 NAME
	@260 DX_STR_ADR_1
	@290 DX_STR_ADR_2
	@320 DX_STR_ADR_3
	@350 DM_CT
	@370 DC_DOM_ST
	@372 DF_ZIP_CDE
	@381 DM_FGN_CNY
	@396 DM_FGN_ST
	@411 DI_VLD_ADR
	@412 COUNT
	@414 LENDER_CODE 
	@422 AMTDU
	@429 LA_CUR_PRI 
	@439 MX_EFFDT
	@447 CUR_SCL_CODE
	@467 DD_BRT
	@475 BI_ATY_3_PTY
	@476 LD_REQ_RSP_ATY_PRF
	@484 NAME_SORT
	@486 FILLER
	@599 FILLER;	
END;
RUN;

%MACRO FUSE;
%LET DSID=%SYSFUNC(OPEN(WORK.G2G));
%LET HASOBS=%SYSFUNC(ATTRN(&DSID,ANY));
%LET RC=%SYSFUNC(CLOSE(&DSID));
%IF &HASOBS=1 %THEN
	%DO;
		PROC PRINTTO PRINT=REPORT2 NEW;
		RUN;
		OPTIONS ORIENTATION = LANDSCAPE;
		OPTIONS PS=39 LS=127 CENTER PAGENO=1 NODATE;
		TITLE 'COMPASS AUTODIALER SPECIAL CAMPAIGN LIST NOTIFICATION';
		TITLE2 'THE FOLLOWING SPECIAL CAMPAIGNS HAVE ENCOUNTERED A PROBLEM - PLEASE INVESTIGATE';
		TITLE3	"RUNDATE &RUNDT";
		FOOTNOTE 'JOB = UTLWM13  	 REPORT = ULWM13.LWM13R2';	
		PROC PRINT NOOBS SPLIT='/' DATA=G2G;
		VAR QUEUE ERR;
		LABEL QUEUE = 'SPECIAL CAMPAGIN' ERR = 'ERROR';
		RUN;
	%END;
	%ELSE %DO;
		PROC PRINTTO PRINT=REPORT2 NEW;
		RUN;
		OPTIONS ORIENTATION = LANDSCAPE;
		OPTIONS PS=39 LS=127 CENTER PAGENO=1 NODATE;
		TITLE 'COMPASS AUTODIALER SPECIAL CAMPAIGN LIST NOTIFICATION';
		TITLE2	"RUNDATE &RUNDT";
		DATA _NULL_;
		FILE PRINT;
			DO;
			PUT // 127*'-';
			PUT //////////////
				@30 '-- THE COMPASS SPECIAL CAMPAIGN UPLOAD WAS CREATED SUCCESSFULLY --';
			PUT ////////////// 
				@42 "JOB = UTLWM13  	 REPORT = ULWM13.LWM13R2";
			END;
		RETURN;
		RUN;	
    %END;
RUN;
%MEND FUSE;
%FUSE;

/*OVERWRITE THE HISTORY TABLE */
/*=====================================================*/
/*NOTE: ONLY USE THE RSUBMIT IF YOU WANT TO OVERWRITE THE DATA SETS WITH A LOCAL RUN!*/
/*RSUBMIT;*/
/*UNCOMMENT FOR PRODUCTION*/
/*=====================================================*/
/*%MACRO OVRWRT(HST_DS,CUR_DS);*/
/*DATA &HST_DS;*/
/*SET &CUR_DS;*/
/*RUN;*/
/*%MEND;*/
/*%OVRWRT(PROGRVW.ULWS03_LWS03R3_HST,ULWS03_LWS03R3);*/
/*%OVRWRT(PROGRVW.ULWS12_LWS12R3_HST,ULWS12_LWS12R3);*/
/*=====================================================*/
/*ENDRSUBMIT;*/