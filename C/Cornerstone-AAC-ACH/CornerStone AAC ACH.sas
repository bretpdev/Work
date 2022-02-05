/*INPUT*/
FILENAME IN_FIL "y:\batch\NFP_EA27_OUT_20120209210529.txt";
/*FILENAME IN_FIL "Q:\Support Services\CornerStone\NFP_EA27_OUT_*.txt";*/

/*OUTPUT*/
%LET REPORT2 = T:\SAS\CornerStone AAC ACH.r2;
FILENAME REPORT3 "T:\SAS\CornerStone AAC ACH.r3";

DATA EA27 ;
	INFILE IN_FIL DSD DLM = '10'x FIRSTOBS=1 MISSOVER END = EOF LRECL=32767;
	INPUT RECORD_TYP $ 1-2 @;
IF RECORD_TYP = '05' THEN INPUT Loan_typ $ 49 ;
else IF RECORD_TYP = '06' THEN INPUT street1 $ 3-27 street2 $ 28-52 City $ 53-68 State $ 69-70 
	zip $ 71-75 zip_4 $ 76-79 email $ 81-130;
IF RECORD_TYP = '07' THEN do;
	INPUT @11 a mmddyy6.;
	ld_lon_1_dsb = min(ld_lon_1_dsb,a);
end;
IF RECORD_TYP = '18' THEN do;
	INPUT awd_typ $ 12 ABA_ROUT $ 74-82 BANK_ACC $ 83-99 BANK_ACC_TYP $ 100
	INSTALL_AMT 116-122 ADD_AMT 123-129 @130 due_date mmddyy8. app_source $ 138 ;
	INSTALL_AMT = INSTALL_AMT / 100;
	ADD_AMT = ADD_AMT / 100;
	output;
end;
retain SSN FNAME LNAME Loan_typ street1 street2 City state zip zip_4 email ld_lon_1_dsb;
IF RECORD_TYP = '01' THEN do;
array cha{13} $ ABA_ROUT BANK_ACC BANK_ACC_TYP app_source awd_typ Loan_typ 
	street1 street2 City State zip zip_4 email;
array nume{4} INSTALL_AMT ADD_AMT due_date ld_lon_1_dsb;
	do i= 1 to 13;
		cha(i) = '';
	end;
	do i=1 to 4;
		nume(i) = .;
	end;
	INPUT SSN $ 3-11 FNAME $ 167-178 LNAME $ 128-162;
end;
format ld_lon_1_dsb due_date mmddyy10.;
format INSTALL_AMT ADD_AMT 7.2;
RUN;
proc sort data=ea27 noduprec; by ssn ld_lon_1_dsb; run;
/*identify borrowers with differing bank info*/
%let ssn_list = '';
proc sql noprint;
select "'" || ssn || "'" into: ssn_list separated by ','
from ea27
group by ssn
having count(distinct ABA_ROUT) > 1
	or count(distinct bank_acc) > 1
	or count(distinct BANK_ACC_TYP) > 1
;
quit;
%put &ssn_list;

data test(keep=ssn repnum );
set ea27 end=last;
where ssn not in (&ssn_list);
by ssn;
retain repnum d c 1;

if first.ssn then d = _n_;
if last.ssn then do;
	if _n_ - c >= 2499 then do;
		c = d;
		repnum + 1;
	end;
	output;
end;
if last then call symput('MAXR2',REPNUM);
run;
%LET MAXR2 = &MAXR2;

DATA EA27;
MERGE EA27 TEST;
BY SSN;
RUN;

%MACRO OUTREP2;
%DO I = 1 %TO &MAXR2;
DATA _NULL_;
SET EA27 ;
where repnum = &I;
FILE "&REPORT2..FILE&I" DELIMITER=',' DSD DROPOVER LRECL=32767;
IF _N_ = 1 THEN DO;
	PUT "SSN,ABA ROUTING NUMBER,BANK ACCOUNT NUMBER,BANK ACCOUNT TYPE,MONTHLY INSTALLMENT,ADDITIONAL AMOUNT,"
	"DUE DATE,SOURCE OF APPLICATION,AWARD TYPE,LOAN TYPE,1ST DISBURSEMENT DATE,BORROWER FIRST NAME,"
	"BORROWER LAST NAME,BORROWER STREET 1 ADDRESS,BORROWER STREET 2 ADDRESS,BORROWER CITY,BORROWER STATE,"
	"BORROWER ZIP,BORROWER ZIP +4,BORROWER E-MAIL";
END;
   PUT SSN ABA_ROUT BANK_ACC BANK_ACC_TYP INSTALL_AMT ADD_AMT due_date app_source awd_typ Loan_typ 
	ld_lon_1_dsb FNAME LNAME street1 street2 City State zip zip_4 email;
RUN;
%END;
%MEND;
%OUTREP2;

PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
/*FOR LANDSCAPE REPORTS:*/
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127;
TITLE 'EA27 BORROWERS WITH MULTIPLE BANK ACCOUNTS';

PROC PRINT NOOBS SPLIT='/' DATA=EA27 WIDTH=UNIFORM WIDTH=MIN LABEL;
where ssn in (&ssn_list);
FORMAT INSTALL_AMT ADD_AMT dollar10.2;
FORMAT due_date ld_lon_1_dsb mmddyy10.;
VAR SSN ABA_ROUT BANK_ACC BANK_ACC_TYP INSTALL_AMT ADD_AMT due_date app_source awd_typ Loan_typ 
	ld_lon_1_dsb FNAME LNAME street1 street2 City State zip zip_4 email;
LABEL SSN = 'Borrower SSN'
	ABA_ROUT = 'ABA Routing Number'
	BANK_ACC = 'Bank Account Number'
	BANK_ACC_TYP = 'Bank Account Type'
	INSTALL_AMT = 'Monthly Installment'
	ADD_AMT = 'Additional Amount'
	due_date = 'Due Date'
	app_source = 'Source of Application'
	awd_typ = 'Award Type'
	Loan_typ = 'Loan Type'
	ld_lon_1_dsb = '1st Disbursement Date'
	FNAME = 'Borrower First Name'
	LNAME = 'Borrower Last Name'
	street1 = 'Borrower Street 1 Address'
	street2 = 'Borrower Street 2 Address'
	City  = 'Borrower City'
	State = 'Borrower State'
	zip = 'Borrower Zip'
	zip_4 = 'Borrower Zip +4'
	email = 'Borrower E-mail'
;
RUN;

PROC PRINTTO;
RUN;
